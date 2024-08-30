using System;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items.Miscellaneous;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class OrbBomb : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 1;
		}

		public override void OnKill(int timeLeft)
		{
			if (Main.rand.NextBool(100))
			{
				Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, ItemID.Heart);
			}
			if (Main.rand.NextBool(100))
			{
				Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, ItemID.Star);
			}
			if (Main.rand.NextBool(100))
			{
				Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, ModContent.ItemType< MissilePickup>());
			}
			if (Main.rand.NextBool(100))
			{
				Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, ModContent.ItemType<UAPickup>());
			}
			for (int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 30, 0f, -(Main.rand.Next(4) / 2), 100, Color.White, 1.5f);
				Main.dust[dust].noGravity = true;
			}
			if (Projectile.timeLeft <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, -(Main.rand.Next(3) / 2), 100, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
			Projectile.width += 40;
			Projectile.height += 40;
			Projectile.scale *= 2;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
			foreach (NPC who in Main.ActiveNPCs) //this is laggy and inneficient, probably
			{
				NPC npc = Main.npc[who.whoAmI];
				if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && Projectile.Hitbox.Intersects(who.Hitbox) && !npc.justHit && !npc.dontTakeDamage && !npc.friendly)
				{
					npc.SimpleStrikeNPC(Projectile.damage, Projectile.direction, Main.rand.NextFloat() <= Main.player[Projectile.owner].GetCritChance<MagicDamageClass>() / 100f, Projectile.knockBack, ModContent.GetInstance<HunterDamageClass>(), true, Main.player[Projectile.owner].luck);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
