using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Systems;

namespace MetroidMod.Content.Walls
{
	public class TourianWallNatural : ModWall
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Walls/TourianWall";
		public override void SetStaticDefaults()
		{
			DustType = 87;
			//ItemDrop= ModContent.ItemType<Items.Walls.TourianWall>();

			AddMapEntry(new Color(39, 48, 63));
		}

		public override bool CanExplode(int i, int j)
		{
			return MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid);
		}

		public override void KillWall(int i, int j, ref bool fail)
		{
			if (!MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid) && !WorldGen.generatingWorld)
			{
				fail = true;
			}
			base.KillWall(i, j, ref fail);
		}
	}
}
