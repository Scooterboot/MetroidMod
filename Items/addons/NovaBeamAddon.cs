using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class NovaBeamAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Nova Beam";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
			"Slot Type: Primary B\n" + 
				"Shots pierce enemies, hitting them multiple times\n" + 
				"Shots set enemies ablaze with Cursed Fire, or Frost Burns them if Ice Beam is installed\n" + 
				"+500% damage\n" + 
				"+250% overheat use";
			item.value = 2500;
			item.rare = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlasmaBeamGreenAddon");
            recipe.AddIngredient("Chlorophyte Bar", 10);
            recipe.AddIngredient("Emerald", 5);
			recipe.AddIngredient("Soul of Sight", 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlasmaBeamRedAddon");
            recipe.AddIngredient("Chlorophyte Bar", 10);
            recipe.AddIngredient("Emerald", 5);
			recipe.AddIngredient("Soul of Sight", 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
