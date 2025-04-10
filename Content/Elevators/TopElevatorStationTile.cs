
using Terraria.DataStructures;

namespace MetroidMod.Content.Elevators{
	internal class TopElevatorStationTile : ElevatorStationTile
	{
		public override bool Animated => false;
		public override int Height => 1;
		public override Point16 Origin => new(1, 0);
	}
}
