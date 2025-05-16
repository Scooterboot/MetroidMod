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

			IL_Player.Update += il =>
			{
				ILCursor c = new(il);
				c.GotoNext(MoveType.Before, i => i.MatchCall(typeof(Collision).GetMethod("LavaCollision", BindingFlags.Static | BindingFlags.Public)));

				ILLabel skipLavaCollisionLabel = null;
				c.GotoPrev(MoveType.After, i => i.MatchBrtrue(out skipLavaCollisionLabel));

				c.EmitLdarg0();
				c.EmitDelegate((Player player) => player.GetModPlayer<ElevatorPlayer>().InElevator);
				c.EmitBrtrue(skipLavaCollisionLabel);

				
				c = new(il);
				c.GotoNext(MoveType.After, i => i.MatchCall(typeof(Collision).GetMethod("WetCollision", BindingFlags.Static | BindingFlags.Public)));

				c.EmitLdarg0();
				c.EmitDelegate((bool anyWet, Player player) =>
				{
					if (anyWet && player.GetModPlayer<ElevatorPlayer>().InElevator)
					{
						Collision.honey = false;
						Collision.shimmer = false;
						return false;
					}

					return anyWet;
				});
			};

			IL_Player.CheckDrowning += il =>
			{
				ILCursor c = new(il);
				ILLabel skipCheck = c.DefineLabel();
				c.EmitLdarg0();
				c.EmitDelegate((Player player) => player.GetModPlayer<ElevatorPlayer>().InElevator);
				c.EmitBrfalse(skipCheck);
				c.EmitRet();
				c.MarkLabel(skipCheck);
			};
		}
	}
}
