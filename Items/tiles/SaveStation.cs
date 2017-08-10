using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class SaveStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Save Station");
			Tooltip.SetDefault("Sets spawn point");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 38;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SaveStation");
		}
public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteOre");
			recipe.AddRecipeGroup("IronBar", 4);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
