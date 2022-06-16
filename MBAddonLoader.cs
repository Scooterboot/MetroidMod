using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
//using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.ModLoader.IO;
using MetroidMod.ID;

namespace MetroidMod
{
	public static class MBAddonLoader
	{
		internal static readonly List<ModMBAddon> addons = new();
		internal static readonly List<GlobalMBAddon> globalAddons = new();

		public static int BombsRecipeGroupID => MetroidMod.MorphBallBombsRecipeGroupID;

		internal static bool TryGetValue(this IList<ModMBAddon> list, int type, out ModMBAddon modMBAddon) =>
			list.TryGetValue(i => i.Type == type, out modMBAddon);
		internal static bool TryGetValue(this IList<ModMBAddon> list, string fullName, out ModMBAddon modMBAddon) =>
			list.TryGetValue(i => i.FullName == fullName, out modMBAddon);
		internal static bool TryGetValue(this IList<ModMBAddon> list, Item item, out ModMBAddon modMBAddon) =>
			list.TryGetValue(i => i.ItemType == item.type, out modMBAddon);

		public static bool TryGetAddon(Item item, out ModMBAddon modMBAddon) =>
			addons.TryGetValue(item, out modMBAddon);

		public static bool TryGetAddon(int type, out ModMBAddon modMBAddon) =>
			addons.TryGetValue(type, out modMBAddon);

		public static bool TryGetAddon(string fullName, out ModMBAddon modMBAddon) =>
			addons.TryGetValue(fullName, out modMBAddon);

		public static bool TryGetAddon<T>(out ModMBAddon modMBAddon) where T : ModMBAddon =>
			addons.TryGetValue(i => i is T, out modMBAddon);

		public static int AddonCount => addons.Count;

		public static ModMBAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModMBAddon modMBAddon) ? modMBAddon : null;

		public static ModMBAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModMBAddon modMBAddon) ? modMBAddon : null;

		public static ModMBAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModMBAddon modMBAddon) ? modMBAddon : null;

		public static ModMBAddon GetAddon<T>() where T : ModMBAddon =>
			addons.TryGetValue(i => i is T, out ModMBAddon modMBAddon) ? modMBAddon : null;

		public static bool IsAMorphTile(Tile tile)
		{
			foreach (ModMBAddon addon in addons)
			{
				if (tile.TileType == addon.TileType) { return true; }
			}
			return false;
		}
		public static string GetAddonSlotName(int AddonSlot)
		{
			return AddonSlot switch
			{
				MorphBallAddonSlotID.Drill => "Drill",
				MorphBallAddonSlotID.Weapon => "Weapon",
				MorphBallAddonSlotID.Special => "Special",
				MorphBallAddonSlotID.Utility => "Utility",
				MorphBallAddonSlotID.Boost => "Boost",
				_ => "Unknown"
			};
		}
	}
}
