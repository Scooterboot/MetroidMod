using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.missilecombo
{
	public class SolarLaserFlareShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Flare Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		int dir = 1;
		float num = 0f;
		float amplitude = 40f;
		bool initialize = false;
		public override void AI()
		{
			if(!initialize)
			{
				//num = Main.rand.Next(16);
				if(Main.rand.NextBool(2))
				{
					dir = -1;
				}
				amplitude = 20f + (float)Main.rand.Next(40);
				initialize = true;
			}
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			Projectile Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || !O.controlUseItem)
			{
				P.Kill();
				return;
			}
			else
			{
				Projectile Beam = Main.projectile[(int)P.ai[1]];
				P.velocity = Beam.velocity * 8f;
				
				float leadDist = Vector2.Distance(oPos,Lead.Center);
				
				if((P.localAI[0]+P.localAI[1]) < leadDist)
				{
					P.localAI[1] = Math.Abs(P.localAI[1]);
				}
				
				for(int i = 0; i < P.oldPos.Length; i++)
				{
					float oldnum = Math.Max(num - 0.0375f*(i+1), 0f);
					
					float oldt = (float)Math.PI*oldnum;
				
					float oldshift = amplitude * (float)Math.Sin(oldt) * dir;
					
					float oldlength = P.localAI[0] + P.localAI[1]*oldnum;
					
					Vector2 oldpos = oPos + Beam.velocity * oldlength;
					
					float oldrot = (float)Math.Atan2((P.velocity.Y),(P.velocity.X));
					P.oldPos[i].X = oldpos.X + (float)Math.Cos(oldrot+((float)Math.PI/2))*oldshift;
					P.oldPos[i].Y = oldpos.Y + (float)Math.Sin(oldrot+((float)Math.PI/2))*oldshift;
					P.oldPos[i] -= P.Size/2f;
				}
				
				float t = (float)Math.PI*num;
				
				float shift = amplitude * (float)Math.Sin(t) * dir;
				
				float length = P.localAI[0] + P.localAI[1]*num;
				
				Vector2 pos = oPos + Beam.velocity * length;
				
				float rot = (float)Math.Atan2((P.velocity.Y),(P.velocity.X));
				P.position.X = pos.X + (float)Math.Cos(rot+((float)Math.PI/2))*shift;
				P.position.Y = pos.Y + (float)Math.Sin(rot+((float)Math.PI/2))*shift;
				P.position -= P.Size/2f;
				
				num = Math.Min(num + 0.0375f, 1f);
				if(num >= 1f)
				{
					P.Kill();
				}
			}
			
			Color color = MetroidModPorted.plaRedColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			if(P.numUpdates == 0)
			{
				P.frame++;
			}
			if(P.frame > 1)
			{
				P.frame = 0;
			}
			
			if(P.numUpdates == 0)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 6, 0, 0, 100, default(Color), P.scale);
				Main.dust[dust].noGravity = true;
			}
			
			Vector2 velocity = P.position - P.oldPos[0];
			if(velocity.Length() < Vector2.Normalize(P.velocity).Length())
			{
				velocity = Vector2.Normalize(P.velocity);
			}
			P.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}
		
		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		/*public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, 6);
		}*/
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 50);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			//mProjectile.PlasmaDraw(projectile, Main.player[Projectile.owner], sb);
			return false;
		}
	}
}
