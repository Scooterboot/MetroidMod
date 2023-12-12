using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class RedBrinstone : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Brinstone");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.RedBrinstone>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(25)
				.AddIngredient(ItemID.RedBrick, 25)
				.AddIngredient(ItemID.Ruby, 1)
				.AddTile(TileID.Furnaces)
				.Register();
			CreateRecipe(25)
				.AddIngredient(ItemID.RedStucco, 25)
				.AddIngredient(ItemID.Ruby, 1)
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}
}
