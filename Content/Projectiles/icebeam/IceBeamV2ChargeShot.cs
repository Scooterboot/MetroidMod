using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace MetroidMod.Content.Projectiles.icebeam
{
	public class IceBeamV2ChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Beam V2 Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 15;
			Projectile.height = 15;
			Projectile.scale = 2.25f;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.iceColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, 59);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 3);
			return false;
		}
	}
}
