using MetroidMod.Common.GlobalItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons
{
	public class WaveBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Beam");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Utility\n" +
				"Shots penetrate terrain by a limited depth\n" +
				string.Format("[c/78BE78:+50% damage]\n") +
				string.Format("[c/BE7878:+50% overheat use]")); */

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.WaveBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 2;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageWaveBeam;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatWaveBeam;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedWaveBeam;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(3)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 8)
				.AddIngredient(ItemID.Amethyst, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
