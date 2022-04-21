using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class RedHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Hatch");
			Tooltip.SetDefault("Opens when hit with a Missile\n" + "Right click to place vertically");
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
			Item.createTile = ModContent.TileType<Content.Tiles.Hatch.RedHatch>();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.RedHatchVertical>();
			}
			else
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.RedHatch>();
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Ruby)
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Ruby);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();*/
		}
	}
}
