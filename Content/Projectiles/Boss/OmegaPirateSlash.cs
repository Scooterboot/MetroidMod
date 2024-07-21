using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class OmegaPirateSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
			Main.projFrames[Projectile.type] = 3;
		}

		float alpha = 2f;
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

			Projectile.frame++;
			if (Projectile.frame >= 3)
			{
				Projectile.frame = 0;
			}

			alpha -= 0.1f;
			if (alpha <= 0f)
			{
				Projectile.Kill();
			}
			else
			{
				Projectile.timeLeft = 10;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 25);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			int num108 = tex.Height / Main.projFrames[Projectile.type];
			int y4 = num108 * Projectile.frame;
			Main.spriteBatch.Draw(tex, new Vector2(((int)(Projectile.Center.X - Main.screenPosition.X)), ((int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Projectile.GetAlpha(Color.White) * Math.Min(alpha, 1f), Projectile.rotation, new Vector2(tex.Width / 2f, num108 / 2f), Projectile.scale, effects, 0f);
			return false;
		}
	}
}
