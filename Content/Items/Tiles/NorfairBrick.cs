using MetroidMod.Content.Items.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class NorfairBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Norfair Brick");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.NorfairBrick>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient(ItemID.AshBlock, 3)
				.AddIngredient(ItemID.HellstoneBrick, 1)
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe()
				.AddIngredient<NorfairBrickWall>(4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
