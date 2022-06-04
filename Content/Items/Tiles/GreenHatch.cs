using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class GreenHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Hatch");
			Tooltip.SetDefault("Opens when hit with a Super Missile\n" + "Right click to place vertically");

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
			Item.createTile = ModContent.TileType<Content.Tiles.Hatch.GreenHatch>();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.GreenHatchVertical>();
			}
			else
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.GreenHatch>();
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.AdamantiteBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Emerald);
			recipe.AddIngredient(ItemID.AdamantiteBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();*/

			CreateRecipe(20)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.TitaniumBar, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Emerald);
			recipe.AddIngredient(ItemID.TitaniumBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();*/
		}
	}
}
