using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class EnergyStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Station");
			Tooltip.SetDefault("Right click the station while standing next to it to recharge your life\n"+
			"Unobtainable. Intended for adventure map creators.");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.EnergyStation>();
		}
	}
}
