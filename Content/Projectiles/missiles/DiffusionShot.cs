using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class DiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Diffusion Shot");
		}
		
		bool initialised = false;
		public float radius = 0.0f;
		public float spin = 0.0f;
		float SpinIncrease = 0.05f;
		public Vector2 basePosition = new Vector2(0f,0f);
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.extraUpdates = 0;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 2f;
			Projectile.timeLeft = 175;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 5;
		}

		public void initialise()
		{
			basePosition = Projectile.Center;
			initialised = true;
		}
		public override void AI()
		{
			if(!initialised)
			{
				initialise();
			}
			SpinIncrease += 0.001f;
			radius += 2.0f;
			spin += SpinIncrease;
			Projectile.position = (basePosition - new Vector2(Projectile.width/2,Projectile.height/2)) + spin.ToRotationVector2()*radius;

			if(!Projectile.Name.Contains("Nebula"))
			{
				Projectile.rotation = 0f;
				Projectile.frameCounter++;
				int frame = 2;
				if(Projectile.frameCounter < frame)
				{
					Projectile.frame = 0;
				}
				else if(Projectile.frameCounter < frame * 2)
				{
					Projectile.frame = 1;
				}
				else if(Projectile.frameCounter < frame * 3)
				{
					Projectile.frame = 2;
				}
				else if(Projectile.frameCounter < frame * 4)
				{
					Projectile.frame = 3;
				}
				else if(Projectile.frameCounter < frame * 5)
				{
					Projectile.frame = 4;
				}
				else if(Projectile.frameCounter < frame * 6)
				{
					Projectile.frame = 3;
				}
				else if(Projectile.frameCounter < frame * 7)
				{
					Projectile.frame = 2;
				}
				else if(Projectile.frameCounter < frame * 8 - 1)
				{
					Projectile.frame = 1;
				}
				else
				{
					Projectile.frame = 1;
					Projectile.frameCounter = 0;
				}
			
				int dustType = 6;
				Color color = MetroidMod.plaRedColor;
				if(Projectile.Name.Contains("Ice"))
				{
					dustType = 135;
					color = MetroidMod.iceColor;
				}
				if(Projectile.Name.Contains("Stardust"))
				{
					dustType = 88;
					color = MetroidMod.iceColor;
					Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 87, 0, 0, 100, default(Color), 1.5f)].noGravity = true;
				}
				Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), 2.5f);
				Main.dust[dust].noGravity = true;
			}
		}
		
		public override void OnKill(int timeLeft)
		{
			int dustType = 6;
			if(Projectile.Name.Contains("Ice") || Projectile.Name.Contains("Stardust"))
			{
				dustType = 135;
			}
			for(int i = 0; i < Projectile.oldPos.Length; i++)
			{
				for (int num70 = 0; num70 < 5; num70++)
				{
					int num71 = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), 4f);
					Main.dust[num71].noGravity = true;
				}
			}
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
	public class IceDiffusionShot : DiffusionShot
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Diffusion Shot");
		}
	}
	public class StardustDiffusionShot : DiffusionShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/missiles/IceDiffusionShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Diffusion Shot");
		}
	}
	public class NebulaDiffusionShot : DiffusionShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/missiles/NebulaMissileImpact";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Diffusion Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Main.projFrames[Projectile.type] = 1;
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.scale = 1f;
		}
		
		public override void AI()
		{
			base.AI();
			
			Projectile P = Projectile;
			P.rotation -= 0.104719758f * 2;
			P.scale = Math.Min(P.scale + 0.01f, 1.5f);
			P.position = P.Center;
			P.width = (P.height = (int)(32f*P.scale));
			P.Center = P.position;
			
			int num3;
			for (int num1012 = 0; num1012 < 1; num1012 = num3 + 1)
			{
				if (Main.rand.NextBool(2))
				{
					Vector2 vector141 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust124 = Main.dust[Dust.NewDust(P.Center - vector141 * 30f, 0, 0, 86, 0f, 0f, 0, default(Color), 1f)];
					dust124.noGravity = true;
					dust124.position = P.Center - vector141 * (float)Main.rand.Next(10, 21);
					dust124.velocity = vector141.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
					dust124.scale = 0.9f + Main.rand.NextFloat();
					dust124.fadeIn = 0.5f;
					dust124.customData = P;
					vector141 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					dust124 = Main.dust[Dust.NewDust(P.Center - vector141 * 30f, 0, 0, 90, 0f, 0f, 0, default(Color), 1f)];
					dust124.noGravity = true;
					dust124.position = P.Center - vector141 * (float)Main.rand.Next(10, 21);
					dust124.velocity = vector141.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
					dust124.scale = 0.9f + Main.rand.NextFloat();
					dust124.fadeIn = 0.5f;
					dust124.customData = P;
					dust124.color = Color.Crimson;
				}
				else
				{
					Vector2 vector142 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust125 = Main.dust[Dust.NewDust(P.Center - vector142 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f)];
					dust125.noGravity = true;
					dust125.position = P.Center - vector142 * (float)Main.rand.Next(20, 31);
					dust125.velocity = vector142.RotatedBy(-1.5707963705062866, default(Vector2)) * 5f;
					dust125.scale = 0.9f + Main.rand.NextFloat();
					dust125.fadeIn = 0.5f;
					dust125.customData = P;
				}
				num3 = num1012;
			}
			Lighting.AddLight(P.Center, 0.7f, 0.2f, 0.6f);
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			return projHitbox.Intersects(targetHitbox) ||
				Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), basePosition, Projectile.Center, Projectile.width, ref point);
		}
		
		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;
			
			P.position = P.Center;
			P.width = (P.height = 176);
			P.Center = P.position;
			P.maxPenetrate = -1;
			P.penetrate = -1;
			P.Damage();
			//Main.PlaySound(SoundID.Item14, P.position);
			for (int num93 = 0; num93 < 4; num93++)
			{
				int num94 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 240, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num94].position = P.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
			}
			for (int num95 = 0; num95 < 30; num95++)
			{
				int num96 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 62, 0f, 0f, 200, default(Color), 3.7f);
				Main.dust[num96].position = P.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				Main.dust[num96].noGravity = true;
				Dust dust = Main.dust[num96];
				dust.velocity *= 3f;
				num96 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 90, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num96].position = P.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				dust = Main.dust[num96];
				dust.velocity *= 2f;
				Main.dust[num96].noGravity = true;
				Main.dust[num96].fadeIn = 1f;
				Main.dust[num96].color = Color.Crimson * 0.5f;
			}
			for (int num97 = 0; num97 < 10; num97++)
			{
				int num98 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 62, 0f, 0f, 0, default(Color), 2.7f);
				Main.dust[num98].position = P.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)P.velocity.ToRotation(), default(Vector2)) * (float)P.width / 2f;
				Main.dust[num98].noGravity = true;
				Dust dust = Main.dust[num98];
				dust.velocity *= 3f;
			}
			for (int num99 = 0; num99 < 10; num99++)
			{
				int num100 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 240, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num100].position = P.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)P.velocity.ToRotation(), default(Vector2)) * (float)P.width / 2f;
				Main.dust[num100].noGravity = true;
				Dust dust = Main.dust[num100];
				dust.velocity *= 3f;
			}
			var entitySource = P.GetSource_Death();
			for (int num101 = 0; num101 < 2; num101++)
			{
				int num102 = Gore.NewGore(entitySource, P.position + new Vector2((float)(P.width * Main.rand.Next(100)) / 100f, (float)(P.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num102].position = P.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				Gore gore = Main.gore[num102];
				gore.velocity *= 0.3f;
				Gore gore17 = Main.gore[num102];
				gore17.velocity.X = gore17.velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
				Gore gore18 = Main.gore[num102];
				gore18.velocity.Y = gore18.velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
			}
			
			for(int i = 0; i < Projectile.oldPos.Length; i++)
			{
				for (int num70 = 0; num70 < 5; num70++)
				{
					int num71 = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, 86, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num71].noGravity = true;
				}
			}
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			Projectile P = Projectile;
			return new Color(255 - P.alpha, 255 - P.alpha, 255 - P.alpha, 255 - P.alpha);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color25 = Lighting.GetColor((int)P.Center.X / 16, (int)P.Center.Y / 16);
			Vector2 pos = P.Center + Vector2.UnitY * P.gfxOffY - Main.screenPosition;
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missiles/NebulaMissileImpact2").Value;
			Color alpha4 = P.GetAlpha(color25);
			Vector2 origin8 = new Vector2((float)tex.Width, (float)tex.Height) / 2f;
			
			Color color57 = alpha4 * 0.8f;
			color57.A /= 2;
			Color color58 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color58.A = alpha4.A;
			float num274 = 0.95f + (P.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color58 *= num274;
			float scale13 = 0.6f + P.scale * 0.6f * num274;
			
			float dist = Math.Max(radius,1);
			Vector2 diff2 = Vector2.Normalize(P.Center - basePosition);
			if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
			{
				diff2 = -Vector2.UnitY;
			}
			
			float spin2 = spin + (float)Math.PI/2;
			
			int k = 1;
			for(float i = 0f; i < dist; i += 1f + (30f*(i/dist)))
			{
				SpriteEffects se = SpriteEffects.None;
				if (k == -1)
				{
					se = SpriteEffects.FlipHorizontally;
				}
				
				Vector2 pos1 = basePosition + spin2.ToRotationVector2() * i;
				Vector2 pos2 = basePosition + diff2 * i;
				
				Vector2 fPos = Vector2.Lerp(pos1,pos2,i/dist) - Main.screenPosition;
				
				float rot = ((float)Math.PI*2f / dist) * i;
				sb.Draw(tex2, fPos, null, alpha4, rot + P.rotation*k, origin8, MathHelper.Lerp(0.1f,P.scale,(i/dist)), se, 0f);
				k *= -1;
			}
			
			sb.Draw(tex2, basePosition-Main.screenPosition, null, alpha4, -P.rotation, origin8, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			
			
			sb.Draw(tex2, pos, null, color58, -P.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex2, pos, null, alpha4, -P.rotation, origin8, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex, pos, null, color57, -P.rotation * 0.7f, origin8, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex2, pos, null, alpha4 * 0.8f, P.rotation * 0.5f, origin8, P.scale * 0.9f, spriteEffects, 0f);
			alpha4.A = 0;
			
			sb.Draw(tex, pos, null, alpha4, P.rotation, origin8, P.scale, spriteEffects, 0f);
			
			return false;
		}
	}
}
