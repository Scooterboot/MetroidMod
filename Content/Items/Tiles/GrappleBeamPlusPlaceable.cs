using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class GrappleBeamPlusPlaceable : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Placeable Grapple Beam");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.ItemTile.GrappleBeamPlusTile>());/*
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.GrappleBeamTile>();*/
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Tools.GrappleBeamPlus>()
				.Register();
		}
	}
}
