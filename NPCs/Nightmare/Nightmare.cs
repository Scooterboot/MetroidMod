using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Nightmare
{
    public class Nightmare : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
		}

		int damage = 50;
		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 80;
			npc.damage = 0;//50;
			npc.defense = 40;
			npc.lifeMax = 30000;
			npc.dontTakeDamage = false;
			npc.alpha = 255;
			npc.scale = 1f;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath12;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 7, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = false;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[44] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;
			npc.aiStyle = -1;
			npc.npcSlots = 5;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Nightmare");
			//bossBag = mod.ItemType("NightmareBag");
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = 0;//(int)(npc.damage * 0.8f);
			damage = (int)(damage * 2 * 0.8f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void NPCLoot()
		{
			MWorld.downedNightmare = true;
			/*if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{*/
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NightmareCoreX"), 1);
				int num = Main.rand.Next(10)+15;
				for(int i = 0; i < num; i++)
				{
					float rot = (float)((Math.PI*2)/num)*i;
					Vector2 pos = npc.position + rot.ToRotationVector2() * 64f;
					Item.NewItem((int)pos.X, (int)pos.Y, npc.width, npc.height, mod.ItemType("NightmareCoreXFragment"), 1);
				}
				/*if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NightmareMusicBox"));
				}
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NightmareMask"));
				}
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NightmareTrophy"));
				}
			}*/
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if(currentState > 0)
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
					int num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisXGore1"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore85 = Main.gore[num373];
					gore85.velocity.X = gore85.velocity.X + 1f;
					gore85.velocity.Y = gore85.velocity.Y + 1f;
					num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisXGore2"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore87 = Main.gore[num373];
					gore87.velocity.X = gore87.velocity.X - 1f;
					gore87.velocity.Y = gore87.velocity.Y + 1f;
					num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisXGore3"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore89 = Main.gore[num373];
					gore89.velocity.X = gore89.velocity.X + 1f;
					gore89.velocity.Y = gore89.velocity.Y - 1f;
					num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisXGore4"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore91 = Main.gore[num373];
					gore91.velocity.X = gore91.velocity.X - 1f;
					gore91.velocity.Y = gore91.velocity.Y - 1f;
				}
			}
		}
		
		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.NightmareHead);
		}
		
		int direction = 1;
		
		NPC Body, Tail;
		NPC[] ArmFront = new NPC[5];
		NPC[] ArmBack = new NPC[3];
		
		Vector2[] armFrontPos1 = new Vector2[5];
		Vector2[] armFrontPos2 = new Vector2[5];
		Vector2[] armBackPos1 = new Vector2[3];
		Vector2[] armBackPos2 = new Vector2[3];
		
		float armAnim = 0f;
		int armNum = 1;
		
		NPC[] ArmLaser = new NPC[6];
		
		Vector2 faceFrame = Vector2.Zero;
		Vector2 faceFrameCounter = Vector2.Zero;
		int faceFrameIndex = 0;
		int[] faceMouthSequence = {2,1,0,1,2,1,0,1,0,1};
		
		Vector2 tailFrame = Vector2.Zero;
		
		int tailFrameNum = 1;
		int tailFrameCounter = 0;
		bool tailSpin = false;
		int tailSpinCounter = 0;
		float tailSpinCounterMax = 10f;
		
		bool tailSpinSoundPlayed = false;
		
		bool isX = false;
		int xCounter = 0;
		bool immuneFlash = false;
		bool TimeLock = false;
		int hitDelay = 0;
		
		Vector2 xFrame = Vector2.Zero;
		Vector2 xFrameCounter = Vector2.Zero;

		bool initialized = false;
		public override bool PreAI()
		{
			if(!initialized)
			{
				npc.TargetClosest(true);
				npc.netUpdate = true;
				int dir = 1;
				if(Main.rand.Next(2) == 1)
				{
					dir = -1;
				}
				npc.Center = new Vector2(Main.player[npc.target].Center.X+120*dir,Main.player[npc.target].Center.Y+500);
				direction = -dir;
				npc.direction = direction;
				
				int b = NPC.NewNPC((int)(npc.Center.X - 44*npc.direction),(int)(npc.Center.Y - 5),mod.NPCType("Nightmare_Body"),npc.whoAmI);
				Body = Main.npc[b];
				Body.position += new Vector2(0,(float)Body.height/2);
				Body.realLife = npc.whoAmI;
				Body.ai[0] = npc.whoAmI;
				
				int t = NPC.NewNPC((int)(npc.Center.X - 76*npc.direction),(int)(npc.Center.Y + 88),mod.NPCType("Nightmare_Tail"),npc.whoAmI);
				Tail = Main.npc[t];
				Tail.position += new Vector2(0,(float)Tail.height/2);
				Tail.ai[0] = npc.whoAmI;
				
				armFrontPos1[0] = new Vector2(84,12);
				armFrontPos1[1] = new Vector2(112,44);
				armFrontPos1[2] = new Vector2(122,78);
				armFrontPos1[3] = new Vector2(122,108);
				armFrontPos1[4] = new Vector2(108,152);
				
				armBackPos1[0] = new Vector2(-30,76);
				armBackPos1[1] = new Vector2(-24,105);
				armBackPos1[2] = new Vector2(-16,130);
				
				armFrontPos2[0] = new Vector2(84,12);
				armFrontPos2[1] = new Vector2(120,52);
				armFrontPos2[2] = new Vector2(136,90);
				armFrontPos2[3] = new Vector2(144,128);
				armFrontPos2[4] = new Vector2(136,182);
				
				armBackPos2[0] = new Vector2(-30,80);
				armBackPos2[1] = new Vector2(-24,117);
				armBackPos2[2] = new Vector2(-16,150);
				
				for(int i = 0; i < 5; i++)
				{
					int af = NPC.NewNPC((int)(npc.Center.X - armFrontPos1[i].X*npc.direction),(int)(npc.Center.Y + armFrontPos1[i].Y),mod.NPCType("Nightmare_ArmFront"),npc.whoAmI);
					ArmFront[i] = Main.npc[af];
					ArmFront[i].position += new Vector2(0,(float)ArmFront[i].height/2);
					ArmFront[i].realLife = npc.whoAmI;
					ArmFront[i].ai[0] = npc.whoAmI;
					ArmFront[i].ai[1] = i;
				}
				
				for(int i = 0; i < 3; i++)
				{
					int ab = NPC.NewNPC((int)(npc.Center.X - armBackPos1[i].X*npc.direction),(int)(npc.Center.Y + armBackPos1[i].Y),mod.NPCType("Nightmare_ArmBack"),npc.whoAmI);
					ArmBack[i] = Main.npc[ab];
					ArmBack[i].position += new Vector2(0,(float)ArmBack[i].height/2);
					ArmBack[i].realLife = npc.whoAmI;
					ArmBack[i].ai[0] = npc.whoAmI;
					ArmBack[i].ai[1] = i;
				}
				
				ArmLaser[0] = ArmFront[3];
				ArmLaser[1] = ArmBack[2];
				ArmLaser[2] = ArmFront[2];
				ArmLaser[3] = ArmBack[1];
				ArmLaser[4] = ArmFront[1];
				ArmLaser[5] = ArmBack[0];
				
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
			if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
			{
				npc.TargetClosest(true);
				if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
				{
					npc.dontTakeDamage = true;
					npc.damage = 0;
					npc.velocity.X *= 0.9f;
					if(npc.velocity.X > 0)
					{
						npc.velocity.X = Math.Max(npc.velocity.X - 0.1f,0f);
					}
					if(npc.velocity.X < 0)
					{
						npc.velocity.X = Math.Max(npc.velocity.X - 0.1f,0f);
					}
					if(npc.velocity.Y < 3f)
					{
						npc.velocity.Y += 0.2f;
					}
					npc.alpha++;
					if(npc.alpha >= 255)
					{
						npc.active = false;
					}
					despawn = true;
				}
				else
				{
					despawn = false;
				}
			}
			else
			{
				despawn = false;
			}
			
			Player player = Main.player[npc.target];
			
			//main states
			state = 0;
			if(npc.life <= (int)(npc.lifeMax*0.6f))
			{
				state = 1;
			}
			if(npc.life <= (int)(npc.lifeMax*0.4f))
			{
				state = 2;
			}
			if(npc.life <= (int)(npc.lifeMax*0.2f))
			{
				state = 3;
			}
			
			//core x states
			if(npc.life <= (int)(npc.lifeMax*0.1f))
			{
				state = 4;
			}
			if(npc.life <= (int)(npc.lifeMax*0.06f))
			{
				state = 5;
			}
			if(npc.life <= (int)(npc.lifeMax*0.03f))
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
							int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
							Main.dust[num71].velocity *= 1.4f;
							Main.dust[num71].noGravity = true;
							int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
							Main.dust[num72].velocity *= 1.4f;
							Main.dust[num72].noGravity = true;
						}
						Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,14);
					}
					currentState = state;
				}
				
				//spawn anim
				if(npc.ai[0] < 660)
				{
					int num = (int)npc.ai[0] % 60;
					if(num == 0)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/NightmareMove_1"));
					}
					
					float num2 = ((num+1f)/60f);
					float num3 = 0.275f;
					if(num2 < 0.5f)
					{
						num3 += 1.65f * 2 * num2;
					}
					else
					{
						num3 += 1.65f * 2 * (1f - num2);
					}
					if(npc.ai[0] > 600)
					{
						num3 = 0.275f + 1.65f * 2 * Math.Min(num2,0.5f);
					}
					npc.velocity.Y = -num3;
					
					npc.ai[0]++;
					
					npc.alpha = Math.Max(npc.alpha-5,127);
					npc.dontTakeDamage = true;
					npc.damage = 0;
				}
				else
				{
					if(npc.ai[1] == 0) // main phase
					{
						if(!despawn)
						{
							npc.alpha = Math.Max(npc.alpha-16,0);
							npc.damage = damage;
							npc.dontTakeDamage = false;
							
							if(randomize1)
							{
								laserCounterMax = Main.rand.Next(120)+(20*currentState);
							}
							if(randomize2)
							{
								orbCounterMax = Main.rand.Next(60)+(10*currentState);
							}
							for(int i = 0; i < 6; i++)
							{
								armLaserCounter[i]++;
								if(armLaserCounter[i] >= 200+(3*i))
								{
									// fire small lasers
									ArmLaser[i].ai[2] = 1;
									armLaserCounter[i] = 3*i + laserCounterMax;
									if(i == 0)
									{
										randomize1 = false;
									}
									if(i == 5)
									{
										randomize1 = true;
									}
								}
								
								armOrbCounter[i]++;
								if(armOrbCounter[i] >= 300+(20*i))
								{
									// fire gravity orbs
									ArmLaser[i].ai[3] = 1;
									armOrbCounter[i] = 100+(20*i) + orbCounterMax;
									if(i == 0)
									{
										randomize2 = false;
									}
									if(i == 5)
									{
										randomize2 = true;
									}
								}
							}
							
							if(Tail != null && Tail.active)
							{
								if(npc.ai[2] <= 500)
								{
									npc.ai[2]++;
								}
								if(npc.ai[2] == 500)
								{
									Tail.ai[1] = 1;
								}
								if(npc.ai[2] >= 440)
								{
									tailSpin = true;
								}
								
								if(npc.ai[2] > 500 && Tail.ai[2] == 0)
								{
									tailSpin = false;
									npc.ai[2]++;
									if(npc.ai[2] > 600)
									{
										for(int i = 0; i < 6; i++)
										{
											armLaserCounter[i] = 0;
											armOrbCounter[i] = 0;
										}
										randomize1 = true;
										randomize2 = true;
										npc.ai[1] = 1;
										if(currentState > 0)
										{
											npc.ai[1] = 2;
										}
										npc.ai[2] = 0;
										npc.ai[3] = direction;
									}
								}
							}
							else
							{
								tailSpin = false;
								npc.ai[2]++;
								if(npc.ai[2] > 600)
								{
									for(int i = 0; i < 6; i++)
									{
										armLaserCounter[i] = 0;
										armOrbCounter[i] = 0;
									}
									randomize1 = true;
									randomize2 = true;
									npc.ai[1] = 2;
									npc.ai[2] = 0;
									npc.ai[3] = direction;
								}
							}
						
							// movement
							if((npc.Center.X > player.Center.X - 150 && direction == 1) || (npc.Center.X > player.Center.X + 200 && direction == -1))
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X *= 0.98f;
								}
								npc.velocity.X -= 0.1f;
								if (npc.velocity.X > 8f)
								{
									npc.velocity.X = 8f;
								}
							}
							else if((npc.Center.X < player.Center.X - 200 && direction == 1) || (npc.Center.X < player.Center.X + 150 && direction == -1))
							{
								if (npc.velocity.X < 0f)
								{
									npc.velocity.X *= 0.98f;
								}
								npc.velocity.X += 0.1f;
								if (npc.velocity.X < -8f)
								{
									npc.velocity.X = -8f;
								}
							}
							if (npc.Center.Y > player.Center.Y - 100f)
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y *= 0.98f;
								}
								npc.velocity.Y -= 0.1f;
								if (npc.velocity.Y > 8f)
								{
									npc.velocity.Y = 8f;
								}
							}
							else if (npc.Center.Y < player.Center.Y - 300f)
							{
								if (npc.velocity.Y < 0f)
								{
									npc.velocity.Y *= 0.98f;
								}
								npc.velocity.Y += 0.1f;
								if (npc.velocity.Y < -8f)
								{
									npc.velocity.Y = -8f;
								}
							}
						}
					}
					if(npc.ai[1] == 1) // dash phase #1
					{
						npc.alpha = Math.Min(npc.alpha+16,127);
						npc.damage = 0;
						npc.dontTakeDamage = true;
						
						if(player.Center.X < Body.Center.X)
						{
							ChangeDir(-1);
						}
						else
						{
							ChangeDir(1);
						}
						
						Vector2 targetPos = player.Center + new Vector2(500*npc.ai[3],-200);
						float targetRot = (float)Math.Atan2(targetPos.Y-npc.Center.Y,targetPos.X-npc.Center.X);
						
						if(npc.ai[2] < 50)
						{
							npc.ai[2]++;
							if(npc.ai[2] == 1)
							{
								Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/NightmareMove_1"));
							}
							npc.velocity = targetRot.ToRotationVector2() * (Vector2.Distance(npc.Center,targetPos) / 24);
						}
						else
						{
							npc.ai[1] = 0;
							if(Tail != null && Tail.active)
							{
								npc.ai[2] = Main.rand.Next(150)+(50*currentState);//300;
							}
							else
							{
								npc.ai[2] = 0;
							}
						}
					}
					if(npc.ai[1] == 2) // dash phase #2
					{
						npc.alpha = Math.Min(npc.alpha+16,127);
						npc.damage = 0;
						npc.dontTakeDamage = true;
						
						if(player.Center.X < Body.Center.X)
						{
							ChangeDir(-1);
						}
						else
						{
							ChangeDir(1);
						}
						
						Vector2 targetPos = player.Center + new Vector2(500*npc.ai[3],-100);
						float targetRot = (float)Math.Atan2(targetPos.Y-npc.Center.Y,targetPos.X-npc.Center.X);
						
						if(npc.ai[2] < 70)
						{
							npc.ai[2]++;
							if(npc.ai[2] == 1)
							{
								Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/NightmareMove_2"));
							}
							npc.velocity = targetRot.ToRotationVector2() * (Vector2.Distance(npc.Center,targetPos) / 24);
						}
						else
						{
							npc.ai[1] = 3;
							npc.ai[2] = 0;
							npc.ai[3] = 0;
						}
					}
					if(npc.ai[1] == 3) // laser beam phase
					{
						if(!despawn)
						{
							npc.alpha = Math.Max(npc.alpha-16,0);
							npc.damage = damage;
							npc.dontTakeDamage = false;
							
							armNum = 1;
							
							if(npc.ai[2] == 0 || npc.ai[3] > 0)
							{
								if(Tail != null && Tail.active)
								{
									if(npc.ai[3] <= 60)
									{
										npc.ai[3]++;
									}
									if(npc.ai[3] == 60)
									{
										Tail.ai[1] = 1;
									}
									tailSpin = true;
									
									if(npc.ai[3] > 60 && Tail.ai[2] == 0)
									{
										tailSpin = false;
										npc.ai[3] = 0;
									}
								}
							}
							npc.ai[2]++;
							if(npc.ai[2] < 200)
							{
								// charge lasers
								for(int i = 0; i < 6; i++)
								{
									if(lasersFired[i] == 0)
									{
										ArmLaser[i].ai[2] = 2;
										lasersFired[i] = 1;
									}
								}
								if(npc.ai[2] == 10)//60)
								{
									Main.PlaySound(SoundLoader.customSoundType, (int)Body.Center.X, (int)Body.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_LaserBeam_Charge"));
								}
								
								// movement
								if((npc.Center.X > player.Center.X - 500 && direction == 1) || (npc.Center.X > player.Center.X + 500 && direction == -1))
								{
									if (npc.velocity.X > 0f)
									{
										npc.velocity.X *= 0.9f;
									}
									npc.velocity.X -= 0.3f;
									if (npc.velocity.X > 4f)
									{
										npc.velocity.X = 4f;
									}
								}
								else if((npc.Center.X < player.Center.X - 500 && direction == 1) || (npc.Center.X < player.Center.X + 500 && direction == -1))
								{
									if (npc.velocity.X < 0f)
									{
										npc.velocity.X *= 0.9f;
									}
									npc.velocity.X += 0.3f;
									if (npc.velocity.X < -4f)
									{
										npc.velocity.X = -4f;
									}
								}
								if (npc.Center.Y > Main.player[npc.target].Center.Y - 100f)
								{
									if (npc.velocity.Y > 0f)
									{
										npc.velocity.Y *= 0.95f;
									}
									npc.velocity.Y -= 0.2f;
									if (npc.velocity.Y > 6f)
									{
										npc.velocity.Y = 6f;
									}
								}
								else if (npc.Center.Y < Main.player[npc.target].Center.Y - 100f)
								{
									if (npc.velocity.Y < 0f)
									{
										npc.velocity.Y *= 0.95f;
									}
									npc.velocity.Y += 0.2f;
									if (npc.velocity.Y < -6f)
									{
										npc.velocity.Y = -6f;
									}
								}
							}
							else
							{
								// fire lasers
								for(int i = 0; i < 6; i++)
								{
									if(lasersFired[i] < 2)
									{
										ArmLaser[i].ai[2] = 3;
										lasersFired[i] = 2;
									}
								}
								if(npc.ai[2] == 200)
								{
									Main.PlaySound(SoundLoader.customSoundType, (int)Body.Center.X, (int)Body.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_LaserBeam_Fire"));
								}
								
								if(npc.ai[2] > 260)
								{
									npc.ai[1] = 0;
									if(Tail != null && Tail.active)
									{
										if(Tail.ai[2] == 1)
										{
											npc.ai[2] = 501;
										}
										else
										{
											npc.ai[2] = Main.rand.Next(150)+(50*currentState);//300;
										}
									}
									else
									{
										npc.ai[2] = Main.rand.Next(60*currentState);
									}
								}
								
								// movement
								npc.velocity.X = npc.velocity.X * 0.98f;
								if(npc.velocity.X > 0f)
								{
									npc.velocity.X -= 0.25f;
									if (npc.velocity.X > 6f)
									{
										npc.velocity.X = 6f;
									}
								}
								else if(npc.velocity.X < 0f)
								{
									npc.velocity.X += 0.25f;
									if (npc.velocity.X < -6f)
									{
										npc.velocity.X = -6f;
									}
								}
								npc.velocity.Y = npc.velocity.Y * 0.98f;
								if(npc.velocity.Y > 0f)
								{
									npc.velocity.Y -= 0.25f;
									if (npc.velocity.Y > 6f)
									{
										npc.velocity.Y = 6f;
									}
								}
								else if(npc.velocity.Y < 0f)
								{
									npc.velocity.Y += 0.25f;
									if (npc.velocity.Y < -6f)
									{
										npc.velocity.Y = -6f;
									}
								}
							}
						}
					}
					else
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
				npc.damage = damage;
				npc.alpha = Math.Max(npc.alpha-16,0);
				if(!isX)
				{
					npc.dontTakeDamage = true;
					npc.ai[0] = 0;
					npc.ai[1] = 0;
					npc.ai[2] = 0;
					npc.ai[3] = 0;
					xCounter++;
					if(Tail != null && Tail.active)
					{
						if(xCounter > 30)
						{
							Main.PlaySound(SoundID.NPCDeath14,Tail.Center);
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
							if(ArmFront[i] != null && ArmFront[i].active)
							{
								ArmFront[i].life = 0;
								ArmFront[i].HitEffect(0, 10.0);
								ArmFront[i].active = false;
							}
						}
						
						for(int i = 0; i < 3; i++)
						{
							if(ArmBack[i] != null && ArmBack[i].active)
							{
								ArmBack[i].life = 0;
								ArmBack[i].HitEffect(0, 10.0);
								ArmBack[i].active = false;
							}
						}
						if(xCounter == 61)
						{
							Main.PlaySound(SoundID.NPCDeath14,Body.Center);
						}
					}
					if(xCounter > 90)
					{
						if(Body != null && Body.active)
						{
							Main.PlaySound(SoundID.NPCDeath14,Body.Center);
							Body.life = 0;
							Body.HitEffect(0, 10.0);
							Body.active = false;
						}
					}
					if(xCounter > 150)
					{
						isX = true;
					}
					
					npc.velocity.X = npc.velocity.X * 0.9f;
					npc.velocity.Y = npc.velocity.Y * 0.9f;
					if (npc.velocity.X > -0.1f && npc.velocity.X < 0.1f)
					{
						npc.velocity.X = 0f;
					}
					if (npc.velocity.Y > -0.1f && npc.velocity.Y < 0.1f)
					{
						npc.velocity.Y = 0f;
					}
				}
				else if(!despawn)
				{
					npc.aiStyle = 5;
					npc.HitSound = SoundID.NPCHit8;
					npc.knockBackResist = 0.5f;
					npc.ai[3]++;
					npc.position += npc.velocity * 1.5f;
					if(npc.ai[3] <= 1 || npc.ai[3] >= 150)
					{
						immuneFlash = false;
						npc.dontTakeDamage = false;
						TimeLock = true;
					}
					else if(npc.ai[3] >= 2)
					{
						immuneFlash = true;
						npc.dontTakeDamage = true;
					}
					if(npc.justHit && hitDelay <= 0)
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
						npc.ai[3] = 2;
						hitDelay = 0;
					}
					if(TimeLock)
					{
						npc.ai[3] = 0;
					}
					if(Main.dayTime && (!player.dead || player.active))
					{
						npc.velocity.Y = npc.velocity.Y + 0.1f;
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
			
			npc.direction = direction;
			
			if(currentState > 0)
			{
				npc.HitSound = SoundID.NPCHit1;
				
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
					Main.PlaySound(SoundLoader.customSoundType, (int)Tail.Center.X, (int)Tail.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityMotor_Start"));
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
				if(ArmFront[i] != null && ArmFront[i].active)
				{
					Vector2 armFPos = Vector2.Lerp(armFrontPos1[i],armFrontPos2[i],armAnim);
					ArmFront[i].Center = new Vector2(npc.Center.X - armFPos.X*npc.direction, npc.Center.Y + armFPos.Y);
				}
			}
			
			for(int i = 0; i < 3; i++)
			{
				if(ArmBack[i] != null && ArmBack[i].active)
				{
					Vector2 armBPos = Vector2.Lerp(armBackPos1[i],armBackPos2[i],armAnim);
					ArmBack[i].Center = new Vector2(npc.Center.X - armBPos.X*npc.direction, npc.Center.Y + armBPos.Y);
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
				npc.position.X += 88*d;
				direction = d;
				npc.direction = direction;
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Player player = Main.player[npc.target];

			npc.spriteDirection = npc.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (npc.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			if(Body.active)
			{
				for(int i = 2; i >= 0; i--)
				{
					if(ArmBack[i].active)
					{
						Texture2D texArmBack = mod.GetTexture("NPCs/Nightmare/Nightmare_ArmBack");
						if(i > 0)
						{
							texArmBack = mod.GetTexture("NPCs/Nightmare/Nightmare_ArmBack"+i);
						}
						
						Vector2 armBackDrawPos = ArmBack[i].Center;
						Color armBackColor = npc.GetAlpha(Lighting.GetColor((int)armBackDrawPos.X / 16, (int)armBackDrawPos.Y / 16));
						sb.Draw(texArmBack, armBackDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,texArmBack.Width,texArmBack.Height)),armBackColor,0f,new Vector2(texArmBack.Width/2,texArmBack.Height/2),1f,effects,0f);
					}
				}
				
				Texture2D texBody = mod.GetTexture("NPCs/Nightmare/Nightmare_Body"),
						texTail = mod.GetTexture("NPCs/Nightmare/Nightmare_TailAnim"),
						texMask = mod.GetTexture("NPCs/Nightmare/Nightmare_MaskGel"),
						texFace = mod.GetTexture("NPCs/Nightmare/Nightmare_OpenFace");
				
				Vector2 bodyDrawPos = Body.Center + new Vector2(11*npc.direction,2);
				Color bodyColor = npc.GetAlpha(Lighting.GetColor((int)bodyDrawPos.X / 16, (int)bodyDrawPos.Y / 16));
				
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
					if(ArmFront[i].active)
					{
						Texture2D texArmFront = mod.GetTexture("NPCs/Nightmare/Nightmare_ArmFront");
						if(i > 0)
						{
							texArmFront = mod.GetTexture("NPCs/Nightmare/Nightmare_ArmFront"+i);
						}
						
						Vector2 armFrontOrigin = new Vector2(texArmFront.Width/2,texArmFront.Height/2);
						if(i == 4)
						{
							armFrontOrigin.X -= 7 * npc.direction;
						}
						
						Vector2 armFrontDrawPos = ArmFront[i].Center;
						Color armFrontColor = npc.GetAlpha(Lighting.GetColor((int)armFrontDrawPos.X / 16, (int)armFrontDrawPos.Y / 16));
						sb.Draw(texArmFront, armFrontDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,texArmFront.Width,texArmFront.Height)),armFrontColor,0f,armFrontOrigin,1f,effects,0f);
					}
				}
			}
			else
			{
				Color color = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));
				Texture2D texCore = mod.GetTexture("NPCs/Nightmare/NightmareX_Core"),
						texShell = mod.GetTexture("NPCs/Nightmare/NightmareX_Shell");
				int coreHeight = (int)(texCore.Height / 8);
				sb.Draw(texCore, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,(int)(coreHeight*xFrame.X),texCore.Width,coreHeight)),color,0f,new Vector2(texCore.Width/2,coreHeight/2),1f,effects,0f);
				int shellHeight = (int)(texShell.Height / 4);
				sb.Draw(texShell, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,(int)(shellHeight*xFrame.Y),texShell.Width,shellHeight)),color,0f,new Vector2(texShell.Width/2,shellHeight/2),1f,effects,0f);
			}
			
			for(int i = 0; i < Main.maxNPCs; i++)
			{
				if(Main.npc[i].active && Main.npc[i].type == mod.NPCType("GravityOrb") && Main.npc[i].ai[0] == npc.whoAmI)
				{
					NPC orb = Main.npc[i];
					Texture2D tex = Main.npcTexture[orb.type];
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
	}
}
