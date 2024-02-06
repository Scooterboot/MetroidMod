using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.spazer
{
	public class SpazerChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spazer Charge Shot");
		}
		Color color = MetroidMod.powColor;
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && player.HeldItem.type == ModContent.ItemType<PowerBeam>())
			{
				if (player.HeldItem.ModItem is PowerBeam hold)
				{
					shot = hold.shotEffect.ToString();
				}
			}
			base.OnSpawn(source);
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Main.projFrames[Projectile.type] = 2;

			mProjectile.amplitude = 10f * Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 8;
		}

		int dustType = 64;
		public override void AI()
		{
			if (shot.Contains("ice") || Projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			else if (shot.Contains("wave") || Projectile.Name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}

			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}

	public class WaveSpazerChargeShot : SpazerChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Charge Shot";
			Projectile.tileCollide = false;

			mProjectile.amplitude = 16f * Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
		}
	}

	public class IceSpazerChargeShot : SpazerChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Charge Shot";
		}
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			if (shot.Contains("wave"))
			{
				Projectile.tileCollide = false;
				Projectile.Name = "Ice Wave Spazer Charge Shot";
				mProjectile.amplitude = 12f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
			}
		}
	}
}
