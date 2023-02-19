using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class OmegaPirateBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag (Omega Pirate)");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

			ItemID.Sets.BossBag[Type] = true;
			SacrificeTotal = 3;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.expert = true;
			Item.rare = ItemRarityID.Expert;
		}

		public override bool CanRightClick() => true;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.PurePhazon>(), 1, 30, 41));
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.OmegaPirateMusicBox>(), 6));
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vanity.OmegaPirateMask>(), 8));
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.OmegaPirateTrophy>(), 11));
		}
	}
}
