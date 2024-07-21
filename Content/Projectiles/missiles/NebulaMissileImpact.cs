using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class NebulaMissileImpact : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Missile Shot");
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			//Projectile.magic = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 5;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		public override void AI()
		{
			Projectile P = Projectile;
			P.ai[0] += 1f;
			P.alpha -= 15;
			if (P.alpha < 0)
			{
				P.alpha = 0;
			}
			P.rotation -= 0.104719758f;
			int num3;
			for (int num1012 = 0; num1012 < 1; num1012 = num3 + 1)
			{
				if (Main.rand.NextBool(2))
				{
					Vector2 vector141 = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi);
					Dust dust124 = Main.dust[Dust.NewDust(P.Center - vector141 * 30f, 0, 0, 86, 0f, 0f, 0, default(Color), 1f)];
					dust124.noGravity = true;
					dust124.position = P.Center - vector141 * (float)Main.rand.Next(10, 21);
					dust124.velocity = vector141.RotatedBy(MathHelper.PiOver2, default(Vector2)) * 6f;
					dust124.scale = 0.9f + Main.rand.NextFloat();
					dust124.fadeIn = 0.5f;
					dust124.customData = P;
					vector141 = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi);
					dust124 = Main.dust[Dust.NewDust(P.Center - vector141 * 30f, 0, 0, 90, 0f, 0f, 0, default(Color), 1f)];
					dust124.noGravity = true;
					dust124.position = P.Center - vector141 * (float)Main.rand.Next(10, 21);
					dust124.velocity = vector141.RotatedBy(MathHelper.PiOver2, default(Vector2)) * 6f;
					dust124.scale = 0.9f + Main.rand.NextFloat();
					dust124.fadeIn = 0.5f;
					dust124.customData = P;
					dust124.color = Color.Crimson;
				}
				else
				{
					Vector2 vector142 = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi);
					Dust dust125 = Main.dust[Dust.NewDust(P.Center - vector142 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f)];
					dust125.noGravity = true;
					dust125.position = P.Center - vector142 * (float)Main.rand.Next(20, 31);
					dust125.velocity = vector142.RotatedBy(-MathHelper.PiOver2, default(Vector2)) * 5f;
					dust125.scale = 0.9f + Main.rand.NextFloat();
					dust125.fadeIn = 0.5f;
					dust125.customData = P;
				}
				num3 = num1012;
			}
			if (P.alpha < 150)
			{
				Lighting.AddLight(P.Center, 0.7f, 0.2f, 0.6f);
			}
			if (P.ai[0] >= 600f)
			{
				P.Kill();
				return;
			}
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
			SoundEngine.PlaySound(SoundID.Item14, P.position);
			for (int num93 = 0; num93 < 4; num93++)
			{
				int num94 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 240, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num94].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
			}
			for (int num95 = 0; num95 < 30; num95++)
			{
				int num96 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 62, 0f, 0f, 200, default(Color), 3.7f);
				Main.dust[num96].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				Main.dust[num96].noGravity = true;
				Dust dust = Main.dust[num96];
				dust.velocity *= 3f;
				num96 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 90, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num96].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				dust = Main.dust[num96];
				dust.velocity *= 2f;
				Main.dust[num96].noGravity = true;
				Main.dust[num96].fadeIn = 1f;
				Main.dust[num96].color = Color.Crimson * 0.5f;
			}
			for (int num97 = 0; num97 < 10; num97++)
			{
				int num98 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 62, 0f, 0f, 0, default(Color), 2.7f);
				Main.dust[num98].position = P.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)P.velocity.ToRotation(), default(Vector2)) * (float)P.width / 2f;
				Main.dust[num98].noGravity = true;
				Dust dust = Main.dust[num98];
				dust.velocity *= 3f;
			}
			for (int num99 = 0; num99 < 10; num99++)
			{
				int num100 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 240, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num100].position = P.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)P.velocity.ToRotation(), default(Vector2)) * (float)P.width / 2f;
				Main.dust[num100].noGravity = true;
				Dust dust = Main.dust[num100];
				dust.velocity *= 3f;
			}
			var entitySource = P.GetSource_Death();
			for (int num101 = 0; num101 < 2; num101++)
			{
				int num102 = Gore.NewGore(entitySource, P.position + new Vector2((float)(P.width * Main.rand.Next(100)) / 100f, (float)(P.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num102].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				Gore gore = Main.gore[num102];
				gore.velocity *= 0.3f;
				Gore gore17 = Main.gore[num102];
				gore17.velocity.X = gore17.velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
				Gore gore18 = Main.gore[num102];
				gore18.velocity.Y = gore18.velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
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
