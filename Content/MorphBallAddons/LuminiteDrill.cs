using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class LuminiteDrill : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/LuminiteDrill/LuminiteDrillItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/LuminiteDrill/LuminiteDrillTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Morph Ball Drill");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~225% pickaxe power");
			DrillPower = 225;

			ItemNameLiteral = true;
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 18)
				.AddIngredient(ItemID.FragmentNebula, 12)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 18)
				.AddIngredient(ItemID.FragmentSolar, 12)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 18)
				.AddIngredient(ItemID.FragmentStardust, 12)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.LunarBar, 18)
				.AddIngredient(ItemID.FragmentVortex, 12)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
