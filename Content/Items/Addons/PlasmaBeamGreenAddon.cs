using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Addons
{
	public class PlasmaBeamGreenAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam (Green)");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				string.Format("[c/FF9696:Power Beam Addon V2]\n") +
				"Slot Type: Primary B\n" +
				"Shots pierce enemies\n" +
				string.Format("[c/78BE78:+100% damage]\n") +
				string.Format("[c/BE7878:+75% overheat use]\n") +
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.PlasmaBeamGreenTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 4;
			mItem.addonDmg = 1f;
			mItem.addonHeat = 0.75f;
			mItem.addonSpeed = -0.15f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.UnknownPlasmaBeam>(1)
				.AddRecipeGroup(MetroidModPorted.T3HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.CursedFlame, 10)
				.AddIngredient(ItemID.SoulofLight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
