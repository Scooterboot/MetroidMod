using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.missileaddons.BeamCombos
{
	public class StardustComboAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Blizzard");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Charge shots create 8 Stardust Dragons on impact that spiral outward, covering terrain in ice which freezes foes\n" + 
			"~Charge shots cost 10 missiles\n" +
			"Frozen enemies will take damage over time, and will spawn a Stardust Guardian when killed.");
		}
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.value = 2500;
			item.rare = 4;
			/*item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("StardustComboAddonTile");*/
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.missileSlotType = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentStardust, 15);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ItemID.Ruby, 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
