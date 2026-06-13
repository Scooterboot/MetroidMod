using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class Phazon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon");
			// Tooltip.SetDefault("'Very radioactive.'\n" + "Glows with Phazon energy");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.PhazonTile>());
			Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.Cyan;
		}
	}
}
