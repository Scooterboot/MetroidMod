using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class WaveBeamAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Wave Beam";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
				"Slot Type: Utility\n" +
				"Shots penetrate terrain by a certain depth\n" +
				"+50% damage\n" +
				"+50% overheat use";

			item.value = 2500;
			item.rare = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Demonite Bar", 5);
            recipe.AddIngredient("Amethyst", 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Crimtane Bar", 5);
            recipe.AddIngredient("Amethyst", 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
