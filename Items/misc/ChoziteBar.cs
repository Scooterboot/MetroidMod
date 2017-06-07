using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
	public class ChoziteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Bar");
			Tooltip.SetDefault("'A durable metal made from Chozite Ore'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 16;
			item.height = 16;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ChoziteBarTile");
			item.value = 100;
			item.rare = 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteOre", 4);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}