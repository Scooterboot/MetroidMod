using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	public class VanityGlowTexture : ModPlayer
	{
		internal static Color glowColor(Color getColor, int shader)
		{
			if (shader > 0)
			{
				//System.Drawing.Color color = System.Drawing.Color.FromArgb(getColor.R,getColor.G,getColor.B);
				//return MColor.HsvColor((double)color.GetHue(),(double)color.GetSaturation()*0.5,1.0);

				return new Color(getColor.R, getColor.G, getColor.B, 0);
			}
			return Color.White;
		}
	}
}
