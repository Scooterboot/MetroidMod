using System.Collections.Generic;
using Terraria;

namespace MetroidMod.Content.ArmCannonAddons
{
	public static class ArmCannonAddonLoader
	{
		internal static readonly List<ModArmCannonAddon> addons = new();

		internal static readonly Dictionary<int, string> unloadedAddons = new();

		internal static bool TryGetValue(this IList<ModArmCannonAddon> list, int type, out ModArmCannonAddon modArmCannonAddon) =>
			list.TryGetValue(i => i.Type == type, out modArmCannonAddon);
		internal static bool TryGetValue(this IList<ModArmCannonAddon> list, string fullName, out ModArmCannonAddon modArmCannonAddon) =>
			list.TryGetValue(i => i.FullName == fullName, out modArmCannonAddon);
		internal static bool TryGetValue(this IList<ModArmCannonAddon> list, Item item, out ModArmCannonAddon modArmCannonAddon) =>
			list.TryGetValue(i => i.ItemType == item.type, out modArmCannonAddon);

		public static bool TryGetAddon(Item item, out ModArmCannonAddon modArmCannonAddon) =>
			addons.TryGetValue(item, out modArmCannonAddon);

		public static bool TryGetAddon(int type, out ModArmCannonAddon modArmCannonAddon) =>
			addons.TryGetValue(type, out modArmCannonAddon);

		public static bool TryGetAddon(string fullName, out ModArmCannonAddon modArmCannonAddon) =>
			addons.TryGetValue(fullName, out modArmCannonAddon);

		public static bool TryGetAddon<T>(out ModArmCannonAddon modArmCannonAddon) where T : ModArmCannonAddon =>
			addons.TryGetValue(i => i is T, out modArmCannonAddon);

		public static int AddonCount => addons.Count;

		public static ModArmCannonAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModArmCannonAddon modArmCannonAddon) ? modArmCannonAddon : null;

		public static ModArmCannonAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModArmCannonAddon modArmCannonAddon) ? modArmCannonAddon : null;

		public static ModArmCannonAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModArmCannonAddon modArmCannonAddon) ? modArmCannonAddon : null;

		public static ModArmCannonAddon GetAddon<T>() where T : ModArmCannonAddon =>
			addons.TryGetValue(i => i is T, out ModArmCannonAddon modArmCannonAddon) ? modArmCannonAddon : null;

		internal static void Unload()
		{
			addons.Clear();
			unloadedAddons.Clear();
		}
	}
}
