using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class ChozoBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Brick Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.ChozoBrickWall>());
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient<Tiles.ChozoBrick>(1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
