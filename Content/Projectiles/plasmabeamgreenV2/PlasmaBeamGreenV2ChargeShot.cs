using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamgreenV2
{
	public class PlasmaBeamGreenV2ChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam Green V2 Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			Projectile.penetrate = 9;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}

		int dustType = 61;
		Color color = MetroidMod.plaGreenColor;
		public override void AI()
		{


			if (shot.Contains("ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			else if (shot.Contains("wave"))
			{
				dustType = 15;
				color = MetroidMod.plaGreenColor2;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Main.projFrames[Projectile.type] > 1)
			{
				if (Projectile.numUpdates == 0)
				{
					Projectile.frame++;
				}
				if (Projectile.frame > 1)
				{
					Projectile.frame = 0;
				}
			}

			if (shot.Contains("wave"))
			{
				Projectile.tileCollide = false;
			}
			if (shot.Contains("wide") || (shot.Contains("wave")))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (shot.Contains("wide") && !shot.Contains("wave"))
			{
				mProjectile.amplitude = 14f * Projectile.scale;
			}
			if (shot.Contains("wave") && !shot.Contains("wide"))
			{
				mProjectile.amplitude = 12f * Projectile.scale;
			}
			if (shot.Contains("wave") && shot.Contains("wide"))
			{
				mProjectile.amplitude = 16f * Projectile.scale;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}

	public class WidePlasmaBeamGreenV2ChargeShot : PlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Beam Green V2 Charge Shot";
		}
	}

	public class WavePlasmaBeamGreenV2ChargeShot : PlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Green V2 Charge Shot";
			Projectile.tileCollide = false;

			mProjectile.amplitude = 12f * Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}
	}

	public class IcePlasmaBeamGreenV2ChargeShot : PlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Green V2 Charge Shot";
		}
	}

	public class IceWidePlasmaBeamGreenV2ChargeShot : WidePlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Beam Green V2 Charge Shot";
		}
	}

	public class IceWavePlasmaBeamGreenV2ChargeShot : WavePlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Green V2 Charge Shot";
			mProjectile.delay = 3;
		}
	}

	public class IceWaveWidePlasmaBeamGreenV2ChargeShot : WavePlasmaBeamGreenV2ChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamredV2/IceWaveWidePlasmaBeamRedV2ChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Beam Green V2 Charge Shot";
			Main.projFrames[Projectile.type] = 1;
			mProjectile.delay = 3;
		}
	}
}
