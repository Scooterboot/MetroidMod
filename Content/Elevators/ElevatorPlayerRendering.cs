using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorPlayerRendering : ModSystem
	{
		private readonly List<Player> _elevatingPlayersDrawBehindBlocks = [];
		/*
		public override void Load()
		{
			IL_Main.DoDraw += il =>
			{
				ILCursor c = new(il);
				c.GotoNext(MoveType.After, i => i.MatchCall(GetPrivateMethod<Main>("RefreshPlayerDrawOrder")));
				c.EmitLdarg0();
				c.EmitDelegate((Main main) =>
				{
					_elevatingPlayersDrawBehindBlocks.Clear();
					MovePlayersToElevatingList((List<Player>)GetPrivateField(main, "_playersThatDrawBehindNPCs"));
					MovePlayersToElevatingList((List<Player>)GetPrivateField(main, "_playersThatDrawAfterProjectiles"));
				});
			};

			IL_Main.DoDraw_WallsTilesNPCs += il =>
			{
				ILCursor c = new(il);
				c.GotoNext(MoveType.Before, i => i.MatchCall(GetPrivateMethod<Main>("DoDraw_Tiles_Solid")));
				c.EmitDelegate(() =>
				{
					ElevatorPlatformDrawing epd = ModContent.GetInstance<ElevatorPlatformDrawing>();
					
					SpriteBatch sb = Main.spriteBatch;

					sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					epd.DrawIdlePlatforms();
					foreach (Player player in _elevatingPlayersDrawBehindBlocks)
					{
						epd.DrawPlayerPlatform(player);
					}

					sb.End();

					// A call is missing here for "Potion of Return", is it too niche to include it yet?
					Main.PlayerRenderer.DrawPlayers(Main.Camera, _elevatingPlayersDrawBehindBlocks);
				});
			};
		}
		*/
		private void MovePlayersToElevatingList(List<Player> players)
		{
			_elevatingPlayersDrawBehindBlocks.AddRange(players.RemoveWhere(player => player.GetModPlayer<ElevatorPlayer>().InElevator));
		}

		private MethodInfo GetPrivateMethod<T>(string methodName)
		{
			return typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
		}

		private object GetPrivateField<T>(T instance, string fieldName)
		{
			return typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
		}
	}
}
