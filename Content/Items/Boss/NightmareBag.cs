using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace MetroidMod.Content.Items.Boss
{
	public class NightmareBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			//Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));

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
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.NightmareCoreX>(), 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.NightmareCoreXFragment>(), 1, 15, 25));
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.NightmareMusicBox>(), 6));
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vanity.NightmareMask>(), 8));
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.NightmareTrophy>(), 11));
		}
	}
}

