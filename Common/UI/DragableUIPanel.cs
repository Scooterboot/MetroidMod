#region using directives

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
		public override void RightMouseDown(UIMouseEvent evt)
		{
			base.RightMouseDown(evt);
			if (enabled)
			{
				DragStart(evt);
			}
		}

		//public override void RightMouseUp(UIMouseEvent evt)
		//{
		//	base.RightMouseUp(evt);
		//	if (enabled)
		//	{
		//		DragEnd(evt);
		//	}
		//}

		private void DragStart(UIMouseEvent evt)
		{
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(Vector2 evt)
		{
			Vector2 end = evt;
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

			if (dragging && !Main.mouseRight)
			{
				DragEnd(Main.MouseScreen);
			}
			if (dragging && enabled)
			{
				Left.Set(Main.mouseX - offset.X, 0f);
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			Rectangle parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace))
			{
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				Recalculate();
			}
		}
	}
}
