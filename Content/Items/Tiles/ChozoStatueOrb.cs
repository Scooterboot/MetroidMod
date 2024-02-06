using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ChozoStatueOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 50;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 134;
			Item.maxStack = 50;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			Item.rare = ItemRarityID.LightRed;
			Item.value = 50000;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.ChozoStatueOrb>();
		}
	}
}
