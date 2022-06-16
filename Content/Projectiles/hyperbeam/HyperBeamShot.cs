using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Projectiles.hyperbeam
{
	public class HyperBeamShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/hyperbeam/HyperBeamShot";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
		}

		bool spawned = false;
		float scale = 0f;
		public override bool PreAI()
		{
			if(!spawned)
			{
				scale = Projectile.scale;
				spawned = true;
			}
			return true;
		}
		public override void AI()
		{
			Projectile P = Projectile;
			MPlayer mp = Main.player[P.owner].GetModPlayer<MPlayer>();
			
			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + 1.57f;
			
			Lighting.AddLight(P.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
			
			P.localAI[0] = Math.Min(P.localAI[0]+0.075f,1f);
			P.localAI[1] = Math.Min(P.localAI[1]+0.025f,1f);
			
			P.scale = scale * P.localAI[0];
		}
		public override bool PreDraw(ref Color lightColor)
		{
			float scale = 0.65f;
			if(Projectile.Name.Contains("Plasma"))
			{
				scale = 1f;
			}
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile,Main.player[Projectile.owner],Main.spriteBatch,10,scale*Projectile.localAI[0]*Projectile.localAI[1],new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
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
			Projectile.tileCollide = false;
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
		public override string Texture => $"{Mod.Name}/Content/Projectiles/hyperbeam/PlasmaHyperBeamShot";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 3f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
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
			Projectile.tileCollide = false;
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
