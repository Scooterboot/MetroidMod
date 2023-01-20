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
			if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Wide") || Projectile.Name.Contains("Vortex"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
				mProjectile.amplitude = 5f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
			}
			if (Projectile.Name.Contains("Wave") || Projectile.Name.Contains("Nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
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
		}

		public override void Kill(int timeLeft)
		{
			int dustType = 64;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 4);
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
	public class IceWaveSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Imperialist Shot";
		}
	}
	public class IceWaveSpazerPlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Green Imperialist Shot";
		}
	}
	public class IceWaveSpazerPlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Red Imperialist Shot";
		}
	}
	public class IceWavePlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green Imperialist Shot";
		}
	}
	public class IceWavePlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red Imperialist Shot";
		}
	}
	public class IceSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Imperialist Shot";
		}
	}
	public class IceSpazerPlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Green Imperialist Shot";
		}
	}
	public class IceSpazerPlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Red Imperialist Shot";
		}
	}
	public class IcePlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green Imperialist Shot";
		}
	}
	public class IcePlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red Imperialist Shot";
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
	public class WaveSpazerImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Imperialist Shot";
		}
	}
	public class WaveSpazerPlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Green Imperialist Shot";
		}
	}
	public class WaveSpazerPlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Red Imperialist Shot";
		}
	}
	public class WavePlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green Imperialist Shot";
		}
	}
	public class WavePlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red Imperialist Shot";
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
	public class SpazerPlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Green Imperialist Shot";
		}
	}
	public class SpazerPlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Red Imperialist Shot";
		}
	}
	public class PlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Imperialist Shot";
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
	public class IceV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice V2 Imperialist Shot";
		}
	}
	public class IceWaveV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave V2 Imperialist Shot";
		}
	}
	public class IceWaveWideImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Imperialist Shot";
		}
	}
	public class IceWaveWideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Nova Imperialist Shot";
		}
	}
	public class IceWaveWidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Green V2 Imperialist Shot";
		}
	}
	public class IceWaveWidePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Red V2 Imperialist Shot";
		}
	}
	public class IceWaveNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova Imperialist Shot";
		}
	}
	public class IceWavePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green V2 Imperialist Shot";
		}
	}
	public class IceWavePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red V2 Imperialist Shot";
		}
	}
	public class IceWideImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Imperialist Shot";
		}
	}
	public class IceWideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Nova Imperialist Shot";
		}
	}
	public class IceWidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Green V2 Imperialist Shot";
		}
	}
	public class IceWidePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Red V2 Imperialist Shot";
		}
	}
	public class IceNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova Imperialist Shot";
		}
	}
	public class IcePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green V2 Imperialist Shot";
		}
	}
	public class IcePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red V2 Imperialist Shot";
		}
	}
	public class WaveWideImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Imperialist Shot";
		}
	}
	public class WaveV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave V2 Imperialist Shot";
		}
	}
	public class WaveWideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Nova Imperialist Shot";
		}
	}
	public class WaveWidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Green V2 Imperialist Shot";
		}
	}
	public class WaveWidePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Red V2 Imperialist Shot";
		}
	}
	public class WaveNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova Imperialist Shot";
		}
	}
	public class WavePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green V2 Imperialist Shot";
		}
	}
	public class WavePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red V2 Imperialist Shot";
		}
	}
	public class WideImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Imperialist Shot";
		}
	}
	public class WideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Nova Imperialist Shot";
		}
	}
	public class WidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Green V2 Imperialist Shot";
		}
	}
	public class WidePlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Red V2 Imperialist Shot";
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
	public class PlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green V2 Imperialist Shot";
		}
	}
	public class PlasmaRedV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red V2 Imperialist Shot";
		}
	}
	public class StardustImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Imperialist Shot";
		}
	}
	public class StardustNebulaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Imperialist Shot";
		}
	}
	public class StardustNebulaVortexImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Imperialist Shot";
		}
	}
	public class StardustNebulaVortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar Imperialist Shot";
		}
	}
	public class StardustNebulaSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar Imperialist Shot";
		}
	}
	public class StardustVortexImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Imperialist Shot";
		}
	}
	public class StardustVortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar Imperialist Shot";
		}
	}
	public class StardustSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar Imperialist Shot";
		}
	}
	public class NebulaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Imperialist Shot";
		}
	}
	public class NebulaVortexImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Imperialist Shot";
		}
	}
	public class NebulaVortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar Imperialist Shot";
		}
	}
	public class NebulaSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar Imperialist Shot";
		}
	}
	public class VortexImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Imperialist Shot";
		}
	}
	public class VortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar Imperialist Shot";
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
}

