using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class EnergyTank : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Tank");

			SacrificeTotal = 10;
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.EnergyTank>();
			Item.rare = ItemRarityID.Green;
			Item.value = 1000;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(null, "EnergyShard", 4)
				.AddIngredient(null, "ChoziteBar", 1)
				.AddIngredient(ItemID.DemoniteBar, 1)
				.AddIngredient(ItemID.ShadowScale, 10)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EnergyShard", 4);
			recipe.AddIngredient(null, "ChoziteBar", 1);
			recipe.AddIngredient(ItemID.DemoniteBar, 1);
			recipe.AddIngredient(ItemID.ShadowScale, 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/

			CreateRecipe(1)
				.AddIngredient(null, "EnergyShard", 4)
				.AddIngredient(null, "ChoziteBar", 1)
				.AddIngredient(ItemID.CrimtaneBar, 1)
				.AddIngredient(ItemID.TissueSample, 10)
				.AddTile(TileID.Anvils)
				.Register();
			/*recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EnergyShard", 4);
			recipe.AddIngredient(null, "ChoziteBar", 1);
			recipe.AddIngredient(ItemID.CrimtaneBar, 1);
			recipe.AddIngredient(ItemID.TissueSample, 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
