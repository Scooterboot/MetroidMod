using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class TorizoClawBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Torizo Claw Beam");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 200;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 76;
			Projectile.height = 76;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
			Main.projFrames[Projectile.type] = 3;
		}

		float alpha = 1f;
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			if (Projectile.frame < 2)
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter > 2)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
			}

			if (Projectile.timeLeft < 100)
			{
				alpha -= 0.01f;
				if (alpha <= 0f)
				{
					Projectile.Kill();
				}
				else
				{
					Projectile.timeLeft = 10;
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.frame == 0)
			{
				Rectangle hb = new Rectangle((int)(Projectile.Center.X - 16), (int)(Projectile.Center.Y - 16), 32, 32);
				return hb.Intersects(targetHitbox);
			}
			if (Projectile.frame == 1)
			{
				for (int i = -1; i < 2; i++)
				{
					Vector2 center = Projectile.Center;
					center += Projectile.rotation.ToRotationVector2() * 16 * i;

					Rectangle hb = new Rectangle((int)(center.X - 16), (int)(center.Y - 16), 32, 32);
					if (hb.Intersects(targetHitbox))
					{
						return true;
					}
				}
			}
			if (Projectile.frame == 2)
			{
				for (int i = -2; i < 3; i++)
				{
					Vector2 center = Projectile.Center;
					center += Projectile.rotation.ToRotationVector2() * 15f * i;
					center += (Projectile.rotation - 1.57f).ToRotationVector2() * 8f;

					Rectangle hb = new Rectangle((int)(center.X - 8), (int)(center.Y - 8), 16, 16);
					if (hb.Intersects(targetHitbox))
					{
						return true;
					}
				}
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 12;
			height = 12;
			return true;
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
			Main.spriteBatch.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Color.White * alpha, Projectile.rotation, new Vector2((float)tex.Width / 2f, (float)num108 / 2f), Projectile.scale, effects, 0f);
			return false;
		}
	}
}
