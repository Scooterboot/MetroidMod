using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Addons.V2
{
	public class NovaBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V2]\n") +
			"Slot Type: Primary B\n" + 
				"Shots pierce enemies\n" + 
				"Shots set enemies ablaze with Cursed Fire, or Frost Burns them if Ice Beam is installed\n" + 
				string.Format("[c/78BE78:+225% damage]\n") +
				string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/BE7878:-15% speed]"));
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.NovaBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 4;
			mItem.addonDmg = 2.25f;
			mItem.addonHeat = 1f;
			mItem.addonSpeed = -0.15f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<PlasmaBeamGreenAddon>()
				.AddIngredient(ItemID.ChlorophyteBar, 8)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.LunarTabletFragment) // This is actually the "Solar Tablet Fragment" found in Lihzahrd chests
				.AddTile(TileID.MythrilAnvil)
				.Register();
			CreateRecipe(1)
				.AddIngredient<PlasmaBeamRedAddon>()
				.AddIngredient(ItemID.ChlorophyteBar, 8)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.LunarTabletFragment)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
