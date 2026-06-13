using MetroidMod.Content.Items.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class TourianPipe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tourian Pipe");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TourianPipe>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<ChoziteOre>(1)
				.AddIngredient(ItemID.TitaniumOre, 1) //I'm making tourian pipes hardmode-exclusive because titanium makes the most sense as the alloy material
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddTile(TileID.AdamantiteForge)
				.Register();

			CreateRecipe(20)
				.AddIngredient<ChoziteOre>(1)
				.AddIngredient(ItemID.AdamantiteOre, 1) //it makes more sense with titanium but rng exists
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddTile(TileID.AdamantiteForge)
				.Register();

			CreateRecipe()
				.AddIngredient<TourianWall>(4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
