using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class MissileExpansion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Expansion");
			Tooltip.SetDefault("A Missile Expansion\n" +
				"Increase maximum Missiles by 5 with each expansion slotted in\n" +
				"Stack it up to 50 expansions for +250 maximum Missiles");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 50;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.rare = 4;
			item.value = Item.buyPrice(0,15,0,0);
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("MissileExpansionTile");

            MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
            mItem.missileSlotType = 2;
        }
	}
}