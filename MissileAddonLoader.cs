using System.Collections.Generic;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod
{
	public static class MissileAddonLoader
	{
		/// <summary>
		/// List of all missile addons that exist.
		/// </summary>
		internal static readonly List<ModMissileAddon> addons = new();

		internal static readonly Dictionary<int, string> unloadedAddons = new();

		//The following methods are all used in order to obtain an addon's ModMissileAddon value through its other forms.
		internal static bool TryGetValue(this IList<ModMissileAddon> list, int type, out ModMissileAddon missile) =>
			list.TryGetValue(i => i.Type == type, out missile);
		internal static bool TryGetValue(this IList<ModMissileAddon> list, string fullName, out ModMissileAddon missile) =>
			list.TryGetValue(i => i.FullName == fullName, out missile);
		internal static bool TryGetValue(this IList<ModMissileAddon> list, Item item, out ModMissileAddon missile) =>
			list.TryGetValue(i => i.ItemType == item.type, out missile);
		public static bool TryGetAddon(Item item, out ModMissileAddon missile) =>
			addons.TryGetValue(item, out missile);
		public static bool TryGetAddon(int type, out ModMissileAddon missile) =>
			addons.TryGetValue(type, out missile);
		public static bool TryGetAddon(string fullName, out ModMissileAddon missile) =>
			addons.TryGetValue(fullName, out missile);
		public static bool TryGetAddon<T>(out ModMissileAddon missile) =>
			addons.TryGetValue(i => i is T, out missile);

		/// <summary>
		/// The total number of missile addons that exist.
		/// </summary>
		public static int AddonCount => addons.Count;

		/// <summary>
		/// Gets the ModMissileAddon of an addon through its <b>Item value.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModMissileAddon missile) ? missile : null;
		/// <summary>
		/// Gets the ModMissileAddon of an addon through its <b>index number.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModMissileAddon missile) ? missile : null;

		/// <summary>
		/// Gets the ModMissileAddon of an addon through its <b>name text.</b><br/>
		/// Used to access an addon's properties for further use.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModMissileAddon missile) ? missile : null;

		/// <summary>
		/// Gets the ModMissileAddon of an addon through <b>idfk</b><br/>
		/// Used to access an addon's properties for further use.<br/>
		/// NOTE: Can someone else check this thing and tell me how it gets the thing?   -Z
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ModMissileAddon GetAddon<T>() where T : ModMissileAddon =>
			addons.TryGetValue(i => i is T, out ModMissileAddon missile) ? missile : null;

		public static bool IsAMissileTile(Tile tile)
		{
			foreach (ModMissileAddon addon in addons)
			{
				if (tile.TileType == addon.TileType) { return true; }
			}
			return false;
		}

		//The following methods are simply some under-the-hood stuff to make sure things actually load properly.
		internal static void ReloadTypes(TagCompound unloadedTag)
		{
			unloadedAddons.Clear();
			Dictionary<string, object> unloaded = new(unloadedTag);
			foreach ((string name, object type) in unloaded)
			{
				unloadedAddons[(int)type] = name;
			}

			HashSet<int> reserveTypes = new();
			foreach ((int type, string name) in unloadedAddons)
			{
				if (addons.TryGetValue(name, out ModMissileAddon missile))
				{
					missile.ChangeType(type);
					reserveTypes.Add(type);
				}
			}

			int freeType = 3;
			foreach (ModMissileAddon missile in addons)
			{
				if (reserveTypes.Contains(missile.Type)) { continue; }

				while (reserveTypes.Contains(freeType)) { freeType++; }

				missile.ChangeType(freeType);
				freeType++;
			}
		}
		//Method Stackems ahead.
		//This is the REAL fun part: where projectile behavior is added onto the beam shots in real-time.
		/// <summary>
		/// Runs the OnSpawn() behavior of every addon in a given array.
		/// <br/>The reason it's OnInitialized instead of OnSpawn is because you can't really insert addons before OnSpawn() runs.
		/// </summary>
		/// <param name="addons"></param>
		/// <param name="shot"></param>
		/// <param name="source"></param>
		public static void AddonOnInitialized(ModMissileAddon[] addons, MProjectile shot, IEntitySource source)
		{
			for (int i = 0; i < addons.Length - 1; ++i)
			{
				if (addons[i] == null) { continue; }
				addons[i].OnSpawn(shot, source);
			}
		}

		internal static void Unload()
		{
			addons.Clear();
			unloadedAddons.Clear();
		}
	}
}
