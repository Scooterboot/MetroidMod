using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyGravitySuitBreastplate : LegacyVariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gravity Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyGravitySuitHelmet") && body.type == mod.ItemType("LegacyGravitySuitBreastplate") && legs.type == mod.ItemType("LegacyGravitySuitGreaves"));
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyGravitySuitGreaves : LegacyVariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gravity Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyGravitySuitHelmet : LegacyVariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gravity Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}
	}
}