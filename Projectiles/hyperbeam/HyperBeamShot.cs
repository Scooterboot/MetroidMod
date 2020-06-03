using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.hyperbeam
{
	public class HyperBeamShot : MProjectile
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/hyperbeam/HyperBeamShot";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
		}

		public override void AI()
		{
			Projectile P = projectile;
			MPlayer mp = Main.player[P.owner].GetModPlayer<MPlayer>();
			
			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + 1.57f;
			
			Lighting.AddLight(P.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
		}
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			float scale = 0.65f;
			if(projectile.Name.Contains("Plasma"))
			{
				scale = 1f;
			}
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(projectile,Main.player[projectile.owner],sb,10,scale,new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
	}
	
	
	public class WaveHyperBeamShot : HyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
		}
	}
	public class SpazerHyperBeamShot : HyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Hyper Beam Shot");
		}
	}
	public class PlasmaHyperBeamShot : HyperBeamShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/hyperbeam/PlasmaHyperBeamShot";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 3f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 4;
		}
	}
	public class WaveSpazerHyperBeamShot : WaveHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Spazer Hyper Beam Shot");
		}
	}
	public class WavePlasmaHyperBeamShot : PlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
		}
	}
	public class SpazerPlasmaHyperBeamShot : PlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Plasma Hyper Beam Shot");
		}
	}
	public class WaveSpazerPlasmaHyperBeamShot : WavePlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Spazer Plasma Hyper Beam Shot");
		}
	}
	
	public class NebulaHyperBeamShot : WaveHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Hyper Beam Shot");
		}
	}
	public class NebulaSpazerHyperBeamShot : WaveSpazerHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Spazer Hyper Beam Shot");
		}
	}
	public class NebulaPlasmaHyperBeamShot : WavePlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Plasma Hyper Beam Shot");
		}
	}
	public class NebulaSpazerPlasmaHyperBeamShot : WaveSpazerPlasmaHyperBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Spazer Plasma Hyper Beam Shot");
		}
	}
}