using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Elevators
{
	/// <summary>
	/// Platforms for idle elevators are drawn separately instead of on tile PreDraw for three reasons:
	/// 1. Tiles are drawn less frequently than everything else (every 4 game draws)
	/// When a platform goes from drawing from a tile into drawing below a player for moving,
	/// the lower draw frequency can lead to the platform blinking, which looks bad imo.
	/// 2. The platform animates at a different speed than the rest of the elevator,
	/// meaning making it part of the spritesheet would increase the amount of combinations a lot.
	/// 3. Drawing it apart from tiles means that it will always draw behind the player, regardless
	/// of whether they're behind tiles due to using an elevator or not.
	/// </summary>
	internal class ElevatorPlatformDrawing : ModSystem
	{
		public void DrawIdlePlatforms()
		{
			HashSet<Elevator> visibleElevators = [];
			TileUtils.IterateVisibleTiles((x, y) =>
			{
				if (Elevator.Find(new(x, y)) is Elevator elevator)
				{
					visibleElevators.Add(elevator);
				}
			});
			foreach (Elevator elevator in visibleElevators)
			{
				if (!elevator.IsInUse)
				{
					DrawPlatform(elevator.ArrivalPosition);
				}
			}
		}

		public void DrawPlayerPlatform(Player player)
		{
			DrawPlatform(player.Bottom);
		}

		private void DrawPlatform(Vector2 position)
		{
			SpriteBatch sb = Main.spriteBatch;
			Texture2D texture = ModContent.Request<Texture2D>($"{nameof(MetroidMod)}/Content/Elevators/ElevatorPlatform", AssetRequestMode.ImmediateLoad).Value;
			Point tile = position.ToTileCoordinates();
			Color color = Lighting.GetColor(tile);

			int frameAmount = 4;
			int frameHeight = texture.Height / frameAmount;
			int frame = (int)(Main.GameUpdateCount / 10 % frameAmount);

			Rectangle source = new(0, frame * frameHeight, texture.Width, frameHeight);
			sb.Draw(texture, position - Main.screenPosition, source, color, 0f, Vector2.UnitX * (texture.Width / 2), 1f, SpriteEffects.None, 0f);
		}
	}
}
