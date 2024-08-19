using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class AmethystMetal : GenericTile
	{
		public override Color MapColor => Color.Purple;
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.GemAmethyst;
	}
}
