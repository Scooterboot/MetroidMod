using MetroidMod.ID;
using System.Collections.Generic;
using Terraria;

namespace MetroidMod.Content.MorphBallAddons
{
	public static class MorphBallAddonLoader
	{
		internal static readonly List<ModMorphBallAddon> addons = new();

		public static int BombsRecipeGroupID => MetroidMod.MorphBallBombsRecipeGroupID;

		internal static bool TryGetValue(this IList<ModMorphBallAddon> list, int type, out ModMorphBallAddon modMorphBallAddon) =>
			list.TryGetValue(i => i.Type == type, out modMorphBallAddon);
		internal static bool TryGetValue(this IList<ModMorphBallAddon> list, string fullName, out ModMorphBallAddon modMorphBallAddon) =>
			list.TryGetValue(i => i.FullName == fullName, out modMorphBallAddon);
		internal static bool TryGetValue(this IList<ModMorphBallAddon> list, Item item, out ModMorphBallAddon modMorphBallAddon) =>
			list.TryGetValue(i => i.ItemType == item.type, out modMorphBallAddon);

		public static bool TryGetAddon(Item item, out ModMorphBallAddon modMorphBallAddon) =>
			addons.TryGetValue(item, out modMorphBallAddon);

		public static bool TryGetAddon(int type, out ModMorphBallAddon modMorphBallAddon) =>
			addons.TryGetValue(type, out modMorphBallAddon);

		public static bool TryGetAddon(string fullName, out ModMorphBallAddon modMorphBallAddon) =>
			addons.TryGetValue(fullName, out modMorphBallAddon);

		public static bool TryGetAddon<T>(out ModMorphBallAddon modMorphBallAddon) where T : ModMorphBallAddon =>
			addons.TryGetValue(i => i is T, out modMorphBallAddon);

		public static int AddonCount => addons.Count;

		public static ModMorphBallAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModMorphBallAddon modMorphBallAddon) ? modMorphBallAddon : null;

		public static ModMorphBallAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModMorphBallAddon modMorphBallAddon) ? modMorphBallAddon : null;

		public static ModMorphBallAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModMorphBallAddon modMorphBallAddon) ? modMorphBallAddon : null;

		public static ModMorphBallAddon GetAddon<T>() where T : ModMorphBallAddon =>
			addons.TryGetValue(i => i is T, out ModMorphBallAddon modMorphBallAddon) ? modMorphBallAddon : null;

		public static bool IsAMorphTile(Tile tile)
		{
			foreach (ModMorphBallAddon addon in addons)
			{
				if (tile.TileType == addon.TileType) { return true; }
			}
			return false;
		}

		internal static void Unload()
		{
			addons.Clear();
		}
	}
}
