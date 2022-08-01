using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Walls
{
	public class NorfairBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			Main.wallDungeon[Type] = true;

			DustType = 87;
			ItemDrop = ModContent.ItemType<Items.Walls.NorfairBrickWall>();

			AddMapEntry(new Color(68, 42, 35));
		}
	}
}
