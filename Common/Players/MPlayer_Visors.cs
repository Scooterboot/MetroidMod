using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Content.Items.Armors;
using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted.Common.Players
{
	public partial class MPlayer : ModPlayer
	{
		/// <summary>
		/// The Suit Addon ID of the current visor in use. If -1, no visor is in use.
		/// </summary>
		public int VisorInUse = -1;
		/// <summary>
		/// The scan progress of the current NPC. This is used purely for UI purposes. <br />
		/// As this value increases, the bestiary entry's info increases,
		/// and the visual progress bar increases in fill. <br />
		/// Range: 0 - 1
		/// </summary>
		public float ScanProgress = 0f;
		public bool ShouldShowVisorUI;
		/// <summary>
		/// The color of the mod's Hud elements, including visor select menu.
		/// </summary>
		public Color HUDColor = Color.LightBlue;
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (Systems.MSystem.VisorUIKey.JustPressed)
			{
				ShouldShowVisorUI = true;
			}
			if (Systems.MSystem.VisorUIKey.JustReleased)
			{
				ShouldShowVisorUI = false;
			}
		}

		public void PostUpdateMiscEffects_Visors()
		{
			if (VisorInUse >= 0)
			{
				HUDColor = SuitAddonLoader.GetAddon(VisorInUse).VisorColor;
			}
		}

		internal static ModSuitAddon[] GetVisorAddons(Player player)
		{
			PowerSuitHelmet armor;
			if (player.armor[0].type == ModContent.ItemType<PowerSuitHelmet>())
			{
				armor = player.armor[0].ModItem as PowerSuitHelmet;
			}
			else if (player.armor[10].type == ModContent.ItemType<PowerSuitHelmet>())
			{
				armor = player.armor[10].ModItem as PowerSuitHelmet;
			}
			else { return new ModSuitAddon[3] { null, null, null }; }
			Item[] sa = armor.SuitAddons;
			ModSuitAddon[] msa = new ModSuitAddon[3];
			for (int i = 0; i < 3; i++)
			{
				if (sa[i].type == ItemID.None) { msa[i] = null; continue; }
				msa[i] = SuitAddonLoader.GetAddon(sa[i]);
			}
			return msa;
		}
	}
}
