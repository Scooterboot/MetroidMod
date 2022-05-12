using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Vanity.Legacy
{
	[AutoloadEquip(EquipType.Body)]
	public class LegacySolarLightSuitBreastplate : LegacyLightSuitBreastplate
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Solar Light Suit Breastplate");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<LegacySolarLightSuitHelmet>() && body.type == ModContent.ItemType<LegacySolarLightSuitBreastplate>() && legs.type == ModContent.ItemType<LegacySolarLightSuitGreaves>();
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
	public class LegacySolarLightSuitGreaves : LegacyLightSuitGreaves
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Solar Light Suit Greaves");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class LegacySolarLightSuitHelmet : LegacyLightSuitHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Solar Light Suit Helmet");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Red;
		}
	}
}
