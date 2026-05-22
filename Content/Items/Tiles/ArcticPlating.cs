using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ArcticPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Arctic Plating");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.ArcticPlating>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(25)
				.AddIngredient(ItemID.SnowBrick, 25)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
