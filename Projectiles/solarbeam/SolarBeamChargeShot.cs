using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.solarbeam
{
	public class SolarBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Beam Charge Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 14;
			projectile.height = 14;
			projectile.scale = 2f;
			projectile.penetrate = 16;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
		}

		int dustType = 6;
		Color color = MetroidMod.novColor;
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
			
			int dType = Utils.SelectRandom<int>(Main.rand, new int[] { 6,158 });
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dType, 0, 0, 100, default(Color), projectile.scale*2);
			Main.dust[dust].noGravity = true;
			if(projectile.Name.Contains("Stardust"))
			{
				dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 87, 0, 0, 100, default(Color), projectile.scale);
				Main.dust[dust].noGravity = true;
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
			mProjectile.Diffuse(projectile, dustType);
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
	
	public class VortexSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Vortex Solar Beam Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
			mProjectile.delay = 4;
		}
	}
	
	public class NebulaSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/SolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Nebula Solar Beam Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 12f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}
	}
	
	public class NebulaVortexSolarBeamChargeShot : NebulaSolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Nebula Vortex Solar Beam Charge Shot";
			mProjectile.amplitude = 16f*projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
		}
	}
	
	public class StardustSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/SolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Solar Beam Charge Shot";
		}
	}
	
	public class StardustVortexSolarBeamChargeShot : VortexSolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Vortex Solar Beam Charge Shot";
		}
	}
	
	public class StardustNebulaSolarBeamChargeShot : NebulaSolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/SolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Nebula Solar Beam Charge Shot";
		}
	}
	
	public class StardustNebulaVortexSolarBeamChargeShot : NebulaVortexSolarBeamChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/solarbeam/VortexSolarBeamChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Stardust Nebula Vortex Solar Beam Charge Shot";
		}
	}
}