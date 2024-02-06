using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class FrozenCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supercooled Plasma Core");
			// Tooltip.SetDefault("'Strange energy core capable of producing supercooled plasma'");

			Item.ResearchUnlockCount = 5;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 18;
			Item.height = 16;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SpectreBar, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
