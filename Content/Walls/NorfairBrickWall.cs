using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidModPorted.Content.Walls
{
	public class NorfairBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = 87;
			ItemDrop = ModContent.ItemType<Items.Walls.NorfairBrickWall>();

			AddMapEntry(new Color(168, 104, 87));
		}
	}
}
