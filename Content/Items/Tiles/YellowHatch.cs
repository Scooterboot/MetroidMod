using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class YellowHatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Hatch");
			Tooltip.SetDefault("Opens when hit with a power bomb\n" + "Right click to place vertically");

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
			Item.createTile = ModContent.TileType<Content.Tiles.Hatch.YellowHatch>();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.YellowHatchVertical>();
			}
			else
			{
				Item.createTile = ModContent.TileType<Content.Tiles.Hatch.YellowHatch>();
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient(ItemID.Topaz)
				.AddIngredient(ItemID.LihzahrdBrick, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Topaz);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();*/
		}
	}
}
