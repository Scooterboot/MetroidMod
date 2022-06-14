using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityPEDSuitBreastplate : VanityVariaSuitV2Breastplate
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated P.E.D. Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityPEDSuitHelmet>() && body.type == ModContent.ItemType<VanityPEDSuitBreastplate>() && legs.type == ModContent.ItemType<VanityPEDSuitGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			base.UpdateVanitySet(P);
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.visorGlowColor = new Color(0, 228, 255);
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityPEDSuitGreaves : VanityVariaSuitV2Greaves
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/PEDSuit/PEDSuitGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated P.E.D. Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityPEDSuitHelmet : VanityVariaSuitV2Helmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated P.E.D. Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
}
