using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class ReserveTank : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reserve Tank");
            Tooltip.SetDefault("Stores a heart picked up when at full health\n" + 
                "Automatically uses the stored heart to save you from death");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ReserveTank");
			item.rare = 2;
			item.value = 1000;
            item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.reserveTanks = 1;
        }
    }
}