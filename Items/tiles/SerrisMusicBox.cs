using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class SerrisMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Vs. Serris Remix)");
			Tooltip.SetDefault("Remix by Skyre Ventes");
		}
		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("SerrisMusicBox");
			item.width = 24;
			item.height = 24;
			item.rare = 8;
			item.value = 1000;
			item.accessory = true;
		}
	}
}
