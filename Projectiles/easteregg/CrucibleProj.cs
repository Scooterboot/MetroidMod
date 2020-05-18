using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Projectiles.easteregg
{
	public class CrucibleProj : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crucible Shot");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.aiStyle = -1;
			projectile.melee = true;
			projectile.light = 0.5f;
			projectile.friendly = true;
			projectile.penetrate = 3;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 0.785f;
			Color color = Color.Red;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 183, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
			
			if (projectile.localAI[1] < 15f)
			{
				projectile.localAI[1] += 1f;
			}
			else
			{
				if (projectile.localAI[0] == 0f)
				{
					projectile.scale -= 0.02f;
					projectile.alpha += 30;
					if (projectile.alpha >= 250)
					{
						projectile.alpha = 255;
						projectile.localAI[0] = 1f;
					}
				}
				else if (projectile.localAI[0] == 1f)
				{
					projectile.scale += 0.02f;
					projectile.alpha -= 30;
					if (projectile.alpha <= 0)
					{
						projectile.alpha = 0;
						projectile.localAI[0] = 0f;
					}
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			Main.PlaySound(SoundID.Item10, P.position);
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
			Projectile P = projectile;
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
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Projectile P = projectile;
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			Color color25 = Lighting.GetColor((int)((double)P.position.X + (double)P.width * 0.5) / 16, (int)(((double)P.position.Y + (double)P.height * 0.5) / 16.0));
			sb.Draw(Main.projectileTexture[P.type], new Vector2(P.position.X - Main.screenPosition.X + (float)(P.width / 2), P.position.Y - Main.screenPosition.Y + (float)(P.height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.projectileTexture[P.type].Width, Main.projectileTexture[P.type].Height)), P.GetAlpha(color25), P.rotation, new Vector2((float)Main.projectileTexture[P.type].Width, 0f), P.scale, effects, 0f);
			return false;
		}
	}
}