using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ItemRoomTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Item Room Block");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.ItemRoomTile>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
