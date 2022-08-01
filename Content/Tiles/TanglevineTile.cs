using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Tiles
{
	public class TanglevineTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileCut[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;

			DustType = DustID.YellowStarDust;
			HitSound = SoundID.Grass;

			AddMapEntry(new Color(248, 192, 96));
		}
	}
}
