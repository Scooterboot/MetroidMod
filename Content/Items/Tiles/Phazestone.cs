using MetroidMod.Content.Items.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class Phazestone : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Brick");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.PhazestoneTile>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient<Phazon>(1)
				.AddIngredient(ItemID.StoneBlock, 5)
				.AddTile(TileID.Furnaces)
				.Register();

			CreateRecipe()
				.AddIngredient<PhazestoneWall>(4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
