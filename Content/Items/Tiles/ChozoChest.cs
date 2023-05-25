using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ChozoChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozodian Chest");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 9999;
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
				.AddIngredient<ChoziteOre>(8)
				.AddRecipeGroup("IronBar", 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
