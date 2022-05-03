using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class NovaWorkTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Worktable");
			Tooltip.SetDefault("Use for crafting Phazon based items");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.NovaWorkTableTile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 10)
				.AddIngredient(ItemID.CursedFlame, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddIngredient(ItemID.CursedFlame, 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/

			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 10)
				.AddIngredient(ItemID.Ichor, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddIngredient(ItemID.Ichor, 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
