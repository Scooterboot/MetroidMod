using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.bombs
{
	public class SolarBombExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Bomb");
		}
		public override void SetDefaults()
		{
			projectile.width = 640;
			projectile.height = 640;
			projectile.scale = 0.01f;
			projectile.aiStyle = -1;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 1;
		}
		float scale = 0.01f;
		float speed = 4f;
		const int maxDist = 74;
		float vacAlpha = 0f;
		
		const int width = 640;
		const int height = 640;
		public override void AI()
        {
            Projectile P = projectile;
			
			P.ai[0]++;
			if(P.ai[0] > maxDist/4)
			{
				speed *= 0.955f;
			}
			if(P.ai[0] > maxDist)
			{
				if(vacAlpha < 1f)
				{
					float vacScale = 6f * (1f - vacAlpha);
					int vacW = (int)((float)width*vacScale);
					int vacH = (int)((float)height*vacScale);
					Rectangle vacRect = new Rectangle((int)(P.Center.X - vacW/2),(int)(P.Center.Y - vacH/2),vacW,vacH);
					for(int i = 0; i < Main.item.Length; i++)
					{
						if (!Main.item[i].active) continue;

						Item I = Main.item[i];
						if(vacRect.Intersects(I.Hitbox))
						{
							Vector2 center = new Vector2(P.Center.X,P.Center.Y-((float)I.height/2f));
							Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(20f,Vector2.Distance(center,I.Center));
							if(Vector2.Distance(center,I.Center) > 1f)
							{
								I.position += velocity;
								I.velocity *= 0f;
							}
						}
					}
					
					vacAlpha += 1f / maxDist;
				}
				else
				{
					P.damage = 0;
				}
			}
			else
			{
				float dScale = Math.Min(10f * (1f - P.ai[0]/maxDist), 4f);
				int num = (int)(100f*dScale);
				for(int i = 0; i < num; i++)
				{
					int dType = Utils.SelectRandom<int>(Main.rand, new int[]
					{
						6,
						259,
						158
					});
					
					float angle = (float)((Math.PI*2)/num)*i;
					Vector2 position = P.Center - new Vector2(20,20);
					position.X += (float)Math.Cos(angle)*((float)P.width/2f - 16f*P.scale);
					position.Y += (float)Math.Sin(angle)*((float)P.height/2f - 16f*P.scale);
					int num20 = Dust.NewDust(position, 40, 40, dType, 0f, 0f, 100, default(Color), 1f);
					Dust dust = Main.dust[num20];
					dust.velocity += Vector2.Normalize(P.Center - dust.position) * 2f;// * 5f * P.scale;
					dust.alpha = 200;
					dust.scale += Main.rand.NextFloat();
					dust.noGravity = true;
				}
			}
			scale += 0.04f*speed;

			P.scale = scale;
			P.position.X += (float)(P.width / 2);
			P.position.Y += (float)(P.height / 2);
			P.width = (int)((float)width * P.scale);
			P.height = (int)((float)height * P.scale);
			P.position.X -= (float)(P.width / 2);
			P.position.Y -= (float)(P.height / 2);
			
			if(P.alpha < 255)
			{
				P.alpha++;
				P.timeLeft = 2;
			}

			if (P.ai[0] == maxDist)
			{
				Rectangle tileRect = new Rectangle((int)(P.position.X / 16), (int)(P.position.Y / 16), (P.width / 16), (P.height / 16));
                for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
                {
                    for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
                    {
                        if (Main.tile[x, y] != null && Main.tile[x, y].active())
                        {
                            if (Main.tile[x, y].type == (ushort)mod.TileType("YellowHatch"))
                                TileLoader.HitWire(x, y, mod.TileType("YellowHatch"));
                            if (Main.tile[x, y].type == (ushort)mod.TileType("YellowHatchVertical"))
                                TileLoader.HitWire(x, y, mod.TileType("YellowHatchVertical"));
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                                TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                                TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                        }
                    }
                }
			}
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 255 - projectile.alpha);
		}

        public override void ModifyHitNPC(NPC npc, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.defense < 1000)
			{
                damage = (int)(damage + npc.defense * 0.5);
			}
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			target.AddBuff(189,300,true);
		}

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			
			
			Projectile P = projectile;
			Texture2D tex = Main.projectileTexture[P.type];
			
			sb.Draw(tex, P.Center - Main.screenPosition, 
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
			P.GetAlpha(Color.White), P.rotation, new Vector2(tex.Width, tex.Height), P.scale, SpriteEffects.None, 0f);
			
			sb.Draw(tex, P.Center - Main.screenPosition, 
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
			P.GetAlpha(Color.White), P.rotation, new Vector2(0, tex.Height), P.scale, SpriteEffects.FlipHorizontally, 0f);
			
			sb.Draw(tex, P.Center - Main.screenPosition, 
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
			P.GetAlpha(Color.White), P.rotation, new Vector2(tex.Width, 0), P.scale, SpriteEffects.FlipVertically, 0f);
			
			sb.Draw(tex, P.Center - Main.screenPosition, 
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
			P.GetAlpha(Color.White), P.rotation, Vector2.Zero, P.scale, SpriteEffects.FlipHorizontally ^ SpriteEffects.FlipVertically, 0f);
			
			if(vacAlpha < 1f)
			{
				Texture2D tex2 = mod.GetTexture("Projectiles/bombs/SolarBombVacuum");
				float vacScale = 6f * (1f - vacAlpha);
				Color color = Color.White * (1f - vacAlpha*0.5f);
				color.A = (byte)(255f*vacAlpha);
				
				sb.Draw(tex2, P.Center - Main.screenPosition, 
				new Rectangle?(new Rectangle(0, 0, tex2.Width, tex2.Height)), 
				color, P.rotation, new Vector2(tex2.Width/2, tex2.Height/2), vacScale, SpriteEffects.None, 0f);
			}
			
			
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}