using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class NorfairBubbleZM : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vibrant Bubble");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.NorfairBubbleZM>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(25)
				.AddIngredient(ItemID.Bubble, 25)
				.Register();
			CreateRecipe(1)
				.AddIngredient<NorfairBubbleSM>(1)
				.Register();
		}
	}
}
