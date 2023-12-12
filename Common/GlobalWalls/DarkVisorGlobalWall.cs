using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalWalls
{
	internal class DarkVisorGlobalWall : GlobalWall
	{
		public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
		{
			if (Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) &&
				SuitAddonLoader.TryGetAddon<Content.SuitAddons.DarkVisor>(out ModSuitAddon darkVisor) &&
				mp.VisorInUse == darkVisor.Type)
			{
				if (!Main.tile[i, j].HasTile)
				{
					r = MathHelper.Clamp(r + 1f, 0f, 1f);
					g = MathHelper.Clamp(g + 1f, 0f, 1f);
					b = MathHelper.Clamp(b + 1f, 0f, 1f);
				}
			}
		}
	}
}
