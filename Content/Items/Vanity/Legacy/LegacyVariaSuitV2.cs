using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidModPorted.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyVariaSuitV2Breastplate : LegacyVariaSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit V2 Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.LightRed;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyVariaSuitV2Helmet>() && body.type == ModContent.ItemType<LegacyVariaSuitV2Breastplate>() && legs.type == ModContent.ItemType<LegacyVariaSuitV2Greaves>();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyVariaSuitV2Greaves : LegacyVariaSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit V2 Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.LightRed;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyVariaSuitV2Helmet : LegacyVariaSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Varia Suit V2 Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.LightRed;
		}
	}
}
