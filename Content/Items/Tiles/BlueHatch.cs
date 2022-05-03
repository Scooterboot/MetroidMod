using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class BlueHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Hatch");
			Tooltip.SetDefault("Opens when hit with any projectile or right clicked\n" + "Right click to place vertically");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 36;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.Hatch.BlueHatch>();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.BlueHatchVertical>();
			}
			else
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.BlueHatch>();
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Sapphire)
				.AddRecipeGroup("IronBar", 5)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.AddRecipeGroup("IronBar", 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();*/
		}
	}
}
