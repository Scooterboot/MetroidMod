using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ItemPedestalTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Item Pedestal");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.ItemPedestalTile>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
