using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class BlueHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Hatch");
			Tooltip.SetDefault("Opens when hit with any projectile or right clicked");
		}
		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 48;
			item.maxStack = 999;
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
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}

	}
}
