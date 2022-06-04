using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.missilecombo
{
	public class SolarLaserFlameTrail : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Flame Trail");
			Main.projFrames[Projectile.type] = 9;
		}
		int maxTimeLeft = 60;
		static int width = 24;
		static int height = 36;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = width;
			Projectile.height = height;
			Projectile.scale = 0.5f;
			Projectile.timeLeft = maxTimeLeft*2;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
       	 	Projectile.localNPCHitCooldown = 10;
		}
		
		bool initialize = false;
		public override void AI()
		{
			Projectile P = Projectile;
			P.rotation = 0f;
			
			if(!initialize)
			{
				P.frame = Main.rand.Next(3);
				P.position.Y -= 2f*P.scale;
				initialize = true;
			}
			
			Color color = MetroidModPorted.plaRedColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			
			P.ai[0] += 1f;
			if(P.ai[0] > 3f)
			{
				float num297 = 0.7f + 0.3f * (P.scale - 1f);
				int num3;
				for(int num299 = 0; num299 < 1; num299 = num3 + 1)
				{
					int num300 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 6, P.velocity.X * 0.2f, P.velocity.Y * 0.2f, 100, default(Color), 1f);
					Dust dust3;
					if(Main.rand.Next(3) != 0)
					{
						Main.dust[num300].noGravity = true;
						dust3 = Main.dust[num300];
						dust3.scale *= 3f;
						Dust dust52 = Main.dust[num300];
						dust52.velocity.X = dust52.velocity.X * 2f;
						Dust dust53 = Main.dust[num300];
						dust53.velocity.Y = dust53.velocity.Y * 2f;
					}
					dust3 = Main.dust[num300];
					dust3.scale *= 1.5f;
					Dust dust54 = Main.dust[num300];
					dust54.velocity.X = dust54.velocity.X * 1.2f;
					Dust dust55 = Main.dust[num300];
					dust55.velocity.Y = dust55.velocity.Y * 1.2f;
					dust3 = Main.dust[num300];
					dust3.scale *= num297;
					num3 = num299;
				}
			}
			
			if(P.ai[0] <= maxTimeLeft)
			{
				P.scale += 2f / maxTimeLeft;
			}
			else
			{
				P.scale -= 2f / maxTimeLeft;
			}
			if(P.scale < 0.5f)
			{
				P.scale = 0.5f;
			}
			
			P.position.X += (float)P.width/2f;
			P.position.Y += (float)P.height;
			P.width = (int)((float)width * P.scale);
			P.height = (int)((float)height * P.scale);
			P.position.X -= (float)P.width/2f;
			P.position.Y -= (float)P.height;
			
			if(P.numUpdates <= 0)
			{
				P.frame++;
				if(P.frame >= 3)
				{
					P.frame = 0;
				}
			}
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.immune[Projectile.owner] = 4;
			target.AddBuff(24,600,true);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 100);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			if(P.ai[0] > 3f)
			{
				SpriteEffects effects = SpriteEffects.None;
				if (P.spriteDirection == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
				int num108 = tex.Height / Main.projFrames[P.type];
				int frame = P.frame;
				float scale = P.scale;
				if(P.scale >= 1.75f)
				{
					scale -= 1f;
					frame += 6;
				}
				else if(P.scale >= 1.25f)
				{
					scale -= 0.5f;
					frame += 3;
				}
				int y4 = num108 * frame;
				
				sb.Draw(tex, new Vector2((float)((int)(P.Center.X - Main.screenPosition.X)), (float)((int)(P.position.Y + P.height - Main.screenPosition.Y))), 
				new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), 
				P.GetAlpha(Color.White), 0f, 
				new Vector2((float)tex.Width/2f, (float)num108-2), 
				scale, effects, 0f);
			}
			return false;
		}
	}
}
