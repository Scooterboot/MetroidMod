using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamgreen
{
	public class PlasmaBeamGreenShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam Green Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.scale = 2f;
			Projectile.penetrate = 6;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 6;
		}

		int dustType = 61;
		Color color = MetroidMod.plaGreenColor;
		public override void AI()
		{
			
			string S  = PowerBeam.SetCondition();
			if (S.Contains("ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if (S.Contains("wave"))
			{
				Projectile.Name += "Wave";
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (!S.Contains("spazer") && S.Contains("wave"))
			{
				mProjectile.amplitude = 8f * Projectile.scale;
			}
			if (S.Contains("spazer") && !S.Contains("wave"))
			{
				mProjectile.amplitude = 7.5f * Projectile.scale;
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (S.Contains("spazer") && S.Contains("wave"))
			{
				mProjectile.amplitude = 12f * Projectile.scale;
			}

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	public class IcePlasmaBeamGreenShot : PlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Green Shot";
		}
	}
}
