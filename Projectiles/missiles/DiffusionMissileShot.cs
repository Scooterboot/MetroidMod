using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Projectiles.missiles;

namespace MetroidMod.Projectiles.missiles
{
	public class DiffusionMissileShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diffusion Missile Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 12;
			projectile.height = 12;
			projectile.scale = 2.25f;
			projectile.velocity *= 0.25f;
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
			if (projectile.ai[0] > (5f+(float)projectile.extraUpdates) && projectile.extraUpdates < 5)
			{
				projectile.extraUpdates++;
				projectile.ai[0] = 0f;
			}
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width += 32;
			P.height += 32;
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
			
			int difType = mod.ProjectileType("DiffusionShot");
			int num = 4;
			if(P.Name.Contains("Ice"))
			{
				difType = mod.ProjectileType("IceDiffusionShot");
			}
			if(P.Name.Contains("Stardust"))
			{
				difType = mod.ProjectileType("StardustDiffusionShot");
				num = 6;
			}
			if(P.Name.Contains("Nebula"))
			{
				difType = mod.ProjectileType("NebulaDiffusionShot");
				num = 5;
			}
			for(int i = 0; i < num; i++)
			{
				float angle = ((float)(Math.PI*2)/num)*i;
				int proj = Projectile.NewProjectile(P.Center.X,P.Center.Y,0f,0f,difType,P.damage,P.knockBack,P.owner);
				DiffusionShot difShot = (DiffusionShot)Main.projectile[proj].modProjectile;
				difShot.spin = angle;
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	public class IceDiffusionMissileShot : DiffusionMissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Diffusion Missile Shot");
		}
	}
	public class StardustDiffusionMissileShot : DiffusionMissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Diffusion Missile Shot");
		}
	}
	public class NebulaDiffusionMissileShot : DiffusionMissileShot
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Diffusion Missile Shot");
		}
	}
}