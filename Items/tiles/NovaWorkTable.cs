using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class NovaWorkTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Worktable");
			Tooltip.SetDefault("Use for crafting Phazon based items");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 999;
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
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.Emerald, 7);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilverBar, 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.Emerald, 7);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.Emerald, 7);
			recipe.AddIngredient(ItemID.Ichor, 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilverBar, 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddIngredient(ItemID.Emerald, 7);
			recipe.AddIngredient(ItemID.Ichor, 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}