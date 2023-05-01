using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class OmegaCannonAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("OmegaCannon");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				string.Format("[c/78BE78:+900% damage]\n") +
				string.Format("[c/BE7878:+1000% overheat use]\n") +
				string.Format("[c/BE7878:Slow as the DMV]\n") +
                string.Format("[c/BE7878:Cannot Pierce walls or enemies]"));

            SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 500000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.OmegaCannonTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonDmg = 100f;
			mItem.addonHeat = 10f;
			mItem.addonSpeed = -.45f;
		}
	

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 15)
                .AddIngredient(ItemID.FragmentVortex, 30)
                .AddIngredient(ItemID.FragmentSolar, 30)
                .AddIngredient(ItemID.FragmentNebula, 30)
                .AddIngredient(ItemID.FragmentStardust, 30)
                .AddIngredient(ItemID.Diamond, 30)
                .AddIngredient<Addons.Hunters.JudicatorAddon>(1)
                .AddIngredient<Addons.Hunters.BattleHammerAddon>(1)
                .AddIngredient<Addons.Hunters.VoltDriverAddon>(1)
                .AddIngredient<Addons.Hunters.MagMaulAddon>(1)
                .AddIngredient<Addons.Hunters.ImperialistAddon>(1)
                .AddIngredient<Addons.Hunters.ShockCoilAddon>(1)
                .AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
