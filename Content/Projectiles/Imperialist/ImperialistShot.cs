using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Content;

namespace MetroidMod.Content.Projectiles.Imperialist
{
	public class ImperialistShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Imperialist Shot");
			Main.projFrames[Projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 32;
			Projectile.scale = 1.5f;
			Projectile.extraUpdates = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
			mProjectile.wavesPerSecond = 1f;
			if (Items.Weapons.PowerBeam.shooty.Contains("green"))
			{
				Projectile.penetrate = 6;
			}
			if (Items.Weapons.PowerBeam.shooty.Contains("nova"))
			{
				Projectile.penetrate = 8;
			}
			if (Items.Weapons.PowerBeam.shooty.Contains("solar"))
			{
				Projectile.penetrate = 12;
			}
		}

		public override void AI()
		{
			if (Items.Weapons.PowerBeam.shooty.Contains("spaze"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
				mProjectile.amplitude = 5f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 1;
			}
			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			if (Items.Weapons.PowerBeam.shooty.Contains("wave"))
			{
				Projectile.tileCollide = false;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			int dustType = ModContent.DustType<Content.Dusts.ImperialistDust>();
			Main.dust[dustType].noGravity = true;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 15, dustType, 2f);
		}
		/*public override void Kill(int timeLeft)
		{
			int dustType = 64;
		}*/

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}

