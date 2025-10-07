using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class ChozoBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = 87;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Items.Walls.ChozoBrickWallNatural>();

			AddMapEntry(new Color(67, 46, 9));
		}
	}
}
