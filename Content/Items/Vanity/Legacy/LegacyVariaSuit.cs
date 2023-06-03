using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyVariaSuitBreastplate : LegacyPowerSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Varia Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyVariaSuitHelmet>() && body.type == ModContent.ItemType<LegacyVariaSuitBreastplate>() && legs.type == ModContent.ItemType<LegacyVariaSuitGreaves>();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyVariaSuitGreaves : LegacyPowerSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Varia Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyVariaSuitHelmet : LegacyPowerSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Varia Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
		}
	}
}
