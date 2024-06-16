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
		private Vector2 LineStart;
		private Vector2 LineEnd;
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
				/*Projectile.penetrate = 6;
				Projectile.maxPenetrate = 6;*/
				yeet = 6;
			}
			if (shot.Contains("nova"))
			{
				/*Projectile.penetrate = 8;
				Projectile.maxPenetrate = 8;*/
				yeet = 8;
			}
			if (shot.Contains("solar"))
			{
				/*Projectile.penetrate = 12;
				Projectile.maxPenetrate = 12;*/
				yeet = 12;
			}
			Projectile.timeLeft = Luminite ? 60 : 40;
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 20;
			Projectile.scale = 1f;
			Projectile.timeLeft = Luminite ? 60 : 40;
			base.SetDefaults();
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;

			MProjectile meep = mProjectile;
			WaveBehavior(Projectile);
			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
			
			if (Projectile.timeLeft == (Luminite ? 60 : 40)) //shadowfreeze
			{
				Projectile.penetrate = -1;
				Projectile.tileCollide = false;
				/*if (shot.Contains("wave") || shot.Contains("nebula"))
				{
					Projectile.tileCollide = false;
				}*/
				//Projectile.velocity.Normalize();
				//Projectile.Center.Floor();
				//Projectile.width += (int)Math.Abs((Projectile.velocity.Y * GetDepth(meep)) * Projectile.width);
				//Projectile.height += (int)Math.Abs((Projectile.velocity.X * GetDepth(meep)) * Projectile.height);
				//Projectile.Center = Main.player[Projectile.owner].Center;
				LineStart = new (Projectile.position.X + (Projectile.velocity.Y * GetDepth(mProjectile)), Projectile.position.Y + (Projectile.velocity.X * GetDepth(mProjectile) * 16f));
				LineEnd = new (Projectile.position.X - (Projectile.velocity.Y * GetDepth(mProjectile)), Projectile.position.Y - (Projectile.velocity.X * GetDepth(mProjectile) * 16f));
			}
			else
			{
				Projectile.tileCollide = true;
				Projectile.penetrate = yeet;
			}
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float _ = float.NaN;
			if (Projectile.timeLeft == (Luminite ? 60 : 40))
			{
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), LineStart, LineEnd, Projectile.width, ref _);
			}
			return base.Colliding(projHitbox, targetHitbox);
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
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.penetrate =	reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
	}
}
