using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Walls
{
	public class NorfairBoilingWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Norfair Boiling Wall");

			Item.ResearchUnlockCount = 400;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableWall(ModContent.WallType<Content.Walls.NorfairBoilingWall>());
		}
	}
}
