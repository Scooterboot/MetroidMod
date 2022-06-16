using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.MissileAddons.BeamCombos
{
	public class VortexComboAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Storm");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Fires vortexes that orbit the player at full charge\n" + 
			"~Each vortex fires a blast after 2 seconds\n" + 
			"~Releasing the charge makes all vortexes immediately fire\n" + 
			"~Initially costs 10 missiles, then drains 5 missiles per second during use");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = 4;
			/*Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = mod.TileType("VortexComboAddonTile");*/
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 0;
			mItem.addonChargeDmg = 1f;
			mItem.addonMissileCost = 10;
			mItem.addonMissileDrain = 5;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentVortex, 15)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
