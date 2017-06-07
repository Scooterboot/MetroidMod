using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.plasmabeamred
{
	public class PlasmaBeamRedShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam Red Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
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
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			if(projectile.Name.Contains("Spazer") || projectile.Name.Contains("Wave"))
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
			mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb, 4);
			return false;
		}
	}
	
	public class SpazerPlasmaBeamRedShot : PlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Spazer Plasma Beam Red Shot";
			
			mProjectile.amplitude = 7.5f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	
	public class WavePlasmaBeamRedShot : PlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Plasma Beam Red Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 3;
		}
	}
	
	public class WaveSpazerPlasmaBeamRedShot : WavePlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Spazer Plasma Beam Red Shot";
			mProjectile.amplitude = 12f*projectile.scale;
		}
	}
	
	public class IcePlasmaBeamRedShot : PlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Plasma Beam Red Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamRedShot : SpazerPlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Spazer Plasma Beam Red Shot";
		}
	}
	
	public class IceWavePlasmaBeamRedShot : WavePlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Plasma Beam Red Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamRedShot : WaveSpazerPlasmaBeamRedShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Spazer Plasma Beam Red Shot";
		}
	}
}