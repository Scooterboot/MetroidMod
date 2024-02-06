using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class DarkSamusBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dark Hunter's Breastplate");
			// Tooltip.SetDefault("'Great for impersonating a dark warrior!'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<DarkSamusHelmet>() && body.type == ModContent.ItemType<DarkSamusBreastplate>() && legs.type == ModContent.ItemType<DarkSamusGreaves>();
		}
		public override void UpdateVanitySet(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.isPowerSuit = true;
			mp.visorGlowColor = new Color(36, 159, 222);
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
	public class DarkSamusGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dark Hunter's Greaves");
			// Tooltip.SetDefault("'Great for impersonating a dark warrior!'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class DarkSamusHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dark Hunter's Helmet");
			// Tooltip.SetDefault("'Great for impersonating a dark warrior!'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
