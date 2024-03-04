using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class ChozoBrickWallNatural : ModWall
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Walls/ChozoBrickWall";
		public override void SetStaticDefaults()
		{
			DustType = 87;

			AddMapEntry(new Color(67, 46, 9));
		}

		public override bool CanExplode(int i, int j)
		{
			return MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo);
		}

		public override void KillWall(int i, int j, ref bool fail)
		{
			if (!MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) && !WorldGen.generatingWorld)
			{
				fail = true;
			}
			base.KillWall(i, j, ref fail);
		}
	}
}
