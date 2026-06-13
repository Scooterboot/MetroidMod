using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class GoldenTorizoTrophy : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Golden Torizo Trophy");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.GoldenTorizoTrophyTile>());
			Item.width = 30;
			Item.height = 30;
		}
	}
}
