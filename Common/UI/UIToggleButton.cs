using System;
using Terraria;
using Terraria.ModLoader;
using MetroidMod;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace MetroidMod.Common.UI
{
    class UIToggleButton : UIImageButton
    {
		/// <summary>
		/// Stores the various possible textures of the button.
		/// </summary>
		private Asset<Texture2D> offTex, offTexHover, offTexClick, onTex, onTexHover, onTexClick;
		/// <summary>
		/// Determines whether the button is "on" or "off".
		/// </summary>
		protected bool toggled;

		public UIToggleButton(Asset<Texture2D> texture, Asset<Texture2D> onTexture) : base(texture)
		{
			offTex = texture;
			onTex = onTexture;
		}

		public override void OnInitialize()
		{
			Width.Pixels = offTex.Width();
			Height.Pixels = offTex.Height();
		}
	}
}
