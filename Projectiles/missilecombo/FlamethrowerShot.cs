using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class FlamethrowerShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flamethrower Shot");
		}
		int maxTimeLeft = 50;
		int size = 32;
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = size;
			projectile.height = size;
			projectile.scale = 0.75f;
			projectile.timeLeft = maxTimeLeft;
			//projectile.extraUpdates = 4;
		}

		bool initialize = false;
		public override void AI()
		{
			Projectile P = projectile;
			
			if(!initialize)
			{
				P.rotation = (float)Main.rand.Next(360) * ((float)Math.PI / 180);
				initialize = true;
			}
			
			Color color = MetroidMod.plaRedColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if (P.ai[0] > 3f)
			{
				float num297 = 1f;
				if (P.ai[0] <= 16f)
				{
					num297 = 0.7f;
				}
				else if (P.ai[0] <= 28f)
				{
					num297 = 0.8f;
				}
				else if (P.ai[0] <= 40f)
				{
					num297 = 0.9f;
				}
				P.ai[0] += 1f;
				int num3;
				for (int num299 = 0; num299 < 1; num299 = num3 + 1)
				{
					int num300 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 6, P.velocity.X * 0.2f, P.velocity.Y * 0.2f, 100, default(Color), 1f);
					Dust dust3;
					if (Main.rand.Next(3) != 0)
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
			else
			{
				P.ai[0] += 1f;
			}
			
			P.scale += 0.5f / maxTimeLeft;
			
			P.position.X += (float)P.width/2f;
			P.position.Y += (float)P.height/2f;
			P.width = (int)((float)size * P.scale);
			P.height = (int)((float)size * P.scale);
			P.position.X -= (float)P.width/2f;
			P.position.Y -= (float)P.height/2f;
			
			//P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + 1.57f;
			if(P.numUpdates <= 0)
			{
				P.rotation += 0.25f;
			}
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			target.AddBuff(24,600,true);
		}

		/*public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, 6);
		}*/
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			//mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb);//, 4);
			mProjectile.DrawCentered(projectile, sb);
			return false;
		}
	}
}