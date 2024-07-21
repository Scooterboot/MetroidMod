using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class PhazonExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon");
		}
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile P = Projectile;
			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + MathHelper.PiOver2;
			P.tileCollide = false;
			P.alpha = 255;
			P.localAI[0] += 1f;
			if (P.localAI[0] > 1f)
			{
				for (int l = 0; l < 4; l++)
				{
					float x = (P.position.X - P.velocity.X * ((float)l * 0.25f) + (P.width / 2));
					float y = (P.position.Y - P.velocity.Y * ((float)l * 0.25f) + (P.height / 2));
					int num20 = Dust.NewDust(new Vector2(x, y), 1, 1, 68, 0f, 0f, 100, default(Color), Main.rand.Next(3, 6));
					Main.dust[num20].position.X = x;
					Main.dust[num20].position.Y = y;
					Main.dust[num20].velocity *= 0.2f;
					Main.dust[num20].noGravity = true;
				}
			}
			if (P.localAI[0] < 60f)
			{
				P.velocity.X += (float)Main.rand.Next(-50, 51) * 0.075f;
				P.velocity.Y += (float)Main.rand.Next(-50, 51) * 0.075f;
			}
			else if (P.velocity.Length() < 16)
			{
				P.velocity *= 1.1f;
			}
		}
	}
}
