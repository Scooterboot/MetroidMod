using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class AQAPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.AQAPlating>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
