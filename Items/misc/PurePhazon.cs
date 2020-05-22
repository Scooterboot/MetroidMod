using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
    public class PurePhazon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pure Phazon");
			Tooltip.SetDefault("'Highly concentrated Phazon, in its purest form.");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 16;
			item.height = 16;
			item.value = 10000;
			item.rare = 1;
		}
    	/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Phazon", 5);
            recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
}