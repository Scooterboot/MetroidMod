using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyVariaSuitBreastplate : LegacyPowerSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 3;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyVariaSuitHelmet") && body.type == mod.ItemType("LegacyVariaSuitBreastplate") && legs.type == mod.ItemType("LegacyVariaSuitGreaves"));
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyVariaSuitGreaves : LegacyPowerSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 3;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyVariaSuitHelmet : LegacyPowerSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 3;
		}
	}
}