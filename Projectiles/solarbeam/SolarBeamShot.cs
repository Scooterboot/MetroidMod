using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.solarbeam
{
	public class SolarBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Beam Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			projectile.penetrate = 12;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{
			if(projectile.Name.Contains("Stardust"))
			{
				dustType = 87;
			}
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			if(projectile.Name.Contains("Vortex") || projectile.Name.Contains("Nebula"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Nebula"));
			}
			if(projectile.Name.Contains("Nebula"))
			{
				mProjectile.HomingBehavior(projectile);
			}
			
			if(projectile.numUpdates == 0 || !projectile.Name.Contains("Stardust"))
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0, 0, 100, default(Color), projectile.scale*2);
				Main.dust[dust].noGravity = true;
				if(projectile.Name.Contains("Stardust"))
				{
					dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 87, 0, 0, 100, default(Color), projectile.scale);
					Main.dust[dust].noGravity = true;
				}
			}
			
			Vector2 velocity = projectile.position - projectile.oldPos[0];
			if(Vector2.Distance(projectile.position, projectile.position+velocity) < Vector2.Distance(projectile.position,projectile.position+projectile.velocity))
			{
				velocity = projectile.velocity;
			}
			projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}

		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 50);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class VortexSolarBeamShot : SolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Vortex Solar Beam Shot";
			
			mProjectile.amplitude = 7.5f*projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
			mProjectile.delay = 3;
		}
	}
	
	public class NebulaSolarBeamShot : SolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/SolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Nebula Solar Beam Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	
	public class NebulaVortexSolarBeamShot : NebulaSolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Nebula Vortex Solar Beam Shot";
			mProjectile.amplitude = 14f*projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
		}
	}
	
	public class StardustSolarBeamShot : SolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/SolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Solar Beam Shot";
		}
	}
	
	public class StardustVortexSolarBeamShot : VortexSolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Vortex Solar Beam Shot";
		}
	}
	
	public class StardustNebulaSolarBeamShot : NebulaSolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/SolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Nebula Solar Beam Shot";
		}
	}
	
	public class StardustNebulaVortexSolarBeamShot : NebulaVortexSolarBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Nebula Vortex Solar Beam Shot";
		}
	}
}