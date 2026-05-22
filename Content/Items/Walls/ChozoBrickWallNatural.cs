using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class ChozoBrickWallNatural : ModItem
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Walls/ChozoBrickWall";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Brick Wall (Natural)");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			ItemID.Sets.DrawUnsafeIndicator[Item.type] = true; //Hey so apparently they just have a thingy to make the unsafe skull show up.    -Z
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.ChozoBrickWallNatural>());
		}
	}
}
