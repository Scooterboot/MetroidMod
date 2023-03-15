using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Imperialist
{
	public class ImperialistShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperialist Shot");
			Main.projFrames[Projectile.type] = 5;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1.5f;
			Projectile.extraUpdates = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			mProjectile.wavesPerSecond = 1f;
		}

		public override void AI()
		{
			if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Wide") || Projectile.Name.Contains("Vortex"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
				mProjectile.amplitude = 5f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 1;
			}
			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			if (Projectile.Name.Contains("Wave") || Projectile.Name.Contains("Nebula"))
			{
				mProjectile.WaveBehavior(Projectile);
				Projectile.tileCollide = false;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			int dustType = 235;
			Main.dust[dustType].noGravity = true;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 20, dustType, 2f);
		}
		bool[] npcPrevHit = new bool[Main.maxNPCs];
		public override void Kill(int timeLeft)
		{
			int dustType = 64;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
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
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 6;
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
	public class PlasmaRedImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red Imperialist Shot";
		}
	}
	public class PlasmaGreenImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class IceWaveNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceWideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class IceNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WaveWideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WaveNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class WideNovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class NovaImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova Imperialist Shot";
			Projectile.penetrate = 8;
		}
	}
	public class StardustNebulaVortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustNebulaSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustVortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class StardustSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class NebulaVortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class NebulaSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class VortexSolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
	public class SolarImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar Imperialist Shot";
			Projectile.penetrate = 12;
		}
	}
}

