using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamgreen
{
	public class PlasmaBeamGreenChargeShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam Green Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = 9;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 12;
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
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			if (S.Contains("wave"))
			{
				Projectile.Name += "Wave";
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (!S.Contains("spazer") && S.Contains("wave"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
			}
			if (S.Contains("spazer") && !S.Contains("wave"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
				mProjectile.wavesPerSecond = 2f;
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (S.Contains("spazer") && S.Contains("wave"))
			{
				mProjectile.amplitude = 14.5f * Projectile.scale;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	
	public class IcePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Green Charge Shot";
		}
	}
	
}
