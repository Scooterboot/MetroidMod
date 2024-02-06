using MetroidMod.Common.GlobalItems;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons.V3
{
	public class SolarBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Beam");
			/* Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V3]\n") +
				"Slot Type: Primary B\n" + 
				"Shots pierce enemies\n" + 
				"Shots set enemies ablaze with the Daybroken debuff\n" + 
				string.Format("[c/78BE78:+300% damage]\n") +
				string.Format("[c/BE7878:+150% overheat use]\n") +
				string.Format("[c/BE7878:-15% speed]")); */

			Item.ResearchUnlockCount = 1;
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.SolarBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 4;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageSolarBeam;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatSolarBeam;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedSolarBeam;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentSolar, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
