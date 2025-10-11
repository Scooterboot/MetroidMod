using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	/// <summary>
	/// Allows getting information about a placed elevator in the world.
	/// </summary>
	internal readonly record struct Elevator
	{
		private Elevator(Point origin)
		{
			Origin = origin;
		}

		/// <summary>
		/// Top left corner of the elevator multitile, in tile coordinates.
		/// </summary>
		public Point Origin { get; }

		/// <summary>
		/// The world position a player stands on when using the elevator.
		/// </summary>
		public Vector2 ArrivalPosition => (Origin + new Point(2, 0)).ToWorldCoordinates(0, 0);

		/// <summary>
		/// Whether this instance refers to a valid elevator in the world.
		/// </summary>
		public bool IsValid
		{
			get {
				if (!IsElevatorTile(Origin)) return false;

				bool correctFrame = TileUtils.GetTopLeftTileInMultitile(Origin.X, Origin.Y).ToPoint() == Origin;
				if (!correctFrame) return false;

				return true;
			}
		}

		/// <summary>
		/// Whether any player is currently riding this elevator.
		/// </summary>
		public bool IsInUse
		{
			get {
				foreach (Player player in Main.ActivePlayers)
				{
					if (player.GetModPlayer<ElevatorPlayer>().IsUsingElevator(this))
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Get the elevator that contains the specified tile position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Elevator? Find(Point position)
		{
			if (!IsElevatorTile(position))
			{
				return null;
			}

			Point origin = TileUtils.GetTopLeftTileInMultitile(position.X, position.Y).ToPoint();
			return new Elevator(origin);
		}

		private static bool IsElevatorTile(Point position)
		{
			Tile tile = Main.tile[position];
			if (!tile.HasTile) return false;

			bool correctTile = tile.TileType == ModContent.TileType<ElevatorStationTile>() || tile.TileType == ModContent.TileType<TopElevatorStationTile>();
			if (!correctTile) return false;

			return true;
		}
	}
}
