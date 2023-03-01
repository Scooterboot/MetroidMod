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
			int shootSpeed = 2;
			if (Projectile.Name.Contains("Wave") || Projectile.Name.Contains("Nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
				mProjectile.HomingBehavior(Projectile, shootSpeed);
			}
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
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, dustType, 2f);
			mProjectile.HomingBehavior(Projectile, shootSpeed);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			int dustType = 269;
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width += 125;
			Projectile.height += 125;
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
			if (Projectile.Name.Contains("Red"))
			{
				if (Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44, 300);
				}
				else
				{
					target.AddBuff(24, 300);
				}
			}
			if (Projectile.Name.Contains("Nova"))
			{
				if (Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44, 300);
				}
				else
				{
					target.AddBuff(39, 300);
				}
			}
			if (Projectile.Name.Contains("Ice") || Projectile.Name.Contains("Stardust"))
			{
				string buffName = "IceFreeze";
				target.AddBuff(Mod.Find<ModBuff>(buffName).Type, 300);
			}

			if (Projectile.Name.Contains("Solar"))
			{
				target.AddBuff(189, 300);
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverDaze, Projectile.position);
			target.AddBuff(31, 180);
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
	public class IceWaveSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer VoltDriver Charge Shot";
		}
	}
	public class IceWaveSpazerPlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWaveSpazerPlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Red VoltDriver Charge Shot";
		}
	}
	public class IceWavePlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWavePlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red VoltDriver Charge Shot";
		}
	}
	public class IceSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer VoltDriver Charge Shot";
		}
	}
	public class IceSpazerPlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceSpazerPlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Red VoltDriver Charge Shot";
		}
	}
	public class IcePlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IcePlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red VoltDriver Charge Shot";
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
	public class WaveSpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer VoltDriver Charge Shot";
		}
	}
	public class WaveSpazerPlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WaveSpazerPlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Red VoltDriver Charge Shot";
		}
	}
	public class WavePlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WavePlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red VoltDriver Charge Shot";
		}
	}
	public class SpazerVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer VoltDriver Charge Shot";
		}
	}
	public class SpazerPlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Green VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class SpazerPlasmaRedVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Red VoltDriver Charge Shot";
		}
	}
	public class PlasmaGreenVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice V2 VoltDriver Charge Shot";
		}
	}
	public class IceWaveV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave V2 VoltDriver Charge Shot";
		}
	}
	public class IceWaveWideVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide VoltDriver Charge Shot";
		}
	}
	public class IceWaveWideNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceWaveWidePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWaveWidePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class IceWaveNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceWavePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWavePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class IceWideVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide VoltDriver Charge Shot";
		}
	}
	public class IceWideNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceWidePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWidePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class IceNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IcePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IcePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class WaveWideVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide VoltDriver Charge Shot";
		}
	}
	public class WaveV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave V2 VoltDriver Charge Shot";
		}
	}
	public class WaveWideNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WaveWidePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WaveWidePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class WaveNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WavePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WavePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class WideVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide VoltDriver Charge Shot";
		}
	}
	public class WideNovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WidePlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WidePlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class NovaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova VoltDriver Charge Shot";
			Projectile.penetrate = 8;
		}
	}
	public class PlasmaGreenV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green V2 VoltDriver Charge Shot";
			Projectile.penetrate = 6;
		}
	}
	public class PlasmaRedV2VoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red V2 VoltDriver Charge Shot";
		}
	}
	public class StardustVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust VoltDriver Charge Shot";
		}
	}
	public class StardustNebulaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula VoltDriver Charge Shot";
		}
	}
	public class StardustNebulaVortexVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex VoltDriver Charge Shot";
		}
	}
	public class StardustNebulaVortexSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustNebulaSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustVortexVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex VoltDriver Charge Shot";
		}
	}
	public class StardustVortexSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class NebulaVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula VoltDriver Charge Shot";
		}
	}
	public class NebulaVortexVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex VoltDriver Charge Shot";
		}
	}
	public class NebulaVortexSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class NebulaSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class VortexVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex VoltDriver Charge Shot";
		}
	}
	public class VortexSolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
	public class SolarVoltDriverChargeShot : VoltDriverChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar VoltDriver Charge Shot";
			Projectile.penetrate = 12;
		}
	}
}
