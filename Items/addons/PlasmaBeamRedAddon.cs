using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class PlasmaBeamRedAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Plasma Beam (Red)";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
				"Slot Type: Primary B\n" +
				"Shots set enemies ablaze with Fire, or Frost Burns them if Ice Beam is installed\n" +
				"Deals double damage if the enemy is already on Fire/Frost Burned\n" +
				"+300% damage\n" +
				"+100% overheat use";

			item.value = 2500;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PlasmaBeamRedTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Ichor", 15);
            recipe.AddIngredient("Ruby", 5);
			recipe.AddIngredient("Soul of Light", 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
