using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class MoltenBubble : GenericTile
	{
		public override Color MapColor => Color.DarkGreen;
		public override SoundStyle HitSound => SoundID.Dig;
		public override int DustType => DustID.Glass;

		public override void SetExtraTileDefaults()
		{
			// Prevents weird shadows from forming around the block as you place more
			Main.tileNoSunLight[Tile.Type] = false;
		}
	}
}
