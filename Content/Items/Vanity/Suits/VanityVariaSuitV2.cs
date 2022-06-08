using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidModPorted.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityVariaSuitV2Breastplate : VanityVariaSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Breastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Varia Suit V2 Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.LightRed;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityVariaSuitV2Helmet>() && body.type == ModContent.ItemType<VanityVariaSuitV2Breastplate>() && legs.type == ModContent.ItemType<VanityVariaSuitV2Greaves>();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityVariaSuitV2Greaves : VanityVariaSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Greaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Varia Suit V2 Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.LightRed;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityVariaSuitV2Helmet : VanityVariaSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidModPorted)}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Helmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Varia Suit V2 Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.LightRed;
		}
	}
}
