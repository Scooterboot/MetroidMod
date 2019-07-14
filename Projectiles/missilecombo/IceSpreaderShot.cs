using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod;
using MetroidMod.Projectiles.missilecombo;

namespace MetroidMod.Projectiles.missilecombo
{
	public class IceSpreaderShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spreader Shot");
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
			
			mProjectile.DustLine(projectile.Center, projectile.velocity, projectile.rotation, 5, 3, 135, 2f);
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
			}
			
			for (int i = 0; i < 360; i += 10)
			{
				float rot = (float)Angle.ConvertToRadians(i);
				int num54 = Projectile.NewProjectile(P.Center.X, P.Center.Y, 0f, 0f, mod.ProjectileType("IceSpreaderDiffusionShot"),P.damage,P.knockBack,P.owner);
				IceSpreaderDiffusionShot difShot = (IceSpreaderDiffusionShot)Main.projectile[num54].modProjectile;
				difShot.spin = rot;
			}
			
			int x = (int)MathHelper.Clamp(P.Center.X / 16,0,Main.maxTilesX-2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16,0,Main.maxTilesY-2);
			Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f);
			int ft = Projectile.NewProjectile(pos.X, pos.Y, 0f, 0f, mod.ProjectileType("IceSpreaderFrozenTerrain"),0,0f,P.owner);
			
			Main.PlaySound(SoundLoader.customSoundType, P.Center,mod.GetSoundSlot(SoundType.Custom, "Sounds/IceSpreaderImpactSound"));
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			target.AddBuff(mod.BuffType("InstantFreeze"),300,true);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
}