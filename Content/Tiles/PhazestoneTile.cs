using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class PhazestoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileDungeon[Type] = true;
			//Main.tileMerge[Type][TileID.Sand] = true;
			//Main.tileMerge[TileID.Sand][Type] = true;

			DustType = DustID.Electric;
			MinPick = 0;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(50, 160, 115));
		}
	}
}
