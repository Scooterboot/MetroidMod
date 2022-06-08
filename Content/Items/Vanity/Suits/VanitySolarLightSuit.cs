using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public abstract class VanitySolarLightSuitBreastplate : VanityLightSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Solar Light Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanitySolarLightSuitHelmet>() && body.type == ModContent.ItemType<VanitySolarLightSuitBreastplate>() && legs.type == ModContent.ItemType<VanitySolarLightSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.thrusters = false;
			mp.visorGlowColor = new Color(255, 238, 127);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public abstract class VanitySolarLightSuitGreaves : VanityLightSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Solar Light Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public abstract class VanitySolarLightSuitHelmet : VanityLightSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/SolarAugment/SolarAugmentHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Solar Light Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
}
