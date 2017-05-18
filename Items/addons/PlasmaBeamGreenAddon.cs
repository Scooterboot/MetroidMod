using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class PlasmaBeamGreenAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Plasma Beam (Green)";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
				"Slot Type: Primary B\n" +
				"Shots pierce enemies, hitting them multiple times\n" +
				"+300% damage\n" +
				"+100% overheat use";

			item.value = 2500;
			item.rare = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Cursed Flame", 15);
            recipe.AddIngredient("Emerald", 5);
			recipe.AddIngredient("Soul of Light", 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
