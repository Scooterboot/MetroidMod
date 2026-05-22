using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ChozoPillarNatural : ModItem
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Tiles/ChozoPillar";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Pillar (Natural)");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.ChozoPillarNatural>());
			Item.width = 16;
			Item.height = 16;
			ItemID.Sets.DrawUnsafeIndicator[Item.type] = true; //Hey so apparently they just have a thingy to make the unsafe skull show up.    -Z
		}
	}
}
