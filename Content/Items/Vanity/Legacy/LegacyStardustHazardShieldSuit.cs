using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyStardustHazardShieldSuitBreastplate : LegacyHazardShieldSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Stardust Hazard Shield Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyStardustHazardShieldSuitHelmet>() && body.type == ModContent.ItemType<LegacyStardustHazardShieldSuitBreastplate>() && legs.type == ModContent.ItemType<LegacyStardustHazardShieldSuitGreaves>();
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
	public class LegacyStardustHazardShieldSuitGreaves : LegacyHazardShieldSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Stardust Hazard Shield Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyStardustHazardShieldSuitHelmet : LegacyHazardShieldSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Stardust Hazard Shield Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
}
