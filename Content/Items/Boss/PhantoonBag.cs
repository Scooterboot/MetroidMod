using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class PhantoonBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.ItemNoGravity[Item.type] = true;

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
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.GravityGel>(), 1, 20, 51));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.KraidPhantoonMusicBox>(), 6));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vanity.PhantoonMask>(), 8));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.PhantoonTrophy>(), 11));
		}
	}
}

