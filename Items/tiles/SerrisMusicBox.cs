using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class SerrisMusicBox : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Music Box (Vs. Serris Remix)";
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.toolTip = "Remix by Skyre Ventes";
			item.createTile = mod.TileType("SerrisMusicBox");
			item.width = 24;
			item.height = 24;
			item.rare = 8;
			item.value = 1000;
			item.accessory = true;
		}
	}
}
