using MetroidMod.Content.Items.Accessories;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class TorizoBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
			Item.ResearchUnlockCount = 3;
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
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.TorizoClaws>(), 3));
			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<TorizoEyeNecklace>(), 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.EnergyShard>(), 1, 15, 36));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ChoziteOre>(), 1, 30, 90));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TorizoMusicBox>(), 6));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vanity.TorizoMask>(), 8));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TorizoTrophy>(), 11));
		}
	}
}

