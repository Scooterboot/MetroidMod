using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public abstract class VanityStardustHazardShieldSuitBreastplate : VanityHazardShieldSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/StardustAugment/StardustAugmentBreastplate";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Stardust Hazard Shield Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityStardustHazardShieldSuitHelmet>() && body.type == ModContent.ItemType<VanityStardustHazardShieldSuitBreastplate>() && legs.type == ModContent.ItemType<VanityStardustHazardShieldSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.thrusters = false;
			mp.visorGlowColor = new Color(0, 228, 255);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public abstract class VanityStardustHazardShieldSuitGreaves : VanityHazardShieldSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/StardustAugment/StardustAugmentGreaves";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Stardust Hazard Shield Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public abstract class VanityStardustHazardShieldSuitHelmet : VanityHazardShieldSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/StardustAugment/StardustAugmentHelmet";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Outdated Stardust Hazard Shield Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
}
