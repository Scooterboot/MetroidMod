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

namespace MetroidModPorted
{
	public static class MBAddonLoader
	{
		internal static readonly List<ModMBAddon> addons = new();
		internal static readonly List<GlobalMBAddon> globalAddons = new();

		internal static bool TryGetValue(this IList<ModMBAddon> list, int type, out ModMBAddon modMBAddon) =>
			list.TryGetValue(i => i.Type == type, out modMBAddon);
		internal static bool TryGetValue(this IList<ModMBAddon> list, string fullName, out ModMBAddon modMBAddon) =>
			list.TryGetValue(i => i.FullName == fullName, out modMBAddon);

		public static int AddonCount => addons.Count;

		public static ModMBAddon GetAddon(int type) =>
			addons.TryGetValue(type, out var modMBAddon) ? modMBAddon : null;

		public static ModMBAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out var modMBAddon) ? modMBAddon : null;

		public static ModMBAddon GetAddon<T>() where T : ModMBAddon =>
			addons.TryGetValue(i => i is T, out var modMBAddon) ? modMBAddon : null;
	}
}
