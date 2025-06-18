using MetroidMod.Content.Items.Miscellaneous;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorStationItem : ModItem
	{
		public virtual int TileType => ModContent.TileType<ElevatorStationTile>();
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType);
			Item.rare = ItemRarityID.LightRed;
			Item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<ChoziteBar>(8)
				.AddIngredient(ItemID.Wire, 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
