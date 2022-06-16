#region using directives

using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

#endregion

namespace MetroidMod.Common.UI
{
	// Pretty much stolen from ExampleMod
	public abstract class DragableUIPanel : UIPanel
	{
		private Vector2 offset;
		public bool dragging;
		public bool enabled;

		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);
			if (enabled && base.Elements.All((UIElement x) => !x.IsMouseHovering))
			{
				DragStart(evt);
			}
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			base.MouseUp(evt);
			if (enabled)
			{
				DragEnd(evt);
			}
		}

		private void DragStart(UIMouseEvent evt)
		{
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt)
		{
			Vector2 end = evt.MousePosition;
			dragging = false;

			Left.Set(end.X - offset.X, 0f);
			Top.Set(end.Y - offset.Y, 0f);

			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (ContainsPoint(Main.MouseScreen) && enabled)
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			if (dragging && enabled)
			{
				Left.Set(Main.mouseX - offset.X, 0f);
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			Rectangle parentSpace = Parent.GetDimensions().ToRectangle();
			Rectangle mouseRect = new(Main.mouseX, Main.mouseY, 0, 0);
			if (!GetDimensions().ToRectangle().Intersects(parentSpace) && mouseRect.Intersects(parentSpace))
			{
				return;
			}
			Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
			Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
			Recalculate();
		}
	}
}
