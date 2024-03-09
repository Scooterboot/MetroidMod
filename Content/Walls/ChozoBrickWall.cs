using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class ChozoBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = 87;

			AddMapEntry(new Color(67, 46, 9));
		}
	}
}
