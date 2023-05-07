using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Content.Walls.ChozoBrickWall>();
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
