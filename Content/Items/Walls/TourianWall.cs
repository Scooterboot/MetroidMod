using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class TourianWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tourian Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.TourianWall>());
		}
	}
}
