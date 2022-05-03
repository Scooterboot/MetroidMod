using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidModPorted.Content.Items.Accessories
{
	public class HiJumpBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hi-Jump Boots");
			Tooltip.SetDefault("Increases jump height\n" + 
			"Stacks with other jump height accessories");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.HiJumpBootsTile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(10)
				.AddIngredient<Miscellaneous.EnergyShard>(3)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Emerald, 1)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(null, "EnergyShard", 3);
			recipe.AddIngredient(ItemID.Topaz, 1);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<Common.Players.MPlayer>().hiJumpBoost = true;
		}
	}
}
