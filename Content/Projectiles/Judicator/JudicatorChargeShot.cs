using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MetroidMod.Content.Projectiles.Judicator
{
	public class JudicatorChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Judicator Charge Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.timeLeft = 60;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0) ;
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = Projectile;

			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(P.position, P.width, P.height, 135, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
			}

			var entitySource = Projectile.GetSource_Death();


			int x = (int)MathHelper.Clamp(P.Center.X / 16, 0, Main.maxTilesX - 2);
			int y = (int)MathHelper.Clamp(P.Center.Y / 16, 0, Main.maxTilesY - 2);
			Vector2 pos = new Vector2((float)x * 16f + 8f, (float)y * 16f + 8f);
			int ft = Projectile.NewProjectile(entitySource, pos.X, pos.Y, 0f, 0f, ModContent.ProjectileType<JudicatorFreeze>(), 0, 0f, P.owner);
			Projectile.Damage();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorFreeze, Projectile.position);
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300, true);
			target.AddBuff(44, 300);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public class NovaJudicatorChargeShot : JudicatorChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Nova Judicator Charge Shot";
				Projectile.penetrate = 8;

			}
		}
		public class PlasmaGreenJudicatorChargeShot : JudicatorChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Plasma Green Judicator Charge Shot";
				Projectile.penetrate = 6;

			}
		}
		public class SolarJudicatorChargeShot : JudicatorChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Solar Judicator Charge Shot";
				Projectile.penetrate = 12;

			}
		}
	}
}
