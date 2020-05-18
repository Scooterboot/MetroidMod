using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Items.vanity.Legacy
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
			item.rare = 7;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (head.type == mod.ItemType("LegacyPhazonSuitHelmet") && body.type == mod.ItemType("LegacyPhazonSuitBreastplate") && legs.type == mod.ItemType("LegacyPhazonSuitGreaves"));
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
			item.rare = 7;
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
			item.rare = 7;
		}
	}
}