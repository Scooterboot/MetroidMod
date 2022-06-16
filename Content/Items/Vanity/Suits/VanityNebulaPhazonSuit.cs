using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityNebulaPhazonSuitBreastplate : VanityPhazonSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Nebula Phazon Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityNebulaPhazonSuitHelmet>() && body.type == ModContent.ItemType<VanityNebulaPhazonSuitBreastplate>() && legs.type == ModContent.ItemType<VanityNebulaPhazonSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			P.GetModPlayer<MPlayer>().visorGlowColor = new Color(255, 55, 255);
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
			player.armorEffectDrawOutlines = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityNebulaPhazonSuitGreaves : VanityPhazonSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Nebula Phazon Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityNebulaPhazonSuitHelmet : VanityPhazonSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/NebulaAugment/NebulaAugmentHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Nebula Phazon Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
}
