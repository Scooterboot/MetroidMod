﻿using Terraria.GameContent.ItemDropRules;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Vanity.Contributor
{
	[AutoloadEquip(EquipType.Head)]
	public class EzloHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ezlo Hat");
			Tooltip.SetDefault("Wandering Spider's contributor vanity item");

			if (!Main.dedServ)
			{
				ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			}
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(25);
			Item.vanity = true;
		}

		public override bool CanRightClick() => true;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EzloHatHair>()));
		}
	}
}
