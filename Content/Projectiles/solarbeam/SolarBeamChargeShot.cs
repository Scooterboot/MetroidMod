using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.solarbeam
{
	public class SolarBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Beam Charge Shot");
			Main.projFrames[Type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.scale = 2f;
			Projectile.penetrate = 16;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			mProjectile.wavesPerSecond = 1.5f;
			mProjectile.delay = 4;
		}

		int dustType = 6;
		Color color = MetroidMod.novColor;
		public override void AI()
		{


			if (Projectile.Name.Contains("Stardust"))
			{
				dustType = 87;
			}
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}

			if (shot.Contains("nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
				mProjectile.HomingBehavior(Projectile);
				//mProjectile.amplitude = 8f * Projectile.scale;
			}
			if (!shot.Contains("vortex") && shot.Contains("nebula"))
			{
				mProjectile.amplitude = 12f * Projectile.scale;
				mProjectile.wavesPerSecond = 2f;
			}
			if (shot.Contains("vortex") && !shot.Contains("nebula"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
				mProjectile.wavesPerSecond = 2f;
				mProjectile.WaveBehavior(Projectile, !shot.Contains("nebula"));
			}
			if (shot.Contains("vortex") && shot.Contains("nebula"))
			{
				mProjectile.amplitude = 16f * Projectile.scale;
				mProjectile.wavesPerSecond = 1.5f;
			}

			int dType = Utils.SelectRandom<int>(Main.rand, new int[] { 6, 158 });
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dType, 0, 0, 100, default(Color), Projectile.scale * 2);
			Main.dust[dust].noGravity = true;
			if (shot.Contains("stardust"))
			{
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}

			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if (Vector2.Distance(Projectile.position, Projectile.position + velocity) < Vector2.Distance(Projectile.position, Projectile.position + Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;
		}

		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 50);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}

	public class VortexSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar Beam Charge Shot";
		}
	}
}
