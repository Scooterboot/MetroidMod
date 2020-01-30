using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class TorizoSwipe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo Swipe");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 200;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 32;
			projectile.height = 32;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			Main.projFrames[projectile.type] = 5;
		}

		public override void AI()
		{
			projectile.rotation = 0f;
			
			if(projectile.frame < 4)
			{
				projectile.frameCounter++;
				if(projectile.frameCounter > 2)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
				}
				projectile.timeLeft = 10;
			}
			else
			{
				projectile.Kill();
			}
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.projectileTexture[projectile.type];
			int num108 = tex.Height / Main.projFrames[projectile.type];
			int y4 = num108 * projectile.frame;
			sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Color.White, projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), projectile.scale, effects, 0f);
			return false;
		}
	}
}