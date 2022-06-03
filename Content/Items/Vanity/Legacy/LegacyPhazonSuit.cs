using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyPhazonSuitBreastplate : LegacyGravitySuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Phazon Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyPhazonSuitHelmet>() && body.type == ModContent.ItemType<LegacyPhazonSuitBreastplate>() && legs.type == ModContent.ItemType<LegacyPhazonSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 64, 0);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyPhazonSuitGreaves : LegacyGravitySuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Phazon Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyPhazonSuitHelmet : LegacyGravitySuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Phazon Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
}
