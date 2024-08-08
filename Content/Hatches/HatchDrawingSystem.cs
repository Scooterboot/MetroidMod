using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	internal class HatchDrawingSystem : ModSystem
	{
		public override void PostDrawTiles()
		{
			GetVisibleTiles(out int startX, out int startY, out int endX, out int endY);

			SpriteBatch spriteBatch = Main.spriteBatch;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			for (int y = startY; y <= endY; y++)
			{
				for (int x = startX; x <= endX; x++)
				{
					DrawPosition(spriteBatch, x, y);
				}
			}

			spriteBatch.End();
		}

		private void DrawPosition(SpriteBatch spriteBatch, int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (ModContent.GetModTile(tile.TileType) is not HatchTile hatchTile) return;

			string doorTexture = hatchTile.Texture;
			Rectangle doorSource = new(tile.TileFrameX, tile.TileFrameY, 16, 16);
			DrawAt(spriteBatch, i, j, doorSource, doorTexture, GetHatchPaintColor(i, j));

			HatchTileEntity tileEntity = hatchTile.TileEntity(i, j);
			int animationFrame = tileEntity.Animation.DoorAnimationFrame;
			bool isInvisible = animationFrame == 4;
			if (!isInvisible)
			{
				string bubbleTexture = tileEntity.Appearance.GetTexturePath(hatchTile.Vertical);
				Rectangle bubbleSource = new(tile.TileFrameX, tile.TileFrameY + animationFrame * (18 * 4), 16, 16);
				DrawAt(spriteBatch, i, j, bubbleSource, bubbleTexture, tile.TileColor);
			}
		}
		
		private void DrawAt(SpriteBatch spriteBatch, int i, int j, Rectangle? source, string texturePath, int paintColor)
		{
			Tile tile = Main.tile[i, j];
			Texture2D baseTexture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value;
			Texture2D texture = ModContent.GetInstance<TilePaintedTextureSystem>().RequestPaintedTexture(baseTexture, paintColor);
			Vector2 drawPosition = new Vector2(i, j).ToWorldCoordinates(0, 0) - Main.screenPosition;
			Color lightColor = Lighting.GetColor(i, j);

			spriteBatch.Draw(
				texture,
				drawPosition,
				source,
				lightColor,
				0f,
				Vector2.Zero,
				1f,
				SpriteEffects.None,
				0f
			);
		}

		private int GetHatchPaintColor(int i, int j)
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

			return Main.tile[origin.X + sx, origin.Y + sy].TileColor;
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
