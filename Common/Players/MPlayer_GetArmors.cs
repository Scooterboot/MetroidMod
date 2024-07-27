using MetroidMod.Content.Items.Armors;
using MetroidMod.ID;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	public enum SuitAddonUIState : byte
	{
		None = 0,
		Helmet = 1,
		Breastplate = 2,
		Reserves = 3,
	}
	public partial class MPlayer : ModPlayer
	{
		public bool ShouldShowArmorUI = false;
		public bool IsPowerSuitHelmet = false;
		public bool ShouldDrawHelmet = false;
		public bool IsPowerSuitBreastplate = false;
		public bool ShouldDrawBreastplate = false;
		public bool IsPowerSuitGreaves = false;
		public bool ShouldDrawGreaves = false;
		public SuitAddonUIState SuitAddonUIState = SuitAddonUIState.None;
		public void ResetEffects_GetArmors()
		{
			ShouldShowArmorUI = false;
			IsPowerSuitHelmet = false;
			ShouldDrawHelmet = false;
			IsPowerSuitBreastplate = false;
			ShouldDrawBreastplate = false;
			IsPowerSuitGreaves = false;
			ShouldDrawGreaves = false;
		}
		private static void ModifyDrawInfo_GetArmors(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.TryGetModPlayer(out MPlayer mp))
			{
				if (mp.ShouldDrawGreaves)
				{
					drawInfo.drawPlayer.legs = GetGreaves(drawInfo.drawPlayer);
				}
				if (mp.ShouldDrawBreastplate)
				{
					drawInfo.drawPlayer.body = GetBreastplate(drawInfo.drawPlayer);
				}
				if (mp.ShouldDrawHelmet)
				{
					drawInfo.drawPlayer.head = GetHelmet(drawInfo.drawPlayer);
				}
			}
		}
		public override void UpdateVisibleVanityAccessories()
		{
			if ((Player.armor[0].type == ModContent.ItemType<PowerSuitHelmet>() && Player.armor[10].IsAir) || Player.armor[10].type == ModContent.ItemType<PowerSuitHelmet>())
			{
				ShouldDrawHelmet = true;
			}
			if ((Player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>() && Player.armor[11].IsAir) || Player.armor[11].type == ModContent.ItemType<PowerSuitBreastplate>())
			{
				ShouldDrawBreastplate = true;
			}
			if ((Player.armor[2].type == ModContent.ItemType<PowerSuitGreaves>() && Player.armor[12].IsAir) || Player.armor[12].type == ModContent.ItemType<PowerSuitGreaves>())
			{
				ShouldDrawGreaves = true;
			}
		}
		public static int GetHelmet(Player player)
		{
			int msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitHelmet), EquipType.Head);
			if(player.armor[0].type == ModContent.ItemType<PowerSuitHelmet>())
			{
				msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitHelmet), EquipType.Head);
				ModSuitAddon[] msa = GetPowerSuit(player);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					int temp = EquipLoader.GetEquipSlot(msa[i].Mod, msa[i].Name, EquipType.Head);
					if (temp != -1)
					{
						msaEqu = temp;
					}
				}
				return msaEqu;
			}
			else if(player.armor[0].type == ModContent.ItemType<ChoziteHelmet>())
			{
				msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(ChoziteHelmet), EquipType.Head);
			}
			return msaEqu;
		}
		public static Asset<Texture2D> GetHelmetGlow(PlayerDrawSet info)
		{
			Player drawPlayer = info.drawPlayer;
			string tex= ModContent.GetInstance<PowerSuitHelmet>().Texture + "_Head_Glow";
			if(drawPlayer.armor[0].type == ModContent.ItemType<PowerSuitHelmet>())
			{
				tex = ModContent.GetInstance<PowerSuitHelmet>().Texture + "_Head_Glow";//"MetroidMod/Content/Items/Armors/PowerSuitHelmet_Head_Glow";
				ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					string temp = msa[i].ArmorTextureHead;
					if (temp != "" && temp != null)
					{
						tex = temp + "_Glow";
					}
				}
			}
			else if(drawPlayer.armor[0].type == ModContent.ItemType<ChoziteHelmet>())
			{
				tex = ModContent.GetInstance<ChoziteHelmet>().Texture + "_Head_Glow";
			}

			return ModContent.Request<Texture2D>(tex);
		}
		public static int GetBreastplate(Player player)
		{
			int msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitBreastplate), EquipType.Body);
			if (player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>())
			{
				msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitBreastplate), EquipType.Body);
				ModSuitAddon[] msa = GetPowerSuit(player);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					int temp = EquipLoader.GetEquipSlot(msa[i].Mod, msa[i].Name, EquipType.Body);
					if (temp != -1)
					{
						msaEqu = temp;
					}
				}
				return msaEqu;
			}
			else if (player.armor[1].type == ModContent.ItemType<ChoziteBreastplate>())
			{
				msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(ChoziteBreastplate), EquipType.Body);
			}
			return msaEqu;
		}
		public static Asset<Texture2D> GetBreastplateGlow(PlayerDrawSet info)
		{
			Player drawPlayer = info.drawPlayer;
			string tex = ModContent.GetInstance<PowerSuitBreastplate>().Texture + "_Body_Glow";
			if (drawPlayer.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>())
			{
				tex = ModContent.GetInstance<PowerSuitBreastplate>().Texture + "_Body_Glow";
				ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					string temp = msa[i].ArmorTextureTorso;
					if (temp != "" && temp != null)
					{
						tex = temp + "_Glow";
					}
				}
			}
			else if (drawPlayer.armor[1].type == ModContent.ItemType<ChoziteBreastplate>())
			{
				tex = ModContent.GetInstance<ChoziteBreastplate>().Texture + "_Body_Glow";
			}

			return ModContent.Request<Texture2D>(tex);
		}
		public static int GetArms(Player player)
		{
			int msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitBreastplate), EquipType.Body);
				ModSuitAddon[] msa = GetPowerSuit(player);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					int temp = EquipLoader.GetEquipSlot(msa[i].Mod, msa[i].Name, EquipType.Body);
				if (temp == -1)
					{
						msaEqu = temp;
					}
				}
				return msaEqu;
			}
		public static Asset<Texture2D> GetArmsGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<PowerSuitBreastplate>().Texture + "_Arms_Glow";
			ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				string temp = msa[i].ArmorTextureArmsGlow;
				if (temp != "" && temp != null)
				{
					tex = temp;
				}
			}

			return ModContent.Request<Texture2D>(tex);
		}
		public static Asset<Texture2D> GetShouldersGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<PowerSuitBreastplate>().Texture + "_Arms_Glow";
			ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				if (i == 0 || msa[i].ShouldOverrideShoulders || msa[0] == null || msa[0].ArmorTextureShouldersGlow == null)
				{
					string temp = msa[i].ArmorTextureShouldersGlow;
					if (temp != "" && temp != null)
					{
						tex = temp;
					}
				}
			}
			return ModContent.Request<Texture2D>(tex);
		}
		public static int GetGreaves(Player player)
		{
			int msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitGreaves), EquipType.Legs);
			if (player.armor[2].type == ModContent.ItemType<PowerSuitGreaves>())
			{
				msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(PowerSuitGreaves), EquipType.Legs);
				ModSuitAddon[] msa = GetPowerSuit(player);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					int temp = EquipLoader.GetEquipSlot(msa[i].Mod, msa[i].Name, EquipType.Legs);
					if (temp != -1)
					{
						msaEqu = temp;
					}
				}
				return msaEqu;
			}
			else if (player.armor[2].type == ModContent.ItemType<ChoziteGreaves>())
			{
				msaEqu = EquipLoader.GetEquipSlot(MetroidMod.Instance, nameof(ChoziteGreaves), EquipType.Legs);
			}
			return msaEqu;
		}
		public static Asset<Texture2D> GetGreavesGlow(PlayerDrawSet info)
		{
			Player drawPlayer = info.drawPlayer;
			string tex = ModContent.GetInstance<PowerSuitGreaves>().Texture + "_Legs_Glow";
			if (drawPlayer.armor[2].type == ModContent.ItemType<PowerSuitGreaves>())
			{
				tex = ModContent.GetInstance<PowerSuitGreaves>().Texture + "_Legs_Glow";
				ModSuitAddon[] msa = GetPowerSuit(info.drawPlayer);
				for (int i = 0; i < msa.Length; i++)
				{
					if (msa[i] == null) { continue; }
					string temp = msa[i].ArmorTextureLegs;
					if (temp != "" && temp != null)
					{
						tex = temp + "_Glow";
					}
				}
			}
			else if (drawPlayer.armor[2].type == ModContent.ItemType<ChoziteGreaves>())
			{
				tex = ModContent.GetInstance<ChoziteGreaves>().Texture + "_Legs_Glow";
			}

			return ModContent.Request<Texture2D>(tex);
		}

		internal static ModSuitAddon[] GetPowerSuit(Player player)
		{
			PowerSuitBreastplate armor;
			if (player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>())
			{
				armor = player.armor[1].ModItem as PowerSuitBreastplate;
			}
			else if (player.armor[11].type == ModContent.ItemType<PowerSuitBreastplate>())
			{
				armor = player.armor[11].ModItem as PowerSuitBreastplate;
			}
			else { return new ModSuitAddon[2] { null, null }; }
			Item[] sa = armor.SuitAddons;
			ModSuitAddon[] msa = new ModSuitAddon[2];
			for (int i = SuitAddonSlotID.Suit_Barrier; i <= SuitAddonSlotID.Suit_Primary; i++)
			{
				if (sa[i].type == ItemID.None) { msa[i - SuitAddonSlotID.Suit_Barrier] = null; continue; }
				msa[i - SuitAddonSlotID.Suit_Barrier] = SuitAddonLoader.GetAddon(sa[i]);
			}
			return msa;
		}
	}
}
