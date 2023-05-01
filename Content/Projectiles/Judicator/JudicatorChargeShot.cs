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
		public class IceNovaJudicatorChargeShot : JudicatorChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Ice Nova Judicator Charge Shot";
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
		public class IcePlasmaGreenJudicatorChargeShot : JudicatorChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Ice Solar Judicator Charge Shot";
				Projectile.penetrate = 12;

			}
		}
		public class IceSolarJudicatorChargeShot : JudicatorChargeShot
		{
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.Name = "Ice Solar Judicator Charge Shot";
				Projectile.penetrate = 12;

			}
		}
	}
}
