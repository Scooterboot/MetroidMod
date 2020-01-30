using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class TorizoClawBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo Claw Beam");
		}
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.timeLeft = 200;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.width = 76;
			projectile.height = 76;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			Main.projFrames[projectile.type] = 3;
		}

		float alpha = 1f;
		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			
			if(projectile.frame < 2)
			{
				projectile.frameCounter++;
				if(projectile.frameCounter > 2)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
				}
			}
			
			if(projectile.timeLeft < 100)
			{
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
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(projectile.frame == 0)
			{
				Rectangle hb = new Rectangle((int)(projectile.Center.X-16),(int)(projectile.Center.Y-16),32,32);
				return hb.Intersects(targetHitbox);
			}
			if(projectile.frame == 1)
			{
				for(int i = -1; i < 2; i++)
				{
					Vector2 center = projectile.Center;
					center += projectile.rotation.ToRotationVector2() * 16 * i;
					
					Rectangle hb = new Rectangle((int)(center.X-16),(int)(center.Y-16),32,32);
					if(hb.Intersects(targetHitbox))
					{
						return true;
					}
				}
			}
			if(projectile.frame == 2)
			{
				for(int i = -2; i < 3; i++)
				{
					Vector2 center = projectile.Center;
					center += projectile.rotation.ToRotationVector2() * 15f * i;
					center += (projectile.rotation-1.57f).ToRotationVector2() * 8f;
					
					Rectangle hb = new Rectangle((int)(center.X-8),(int)(center.Y-8),16,16);
					if(hb.Intersects(targetHitbox))
					{
						return true;
					}
				}
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 12;
			height = 12;
			return true;
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
			sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Color.White*alpha, projectile.rotation, new Vector2((float)tex.Width/2f, (float)num108/2f), projectile.scale, effects, 0f);
			return false;
		}
	}
}