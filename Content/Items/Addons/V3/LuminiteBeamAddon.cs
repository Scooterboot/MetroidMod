using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Addons.V3
{
	public class LuminiteBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V3]\n") +
			"Slot Type: Charge\n" +
			"Adds Charge Effect\n" + 
			"~Charge by holding click\n" + 
			"~Charge shots deal x5 damage, but overheat x3 the normal use");

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.LuminiteBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonChargeDmg = 5;
			mItem.addonChargeHeat = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddIngredient(ItemID.FragmentNebula, 2)
				.AddIngredient(ItemID.FragmentSolar, 2)
				.AddIngredient(ItemID.FragmentVortex, 2)
				.AddIngredient(ItemID.FragmentStardust, 2)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
