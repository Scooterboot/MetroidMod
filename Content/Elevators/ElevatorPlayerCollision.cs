using System.Reflection;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorPlayerCollision : ModSystem
	{
		public override void Load()
		{
			IL_Player.Update += il =>
			{
				ILCursor c = new(il);
				c.GotoNext(MoveType.Before, i => i.MatchCall(typeof(Player).GetMethod("SlopeDownMovement")));

				ILLabel skipCollisionLabel = null;
				c.GotoPrev(MoveType.After, i => i.MatchBrtrue(out skipCollisionLabel));

				c.EmitLdarg0();
				c.EmitDelegate((Player player) => player.GetModPlayer<ElevatorPlayer>().InElevator);
				c.EmitBrtrue(skipCollisionLabel);
			};

			IL_Player.Update += il =>
			{
				ILCursor c = new(il);
				c.GotoNext(MoveType.Before, i => i.MatchCall(typeof(Player).GetMethod("GetHurtTile", BindingFlags.Instance | BindingFlags.NonPublic)));

				ILLabel skipHurtTileLabel = null;
				c.GotoPrev(MoveType.After, i => i.MatchBrtrue(out skipHurtTileLabel));

				c.EmitLdarg0();
				c.EmitDelegate((Player player) => player.GetModPlayer<ElevatorPlayer>().InElevator);
				c.EmitBrtrue(skipHurtTileLabel);
			};
		}
	}
}
