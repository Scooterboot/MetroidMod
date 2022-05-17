using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.Boss
{
	public class KraidSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
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

		float alpha = 1f;
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			
			Projectile.frame++;
			if(Projectile.frame >= 3)
			{
				Projectile.frame = 0;
			}
			
			alpha -= 0.01f;
			if(alpha <= 0f)
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
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
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
			Main.spriteBatch.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Projectile.GetAlpha(Color.White)*alpha, Projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), Projectile.scale, effects, 0f);
			return false;
		}
	}
}
