using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class GreenBrinstarRootsTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Overgrowth Vines");
			// Tooltip.SetDefault("'What happened to Brinstar?'");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.GreenBrinstarRootsTile>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
