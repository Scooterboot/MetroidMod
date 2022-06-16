using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Walls
{
	public class ChozoBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.wallHouse[Type] = true;

			DustType = 87;
			ItemDrop = ModContent.ItemType<Items.Walls.ChozoBrickWall>();
		}
	}
}
