using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.Boss
{
	public class KraidBellySpike : ModProjectile
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
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
			Main.projFrames[Projectile.type] = 4;
		}

		bool stoptracking = false;
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			
			Player player = Main.player[(int)Projectile.ai[1]];
			if(Projectile.numUpdates <= 0)
			{
				Projectile.localAI[0]++;
			}
			if(Projectile.localAI[0] > 30 && Projectile.localAI[0] <= 45 && !stoptracking && !player.dead)
			{
				Vector2 vector = Projectile.Center;
				bool flag5 = false;
				if(player.active)
				{
					vector = player.Center;
					flag5 = true;
				}
				if (!flag5)
				{
					vector = Projectile.Center + Projectile.velocity*100f;
				}
				float num243 = 4f;
				Vector2 vector2 = vector - Projectile.Center;
				float num246 = (float)Math.Sqrt((double)(vector2.X * vector2.X + vector2.Y * vector2.Y));
				num246 = num243 / num246;
				vector2 *= num246;
				Projectile.velocity = (Projectile.velocity * 31f + vector2) / 32f;

				if(Vector2.Distance(player.position,Projectile.position) <= Projectile.localAI[0]*3)
				{
					stoptracking = true;
				}
			}
			if(Projectile.localAI[0] >= 20)
			{
				Projectile.extraUpdates = 1;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if(!Main.npc[(int)Projectile.ai[0]].active || Projectile.localAI[0] >= 35)
			{
				SpriteEffects effects = SpriteEffects.None;
				if (Projectile.spriteDirection == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
				int num108 = tex.Height / Main.projFrames[Projectile.type];
				int y4 = num108 * Projectile.frame;
				Main.spriteBatch.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y + Projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width/2f, (float)Projectile.height/2f), Projectile.scale, effects, 0f);
			}
			return false;
		}
	}
}
