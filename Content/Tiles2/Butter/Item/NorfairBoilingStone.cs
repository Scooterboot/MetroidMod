using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles2.Butter.Item
{
	public class NorfairBoilingStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Norfair Boiling Stone");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Content.Tiles2.Butter.NorfairBoilingStone>();//refers to the tile!
		}
		public override void AddRecipes()
		{

			CreateRecipe(20)
				.AddIngredient(ItemID.StoneBlock, 20)
				.AddIngredient(ItemID.Hellstone, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
