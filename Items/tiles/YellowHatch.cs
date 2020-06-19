using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tiles
{
	public class YellowHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Hatch");
			Tooltip.SetDefault("Opens when hit with a power bomb\n" + "Right click to place vertically");
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
			item.createTile = mod.TileType("YellowHatch");
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.createTile = mod.TileType("YellowHatchVertical");
			}
			else
			{
				item.createTile = mod.TileType("YellowHatch");
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Topaz);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
}