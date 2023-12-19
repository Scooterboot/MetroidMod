using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using MetroidMod.Content.Items.Weapons;
using Terraria.GameContent;
using Terraria.DataStructures;
using System.IO;

namespace MetroidMod.Content.Projectiles.Judicator
{
	public class JudicatorChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Judicator Charge Shot");
		}
		private int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		public override void OnSpawn(IEntitySource source)
		{
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
			Projectile.width = 16;//32
			Projectile.height = 16;//20
			Projectile.scale = 1f;
			Projectile.timeLeft = 60;	
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
			if (Projectile.timeLeft == 1) //shadowfreeze
			{
				if(shot.Contains("wave") || shot.Contains("nebula"))
				{
					Projectile.tileCollide = false;
				}
				MProjectile meep = mProjectile;
				int widthbonus = Math.Abs(Projectile.direction * Projectile.width / Projectile.width);
				int heightbonus = Math.Abs(Projectile.direction * Projectile.height / Projectile.height);
				Projectile.width += widthbonus + GetDepth(meep) * 16;
				Projectile.height += heightbonus + GetDepth(meep) * 16;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorFreeze, Projectile.position);
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300);
			target.AddBuff(44, 300);
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
			Projectile.penetrate = (int)reader.ReadSingle();
			Projectile.maxPenetrate = (int)reader.ReadSingle();
		}
	}
}
