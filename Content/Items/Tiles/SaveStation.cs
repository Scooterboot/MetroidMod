using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class SaveStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Save Station");
			Tooltip.SetDefault("Sets spawn point");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 38;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.SaveStation>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(null, "ChoziteOre")
				.AddRecipeGroup("IronBar", 4)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteOre");
			recipe.AddRecipeGroup("IronBar", 4);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
