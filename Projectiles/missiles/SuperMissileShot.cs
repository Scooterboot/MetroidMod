using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missiles
{
	public class SuperMissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 12;
			projectile.height = 12;
			projectile.scale = 2f;
			projectile.extraUpdates = 0;
			projectile.timeLeft = 2000;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			
			int dustType = 6;
			float scale = 2f;
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			if(projectile.Name.Contains("Stardust") || projectile.Name.Contains("Nebula"))
			{
				dustType = 87;
				scale = 1f;
				int dustType2 = 88;
				if(projectile.Name.Contains("Nebula"))
				{
					dustType = 255;
					scale = 1.5f;
					dustType2 = 240;//254;
				}
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType2, 0, 0, 100, default(Color), 2f);
				Main.dust[dust].noGravity = true;
			}
			mProjectile.DustLine(projectile.Center-projectile.velocity*0.5f, projectile.velocity, projectile.rotation, 5, 3, dustType, scale);
			
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > (5f+(float)projectile.extraUpdates) && projectile.extraUpdates < 10)
			{
				projectile.extraUpdates++;
				projectile.ai[0] = 0f;
			}
			
			if(mProjectile.seeking && mProjectile.seekTarget > -1)
			{
				float num236 = projectile.position.X;
				float num237 = projectile.position.Y;
				bool flag5 = false;
				projectile.ai[1] += 1f;
				if(projectile.ai[1] > 5f && (projectile.numUpdates <= 0 || (projectile.numUpdates <= 1 && (projectile.Name.Contains("Stardust") || projectile.Name.Contains("Nebula")))))
				{
					projectile.ai[1] = 5f;
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
			Projectile P = projectile;
			int size = 80;
			if(P.Name.Contains("Stardust"))
			{
				size = 100;
			}
			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width += size;
			P.height += size;
			P.position.X = P.position.X - (float)(P.width / 2);
			P.position.Y = P.position.Y - (float)(P.height / 2);

			Main.PlaySound(2,(int)P.position.X,(int)P.position.Y,14);
			
			int dustType = 6;
			int dustType2 = 30;
			float scale = 1f;
			if(P.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			if(P.Name.Contains("Stardust"))
			{
				dustType = 88;
				dustType2 = 87;
				scale = 0.6f;
			}
			if(P.Name.Contains("Nebula"))
			{
				dustType = 255;
				dustType2 = 240;
				scale = 0.75f;
			}
			for (int num70 = 0; num70 < 25f*(2f-scale); num70++)
			{
				int num71 = Dust.NewDust(P.position, P.width, P.height, dustType, 0f, 0f, 100, default(Color), 5f*scale);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(P.position, P.width, P.height, dustType2, 0f, 0f, 100, default(Color), 3f*scale);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			P.Damage();
			
			if(P.Name.Contains("Nebula"))
			{
				int n = Projectile.NewProjectile(P.Center.X, P.Center.Y, 0f, 0f, mod.ProjectileType("NebulaMissileImpact"),P.damage,P.knockBack,P.owner);
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	public class IceSuperMissileShot : SuperMissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Super Missile Shot");
		}
	}
	public class StardustMissileShot : SuperMissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Missile Shot");
		}
	}
	public class NebulaMissileShot : SuperMissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Missile Shot");
		}
	}
}