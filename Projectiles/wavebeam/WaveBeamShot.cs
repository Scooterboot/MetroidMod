using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.wavebeam
{
	public class WaveBeamShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Beam Shot";
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}

		int dustType = 62;
		Color color = MetroidMod.waveColor;
		public override void AI()
		{
			if(projectile.name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
				
				if(projectile.numUpdates == 0)
				{
					projectile.rotation += 0.5f*projectile.direction;
				}
			}
			else
			{
				projectile.rotation = 0;
			}
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			mProjectile.WaveBehavior(projectile);
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, dustType);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCentered(projectile, sb);
			return false;
		}
	}
	
	public class IceWaveBeamShot : WaveBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Beam Shot";
		}
	}
}