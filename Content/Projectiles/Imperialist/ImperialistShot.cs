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
			Projectile.height = 32;
			Projectile.scale = 1.5f;
			Projectile.extraUpdates = 60;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			/*if (Projectile.Name.Contains("Green"))
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
			}*/
			if (Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Wide") || Projectile.Name.Contains("Vortex"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
				mProjectile.amplitude = 15f * Projectile.scale;
				mProjectile.delay = 0;
			}
			if (Projectile.Name.Contains("Wave") || Projectile.Name.Contains("Nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
				mProjectile.wavesPerSecond = 0f;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			int dustType = 235;
			Main.dust[dustType].noGravity = true;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 20, dustType, 2f);
			/*int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 271, 0, 0, 100, default(Color), Projectile.scale);
			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 271, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust2].noGravity = true;*/
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
			Projectile.penetrate = 8;
		}
	}
	public class IceWaveWidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class IceWavePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class IceWidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class IcePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class WaveWidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class WavePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class WidePlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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
			Projectile.penetrate = 8;
		}
	}
	public class PlasmaGreenV2ImperialistShot : ImperialistShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Green V2 Imperialist Shot";
			Projectile.penetrate = 6;
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

