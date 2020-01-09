using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
    public class NightmareCoreXFragment : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Core-X Fragment");
			Tooltip.SetDefault("Soft and squishy\n" + 
			"Contains gravity altering properties");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 6));
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 16;
			item.height = 16;
			item.value = 10000;
			item.rare = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NightmareCoreX");
			recipe.SetResult(this,20);
			recipe.AddRecipe();
		}
	}
}