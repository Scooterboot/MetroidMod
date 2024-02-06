using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyGravitySuitBreastplate : LegacyVariaSuitV2Breastplate
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Gravity Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyGravitySuitHelmet>() && body.type == ModContent.ItemType<LegacyGravitySuitBreastplate>() && legs.type == ModContent.ItemType<LegacyGravitySuitGreaves>();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyGravitySuitGreaves : LegacyVariaSuitV2Greaves
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Gravity Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyGravitySuitHelmet : LegacyVariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Gravity Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
}
