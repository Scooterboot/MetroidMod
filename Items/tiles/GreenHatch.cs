using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class GreenHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Hatch");
			Tooltip.SetDefault("Opens when hit with a Super Missile\n" + "Right click to place vertically");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 36;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("GreenHatch");
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.createTile = mod.TileType("GreenHatchVertical");
			}
			else
			{
				item.createTile = mod.TileType("GreenHatch");
			}
			return base.CanUseItem(player);
		}
public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Emerald);
			recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}

	}
}
