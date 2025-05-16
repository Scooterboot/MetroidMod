using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class TopElevatorStationItem : ElevatorStationItem
	{
		public override int TileType => ModContent.TileType<TopElevatorStationTile>();
	}
}
