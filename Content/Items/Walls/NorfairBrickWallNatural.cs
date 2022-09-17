using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Items.Walls
{
	public class NorfairBrickWallNatural : ModItem
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Walls/NorfairBrickWall";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Norfair Brick Wall (Natural)");
			Tooltip.SetDefault("'Welcome to hell.'");

			SacrificeTotal = 400;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Content.Walls.NorfairBrickWallNatural>();
		}
	}
}
