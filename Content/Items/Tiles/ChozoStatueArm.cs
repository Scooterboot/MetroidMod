using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public abstract class ChozoStatueArmBuilder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
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
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.StoneBlock, 50)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
	public class ChozoStatueArm : ChozoStatueArmBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoStatueArm>();
		}
	}
	public class ChozoStatueArm2 : ChozoStatueArmBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoStatueArm2>();
		}
	}
	public class ChozoStatueArm3 : ChozoStatueArmBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoStatueArm3>();
		}
	}
}
