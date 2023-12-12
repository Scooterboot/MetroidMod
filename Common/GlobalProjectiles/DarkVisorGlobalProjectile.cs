using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalProjectiles
{
	internal class DarkVisorGlobalProjectile : GlobalProjectile
	{
		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			if (Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) &&
				SuitAddonLoader.TryGetAddon<Content.SuitAddons.DarkVisor>(out ModSuitAddon darkVisor) &&
				mp.VisorInUse == darkVisor.Type)
			{
				lightColor = new Color(255, 0, 0);
			}
			return true;
		}
	}
}
