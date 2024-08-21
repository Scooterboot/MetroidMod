using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace MetroidMod.Content.Tiles2.Butter
{
	internal class ZebMetal : GenericTile
	{
		public override Color MapColor => Color.LightSteelBlue;
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.GemDiamond;
	}
}
