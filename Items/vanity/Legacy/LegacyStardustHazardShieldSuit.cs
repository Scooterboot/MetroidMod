using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyStardustHazardShieldSuitBreastplate : LegacyHazardShieldSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Stardust Hazard Shield Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 7;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyStardustHazardShieldSuitHelmet") && body.type == mod.ItemType("LegacyStardustHazardShieldSuitBreastplate") && legs.type == mod.ItemType("LegacyStardustHazardShieldSuitGreaves"));
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
			DisplayName.SetDefault("Ancient Stardust Hazard Shield Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 7;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyStardustHazardShieldSuitHelmet : LegacyHazardShieldSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Stardust Hazard Shield Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 7;
		}
	}
}