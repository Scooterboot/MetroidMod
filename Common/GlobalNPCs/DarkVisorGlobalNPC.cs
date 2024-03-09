using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalNPCs
{
	/// <summary>
	/// Class specifically made for the Dark Visor.
	/// </summary>
	public class DarkVisorGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) &&
				SuitAddonLoader.TryGetAddon<Content.SuitAddons.DarkVisor>(out ModSuitAddon darkVisor) &&
				mp.VisorInUse == darkVisor.Type)
			{
				drawColor = new Color(255, 0, 0);
			}
		}
	}
}
