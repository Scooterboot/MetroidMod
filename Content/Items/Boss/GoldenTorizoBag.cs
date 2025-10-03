using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class GoldenTorizoBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Treasure Bag (Golden Torizo)");
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

			ItemID.Sets.BossBag[Type] = true;
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
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Accessories.ScrewAttack>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TorizoMusicBox>(), 6));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Miscellaneous.EnergyShard>(), 1, 60, 144));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.GoldenTorizoTrophy>(), 11));

			LeadingConditionRule Legendary = new LeadingConditionRule(new Conditions.ZenithSeedIsUp());
			LeadingConditionRule Worthy = new LeadingConditionRule(new Conditions.ForTheWorthyIsUp());
			Legendary.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Weapons.TorizoClaws>(), 3));
			Legendary.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Weapons.TorizoSpitter>(), 3));
			Legendary.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tiles.ChoziteOre>(), 1, 30, 90));
			//Legendary.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Vanity.TorizoMask>(), 8));
			//Legendary.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tiles.TorizoTrophy>(), 11));
			//Legendary.OnSuccess(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Tiles.TorizoRelic>()));
			Worthy.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Weapons.TorizoClaws>(), 3));
			Worthy.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Weapons.TorizoSpitter>(), 3));
			Worthy.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tiles.ChoziteOre>(), 1, 30, 90));
			//Worthy.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Vanity.TorizoMask>(), 8));
			//Worthy.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tiles.TorizoTrophy>(), 11));
			//Worthy.OnSuccess(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Tiles.TorizoRelic>()));
			itemLoot.Add(Worthy);
			itemLoot.Add(Legendary);
		}
	}
}

