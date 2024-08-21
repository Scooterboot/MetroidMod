using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Visuals
{
	/// <summary>
	/// Utility system that allows painting any texture with the same paint shaders
	/// that are applied to regular world tiles. Useful for paintable world objects
	/// that need to be rendered manually.
	/// </summary>
	internal class TilePaintedTextureSystem : ModSystem
	{
		private readonly Dictionary<(Texture2D, int), PaintedTextureRenderTargetHolder> cachedRenders = [];

		/// <summary>
		/// Paints the provided texture using the specified paint color.
		/// If the texture isn't ready yet, it will return the default, unpainted one.
		/// </summary>
		/// <param name="texture">The texture to be painted.</param>
		/// <param name="paintId">The PaintID to paint the texture with.</param>
		/// <returns></returns>
		public Texture2D RequestPaintedTexture(Texture2D texture, int paintId)
		{
			if (paintId == PaintID.None) return texture;

			var key = (texture, paintId);
			if (!cachedRenders.ContainsKey(key))
			{
				cachedRenders[key] = new(texture, paintId);

				// At this point I feel like it's cleaner to commit this atrocity than the alternative.
				TilePaintSystemV2 paintSystem = Main.instance.TilePaintSystem;
				var requests = paintSystem.GetType().GetField("_requests", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(paintSystem)
					as List<TilePaintSystemV2.ARenderTargetHolder>;
				requests.Add(cachedRenders[key]);

			}

			return cachedRenders[key].IsReady ? cachedRenders[key].Target : texture;
		}

		private class PaintedTextureRenderTargetHolder(Texture2D BaseTexture, int PaintColor) : TilePaintSystemV2.ARenderTargetHolder
		{
			public override void Prepare()
			{
				PrepareTextureIfNecessary(BaseTexture, null);
			}

			public override void PrepareShader()
			{
				PrepareShader(PaintColor, new TreePaintingSettings());
			}
		}
	}
}
