using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity.Suits
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityVariaSuitBreastplate : Legacy.LegacyPowerSuitBreastplate
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitBreastplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Varia Suit Breastplate");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
		}
		public override void UpdateVanitySet(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.isPowerSuit = true;
			mp.visorGlowColor = new Color(0, 248, 112);
			if (P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1) || mp.SMoveEffect > 0) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
			{
				mp.jet = true;
			}
			else if (mp.shineDirection == 0 || mp.shineDirection == 5)
			{
				mp.jet = false;
			}
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityVariaSuitHelmet>() && body.type == ModContent.ItemType<VanityVariaSuitBreastplate>() && legs.type == ModContent.ItemType<VanityVariaSuitGreaves>();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityVariaSuitGreaves : Legacy.LegacyPowerSuitGreaves
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitGreaves";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Varia Suit Greaves");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityVariaSuitHelmet : Legacy.LegacyPowerSuitHelmet
	{
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/SuitAddons/VariaSuit/VariaSuitHelmet";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outdated Varia Suit Helmet");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Orange;
		}
	}
}
