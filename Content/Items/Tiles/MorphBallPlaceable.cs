using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class MorphBallPlaceable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Placeable Morph Ball");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.MorphBallTile>();
		}
	}
}
