using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class SerrisTrophy : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Serris Trophy";
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SerrisTrophyTile");
		}
	}
}