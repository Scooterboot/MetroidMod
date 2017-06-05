using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.spazer
{
	public class SpazerChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Spazer Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			Main.projFrames[projectile.type] = 2;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 8;
		}

		int dustType = 64;
		Color color = MetroidMod.powColor;
		public override void AI()
		{
			if(projectile.name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			else if(projectile.name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor;
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
			
			mProjectile.WaveBehavior(projectile, !projectile.name.Contains("Wave"));
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, dustType);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile,Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class WaveSpazerChargeShot : SpazerChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Spazer Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 16f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
		}
	}
	
	public class IceSpazerChargeShot : SpazerChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Spazer Charge Shot";
		}
	}
	
	public class IceWaveSpazerChargeShot : WaveSpazerChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Spazer Charge Shot";
		}
	}
}