using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class BlueHatch : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Blue Hatch";
			item.width = 54;
			item.height = 48;
			item.maxStack = 999;
			item.toolTip = "Opens when hit with any projectile";
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("BlueHatch");
		}
public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Sapphire");
			recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}

	}
}