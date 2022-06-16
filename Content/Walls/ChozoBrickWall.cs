using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Walls
{
	public class ChozoBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = 87;
			ItemDrop = ModContent.ItemType<Items.Walls.ChozoBrickWall>();

			AddMapEntry(new Color(67, 46, 9));
		}
	}
}
