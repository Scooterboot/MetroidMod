using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.wavebeam
{
	public class WaveBeamChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Beam Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 2;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}

		int dustType = 62;
		Color color = MetroidMod.waveColor;
		public override void AI()
		{
			if(projectile.name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(projectile.numUpdates == 0)
			{
				projectile.rotation += 0.5f*projectile.direction;
				if(Main.projFrames[projectile.type] > 1)
				{
					projectile.frame++;
				}
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			mProjectile.WaveBehavior(projectile);
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, dustType);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCentered(projectile, sb);
			return false;
		}
	}
	
	public class IceWaveBeamChargeShot : WaveBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Beam Charge Shot";
			Main.projFrames[projectile.type] = 1;
		}
	}
}