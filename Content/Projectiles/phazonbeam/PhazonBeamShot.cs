using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Projectiles.phazonbeam
{
	public class PhazonBeamShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/phazonbeam/PhazonBeamShot";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Beam Shot");
		}
		int maxTime = 60;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;//1.5f;
			Projectile.timeLeft = maxTime;
			Projectile.extraUpdates = 4;
			//Projectile.tileCollide = false;
			//Projectile.penetrate = -1;
			//Projectile.usesLocalNPCImmunity = true;
			//Projectile.localNPCHitCooldown = 4;
		}

		bool initialize = false;
		Vector2 vel = Vector2.Zero;
		float speed = 15f;
		public override void AI()
		{
			Projectile P = Projectile;
			P.rotation = (float)Math.Atan2(P.velocity.Y, P.velocity.X) + 1.57f;
			
			bool isWave = (P.Name.Contains("Wave") || P.Name.Contains("Nebula")),
			isSpazer = P.Name.Contains("Spazer"),
			isPlasma = P.Name.Contains("Plasma"),
			isNebula = P.Name.Contains("Nebula");
			
			if(!initialize)
			{
				vel = Vector2.Normalize(P.velocity);
				if (float.IsNaN(vel.X) || float.IsNaN(vel.Y))
				{
					vel = -Vector2.UnitY;
				}
				P.velocity = vel * speed;
				
				for(int i = 0; i < P.oldRot.Length; i++)
				{
					P.oldRot[i] = P.rotation;
				}
				
				if(isSpazer && !isWave)
				{
					P.velocity.X += (float)Main.rand.Next(-50,51) * 0.05f;
					P.velocity.Y += (float)Main.rand.Next(-50,51) * 0.05f;
				}
				
				initialize = true;
			}
			Lighting.AddLight(P.Center, 0f/255f,148f/255f,255f/255f);
			
			//if(P.numUpdates == 0)
			//{
				int dust = Dust.NewDust(P.position, P.width, P.height, DustID.BlueCrystalShard, 0, 0, 100, default(Color), P.scale);
				Main.dust[dust].noGravity = true;
			//}
			
			float mult = 0.005f;
			if(isWave)
			{
				mult = 0.015f;
				if(isSpazer)
				{
					mult = 0.025f;
				}
			}
			P.velocity.X += (float)Main.rand.Next(-50,51) * mult;
			P.velocity.Y += (float)Main.rand.Next(-50,51) * mult;
			
			if(isNebula)
			{
				mProjectile.HomingBehavior(P,speed);
			}
		}
		
		public override void PostAI()
		{
			Projectile P = Projectile;
			P.localAI[0] += 1f;
			for (int i = P.oldPos.Length-1; i > 0; i--)
			{
				P.oldPos[i] = P.oldPos[i - 1];
			}
			P.oldPos[0] = P.position+P.velocity;
			
			float width = 24f * P.scale / 2f;
			if(Vector2.Distance(P.oldPos[0],P.oldPos[1]) > width && P.localAI[0] > P.extraUpdates*3)
			{
				for(int i = 1; i < P.oldPos.Length; i++)
				{
					Vector2 pos = P.oldPos[i-1] - P.oldPos[i];
					float len = pos.Length();
					
					len = (len - (float)width) / len;
					pos.X *= len;
					pos.Y *= len;
					P.oldPos[i] += pos;
				}
			}
			
			for (int i = P.oldRot.Length-1; i > 0; i--)
			{
				P.oldRot[i] = P.oldRot[i - 1];
			}
			P.oldRot[0] = P.rotation;
		}
		
		/*public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, 68);
		}*/
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	public class WavePhazonBeamShot : PhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Phazon Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
		}
	}
	public class SpazerPhazonBeamShot : PhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Phazon Beam Shot");
		}
	}
	public class PlasmaPhazonBeamShot : PhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Phazon Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
		}
	}
	public class SpazerPlasmaPhazonBeamShot : PlasmaPhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Plasma Phazon Beam Shot");
		}
	}
	public class WaveSpazerPhazonBeamShot : WavePhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Spazer Phazon Beam Shot");
		}
	}
	public class WavePlasmaPhazonBeamShot : PlasmaPhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Plasma Phazon Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
		}
	}
	public class WaveSpazerPlasmaPhazonBeamShot : WavePlasmaPhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Spazer Plasma Phazon Beam Shot");
		}
	}
	
	public class NebulaPhazonBeamShot : WavePhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Phazon Beam Shot");
		}
	}
	public class NebulaSpazerPhazonBeamShot : WaveSpazerPhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Spazer Phazon Beam Shot");
		}
	}
	public class NebulaPlasmaPhazonBeamShot : WavePlasmaPhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Plasma Phazon Beam Shot");
		}
	}
	public class NebulaSpazerPlasmaPhazonBeamShot : WaveSpazerPlasmaPhazonBeamShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Spazer Plasma Phazon Beam Shot");
		}
	}
}
