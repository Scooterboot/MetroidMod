using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class PurePhazon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Pure Phazon");
			// Tooltip.SetDefault("'Highly concentrated Phazon, in its purest form.");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
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
