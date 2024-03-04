using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public abstract class AreaBag : ModItem
	{
		public override string Texture => $"{Mod.Name}/Content/Items/Miscellaneous/AreaBag";
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

			ItemID.Sets.OpenableBag[Type] = true;
			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.buyPrice(0, 0, 50, 0);
		}

		public override bool CanRightClick() => true;
	}
	public class BrinstarBag : AreaBag
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Brinstar Bag");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Green;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.BrinstoneTile>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.GreenBrinstarRootsTile>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.BrinstoneTile>(), 1, 100, 100));
		}
	}
	public class NorfairBag : AreaBag
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Norfair Bag");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Green;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.NorfairBrick>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.NorfairBubbleSM>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.NorfairBubbleZM>(), 1, 100, 100));
		}
	}
	public class ChozoRuinsBag : AreaBag
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Chozo Ruins Bag");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Yellow;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ChozoBrick>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ChozoPillar>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ChozoStatue>(), 1, 1, 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ChozoStatueArm>(), 1, 1, 1));
		}
	}
	public class Sector2TropicBag : AreaBag
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Sector 2 (TRO) Bag");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Green;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TropicPlating1>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TropicPlating2>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TropicPlating3>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TropicPlating4>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TropicPlating5>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TanglevineTile>(), 1, 100, 100));
		}
	}
	public class Sector5ArcticBag : AreaBag
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Sector 5 (ARC) Bag");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Green;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ArcticPlating>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.ExtraArcticPlating>(), 1, 100, 100));
		}
	}
	public class TourianBag : AreaBag
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Tourian Bag");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = ItemRarityID.Green;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TourianPipe>(), 1, 100, 100));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tiles.TourianPipeAccent>(), 1, 100, 100));
		}
	}
}
