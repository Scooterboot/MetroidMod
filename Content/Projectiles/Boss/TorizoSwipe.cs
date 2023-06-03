using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class TorizoSwipe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Torizo Swipe");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 200;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
			Main.projFrames[Type] = 5;
		}

		public override void AI()
		{
			Projectile.rotation = 0f;
			
			if(Projectile.frame < 4)
			{
				Projectile.frameCounter++;
				if(Projectile.frameCounter > 2)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				Projectile.timeLeft = 10;
			}
			else
			{
				Projectile.Kill();
			}
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
			Main.spriteBatch.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Color.White, Projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), Projectile.scale, effects, 0f);
			return false;
		}
	}
}
