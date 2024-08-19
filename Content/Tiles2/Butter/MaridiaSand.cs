using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class MaridiaSand : GenericTile
	{
		public override Color MapColor => Color.SandyBrown;
		public override SoundStyle HitSound => SoundID.Dig;
		public override int DustType => DustID.Sand;
	}
}
