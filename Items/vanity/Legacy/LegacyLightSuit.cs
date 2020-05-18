using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyLightSuitBreastplate : LegacyDarkSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Light Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 7;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyLightSuitHelmet") && body.type == mod.ItemType("LegacyLightSuitBreastplate") && legs.type == mod.ItemType("LegacyLightSuitGreaves"));
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.thrusters = false;
			mp.visorGlowColor = new Color(255, 248, 224);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyLightSuitGreaves : LegacyDarkSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Light Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 7;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyLightSuitHelmet : LegacyDarkSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Light Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 7;
		}
	}
}