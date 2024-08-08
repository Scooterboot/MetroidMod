using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	internal class HatchDrawingSystem : ModSystem
	{
		public override void Load()
		{
			IL_Main.DoDraw += il =>
			{
				ILCursor c = new(il);
				
				// In this case, we would like to draw hatches in front of water
				// so, let's go to the lines of code that draw water in the foreground
				c.GotoNext(i =>
				{
					bool loadsScene = i.MatchLdsfld(typeof(Overlays).GetField("Scene", BindingFlags.Public | BindingFlags.Static));
					if (!loadsScene) return false;

					bool sceneIsWater = i.Next.Next.MatchLdcI4((int)RenderLayers.ForegroundWater);
					return sceneIsWater;
				});
				// There's code that jumps to the current location, make sure we place stuff
				// after said location so it doesn't get skipped
				c.MoveAfterLabels();

				// Our cursor now inserts code directly before water is drawn
				// Let's add our hook now!
				c.EmitDelegate(() =>
				{
					// The code currently has the correct spriteBatch set, so we don't need
					// to begin and end one oursellves
					DrawHatches();
				});
			};
		}

		private void DrawHatches()
		{
			GetVisibleTiles(out int startX, out int startY, out int endX, out int endY);

			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int y = startY; y <= endY; y++)
			{
				for (int x = startX; x <= endX; x++)
				{
					DrawPosition(spriteBatch, x, y);
				}
			}
		}

		private void DrawPosition(SpriteBatch spriteBatch, int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (ModContent.GetModTile(tile.TileType) is not HatchTile hatchTile) return;

			string doorTexture = hatchTile.Texture;
			Rectangle doorSource = new(tile.TileFrameX, tile.TileFrameY, 16, 16);
			DrawAt(spriteBatch, i, j, doorSource, doorTexture, GetHatchPaintSource(i, j));

			HatchTileEntity tileEntity = hatchTile.TileEntity(i, j);
			int animationFrame = tileEntity.Animation.DoorAnimationFrame;
			bool isInvisible = animationFrame == 4;
			if (!isInvisible)
			{
				string bubbleTexture = tileEntity.Appearance.GetTexturePath(hatchTile.Vertical);
				Rectangle bubbleSource = new(tile.TileFrameX, tile.TileFrameY + animationFrame * (18 * 4), 16, 16);
				DrawAt(spriteBatch, i, j, bubbleSource, bubbleTexture, new(i, j));
			}
		}
		
		private void DrawAt(SpriteBatch spriteBatch, int i, int j, Rectangle? source, string texturePath, Point paintSource)
		{
			Tile paintSourceTile = Main.tile[paintSource];
			if (!TileDrawing.IsVisible(paintSourceTile)) return;

			Texture2D baseTexture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value;
			Texture2D texture = ModContent.GetInstance<TilePaintedTextureSystem>().RequestPaintedTexture(baseTexture, paintSourceTile.TileColor);
			Vector2 drawPosition = new Vector2(i, j).ToWorldCoordinates(0, 0) - Main.screenPosition;
			Color color = paintSourceTile.IsTileFullbright ? Color.White : Lighting.GetColor(i, j);

			spriteBatch.Draw(
				texture,
				drawPosition,
				source,
				color,
				0f,
				Vector2.Zero,
				1f,
				SpriteEffects.None,
				0f
			);
		}

		private Point GetHatchPaintSource(int i, int j)
		{
			var hatchTile = ModContent.GetModTile(Main.tile[i, j].TileType) as HatchTile;
			var origin = TileUtils.GetTopLeftTileInMultitile(i, j);
			int sx = i - origin.X;
			int sy = j - origin.Y;

			if(hatchTile.Vertical)
			{
				sy = Math.Clamp(sy, 1, 2);
			}
			else
			{
				sx = Math.Clamp(sx, 1, 2);
			}

			return new(origin.X + sx, origin.Y + sy);
		}

		// TODO this method really doesn't belong here, but afaik not needed anywhere else yet
		private void GetVisibleTiles(out int startX, out int startY, out int endX, out int endY)
		{
			Vector2 screenPosition = Main.screenPosition;
			Vector2 screenSize = new(Main.screenWidth, Main.screenHeight);
			Vector2 tileSize = new(16, 16);

			startX = (int)(screenPosition.X / tileSize.X) - 1;
			startY = (int)(screenPosition.Y / tileSize.Y) - 1;
			endX = (int)((screenPosition.X + screenSize.X) / tileSize.X) + 1;
			endY = (int)((screenPosition.Y + screenSize.Y) / tileSize.Y) + 1;

			startX = Math.Max(0, startX);
			startY = Math.Max(0, startY);
			endX = Math.Min(Main.maxTilesX - 1, endX);
			endY = Math.Min(Main.maxTilesY - 1, endY);
		}
	}
}
