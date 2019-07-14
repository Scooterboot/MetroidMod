using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class IceSpreaderFrozenTerrain : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spreader Terrain");
		}
		
		public override void SetDefaults()
		{
			projectile.width = 56;
			projectile.height = 56;
			projectile.scale = 0.75f;
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.extraUpdates = 0;
		}
		
		static int range = 320;
		float[,] rotation = new float[range*2/16,range*2/16];
		float[,] alpha = new float[range*2/16,range*2/16];
		Vector2[,] addedPos = new Vector2[range*2/16,range*2/16];

		bool init = false;
		public override void AI()
		{
			Projectile P = projectile;
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
				if(Main.rand.Next(2) == 0)
				{
					P.spriteDirection = -1;
				}
				init = true;
			}
			
			int xmin = (int)(P.Center.X - range) / 16;
			int xmax = (int)(P.Center.X + range) / 16;
			int ymin = (int)(P.Center.Y - range) / 16;
			int ymax = (int)(P.Center.Y + range) / 16;
			for(int x = xmin; x < xmax; x++)
			{
				for(int y = ymin; y < ymax; y++)
				{
					Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f);
					if (Main.tile[x, y] != null && Main.tile[x, y].active())
					{
						if(Vector2.Distance(pos,P.Center) <= range)
						{
							Rectangle projRect = new Rectangle((int)pos.X-P.width/2,(int)pos.Y-P.height/2,P.width,P.height);
							for(int i = 0; i < Main.maxNPCs; i++)
							{
								if(Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].dontTakeDamage)
								{
									NPC npc = Main.npc[i];
									Rectangle npcRect = new Rectangle((int)npc.position.X,(int)npc.position.Y,npc.width,npc.height);
									if(projRect.Intersects(npcRect))
									{
										npc.AddBuff(mod.BuffType("InstantFreeze"),600,true);
									}
								}
							}
						}
					}
					float rate = 0.05f + (0.05f * (1f - Vector2.Distance(pos,P.Center)/range));
					if(P.timeLeft > 20)
					{
						alpha[x-xmin,y-ymin] = Math.Min(alpha[x-xmin,y-ymin] + rate, 1f);
					}
					else
					{
						alpha[x-xmin,y-ymin] = Math.Max(alpha[x-xmin,y-ymin] - rate,0f);
					}
				}
			}
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			target.AddBuff(mod.BuffType("InstantFreeze"),600,true);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Projectile P = projectile;
			
			SpriteEffects effects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[P.type];
			
			int xmin = (int)(P.Center.X - range) / 16;
			int xmax = (int)(P.Center.X + range) / 16;
			int ymin = (int)(P.Center.Y - range) / 16;
			int ymax = (int)(P.Center.Y + range) / 16;
			for(int x = xmin; x < xmax; x++)
			{
				for(int y = ymin; y < ymax; y++)
				{
					if (Main.tile[x, y] != null && Main.tile[x, y].active())
					{
						Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f) + addedPos[x-xmin,y-ymin];
						if(Vector2.Distance(pos,P.Center) <= range)
						{
							Color tileColor = Lighting.GetColor(x,y);
							Color color = P.GetAlpha(tileColor);
							float alphaScale = alpha[x-xmin,y-ymin];
							
							sb.Draw(tex, new Vector2((float)((int)(pos.X - Main.screenPosition.X)), (float)((int)(pos.Y - Main.screenPosition.Y))), 
							new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
							color*alphaScale, rotation[x-xmin,y-ymin], 
							new Vector2((float)tex.Width/2f, (float)tex.Height/2f), 
							P.scale*alphaScale, effects, 0f);
						}
					}
				}
			}
			return false;
		}
	}
}