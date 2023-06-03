using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityPhazonSuitBreastplate : VanityGravitySuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitBreastplate";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Phazon Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityPhazonSuitHelmet>() && body.type == ModContent.ItemType<VanityPhazonSuitBreastplate>() && legs.type == ModContent.ItemType<VanityPhazonSuitGreaves>();
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
	public class VanityPhazonSuitGreaves : VanityGravitySuitGreaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitGreaves";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Phazon Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityPhazonSuitHelmet : VanityGravitySuitHelmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/PhazonSuit/PhazonSuitHelmet";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Phazon Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
}
