using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class VanityFusionOmegaSuitBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
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
			return head.type == ModContent.ItemType<VanityDreadSuitHelmet>() && body.type == ModContent.ItemType<VanityDreadSuitBreastplate>() && legs.type == ModContent.ItemType<VanityDreadSuitGreaves>();
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
	public class VanityFusionOmegaSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
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
	public class VanityFusionOmegaSuitHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
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
