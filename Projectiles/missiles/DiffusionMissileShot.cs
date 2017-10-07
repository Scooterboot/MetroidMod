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
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			mProjectile.DustLine(projectile.Center-projectile.velocity*0.5f, projectile.velocity, projectile.rotation, 5, 3, dustType, 2f);
			
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > (5f+(float)projectile.extraUpdates) && projectile.extraUpdates < 5)
			{
				projectile.extraUpdates++;
				projectile.ai[0] = 0f;
			}
		}
		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width += 32;
			projectile.height += 32;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			Main.PlaySound(2,(int)projectile.position.X,(int)projectile.position.Y,14);
			
			int dustType = 6;
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 135;
			}
			for (int num70 = 0; num70 < 15; num70++)
			{
				int num71 = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 30, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			projectile.Damage();
			
			int difType = mod.ProjectileType("DiffusionShot");
			if(projectile.Name.Contains("Ice"))
			{
				difType = mod.ProjectileType("IceDiffusionShot");
			}
			for(int i = 0; i < 4; i++)
			{
				float angle = ((float)Math.PI/2)*i;
				int proj = Projectile.NewProjectile(projectile.Center.X,projectile.Center.Y,0f,0f,difType,projectile.damage,projectile.knockBack,projectile.owner);
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
}