using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class MoltenBubble : GenericTile
	{
		public override Color MapColor => new(36, 87, 59);
		// TODO: assign appropiate values for these!
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.Firework_Red;

		public override void SetExtraTileDefaults()
		{
			// These prevent these blocks from creating shadows,
			// which can cause weird visual artifacts otherwise
			Main.tileBlockLight[Tile.Type] = false;
			Main.tileNoSunLight[Tile.Type] = false;
		}
	}
}
