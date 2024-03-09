using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityHazardShieldSuitBreastplate : VanityPEDSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitBreastplate";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Hazard Shield Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityHazardShieldSuitHelmet>() && body.type == ModContent.ItemType<VanityHazardShieldSuitBreastplate>() && legs.type == ModContent.ItemType<VanityHazardShieldSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.visorGlowColor = new Color(0, 228, 255);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityHazardShieldSuitGreaves : VanityPEDSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitGreaves";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Hazard Shield Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityHazardShieldSuitHelmet : VanityPEDSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/HazardShieldSuit/HazardShieldSuitHelmet";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Hazard Shield Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
}
