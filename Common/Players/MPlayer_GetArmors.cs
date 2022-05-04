using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted.Common.Players
{
	public partial class MPlayer : ModPlayer
	{
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
		public override void UpdateEquips()
		{
			base.UpdateEquips();
		}
		public override void UpdateVisibleVanityAccessories()
		{
			if ((Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.PowerSuitHelmet>() && Player.armor[10].IsAir) || Player.armor[10].type == ModContent.ItemType<Content.Items.Armors.PowerSuitHelmet>())
			{
				ShouldDrawHelmet = true;
			}
			if ((Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.PowerSuitBreastplate>() && Player.armor[11].IsAir) || Player.armor[11].type == ModContent.ItemType<Content.Items.Armors.PowerSuitBreastplate>())
			{
				ShouldDrawBreastplate = true;
			}
			if ((Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.PowerSuitGreaves>() && Player.armor[12].IsAir) || Player.armor[12].type == ModContent.ItemType<Content.Items.Armors.PowerSuitGreaves>())
			{
				ShouldDrawGreaves = true;
			}
		}
		//MetroidModPorted.Instance.Logger.Debug(result);
		public static int GetHelmet(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(nameof(Content.Items.Armors.PowerSuitHelmet), EquipType.Head);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Item.Name, EquipType.Head);
				if (temp != -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetHelmetGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<Content.Items.Armors.PowerSuitHelmet>().Texture + "_Head_Glow";//"MetroidModPorted/Content/Items/Armors/PowerSuitHelmet_Head_Glow";
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

			return ModContent.Request<Texture2D>(tex);
		}
		/*public static void DrawHelmetArmorColor(Player player, ref int glowMask, ref Color glowMaskColor)
		{

		}*/
		public static int GetBreastplate(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(nameof(Content.Items.Armors.PowerSuitBreastplate), EquipType.Body);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Item.Name, EquipType.Body);
				if (temp != -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetBreastplateGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<Content.Items.Armors.PowerSuitBreastplate>().Texture + "_Body_Glow";//"MetroidModPorted/Content/Items/Armors/PowerSuitBreastplate_Body_Glow";
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

			return ModContent.Request<Texture2D>(tex);
		}
		public static int GetArms(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(nameof(Content.Items.Armors.PowerSuitBreastplate), EquipType.Body);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Item.Name, EquipType.Body);
				if (temp == -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetArmsGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<Content.Items.Armors.PowerSuitBreastplate>().Texture + "_Arms_Glow";
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
		public static int GetGreaves(Player player)
		{
			int msaEqu = MetroidModPorted.Instance.GetEquipSlot(nameof(Content.Items.Armors.PowerSuitGreaves), EquipType.Legs);
			ModSuitAddon[] msa = GetPowerSuit(player);
			for (int i = 0; i < msa.Length; i++)
			{
				if (msa[i] == null) { continue; }
				int temp = msa[i].Mod.GetEquipSlot(msa[i].Item.Name, EquipType.Legs);
				if (temp != -1)
				{
					msaEqu = temp;
				}
			}

			return msaEqu;
		}
		public static Asset<Texture2D> GetGreavesGlow(PlayerDrawSet info)
		{
			string tex = ModContent.GetInstance<Content.Items.Armors.PowerSuitGreaves>().Texture + "_Legs_Glow";//"MetroidModPorted/Content/Items/Armors/PowerSuitGreaves_Legs_Glow";
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
	}
}
