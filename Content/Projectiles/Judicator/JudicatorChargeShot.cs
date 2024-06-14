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
	public class JudicatorChargeShot : MProjectile
	{
		//todo: add balance for luminite
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Judicator Charge Shot");
		}
		private int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		private int yeet = 1;
		private Vector2 move;
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			if (source is EntitySource_Parent parent && parent.Entity is Player player && player.HeldItem.type == ModContent.ItemType<PowerBeam>())
			{
				if (player.HeldItem.ModItem is PowerBeam hold)
				{
					shot = hold.shotEffect.ToString();
				}
			}
			if (shot.Contains("green"))
			{
				Projectile.penetrate = 6;
				Projectile.maxPenetrate = 6;
				yeet = 6;
			}
			if (shot.Contains("nova"))
			{
				Projectile.penetrate = 8;
				Projectile.maxPenetrate = 8;
				yeet = 8;
			}
			if (shot.Contains("solar"))
			{
				Projectile.penetrate = 12;
				Projectile.maxPenetrate = 12;
				yeet = 12;
			}
			Projectile.timeLeft = Luminite ? 60 : 40;
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;//32
			Projectile.height = 16;//20
			Projectile.scale = 1f;
			Projectile.timeLeft = Luminite ? 60 : 40;
			//Projectile.extraUpdates = 2;
			base.SetDefaults();
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
			if (Projectile.timeLeft == (Luminite ? 60 : 40)) //shadowfreeze
			{
				move = Projectile.velocity;
				Projectile.penetrate = -1;
				if (shot.Contains("wave") || shot.Contains("nebula"))
				{
					Projectile.tileCollide = false;
				}
				MProjectile meep = mProjectile;
				Projectile.velocity.Normalize();
				int widthbonus = Math.Abs((int)Projectile.velocity.X * 16);
				int heightbonus = Math.Abs((int)Projectile.velocity.X * 16);
				Projectile.width *= widthbonus + GetDepth(meep);
				Projectile.height *= heightbonus + GetDepth(meep);
			}
			else
			{
				Projectile.penetrate = yeet;
				Projectile.velocity = move;
				Projectile.width = 16;
				Projectile.height = 16;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorFreeze, Projectile.position);
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300);
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
			writer.Write(yeet);
			writer.WriteVector2(move);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.penetrate =	reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
			yeet = reader.ReadInt32();
			move = reader.ReadVector2();
			base.ReceiveExtraAI(reader);
		}
	}
}
