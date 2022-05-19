using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using MetroidModPorted;
using static MetroidModPorted.MetroidModPorted;

namespace ExampleMetroidAddonMod.Content.MorphBallAddons
{
	// This is an example of a morph ball bomb addon. It's special thing
	// is that it releases bees on impact and on detonation.
	public class ExampleBomb : ModMBWeapon
	{
		// Full path to the bomb projectile texture, including mod name.
		// Do not include file extensions.
		public override string ProjectileTexture => $"{Mod.Name}/Content/MorphBallAddons/ExampleBombProjectile";

		// Full path to the addon item texture, including mod name. Do not
		// include file extensions.
		public override string ItemTexture => $"{Mod.Name}/Content/MorphBallAddons/ExampleBombItem";

		// Full path to the addon tile texture, including mod name. Do not
		// include file extensions.
		public override string TileTexture => $"{Mod.Name}/Content/MorphBallAddons/ExampleBombTile";

		// If this is set to true, the addon will not be equippable in the slot.
		public override bool AddOnlyAddonItem => false;

		// In a nutshell, this asks if the addon can be found on chozo statues
		// throughout the world.
		public override bool CanGenerateOnChozoStatue(Tile tile)
		{
			return true;
		}

		public override void SetStaticDefaults()
		{
			// Set the name of the item.
			DisplayName.SetDefault("Example Morph Ball Bombs");

			// Set the name of the bomb projectile, pre-explosion.
			ModProjectile.DisplayName.SetDefault("Example Morph Ball Bomb");

			// Set the tooltip of the item.
			Tooltip.SetDefault("-Right Click to set off a bomb\n" +
			"This is an example morph ball weapon addon.\n" +
			"Explodes into a swarm of friendly bees.");

			// In a nutshell, this asks whether or not to add the word 'Addon' to the
			// end of the item name. If this is true, it will not add the word to the
			// end of the name. If it is false, it will add the word to the item name.
			// It is false by default.
			ItemNameLiteral = true;
		}

		// This is effectively the same as ModItem.SetDefaults(), with a few exceptions.
		// The useStyle, useTurn, useAnimation, useTime, vanity, autoReuse, consumable,
		// and createTile values are all locked into place. The item's width and height
		// are dependent of the item texture's width and height.
		public override void SetItemDefaults(Item item)
		{
			// See ExampleMod items' SetDefaults() to know what these mean.
			item.damage = 20;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
		}

		// This is called after the bomb projectile explodes, but before the dusts are
		// emitted. Use this to set the dust types, dust scales, and even summon
		// projectiles. This example spawns bees.
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

		// This is called when the bomb projectile hits an npc. Use this to summon
		// projectiles, if you choose.
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

		// This just creates a recipe for the addon. It is essentially the same as
		// ModItem.AddRecipes(), which is essentially the same as Mod.AddRecipes().
		// Note: automatically, the addon is added to a recipe group before this
		// hook is called.
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
