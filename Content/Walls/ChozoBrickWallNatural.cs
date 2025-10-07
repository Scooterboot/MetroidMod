using MetroidMod.Common.Systems;
using MetroidMod.Content.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class ChozoBrickWallNatural : ModWall
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Walls/ChozoBrickWall";
		public override void SetStaticDefaults()
		{
			DustType = 87;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Items.Walls.ChozoBrickWall>();
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
		
		public override bool Drop(int i, int j, ref int type)
		{
			type = ModContent.ItemType<Items.Walls.ChozoBrickWall>();
			return true;
		}
	}
}
