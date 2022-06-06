using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Addons.V2
{
	public class WideBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wide Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V2]\n") +
				"Slot Type: Primary A\n" +
				"Beam fires 3 shots at once, effectively tripling its damage\n" +
				string.Format("[c/78BE78:+100% damage]\n") +
				string.Format("[c/BE7878:+75% overheat use]\n") +
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.WideBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 3;
			mItem.addonDmg = 1f;
			mItem.addonHeat = 0.75f;
			mItem.addonSpeed = 0.15f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<SpazerAddon>(1)
				.AddIngredient(ItemID.HallowedBar, 8)
				.AddIngredient(ItemID.SoulofFlight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
