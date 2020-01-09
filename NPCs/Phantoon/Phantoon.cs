using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Phantoon
{
    public class Phantoon : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantoon");
		}
		int damage = 65;
		public override void SetDefaults()
		{
			npc.width = 92;
			npc.height = 180;
			npc.damage = 0;
			npc.defense = 50;
			npc.lifeMax = 15000;
			npc.dontTakeDamage = true;
			npc.alpha = 255;
			npc.scale = 1f;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath10;
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
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Kraid");
			bossBag = mod.ItemType("PhantoonBag");
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale + 1);
			npc.damage = 0;//(int)(npc.damage * 0.7f);
			damage *= 2;
			damage = (int)(damage * 0.7f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GravityGel"), Main.rand.Next(20, 51));
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KraidPhantoonMusicBox"));
				}
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhantoonMask"));
				}
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhantoonTrophy"));
				}
			}
		}		
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0)
			{
				for (int num350 = 0; num350 < 20; num350++)
				{
					int num351 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 50, default(Color), 1.5f);
					Main.dust[num351].velocity *= 2f;
					Main.dust[num351].noGravity = true;
				}
				int num352 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), 61, npc.scale);
				Main.gore[num352].velocity *= 0.3f;
				num352 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), 62, npc.scale);
				Main.gore[num352].velocity *= 0.3f;
				num352 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), 63, npc.scale);
				Main.gore[num352].velocity *= 0.3f;
			}
		}
		
		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.PhantoonHead);
			if(npc.alpha > 192)
			{
				index = -1;
			}
		}

		bool initialized = false;
		public override bool PreAI()
		{
			if(!initialized)
			{
				npc.TargetClosest(true);
				npc.netUpdate = true;
				npc.Center = new Vector2(Main.player[npc.target].Center.X,Main.player[npc.target].Center.Y-200);
				initialized = true;
			}
			return true;
		}

		int[] spawnProj = new int[8];
		int spawnProjIndex = 0;
		
		int frameNum = 1;
		
		int eyeOpen = 0;
		int eyeFrame = 0;
		int eyeFrameCounter = 0;
		
		int state = 0;
		
		bool initialTeleport = false;

		public override void AI()
		{
			if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
			{
				npc.TargetClosest(true);
				if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
				{
					npc.ai[2] = 3f;
					eyeOpen = 0;
					npc.alpha += 3;
					if(npc.alpha > 255)
					{
						npc.active = false;
					}
				}
			}
			
			state = 0;
			if(npc.life <= (int)(npc.lifeMax*0.8f))
			{
				state = 1;
			}
			if(npc.life <= (int)(npc.lifeMax*0.55f))
			{
				state = 2;
			}
			if(npc.life <= (int)(npc.lifeMax*0.3f))
			{
				state = 3;
			}
			
			int[,] fireBallRand = new int[4,2];
			fireBallRand[0,0] = 15;
			fireBallRand[1,0] = 30;
			fireBallRand[2,0] = 40;
			fireBallRand[3,0] = 50;
			
			fireBallRand[0,1] = 25;
			fireBallRand[1,1] = 50;
			
			fireBallRand[2,1] = 60;
			
			if(state == 1)
			{
				fireBallRand[2,0] = 45;
				fireBallRand[3,0] = 60;
				
				fireBallRand[0,1] = 30;
				fireBallRand[1,1] = 60;
			}
			
			if(state == 2)
			{
				fireBallRand[2,0] = 50;
				fireBallRand[3,0] = 70;
				
				fireBallRand[0,1] = 40;
				fireBallRand[1,1] = 70;
			
				fireBallRand[2,1] = 75;
			}
			
			if(state == 2)
			{
				fireBallRand[2,0] = 55;
				fireBallRand[3,0] = 80;
				
				fireBallRand[0,1] = 50;
				fireBallRand[1,1] = 80;
			
				fireBallRand[2,1] = 75;
			}

			if(npc.ai[0] < 660) // spawn animation
			{
				npc.ai[0]++;

				if(npc.ai[0] > 30 && spawnProjIndex < 8) // summon fire balls
				{
					npc.ai[1]++;
					if(npc.ai[1] > 30)
					{
						float dist = 120;
						float rot = -((float)Math.PI/2) + (((float)Math.PI/4)*spawnProjIndex);
						Vector2 pos = npc.Center + new Vector2((float)Math.Cos(rot)*dist,(float)Math.Sin(rot)*dist);

						spawnProj[spawnProjIndex] = spawnFireBall(pos.X,pos.Y);
						NPC fb = Main.npc[spawnProj[spawnProjIndex]];
						fb.ai[1] = -1;
						fb.ai[2] = rot;

						spawnProjIndex++;
						npc.ai[1] = 0;
					}
				}
				else
				{
					npc.ai[1] = 0;
				}
				if(npc.ai[0] > 300) // rotate fire balls
				{
					for(int i = 0; i < spawnProj.Length; i++)
					{
						Main.npc[spawnProj[i]].ai[3] = 1;
						if(npc.ai[0] > 460) // pull fire balls in
						{
							Main.npc[spawnProj[i]].ai[3] = 2;
						}
					}
				}
				if(npc.ai[0] > 550) // fade into existance
				{
					npc.alpha = Math.Max(npc.alpha-3,127);
				}
				npc.ai[2] = 0f;
			}
			else // AI
			{
				if(npc.ai[2] == 0f) // basic movement and fireball spawning
				{
					npc.ai[1]++;
					
					if(npc.ai[1] % 20 == 0f) // spawn fireball every 20 frames
					{
						int rand = Main.rand.Next(100);
						if(Vector2.Distance(npc.Center,Main.player[npc.target].Center) >= (600 - 100*state)) // spawn only super and targeting fireballs when too far from the player
						{
							if(rand < fireBallRand[0,1])
							{
								//spawn super fireball
								int fire = spawnFireBall(npc.Center.X,npc.Center.Y);
								Main.npc[fire].ai[1] = 2;
							}
							else if(rand < fireBallRand[1,1])
							{
								//spawn targeting fireballs
								for(int i = 0; i < 2 + 2*state; i++)
								{
									int fire = spawnFireBall(npc.Center.X,npc.Center.Y);
									NPC fb = Main.npc[fire];
									fb.ai[1] = 1;
									fb.ai[2] = -((float)Math.PI/2) + (((float)Math.PI/4)*Main.rand.Next(8));
								}
							}
						}
						else // spawn bounce or targeting fireballs when close to the player
						{
							if(rand < fireBallRand[0,0])
							{
								//spawn basic bounce fireball
								int fb = spawnFireBall(npc.Center.X,npc.Center.Y+46);
							}
							else if(rand < fireBallRand[1,0])
							{
								//spawn bundle of bounce fireballs
								for(int i = 0; i < 5; i++)
								{
									int fb = spawnFireBall(npc.Center.X,npc.Center.Y+46);
								}
							}
							else if(rand < fireBallRand[2,0])
							{
								//spawn targeting fireballs
								for(int i = 0; i < 2+state; i++)
								{
									int fire = spawnFireBall(npc.Center.X,npc.Center.Y);
									NPC fb = Main.npc[fire];
									fb.ai[1] = 1;
									fb.ai[2] = -((float)Math.PI/2) + (((float)Math.PI/4)*Main.rand.Next(8));
								}
							}
							else if(rand < fireBallRand[3,0])
							{
								//spawn super fireball
								int fire = spawnFireBall(npc.Center.X,npc.Center.Y);
								Main.npc[fire].ai[1] = 2;
							}
						}
					}
					
					eyeOpen = 0;
					npc.alpha = Math.Max(npc.alpha-3,127);
					
					if(npc.ai[1] >= 360f) // open eye
					{
						npc.TargetClosest(true);
						npc.netUpdate = true;
						npc.ai[2] = 1f;
						npc.ai[1] = Main.rand.Next(241) + 20*state;
						npc.ai[3] = 2f;
						
						for(int i = 0; i < 8; i++)
						{
							int fire = spawnFireBall(npc.Center.X,npc.Center.Y);
							NPC fb = Main.npc[fire];
							fb.ai[1] = 4;
							fb.ai[2] = -((float)Math.PI/2) + (((float)Math.PI/4)*i);
						}
					}

					// movement
					if (npc.Center.X > Main.player[npc.target].Center.X + 150f)
					{
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X = npc.velocity.X * 0.98f;
						}
						npc.velocity.X -= 0.1f;
						if (npc.velocity.X > 8f)
						{
							npc.velocity.X = 8f;
						}
					}
					else if (npc.Center.X < Main.player[npc.target].Center.X - 150f)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X = npc.velocity.X * 0.98f;
						}
						npc.velocity.X += 0.1f;
						if (npc.velocity.X < -8f)
						{
							npc.velocity.X = -8f;
						}
					}
					if (npc.Center.Y > Main.player[npc.target].Center.Y - 150f)
					{
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y = npc.velocity.Y * 0.98f;
						}
						npc.velocity.Y -= 0.1f;
						if (npc.velocity.Y > 5f)
						{
							npc.velocity.Y = 5f;
						}
					}
					else if (npc.Center.Y < Main.player[npc.target].Center.Y - 250f)
					{
						if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y = npc.velocity.Y * 0.98f;
						}
						npc.velocity.Y += 0.1f;
						if (npc.velocity.Y < -5f)
						{
							npc.velocity.Y = -5f;
						}
					}
				}
				if(npc.ai[2] == 1f) // eye open - teleporting phase
				{
					npc.ai[3]++;
					if(npc.ai[3] < 60f)
					{
						if(npc.ai[3] == 1f)
						{
							npc.Center = new Vector2(Main.player[npc.target].Center.X + (100 + Main.rand.Next(101)) * ((Main.rand.Next(2) == 0) ? 1 : -1),Main.player[npc.target].Center.Y - 100 - Main.rand.Next(201));
							
							if(Main.rand.Next(100) < fireBallRand[2,1])
							{
								for(int i = 0; i < 8; i++)
								{
									int fire = spawnFireBall(npc.Center.X,npc.Center.Y);
									NPC fb = Main.npc[fire];
									fb.ai[1] = 1;
									fb.ai[2] = -((float)Math.PI/2) + (((float)Math.PI/4)*i);
								}
							}
							else
							{
								int num = 1;
								if(npc.Center.X > Main.player[npc.target].Center.X)
								{
									num = -1;
								}
								
								for(int i = 0; i < 8; i++)
								{
									float xpos = 64 + 36*i;
									int fire = spawnFireBall(npc.Center.X + xpos*num,npc.Center.Y - 40f);
									Main.npc[fire].ai[2] = 1 + 5*i;
								}
							}
							initialTeleport = true;
						}
						npc.alpha = Math.Max(npc.alpha-25,0);
						eyeOpen = 1;
					}
					else
					{
						npc.alpha = Math.Min(npc.alpha+8,255);
						eyeOpen = 0;
						if(npc.ai[3] > 100f)
						{
							npc.ai[3] = 0f;
						}
					}
					
					if(npc.justHit && initialTeleport) // change phase after being hit
					{
						npc.ai[2] = 2f;
						npc.ai[3] = 0f;
						eyeOpen = 2;
						initialTeleport = false;
					}
					
					// movement
					npc.velocity.X = npc.velocity.X * 0.9f;
					if(npc.velocity.X > 0f)
					{
						npc.velocity.X -= 0.25f;
						if (npc.velocity.X > 4f)
						{
							npc.velocity.X = 4f;
						}
					}
					else if(npc.velocity.X < 0f)
					{
						npc.velocity.X += 0.25f;
						if (npc.velocity.X < -4f)
						{
							npc.velocity.X = -4f;
						}
					}
					npc.velocity.Y = npc.velocity.Y * 0.9f;
					if(npc.velocity.Y > 0f)
					{
						npc.velocity.Y -= 0.25f;
						if (npc.velocity.Y > 4f)
						{
							npc.velocity.Y = 4f;
						}
					}
					else if(npc.velocity.Y < 0f)
					{
						npc.velocity.Y += 0.25f;
						if (npc.velocity.Y < -4f)
						{
							npc.velocity.Y = -4f;
						}
					}
				}
				if(npc.ai[2] == 2f) // eye open - chase player phase
				{
					npc.ai[3]++;
					if(npc.ai[3] >= (1000 - 75*state)) // change back to main phase after enough damage is recieved
					{
						eyeOpen = 0;
						npc.alpha = Math.Min(npc.alpha+10,255);
						if(npc.alpha >= 255)
						{
							npc.Center = new Vector2(Main.player[npc.target].Center.X - 200 + Main.rand.Next(401),Main.player[npc.target].Center.Y - 100 - Main.rand.Next(201));
							npc.velocity = new Vector2(5f-Main.rand.Next(11),5f-Main.rand.Next(11));
							npc.ai[2] = 0f;
							npc.ai[3] = 0f;
						}
					}
					else
					{
						npc.alpha = Math.Max(npc.alpha-25,0);
						
						// movement
						float speed = 0.2f + 0.05f*state;
						if (npc.Center.X > Main.player[npc.target].Center.X)
						{
							npc.velocity.X -= speed;
						}
						else if (npc.Center.X < Main.player[npc.target].Center.X)
						{
							npc.velocity.X += speed;
						}
						if (npc.Center.Y > Main.player[npc.target].Center.Y)
						{
							npc.velocity.Y -= speed;
						}
						else if (npc.Center.Y < Main.player[npc.target].Center.Y)
						{
							npc.velocity.Y += speed;
						}
						float speedmax = 8f + 2f*state;
						if(npc.velocity.X < -speedmax)
						{
							npc.velocity.X = -speedmax;
						}
						if(npc.velocity.X > speedmax)
						{
							npc.velocity.X = speedmax;
						}
						if(npc.velocity.Y < -speedmax)
						{
							npc.velocity.Y = -speedmax;
						}
						if(npc.velocity.Y > speedmax)
						{
							npc.velocity.Y = speedmax;
						}
					}
				}
			}
			
			npc.rotation = MathHelper.Clamp(npc.velocity.X,-4f,4f) / 30f;
			
			if(eyeOpen > 0)
			{
				npc.dontTakeDamage = false;
				npc.damage = damage;
				
				if((eyeOpen == 1 && eyeFrame < 2) || (eyeOpen == 2 && eyeFrame < 3))
				{
					eyeFrameCounter++;
					if(eyeFrameCounter > 4)
					{
						eyeFrame++;
						eyeFrameCounter = 0;
					}
				}
				else if(eyeFrame >= 3)
				{
					if(eyeOpen == 1)
					{
						eyeFrame = 2;
					}
					else
					{
						float targetRot = (float)Math.Atan2(Main.player[npc.target].Center.Y-(npc.Center.Y+22),Main.player[npc.target].Center.X-npc.Center.X);
						if(targetRot >= (float)(Math.PI*2))
						{
							targetRot -= (float)(Math.PI*2);
						}
						if(targetRot < 0)
						{
							targetRot += (float)(Math.PI*2);
						}
						//eyeFrame = 3 + (int)Math.Floor(Math.Min(8 * (targetRot / (float)(Math.PI*2)),7));
						eyeFrame = (int)(3 + (float)Math.Round(7 * (targetRot / (float)(Math.PI*2))));
					}
				}
			}
			else
			{
				npc.dontTakeDamage = true;
				npc.damage = 0;
				
				if(eyeFrame > 0)
				{
					if(eyeFrame > 2)
					{
						eyeFrame = 2;
					}
					eyeFrameCounter++;
					if(eyeFrameCounter > 4)
					{
						eyeFrame--;
						eyeFrameCounter = 0;
					}
				}
			}
			
			npc.frame.Y = eyeFrame;
			
			npc.frameCounter += 1;
			if(npc.frameCounter > 8)
			{
				npc.frame.X += frameNum;
				if(npc.frame.X <= 0f)
				{
					frameNum = 1;
				}
				if(npc.frame.X >= 2f)
				{
					frameNum = -1;
				}
				npc.frameCounter = 0;
			}
		}

		int spawnFireBall(float posX, float posY, bool playSound = true)
		{
			if(playSound)
				Main.PlaySound(SoundLoader.customSoundType, (int)posX, (int)posY, mod.GetSoundSlot(SoundType.Custom, "Sounds/PhantoonFire"));

			if (Main.netMode == NetmodeID.MultiplayerClient) return (200);
			int fb = NPC.NewNPC((int)posX,(int)posY,mod.NPCType("PhantoonFireBall"),npc.whoAmI, npc.whoAmI);
			Main.npc[fb].TargetClosest(true);
			Main.npc[fb].netUpdate = true;
			return fb;
		}
		
		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if(npc.ai[2] == 2f)
			{
				npc.ai[3] += damage / 2;
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(npc.ai[2] == 2f)
			{
				npc.ai[3] += damage / 2;
			}
		}

		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Player player = Main.player[npc.target];

			npc.direction = 1;
			npc.spriteDirection = npc.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (npc.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Color color = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));

			Texture2D texMain = mod.GetTexture("NPCs/Phantoon/Phantoon_Main"),
					texBottom = mod.GetTexture("NPCs/Phantoon/Phantoon_Bottom");

			// draw base
			int texH = (texMain.Height/11);
			sb.Draw(texMain,new Vector2((int)(npc.Center.X - Main.screenPosition.X),(int)(npc.Center.Y - Main.screenPosition.Y)),new Rectangle?(new Rectangle(0,texH*npc.frame.Y,texMain.Width,texH)),color,npc.rotation,new Vector2(texMain.Width/2,96),1f,effects,0f);

			// draw bottom
			int texBH = (texBottom.Height/3);
			sb.Draw(texBottom,new Vector2((int)(npc.Center.X - Main.screenPosition.X),(int)(npc.Center.Y - Main.screenPosition.Y)),new Rectangle?(new Rectangle(0,texBH*npc.frame.X,texBottom.Width,texBH)),color,npc.rotation,new Vector2(texBottom.Width/2,-54),1f,effects,0f);

			return false;
		}
	}
}
