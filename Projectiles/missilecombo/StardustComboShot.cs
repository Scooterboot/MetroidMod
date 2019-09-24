using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod;
using MetroidMod.Projectiles.missilecombo;

namespace MetroidMod.Projectiles.missilecombo
{
	public class StardustComboShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Combo Shot");
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
			
			mProjectile.DustLine(projectile.Center, projectile.velocity, projectile.rotation, 5, 3, 87, 1f);
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, 0, 0, 100, default(Color), 2f);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			
			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width += 48;
			P.height += 48;
			P.position.X = P.position.X - (float)(P.width / 2);
			P.position.Y = P.position.Y - (float)(P.height / 2);
			
			for (int i = 0; i < 25; i++)
			{
				int d = Dust.NewDust(P.position, P.width, P.height, 88, 0f, 0f, 100, default(Color), 5f);
				Main.dust[d].velocity *= 1.4f;
				Main.dust[d].noGravity = true;
				d = Dust.NewDust(P.position, P.width, P.height, 87, 0f, 0f, 100, default(Color), 3f);
				Main.dust[d].velocity *= 1.4f;
				Main.dust[d].noGravity = true;
			}
			
			for(int i = 0; i < 8; i++)
			{
				float angle = ((float)Math.PI/4)*i;
				int num54 = Projectile.NewProjectile(P.Center.X, P.Center.Y, 0f, 0f, mod.ProjectileType("StardustComboDiffusionShot"),P.damage,P.knockBack,P.owner);
				StardustComboDiffusionShot difShot = (StardustComboDiffusionShot)Main.projectile[num54].modProjectile;
				difShot.spin = angle;
			}
			
			float k = 0f;
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType("StardustFrozenTerrain") && Main.projectile[i].owner == P.owner)
				{
					if(k < Main.projectile[i].ai[0])
					{
						k = Main.projectile[i].ai[0];
					}
				}
			}
			
			int x = (int)MathHelper.Clamp(P.Center.X / 16,0,Main.maxTilesX-2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16,0,Main.maxTilesY-2);
			Vector2 pos = new Vector2((float)x*16f + 8f,(float)y*16f + 8f);
			int ft = Projectile.NewProjectile(pos.X, pos.Y, 0f, 0f, mod.ProjectileType("StardustFrozenTerrain"),P.damage,P.knockBack,P.owner);
			Main.projectile[ft].ai[0] = k+1;
			
			Main.PlaySound(SoundLoader.customSoundType, P.Center,mod.GetSoundSlot(SoundType.Custom, "Sounds/IceSpreaderImpactSound"));
			Main.PlaySound(SoundLoader.customSoundType, P.Center,mod.GetSoundSlot(SoundType.Custom, "Sounds/StardustAfterImpactSound"));
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