using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class CrateriaSlab : GenericTile
	{
		public override Color MapColor => new(50, 41, 76);
		// TODO: assign appropiate values for these!
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.Firework_Red;

		public override void SetExtraTileDefaults()
		{
			// Required for the texture of the tile to display properly
			Main.tileLargeFrames[Tile.Type] = 1;
		}
	}
}
