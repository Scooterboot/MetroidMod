using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons
{
	public class SpazerAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer Beam");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Primary A\n" +
				"Beam fires 3 shots at once, effectively tripling its damage\n" +
				string.Format("[c/78BE78:+25% damage]\n") +
				string.Format("[c/BE7878:+50% overheat use]\n") +
				string.Format("[c/78BE78:+15% speed]"));

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.SpazerTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 3;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageSpazer;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatSpazer;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedSpazer;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.Stinger, 12)
				.AddIngredient(ItemID.JungleSpores, 12)
				.AddIngredient(ItemID.Topaz, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
