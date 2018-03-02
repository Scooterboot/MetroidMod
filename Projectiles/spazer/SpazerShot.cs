using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.spazer
{
	public class SpazerShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 4;
			projectile.height = 4;
			projectile.scale = 2f;
			
			mProjectile.amplitude = 7.5f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 64;
		Color color = MetroidMod.powColor;
		public override void AI()
		{
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			else if(projectile.Name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor;
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Wave"));
			
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
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile,Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class WaveSpazerShot : SpazerShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Spazer Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 12f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
		}
	}
	
	public class IceSpazerShot : SpazerShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Spazer Shot";
		}
	}
	
	public class IceWaveSpazerShot : WaveSpazerShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Spazer Shot";
		}
	}
}