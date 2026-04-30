using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorPlayerRendering : ModSystem
	{
		private readonly List<Player> _elevatingPlayersDrawBehindBlocks = [];

		public override void PostDrawTiles()
		{
			// In the meantime since we can't call this in the proper place
			// we call it here, TODO please remove this workaround
			// and re-enable the code below once (/IF) MonoMod is fixed!

			Main.spriteBatch.Begin(
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				SamplerState.LinearWrap,
				DepthStencilState.None,
				RasterizerState.CullNone,
				null,
				Matrix.Identity);
			ElevatorPlatformDrawing epd = ModContent.GetInstance<ElevatorPlatformDrawing>();
			epd.DrawIdlePlatforms();
			foreach (Player player in Main.ActivePlayers)
			{
				if (player.GetModPlayer<ElevatorPlayer>().InElevator)
				{
					epd.DrawPlayerPlatform(player);
				}
			}
			Main.spriteBatch.End();
		}

		public override void Load()
		{
			// Fuck it we ball :D
			return; // Maybe remove all this once they fix MonoMod?

			//jopojelly said in a thread this SHOULD help prevent the cellref crash, but it doesn't 100% prevent it :(
			//It seems more stable but the fact that it's inconsistent drives me insane
			//Likely gonna have to wait until they fix MonoMod itself

			Main.QueueMainThreadAction(() =>
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
			});
		}

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
