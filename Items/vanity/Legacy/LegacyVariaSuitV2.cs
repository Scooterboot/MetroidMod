using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyVariaSuitV2Breastplate : LegacyVariaSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit V2 Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 4;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyVariaSuitV2Helmet") && body.type == mod.ItemType("LegacyVariaSuitV2Breastplate") && legs.type == mod.ItemType("LegacyVariaSuitV2Greaves"));
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyVariaSuitV2Greaves : LegacyVariaSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit V2 Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 4;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyVariaSuitV2Helmet : LegacyVariaSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit V2 Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 4;
		}
	}
}