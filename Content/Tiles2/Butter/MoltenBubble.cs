using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class MoltenBubble : GenericTile
	{
		public override Color MapColor => Color.DarkGreen;
		public override SoundStyle HitSound => SoundID.Dig;
		public override int DustType => DustID.Glass;
	}
}
