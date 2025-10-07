using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class MetroidHiveWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 400;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<MetroidHiveWallNatural>();
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Content.Walls.MetroidHiveWall>();
			Item.rare = ItemRarityID.Green;
		}
	}
}
