using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class MetroidHiveWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Sand;

			AddMapEntry(new Color(65, 55, 17));
		}
	}
}
