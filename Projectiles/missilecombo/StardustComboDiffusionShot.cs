using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class StardustComboDiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Dragon");
		}
		
		bool initialised = false;
		float radius = 5f;//0.0f;
		public float spin = 0.0f;
		float SpinIncrease = 0.05f;
		Vector2 basePosition = new Vector2(0f,0f);
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.extraUpdates = 0;
			projectile.width = 30;
			projectile.height = 30;
			projectile.scale = 1f;
			projectile.timeLeft = 140;//175;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
		}
		
		const int SegLength = 6;
		Vector2[] segmentPos = new Vector2[SegLength];
		float[] segmentRot = new float[SegLength];

		public void initialise()
		{
			basePosition = projectile.Center;
			for(int i = 0; i < segmentPos.Length; i++)
			{
				segmentPos[i] = projectile.Center;
			}
			initialised = true;
		}
		public override void AI()
		{
			Projectile P = projectile;
			if(!initialised)
			{
				initialise();
			}
			SpinIncrease += 0.0005f;
			radius += 3.0f;
			spin += SpinIncrease;
			P.position = (basePosition - new Vector2(P.width/2,P.height/2)) + spin.ToRotationVector2()*radius;
			
			Vector2 vel = P.position - P.oldPos[0];
			if(vel != Vector2.Zero)
			{
				vel.Normalize();
				P.rotation = vel.ToRotation() + 1.57f;
			}

			Color color = MetroidMod.iceColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			
			segmentPos[0] = P.Center;
			segmentRot[0] = P.rotation;
			
			for(int i = 1; i < segmentPos.Length; i++)
			{
				Vector2 pos = segmentPos[i-1] - segmentPos[i];
				segmentRot[i] = pos.ToRotation() + 1.57f;
				float len = pos.Length();
				int width = P.width/2;
				
				len = (len - (float)width) / len;
				pos.X *= len;
				pos.Y *= len;
				segmentPos[i] += pos;
				
				if (Main.rand.Next(30) == 0)
				{
					Vector2 dustPos = segmentPos[i] - new Vector2(P.width/2,P.height/2);
					int num1049 = Dust.NewDust(dustPos, P.width, P.height, 135, 0f, 0f, 0, default(Color), 2f);
					Main.dust[num1049].noGravity = true;
					Main.dust[num1049].fadeIn = 2f;
				}
			}
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = projectile;
			for(int i = 0; i < segmentPos.Length; i++)
			{
				Vector2 pos = segmentPos[i] - new Vector2(P.width/2,P.height/2);
				Rectangle rect = new Rectangle((int)pos.X,(int)pos.Y,projHitbox.Width,projHitbox.Height);
				return rect.Intersects(targetHitbox);
			}
			return null;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			target.AddBuff(mod.BuffType("InstantFreeze"),600,true);
		}
		
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < projectile.oldPos.Length; i++)
			{
				for (int num70 = 0; num70 < 5; num70++)
				{
					int num71 = Dust.NewDust(projectile.oldPos[i], projectile.width, projectile.height, 88, 0f, 0f, 100, default(Color), 4f);
					Main.dust[num71].noGravity = true;
					num71 = Dust.NewDust(projectile.oldPos[i], projectile.width, projectile.height, 87, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num71].noGravity = true;
				}
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Projectile P = projectile;
			for(int i = segmentPos.Length-1; i >= 0; i--)
			{
				Texture2D tex = Main.projectileTexture[P.type];
				if(i == segmentPos.Length-1)
				{
					tex = mod.GetTexture("Projectiles/missilecombo/StardustComboDiffusionShot3");
				}
				else if(i > 0)
				{
					if(i % 2 == 0)
					{
						tex = mod.GetTexture("Projectiles/missilecombo/StardustComboDiffusionShot2");
					}
					else
					{
						tex = mod.GetTexture("Projectiles/missilecombo/StardustComboDiffusionShot1");
					}
				}
				Color color = P.GetAlpha(Color.White);
				color.A /= 2;
				
				sb.Draw(tex, 
				segmentPos[i] - Main.screenPosition, 
				new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), 
				color, 
				segmentRot[i], 
				new Vector2((float)tex.Width/2f, (float)tex.Height/2), 
				1f, 
				SpriteEffects.None, 
				0f);
			}
			return false;
		}
	}
}