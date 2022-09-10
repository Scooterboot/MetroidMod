using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Systems;

namespace MetroidMod.Content.Walls
{
	public class ChozoBrickWallNatural : ModWall
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Walls/ChozoBrickWall";
		public override void SetStaticDefaults()
		{
			DustType = 87;
			ItemDrop = ModContent.ItemType<Items.Walls.ChozoBrickWall>();

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
