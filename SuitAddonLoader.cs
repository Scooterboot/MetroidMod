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
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using MetroidMod.Content.Items.Armors;
using MetroidMod.Default;
using MetroidMod.ID;

using MetroidMod.Common.Players;

namespace MetroidMod
{
	public static class SuitAddonLoader
	{
		internal static readonly List<ModSuitAddon> addons = new();
		internal static readonly List<GlobalSuitAddon> globalAddons = new();

		internal static readonly Dictionary<int, string> unloadedAddons = new();

		internal static bool TryGetValue(this IList<ModSuitAddon> list, int type, out ModSuitAddon modSuitAddon) =>
			list.TryGetValue(i => i.Type == type, out modSuitAddon);
		internal static bool TryGetValue(this IList<ModSuitAddon> list, string fullName, out ModSuitAddon modSuitAddon) =>
			list.TryGetValue(i => i.FullName == fullName, out modSuitAddon);
		internal static bool TryGetValue(this IList<ModSuitAddon> list, Item item, out ModSuitAddon modSuitAddon) =>
			list.TryGetValue(i => i.ItemType == item.type, out modSuitAddon);

		public static bool TryGetAddon(Item item, out ModSuitAddon modSuitAddon) =>
			addons.TryGetValue(item, out modSuitAddon);

		public static bool TryGetAddon(int type, out ModSuitAddon modSuitAddon) =>
			addons.TryGetValue(type, out modSuitAddon);

		public static bool TryGetAddon(string fullName, out ModSuitAddon modSuitAddon) =>
			addons.TryGetValue(fullName, out modSuitAddon);

		public static bool TryGetAddon<T>(out ModSuitAddon modSuitAddon) where T : ModSuitAddon =>
			addons.TryGetValue(i => i is T, out modSuitAddon);

		public static int AddonCount => addons.Count;

		public static ModSuitAddon GetAddon(Item item) =>
			addons.TryGetValue(item, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon<T>() where T : ModSuitAddon =>
			addons.TryGetValue(i => i is T, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

		public static bool IsASuitTile(Tile tile)
		{
			foreach(ModSuitAddon addon in addons)
			{
				if(tile.TileType == addon.TileType) { return true; }
			}
			return false;
		}

		/*public static void OnUpdate(Player player, Item[] items)
		{
			foreach (Item item in items)
			{
				if (item.type == ItemID.None) { continue; }
				SuitAddonItem addonItem = (SuitAddonItem)item.ModItem;
				ModSuitAddon addon = addonItem.modSuitAddon;
				addon.OnUpdate(player);
			}
		}*/

		public static string GetAddonSlotName(int AddonSlot)
		{
			return AddonSlot switch
			{
				SuitAddonSlotID.Tanks_Energy => "Energy Tank",
				SuitAddonSlotID.Tanks_Reserve => "Reserve Tank",
				SuitAddonSlotID.Suit_Barrier => "Barrier",
				SuitAddonSlotID.Suit_Primary => "Primary",
				SuitAddonSlotID.Visor_Scan => "Scan Visor",
				SuitAddonSlotID.Visor_Utility => "Utility Visor",
				SuitAddonSlotID.Visor_AltVision => "Alt Visor",
				_ => "Unknown"
			};
		}

		public static string GetSetBonusText(Player player)
		{
			string returnVal = "\n";
			int index = 0;
			Item[] items = new Item[SuitAddonSlotID.Count];
			foreach (Item item in (player.armor[0].ModItem as PowerSuitHelmet).SuitAddons)
			{
				items[index++] = item;
			}
			foreach (Item item in (player.armor[1].ModItem as PowerSuitBreastplate).SuitAddons)
			{
				items[index++] = item;
			}
			foreach (Item item in items)
			{
				if (item.type == ItemID.None) { continue; }
				SuitAddonItem addonItem = (SuitAddonItem)item.ModItem;
				ModSuitAddon addon = addonItem?.modSuitAddon;
				string temp = addon.GetSetBonusText();
				if (temp != null && temp != "")
				{
					returnVal += temp + "\n";
				}
			}
			return returnVal;
		}

		public static void OnUpdateArmorSet(Player player)
		{
			int index = 0;
			Item[] items = new Item[SuitAddonSlotID.Count];
			foreach (Item item in (player.armor[0].ModItem as PowerSuitHelmet).SuitAddons)
			{
				items[index++] = item;
			}
			foreach (Item item in (player.armor[1].ModItem as PowerSuitBreastplate).SuitAddons)
			{
				items[index++] = item;
			}
			ModSuitAddon[] suitAddons = new ModSuitAddon[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] == null || !TryGetAddon(items[i], out ModSuitAddon addon)) { continue; }
				suitAddons[i] = addon;

				addon.OnUpdateArmorSet(player, items[i].stack);
			}
			foreach (GlobalSuitAddon gsa in globalAddons)
			{
				gsa.OnUpdateArmorSet(MPlayer.GetPowerSuit(player), player);
			}
		}

		public static void OnUpdateVanitySet(Player player)
		{
			Item[] items = (GetBreastplate(player, true).ModItem as PowerSuitBreastplate).SuitAddons;
			for (int i = SuitAddonSlotID.Suit_Barrier; i <= SuitAddonSlotID.Suit_Primary; i++)
			{
				if (items[i] == null || !TryGetAddon(items[i], out ModSuitAddon addon)) { continue; }
				addon.OnUpdateVanitySet(player);
			}
			foreach (GlobalSuitAddon gsa in globalAddons)
			{
				gsa.OnUpdateVanitySet(MPlayer.GetPowerSuit(player), player);
			}
		}

		public static Item GetBreastplate(Player player, bool lookingForVanity)
		{
			if (player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>() && (!lookingForVanity || player.armor[11].IsAir))
			{
				return player.armor[1];
			}
			if (player.armor[11].type == ModContent.ItemType<PowerSuitBreastplate>() && lookingForVanity)
			{
				return player.armor[11];
			}
			return null;
		}

		public static void ArmorSetShadows(Player player)
		{
			Item[] items = (GetBreastplate(player, true).ModItem as PowerSuitBreastplate).SuitAddons;
			ModSuitAddon[] suitAddons = new ModSuitAddon[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] == null || !TryGetAddon(items[i], out ModSuitAddon addon)) { continue; }
				suitAddons[i] = addon;

				addon.ArmorSetShadows(player);
			}
			foreach (GlobalSuitAddon gsa in globalAddons)
			{
				gsa.ArmorSetShadows(suitAddons, player);
			}
		}

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
				if (addons.TryGetValue(name, out ModSuitAddon modSuitAddon))
				{
					modSuitAddon.ChangeType(type);
					reserveTypes.Add(type);
				}
			}

			int freeType = 3;
			foreach (ModSuitAddon modSuitAddon in addons)
			{
				if (reserveTypes.Contains(modSuitAddon.Type)) { continue; }

				while (reserveTypes.Contains(freeType)) { freeType++; }

				modSuitAddon.ChangeType(freeType);
				freeType++;
			}
		}

		internal static void Unload()
		{
			addons.Clear();
			globalAddons.Clear();
			unloadedAddons.Clear();
		}
	}
}
