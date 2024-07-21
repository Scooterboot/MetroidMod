using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
//using MetroidMod;
//using MetroidMod.Content.Projectiles.missilecombo;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class StardustComboShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Combo Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;

			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, 87, 1f);

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 88, 0, 0, 100, default(Color), 2f);
			Main.dust[dust].noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;

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

			var entitySource = Projectile.GetSource_Death();
			for (int i = 0; i < 8; i++)
			{
				float angle = ((float)Math.PI / 4) * i;
				int num54 = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ModContent.ProjectileType<StardustComboDiffusionShot>(), P.damage, P.knockBack, P.owner);
				StardustComboDiffusionShot difShot = (StardustComboDiffusionShot)Main.projectile[num54].ModProjectile;
				difShot.spin = angle;
			}

			float k = 0f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<StardustFrozenTerrain>() && Main.projectile[i].owner == P.owner)
				{
					if (k < Main.projectile[i].ai[0])
					{
						k = Main.projectile[i].ai[0];
					}
				}
			}

			int x = (int)MathHelper.Clamp(P.Center.X / 16, 0, Main.maxTilesX - 2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16, 0, Main.maxTilesY - 2);
			Vector2 pos = new Vector2((float)x * 16f + 8f, (float)y * 16f + 8f);
			int ft = Projectile.NewProjectile(Projectile.GetSource_Death(), pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<StardustFrozenTerrain>(), P.damage, P.knockBack, P.owner);
			Main.projectile[ft].ai[0] = k + 1;

			SoundEngine.PlaySound(Sounds.Items.Weapons.IceSpreaderImpactSound, P.Center);
			SoundEngine.PlaySound(Sounds.Items.Weapons.StardustAfterImpactSound, P.Center);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300, true);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
}
