using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.plasmabeamgreenV2
{
	public class PlasmaBeamGreenV2ChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Plasma Beam Green V2 Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.penetrate = 9;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
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
			else if(projectile.name.Contains("Wave"))
			{
				dustType = 15;
				color = MetroidMod.plaGreenColor2;
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(Main.projFrames[projectile.type] > 1)
			{
				if(projectile.numUpdates == 0)
				{
					projectile.frame++;
				}
				if(projectile.frame > 1)
				{
					projectile.frame = 0;
				}
			}
			
			if(projectile.name.Contains("Wide") || projectile.name.Contains("Wave"))
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
	
	public class WidePlasmaBeamGreenV2ChargeShot : PlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wide Plasma Beam Green V2 Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}
	}
	
	public class WavePlasmaBeamGreenV2ChargeShot : PlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Plasma Beam Green V2 Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 12f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}
	}
	
	public class WaveWidePlasmaBeamGreenV2ChargeShot : WavePlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Wide Plasma Beam Green V2 Charge Shot";
			mProjectile.amplitude = 16f*projectile.scale;
		}
	}
	
	public class IcePlasmaBeamGreenV2ChargeShot : PlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Plasma Beam Green V2 Charge Shot";
		}
	}
	
	public class IceWidePlasmaBeamGreenV2ChargeShot : WidePlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wide Plasma Beam Green V2 Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamGreenV2ChargeShot : WavePlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Plasma Beam Green V2 Charge Shot";
			Main.projFrames[projectile.type] = 1;
			mProjectile.delay = 3;
		}
	}
	
	public class IceWaveWidePlasmaBeamGreenV2ChargeShot : WaveWidePlasmaBeamGreenV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Wide Plasma Beam Green V2 Charge Shot";
			Main.projFrames[projectile.type] = 1;
			mProjectile.delay = 3;
		}
	}
}
