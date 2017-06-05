using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.widebeam
{
	public class WideBeamShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wide Beam Shot";
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			Main.projFrames[projectile.type] = 2;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 63;
		Color color = MetroidMod.wideColor;
		Color color2 = MetroidMod.wideColor;
		public override void AI()
		{
			if(projectile.name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
				color2 = default(Color);
			}
			else if(projectile.name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor2;
				color2 = default(Color);
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(projectile.numUpdates == 0)
			{
				if(projectile.name.Contains("Wave"))
				{
					projectile.frame++;
				}
				else
				{
					projectile.frameCounter++;
					if(projectile.frameCounter > 3)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
				}
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			mProjectile.WaveBehavior(projectile, !projectile.name.Contains("Wave"));
			
			if(projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, color2, projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, dustType, true, 1f, color2);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile,Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class WaveWideBeamShot : WideBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Wide Beam Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 14f*projectile.scale;
		}
	}
	
	public class IceWideBeamShot : WideBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wide Beam Shot";
		}
	}
	
	public class IceWaveWideBeamShot : WaveWideBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Wide Beam Shot";
		}
	}
}