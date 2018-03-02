using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.vortexbeam
{
	public class VortexBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Beam Charge Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			Main.projFrames[projectile.type] = 2;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 8;
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
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Nebula"));
			if(projectile.Name.Contains("Nebula"))
			{
				mProjectile.HomingBehavior(projectile);
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale*scale);
			Main.dust[dust].noGravity = true;
			if(projectile.Name.Contains("Stardust"))
			{
				dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 87, 0, 0, 100, default(Color), projectile.scale);
				Main.dust[dust].noGravity = true;
			}
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
	
	public class NebulaVortexBeamChargeShot : VortexBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Nebula Vortex Beam Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 14f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
		}
	}
	
	public class StardustVortexBeamChargeShot : VortexBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Vortex Beam Charge Shot";
		}
	}
	
	public class StardustNebulaVortexBeamChargeShot : NebulaVortexBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Nebula Vortex Beam Charge Shot";
		}
	}
}