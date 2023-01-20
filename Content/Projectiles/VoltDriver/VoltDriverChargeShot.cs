using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.VoltDriver
{
	public class VoltDriverChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Volt Driver Charge Shot");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;

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
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
            if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f*Projectile.direction;
				Projectile.frame++;
			}
			if(Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
			int dustType = 269;
			int shootSpeed = 2;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
            if (Projectile.Name.Contains("Spazer"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }

		public override void Kill(int timeLeft)
		{
			int dustType = 269;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 250;
			Projectile.height += 250;
			Projectile.scale = 5f;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			mProjectile.Diffuse(Projectile, 269);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverChargeImpactSound, Projectile.position);
			Projectile.Damage();
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverChargeImpactSound, Projectile.position);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverDaze, Projectile.position);
			target.AddBuff (31, 180);
		}
	}
    public class SpazerVoltDriverChargeShot : VoltDriverChargeShot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Name = "Spazer Volt Driver Charge Shot";

            mProjectile.amplitude = 25f * Projectile.scale;
            mProjectile.wavesPerSecond = 1f;
            mProjectile.delay = 4;
        }
    }
	public class PlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green VoltDriver Charge Shot";
		}
	}
	public class PlasmaGreenWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Wave VoltDriver Charge Shot";
		}
	}
	public class PlasmaGreenIceVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice VoltDriver Charge Shot";
		}
	}
	public class PlasmaGreenIceWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Wave VoltDriver Charge Shot";
		}
	}
	public class WaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave VoltDriver Charge Shot";
		}
	}
	public class IceVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice VoltDriver Charge Shot";
		}
	}
	public class IceWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave VoltDriver Charge Shot";
		}
	}
	public class PlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Charge Shot";
		}
	}
	public class PlasmaRedWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave VoltDriver Charge Shot";
		}
	}
	public class PlasmaRedIceVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice VoltDriver Charge Shot";
		}
	}
	public class PlasmaRedIceWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave VoltDriver Charge Shot";
		}
	}
	public class NovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Charge Shot";
		}
	}
	public class NovaWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave VoltDriver Charge Shot";
		}
	}
	public class NovaIceVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice VoltDriver Charge Shot";
		}
	}
	public class NovaIceWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave VoltDriver Charge Shot";
		}
	}
	public class SolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Charge Shot";
		}
	}
	public class SolarWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave VoltDriver Charge Shot";
		}
	}
	public class SolarIceVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice VoltDriver Charge Shot";
		}
	}
	public class SolarIceWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave VoltDriver Charge Shot";
		}
	}
	public class PlasmaGreenSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaGreenSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaGreenIceSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaGreenIceSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class SpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class IceSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class IceWaveSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaRedSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaRedSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaRedSpazerIceVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Spazer Ice VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class PlasmaRedSpazerIceWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Spazer Ice Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class NovaSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class NovaSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class NovaIceSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Ice Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class NovaIceSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Ice Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class SolarSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class SolarSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class SolarIceSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Ice Spazer VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	public class SolarIceSpazerWaveVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Ice Spazer Wave VoltDriver Charge Shot";
			mProjectile.amplitude = 25f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
}
