using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidModPorted.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityGravitySuitBreastplate : VanityVariaSuitV2Breastplate
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Gravity Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityGravitySuitHelmet>() && body.type == ModContent.ItemType<VanityGravitySuitBreastplate>() && legs.type == ModContent.ItemType<VanityGravitySuitGreaves>();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityGravitySuitGreaves : VanityVariaSuitV2Greaves
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Gravity Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityGravitySuitHelmet : VanityVariaSuitV2Helmet
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/GravitySuit/GravitySuitHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Gravity Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Pink;
		}
	}
}
