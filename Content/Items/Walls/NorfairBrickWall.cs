using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class NorfairBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Norfair Brick Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.NorfairBrickWall>());
		}
		public override void AddRecipes()
		{
			CreateRecipe(4)
				.AddIngredient<Tiles.NorfairBrick>(1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
