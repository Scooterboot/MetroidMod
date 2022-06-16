using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityDarkSuitBreastplate : VanityVariaSuitV2Breastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/DarkSuit/DarkSuitBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Dark Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityDarkSuitHelmet>() && body.type == ModContent.ItemType<VanityDarkSuitBreastplate>() && legs.type == ModContent.ItemType<VanityDarkSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.visorGlowColor = new Color(255, 64, 64);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityDarkSuitGreaves : VanityVariaSuitV2Greaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/DarkSuit/DarkSuitGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Dark Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityDarkSuitHelmet : VanityVariaSuitV2Helmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/DarkSuit/DarkSuitHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Dark Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
}
