using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Mobs
{
	public class MetareeRock : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 6;
			Projectile.scale = 2;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;

			Projectile.numHits = 1;
		}

		public override bool PreAI()
		{
			if (Projectile.ai[0]++ >= 20) // Hacky, god help me.
				Projectile.tileCollide = true;

			Projectile.rotation += (Math.Abs(Projectile.velocity.Y) + Math.Abs(Projectile.velocity.X)) * (Projectile.velocity.X > 0f ? 0.02F : -0.02F);
			Projectile.velocity.Y += 0.2F;
			Projectile.velocity.X *= 0.98F;

			return false;
		}

		public override void Kill(int timeLeft)
		{
			int num4;
			for (int num471 = 0; num471 < 30; num471 = num4 + 1)
			{
				int num472 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 1);
				if (Main.rand.NextBool(2))
				{
					Dust dust = Main.dust[num472];
					dust.scale *= 1.4f;
				}
				num4 = num471;
			}
		}
	}
}
