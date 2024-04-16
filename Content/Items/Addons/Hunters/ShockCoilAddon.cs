using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class ShockCoilAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("ShockCoil");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Fires a short range electric charge that heals and restores energy when fully charged\n" +
				"Charges on enemy damage and overheats when fully charged\n" +
				//string.Format("[c/78BE78:+10% damage]\n") +
				string.Format("[c/BE7878:Cannot pierce enemies]\n") +
				string.Format("[c/BE7878:Cannot pseudo-screw]\n") +
				string.Format("[c/BE7878:Probably still bugged]\n")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<BattleHammerAddon>();

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.ShockCoilTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.beamSlotType = BeamChangeSlotID.ShockCoil;
			mItem.addonUACost = 400f / 60f;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageShockCoil;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.heatShockCoil;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.Wire, 30)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddIngredient(ItemID.Buggy, 1)
				//.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 8)
				.AddTile(TileID.Hellforge)
				.Register();
		}
	}
}
