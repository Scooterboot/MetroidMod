using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class SerrisBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Treasure Bag (Serris)");
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			//Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));

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
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.SerrisCoreX>(), 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.SerrisMusicBox>(), 6));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vanity.SerrisMask>(), 8));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.SerrisTrophy>(), 11));
		}
	}
}

