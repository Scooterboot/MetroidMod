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
			// DisplayName.SetDefault("Phazon Beam Shot");
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
			string S = Items.Weapons.PowerBeam.shooty;
			P.rotation = (float)Math.Atan2(P.velocity.Y, P.velocity.X) + 1.57f;
			
			bool isWave = (S.Contains("wave") || S.Contains("nebula")),
			isSpazer = S.Contains("spazer") || S.Contains("wide") || S.Contains("vortex"),
			isPlasma = S.Contains("plasmagreen") || S.Contains("nova") || S.Contains("solar"),
			isNebula = S.Contains("nebula");
			
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
				Projectile.tileCollide = false;
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
			if (isPlasma)
			{
				Projectile.penetrate = -1;
				Projectile.usesLocalNPCImmunity = true;
				Projectile.localNPCHitCooldown = 4;
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
}
