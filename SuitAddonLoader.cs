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

		public static int AddonCount => addons.Count;

		public static ModSuitAddon GetAddon(int type) =>
			addons.TryGetValue(type, out var modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon(string fullName) =>
			addons.TryGetValue(fullName, out var modSuitAddon) ? modSuitAddon : null;

		public static ModSuitAddon GetAddon<T>() where T : ModSuitAddon =>
			addons.TryGetValue(i => i is T, out var modSuitAddon) ? modSuitAddon : null;

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
			for (int i = SuitAddonSlotID.Suit_Varia; i < SuitAddonSlotID.Misc_Grip; i++)
			{
				Item item = items[i];
				if (item.type == ItemID.None) { continue; }
				SuitAddonItem addonItem = (SuitAddonItem)item.ModItem;
				ModSuitAddon addon = addonItem?.modSuitAddon;
				addon.OnUpdateVanitySet(player);
			}
		}

		public static int GetHelmet(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(ModContent.GetInstance<Content.Items.Armors.PowerSuitHelmet>().Name, EquipType.Head);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Name, EquipType.Head);
				if (temp != -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetHelmetGlow(PlayerDrawSet info)
		{
			string tex = "MetroidModPorted/Content/Items/Armors/PowerSuitHelmet_Head_Glow";
			ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				string temp = msa[i].ArmorTextureHead;
				if (temp != "" && temp != null)
				{
					tex = temp;
				}
			}

			return ModContent.Request<Texture2D>(tex);
		}
		/*public static void DrawHelmetArmorColor(Player player, ref int glowMask, ref Color glowMaskColor)
		{

		}*/
		public static int GetBreastplate(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(ModContent.GetInstance<Content.Items.Armors.PowerSuitBreastplate>().Name, EquipType.Body);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Name, EquipType.Body);
				if (temp != -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetBreastplateGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<Content.Items.Armors.PowerSuitBreastplate>().Texture+"";//"MetroidModPorted/Content/Items/Armors/PowerSuitBreastplate_Body_Glow";
			ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				string temp = msa[i].ArmorTextureTorso;
				if (temp != "" && temp != null)
				{
					tex = temp;
				}
			}

			return ModContent.Request<Texture2D>(tex);
		}
		public static int GetArms(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(ModContent.GetInstance<Content.Items.Armors.PowerSuitBreastplate>().Name, EquipType.Body);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Name, EquipType.Body);
				if (temp == -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetArmsGlow(PlayerDrawSet info)
		{
			string tex = "MetroidModPorted/Content/Items/Armors/PowerSuitBreastplate_Body_Glow";
			ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				string temp = msa[i].ArmorTextureTorso;
				if (temp != "" && temp != null)
				{
					tex = temp;
				}
			}

			return ModContent.Request<Texture2D>(tex);
		}
		public static int GetGreaves(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(ModContent.GetInstance<Content.Items.Armors.PowerSuitGreaves>().Name, EquipType.Legs);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Name, EquipType.Legs);
				if (temp != -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetGreavesGlow(PlayerDrawSet info)
		{
			string tex = "MetroidModPorted/Content/Items/Armors/PowerSuitGreaves_Legs_Glow";
			ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				string temp = msa[i].ArmorTextureLegs;
				if (temp != "" && temp != null)
				{
					tex = temp;
				}
			}

			return ModContent.Request<Texture2D>(tex);
		}

		internal static ModSuitAddon[] GetPowerSuit(Player player)
		{
			MPlayer mPlayer = player.GetModPlayer<MPlayer>();
			Item[] sa = mPlayer.SuitAddons;
			ModSuitAddon[] msa = new ModSuitAddon[4];
			for (int i = SuitAddonSlotID.Suit_Varia; i <= SuitAddonSlotID.Suit_LunarAugment; i++)
			{
				if (sa[i].type == ItemID.None) { msa[i - SuitAddonSlotID.Suit_Varia] = null; continue; }
				msa[i - SuitAddonSlotID.Suit_Varia] = ((SuitAddonItem)sa[i].ModItem).modSuitAddon;
			}
			return msa;
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
