using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Mobs
{
	public class CacatacSpike : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 4;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.scale = 1.5F;

			Projectile.numHits = 1;
			Projectile.timeLeft = 60;
		}

		public override bool PreAI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);

			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 8; ++i)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GrassBlades, Projectile.velocity.X, Projectile.velocity.Y);
			}
		}
	}
}
