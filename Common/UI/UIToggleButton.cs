using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	internal class UIToggleButton : UIImageButton
	{
		/// <summary>
		/// Stores the various possible textures of the button.
		/// </summary>
		private readonly Asset<Texture2D> offTex;
		private Asset<Texture2D> offTexHover;
		private Asset<Texture2D> offTexClick;
		private Asset<Texture2D> onTex;
		private readonly Asset<Texture2D> onTexHover;
		private readonly Asset<Texture2D> onTexClick;

		/// <summary>
		/// The base filepath off of which all iterations of the button's texture are grabbed.
		/// <br/>Should be the filepath for the default texture.
		/// </summary>
		private readonly string texPath;
		/// <summary>
		/// Determines whether the button is "on" or "off".
		/// </summary>
		protected bool toggled;

		public UIToggleButton(Asset<Texture2D> texture, string texturePath, bool includeClickState = false) : base(texture)
		{
			//Every fiber of my being hates that I can't just use the path alone or get the path off of the texture
			//Unforunately it is out of my control at this point
			offTex = texture;
			texPath = texturePath;
		}

		public override void OnInitialize()
		{
			Width.Pixels = offTex.Width();
			Height.Pixels = offTex.Height();

			offTexHover = ModContent.Request<Texture2D>(texPath + "_Hover");
			onTex = ModContent.Request<Texture2D>(texPath + "_On");

			offTexClick = ModContent.Request<Texture2D>(texPath + "_Click");
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			//If left mouse is not down:
			//switch texture to hover
			if (!Main.mouseLeft)
			{

			}
			base.MouseOver(evt);
		}

		public override void LeftMouseDown(UIMouseEvent evt)
		{
			//switch texture to click
			base.LeftMouseDown(evt);
		}

		public override void LeftMouseUp(UIMouseEvent evt)
		{
			//toggle the toggled bool
			base.LeftMouseUp(evt);
		}
	}
}
