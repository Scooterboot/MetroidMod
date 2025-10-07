using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class MetroidHiveWallNatural : ModItem
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Walls/MetroidHiveWall";
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<MetroidHiveWall>();
			Item.ResearchUnlockCount = 400;
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
			Item.createWall = ModContent.WallType<Content.Walls.MetroidHiveWallNatural>();
			ItemID.Sets.DrawUnsafeIndicator[Item.type] = true; 
			Item.rare = ItemRarityID.Green;
		}
	}
}
