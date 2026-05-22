using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class CrateriaStoneWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crateria Stone Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.CrateriaStoneWall>());
		}
	}
}
