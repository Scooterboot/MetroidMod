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
using System.IO;

using MetroidModPorted.Common.Systems;

namespace MetroidModPorted.Content.NPCs.Nightmare
{
	[AutoloadBossHead]
	public class Nightmare : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					39,
					44,
					ModContent.BuffType<Buffs.PhazonDebuff>()
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}

		int damage = 50;
		public override void SetDefaults()
		{
			NPC.width = 80;
			NPC.height = 80;
			NPC.damage = 0;//50;
			NPC.defense = 40;
			NPC.lifeMax = 30000;
			NPC.dontTakeDamage = false;
			NPC.alpha = 255;
			NPC.scale = 1f;
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath12;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 7, 0);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = false;
			NPC.aiStyle = -1;
			NPC.npcSlots = 5;
			if (Main.netMode != NetmodeID.Server) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Nightmare"); }
			//bossBag = mod.ItemType("NightmareBag");
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A bio mechanical monstrosity created by a space faring species of humans. It can change it's positioning almost instantly and increase gravity to extreme conditions. If encountered it is best to destroy the gravity generator so avoiding its energy lasers is possible. While mechanical, its organic parts are not fully immune to damage. Blast the faceplate off to get to its true form! Sometimes however… a creature isn't what it appears to be…")
			});
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
			NPC.damage = 0;//(int)(NPC.damage * 0.8f);
			damage = (int)(damage * 2 * 0.8f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void OnKill()
		{
			MSystem.bossesDown |= MetroidBossDown.downedNightmare;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.NightmareBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.NightmareCoreX>(), 1));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.NightmareCoreXFragment>(), 1, 15, 25));
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.NightmareMusicBox>(), 6));
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.NightmareMask>(), 8));
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.NightmareTrophy>(), 11));

			npcLoot.Add(notExpertRule);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(currentState > 0)
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
				var entitySource = NPC.GetSource_Death();

				Gore newGore = Main.gore[Gore.NewGore(entitySource, NPC.position, NPC.velocity * .4f, Mod.Find<ModGore>("SerrisXGore1").Type)];
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
		}
		
		/*public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.NightmareHead);
		}*/
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
		}
		
		int direction = 1;

		int _body, _tail;
		NPC Body
		{
			get { return Main.npc[_body]; }
		}
		NPC Tail
		{
			get { return Main.npc[_tail]; }
		}

		int[] _armFront = new int[5];
		int[] _armBack = new int[3];
		
		Vector2[] armFrontPos1 = new Vector2[5];
		Vector2[] armFrontPos2 = new Vector2[5];
		Vector2[] armBackPos1 = new Vector2[3];
		Vector2[] armBackPos2 = new Vector2[3];
		
		float armAnim = 0f;
		int armNum = 1;
		
		NPC GetArmLaser(int i)
		{
			switch (i)
			{
				case 0:
					return Main.npc[_armFront[3]];
				case 1:
					return Main.npc[_armBack[2]];
				case 2:
					return Main.npc[_armFront[2]];
				case 3:
					return Main.npc[_armBack[1]];
				case 4:
					return Main.npc[_armFront[1]];
				default:
					return Main.npc[_armBack[0]];
			}
		}
		
		Vector2 faceFrame = Vector2.Zero;
		Vector2 faceFrameCounter = Vector2.Zero;
		int faceFrameIndex = 0;
		int[] faceMouthSequence = {2,1,0,1,2,1,0,1,0,1};
		
		Vector2 tailFrame = Vector2.Zero;
		
		int tailFrameNum = 1;
		bool tailSpin = false;
		int tailSpinCounter = 0;
		int tailFrameCounter = 0;
		float tailSpinCounterMax = 10f;
		bool tailSpinSoundPlayed = false;
		
		
		bool isX = false;
		int xCounter = 0;
		int hitDelay = 0;
		bool TimeLock = false;
		bool immuneFlash = false;
		
		Vector2 xFrame = Vector2.Zero;
		Vector2 xFrameCounter = Vector2.Zero;

		bool initialized = false;
		public override bool PreAI()
		{
			if(!initialized)
			{
				/* Initialize positions outside of network check so they're initialized on client machines as well. */
				armFrontPos1[0] = new Vector2(84, 12);
				armFrontPos1[1] = new Vector2(112, 44);
				armFrontPos1[2] = new Vector2(122, 78);
				armFrontPos1[3] = new Vector2(122, 108);
				armFrontPos1[4] = new Vector2(108, 152);

				armBackPos1[0] = new Vector2(-30, 76);
				armBackPos1[1] = new Vector2(-24, 105);
				armBackPos1[2] = new Vector2(-16, 130);

				armFrontPos2[0] = new Vector2(84, 12);
				armFrontPos2[1] = new Vector2(120, 52);
				armFrontPos2[2] = new Vector2(136, 90);
				armFrontPos2[3] = new Vector2(144, 128);
				armFrontPos2[4] = new Vector2(136, 182);

				armBackPos2[0] = new Vector2(-30, 80);
				armBackPos2[1] = new Vector2(-24, 117);
				armBackPos2[2] = new Vector2(-16, 150);

				NPC.TargetClosest(true);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					var entitySource = NPC.GetSource_FromAI();
					NPC.netUpdate = true;
					int dir = Main.rand.NextBool(2)? -1 : 1;

					direction = -dir;
					NPC.direction = direction;
					NPC.Center = new Vector2(Main.player[NPC.target].Center.X + 120 * dir, Main.player[NPC.target].Center.Y + 500);

					_body = NPC.NewNPC(entitySource, (int)(NPC.Center.X - 44 * NPC.direction), (int)(NPC.Center.Y - 5), ModContent.NPCType<Nightmare_Body>(), NPC.whoAmI, NPC.whoAmI);
					Body.position += new Vector2(0, (float)Body.height / 2);
					Body.realLife = NPC.whoAmI;
					Body.netUpdate = true;

					_tail = NPC.NewNPC(entitySource, (int)(NPC.Center.X - 76 * NPC.direction), (int)(NPC.Center.Y + 88), ModContent.NPCType<Nightmare_Tail>(), NPC.whoAmI, NPC.whoAmI);
					Tail.position += new Vector2(0, (float)Tail.height / 2);
					Tail.netUpdate = true;

					for (int i = 0; i < 5; i++)
					{
						_armFront[i] = NPC.NewNPC(entitySource, (int)(NPC.Center.X - armFrontPos1[i].X * NPC.direction), (int)(NPC.Center.Y + armFrontPos1[i].Y), ModContent.NPCType<Nightmare_ArmFront>(), NPC.whoAmI,
							NPC.whoAmI, i);
						Main.npc[_armFront[i]].position += new Vector2(0, (float)Main.npc[_armFront[i]].height / 2);
						Main.npc[_armFront[i]].realLife = NPC.whoAmI;
						Main.npc[_armFront[i]].netUpdate = true;
					}

					for (int i = 0; i < 3; i++)
					{
						_armBack[i] = NPC.NewNPC(entitySource, (int)(NPC.Center.X - armBackPos1[i].X * NPC.direction), (int)(NPC.Center.Y + armBackPos1[i].Y), ModContent.NPCType<Nightmare_ArmBack>(), NPC.whoAmI,
							NPC.whoAmI, i);
						Main.npc[_armBack[i]].position += new Vector2(0, (float)Main.npc[_armBack[i]].height / 2);
						Main.npc[_armBack[i]].realLife = NPC.whoAmI;
						Main.npc[_armBack[i]].netUpdate = true;
					}
				}

				initialized = true;
			}
			return true;
		}
		
		int[] armLaserCounter = new int[6];
		int[] armOrbCounter = new int[6];
		
		int laserCounterMax = 0;
		int orbCounterMax = 0;
		bool randomize1 = true;
		bool randomize2 = true;
		
		int[] lasersFired = new int[6];
		
		int state = 0;
		int currentState = 0;
		public override void AI()
		{
			bool despawn = false;
			if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 2000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
			{
				NPC.TargetClosest(true);
				if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 2000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
				{
					NPC.damage = 0;
					NPC.velocity.X *= 0.9f;
					NPC.dontTakeDamage = true;
					if(NPC.velocity.X > 0)
					{
						NPC.velocity.X = Math.Max(NPC.velocity.X - 0.1f,0f);
					}
					if(NPC.velocity.X < 0)
					{
						NPC.velocity.X = Math.Max(NPC.velocity.X - 0.1f,0f);
					}
					if(NPC.velocity.Y < 3f)
					{
						NPC.velocity.Y += 0.2f;
					}
					if(NPC.alpha++ >= 255)
					{
						NPC.active = false;
					}
					despawn = true;
				}
			}
			
			Player player = Main.player[NPC.target];
			
			//main states
			state = 0;
			if(NPC.life <= (int)(NPC.lifeMax*0.6f))
			{
				state = 1;
			}
			if(NPC.life <= (int)(NPC.lifeMax*0.4f))
			{
				state = 2;
			}
			if(NPC.life <= (int)(NPC.lifeMax*0.2f))
			{
				state = 3;
			}
			
			//core x states
			if(NPC.life <= (int)(NPC.lifeMax*0.1f))
			{
				state = 4;
			}
			if(NPC.life <= (int)(NPC.lifeMax*0.06f))
			{
				state = 5;
			}
			if(NPC.life <= (int)(NPC.lifeMax*0.03f))
			{
				state = 6;
			}
			
			if(state < 4)
			{
				if(state > 0)
				{
					if(currentState == 0)
					{
						for (int num70 = 0; num70 < 15; num70++)
						{
							int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
							Main.dust[num71].velocity *= 1.4f;
							Main.dust[num71].noGravity = true;
							int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
							Main.dust[num72].velocity *= 1.4f;
							Main.dust[num72].noGravity = true;
						}
						SoundEngine.PlaySound(SoundID.NPCDeath14,NPC.position);
					}
					currentState = state;
				}
				
				// Spawn anim
				if(NPC.ai[0] < 660)
				{
					int num = (int)NPC.ai[0] % 60;
					if(num == 0)
					{
						SoundEngine.PlaySound(Sounds.NPCs.NightmareMove_1, NPC.Center);
					}
					
					float num2 = ((num + 1f) / 60f);
					float num3 = 0.275f;
					if(num2 < 0.5f)
					{
						num3 += 1.65f * 2 * num2;
					}
					else
					{
						num3 += 1.65f * 2 * (1f - num2);
					}
					if(NPC.ai[0]++ > 600)
					{
						num3 = 0.275f + 1.65f * 2 * Math.Min(num2, 0.5f);
					}
					NPC.velocity.Y = -num3;
					
					NPC.damage = 0;
					NPC.dontTakeDamage = true;
					NPC.alpha = Math.Max(NPC.alpha - 5, 127);
				}
				else
				{
					// Main phase
					if (NPC.ai[1] == 0 && !despawn)
					{
						NPC.damage = damage;
						NPC.dontTakeDamage = false;
						NPC.alpha = Math.Max(NPC.alpha - 16, 0);

						if (randomize1)
							laserCounterMax = Main.rand.Next(120) + (20 * currentState);
						if (randomize2)
							orbCounterMax = Main.rand.Next(60) + (10 * currentState);

						for (int i = 0; i < 6; i++)
						{
							armLaserCounter[i]++;
							if (armLaserCounter[i] >= 200 + (3 * i))
							{
								// Fire small lasers
								GetArmLaser(i).ai[2] = 1;
								GetArmLaser(i).netUpdate = true;
								armLaserCounter[i] = 3 * i + laserCounterMax;
								if (i == 0)
								{
									randomize1 = false;
								}
								if (i == 5)
								{
									randomize1 = true;
								}
							}

							armOrbCounter[i]++;
							if (armOrbCounter[i] >= 300 + (20 * i))
							{
								// Fire gravity orbs
								GetArmLaser(i).ai[3] = 1;
								GetArmLaser(i).netUpdate = true;
								armOrbCounter[i] = 100 + (20 * i) + orbCounterMax;
								if (i == 0)
								{
									randomize2 = false;
								}
								if (i == 5)
								{
									randomize2 = true;
								}
							}
						}
							
						if(Tail != null && Tail.active)
						{
							if(NPC.ai[2] <= 500)
							{
								NPC.ai[2]++;
							}
							if(NPC.ai[2] == 500)
							{
								Tail.ai[1] = 1;
							}
							if(NPC.ai[2] >= 440)
							{
								tailSpin = true;
							}
								
							if(NPC.ai[2] > 500 && Tail.ai[2] == 0)
							{
								tailSpin = false;
								NPC.ai[2]++;
								if(NPC.ai[2] > 600)
								{
									for(int i = 0; i < 6; i++)
									{
										armLaserCounter[i] = 0;
										armOrbCounter[i] = 0;
									}
									randomize1 = true;
									randomize2 = true;
									NPC.ai[1] = 1;
									if(currentState > 0)
									{
										NPC.ai[1] = 2;
									}
									NPC.ai[2] = 0;
									NPC.ai[3] = direction;
								}
							}
						}
						else
						{
							tailSpin = false;
							NPC.ai[2]++;
							if(NPC.ai[2] > 600)
							{
								for(int i = 0; i < 6; i++)
								{
									armLaserCounter[i] = 0;
									armOrbCounter[i] = 0;
								}
								randomize1 = true;
								randomize2 = true;
								NPC.ai[1] = 2;
								NPC.ai[2] = 0;
								NPC.ai[3] = direction;
							}
						}
						
						// Movement
						if((NPC.Center.X > player.Center.X - 150 && direction == 1) || (NPC.Center.X > player.Center.X + 200 && direction == -1))
						{
							if (NPC.velocity.X > 0f)
							{
								NPC.velocity.X *= 0.98f;
							}
							NPC.velocity.X -= 0.1f;
							if (NPC.velocity.X > 8f)
							{
								NPC.velocity.X = 8f;
							}
						}
						else if((NPC.Center.X < player.Center.X - 200 && direction == 1) || (NPC.Center.X < player.Center.X + 150 && direction == -1))
						{
							if (NPC.velocity.X < 0f)
							{
								NPC.velocity.X *= 0.98f;
							}
							NPC.velocity.X += 0.1f;
							if (NPC.velocity.X < -8f)
							{
								NPC.velocity.X = -8f;
							}
						}
						if (NPC.Center.Y > player.Center.Y - 100f)
						{
							if (NPC.velocity.Y > 0f)
							{
								NPC.velocity.Y *= 0.98f;
							}
							NPC.velocity.Y -= 0.1f;
							if (NPC.velocity.Y > 8f)
							{
								NPC.velocity.Y = 8f;
							}
						}
						else if (NPC.Center.Y < player.Center.Y - 300f)
						{
							if (NPC.velocity.Y < 0f)
							{
								NPC.velocity.Y *= 0.98f;
							}
							NPC.velocity.Y += 0.1f;
							if (NPC.velocity.Y < -8f)
							{
								NPC.velocity.Y = -8f;
							}
						}
					}

					// Dash phase #1
					if (NPC.ai[1] == 1)
					{
						NPC.damage = 0;
						NPC.dontTakeDamage = true;
						NPC.alpha = Math.Min(NPC.alpha + 16,127);
						
						if(player.Center.X < Body.Center.X)
						{
							ChangeDir(-1);
						}
						else
						{
							ChangeDir(1);
						}
						
						Vector2 targetPos = player.Center + new Vector2(500*NPC.ai[3],-200);
						float targetRot = (float)Math.Atan2(targetPos.Y-NPC.Center.Y,targetPos.X-NPC.Center.X);
						
						if(NPC.ai[2] < 50)
						{
							NPC.ai[2]++;
							if(NPC.ai[2] == 1)
							{
								SoundEngine.PlaySound(Sounds.NPCs.NightmareMove_1, NPC.Center);
							}
							NPC.velocity = targetRot.ToRotationVector2() * (Vector2.Distance(NPC.Center,targetPos) / 24);
						}
						else
						{
							NPC.ai[1] = 0;
							if(Tail != null && Tail.active)
							{
								NPC.ai[2] = Main.rand.Next(150)+(50*currentState);//300;
							}
							else
							{
								NPC.ai[2] = 0;
							}
						}
					}
					
					// Dash phase #2
					if (NPC.ai[1] == 2)
					{
						NPC.alpha = Math.Min(NPC.alpha + 16, 127);
						NPC.damage = 0;
						NPC.dontTakeDamage = true;
						
						if(player.Center.X < Body.Center.X)
						{
							ChangeDir(-1);
						}
						else
						{
							ChangeDir(1);
						}
						
						Vector2 targetPos = player.Center + new Vector2(500 * NPC.ai[3], -100);
						float targetRot = (float)Math.Atan2(targetPos.Y - NPC.Center.Y, targetPos.X - NPC.Center.X);
						
						if(NPC.ai[2] < 70)
						{
							NPC.ai[2]++;
							if(NPC.ai[2] == 1)
							{
								SoundEngine.PlaySound(Sounds.NPCs.NightmareMove_2, NPC.Center);
							}
							NPC.velocity = targetRot.ToRotationVector2() * (Vector2.Distance(NPC.Center,targetPos) / 24);
						}
						else
						{
							NPC.ai[1] = 3;
							NPC.ai[2] = 0;
							NPC.ai[3] = 0;
						}
					}

					// Laser beam phase
					if (NPC.ai[1] == 3 && !despawn)
					{
						NPC.damage = damage;
						NPC.dontTakeDamage = false;
						NPC.alpha = Math.Max(NPC.alpha - 16,0);
							
						armNum = 1;
							
						if(NPC.ai[2] == 0 || NPC.ai[3] > 0)
						{
							if(Tail != null && Tail.active)
							{
								if(NPC.ai[3] <= 60)
								{
									NPC.ai[3]++;
								}
								if(NPC.ai[3] == 60)
								{
									Tail.ai[1] = 1;
								}
								tailSpin = true;
									
								if(NPC.ai[3] > 60 && Tail.ai[2] == 0)
								{
									tailSpin = false;
									NPC.ai[3] = 0;
								}
							}
						}

						NPC.ai[2]++;
						if(NPC.ai[2] < 200)
						{
							// Charge lasers
							for(int i = 0; i < 6; i++)
							{
								if(lasersFired[i] == 0)
								{
									GetArmLaser(i).ai[2] = 2;
									lasersFired[i] = 1;
								}
							}
							if(NPC.ai[2] == 10)
							{
								SoundEngine.PlaySound(Sounds.NPCs.Nightmare_LaserBeam_Charge, Body.Center);
							}
								
							// Movement
							if((NPC.Center.X > player.Center.X - 500 && direction == 1) || (NPC.Center.X > player.Center.X + 500 && direction == -1))
							{
								if (NPC.velocity.X > 0f)
								{
									NPC.velocity.X *= 0.9f;
								}
								NPC.velocity.X -= 0.3f;
								if (NPC.velocity.X > 4f)
								{
									NPC.velocity.X = 4f;
								}
							}
							else if((NPC.Center.X < player.Center.X - 500 && direction == 1) || (NPC.Center.X < player.Center.X + 500 && direction == -1))
							{
								if (NPC.velocity.X < 0f)
								{
									NPC.velocity.X *= 0.9f;
								}
								NPC.velocity.X += 0.3f;
								if (NPC.velocity.X < -4f)
								{
									NPC.velocity.X = -4f;
								}
							}
							if (NPC.Center.Y > Main.player[NPC.target].Center.Y - 100f)
							{
								if (NPC.velocity.Y > 0f)
								{
									NPC.velocity.Y *= 0.95f;
								}
								NPC.velocity.Y -= 0.2f;
								if (NPC.velocity.Y > 6f)
								{
									NPC.velocity.Y = 6f;
								}
							}
							else if (NPC.Center.Y < Main.player[NPC.target].Center.Y - 100f)
							{
								if (NPC.velocity.Y < 0f)
								{
									NPC.velocity.Y *= 0.95f;
								}
								NPC.velocity.Y += 0.2f;
								if (NPC.velocity.Y < -6f)
								{
									NPC.velocity.Y = -6f;
								}
							}
						}
						else
						{
							// Fire lasers
							for(int i = 0; i < 6; i++)
							{
								if(lasersFired[i] < 2)
								{
									GetArmLaser(i).ai[2] = 3;
									lasersFired[i] = 2;
								}
							}
							if(NPC.ai[2] == 200)
							{
								SoundEngine.PlaySound(Sounds.NPCs.Nightmare_LaserBeam_Fire, Body.Center);
							}
								
							if(NPC.ai[2] > 260)
							{
								NPC.ai[1] = 0;
								if(Tail != null && Tail.active)
								{
									if(Tail.ai[2] == 1)
									{
										NPC.ai[2] = 501;
									}
									else
									{
										NPC.ai[2] = Main.rand.Next(150)+(50*currentState);//300;
									}
								}
								else
								{
									NPC.ai[2] = Main.rand.Next(60*currentState);
								}
							}
								
							// movement
							NPC.velocity.X = NPC.velocity.X * 0.98f;
							if(NPC.velocity.X > 0f)
							{
								NPC.velocity.X -= 0.25f;
								if (NPC.velocity.X > 6f)
								{
									NPC.velocity.X = 6f;
								}
							}
							else if(NPC.velocity.X < 0f)
							{
								NPC.velocity.X += 0.25f;
								if (NPC.velocity.X < -6f)
								{
									NPC.velocity.X = -6f;
								}
							}
							NPC.velocity.Y = NPC.velocity.Y * 0.98f;
							if(NPC.velocity.Y > 0f)
							{
								NPC.velocity.Y -= 0.25f;
								if (NPC.velocity.Y > 6f)
								{
									NPC.velocity.Y = 6f;
								}
							}
							else if(NPC.velocity.Y < 0f)
							{
								NPC.velocity.Y += 0.25f;
								if (NPC.velocity.Y < -6f)
								{
									NPC.velocity.Y = -6f;
								}
							}
						}
					}
					else if (NPC.ai[1] != 3)
					{
						for(int i = 0; i < 6; i++)
						{
							lasersFired[i] = 0;
						}
					}
				}
			}
			else
			{
				NPC.damage = damage;
				NPC.alpha = Math.Max(NPC.alpha - 16,0);

				if(!isX)
				{
					xCounter++;
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
					NPC.ai[3] = 0;
					NPC.dontTakeDamage = true;

					if(Tail != null && Tail.active)
					{
						if(xCounter > 30)
						{
							SoundEngine.PlaySound(SoundID.NPCDeath14, Tail.Center);
							Tail.life = 0;
							Tail.HitEffect(0, 10.0);
							Tail.active = false;
						}
					}
					else if(xCounter < 30)
					{
						xCounter = 30;
					}
					if(xCounter > 60)
					{
						for(int i = 0; i < 5; i++)
						{
							if(Main.npc[_armFront[i]] != null && Main.npc[_armFront[i]].active)
							{
								Main.npc[_armFront[i]].life = 0;
								Main.npc[_armFront[i]].HitEffect(0, 10.0);
								Main.npc[_armFront[i]].active = false;
							}
						}
						
						for(int i = 0; i < 3; i++)
						{
							if(Main.npc[_armBack[i]] != null && Main.npc[_armBack[i]].active)
							{
								Main.npc[_armBack[i]].life = 0;
								Main.npc[_armBack[i]].HitEffect(0, 10.0);
								Main.npc[_armBack[i]].active = false;
							}
						}
						if(xCounter == 61)
						{
							SoundEngine.PlaySound(SoundID.NPCDeath14,Body.Center);
						}
					}
					if(xCounter > 90)
					{
						if(Body != null && Body.active)
						{
							SoundEngine.PlaySound(SoundID.NPCDeath14,Body.Center);
							Body.life = 0;
							Body.HitEffect(0, 10.0);
							Body.active = false;
						}
					}
					if(xCounter > 150)
					{
						isX = true;
					}
					
					NPC.velocity.X = NPC.velocity.X * 0.9f;
					NPC.velocity.Y = NPC.velocity.Y * 0.9f;
					if (NPC.velocity.X > -0.1f && NPC.velocity.X < 0.1f)
					{
						NPC.velocity.X = 0f;
					}
					if (NPC.velocity.Y > -0.1f && NPC.velocity.Y < 0.1f)
					{
						NPC.velocity.Y = 0f;
					}
				}
				else if(!despawn)
				{
					NPC.aiStyle = 5;
					NPC.knockBackResist = 0.5f;
					NPC.HitSound = SoundID.NPCHit8;
					NPC.position += NPC.velocity * 1.5f;

					NPC.ai[3]++;
					if(NPC.ai[3] <= 1 || NPC.ai[3] >= 150)
					{
						immuneFlash = false;
						NPC.dontTakeDamage = false;
						TimeLock = true;
					}
					else if(NPC.ai[3] >= 2)
					{
						immuneFlash = true;
						NPC.dontTakeDamage = true;
					}
					if(NPC.justHit && hitDelay <= 0)
					{
						hitDelay = 1;
					}
					if(hitDelay > 0)
					{
						hitDelay++;
					}
					if (hitDelay > 5)
					{
						TimeLock = false;
						NPC.ai[3] = 2;
						hitDelay = 0;
					}
					if(TimeLock)
					{
						NPC.ai[3] = 0;
					}
					if(Main.dayTime && (!player.dead || player.active))
					{
						NPC.velocity.Y = NPC.velocity.Y + 0.1f;
					}
				}
			}
			
			float armAnimSpeed = 0.035f;
			if(armAnim < 0.5f)
			{
				armAnimSpeed *= 3 * armAnim;
			}
			else
			{
				armAnimSpeed *= 3 * (1f - armAnim);
			}
			if(armAnimSpeed < 0.01f)
			{
				armAnimSpeed = 0.01f;
			}
			armAnim = MathHelper.Clamp(armAnim + armAnimSpeed * armNum, 0f, 1f);
			if(armNum == 1 && armAnim >= 1f)
			{
				armNum = -1;
			}
			if(armNum == -1 && armAnim <= 0f)
			{
				armNum = 1;
			}
			
			NPC.direction = direction;
			
			if(currentState > 0)
			{
				NPC.HitSound = SoundID.NPCHit1;
				
				faceFrameCounter.X++;
				if(faceFrameCounter.X > 10)
				{
					faceFrame.X++;
					faceFrameCounter.X = 0;
				}
				if(faceFrame.X > 3)
				{
					faceFrame.X = 0;
				}
				faceFrameCounter.Y++;
				if(faceFrameCounter.Y > 30)
				{
					faceFrameIndex++;
					if(faceFrameIndex > 9)
					{
						faceFrameCounter.Y = 0;
					}
					else
					{
						faceFrameCounter.Y = 20;
					}
				}
				if(faceFrameIndex > 9)
				{
					faceFrameIndex = 0;
				}
				faceFrame.Y = faceMouthSequence[faceFrameIndex] + (3 * (currentState-1));
			}
			
			tailFrameCounter++;
			if(tailFrameCounter > 8)
			{
				tailFrame.Y += tailFrameNum;
				tailFrameCounter = 0;
			}
			if(tailFrameNum == 1 && tailFrame.Y > 3)
			{
				tailFrameNum = -1;
			}
			if(tailFrameNum == -1 && tailFrame.Y < 1)
			{
				tailFrameNum = 1;
			}
			
			if(tailSpin)
			{
				tailSpinCounterMax = Math.Max(tailSpinCounterMax-0.25f,0f);
				if(Tail != null && Tail.active && !tailSpinSoundPlayed)
				{
					SoundEngine.PlaySound(Sounds.NPCs.Nightmare_GravityMotor_Start, Tail.Center);
					tailSpinSoundPlayed = true;
				}
			}
			else
			{
				tailSpinCounterMax = Math.Min(tailSpinCounterMax+0.25f,10f);
				tailSpinSoundPlayed = false;
			}
			if(tailSpinCounterMax < 10f || tailFrame.X > 0f)
			{
				tailSpinCounter++;
				if(tailSpinCounter > tailSpinCounterMax)
				{
					tailFrame.X += 1f;
					tailSpinCounter = 0;
				}
			}
			if(tailFrame.X > 2f)
			{
				tailFrame.X = 0f;
			}
			
			for(int i = 0; i < 5; i++)
			{
				if(Main.npc[_armFront[i]] != null && Main.npc[_armFront[i]].active)
				{
					Vector2 armFPos = Vector2.Lerp(armFrontPos1[i],armFrontPos2[i],armAnim);
					Main.npc[_armFront[i]].Center = new Vector2(NPC.Center.X - armFPos.X*NPC.direction, NPC.Center.Y + armFPos.Y);
				}
			}
			
			for(int i = 0; i < 3; i++)
			{
				if(Main.npc[_armBack[i]] != null && Main.npc[_armBack[i]].active)
				{
					Vector2 armBPos = Vector2.Lerp(armBackPos1[i],armBackPos2[i],armAnim);
					Main.npc[_armBack[i]].Center = new Vector2(NPC.Center.X - armBPos.X*NPC.direction, NPC.Center.Y + armBPos.Y);
				}
			}
			
			if(state >= 4)
			{
				xFrame.Y = state - 4;
				if(!isX || immuneFlash)
				{
					xFrameCounter.Y++;
					if(xFrameCounter.Y <= 5)
					{
						xFrame.Y = 3;
					}
					if(xFrameCounter.Y > 10)
					{
						xFrameCounter.Y = 0;
					}
				}
				else
				{
					xFrameCounter.Y = 0;
				}
				
				xFrameCounter.X++;
				if(xFrameCounter.X > 5)
				{
					xFrame.X++;
					xFrameCounter.X = 0;
				}
				if(xFrame.X > 7)
				{
					xFrame.X = 0;
				}
			}
		}
		
		void ChangeDir(int d)
		{
			if(direction == -d)
			{
				NPC.position.X += 88*d;
				direction = d;
				NPC.direction = direction;
			}
			NPC.netUpdate = true;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Player player = Main.player[NPC.target];

			NPC.spriteDirection = NPC.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			if(Body != null && Body.active)
			{
				for(int i = 2; i >= 0; i--)
				{
					if(Main.npc[_armBack[i]] != null && Main.npc[_armBack[i]].active)
					{
						Texture2D texArmBack = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_ArmBack").Value;
						if(i > 0)
							texArmBack = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_ArmBack" + i).Value;
						
						Vector2 armBackDrawPos = Main.npc[_armBack[i]].Center;
						Color armBackColor = NPC.GetAlpha(Lighting.GetColor((int)armBackDrawPos.X / 16, (int)armBackDrawPos.Y / 16));
						sb.Draw(texArmBack, armBackDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,texArmBack.Width,texArmBack.Height)),armBackColor,0f,new Vector2(texArmBack.Width/2,texArmBack.Height/2),1f,effects,0f);
					}
				}
				
				Texture2D texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_Body").Value,
						texTail = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_TailAnim").Value,
						texMask = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_MaskGel").Value,
						texFace = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_OpenFace").Value;
				
				Vector2 bodyDrawPos = Body.Center + new Vector2(11*NPC.direction,2);
				Color bodyColor = NPC.GetAlpha(Lighting.GetColor((int)bodyDrawPos.X / 16, (int)bodyDrawPos.Y / 16));
				
				if(currentState <= 0)
				{
					sb.Draw(texBody, bodyDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,texBody.Width,texBody.Height)),bodyColor,0f,new Vector2(texBody.Width/2,texBody.Height/2),1f,effects,0f);
				}
				else
				{
					int faceWidth = texFace.Width / 4;
					int faceHeight = texFace.Height / 9;
					
					sb.Draw(texFace, bodyDrawPos - Main.screenPosition, new Rectangle?(new Rectangle((int)faceFrame.X*faceWidth,(int)faceFrame.Y*faceHeight,faceWidth,faceHeight)),bodyColor,0f,new Vector2(faceWidth/2,faceHeight/2),1f,effects,0f);
				}
			
				if(Tail.active)
				{
					Vector2 tailDrawPos = Tail.Center;
					
					int tailWidth = texTail.Width / 3;
					int tailHeight = texTail.Height / 5;
					
					sb.Draw(texTail, tailDrawPos - Main.screenPosition, new Rectangle?(new Rectangle((int)tailFrame.X*tailWidth,(int)tailFrame.Y*tailHeight,tailWidth,tailHeight)),bodyColor,0f,new Vector2(tailWidth/2,tailHeight/2),1f,effects,0f);
				}
				
				for(int i = 4; i >= 0; i--)
				{
					if(Main.npc[_armFront[i]].active)
					{
						Texture2D texArmFront = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_ArmFront").Value;
						if(i > 0)
						{
							texArmFront = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/Nightmare_ArmFront" + i).Value;
						}
						
						Vector2 armFrontOrigin = new Vector2(texArmFront.Width/2,texArmFront.Height/2);
						if(i == 4)
						{
							armFrontOrigin.X -= 7 * NPC.direction;
						}
						
						Vector2 armFrontDrawPos = Main.npc[_armFront[i]].Center;
						Color armFrontColor = NPC.GetAlpha(Lighting.GetColor((int)armFrontDrawPos.X / 16, (int)armFrontDrawPos.Y / 16));
						sb.Draw(texArmFront, armFrontDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,texArmFront.Width,texArmFront.Height)),armFrontColor,0f,armFrontOrigin,1f,effects,0f);
					}
				}
			}
			else
			{
				Color color = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));
				Texture2D texCore = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/NightmareX_Core").Value,
						texShell = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Nightmare/NightmareX_Shell").Value;
				int coreHeight = (int)(texCore.Height / 8);
				sb.Draw(texCore, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,(int)(coreHeight*xFrame.X),texCore.Width,coreHeight)),color,0f,new Vector2(texCore.Width/2,coreHeight/2),1f,effects,0f);
				int shellHeight = (int)(texShell.Height / 4);
				sb.Draw(texShell, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,(int)(shellHeight*xFrame.Y),texShell.Width,shellHeight)),color,0f,new Vector2(texShell.Width/2,shellHeight/2),1f,effects,0f);
			}
			
			for(int i = 0; i < Main.maxNPCs; i++)
			{
				if(Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<GravityOrb>() && Main.npc[i].ai[0] == NPC.whoAmI)
				{
					NPC orb = Main.npc[i];
					Texture2D tex = Terraria.GameContent.TextureAssets.Npc[orb.type].Value;
					SpriteEffects effects2 = SpriteEffects.None;
					if (orb.direction == -1)
					{
						effects2 = SpriteEffects.FlipHorizontally;
					}
					int height = (int)(tex.Height / Main.npcFrameCount[orb.type]);
					sb.Draw(tex, orb.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, height*orb.frame.Y, tex.Width, height)), orb.GetAlpha(Color.White), orb.rotation, new Vector2((float)tex.Width/2f, (float)height/2f), orb.scale, effects2, 0f);
				}
			}
			
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(direction);

			writer.Write((byte)_body);
			writer.Write((byte)_tail);

			for (int i = 0; i < _armBack.Length; ++i)
				writer.Write((byte)_armBack[i]);
			for (int i = 0; i < _armFront.Length; ++i)
				writer.Write((byte)_armFront[i]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			direction = reader.ReadInt32();

			_body = reader.ReadByte();
			_tail = reader.ReadByte();

			for (int i = 0; i < _armBack.Length; ++i)
				_armBack[i] = reader.ReadByte();
			for (int i = 0; i < _armFront.Length; ++i)
				_armFront[i] = reader.ReadByte();
		}
	}
}
