using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.EasterEgg
{
	public class CrucibleProj : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crucible Shot");
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.light = 0.5f;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
			Color color = Color.Red;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 183, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;

			if (Projectile.localAI[1] < 15f)
			{
				Projectile.localAI[1] += 1f;
			}
			else
			{
				if (Projectile.localAI[0] == 0f)
				{
					Projectile.scale -= 0.02f;
					Projectile.alpha += 30;
					if (Projectile.alpha >= 250)
					{
						Projectile.alpha = 255;
						Projectile.localAI[0] = 1f;
					}
				}
				else if (Projectile.localAI[0] == 1f)
				{
					Projectile.scale += 0.02f;
					Projectile.alpha -= 30;
					if (Projectile.alpha <= 0)
					{
						Projectile.alpha = 0;
						Projectile.localAI[0] = 0f;
					}
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;
			SoundEngine.PlaySound(SoundID.Item10, P.position);
			int num535;
			for (int num802 = 4; num802 < 31; num802 = num535 + 1)
			{
				float num803 = P.oldVelocity.X * (30f / (float)num802);
				float num804 = P.oldVelocity.Y * (30f / (float)num802);
				int num805 = Dust.NewDust(new Vector2(P.oldPosition.X - num803, P.oldPosition.Y - num804), 8, 8, 183, P.oldVelocity.X, P.oldVelocity.Y, 255, default(Color), 1.8f);
				Main.dust[num805].noGravity = true;
				Dust dust = Main.dust[num805];
				dust.velocity *= 0.5f;
				num805 = Dust.NewDust(new Vector2(P.oldPosition.X - num803, P.oldPosition.Y - num804), 8, 8, 6, P.oldVelocity.X, P.oldVelocity.Y, 255, default(Color), 1.4f);
				dust = Main.dust[num805];
				dust.velocity *= 0.05f;
				Main.dust[num805].noGravity = true;
				num535 = num802;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			Projectile P = Projectile;
			if (P.localAI[1] >= 15f)
			{
				return new Color(255, 255, 255, P.alpha);
			}
			if (P.localAI[1] < 5f)
			{
				return Color.Transparent;
			}
			int expr_1653 = (int)((P.localAI[1] - 5f) / 10f * 255f);
			return new Color(expr_1653, expr_1653, expr_1653, expr_1653);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Color color25 = Lighting.GetColor((int)((double)P.position.X + (double)P.width * 0.5) / 16, (int)(((double)P.position.Y + (double)P.height * 0.5) / 16.0));
			Main.spriteBatch.Draw(TextureAssets.Projectile[P.type].Value, new Vector2(P.position.X - Main.screenPosition.X + (float)(P.width / 2), P.position.Y - Main.screenPosition.Y + (float)(P.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.Projectile[P.type].Width(), TextureAssets.Projectile[P.type].Height())), P.GetAlpha(color25), P.rotation, new Vector2((float)TextureAssets.Projectile[P.type].Width(), 0f), P.scale, effects, 0f);
			return false;
		}
	}
}
