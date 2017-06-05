using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.plasmabeamgreen
{
	public class PlasmaBeamGreenChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Plasma Beam Green Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.penetrate = 9;
			Main.projFrames[projectile.type] = 2;
		}

		int dustType = 61;
		Color color = MetroidMod.plaGreenColor;
		public override void AI()
		{
			if(projectile.name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			if(projectile.name.Contains("Spazer") || projectile.name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.name.Contains("Wave"));
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, dustType);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class SpazerPlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Spazer Plasma Beam Green Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 12;
		}
	}
	
	public class WavePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Plasma Beam Green Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 12;
		}
	}
	
	public class WaveSpazerPlasmaBeamGreenChargeShot : WavePlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Spazer Plasma Beam Green Charge Shot";
			mProjectile.amplitude = 14.5f*projectile.scale;
		}
	}
	
	public class IcePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamGreenChargeShot : SpazerPlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Spazer Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamGreenChargeShot : WavePlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamGreenChargeShot : WaveSpazerPlasmaBeamGreenChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Spazer Plasma Beam Green Charge Shot";
		}
	}
}