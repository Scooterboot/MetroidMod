using System;
using System.IO;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Judicator
{
	public class JudicatorShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Judicator Shot");
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
			Projectile.width = 5; //10
			Projectile.height = 11; //22
			Projectile.scale = 1f;
			//Projectile.penetrate = 1;
			//Projectile.aiStyle = 0;
			Projectile.timeLeft = 90;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}


		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);

				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}

				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}

			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (var i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);
			}
			mProjectile.DustyDeath(Projectile, 135);
			SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);
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
