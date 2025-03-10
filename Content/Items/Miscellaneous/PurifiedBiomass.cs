using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	internal class PurifiedBiomass : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 16;
			Item.height = 16;
			Item.value = 388;
			Item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			CreateRecipe(12)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 10)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 2)
				.AddIngredient(ItemID.PurificationPowder, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
