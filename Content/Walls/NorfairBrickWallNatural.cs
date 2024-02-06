using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Systems;

namespace MetroidMod.Content.Walls
{
	public class NorfairBrickWallNatural : ModWall
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Walls/NorfairBrickWall";
		public override void SetStaticDefaults()
		{
			DustType = 87;

			AddMapEntry(new Color(68, 42, 35));
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
