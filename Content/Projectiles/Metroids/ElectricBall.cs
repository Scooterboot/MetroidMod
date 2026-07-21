using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Metroids
{
	public class ElectricBall : ModProjectile
	{
		public override void SetDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Projectile.width = Projectile.height = 20;
			Projectile.scale = 0.75f;
			Projectile.hostile = true;
			Projectile.friendly = false;

			Projectile.timeLeft = 300;
		}

		public override bool PreAI()
		{
			if (Projectile.velocity.Y < 6)
			{
				Projectile.velocity.Y += 0.2f;
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 4;
			}
			return false;
		}

		public override void OnKill(int timeLeft) //TODO: Make the projectile burst in a small explosion
		{
			for (int i = 0; i < 8; ++i)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4));
			}
		}
	}
}
