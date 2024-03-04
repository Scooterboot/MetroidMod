using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityGravityDreadSuitBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gravity 'Dread' Suit Breastplate");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VanityGravityDreadSuitHelmet>() && body.type == ModContent.ItemType<VanityGravityDreadSuitBreastplate>() && legs.type == ModContent.ItemType<VanityGravityDreadSuitGreaves>();
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
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VanityGravityDreadSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gravity 'Dread' Suit Greaves");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class VanityGravityDreadSuitHelmet : ModItem
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Vanity/VanityDreadSuitHelmet";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gravity 'Dread' Suit Helmet");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
	}
}
