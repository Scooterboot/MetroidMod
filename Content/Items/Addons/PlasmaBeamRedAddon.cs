using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Items.Miscellaneous;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons
{
	public class PlasmaBeamRedAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam (Red)");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				string.Format("[c/FF9696:Power Beam Addon V2]\n") +
				"Slot Type: Primary B\n" +
				"Shots set enemies ablaze with Fire, or Frost Burns them if Ice Beam is installed\n" +
				string.Format("[c/78BE78:+100% damage]\n") +
				string.Format("[c/BE7878:+75% overheat use]\n") +
				string.Format("[c/BE7878:-15% speed]")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PlasmaBeamGreenAddon>();
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 70000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.PlasmaBeamRedTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 4;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damagePlasmaBeamRed;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatPlasmaBeamRed;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedPlasmaBeamRed;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<ChoziteBar>(3)
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ItemID.Ruby, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
