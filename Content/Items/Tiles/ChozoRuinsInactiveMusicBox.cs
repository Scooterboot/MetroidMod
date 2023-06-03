using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ChozoRuinsInactiveMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Music Box (Chozo Ruins)");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoRuinsInactiveMusicBox>();
			Item.hasVanityEffects = true;
			Item.width = 24;
			Item.height = 32;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 1000;
			Item.accessory = true;
		}
	}
}
