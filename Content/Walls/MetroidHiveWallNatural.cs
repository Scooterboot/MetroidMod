using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class MetroidHiveWallNatural : ModWall
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Walls/MetroidHiveWall";
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Sand;

			AddMapEntry(new Color(65, 55, 17));
		}
		public override bool Drop(int i, int j, ref int type)
		{
			type = ModContent.ItemType<Items.Walls.MetroidHiveWall>();
			return true;
		}
	}
}
