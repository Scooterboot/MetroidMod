using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class NorfairBubbleSM : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<NorfairBubbleZM>()] = true;

			DustType = DustID.Grass;
			MinPick = 100;
			HitSound = SoundID.Drip;

			AddMapEntry(new Color(13, 88, 33));
		}
	}
}
