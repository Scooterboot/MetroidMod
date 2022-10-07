using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.V2
{
	public class WaveBeamV2Addon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam V2");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V2]\n") +
				"Slot Type: Utility\n" +
				"Shots penetrate terrain by an extended depth\n" +
				string.Format("[c/78BE78:+125% damage]\n") +
				string.Format("[c/BE7878:+75% overheat use]"));

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.WaveBeamV2Tile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 2;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageWaveBeamV2;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatWaveBeamV2;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedWaveBeamV2;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<WaveBeamAddon>(1)
				.AddIngredient(ItemID.HallowedBar, 8)
				.AddIngredient(ItemID.SoulofMight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
