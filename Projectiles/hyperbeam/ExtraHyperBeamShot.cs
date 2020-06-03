using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.hyperbeam
{
	public class ExtraHyperBeamShot : MProjectile
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/hyperbeam/ExtraHyperBeamShot";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 1f;
		}

		bool initialized = false;
		float speed = 8f;
		public override void AI()
		{
			Projectile P = projectile;
			MPlayer mp = Main.player[P.owner].GetModPlayer<MPlayer>();
			
			bool isWave = (P.Name.Contains("Wave") || P.Name.Contains("Nebula")),
			isSpazer = P.Name.Contains("Spazer"),
			isPlasma = P.Name.Contains("Plasma"),
			isNebula = P.Name.Contains("Nebula");
			
			if(!initialized)
			{
				speed = P.velocity.Length();
				initialized = true;
			}
			
			if(isSpazer || isWave)
			{
				mProjectile.WaveBehavior(projectile, !isWave);
			}
			if(isNebula)
			{
				if(initialized)
				{
					mProjectile.HomingBehavior(P,speed);
				}
			}
			
			Lighting.AddLight(P.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
			
			Vector2 velocity = projectile.position - projectile.oldPos[0];
			if(Vector2.Distance(projectile.position, projectile.position+velocity) < Vector2.Distance(projectile.position,projectile.position+projectile.velocity))
			{
				velocity = projectile.velocity;
			}
			projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(projectile,Main.player[projectile.owner],sb,10,0.6f,new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
	}
	
	
	public class ExtraWaveHyperBeamShot : ExtraHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraSpazerHyperBeamShot : ExtraHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			
			mProjectile.amplitude = 10f*projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraPlasmaHyperBeamShot : ExtraHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
		}
	}
	public class ExtraWaveSpazerHyperBeamShot : ExtraWaveHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Spazer Hyper Beam Shot");
		}
	}
	public class ExtraWavePlasmaHyperBeamShot : ExtraPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraSpazerPlasmaHyperBeamShot : ExtraPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			
			mProjectile.amplitude = 10f*projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraWaveSpazerPlasmaHyperBeamShot : ExtraWavePlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Spazer Plasma Hyper Beam Shot");
		}
	}
	
	public class ExtraNebulaHyperBeamShot : ExtraWaveHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Hyper Beam Shot");
		}
	}
	public class ExtraNebulaSpazerHyperBeamShot : ExtraWaveSpazerHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Spazer Hyper Beam Shot");
		}
	}
	public class ExtraNebulaPlasmaHyperBeamShot : ExtraWavePlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Plasma Hyper Beam Shot");
		}
	}
	public class ExtraNebulaSpazerPlasmaHyperBeamShot : ExtraWaveSpazerPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Spazer Plasma Hyper Beam Shot");
		}
	}
}