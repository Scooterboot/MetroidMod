using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Tiles
{
	public class RedSwitch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Switch");

			SacrificeTotal = 5;
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.RedSwitch>();
		}
	}
}
