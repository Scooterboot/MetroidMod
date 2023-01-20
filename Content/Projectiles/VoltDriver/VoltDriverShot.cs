using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.VoltDriver
{
	public class VoltDriverShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Volt Driver Shot");
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.scale = 0.75f;
			Projectile.damage = 15;
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
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);

            if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
            Projectile.frame++;
			if(Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
            if (Projectile.Name.Contains("Spazer"))
            {
                mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
            }
        }
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 269);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverImpactSound, Projectile.position);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
	public class PlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green VoltDriver Shot";
		}
	}
	public class PlasmaGreenWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Wave VoltDriver Shot";
		}
	}
	public class PlasmaGreenIceVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice VoltDriver Shot";
		}
	}
	public class PlasmaGreenIceWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Ice Wave VoltDriver Shot";
		}
	}
	public class WaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave VoltDriver Shot";
		}
	}
	public class IceVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice VoltDriver Shot";
		}
	}
	public class IceWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave VoltDriver Shot";
		}
	}
	public class PlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Shot";
		}
	}
	public class PlasmaRedWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave VoltDriver Shot";
		}
	}
	public class PlasmaRedIceVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice VoltDriver Shot";
		}
	}
	public class PlasmaRedIceWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave VoltDriver Shot";
		}
	}
	public class NovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Shot";
		}
	}
	public class NovaWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave VoltDriver Shot";
		}
	}
	public class NovaIceVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice VoltDriver Shot";
		}
	}
	public class NovaIceWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave VoltDriver Shot";
		}
	}
	public class SolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Shot";
		}
	}
	public class SolarWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Wave VoltDriver Shot";
		}
	}
	public class SolarIceVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice VoltDriver Shot";
		}
	}
	public class SolarIceWaveVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Ice Wave VoltDriver Shot";
		}
	}
}
