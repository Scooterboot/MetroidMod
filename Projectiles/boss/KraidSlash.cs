using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class KraidSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 100;
			projectile.height = 100;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			Main.projFrames[projectile.type] = 3;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)projectile.position.X + (int)(projectile.velocity.X*10), (int)projectile.position.Y + (int)(projectile.velocity.Y*10), projectile.width, projectile.height);
		}
		float alpha = 1f;
		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			
			projectile.frame++;
			if(projectile.frame >= 3)
			{
				projectile.frame = 0;
			}
			
			alpha -= 0.01f;
			if(alpha <= 0f)
			{
				projectile.Kill();
			}
			else
			{
				projectile.timeLeft = 10;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
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
			sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White)*alpha, projectile.rotation, new Vector2((float)tex.Width/2f, (float)tex.Height/2f), projectile.scale, effects, 0f);
			return false;
		}
	}
}
