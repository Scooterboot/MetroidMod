using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class PhazestoneWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.PhazestoneWall>());
		}
	}
}
