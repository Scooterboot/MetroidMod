using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.V3
{
	public class VortexBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V3]\n") +
				"Slot Type: Primary A\n" +
				"Beam fires 5 shots at once\n" +
				string.Format("[c/78BE78:+150% damage]\n") +
				string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/78BE78:+25% speed]"));

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.VortexBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 3;
			mItem.addonDmg = 1.5f;
			mItem.addonHeat = 1f;
			mItem.addonSpeed = 0.25f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentVortex, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
