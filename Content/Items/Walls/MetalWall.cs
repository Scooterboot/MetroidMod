using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class MetalWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Metal Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.MetalWall>());
		}
	}
}
