using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.vortexbeam
{
	public class VortexBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 4;
			projectile.height = 4;
			projectile.scale = 2f;
			
			mProjectile.amplitude = 7f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 229;
		Color color = MetroidMod.lumColor;
		float scale = 1f;
		public override void AI()
		{
			if(projectile.Name.Contains("Stardust"))
			{
				dustType = 88;
				color = MetroidMod.iceColor;
				scale = 0.5f;
			}
			else if(projectile.Name.Contains("Nebula"))
			{
				dustType = 255;
				color = MetroidMod.waveColor;
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Nebula"));
			if(projectile.Name.Contains("Nebula"))
			{
				mProjectile.HomingBehavior(projectile);
			}
			
			if(projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale*0.5f);
				Main.dust[dust].noGravity = true;
				if(projectile.Name.Contains("Stardust"))
				{
					dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 87, 0, 0, 100, default(Color), projectile.scale);
					Main.dust[dust].noGravity = true;
				}
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
	
	public class NebulaVortexBeamShot : VortexBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Nebula Vortex Beam Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
		}
	}
	
	public class StardustVortexBeamShot : VortexBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Vortex Beam Shot";
		}
	}
	
	public class StardustNebulaVortexBeamShot : NebulaVortexBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Nebula Vortex Beam Shot";
		}
	}
}