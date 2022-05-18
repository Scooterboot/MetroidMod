using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using MetroidModPorted;
using static MetroidModPorted.MetroidModPorted;

namespace ExampleMetroidAddonMod.Content.MorphBallAddons
{
	public class BeeBomb : ModMBWeapon
	{
		public override string ProjectileTexture => $"{Mod.Name}/Content/MorphBallAddons/BeeBombProjectile";

		public override string ItemTexture => $"{Mod.Name}/Content/MorphBallAddons/BeeBombItem";

		public override string TileTexture => $"{Mod.Name}/Content/MorphBallAddons/BeeBombTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bee Morph Ball Bombs");
			Tooltip.SetDefault("-Right Click to set off a bomb\n" +
			"");
			ItemNameLiteral = true;
		}

		public override void SetItemDefaults(Item item)
		{
			item.damage = 20;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.t_Honey;
			dustScale = DustID.Honey;

			int max = Main.rand.Next(10, 20);
			float angle = Main.rand.Next(360 / max);
			for (int i = 0; i < max; i++)
			{
				float rot = (float)(angle + (360f / max * i * (Math.PI / 180)));
				Vector2 vel = rot.ToRotationVector2() * 5f;
				Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, Main.player[Projectile.owner].beeType(), Projectile.damage / max, Projectile.knockBack + 3, Projectile.owner)];
				proj.timeLeft = 60;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int max = 12;
			float angle = Main.rand.Next(360 / max);
			for (int i = 0; i < max; i++)
			{
				float rot = (float)(angle + (360f / max * i * (Math.PI / 180)));
				Vector2 vel = rot.ToRotationVector2() * 5f;
				Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, Main.player[Projectile.owner].beeType(), Projectile.damage / max, Projectile.knockBack + 3, Projectile.owner)];
				proj.timeLeft = 60;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MorphBallBombsRecipeGroupID, 1)
				.AddIngredient(ItemID.BeeWax, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
