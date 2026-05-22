using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class BrinstoneTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Brinstone");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.BrinstoneTile>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
