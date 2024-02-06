using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyDarkSuitBreastplate : LegacyVariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Dark Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyDarkSuitHelmet>() && body.type == ModContent.ItemType<LegacyDarkSuitBreastplate>() && legs.type == ModContent.ItemType<LegacyDarkSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.thrusters = false;
			mp.visorGlowColor = new Color(255, 64, 64);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyDarkSuitGreaves : LegacyVariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Dark Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyDarkSuitHelmet : LegacyVariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Dark Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
}
