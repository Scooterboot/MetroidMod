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
			// DisplayName.SetDefault("Volt Driver Shot");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.scale = 0.75f;
			Projectile.damage = 15;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (Projectile.Name.Contains("Wave"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
			}
			if (Projectile.Name.Contains("Nebula"))
			{
				Projectile.tileCollide = false;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
			Projectile.frame++;
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
	public class IceWaveSpazerVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer VoltDriver Shot";
		}
	}
	public class IceWaveSpazerPlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWaveSpazerPlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Red VoltDriver Shot";
		}
	}
	public class IceWavePlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWavePlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red VoltDriver Shot";
		}
	}
	public class IceSpazerVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer VoltDriver Shot";
		}
	}
	public class IceSpazerPlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceSpazerPlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Red VoltDriver Shot";
		}
	}
	public class IcePlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IcePlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red VoltDriver Shot";
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
	public class WaveSpazerVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer VoltDriver Shot";
		}
	}
	public class WaveSpazerPlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WaveSpazerPlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Red VoltDriver Shot";
		}
	}
	public class WavePlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WavePlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red VoltDriver Shot";
		}
	}
	public class SpazerVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer VoltDriver Shot";
		}
	}
	public class SpazerPlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class SpazerPlasmaRedVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Red VoltDriver Shot";
		}
	}
	public class PlasmaGreenVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green VoltDriver Shot";
			Projectile.penetrate = 6;
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
	public class IceV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice V2 VoltDriver Shot";
		}
	}
	public class IceWaveV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave V2 VoltDriver Shot";
		}
	}
	public class IceWaveWideVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide VoltDriver Shot";
		}
	}
	public class IceWaveWideNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Nova VoltDriver Shot";
		}
	}
	public class IceWaveWidePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWaveWidePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Red V2 VoltDriver Shot";
		}
	}
	public class IceWaveNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceWavePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWavePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red V2 VoltDriver Shot";
		}
	}
	public class IceWideVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide VoltDriver Shot";
		}
	}
	public class IceWideNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceWidePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IceWidePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Red V2 VoltDriver Shot";
		}
	}
	public class IceNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IcePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class IcePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red V2 VoltDriver Shot";
		}
	}
	public class WaveWideVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide VoltDriver Shot";
		}
	}
	public class WaveV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave V2 VoltDriver Shot";
		}
	}
	public class WaveWideNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WaveWidePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WaveWidePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Red V2 VoltDriver Shot";
		}
	}
	public class WaveNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WavePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WavePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red V2 VoltDriver Shot";
		}
	}
	public class WideVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide VoltDriver Shot";
		}
	}
	public class WideNovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WidePlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class WidePlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Red V2 VoltDriver Shot";
		}
	}
	public class NovaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova VoltDriver Shot";
			Projectile.penetrate = 8;
		}
	}
	public class PlasmaGreenV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green V2 VoltDriver Shot";
			Projectile.penetrate = 6;
		}
	}
	public class PlasmaRedV2VoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red V2 VoltDriver Shot";
		}
	}
	public class StardustVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust VoltDriver Shot";
		}
	}
	public class StardustNebulaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula VoltDriver Shot";
		}
	}
	public class StardustNebulaVortexVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex VoltDriver Shot";
		}
	}
	public class StardustNebulaVortexSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustNebulaSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustVortexVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex VoltDriver Shot";
		}
	}
	public class StardustVortexSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class NebulaVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula VoltDriver Shot";
		}
	}
	public class NebulaVortexVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex VoltDriver Shot";
		}
	}
	public class NebulaVortexSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class NebulaSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class VortexVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex VoltDriver Shot";
		}
	}
	public class VortexSolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
	public class SolarVoltDriverShot : VoltDriverShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar VoltDriver Shot";
			Projectile.penetrate = 12;
		}
	}
}
