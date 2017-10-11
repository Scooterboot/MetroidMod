using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles
{
	public class PhazonExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon");
		}
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 0;
			projectile.timeLeft = 500;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile P = projectile;
			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + 1.57f;
			P.tileCollide = false;
			P.alpha = 255;
			P.localAI[0] += 1f;
			if (P.localAI[0] > 1f)
			{
				for (int l = 0; l < 4; l++)
				{
					float x = (P.position.X - P.velocity.X * ((float)l * 0.25f) + (P.width/2));
					float y = (P.position.Y - P.velocity.Y * ((float)l * 0.25f) + (P.height/2));
					int num20 = Dust.NewDust(new Vector2(x, y), 1, 1, 68, 0f, 0f, 100, default(Color), Main.rand.Next(3, 6));
					Main.dust[num20].position.X = x;
					Main.dust[num20].position.Y = y;
					Main.dust[num20].velocity *= 0.2f;
					Main.dust[num20].noGravity = true;
				}
			}
			if(P.localAI[0] < 60f)
			{
				P.velocity.X += (float)Main.rand.Next(-50,51) * 0.075f;
				P.velocity.Y += (float)Main.rand.Next(-50,51) * 0.075f;
			}
			else if(P.velocity.Length() < 16)
			{
				P.velocity *= 1.1f;
			}
		}
	}
}