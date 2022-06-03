using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.Boss
{
	public class Torizo_EnergyParticle : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Tank Particle");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.scale = 0f;
			Projectile.alpha = 255;
		}
		float acc = 5f;//3f;
		float projSpeed = 12f;
		Vector2 vel;
		public override void AI()
		{
			Projectile P = Projectile;
			
			bool rotFlag = true;
			if(P.localAI[0] < 90)
			{
				P.velocity *= 0.9f;
				if(P.velocity.Length() < 1f)
				{
					P.velocity *= 0f;
					rotFlag = false;
				}
				else if(P.velocity.Length() > 1f)
				{
					vel = Vector2.Normalize(P.velocity) * 8f;
				}
				P.localAI[0]++;
			}
			else
			{
				if(P.velocity.Length() > 0.1f)
				{
					vel = P.velocity;
				}
				Vector2 dest = new Vector2(P.ai[0],P.ai[1]);
				
				Vector2 vec = dest - P.Center;
				float num2 = vec.Length();
				num2 = projSpeed / num2;
				vec *= num2;
				P.velocity.X = (vel.X * acc + vec.X) / (acc + 1f);
				P.velocity.Y = (vel.Y * acc + vec.Y) / (acc + 1f);
				
				if(Vector2.Distance(dest,P.Center+P.velocity) < projSpeed)
				{
					P.Kill();
				}
			}
			
			if(rotFlag)
			{
				P.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			}
			else
			{
				P.rotation += 0.5f;
			}
			mProjectile.DustLine(P.position, P.velocity, P.rotation, 0, 3, 57, 1.5f);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
