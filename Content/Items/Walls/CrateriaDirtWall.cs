using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class CrateriaDirtWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Maridia Brick Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.CrateriaDirtWall>());
		}
	}
}
