using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityLightSuitBreastplate : VanityDarkSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/LightSuit/LightSuitBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Light Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityLightSuitHelmet>() && body.type == ModContent.ItemType<VanityLightSuitBreastplate>() && legs.type == ModContent.ItemType<VanityLightSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.visorGlowColor = new Color(255, 248, 224);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityLightSuitGreaves : VanityDarkSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/LightSuit/LightSuitGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Light Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityLightSuitHelmet : VanityDarkSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/LightSuit/LightSuitHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Light Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Lime;
		}
	}
}
