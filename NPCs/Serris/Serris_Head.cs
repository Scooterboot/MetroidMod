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

namespace MetroidMod.NPCs.Serris
{
    public class Serris_Head : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 15;
		}
		int damage = 20;
		int speedDamage = 35;//60;
		int coreDamage = 30;
		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 60;
			npc.damage = damage;
			npc.defense = 28;
			npc.lifeMax = 4000;//1750;
			//npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/CoreXDeath");
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 7, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.frameCounter = 0;
			npc.aiStyle = 6;
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

			return NPC.NewNPC((int)MathHelper.Clamp(tileX,num11,num12) * 16 + 8, (int)MathHelper.Clamp(tileY,num13,num14) * 16, this.npc.type, 0, 0f, 0f, 0f, 0f, 255);
		}
		
		bool tailSpawned = false;
		bool initialBoost = false;
		SoundEffectInstance soundInstance;
		int numUpdates = 0;
		int maxUpdates = 0;
		NPC[] body = new NPC[10];
		int state = 1;
		float mouthFrame = 0f;
		int mouthNum = 1;
		int glowFrame = 0;
		int glowNum = 1;
		int glowFrameCounter = 0;
		float oldRot = 0f;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (!player.dead)
			{
				npc.timeLeft = 60;
			}
			// base phase
			if(npc.localAI[0] == 0)
			{
				// handle spawning body segments
				if(!tailSpawned)
				{
					int prev = npc.whoAmI;
					for(int i = 0; i < body.Length; i++)
					{
						int type = mod.NPCType("Serris_Body");
						if(i > 6)
						{
							type = mod.NPCType("Serris_Tail");
						}
						int srs = NPC.NewNPC((int) npc.Center.X, (int) npc.Center.Y, type, npc.whoAmI);
						body[i] = Main.npc[srs];
						body[i].realLife = npc.whoAmI;
						body[i].ai[2] = (float)npc.whoAmI;
						body[i].ai[1] = (float)prev;
						if(i > 7)
						{
							body[i].localAI[0] = 1f;
						}
						if(prev != npc.whoAmI)
						{
							Main.npc[prev].ai[0] = (float)srs;
						}
						NetMessage.SendData(23, -1, -1, null, srs, 0f, 0f, 0f, 0);
						prev = srs;
					}
					tailSpawned = true;
				}
				for(int i = 0; i < body.Length; i++)
				{
					if(body[i] == null || !body[i].active)
					{
						tailSpawned = false;
					}
				}
				
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
					mouthFrame = 0f;
					glowFrame = 1;
					npc.localAI[0] = 1;
					npc.localAI[1] = 0;
					npc.localAI[2] = 0;
					npc.localAI[3] = 0;
					npc.aiStyle = -1;
					return;
				}
				
				// normal movement
				if(npc.localAI[2] == 0)
				{
					npc.damage = damage;
					//npc.dontTakeDamage = false;
					npc.chaseable = true;
					//npc.position += npc.velocity*1.075f;
					maxUpdates = 1;
					oldRot = npc.rotation;
					
					//initial speed boost
					if(!initialBoost)
					{
						npc.localAI[2] = 1;
						npc.localAI[3] = 30;
						npc.TargetClosest(true);
						initialBoost = true;
					}
					
					// activate speed boost on hit
					if(npc.justHit)
					{
						npc.localAI[2] = 1;
						npc.TargetClosest(true);
					}
				}
				// stunned movement
				if(npc.localAI[2] == 1)
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
						npc.localAI[2] = 2;
						npc.localAI[3] = 0;
					}
				}
				// speedboost movement
				if(npc.localAI[2] == 2)
				{
					state = 4;
					mouthNum = -3;
					npc.damage = speedDamage;
					//npc.dontTakeDamage = true;
					npc.chaseable = false;
					npc.position += npc.velocity*1.5f;
					maxUpdates = 1;
					
					Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 2.0f, 2.0f, 2.0f);
					
					if(soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SerrisAccel"));
					}
					else
					{
						Vector2 screenPos = new Vector2(Main.screenPosition.X + (float)Main.screenWidth * 0.5f, Main.screenPosition.Y + (float)Main.screenHeight * 0.5f);
						
						float pan = (npc.Center.X - screenPos.X) / ((float)Main.screenWidth * 0.5f);
						float numX = Math.Abs(npc.Center.X - screenPos.X);
						float numY = Math.Abs(npc.Center.Y - screenPos.Y);
						float numL = (float)Math.Sqrt((double)(numX * numX + numY * numY));
						float volume = 1f - numL / ((float)Main.screenWidth * 1.5f);
						
						if (pan < -1f)
						{
							pan = -1f;
						}
						if (pan > 1f)
						{
							pan = 1f;
						}
						if (volume > 1f)
						{
							volume = 1f;
						}
						if (volume <= 0f)
						{
							volume = 0f;
							soundInstance.Stop(true);
						}
						
						if(soundInstance != null)
						{
							soundInstance.Volume = volume;
							soundInstance.Pan = pan;
						}
					}
					
					if(numUpdates <= 0)
					{
						npc.localAI[3]++;
					}
					if(npc.localAI[3] > 480)
					{
						npc.localAI[2] = 0;
						npc.localAI[3] = 0;
						npc.TargetClosest(true);
					}
				}
				else
				{
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
				}
			}
			else
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				npc.damage = coreDamage;
			}
			
			// transform phase
			if(npc.localAI[0] == 1)
			{
				npc.aiStyle = -1;
				for(int i = 0; i < body.Length; i++)
				{
					if(body[i] != null && body[i].active)
					{
						body[i].life = 0;
						body[i].HitEffect(0, 10.0);
						body[i].active = false;
					}
				}
				if(npc.localAI[1] == 0)
				{
					Main.PlaySound(SoundID.NPCDeath14,npc.Center);
				}
				
				npc.localAI[2] += 0.01f;
				if ((double)npc.localAI[2] > 0.5)
				{
					npc.localAI[2] = 0.5f;
				}
				//npc.dontTakeDamage = true;
				npc.chaseable = false;
				npc.rotation += npc.localAI[2];
				npc.localAI[1] += 1f;
				if(npc.localAI[1] <= 1f)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SerrisDeath"));
				}
				if(npc.localAI[1] > 170f)
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);
					int gore = Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/SerrisXTransGore1"), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
					gore = Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/SerrisXTransGore2"), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
					for (int v = 0; v < 3; v++)
					{
						gore = Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/SerrisXTransGore3"), 1f);
						Main.gore[gore].velocity *= 0.4f;
						Main.gore[gore].timeLeft = 60;
					}
					for (int num136 = 0; num136 < 20; num136++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f, 0, default(Color), 1f);
					}
					//Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
					npc.position.X += (float)(npc.width / 2);
					npc.position.Y += (float)(npc.height / 2);
					npc.width = 70;
					npc.height = 70;
					npc.position.X -= (float)(npc.width / 2);
					npc.position.Y -= (float)(npc.height / 2);
					
					npc.localAI[0] = 2;
					npc.localAI[1] = 0;
					npc.localAI[2] = 0;
					npc.localAI[3] = 100;
				}
				
				npc.velocity.X = npc.velocity.X * 0.98f;
				npc.velocity.Y = npc.velocity.Y * 0.98f;
				if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
				{
					npc.velocity.X = 0f;
				}
				if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
				{
					npc.velocity.Y = 0f;
				}
			}
			// core x phase
			if(npc.localAI[0] == 2)
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
					//npc.dontTakeDamage = true;
					npc.chaseable = false;
					npc.position -= npc.velocity;
					npc.velocity *= 0f;
					npc.localAI[3]--;
				}
				else
				{
					if(npc.localAI[1] == 0)
					{
						//npc.dontTakeDamage = false;
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
						//npc.dontTakeDamage = true;
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
					{
						npc.velocity.Y = npc.velocity.Y + 0.1f;
					}
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
			return (npc.localAI[0] == 0 && npc.localAI[2] != 2) || (npc.localAI[0] == 2 && npc.localAI[1] != 2);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return (npc.localAI[0] == 0 && npc.localAI[2] != 2) || (npc.localAI[0] == 2 && npc.localAI[1] != 2);
		}
		int spriteDir = 1;
		public override void PostAI()
		{
			if(npc.localAI[0] == 2)
			{
				npc.rotation = 0f;
			}
			else
			{
				if (npc.velocity.X > 0f)
				{
					spriteDir = 1;
				}
				if (npc.velocity.X < 0f)
				{
					spriteDir = -1;
				}
				npc.spriteDirection = spriteDir;
			}
			
			if(!npc.active)
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
			}
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
			if (spriteDir == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
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
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if(npc.localAI[0] == 2)
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
					
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
				}
			}
		}
		
		int sbFrame = 0;
		int sbFrameCounter = 0;
		int coreFrame = 0;
		int coreFrameCounter = 0;
		int flashFrame = 0;
		int flashFrameCounter = 0;
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			if(npc.localAI[0] < 2)
			{
				Texture2D texHead = mod.GetTexture("NPCs/Serris/Serris_Head"),
					texJaw = mod.GetTexture("NPCs/Serris/Serris_Jaw"),
					texBody = mod.GetTexture("NPCs/Serris/Serris_Body"),
					texFins = mod.GetTexture("NPCs/Serris/Serris_Fins"),
					texTail = mod.GetTexture("NPCs/Serris/Serris_Tail");
				Vector2 headOrig = new Vector2(34,31),
					jawOrig1 = new Vector2(30,11),
					jawOrig2 = new Vector2(34,1),
					bodyOrig = new Vector2(32,35),
					finsOrig = new Vector2(52,31),
					tailOrig = new Vector2(28,29);
				int headHeight = texHead.Height / 15,
					jawHeight = texJaw.Height / 5,
					bodyHeight = texBody.Height / 10,
					finsHeight = texFins.Height / 15,
					tailHeight = texTail.Height / 15;
				SpriteEffects effects = SpriteEffects.None;
				if (spriteDir == -1)
				{
					effects = SpriteEffects.FlipVertically;
					headOrig.Y = headHeight - headOrig.Y;
					jawOrig1.Y = jawHeight - jawOrig1.Y;
					jawOrig2.Y = jawHeight - jawOrig2.Y;
					bodyOrig.Y = bodyHeight - bodyOrig.Y;
					finsOrig.Y = finsHeight - finsOrig.Y;
					tailOrig.Y = tailHeight - tailOrig.Y;
				}
				int frame = state-1;
				if(state == 4)
				{
					frame = sbFrame+3;
				}
				
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
				
				for(int i = body.Length-1; i >= 0; i--)
				{
					if(body[i] != null && body[i].active)
					{
						Vector2 bpos = body[i].Center;
						float bodyRot = body[i].rotation - 1.57f;
						Color bodyColor = npc.GetAlpha(Lighting.GetColor((int)bpos.X / 16, (int)bpos.Y / 16));
						if(i > 6)
						{
							int yFrame = frame * (tailHeight*3);
							if(i == 8)
							{
								yFrame += tailHeight;
							}
							if(i == 9)
							{
								yFrame += tailHeight*2;
							}
							sb.Draw(texTail, bpos - Main.screenPosition, new Rectangle?(new Rectangle(0,yFrame,texTail.Width,tailHeight)), 
							bodyColor, bodyRot, tailOrig, 1f, effects, 0f);
						}
						else
						{
							if(i == 0)
							{
								for(int j = 0; j < 3; j++)
								{
									int finFrame = finsHeight*j + frame*(finsHeight*3);
									Vector2 finPos = new Vector2(4,-16);
									float bodyRot2 = body[i+1].rotation - 1.57f;
									Vector2 finRotPos = bodyRot.ToRotationVector2();
									if (float.IsNaN(finRotPos.X) || float.IsNaN(finRotPos.Y))
									{
										finRotPos = -Vector2.UnitY;
									}
									if(j == 0)
									{
										finPos = new Vector2(-14,-14);
										finRotPos = Vector2.Normalize(Vector2.Lerp(finRotPos,bodyRot2.ToRotationVector2(),0.5f));
									}
									if(j == 2)
									{
										finPos = new Vector2(20,-16);
										finRotPos = Vector2.Normalize(Vector2.Lerp(finRotPos,headRot.ToRotationVector2(),0.5f));
									}
									if(spriteDir == -1)
									{
										finPos.Y *= -1;
									}
									float finRot = finRotPos.ToRotation();
									finRot += (((float)Math.PI/16) - ((float)Math.PI/8)*(1f - mouthFrame))*0.5f * spriteDir;
									float finPosRot = finPos.ToRotation() + bodyRot;
									Vector2 finalFinPos = body[i].Center + finPosRot.ToRotationVector2() * finPos.Length();
									sb.Draw(texFins, finalFinPos - Main.screenPosition, new Rectangle?(new Rectangle(0,finFrame,texFins.Width,finsHeight)), 
									bodyColor, finRot, finsOrig, 1f, effects, 0f);
								}
							}
							int yFrame = frame * (bodyHeight*2);
							if(i > 1)
							{
								yFrame += bodyHeight;
							}
							sb.Draw(texBody, bpos - Main.screenPosition, new Rectangle?(new Rectangle(0,yFrame,texBody.Width,bodyHeight)), 
							bodyColor, bodyRot, bodyOrig, 1f, effects, 0f);
						}
					}
				}
				
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
    }
}
