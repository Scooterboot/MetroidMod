using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyPEDSuitBreastplate : LegacyVariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient P.E.D. Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyPEDSuitHelmet>() && body.type == ModContent.ItemType<LegacyPEDSuitBreastplate>() && legs.type == ModContent.ItemType<LegacyPEDSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.thrusters = false;
			mp.visorGlowColor = new Color(0, 228, 255);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyPEDSuitGreaves : LegacyVariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient P.E.D. Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = 5;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyPEDSuitHelmet : LegacyVariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient P.E.D. Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = 5;
		}
	}
}
