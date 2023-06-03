using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class PhazonBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon Bar");
			// Tooltip.SetDefault("'Very radioactive.'\n" + "Glows with Phazon energy");
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(20, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			Item.ResearchUnlockCount = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 10000;
			Item.rare = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.PhazonBarTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(null, "Phazon", 5)
				.AddTile(null, "NovaWorkTableTile")
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Phazon", 5);
			recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
