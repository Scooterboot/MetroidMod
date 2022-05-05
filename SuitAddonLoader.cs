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
using MetroidModPorted.Default;
using MetroidModPorted.ID;

using MetroidModPorted.Common.Players;

namespace MetroidModPorted
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
			list.TryGetValue(i => i.Item.Type == item.type, out modSuitAddon);

		public static bool TryGetAddon(Item item, out ModSuitAddon modSuitAddon) =>
			addons.TryGetValue(item, out modSuitAddon);

		public static int AddonCount => addons.Count;

		public static ModSuitAddon GetAddon(int type) =>
			addons.TryGetValue(type, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon<T>() where T : ModSuitAddon =>
			addons.TryGetValue(i => i is T, out ModSuitAddon modSuitAddon) ? modSuitAddon : null;

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
				SuitAddonSlotID.Suit_Varia => "Varia",
				SuitAddonSlotID.Suit_Utility => "Utility",
				SuitAddonSlotID.Suit_Augment => "Augmentation",
				SuitAddonSlotID.Suit_LunarAugment => "Secondary Augmentation",
				SuitAddonSlotID.Misc_Grip => "Hand",
				SuitAddonSlotID.Misc_Attack => "Attack",
				SuitAddonSlotID.Boots_JumpHeight => "Boots",
				SuitAddonSlotID.Boots_Jump => "Jump",
				SuitAddonSlotID.Boots_Speed => "Speed Augmentation",
				_ => "Unknown"
			};
		}

		public static string GetSetBonusText(Player player)
		{
			string returnVal = "\n";
			MPlayer mPlayer = player.GetModPlayer<MPlayer>();
			Item[] items = mPlayer.SuitAddons;
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
			MPlayer mPlayer = player.GetModPlayer<MPlayer>();
			Item[] items = mPlayer.SuitAddons;
			foreach (Item item in items)
			{
				if (item.type == ItemID.None) { continue; }
				SuitAddonItem addonItem = (SuitAddonItem)item.ModItem;
				ModSuitAddon addon = addonItem?.modSuitAddon;
				addon.OnUpdateArmorSet(player);
			}
		}

		public static void OnUpdateVanitySet(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			Item[] items = mp.SuitAddons;
			for (int i = SuitAddonSlotID.Suit_Varia; i <= SuitAddonSlotID.Suit_LunarAugment; i++)
			{
				Item item = items[i];
				if (item.type == ItemID.None) { continue; }
				SuitAddonItem addonItem = (SuitAddonItem)item.ModItem;
				ModSuitAddon addon = addonItem?.modSuitAddon;
				addon.OnUpdateVanitySet(player);
			}
		}

		public static void ArmorSetShadows(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			Item[] items = mp.SuitAddons;
			for (int i = 0; i < SuitAddonSlotID.Count; i++)
			{
				Item item = items[i];
				if (item.type == ItemID.None) { continue; }
				SuitAddonItem addonItem = (SuitAddonItem)item.ModItem;
				ModSuitAddon addon = addonItem?.modSuitAddon;
				addon.ArmorSetShadows(player);
			}
		}

		public static bool IsVanitySet(int head, int body, int legs)
		{
			return false;
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
