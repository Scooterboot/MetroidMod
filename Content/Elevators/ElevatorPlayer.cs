using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorPlayer : ModPlayer
	{
		private ElevatorRide? _currentRide;
		private Vector2 lastMovedPosition;

		/// <summary>
		/// Whether the player is currently riding an elevator.
		/// </summary>
		public bool InElevator => _currentRide != null;

		public bool IsUsingElevator(Elevator elevator)
		{
			if (_currentRide is not ElevatorRide ride) return false;
			return ride.Start == elevator || ride.End == elevator;
		}

		public override void PreUpdate()
		{
			TryLeaveInvalidElevator();
		}

		public override void PreUpdateMovement()
		{
			TryRideElevator();
			PerformElevatorMovement();
		}

		public override bool CanUseItem(Item item)
		{
			return !InElevator;
		}

		private void TryLeaveInvalidElevator()
		{
			if (_currentRide is ElevatorRide ride)
			{
				bool elevatorBroken = !(ride.Start.IsValid && ride.End.IsValid);
				bool leftElevator = Player.position != lastMovedPosition;

				if (elevatorBroken || leftElevator)
				{
					_currentRide = null;
				}
			}
		}

		private void TryRideElevator()
		{
			if (_currentRide != null) return;

			bool goUp = Player.controlUp;
			bool goDown = Player.controlDown;
			
			int direction = (goUp ? -1 : 0) + (goDown ? 1 : 0);
			if (direction == 0) return;
			if (GetElevatorUnderPlayer() is not Elevator start) return;
			if (GetTargetElevator(start, direction) is not Elevator end) return;
			_currentRide = new(start, end);
			Player.Center = new Vector2(start.ArrivalPosition.X, Player.Center.Y);
		}

		private void PerformElevatorMovement()
		{
			if (_currentRide is not ElevatorRide ride) return;

			float current = Player.Bottom.Y;
			float target = ride.End.ArrivalPosition.Y;
			float speed = 3f;

			float displacement = Approach(current, target, speed) - current;
			if (displacement == 0)
			{
				_currentRide = null;
				return;
			}

			Player.position.Y += displacement;
			Player.velocity = Vector2.Zero;
			lastMovedPosition = Player.position;
		}

		private static float Approach(float current, float target, float speed)
		{
			float displacement = target - current;
			float direction = Math.Sign(displacement);
			displacement = Math.Abs(displacement);
			displacement = Math.Min(displacement, speed);
			return current + displacement * direction;
		}

		private Elevator? GetElevatorUnderPlayer()
		{
			return Elevator.Find(Player.Bottom.ToTileCoordinates());
		}

		private static Elevator? GetTargetElevator(Elevator startElevator, int yDirection)
		{
			Point current = startElevator.Origin;

			while (true)
			{
				current.Y += yDirection;

				if (!WorldGen.InWorld(current.X, current.Y)) break;
				if(Elevator.Find(current) is not Elevator elevator) continue;
				if (elevator == startElevator) continue;
				if (elevator.Origin.X != startElevator.Origin.X) continue;

				return elevator;
			}

			return null;
		}
	}

	internal record struct ElevatorRide(Elevator Start, Elevator End);
}
