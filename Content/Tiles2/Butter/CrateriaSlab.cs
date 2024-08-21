using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class CrateriaSlab : GenericTile
	{
		public override Color MapColor => Color.DarkMagenta;
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.PurpleMoss;

		public override void SetExtraTileDefaults()
		{
			// Required for the texture of the tile to display properly
			Main.tileLargeFrames[Tile.Type] = 1;
		}
	}
}
