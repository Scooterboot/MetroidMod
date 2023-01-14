using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using MetroidMod.Common.Systems;

namespace MetroidMod.Content.NPCs.Serris
{
	[AutoloadBossHead]
	public class Serris_Head : Serris
	{
		public override string BossHeadTexture => Texture + "_Head_Boss_1";
		public const string SerrisHead = "MetroidMod/Content/NPCs/Serris/Serris_Head_Head_Boss_";

		public override void Load()
		{
			base.Load();
			for (int k = 1; k <= 7; k++)
			{
				Mod.AddBossHeadTexture(SerrisHead + k);
			}
		}

		/* ai[3] and localAI[0] cannot be used. The rest is readily available. */


		internal enum SerrisState
		{
			JustSpawned = 0,
			NormalBehaviour = 1,
			Transforming = 2,
			CoreXState = 3
		}

		internal SerrisState ai_state
		{
			get { return (SerrisState)((int)NPC.ai[0]); }
			set { NPC.ai[0] = (int)value; }
		}
		internal float extra_state
		{
			get { return NPC.ai[1]; }
			set { NPC.ai[1] = value; }
		}

		int damage = 20;
		int speedDamage = 35;//60;
		int coreDamage = 30;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[NPC.type] = 15;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					39
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}
		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 60;
			NPC.damage = damage;
			NPC.defense = 28;
			NPC.lifeMax = 4000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = Sounds.NPCs.CoreXDeath;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 7, 0);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 5;
			NPC.boss = true;
			//bossBag = mod.ItemType("SerrisBag");
			if (Main.netMode != NetmodeID.Server) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Serris"); }
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("An invasive species brought by the Gizzard tribe and released into the seas after the tribe's collapse. The creature moves at extremely high speeds and is hard to keep an eye on. Attacking it will cause it to immediately retaliate and rush into you. Be aware of the creature's speed and strike with a charged attack at the head when it's not moving. Sometimes however... a creature isn't what it appears to be...")
			});
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * bossLifeScale) + 51;
			//damage = (int)(damage * 0.7f * Main.expertDamage);
			//speedDamage = (int)(speedDamage * 0.7f * Main.expertDamage);
			//coreDamage = (int)(coreDamage * 0.7f * Main.expertDamage);
			NPC.damage = damage;
		}
		
		public override int SpawnNPC(int tileX, int tileY)
		{
			int spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);
			int spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);
			int num11 = (int)(Main.player[NPC.target].position.X / 16f) - spawnRangeX;
			int num12 = (int)(Main.player[NPC.target].position.X / 16f) + spawnRangeX;
			int num13 = (int)(Main.player[NPC.target].position.Y / 16f) - spawnRangeY;
			int num14 = (int)(Main.player[NPC.target].position.Y / 16f) + spawnRangeY;

			return NPC.NewNPC(NPC.GetSource_FromAI(), (int)MathHelper.Clamp(tileX,num11,num12) * 16 + 8, (int)MathHelper.Clamp(tileY,num13,num14) * 16, Type);
		}
		
		bool initialBoost = false;
		//SoundEffectInstance soundInstance;
		int soundCounter = 0;
		int numUpdates = 0;
		int maxUpdates = 0;
		public int state = 1;
		public float mouthFrame = 0f;
		int mouthNum = 1;
		int glowFrame = 0;
		int glowNum = 1;
		int glowFrameCounter = 0;
		float oldRot = 0f;

		public override void AI()
		{
			if (ai_state <= SerrisState.NormalBehaviour)
			{
				this.Update_Worm(true);

				state = 1;
				if(NPC.life < (int)(NPC.lifeMax * 0.6f))
				{
					state = 3;
				}
				else if(NPC.life < (int)(NPC.lifeMax * 0.8f))
				{
					state = 2;
				}
				
				if(numUpdates == 0)
				{
					mouthFrame += 0.04f*mouthNum;
					if(mouthFrame <= 0f)
					{
						mouthFrame = 0f;
						mouthNum = 1;
					}
					if(mouthFrame >= 1f)
					{
						mouthFrame = 1f;
						mouthNum = -1;
					}
					
					glowFrameCounter++;
					if(glowFrameCounter > 8)
					{
						glowFrame += glowNum;
						glowFrameCounter = 0;
					}
					if(glowFrame <= 0)
					{
						glowFrame = 0;
						glowNum = 1;
					}
					if(glowFrame >= 2)
					{
						glowFrame = 2;
						glowNum = -1;
					}
				}
				
				if(NPC.life < (int)(NPC.lifeMax * 0.5f))
				{
					glowFrame = 1;
					mouthFrame = 0f;
					extra_state = 0;
					NPC.localAI[1] = 0;
					NPC.localAI[2] = 0;
					NPC.localAI[3] = 0;
					NPC.netUpdate = true;
					ai_state = SerrisState.Transforming;
					return;
				}
				
				// normal movement
				if(extra_state == 0)
				{
					NPC.damage = damage;
					NPC.chaseable = true;
					maxUpdates = 1;
					oldRot = NPC.rotation;
					
					//initial speed boost
					if(!initialBoost)
					{
						extra_state = 1;
						NPC.localAI[3] = 30;
						initialBoost = true;
						NPC.TargetClosest(true);
					}
					
					// activate speed boost on hit
					if(NPC.justHit)
					{
						extra_state = 1;
						NPC.TargetClosest(true);
					}
				}
				// stunned movement
				if(extra_state == 1)
				{
					mouthNum = 2;
					
					NPC.position -= NPC.velocity;
					NPC.rotation = oldRot;
					
					if(NPC.localAI[3] == 1)
					{
						SoundEngine.PlaySound(Sounds.NPCs.SerrisHurt, NPC.Center);
					}
					
					NPC.localAI[3]++;
					if(NPC.localAI[3] > 30)
					{
						extra_state = 2;
						NPC.localAI[3] = 0;
					}
				}
				// speedboost movement
				if(extra_state == 2)
				{
					state = 4;
					mouthNum = -3;
					maxUpdates = 1;
					NPC.chaseable = false;
					NPC.damage = speedDamage;
					NPC.position += NPC.velocity * 1.5f;
					
					Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 2.0f, 2.0f, 2.0f);

					if (Main.netMode != NetmodeID.Server)
					{
						/*if (soundInstance == null || soundInstance.State != SoundState.Playing)
						{
							if(soundInstance == null)
							{
								soundInstance = SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)NPC.Center.X, (int)NPC.Center.Y, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/SerrisAccel"));
							}
							else
							{
								soundInstance.Play();
							}
						}
						else
						{
							Vector2 screenPos = new Vector2(Main.screenPosition.X + Main.screenWidth * .5f, Main.screenPosition.Y + Main.screenHeight * .5f);

							float pan = (NPC.Center.X - screenPos.X) / (Main.screenWidth * .5f);
							float numX = Math.Abs(NPC.Center.X - screenPos.X);
							float numY = Math.Abs(NPC.Center.Y - screenPos.Y);
							float numL = (float)Math.Sqrt(numX * numX + numY * numY) + .5f;
							float volume = 1f - numL / (Main.screenWidth);

							pan = MathHelper.Clamp(pan, -1f, 1f);
							volume = MathHelper.Clamp(volume, 0f, 1f);

							if (volume == 0f)
								soundInstance.Stop(true);

							soundInstance.Pan = pan;
							soundInstance.Volume = volume * Main.soundVolume;
						}*/
						if(soundCounter <= 0)
						{
							SoundEngine.PlaySound(Sounds.NPCs.SerrisAccel, NPC.Center);
							soundCounter = 21;
						}
						else if(numUpdates == 0)
						{
							soundCounter--;
						}
					}
					
					if(numUpdates <= 0)
					{
						NPC.localAI[3]++;
					}
					if(NPC.localAI[3] > 480)
					{
						extra_state = 0;
						NPC.localAI[3] = 0;
						NPC.netUpdate = true;
						NPC.TargetClosest(true);
					}
				}
				//else if (soundInstance != null)
				//	soundInstance.Stop(true);
				else
				{
					soundCounter = 0;
				}
			}
			else
			{
				//if(soundInstance != null)
				//	soundInstance.Stop(true);
				NPC.damage = coreDamage;
				
				if(NPC.timeLeft < 300)
					NPC.timeLeft = 300;
			}
			
			if(ai_state == SerrisState.Transforming)
			{
				if(NPC.localAI[1] == 0)
					SoundEngine.PlaySound(SoundID.NPCDeath14,NPC.Center);
				
				extra_state += 0.01f;
				if (extra_state > 0.5f)
					extra_state = 0.5f;

				NPC.chaseable = false;
				NPC.rotation += extra_state;

				if(NPC.localAI[1]++ <= 1f)
					SoundEngine.PlaySound(Sounds.NPCs.SerrisDeath, NPC.Center);

				if(NPC.localAI[1] > 170f)
				{
					SoundEngine.PlaySound(SoundID.Item14, NPC.position);

					if (Main.netMode != NetmodeID.Server)
					{
						var entitySource = NPC.GetSource_FromAI();
						Gore newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-30, 31) * .2f, Main.rand.Next(-30, 31) * .2f), Mod.Find<ModGore>("SerrisXTransGore1").Type)];
						newGore.velocity *= .4f;
						newGore.timeLeft = 60;

						newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-30, 31) * .2f, Main.rand.Next(-30, 31) * .2f), Mod.Find<ModGore>("SerrisXTransGore2").Type)];
						newGore.velocity *= .4f;
						newGore.timeLeft = 60;

						for (int v = 0; v < 3; v++)
						{
							newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-30, 31) * .2f, Main.rand.Next(-30, 31) * .2f), Mod.Find<ModGore>("SerrisXTransGore3").Type)];
							newGore.velocity *= .4f;
							newGore.timeLeft = 60;
						}

						for (int num136 = 0; num136 < 20; num136++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
						}
					}

					NPC.position.X += (NPC.width / 2);
					NPC.position.Y += (NPC.height / 2);
					NPC.width = 70;
					NPC.height = 70;
					NPC.position.X -= (NPC.width / 2);
					NPC.position.Y -= (NPC.height / 2);

					extra_state = 0;
					NPC.localAI[1] = 0;
					NPC.localAI[3] = 100;
					NPC.netUpdate = true;
					ai_state = SerrisState.CoreXState;
				}
				
				NPC.velocity.X *= 0.98f;
				NPC.velocity.Y *= 0.98f;
				if (NPC.velocity.X > -0.1f && NPC.velocity.X < 0.1f)
					NPC.velocity.X = 0f;
				if (NPC.velocity.Y > -0.1f && NPC.velocity.Y < 0.1f)
					NPC.velocity.Y = 0f;
			}

			if (ai_state == SerrisState.CoreXState)
			{
				state = 5;
				if(NPC.life < (int)(NPC.lifeMax * 0.1f))
				{
					state = 7;
				}
				else if(NPC.life < (int)(NPC.lifeMax * 0.3f))
				{
					state = 6;
				}
				NPC.aiStyle = 5;
				NPC.width = 70;
				NPC.height = 70;
				NPC.damage = coreDamage;
				NPC.HitSound = SoundID.NPCHit1;
				NPC.knockBackResist = 0.5f;
				
				NPC.position += NPC.velocity * 1.5f;
				
				if(NPC.localAI[3] > 0)
				{
					NPC.chaseable = false;
					NPC.position -= NPC.velocity;
					NPC.velocity *= 0f;
					NPC.localAI[3]--;
				}
				else
				{
					if(NPC.localAI[1] == 0)
					{
						NPC.chaseable = true;
						if(NPC.justHit)
						{
							NPC.localAI[1] = 1;
							SoundEngine.PlaySound(Sounds.NPCs.CoreXHurt, NPC.Center);
							NPC.TargetClosest(true);
						}
					}
					if(NPC.localAI[1] == 1)
					{
						NPC.localAI[2]++;
						if(NPC.localAI[2] > 8)
						{
							NPC.localAI[1] = 2;
							NPC.localAI[2] = 0;
						}
					}
					if(NPC.localAI[1] == 2)
					{
						NPC.chaseable = false;
						NPC.localAI[2]++;
						if(NPC.localAI[2] > 150)
						{
							NPC.localAI[1] = 0;
							NPC.localAI[2] = 0;
							NPC.TargetClosest(true);
						}
					}
					
					if(Main.dayTime && (!Main.player[NPC.target].dead || Main.player[NPC.target].active))
						NPC.velocity.Y = NPC.velocity.Y + 0.1f;
				}
			}

			if (NPC.active && maxUpdates > 0)
			{
				numUpdates--;
				if (numUpdates >= 0)
				{
					NPC.UpdateNPC(NPC.whoAmI);
				}
				else
				{
					numUpdates = maxUpdates;
				}
			}
			else
			{
				numUpdates = 0;
			}
			maxUpdates = 0;
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return ((ai_state == SerrisState.NormalBehaviour && extra_state != 2) || (ai_state == SerrisState.CoreXState && NPC.localAI[1] != 2)) ? null : false;
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return ((ai_state == SerrisState.NormalBehaviour && extra_state != 2) || (ai_state == SerrisState.CoreXState && NPC.localAI[1] != 2)) ? null : false;
		}

		public override void PostAI()
		{
			if(ai_state == SerrisState.CoreXState)
				NPC.rotation = 0f;
			else
				NPC.spriteDirection = Math.Sign(NPC.velocity.X);
			
			//if(!NPC.active && soundInstance != null)
			//	soundInstance.Stop(true);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if(ai_state == SerrisState.CoreXState && Main.netMode != NetmodeID.Server)
			{
				for (int m = 0; m < (NPC.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
					if (NPC.life <= 0 && m % 2 == 0)
					{
						Main.dust[dustID].noGravity = true;
					}
				}
			}

			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					var entitySource = NPC.GetSource_Death();
					Gore newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, NPC.velocity *.4f, Mod.Find<ModGore>("SerrisXGore1").Type)];
					newGore.timeLeft = 60;
					newGore.velocity += Vector2.One;

					newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, NPC.velocity * .4f, Mod.Find<ModGore>("SerrisXGore2").Type)];
					newGore.timeLeft = 60;
					newGore.velocity += new Vector2(-1f, 1f);

					newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, NPC.velocity * .4f, Mod.Find<ModGore>("SerrisXGore3").Type)];
					newGore.timeLeft = 60;
					newGore.velocity += new Vector2(1f, -1f);

					newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, NPC.velocity * .4f, Mod.Find<ModGore>("SerrisXGore4").Type)];
					newGore.timeLeft = 60;
					newGore.velocity -= Vector2.One;
				}
					
				//if(soundInstance != null)
				//	soundInstance.Stop(true);
			}
		}
		
		public int sbFrame = 0;
		int sbFrameCounter = 0;
		int coreFrame = 0;
		int coreFrameCounter = 0;
		int flashFrame = 0;
		int flashFrameCounter = 0;
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			if(ai_state <= SerrisState.Transforming)
			{
				Texture2D texHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/Serris_Head").Value,
					texJaw = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/Serris_Jaw").Value;
				Vector2 headOrig = new Vector2(34, 31),
					jawOrig1 = new Vector2(30, 11),
					jawOrig2 = new Vector2(34, 1);
				int headHeight = texHead.Height / 15,
					jawHeight = texJaw.Height / 5;
				SpriteEffects effects = SpriteEffects.None;
				if (NPC.spriteDirection == -1)
				{
					effects = SpriteEffects.FlipVertically;
					headOrig.Y = headHeight - headOrig.Y;
					jawOrig1.Y = jawHeight - jawOrig1.Y;
					jawOrig2.Y = jawHeight - jawOrig2.Y;
				}
				int frame = state - 1;
				if(state == 4)
					frame = sbFrame + 3;
				
				sbFrameCounter++;
				if(sbFrameCounter > 5)
				{
					sbFrame++;
					sbFrameCounter = 0;
				}
				if(sbFrame > 1)
				{
					sbFrame = 0;
				}
				
				float headRot = NPC.rotation - 1.57f;
				
				Color headColor = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));
				Vector2 jawOrig = Vector2.Lerp(jawOrig1,jawOrig2,mouthFrame);
				int jawFrame = frame * jawHeight;
				sb.Draw(texJaw, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,jawFrame,texJaw.Width,jawHeight)), 
				headColor, headRot, jawOrig, 1f, effects, 0f);
				
				int headFrame = frame * (headHeight*3);
				headFrame += headHeight * glowFrame;
				sb.Draw(texHead, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,headFrame,texHead.Width,headHeight)), 
				headColor, headRot, headOrig, 1f, effects, 0f);
			}
			else
			{
				Texture2D texCore = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/SerrisCoreX").Value,
					texShell = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/SerrisCoreX_Shell").Value;
				int coreHeight = texCore.Height / 8;
				int shellHeight = texShell.Height / 4;
				
				coreFrameCounter++;
				if(coreFrameCounter > 5)
				{
					coreFrame++;
					coreFrameCounter = 0;
				}
				if(coreFrame >= 8)
				{
					coreFrame = 0;
				}
				
				if(NPC.localAI[1] == 2 || NPC.localAI[3] > 0)
				{
					flashFrameCounter++;
					if(flashFrameCounter > 4)
					{
						flashFrame++;
						flashFrameCounter = 0;
					}
					if(flashFrame > 1)
					{
						flashFrame = 0;
					}
				}
				else
				{
					flashFrame = 0;
					flashFrameCounter = 0;
				}
				
				int shellFrame = state-5;
				if(flashFrame > 0)
				{
					shellFrame = 3;
				}
				
				Color color = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));
				
				sb.Draw(texCore, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,coreHeight*coreFrame,texCore.Width,coreHeight)), 
				color, 0f, new Vector2(texCore.Width/2,coreHeight/2), 1f, SpriteEffects.None, 0f);
				
				sb.Draw(texShell, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,shellHeight*shellFrame,texShell.Width,shellHeight)), 
				color, 0f, new Vector2(texShell.Width/2,shellHeight/2), 1f, SpriteEffects.None, 0f);
			}
			
			return false;
		}


		public override void BossHeadSlot(ref int index)
		{
			index = ModContent.GetModBossHeadSlot(SerrisHead + state);
		}
		public override void BossHeadRotation(ref float rotation)
		{
			rotation = NPC.rotation;
		}
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.SerrisBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.SerrisCoreX>(), 1));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.SerrisMusicBox>(), 6));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.SerrisMask>(), 8));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.SerrisTrophy>(), 11));

			npcLoot.Add(notExpertRule);
		}
		public override void OnKill()
		{
			MSystem.bossesDown |= MetroidBossDown.downedSerris;
		}
	}
}
