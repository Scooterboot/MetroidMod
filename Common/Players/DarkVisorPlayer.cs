using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	public class DarkVisorPlayer : ModPlayer
	{
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) &&
				SuitAddonLoader.TryGetAddon<Content.SuitAddons.DarkVisor>(out ModSuitAddon darkVisor) &&
				mp.VisorInUse == darkVisor.Type)
			{
				g = 0;
				b = 0;
				drawInfo.bodyGlowColor = new Color(255, 0, 0);
			}
		}
	}
}
