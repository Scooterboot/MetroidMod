using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class NovaWorkTable : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Nova Worktable";
			item.width = 30;
			item.height = 30;
			item.maxStack = 999;
			item.toolTip = "Use for crafting Phazon based items";
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("NovaWorkTableTile");
		}
public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Tungsten Bar", 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient("Emerald", 7);
			recipe.AddIngredient("Cursed Flame", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient("Silver Bar", 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient("Emerald", 7);
			recipe.AddIngredient("Cursed Flame", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient("Tungsten Bar", 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient("Emerald", 7);
			recipe.AddIngredient("Ichor", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient("Silver Bar", 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient("Emerald", 7);
			recipe.AddIngredient("Ichor", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}