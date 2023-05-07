using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Projectiles.hyperbeam
{
	public class ExtraHyperBeamShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/hyperbeam/ExtraHyperBeamShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
		}

		bool initialized = false;
		float speed = 8f;
		public override void AI()
		{
			Projectile P = Projectile;
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
				mProjectile.WaveBehavior(Projectile, !isWave);
			}
			if(isNebula)
			{
				if(initialized)
				{
					mProjectile.HomingBehavior(P,speed);
				}
			}
			
			Lighting.AddLight(P.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
			
			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if(Vector2.Distance(Projectile.position, Projectile.position+velocity) < Vector2.Distance(Projectile.position,Projectile.position+Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile,Main.player[Projectile.owner],Main.spriteBatch,10,0.6f,new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
	}
	
	
	public class ExtraWaveHyperBeamShot : ExtraHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*Projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraSpazerHyperBeamShot : ExtraHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spazer Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			
			mProjectile.amplitude = 10f*Projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraPlasmaHyperBeamShot : ExtraHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
	}
	public class ExtraWaveSpazerHyperBeamShot : ExtraWaveHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Spazer Hyper Beam Shot");
		}
	}
	public class ExtraWavePlasmaHyperBeamShot : ExtraPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*Projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraSpazerPlasmaHyperBeamShot : ExtraPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spazer Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			
			mProjectile.amplitude = 10f*Projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	public class ExtraWaveSpazerPlasmaHyperBeamShot : ExtraWavePlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Spazer Plasma Hyper Beam Shot");
		}
	}
	
	public class ExtraNebulaHyperBeamShot : ExtraWaveHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Hyper Beam Shot");
		}
	}
	public class ExtraNebulaSpazerHyperBeamShot : ExtraWaveSpazerHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Spazer Hyper Beam Shot");
		}
	}
	public class ExtraNebulaPlasmaHyperBeamShot : ExtraWavePlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Plasma Hyper Beam Shot");
		}
	}
	public class ExtraNebulaSpazerPlasmaHyperBeamShot : ExtraWaveSpazerPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Spazer Plasma Hyper Beam Shot");
		}
	}
}
