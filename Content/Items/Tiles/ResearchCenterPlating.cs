using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ResearchCenterPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.ResearchCenterPlating>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
