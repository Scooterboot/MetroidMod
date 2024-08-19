using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class SmoothCrateria : GenericTile
	{
		public override Color MapColor => Color.DarkCyan;
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.PurpleMoss;
	}
}
