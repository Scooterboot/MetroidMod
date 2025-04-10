using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class TopElevatorStationItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<TopElevatorStationTile>());
		}
	}
}
