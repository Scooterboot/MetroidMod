using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyDarkSuitBreastplate : LegacyVariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Dark Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyDarkSuitHelmet") && body.type == mod.ItemType("LegacyDarkSuitBreastplate") && legs.type == mod.ItemType("LegacyDarkSuitGreaves"));
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
			DisplayName.SetDefault("Ancient Dark Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyDarkSuitHelmet : LegacyVariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Dark Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}
	}
}