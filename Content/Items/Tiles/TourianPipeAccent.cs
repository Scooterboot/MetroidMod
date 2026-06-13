using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class TourianPipeAccent : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tourian Accent Pipe");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TourianPipeAccent>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
