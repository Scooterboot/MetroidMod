using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles2.Butter.Item
{
	public class MetalWorkbenchItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Metal Workbench");

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
			Item.createTile = ModContent.TileType<Content.Tiles2.Butter.MetalWorkbench>();//refers to the tile!
		}
		public override void AddRecipes()
		{

			CreateRecipe(1)
				.AddIngredient(Mod, "MetalBlock", 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
