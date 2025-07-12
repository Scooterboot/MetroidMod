using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class NorfairPurpleMass : GenericTile
	{
		public override Color MapColor => new(173, 48, 148);
		public override SoundStyle HitSound => SoundID.Dig;
		public override int DustType => DustID.PurpleCrystalShard;
	}
}
