using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.boss
{
	public class KraidBellySpike : ModProjectile
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
			projectile.width = 42;
			projectile.height = 42;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			Main.projFrames[projectile.type] = 4;
		}

		bool stoptracking = false;
		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			
			Player player = Main.player[(int)projectile.ai[1]];
			if(projectile.numUpdates <= 0)
			{
				projectile.localAI[0]++;
			}
			if(projectile.localAI[0] > 30 && projectile.localAI[0] <= 45 && !stoptracking && !player.dead)
			{
				Vector2 vector = projectile.Center;
				bool flag5 = false;
				if(player.active)
				{
					vector = player.Center;
					flag5 = true;
				}
				if (!flag5)
				{
					vector = projectile.Center + projectile.velocity*100f;
				}
				float num243 = 4f;
				Vector2 vector2 = vector - projectile.Center;
				float num246 = (float)Math.Sqrt((double)(vector2.X * vector2.X + vector2.Y * vector2.Y));
				num246 = num243 / num246;
				vector2 *= num246;
				projectile.velocity = (projectile.velocity * 31f + vector2) / 32f;

				if(Vector2.Distance(player.position,projectile.position) <= projectile.localAI[0]*3)
				{
					stoptracking = true;
				}
			}
			if(projectile.localAI[0] >= 20)
			{
				projectile.extraUpdates = 1;
			}
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			if(!Main.npc[(int)projectile.ai[0]].active || projectile.localAI[0] >= 35)
			{
				SpriteEffects effects = SpriteEffects.None;
				if (projectile.spriteDirection == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				Texture2D tex = Main.projectileTexture[projectile.type];
				int num108 = tex.Height / Main.projFrames[projectile.type];
				int y4 = num108 * projectile.frame;
				sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)projectile.height/2f), projectile.scale, effects, 0f);
			}
			return false;
		}
	}
}