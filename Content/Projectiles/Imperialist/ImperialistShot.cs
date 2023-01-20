using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.Imperialist
{
	public class ImperialistShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperialist Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 2f;

		}

		public override void AI()
		{
			if (Projectile.Name.Contains("Wave"))
			{
				Projectile.tileCollide = false;
			}
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
			if (Projectile.Name.Contains("Spazer"))
			{
				mProjectile.amplitude = 5f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			int dustType = 271;
			int shootSpeed = 80;
			int distance = 0;
			int accuracy = 0;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed, distance, accuracy);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 271, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
			if (Projectile.Name.Contains("Spazer"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
		}

		public override void Kill(int timeLeft)
		{
			int dustType = 64;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.Name.Contains("Ice"))
			{
				target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300, true);
			}
		}
	}
    public class SpazerImperialistShot : ImperialistShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer Imperialist Shot";
        }
    }
	public class WaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Imperialist Shot";
		}
	}
	public class IceImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Imperialist Shot";
		}
	}
	public class IceWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Imperialist Shot";
		}
	}
	public class SpazerWaveIceImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Wave Ice Imperialist Shot";
		}
	}
	public class PlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Imperialist Shot";
		}
	}
	public class PlasmaGreenSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Spazer Imperialist Shot";
		}
	}
	public class PlasmaGreenSpazerWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Spazer Wave Imperialist Shot";
		}
	}
	public class PlasmaGreenWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Wave Imperialist Shot";
		}
	}
	public class PlasmaGreenIceWaveSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Wave Spazer Imperialist Shot";
		}
	}
	public class PlasmaGreenIceWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Wave Imperialist Shot";
		}
	}
	public class PlasmaGreenIceSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Spazer Imperialist Shot";
		}
	}
	public class PlasmaGreenIceImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Imperialist Shot";
		}
	}
	public class PlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Imperialist Shot";
		}
	}
	public class PlasmaRedSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Spazer Imperialist Shot";
		}
	}
	public class PlasmaRedSpazerWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Spazer Wave Imperialist Shot";
		}
	}
	public class PlasmaRedWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave Imperialist Shot";
		}
	}
	public class PlasmaRedIceWaveSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave Spazer Imperialist Shot";
		}
	}
	public class PlasmaRedIceWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave Imperialist Shot";
		}
	}
	public class PlasmaRedIceSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Spazer Imperialist Shot";
		}
	}
	public class PlasmaRedIceImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Imperialist Shot";
		}
	}
	public class NovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Imperialist Shot";
		}
	}
	public class NovaSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Spazer Imperialist Shot";
		}
	}
	public class NovaSpazerWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Spazer Wave Imperialist Shot";
		}
	}
	public class NovaWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Wave Imperialist Shot";
		}
	}
	public class NovaIceWaveSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Ice Wave Spazer Imperialist Shot";
		}
	}
	public class NovaIceWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Ice Wave Imperialist Shot";
		}
	}
	public class NovaIceSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Ice Spazer Imperialist Shot";
		}
	}
	public class NovaIceImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Ice Imperialist Shot";
		}
	}
	public class SolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Imperialist Shot";
		}
	}
	public class SolarSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Spazer Imperialist Shot";
		}
	}
	public class SolarSpazerWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Spazer Wave Imperialist Shot";
		}
	}
	public class SolarWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Wave Imperialist Shot";
		}
	}
	public class SolarIceWaveSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Ice Wave Spazer Imperialist Shot";
		}
	}
	public class SolarIceWaveImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Ice Wave Imperialist Shot";
		}
	}
	public class SolarIceSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Ice Spazer Imperialist Shot";
		}
	}
	public class SolarIceImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Ice Imperialist Shot";
		}
	}
}
