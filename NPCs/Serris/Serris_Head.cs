using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria.ID;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Serris
{
    public class Serris_Head : ModNPC
    {
        bool TailSpawned = false;
		bool SpeedBoost = false;
		bool TimeLock = false;
		int hitDelay = 0;
		int hitTime = 45;
		int SoundDelay = 0;
		int srs = 0;
		int state = 1;
		int Previous = 0;
		public bool isX = false;
		public bool becameX = false;
		bool immuneFlash = true;
		bool TimeLock2 = false;
		int numUpdates = 0;
		int maxUpdates = 0;
		int hitDelay2 = 0;
		//int aiCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.width = 70;
			npc.height = 70;
			npc.damage = 20;
			npc.defense = 28;
			npc.lifeMax = 1750;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
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
			npc.damage = (int)(npc.damage * 0.7f);
		}
		
		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (!player.dead)
			{
				npc.timeLeft = 60;
			}
			maxUpdates = 0;
			if(!becameX)
			{
				if(numUpdates == 0)
				{
					npc.ai[0]++;
				}
				if (!TailSpawned)
				{
					int body = mod.NPCType("Serris_Body");
					int tail = mod.NPCType("Serris_Tail");
					Previous = npc.whoAmI;
					for (int num36 = 0; num36 < 8; num36++)
					{
						if (num36 >= 0 && num36 < 7)
						{
							srs = NPC.NewNPC((int) npc.position.X+(npc.width/2), (int) npc.position.Y+(npc.height/2), body, npc.whoAmI);
						}
						else
						{
							srs = NPC.NewNPC((int) npc.position.X+(npc.width/2), (int) npc.position.Y+(npc.height/2), tail, npc.whoAmI);
						}
						Main.npc[srs].realLife = npc.whoAmI;
						Main.npc[srs].ai[2] = (float)npc.whoAmI;
						Main.npc[srs].ai[1] = (float)Previous;
						if(Previous != npc.whoAmI)
						{
							Main.npc[Previous].ai[0] = (float)srs;
						}
						NetMessage.SendData(23, -1, -1, null, srs, 0f, 0f, 0f, 0);
						Previous = srs;
					}
					TailSpawned = true;
				}
				if(!Main.npc[srs].active && !isX)
				{
					TailSpawned = false;
				}
				float mult = 1.075f;
					if(npc.life <= (int)(npc.lifeMax) && npc.life >= (int)(npc.lifeMax * 0.8f))
					{
						hitTime = 45;
						state = 1;
					}
					if(npc.life < (int)(npc.lifeMax * 0.8f) && npc.life >= (int)(npc.lifeMax * 0.6f))
					{
						hitTime = 35;
						state = 2;
					}
					if(npc.life < (int)(npc.lifeMax * 0.6f))
					{
						hitTime = 25;
						state = 3;
					}
					if (Math.Abs(npc.position.X - player.position.X) + Math.Abs(npc.position.Y - player.position.Y) > 2500f)
					{
						npc.velocity = npc.DirectionTo(player.Center) * 5;
					}
				if(npc.ai[0] <= 1 || npc.ai[0] >= 480)
				{
					SpeedBoost = false;
					TimeLock = true;
					SoundDelay = 0;
					npc.dontTakeDamage = false;
					npc.damage = 20;
					Main.npc[srs].damage = 20;
				}
				else if(npc.ai[0] >= 2)
				{
					npc.dontTakeDamage = true;
					mult = 1.25f;
					maxUpdates = 1;
					SpeedBoost = true;
					state = 4;
					Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 2.0f, 2.0f, 2.0f);
					if(numUpdates == 0)
					{
						SoundDelay++;
					}
					npc.damage = 60;
					Main.npc[srs].damage = 60;
					if(SoundDelay > 14)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SerrisAccel"));
						SoundDelay = 0;
					}
				}
				npc.position += npc.velocity*mult;
				if(npc.justHit && hitDelay <= 0)
				{
					//TimeLock = false;
					//npc.ai[0] = 2;
					hitDelay = 1;
					Main.PlaySound(15,(int)npc.position.X,(int)npc.position.Y,0);
				}
				if(hitDelay > 0)
				{
					hitDelay++;
					npc.position -= npc.velocity*mult;
				}
					
				if (hitDelay >= hitTime)
				{
					TimeLock = false;
					npc.ai[0] = 2;
					hitDelay = 0;
				}
				if(TimeLock)
				{
					npc.ai[0] = 0;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5)
				{
					npc.aiStyle = -1;
					npc.width = 70;
					npc.height = 70;
					becameX = true;
					npc.ai[0] = 1f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					return;
				}
			}
			else if(npc.ai[0] == 1f || npc.ai[0] == 2f)
			{
				if (npc.ai[0] == 1f)
				{
					npc.ai[2] += 0.005f;
					if ((double)npc.ai[2] > 0.5)
					{
						npc.ai[2] = 0.5f;
					}
				}
				else
				{
					npc.ai[2] -= 0.005f;
					if (npc.ai[2] < 0f)
					{
						npc.ai[2] = 0f;
					}
				}
				npc.dontTakeDamage = true;
				npc.rotation += npc.ai[2];
				npc.ai[1] += 1f;
				if (npc.ai[1] >= 200f)
				{
					if(npc.ai[1] == 200f || npc.ai[1] == 250f)
					{
						npc.ai[0] += 1f;
					}
					if(npc.ai[1] == 300f)
					{
						npc.ai[0] += 1f;
						npc.ai[1] = 0f;
					}
					if (npc.ai[0] == 3f)
					{
						npc.ai[2] = 0f;
					}
					else if(npc.ai[1] == 200f || npc.ai[1] == 250f)
					{
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);
						int gore = Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("SerrisXTransGore1"), 1f);
						Main.gore[gore].velocity *= 0.4f;
						Main.gore[gore].timeLeft = 60;
						gore = Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("SerrisXTransGore2"), 1f);
						Main.gore[gore].velocity *= 0.4f;
						Main.gore[gore].timeLeft = 60;
						for (int v = 0; v < 3; v++)
						{
							gore = Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("SerrisXTransGore3"), 1f);
							Main.gore[gore].velocity *= 0.4f;
							Main.gore[gore].timeLeft = 60;
						}
						for (int num136 = 0; num136 < 20; num136++)
						{
							Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f, 0, default(Color), 1f);
						}
						//Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
						isX = true;
						npc.position.X += (float)(npc.width / 2);
						npc.position.Y += (float)(npc.height / 2);
						npc.width = 94;
						npc.height = 94;
						npc.position.X -= (float)(npc.width / 2);
						npc.position.Y -= (float)(npc.height / 2);
					}
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
					return;
				}
			}
			else
			{
				if(isX)
				{
					if(npc.life <= (int)(npc.lifeMax * 0.5f) && npc.life >= (int)(npc.lifeMax * 0.3f))
					{
						state = 5;
					}
					if(npc.life < (int)(npc.lifeMax * 0.3f) && npc.life >= (int)(npc.lifeMax * 0.1f))
					{
						state = 6;
					}
					if(npc.life < (int)(npc.lifeMax * 0.1f))
					{
						state = 7;
					}
					npc.aiStyle = 5;
					npc.width = 70;
					npc.height = 70;
					npc.damage = 30;
					npc.HitSound = SoundID.NPCHit8;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.knockBackResist = 0.5f;
					npc.ai[3]++;
					npc.position += npc.velocity * 1.5f;
					if(npc.ai[3] <= 1 || npc.ai[3] >= 150)
					{
						immuneFlash = false;
						npc.dontTakeDamage = false;
						TimeLock2 = true;
					}
					else if(npc.ai[3] >= 2)
					{
						immuneFlash = true;
						npc.dontTakeDamage = true;
					}
					if(npc.justHit && hitDelay2 <= 0)
					{
						//TimeLock2 = false;
						//npc.ai[3] = 2;
						hitDelay2 = 1;
					}
					if(hitDelay2 > 0)
					{
						hitDelay2++;
					}
					if (hitDelay2 >= 15)
					{
						TimeLock2 = false;
						npc.ai[3] = 2;
						hitDelay2 = 0;
					}
					if(TimeLock2)
					{
						npc.ai[3] = 0;
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
		}
		public override void PostAI()
		{
			if(isX)
			{
				npc.rotation = 0f;
			}
			else
			{
				if (npc.velocity.X < 0f)
				{
					npc.spriteDirection = 1;
				}
				else if (npc.velocity.X > 0f)
				{
					npc.spriteDirection = -1;
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
			if (npc.spriteDirection == -1)
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
				for (int m = 0; m < (npc.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, npc.life <= 0 && m % 2 == 0 ? 3f : 1f);
					if (npc.life <= 0 && m % 2 == 0)
					{
						Main.dust[dustID].noGravity = true;
					}
				}
				if (npc.life <= 0)
				{
					int num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("SerrisGore1"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore85 = Main.gore[num373];
					gore85.velocity.X = gore85.velocity.X + 1f;
					gore85.velocity.Y = gore85.velocity.Y + 1f;
					num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("SerrisGore1"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore87 = Main.gore[num373];
					gore87.velocity.X = gore87.velocity.X - 1f;
					gore87.velocity.Y = gore87.velocity.Y + 1f;
					num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("SerrisGore1"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore89 = Main.gore[num373];
					gore89.velocity.X = gore89.velocity.X + 1f;
					gore89.velocity.Y = gore89.velocity.Y - 1f;
					num373 = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("SerrisGore1"), 1f);
					Main.gore[num373].velocity *= 0.4f;
					Main.gore[num373].timeLeft = 60;
					Gore gore91 = Main.gore[num373];
					gore91.velocity.X = gore91.velocity.X - 1f;
					gore91.velocity.Y = gore91.velocity.Y - 1f;
				}
			}
		}
		float xFrame = 0f;
		int xHeight = 0;
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D tex = mod.GetTexture("NPCs/Serris/SerrisCoreX");
			xHeight = tex.Height;
			Rectangle rect = new Rectangle(0, (int)xFrame, tex.Width, (tex.Height/8));
			Vector2 origin = new Vector2((float)(rect.Width/2), (float)(rect.Height/8) / 2);
			SpriteEffects effects = SpriteEffects.None;
			if (npc.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Color buffColor = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			Color alpha2 = npc.GetAlpha(buffColor);
			if (npc.behindTiles)
			{
				int num44 = (int)((npc.position.X - 8f) / 16f);
				int num45 = (int)((npc.position.X + (float)npc.width + 8f) / 16f);
				int num46 = (int)((npc.position.Y - 8f) / 16f);
				int num47 = (int)((npc.position.Y + (float)npc.height + 8f) / 16f);
				for (int m = num44; m <= num45; m++)
				{
					for (int n = num46; n <= num47; n++)
					{
						if (Lighting.Brightness(m, n) == 0f)
						{
							buffColor = Color.Black;
						}
					}
				}
			}
			if(isX)
			{
				sb.Draw(tex, new Vector2(npc.position.X - Main.screenPosition.X + (float)(npc.width/2) - (float)tex.Width / 2f + origin.X, npc.position.Y - Main.screenPosition.Y + (float)npc.height - (float)(tex.Height / 8) + 4f + origin.Y), new Rectangle?(rect), alpha2, 0f, origin, 1f, effects, 0f);
			}
			Texture2D tex2 = Main.npcTexture[npc.type];
			Rectangle rect2 = new Rectangle((int)npc.frame.X, (int)npc.frame.Y, (tex2.Width/3), (tex2.Height/Main.npcFrameCount[npc.type]));
			Vector2 vector13 = new Vector2((float)((tex2.Width/3) / 2), (float)((tex2.Height/Main.npcFrameCount[npc.type]) / 2));
			sb.Draw(tex2, new Vector2(npc.position.X - Main.screenPosition.X + (float)(npc.width / 2) - (float)(tex2.Width/3) / 2f + vector13.X, npc.position.Y - Main.screenPosition.Y + (float)npc.height - (float)(tex2.Height / Main.npcFrameCount[npc.type]) + 4f + vector13.Y), new Rectangle?(rect2), alpha2, npc.rotation, vector13, 1f, effects, 0f);
			return false;
		}
		int xCounter = 0;
		public override void FindFrame(int frameHeight)
		{
			int num = 1;
			if (!Main.dedServ)
			{
				num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			}
			int num2 = 1;
			if (!Main.dedServ)
			{
				num2 = xHeight/8;
			}
			npc.frameCounter += 1.0 / (double)(1+maxUpdates);
			xCounter++;
			if(SpeedBoost && !isX)
			{
				if(npc.frameCounter >= 0 && npc.frameCounter < 5)
				{
					npc.frame.Y = num;
				}
				if(npc.frameCounter >= 5 && npc.frameCounter < 10)
				{
					npc.frame.Y = num*2;
				}
				if(npc.frameCounter >= 10)
				{
					npc.frameCounter = 0;
				}
			}
			else if(isX)
			{
				if(immuneFlash)
				{
					if(npc.frameCounter >= 0 && npc.frameCounter < 5)
					{
						npc.frame.Y = num*4;
					}
					if(npc.frameCounter >= 5 && npc.frameCounter < 10)
					{
						npc.frame.Y = num*3;
					}
					if(npc.frameCounter >= 10)
					{
						npc.frameCounter = 0;
					}
				}
				else
				{
					npc.frame.Y = num*3;
				}

				if (xCounter >= 0 && xCounter < 5)
				{
					xFrame = 0;
				}
				if (xCounter >= 5 && xCounter < 10)
				{
					xFrame = num2;
				}
				if (xCounter >= 10 && xCounter < 15)
				{
					xFrame = num2*2;
				}
				if (xCounter >= 15 && xCounter < 20)
				{
					xFrame = num2*3;
				}
				if (xCounter >= 20 && xCounter < 25)
				{
					xFrame = num2*4;
				}
				if (xCounter >= 25 && xCounter < 30)
				{
					xFrame = num2*5;
				}
				if (xCounter >= 30 && xCounter < 35)
				{
					xFrame = num2*6;
				}
				if (xCounter >= 35 && xCounter < 40)
				{
					xFrame = num2*7;
				}
				if (xCounter >= 40)
				{
					xCounter = 0;
				}
			}
			else
			{
				npc.frame.Y = 0;
				npc.frameCounter = 0;
			}

			int num3 = 1;
			if (!Main.dedServ)
			{
				num3 = Main.npcTexture[npc.type].Width / 3;
			}
			if(isX)
			{
				if(npc.life <= (int)(npc.lifeMax * 0.5f) && npc.life >= (int)(npc.lifeMax * 0.3f))
				{
					npc.frame.X = 0;
				}
				if(npc.life < (int)(npc.lifeMax * 0.3f) && npc.life >= (int)(npc.lifeMax * 0.1f))
				{
					npc.frame.X = num3;
				}
				if(npc.life < (int)(npc.lifeMax * 0.1f))
				{
					npc.frame.X = num3*2;
				}
			}
			else
			{
				if(npc.life <= (int)(npc.lifeMax) && npc.life >= (int)(npc.lifeMax * 0.8f))
				{
					npc.frame.X = 0;
				}
				if(npc.life < (int)(npc.lifeMax * 0.8f) && npc.life >= (int)(npc.lifeMax * 0.6f))
				{
					npc.frame.X = num3;
				}
				if(npc.life < (int)(npc.lifeMax * 0.6f))
				{
					npc.frame.X = num3*2;
				}
			}
		}
    }
}
