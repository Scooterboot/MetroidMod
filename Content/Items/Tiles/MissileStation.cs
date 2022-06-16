using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class MissileStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Station");
			Tooltip.SetDefault("Right click the station while standing next to it to recharge your missiles");

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
			Item.createTile = ModContent.TileType<Content.Tiles.MissileStation>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(null, "MissileExpansion")
				.AddRecipeGroup("IronBar", 5)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MissileExpansion");
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
