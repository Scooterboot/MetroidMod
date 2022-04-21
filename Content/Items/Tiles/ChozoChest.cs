using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class ChozoChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozodian Chest");
		}
		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoChest>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(null, "ChoziteOre", 8)
				.AddRecipeGroup("IronBar", 2)
				.AddTile(TileID.WorkBenches)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ChoziteOre"), 8);
			recipe.AddIngredient(ItemID.IronBar, 2);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();*/

			/*recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ChoziteOre"), 8);
			recipe.AddIngredient(ItemID.LeadBar, 2);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}