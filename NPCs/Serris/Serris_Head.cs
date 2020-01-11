using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace MetroidMod.NPCs.Serris
{
    public class Serris_Head : Serris
	{
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
			get { return (SerrisState)((int)npc.ai[0]); }
			set { npc.ai[0] = (int)value; }
		}
		internal float extra_state
		{
			get { return npc.ai[1]; }
			set { npc.ai[1] = value; }
		}

		int damage = 20;
		int speedDamage = 35;//60;
		int coreDamage = 30;

		public override bool Autoload(ref string name)
		{
			return (true);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 15;
		}
		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 60;
			npc.damage = damage;
			npc.defense = 28;
			npc.lifeMax = 4000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/CoreXDeath");
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 7, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.aiStyle = -1;
			npc.npcSlots = 5;
			npc.boss = true;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			bossBag = mod.ItemType("SerrisBag");
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Serris");
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale) + 51;
			damage = (int)(damage * 0.7f);
			speedDamage = (int)(speedDamage * 0.7f);
			coreDamage = (int)(coreDamage * 0.7f);
			npc.damage = damage;
		}
		
		public override int SpawnNPC(int tileX, int tileY)
		{
			int spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);
			int spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);
			int num11 = (int)(Main.player[npc.target].position.X / 16f) - spawnRangeX;
			int num12 = (int)(Main.player[npc.target].position.X / 16f) + spawnRangeX;
			int num13 = (int)(Main.player[npc.target].position.Y / 16f) - spawnRangeY;
			int num14 = (int)(Main.player[npc.target].position.Y / 16f) + spawnRangeY;

			return NPC.NewNPC((int)MathHelper.Clamp(tileX,num11,num12) * 16 + 8, (int)MathHelper.Clamp(tileY,num13,num14) * 16, this.npc.type);
		}
		
		bool initialBoost = false;
		SoundEffectInstance soundInstance;
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
				if(npc.life < (int)(npc.lifeMax * 0.6f))
				{
					state = 3;
				}
				else if(npc.life < (int)(npc.lifeMax * 0.8f))
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
				
				if(npc.life < (int)(npc.lifeMax * 0.5f))
				{
					glowFrame = 1;
					mouthFrame = 0f;
					extra_state = 0;
					npc.localAI[1] = 0;
					npc.localAI[2] = 0;
					npc.localAI[3] = 0;
					npc.netUpdate = true;
					ai_state = SerrisState.Transforming;
					return;
				}
				
				// normal movement
				if(extra_state == 0)
				{
					npc.damage = damage;
					npc.chaseable = true;
					maxUpdates = 1;
					oldRot = npc.rotation;
					
					//initial speed boost
					if(!initialBoost)
					{
						extra_state = 1;
						npc.localAI[3] = 30;
						initialBoost = true;
						npc.TargetClosest(true);
					}
					
					// activate speed boost on hit
					if(npc.justHit)
					{
						extra_state = 1;
						npc.TargetClosest(true);
					}
				}
				// stunned movement
				if(extra_state == 1)
				{
					mouthNum = 2;
					
					npc.position -= npc.velocity;
					npc.rotation = oldRot;
					
					if(npc.localAI[3] == 1)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SerrisHurt"));
					}
					
					npc.localAI[3]++;
					if(npc.localAI[3] > 30)
					{
						extra_state = 2;
						npc.localAI[3] = 0;
					}
				}
				// speedboost movement
				if(extra_state == 2)
				{
					state = 4;
					mouthNum = -3;
					maxUpdates = 1;
					npc.chaseable = false;
					npc.damage = speedDamage;
					npc.position += npc.velocity * 1.5f;
					
					Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 2.0f, 2.0f, 2.0f);

					if (Main.netMode != NetmodeID.Server)
					{
						if (soundInstance == null || soundInstance.State != SoundState.Playing)
						{
							if (soundInstance == null)
								soundInstance = mod.GetSound("Sounds/SerrisAccel").CreateInstance();
							Main.PlaySoundInstance(soundInstance);
						}
						else
						{
							Vector2 screenPos = new Vector2(Main.screenPosition.X + Main.screenWidth * .5f, Main.screenPosition.Y + Main.screenHeight * .5f);

							float pan = (npc.Center.X - screenPos.X) / (Main.screenWidth * .5f);
							float numX = Math.Abs(npc.Center.X - screenPos.X);
							float numY = Math.Abs(npc.Center.Y - screenPos.Y);
							float numL = (float)Math.Sqrt(numX * numX + numY * numY) + .5f;
							float volume = 1f - numL / (Main.screenWidth);

							pan = MathHelper.Clamp(pan, -1f, 1f);
							volume = MathHelper.Clamp(volume, 0f, 1f);

							if (volume == 0f)
								soundInstance.Stop(true);

							soundInstance.Pan = pan;
							soundInstance.Volume = volume * Main.soundVolume;
						}
					}
					
					if(numUpdates <= 0)
					{
						npc.localAI[3]++;
					}
					if(npc.localAI[3] > 480)
					{
						extra_state = 0;
						npc.localAI[3] = 0;
						npc.netUpdate = true;
						npc.TargetClosest(true);
					}
				}
				else if (soundInstance != null)
					soundInstance.Stop(true);
			}
			else
			{
				if(soundInstance != null)
					soundInstance.Stop(true);
				npc.damage = coreDamage;
			}
			
			if(ai_state == SerrisState.Transforming)
			{
				if(npc.localAI[1] == 0)
					Main.PlaySound(SoundID.NPCDeath14,npc.Center);
				
				extra_state += 0.01f;
				if (extra_state > 0.5f)
					extra_state = 0.5f;

				npc.chaseable = false;
				npc.rotation += extra_state;

				if(npc.localAI[1]++ <= 1f)
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SerrisDeath"));

				if(npc.localAI[1] > 170f)
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);

					Gore newGore = Main.gore[Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * .2f, Main.rand.Next(-30, 31) * .2f), mod.GetGoreSlot("Gores/SerrisXTransGore1"))];
					newGore.velocity *= .4f;
					newGore.timeLeft = 60;

					newGore = Main.gore[Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * .2f, Main.rand.Next(-30, 31) * .2f), mod.GetGoreSlot("Gores/SerrisXTransGore2"))];
					newGore.velocity *= .4f;
					newGore.timeLeft = 60;

					for (int v = 0; v < 3; v++)
					{
						newGore = Main.gore[Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * .2f, Main.rand.Next(-30, 31) * .2f), mod.GetGoreSlot("Gores/SerrisXTransGore3"))];
						newGore.velocity *= .4f;
						newGore.timeLeft = 60;
					}

					for (int num136 = 0; num136 < 20; num136++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
					}

					npc.position.X += (npc.width / 2);
					npc.position.Y += (npc.height / 2);
					npc.width = 70;
					npc.height = 70;
					npc.position.X -= (npc.width / 2);
					npc.position.Y -= (npc.height / 2);

					extra_state = 0;
					npc.localAI[1] = 0;
					npc.localAI[3] = 100;
					npc.netUpdate = true;
					ai_state = SerrisState.CoreXState;
				}
				
				npc.velocity.X *= 0.98f;
				npc.velocity.Y *= 0.98f;
				if (npc.velocity.X > -0.1f && npc.velocity.X < 0.1f)
					npc.velocity.X = 0f;
				if (npc.velocity.Y > -0.1f && npc.velocity.Y < 0.1f)
					npc.velocity.Y = 0f;
			}

			if (ai_state == SerrisState.CoreXState)
			{
				state = 5;
				if(npc.life < (int)(npc.lifeMax * 0.1f))
				{
					state = 7;
				}
				else if(npc.life < (int)(npc.lifeMax * 0.3f))
				{
					state = 6;
				}
				npc.aiStyle = 5;
				npc.width = 70;
				npc.height = 70;
				npc.damage = 30;
				npc.HitSound = SoundID.NPCHit1;
				npc.knockBackResist = 0.5f;
				
				npc.position += npc.velocity * 1.5f;
				
				if(npc.localAI[3] > 0)
				{
					npc.chaseable = false;
					npc.position -= npc.velocity;
					npc.velocity *= 0f;
					npc.localAI[3]--;
				}
				else
				{
					if(npc.localAI[1] == 0)
					{
						npc.chaseable = true;
						if(npc.justHit)
						{
							npc.localAI[1] = 1;
							Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/CoreXHurt"));
							npc.TargetClosest(true);
						}
					}
					if(npc.localAI[1] == 1)
					{
						npc.localAI[2]++;
						if(npc.localAI[2] > 8)
						{
							npc.localAI[1] = 2;
							npc.localAI[2] = 0;
						}
					}
					if(npc.localAI[1] == 2)
					{
						npc.chaseable = false;
						npc.localAI[2]++;
						if(npc.localAI[2] > 150)
						{
							npc.localAI[1] = 0;
							npc.localAI[2] = 0;
							npc.TargetClosest(true);
						}
					}
					
					if(Main.dayTime && (!Main.player[npc.target].dead || Main.player[npc.target].active))
						npc.velocity.Y = npc.velocity.Y + 0.1f;
				}
			}

			if (npc.active && maxUpdates > 0)
			{
				numUpdates--;
				if (numUpdates >= 0)
				{
					npc.UpdateNPC(npc.whoAmI);
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
			return (ai_state == SerrisState.NormalBehaviour && extra_state != 2) || (ai_state == SerrisState.CoreXState && npc.localAI[1] != 2);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return (ai_state == SerrisState.NormalBehaviour && extra_state != 2) || (ai_state == SerrisState.CoreXState && npc.localAI[1] != 2);
		}

		public override void PostAI()
		{
			if(ai_state == SerrisState.CoreXState)
				npc.rotation = 0f;
			else
				npc.spriteDirection = Math.Sign(npc.velocity.X);
			
			if(!npc.active && soundInstance != null)
				soundInstance.Stop(true);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if(ai_state == SerrisState.CoreXState)
			{
				for (int m = 0; m < (npc.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, npc.life <= 0 && m % 2 == 0 ? 3f : 1f);
					if (npc.life <= 0 && m % 2 == 0)
					{
						Main.dust[dustID].noGravity = true;
					}
				}
			}

			if (npc.life <= 0)
			{
				Gore newGore = Main.gore[Gore.NewGore(npc.position, npc.velocity *.4f, mod.GetGoreSlot("Gores/SerrisXGore1"))];
				newGore.timeLeft = 60;
				newGore.velocity += Vector2.One;

				newGore = Main.gore[Gore.NewGore(npc.position, npc.velocity * .4f, mod.GetGoreSlot("Gores/SerrisXGore2"))];
				newGore.timeLeft = 60;
				newGore.velocity += new Vector2(-1f, 1f);

				newGore = Main.gore[Gore.NewGore(npc.position, npc.velocity * .4f, mod.GetGoreSlot("Gores/SerrisXGore3"))];
				newGore.timeLeft = 60;
				newGore.velocity += new Vector2(1f, -1f);

				newGore = Main.gore[Gore.NewGore(npc.position, npc.velocity * .4f, mod.GetGoreSlot("Gores/SerrisXGore4"))];
				newGore.timeLeft = 60;
				newGore.velocity -= Vector2.One;
					
				if(soundInstance != null)
					soundInstance.Stop(true);
			}
		}
		
		public int sbFrame = 0;
		int sbFrameCounter = 0;
		int coreFrame = 0;
		int coreFrameCounter = 0;
		int flashFrame = 0;
		int flashFrameCounter = 0;
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			if(ai_state <= SerrisState.Transforming)
			{
				Texture2D texHead = mod.GetTexture("NPCs/Serris/Serris_Head"),
					texJaw = mod.GetTexture("NPCs/Serris/Serris_Jaw");
				Vector2 headOrig = new Vector2(34, 31),
					jawOrig1 = new Vector2(30, 11),
					jawOrig2 = new Vector2(34, 1);
				int headHeight = texHead.Height / 15,
					jawHeight = texJaw.Height / 5;
				SpriteEffects effects = SpriteEffects.None;
				if (npc.spriteDirection == -1)
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
				
				float headRot = npc.rotation - 1.57f;
				
				Color headColor = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));
				Vector2 jawOrig = Vector2.Lerp(jawOrig1,jawOrig2,mouthFrame);
				int jawFrame = frame * jawHeight;
				sb.Draw(texJaw, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,jawFrame,texJaw.Width,jawHeight)), 
				headColor, headRot, jawOrig, 1f, effects, 0f);
				
				int headFrame = frame * (headHeight*3);
				headFrame += headHeight * glowFrame;
				sb.Draw(texHead, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,headFrame,texHead.Width,headHeight)), 
				headColor, headRot, headOrig, 1f, effects, 0f);
			}
			else
			{
				Texture2D texCore = mod.GetTexture("NPCs/Serris/SerrisCoreX"),
					texShell = mod.GetTexture("NPCs/Serris/SerrisCoreX_Shell");
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
				
				if(npc.localAI[1] == 2 || npc.localAI[3] > 0)
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
				
				Color color = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));
				
				sb.Draw(texCore, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,coreHeight*coreFrame,texCore.Width,coreHeight)), 
				color, 0f, new Vector2(texCore.Width/2,coreHeight/2), 1f, SpriteEffects.None, 0f);
				
				sb.Draw(texShell, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,shellHeight*shellFrame,texShell.Width,shellHeight)), 
				color, 0f, new Vector2(texShell.Width/2,shellHeight/2), 1f, SpriteEffects.None, 0f);
			}
			
			return false;
		}


		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.SerrisHead + state);
		}
		public override void BossHeadRotation(ref float rotation)
		{
			rotation = npc.rotation;
		}
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SerrisCoreX"));
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SerrisMusicBox"));
				}
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SerrisMask"));
				}
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SerrisTrophy"));
				}
			}
		}
	}
}
