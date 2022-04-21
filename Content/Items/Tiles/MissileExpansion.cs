using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
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
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 50;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 2500;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.MissileExpansionTile>();

            //Common.GlobalItems.MGlobalItem mItem = Item.GetGlobalItem<Common.GlobalItems.MGlobalItem>();
            //mItem.missileSlotType = 2;
        }
	}
}
