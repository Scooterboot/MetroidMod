using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace MetroidMod.Content.Items.Boss
{
	public class KraidBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag (Kraid)");
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
			Item.rare = -12;
		}

		public override bool CanRightClick() => true;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.KraidTissue>(), 1, 20, 31));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.UnknownPlasmaBeam>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.KraidPhantoonMusicBox>(), 6));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vanity.KraidMask>(), 8));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.KraidTrophy>(), 11));
		}
	}
}

