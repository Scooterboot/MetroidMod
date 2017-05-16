using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class ChargeBeamAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Charge Beam";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"Adds Charge Effect\n" + 
			"~Charge by holding click\n" + 
			"~Charge shots deal x3 damage, but overheat x2 the normal use\n" + 
			"Primary A and B combinations are still impossible";
			item.value = 2500;
			item.rare = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Mana Crystal");
            recipe.AddIngredient("Fallen Star", 2);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
