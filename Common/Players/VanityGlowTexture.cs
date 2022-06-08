using System;
using System.Linq;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Capture;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MetroidModPorted;

namespace MetroidModPorted.Common.Players
{
    public class VanityGlowTexture : ModPlayer  
    {
		internal static Color glowColor(Color getColor, int shader)
		{
			if(shader > 0)
			{
				//System.Drawing.Color color = System.Drawing.Color.FromArgb(getColor.R,getColor.G,getColor.B);
				//return MColor.HsvColor((double)color.GetHue(),(double)color.GetSaturation()*0.5,1.0);
				
				return new Color(getColor.R,getColor.G,getColor.B,0);
			}
			return Color.White;
		}
	}
}
