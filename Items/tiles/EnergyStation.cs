using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class EnergyStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Station");
			Tooltip.SetDefault("Right click the station while standing next to it to recharge your life");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("EnergyStation");
		}
	}
}
