using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class MaridiaStone : GenericTile
	{
		public override Color MapColor => Color.Teal;
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.BlueMoss;
	}
}
