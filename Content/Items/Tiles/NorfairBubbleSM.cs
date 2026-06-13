using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class NorfairBubbleSM : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Classic Bubble");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.NorfairBubbleSM>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(25)
				.AddIngredient(ItemID.Bubble, 25)
				.Register();
			CreateRecipe(1)
				.AddIngredient<NorfairBubbleZM>(1)
				.Register();
		}
	}
}
