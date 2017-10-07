using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missiles
{
	public class MissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			
			int dustType = 6;
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			mProjectile.DustLine(projectile.Center, projectile.velocity, projectile.rotation, 5, 3, dustType, 2f);
			
			if(mProjectile.seeking && mProjectile.seekTarget > -1)
			{
				float num236 = projectile.position.X;
				float num237 = projectile.position.Y;
				bool flag5 = false;
				projectile.ai[0] += 1f;
				if (projectile.ai[0] > 5f && projectile.numUpdates <= 0)
				{
					projectile.ai[0] = 5f;
					int num239 = mProjectile.seekTarget;
					if(Main.npc[num239].active)
					{
						num236 = Main.npc[num239].position.X + (float)(Main.npc[num239].width / 2);
						num237 = Main.npc[num239].position.Y + (float)(Main.npc[num239].height / 2);
						flag5 = true;
					}
					else
					{
						mProjectile.seekTarget = -1;
					}
				}
				if (!flag5)
				{
					num236 = projectile.position.X + (float)(projectile.width / 2) + projectile.velocity.X * 100f;
					num237 = projectile.position.Y + (float)(projectile.height / 2) + projectile.velocity.Y * 100f;
				}
				float num243 = 8f;
				Vector2 vector22 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num244 = num236 - vector22.X;
				float num245 = num237 - vector22.Y;
				float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
				num246 = num243 / num246;
				num244 *= num246;
				num245 *= num246;
				projectile.velocity.X = (projectile.velocity.X * 11f + num244) / 12f;
				projectile.velocity.Y = (projectile.velocity.Y * 11f + num245) / 12f;
			}
		}
		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width += 48;
			projectile.height += 48;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			Main.PlaySound(2,(int)projectile.position.X,(int)projectile.position.Y,14);
			
			int dustType = 6;
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 30, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			projectile.Damage();
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	public class IceMissileShot : MissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Missile Shot");
		}
	}
}