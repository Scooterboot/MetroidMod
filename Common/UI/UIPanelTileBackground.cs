using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	public class UIPanelTileBackground : UIElement
	{
		public Asset<Texture2D> panelAsset;
		public Asset<Texture2D> borderAsset;
		public int cornerSize;
		public int barSize;
		public Color panelColor = Color.White;

		public UIPanelTileBackground(Asset<Texture2D> panel, Asset<Texture2D> border, int cornerSize = 12, int barSize = 4)
		{
			panelAsset = panel;
			borderAsset = border;
			this.cornerSize = cornerSize;
			this.barSize = barSize;
			OverrideSamplerState = SamplerState.PointWrap;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Point point = new((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new(point.X + (int)dimensions.Width - cornerSize, point.Y + (int)dimensions.Height - cornerSize);
			int width = point2.X - point.X - cornerSize;
			int height = point2.Y - point.Y - cornerSize;

			if (panelAsset != null)
			{
				Rectangle drawArea = new(point.X + (cornerSize / 2), point.Y + (cornerSize / 2), width + cornerSize, height + cornerSize);
				spriteBatch.Draw(panelAsset.Value, drawArea with { X = drawArea.X - 2, Y = drawArea.Y - 2 }, drawArea with { X = 0, Y = 0 }, panelColor);
			}
			if (borderAsset != null)
			{
				// Copied from UIPanel
				Texture2D texture = borderAsset.Value;
				Color color = Color.White;
				spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, cornerSize, cornerSize), new Rectangle(0, 0, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, cornerSize, cornerSize), new Rectangle(cornerSize + barSize, 0, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, cornerSize, cornerSize), new Rectangle(0, cornerSize + barSize, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, cornerSize, cornerSize), new Rectangle(cornerSize + barSize, cornerSize + barSize, cornerSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(point.X + cornerSize, point.Y, width, cornerSize), new Rectangle(cornerSize, 0, barSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(point.X + cornerSize, point2.Y, width, cornerSize), new Rectangle(cornerSize, cornerSize + barSize, barSize, cornerSize), color);
				spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + cornerSize, cornerSize, height), new Rectangle(0, cornerSize, cornerSize, barSize), color);
				spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + cornerSize, cornerSize, height), new Rectangle(cornerSize + barSize, cornerSize, cornerSize, barSize), color);
				spriteBatch.Draw(texture, new Rectangle(point.X + cornerSize, point.Y + cornerSize, width, height), new Rectangle(cornerSize, cornerSize, barSize, barSize), color);
			}
		}
	}
}
