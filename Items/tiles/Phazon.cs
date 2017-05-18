using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class Phazon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Phazon";
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.toolTip = "'Very radioactive.'\n" + "Glows with Phazon energy";
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PhazonTile");
		}


	}
}