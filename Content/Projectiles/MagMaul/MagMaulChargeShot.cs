using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.MagMaul
{
	public class MagMaulChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MagMaul Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1.5f;
			Projectile.aiStyle = 1;
		}

		public override void AI()
		{
			if (Projectile.Name.Contains("Green"))
			{
				Projectile.penetrate = 6;
			}
			if (Projectile.Name.Contains("Nova"))
			{
				Projectile.penetrate = 8;
			}
			if (Projectile.Name.Contains("Solar"))
			{
				Projectile.penetrate = 12;
			}
			//annoyingly, "if (Projectile.Name.Contains("Spazer"))" doesnt work for charge shots' amplitutde
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
			if (Projectile.Name.Contains("Spazer"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
		}
		public override void Kill(int timeLeft)
		{
			int dustType = 286;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 250;
			Projectile.height += 250;
			Projectile.scale = 5f;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			mProjectile.Diffuse(Projectile, 286);
			SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulExplode, Projectile.position);
			Projectile.Damage();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(24, 600);
		}
		public class SpazerMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Spazer MagMaul Charge Shot";

				mProjectile.amplitude = 15f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 4;
			}
		}
		public class NovaMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Nova MagMaul Charge Shot";
			}
		}
		public class SolarMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Solar MagMaul Charge Shot";
			}
		}
		public class PlasmaGreenMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Plasma Green MagMaul Charge Shot";
			}
		}
		public class NovaSpazerMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Nova Spazer MagMaul Charge Shot";
				mProjectile.amplitude = 15f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 4;
			}
		}
		public class SolarSpazerMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Solar Spazer MagMaul Charge Shot";
				mProjectile.amplitude = 15f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 4;
			}
		}
		public class PlasmaGreenSpazerMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Plasma Spazer MagMaul Charge Shot";
				mProjectile.amplitude = 15f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 4;
			}
		}
		public class PlasmaRedSpazerMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Plasma Red Spazer MagMaul Charge Shot";
				mProjectile.amplitude = 15f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 4;
			}
		}
		public class PlasmaRedMagMaulChargeShot : MagMaulChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Plasma Red MagMaul Charge Shot";
			}
		}
	}
}
