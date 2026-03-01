using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class PhazonGrass : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			//Main.tileMerge[Type][TileID.Sand] = true;
			//Main.tileMerge[TileID.Sand][Type] = true;

			DustType = DustID.Dirt;
			MinPick = 0;
			HitSound = SoundID.Dig;

			AddMapEntry(new Color(50, 100, 160));
		}
	}
}
