using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class GripLedge : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Grip Ledge");
			// Tooltip.SetDefault("Cannot be stood on. \nCan be gripped to using Power Grip \nCan be toggle on or off with wire");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.GripLedge>();
			Item.rare = ItemRarityID.Green;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(99)
				.AddIngredient(null, "ChoziteBar", 4)
				.AddIngredient(ItemID.Emerald, 3)
				.AddIngredient(ItemID.Topaz, 2)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "ChoziteBar", 4);
			recipe.AddIngredient(ItemID.Emerald, 3);
			recipe.AddIngredient(ItemID.Topaz, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 99);
			recipe.AddRecipe();*/
		}
	}
}
