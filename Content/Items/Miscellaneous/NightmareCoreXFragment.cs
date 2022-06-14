using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Miscellaneous
{
	public class NightmareCoreXFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Core-X Fragment");
			Tooltip.SetDefault("Soft and squishy\n" + 
			"Contains gravity altering properties");
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			SacrificeTotal = 20;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 16;
			Item.height = 16;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
		}
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<NightmareCoreX>(1)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NightmareCoreX");
			recipe.SetResult(this,20);
			recipe.AddRecipe();*/
		}
	}
}
