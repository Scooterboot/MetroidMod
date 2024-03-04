using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class StardustComboDiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Dragon");
		}
		
		bool initialised = false;
		float radius = 5f;//0.0f;
		public float spin = 0.0f;
		float SpinIncrease = 0.05f;
		Vector2 basePosition = new Vector2(0f,0f);
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.extraUpdates = 0;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.scale = 1f;
			Projectile.timeLeft = 140;//175;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		
		const int SegLength = 6;
		Vector2[] segmentPos = new Vector2[SegLength];
		float[] segmentRot = new float[SegLength];

		public void initialise()
		{
			basePosition = Projectile.Center;
			for(int i = 0; i < segmentPos.Length; i++)
			{
				segmentPos[i] = Projectile.Center;
			}
			initialised = true;
		}
		public override void AI()
		{
			Projectile P = Projectile;
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
				
				if (Main.rand.NextBool(30))
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
			Projectile P = Projectile;
			for(int i = 0; i < segmentPos.Length; i++)
			{
				Vector2 pos = segmentPos[i] - new Vector2(P.width/2,P.height/2);
				Rectangle rect = new Rectangle((int)pos.X,(int)pos.Y,projHitbox.Width,projHitbox.Height);
				return rect.Intersects(targetHitbox);
			}
			return null;
		}
		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{	
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(),600,true);
		}
		
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < Projectile.oldPos.Length; i++)
			{
				for (int num70 = 0; num70 < 5; num70++)
				{
					int num71 = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, DustID.GemSapphire, 0f, 0f, 100, default(Color), 4f);
					Main.dust[num71].noGravity = true;
					num71 = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, DustID.GemTopaz, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num71].noGravity = true;
				}
			}
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			SpriteBatch sb = Main.spriteBatch;
			for(int i = segmentPos.Length-1; i >= 0; i--)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
				if(i == segmentPos.Length-1)
				{
					tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/StardustComboDiffusionShot3").Value;
				}
				else if(i > 0)
				{
					if(i % 2 == 0)
					{
						tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/StardustComboDiffusionShot2").Value;
					}
					else
					{
						tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/StardustComboDiffusionShot1").Value;
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
