using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	internal class UIDraggableBase : UIPanel
	{
		// Stores the offset from the top left of the UIPanel while dragging
		private Vector2 offset;
		// A flag that checks if the panel is currently being dragged
		private bool dragging;

		public override void RightMouseDown(UIMouseEvent evt)
		{
			// When you override UIElement methods, don't forget call the base method
			// This helps to keep the basic behavior of the UIElement
			base.RightMouseDown(evt);
			// When the mouse button is down on this element, then we start dragging
			if (this.Elements.Contains(evt.Target))
			{
				DragStart(evt);
			}
		}

		public override void RightMouseUp(UIMouseEvent evt)
		{
			base.RightMouseUp(evt);
			// When the mouse button is up, then we stop dragging
			if (this.Elements.Contains(evt.Target))
			{
				DragEnd(evt);
			}
		}

		private void DragStart(UIMouseEvent evt)
		{
			// The offset variable helps to remember the position of the panel relative to the mouse position
			// So no matter where you start dragging the panel, it will move smoothly
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt)
		{
			Vector2 endMousePosition = evt.MousePosition;
			dragging = false;

			Left.Set(endMousePosition.X - offset.X, 0f);
			Top.Set(endMousePosition.Y - offset.Y, 0f);

			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Checking ContainsPoint and then setting mouseInterface to true is very common
			// This causes clicks on this UIElement to not cause the player to use current items
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			if (dragging)
			{
				Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			// Here we check if the DraggableUIPanel is outside the Parent UIElement rectangle
			// (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DraggableUIPanel is outside the whole screen)
			// By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
			var parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace))
			{
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				// Recalculate forces the UI system to do the positioning math again.
				Recalculate();
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//fuck you no drawing
			//frowny face dot jay-peg
		}
	}
}
