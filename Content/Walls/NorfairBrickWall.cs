using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class NorfairBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = 87;
			//ItemDrop = ModContent.ItemType<Items.Walls.NorfairBrickWall>();

			AddMapEntry(new Color(68, 42, 35));
		}
	}
}
