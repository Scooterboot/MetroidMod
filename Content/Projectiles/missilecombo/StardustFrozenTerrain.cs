using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Graphics;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class StardustFrozenTerrain : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Terrain");
		}
		
		int size = 42;
		public override void SetDefaults()
		{
			Projectile.width = size;
			Projectile.height = size;
			Projectile.scale = 0.75f;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 0;
		}
		
		const int MaxRange = 424;//528;
		int range = 0;
		float[,] rotation = new float[MaxRange*2/16,MaxRange*2/16];
		float[,] alpha = new float[MaxRange*2/16,MaxRange*2/16];
		Vector2[,] addedPos = new Vector2[MaxRange*2/16,MaxRange*2/16];
		
		int[] freezeDelay = new int[Main.maxNPCs];
		
		bool guardSpawned = false;
		int damage = 0;

		bool init = false;
		public override void AI()
		{
			Projectile P = Projectile;
			if(!init)
			{
				for(int x = 0; x < rotation.GetLength(0); x++)
				{
					for(int y = 0; y < rotation.GetLength(1); y++)
					{
						rotation[x,y] = (float)Main.rand.Next(360) * ((float)Math.PI / 180);
					}
				}
				for(int x = 0; x < addedPos.GetLength(0); x++)
				{
					for(int y = 0; y < addedPos.GetLength(1); y++)
					{
						addedPos[x,y].X = (float)Main.rand.Next(-40,41) * 0.1f;
						addedPos[x,y].Y = (float)Main.rand.Next(-40,41) * 0.1f;
					}
				}
				P.spriteDirection = 1;
				if(Main.rand.NextBool(2))
				{
					P.spriteDirection = -1;
				}
				damage = P.damage;
				P.damage = 0;
				init = true;
			}
			
			range = Math.Min(range + 3, MaxRange);
			
			if(range > MaxRange/2 && !guardSpawned)
			{
				int g = Projectile.NewProjectile(Projectile.GetSource_FromAI(), P.Center.X, P.Center.Y-40f, 0f, 0f, ModContent.ProjectileType<StardustComboGuardian>(),damage,P.knockBack,P.owner);
				Main.projectile[g].ai[0] = P.whoAmI;
				guardSpawned = true;
			}
			
			int xmin = (int)(P.Center.X - MaxRange) / 16;
			int xmax = (int)(P.Center.X + MaxRange) / 16;
			int ymin = (int)(P.Center.Y - MaxRange) / 16;
			int ymax = (int)(P.Center.Y + MaxRange) / 16;
			for(int x = xmin; x < xmax; x++)
			{
				for(int y = ymin; y < ymax; y++)
				{
					Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f);
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						if(Vector2.Distance(pos,P.Center) <= range)
						{
							int fSize = (int)((float)size * P.scale * MathHelper.Clamp(alpha[x-xmin,y-ymin],0f,1f));
							if(fSize > 0)
							{
								Rectangle projRect = new Rectangle((int)pos.X-fSize/2,(int)pos.Y-fSize/2,fSize,fSize);
								for(int i = 0; i < Main.maxNPCs; i++)
								{
									if(Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].dontTakeDamage)
									{
										NPC npc = Main.npc[i];
										Rectangle npcRect = new Rectangle((int)npc.position.X,(int)npc.position.Y,npc.width,npc.height);
										
										if(projRect.Intersects(npcRect))
										{
											if(freezeDelay[i] <= 0)
											{
												npc.AddBuff(ModContent.BuffType<Buffs.IceFreeze>(),600,true);
												freezeDelay[i] = 20;
											}
											else
											{
												freezeDelay[i]--;
											}
										}
									}
								}
							}
						}
					}
					if(Vector2.Distance(pos,P.Center) <= range || range >= MaxRange)
					{
						float rate = 0.1f;
						if(P.timeLeft > 30)
						{
							alpha[x-xmin,y-ymin] = Math.Min(alpha[x-xmin,y-ymin] + rate, 1f + 2f*(Vector2.Distance(pos,P.Center)/MaxRange));
						}
						else
						{
							alpha[x-xmin,y-ymin] = Math.Max(alpha[x-xmin,y-ymin] - rate,0f);
						}
					}
				}
			}
			
			int max = 3;
			if(P.timeLeft > 30)
			{
				for(int i = 0; i < Main.maxProjectiles; i++)
				{
					if(checkOtherProj(P,Main.projectile[i]) && Main.projectile[i] != P)
					{
						if(P.ai[0] > max && Main.projectile[i].ai[0] == 1)
						{
							Main.projectile[i].timeLeft = 30;
						}
					}
				}
				
				bool flag = false;
				for(int i = 0; i < Main.maxProjectiles; i++)
				{
					if(checkOtherProj(P,Main.projectile[i]))
					{
						if(Main.projectile[i].ai[0] == 1)
						{
							flag = true;
							break;
						}
					}
				}
				if(!flag)
				{
					for(int i = 0; i < Main.maxProjectiles; i++)
					{
						if(checkOtherProj(P,Main.projectile[i]))
						{
							Main.projectile[i].ai[0]--;
						}
					}
				}
			}
		}
		
		bool checkOtherProj(Projectile P, Projectile otherProj)
		{
			return (otherProj.active && otherProj.timeLeft > 30 && otherProj.type == P.type && otherProj.owner == P.owner);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 50);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			
			SpriteEffects effects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			
			int xmin = (int)(P.Center.X - MaxRange) / 16;
			int xmax = (int)(P.Center.X + MaxRange) / 16;
			int ymin = (int)(P.Center.Y - MaxRange) / 16;
			int ymax = (int)(P.Center.Y + MaxRange) / 16;
			for(int x = xmin; x < xmax; x++)
			{
				for(int y = ymin; y < ymax; y++)
				{
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						Color tileColor = Lighting.GetColor(x,y);
						tileColor.B = (byte)Math.Max((int)tileColor.B,25);
						Color color = P.GetAlpha(tileColor);
						float alphaScale = MathHelper.Clamp(alpha[x-xmin,y-ymin],0f,1f);
						
						Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f);
						
						int num = 50;
						Rectangle screenRect = new Rectangle((int)(Main.screenPosition.X - (float)num), (int)(Main.screenPosition.Y - (float)num), Main.screenWidth + num * 2, Main.screenHeight + num * 2);
						Rectangle rect = new Rectangle((int)pos.X-23, (int)pos.Y-23, 56, 56);
						if(screenRect.Intersects(rect))
						{
							if(Vector2.Distance(pos,P.Center) <= range)
							{
								Vector2 pos2 = pos + addedPos[x-xmin,y-ymin];
								
								sb.Draw(tex, new Vector2((float)((int)(pos2.X - Main.screenPosition.X)), (float)((int)(pos2.Y - Main.screenPosition.Y))), 
								new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
								color*alphaScale, rotation[x-xmin,y-ymin], 
								new Vector2((float)tex.Width/2f, (float)tex.Height/2f), 
								P.scale*alphaScale, effects, 0f);
							}
							else if(Vector2.Distance(pos,P.Center) <= range+16)
							{
								float trot = (float)Math.Atan2((pos.Y - P.Center.Y), (pos.X - P.Center.X));
								Vector2 pos2 = P.Center + addedPos[x-xmin,y-ymin] + trot.ToRotationVector2()*range;
								Color color2 = color*alphaScale;
								Color color3 = color2*0.5f;
								color3.A = color2.A;
								
								sb.Draw(tex, new Vector2((float)((int)(pos2.X - Main.screenPosition.X)), (float)((int)(pos2.Y - Main.screenPosition.Y))), 
								new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
								color3, rotation[x-xmin,y-ymin], 
								new Vector2((float)tex.Width/2f, (float)tex.Height/2f), 
								P.scale*alphaScale, effects, 0f);
							}
						}
					}
				}
			}
			
			return false;
		}
	}
}
