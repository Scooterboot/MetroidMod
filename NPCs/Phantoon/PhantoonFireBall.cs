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
    public class PhantoonFireBall : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Ball");
			Main.npcFrameCount[npc.type] = 7;
		}
		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 28;
			npc.damage = 0;//50;
			npc.defense = 10;
			npc.lifeMax = 50;
			npc.knockBackResist = 0;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath3;
			npc.value = 0;
			npc.lavaImmune = true;
			npc.behindTiles = false;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[44] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;
			
			npc.dontTakeDamage = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
		}
		int damage = 66;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			damage *= 2;
		}
		
		Vector2 startPos;
		
		bool initialized = false;
		public override bool PreAI()
		{
			if(!initialized)
			{
				startPos = npc.Center;
				initialized = true;
			}
			return true;
		}

		float dist = 0;//120;
		bool bounced = false;
		int bounceCounter = 0;
		int frameSet = 0;
		int currentFrame = 0;
		int timeLeft = 300;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			NPC creator = Main.npc[(int)npc.ai[0]];

			if(npc.ai[1] == -1) // phantoon spawn animation behavior
			{
				npc.active = creator.active;
				if(npc.ai[3] == 1 || npc.ai[3] == 2)
				{
					npc.ai[2] += (float)Math.PI/120;
					
					if(npc.ai[3] == 2)
					{
						dist -= 1f;
					}
				}
				else
				{
					dist = 120;
				}
				
				npc.Center = creator.Center + new Vector2((float)Math.Cos(npc.ai[2])*dist,(float)Math.Sin(npc.ai[2])*dist);
				
				if(dist <= 10f)
				{
					npc.alpha += 25;
					if(npc.alpha >= 255)
					{
						npc.active = false;
					}
				}
				npc.frameCounter++;
				if(npc.frameCounter > 4)
				{
					npc.frame.Y++;
					npc.frameCounter = 0;
				}
				if(npc.frame.Y >= 3)
				{
					npc.frame.Y = 0;
				}
			}
			else
			{
				if(timeLeft > 0)
				{
					if(npc.dontTakeDamage)
					{
						npc.alpha = 127;
					}
					else
					{
						npc.alpha = 0;
					}
					timeLeft--;
				}
				else
				{
					npc.alpha += 25;
					if(npc.alpha >= 255)
					{
						npc.active = false;
					}
				}
			}

			if(npc.ai[1] == 0) // basic bounce behavior
			{
				npc.noTileCollide = false;
				
				npc.frameCounter++;
				if(npc.frameCounter > 4)
				{
					currentFrame++;
					npc.frameCounter = 0;
				}
				if(currentFrame >= 3)
				{
					currentFrame = 0;
				}
				
				if(npc.ai[2] <= 0f)
				{
					if(!bounced)
					{
						frameSet = 0;
						if(npc.velocity.Y == 0f)
						{
							if(bounceCounter > 0)
							{
								currentFrame = 0;
								frameSet = 6;
								if(timeLeft > 0)
								{
									npc.alpha = 0;
								}
								npc.velocity = Vector2.Zero;
							}
							bounceCounter++;
						}
						if(bounceCounter > 10)
						{
							npc.velocity.X = (10-Main.rand.Next(21))/2;
							npc.velocity.Y = -(3+Main.rand.Next(4));
							bounced = true;
						}
					}
					else
					{
						if(npc.velocity.Y == 0f)
						{
							npc.life = 0;
							npc.HitEffect(0, 10.0);
							npc.active = false;
						}
						frameSet = 3;
						npc.dontTakeDamage = false;
						npc.damage = damage;
					}
					npc.velocity.Y += 0.1f;
				}
				else
				{
					npc.ai[2] -= 1f;
					npc.dontTakeDamage = false;
					npc.damage = damage;
				}
				
				npc.frame.Y = currentFrame + frameSet;
			}

			if(npc.ai[1] == 1) // targeting behavior
			{
				npc.frameCounter++;
				if(npc.frameCounter > 4)
				{
					currentFrame++;
					npc.frameCounter = 0;
				}
				if(currentFrame >= 3)
				{
					currentFrame = 0;
				}

				float targetRot = (float)Math.Atan2(player.Center.Y-npc.Center.Y,player.Center.X-npc.Center.X);
				if(npc.ai[3] < 30)
				{
					if(creator.active)
					{
						npc.Center = creator.Center + new Vector2((float)Math.Cos(npc.ai[2])*dist,(float)Math.Sin(npc.ai[2])*dist);
						startPos = creator.Center;
					}
					else
					{
						npc.Center = startPos + new Vector2((float)Math.Cos(npc.ai[2])*dist,(float)Math.Sin(npc.ai[2])*dist);
					}
					
					if(dist < 120)
					{
						dist += 6f;
					}
					else
					{
						npc.ai[3]++;
					}

					if(npc.ai[3] == 30)
					{
						npc.velocity = targetRot.ToRotationVector2()*12;
					}
					frameSet = 0;
					npc.dontTakeDamage = true;
				}
				else
				{
					frameSet = 3;
					//npc.dontTakeDamage = false;
					npc.damage = damage;
					//npc.velocity += targetRot.ToRotationVector2()*0.2f;
				}
				
				npc.frame.Y = currentFrame + frameSet;
			}

			if(npc.ai[1] == 2) // super fireball
			{
				npc.dontTakeDamage = false;
				npc.damage = damage;
				float targetRot = (float)Math.Atan2(player.Center.Y-npc.Center.Y,player.Center.X-npc.Center.X);
				if(npc.ai[2] == 0f)
				{
					for(int i = 0; i < 4; i++)
					{
						int fb = NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y,mod.NPCType("PhantoonFireBall"),npc.whoAmI);
						Main.npc[fb].target = npc.target;
						Main.npc[fb].ai[0] = npc.whoAmI;
						Main.npc[fb].ai[1] = 3;
						Main.npc[fb].ai[2] = (((float)Math.PI/2)*i);
					}
					npc.velocity = targetRot.ToRotationVector2();
					npc.ai[2] = 1f;
				}
				else
				{
					if(npc.velocity.Length() < 16)
					{
						npc.velocity *= 1.025f;
						if(Vector2.Distance(player.Center,npc.Center) <= 600)
						{
							npc.velocity += targetRot.ToRotationVector2()*0.5f;
						}
					}
				}
				
				npc.frameCounter++;
				if(npc.frameCounter > 4)
				{
					npc.frame.Y++;
					npc.frameCounter = 0;
				}
				if(npc.frame.Y >= 6 || npc.frame.Y < 3)
				{
					npc.frame.Y = 3;
				}
			}

			if(npc.ai[1] == 3) // super fireball #2
			{
				if(creator.type == npc.type && creator.active && creator.ai[1] == 2)
				{
					npc.Center = creator.Center + new Vector2((float)Math.Cos(npc.ai[2])*28,(float)Math.Sin(npc.ai[2])*28) - creator.velocity;
					npc.velocity = creator.velocity;
					npc.ai[2] += (float)Math.PI/60;
					
					npc.frameCounter++;
					if(npc.frameCounter > 4)
					{
						npc.frame.Y++;
						npc.frameCounter = 0;
					}
					if(npc.frame.Y >= 3)
					{
						npc.frame.Y = 0;
					}
					npc.damage = damage;
				}
				else
				{
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.dontTakeDamage = true;
					npc.damage = 0;
				}
			}

			if(npc.ai[1] == 4) // behavior for when phantoon opens his eye
			{
				npc.Center = startPos + new Vector2((float)Math.Cos(npc.ai[2])*dist,(float)Math.Sin(npc.ai[2])*dist);
				dist += 8f;
				npc.ai[2] += (float)Math.PI/60;
				
				npc.dontTakeDamage = false;
				npc.damage = damage;
				npc.frameCounter++;
				if(npc.frameCounter > 4)
				{
					npc.frame.Y++;
					npc.frameCounter = 0;
				}
				if(npc.frame.Y >= 3)
				{
					npc.frame.Y = 0;
				}
			}

			float num = (255f - npc.alpha) / 255f;
			Lighting.AddLight(npc.Center, 0f,0.75f*num,1f*num);
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];
			int texH = (tex.Height/7);
			sb.Draw(tex,npc.Center - Main.screenPosition,new Rectangle?(new Rectangle(0,texH*npc.frame.Y,tex.Width,texH)),npc.GetAlpha(Color.White),0f,new Vector2(tex.Width/2,texH-(npc.height/2)-1),1f,SpriteEffects.None,0f);
			return false;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 59, 0f, -(Main.rand.Next(4)/2), 100, Color.White, 1.5f);
				Main.dust[dust].noGravity = true;
			}
			if(npc.life <= 0)
			{
				for(int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 56, 0f, -(Main.rand.Next(3)/2), 100, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
