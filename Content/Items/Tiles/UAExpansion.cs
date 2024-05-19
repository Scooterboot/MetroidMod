using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Tiles.ItemTile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class UAExpansion : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Missile Expansion");
			/* Tooltip.SetDefault("A Missile Expansion\n" +
				"Increase maximum Missiles by 5 with each expansion slotted in\n" +
				"Stack it up to 50 expansions for +250 maximum Missiles"); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EnergyTank>();
			Item.ResearchUnlockCount = 12;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 12;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 500000;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<UAExpansionTile>();

			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 5;
		}
	}
}
