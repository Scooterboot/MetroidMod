using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.plasmabeamredV2
{
	public class PlasmaBeamRedV2Shot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Plasma Beam Red V2 Shot";
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			Main.projFrames[projectile.type] = 2;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
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
			
			if(projectile.Name.Contains("Wide") || projectile.Name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Wave"));
			}
			
			if(projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			if(projectile.Name.Contains("Ice") && projectile.Name.Contains("Wave"))
			{
				mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			}
			else
			{
				mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb, 4);
			}
			return false;
		}
	}
	
	public class WidePlasmaBeamRedV2Shot : PlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wide Plasma Beam Red V2 Shot";
			
			mProjectile.amplitude = 7.5f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	
	public class WavePlasmaBeamRedV2Shot : PlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Plasma Beam Red V2 Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	
	public class WaveWidePlasmaBeamRedV2Shot : WavePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Wide Plasma Beam Red V2 Shot";
			mProjectile.amplitude = 16f*projectile.scale;
		}
	}
	
	public class IcePlasmaBeamRedV2Shot : PlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Plasma Beam Red V2 Shot";
		}
	}
	
	public class IceWidePlasmaBeamRedV2Shot : WidePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wide Plasma Beam Red V2 Shot";
		}
	}
	
	public class IceWavePlasmaBeamRedV2Shot : WavePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Plasma Beam Red V2 Shot";
			Main.projFrames[projectile.type] = 1;
		}
	}
	
	public class IceWaveWidePlasmaBeamRedV2Shot : WaveWidePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Wide Plasma Beam Red V2 Shot";
			Main.projFrames[projectile.type] = 1;
		}
	}
}