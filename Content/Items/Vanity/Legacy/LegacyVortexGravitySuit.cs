using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacyVortexGravitySuitBreastplate : LegacyTerraGravitySuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vortex Gravity Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacyVortexGravitySuitHelmet>() && body.type == ModContent.ItemType<LegacyVortexGravitySuitBreastplate>() && legs.type == ModContent.ItemType<LegacyVortexGravitySuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(67, 255, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class LegacyVortexGravitySuitGreaves : LegacyTerraGravitySuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vortex Gravity Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacyVortexGravitySuitHelmet : LegacyTerraGravitySuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Vortex Gravity Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
}
