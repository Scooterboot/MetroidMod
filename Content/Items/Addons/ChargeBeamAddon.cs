using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons
{
	public class ChargeBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Beam");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
			"Slot Type: Charge\n" +
			"Adds Charge Effect\n" + 
			"~Charge by holding click\n" + 
			"~Charge shots deal x3 damage, but overheat x2 the normal use");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.ChargeBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonChargeDmg = 3;
			mItem.addonChargeHeat = 2;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.ManaCrystal, 1)
				.AddIngredient(ItemID.FallenStar, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
