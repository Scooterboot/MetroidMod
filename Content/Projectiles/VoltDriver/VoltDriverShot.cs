using System;
using System.IO;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.VoltDriver
{
	public class VoltDriverShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Volt Driver Shot");
			Main.projFrames[Projectile.type] = 4;
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
			if (shot.Contains("green"))
			{
				Projectile.penetrate = 6;
				Projectile.maxPenetrate = 6;
			}
			if (shot.Contains("nova"))
			{
				Projectile.penetrate = 8;
				Projectile.maxPenetrate = 8;
			}
			if (shot.Contains("solar"))
			{
				Projectile.penetrate = 12;
				Projectile.maxPenetrate = 12;
			}
			base.OnSpawn(source);
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 12;//22
			Projectile.height = 12; //22
			Projectile.scale = .5f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			//Projectile.extraUpdates = 3;
		}
		public override void AI()
		{
			if (shot.Contains("wave") || shot.Contains("nebula"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 269);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverImpactSound, Projectile.position);
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
			Projectile.penetrate = reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
		}
	}
}
