using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles2.Butter.Item
{
	public class MetalChestItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Metal Chest");

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
			Item.createTile = ModContent.TileType<Content.Tiles2.Butter.MetalChest>();//refers to the tile!
		}
		public override void AddRecipes()
		{

			CreateRecipe(1)
				.AddIngredient(ItemID.IronBar, 2)
				.AddIngredient(Mod, "MetalBlock", 8)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
