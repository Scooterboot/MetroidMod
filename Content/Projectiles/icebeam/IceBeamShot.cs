using System.IO;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.icebeam
{
	public class IceBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Beam Shot");
		}
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && (player.HeldItem.type == ModContent.ItemType<PowerBeam>() ||player.HeldItem.type == ModContent.ItemType<ArmCannon>()))
			{
				if (player.HeldItem.ModItem is PowerBeam hold)
				{
					shot = hold.shotEffect.ToString();
				}
				else if (player.HeldItem.ModItem is ArmCannon hold2)
				{
					shot = hold2.shotEffect.ToString();
				}
			}
			if (shot.Contains("wave"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 3;
			}
			if (shot.Contains("nova"))
			{
				Projectile.penetrate = 11;
				Projectile.maxPenetrate = 11;
			}
			if (shot.Contains("solar"))
			{
				Projectile.penetrate = 16;
				Projectile.maxPenetrate = 16;
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1.5f;
		}

		public override void AI()
		{
			if (shot.Contains("wave"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
			}
			Color color = MetroidMod.iceColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;

				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 59);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.penetrate);
			writer.Write(Projectile.maxPenetrate);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.penetrate = (int)reader.ReadInt32();
			Projectile.maxPenetrate = (int)reader.ReadInt32();
		}
	}
}
