
using Microsoft.Xna.Framework;
using SubworldLibrary;

namespace MetroidMod.Common.Players
{
	public partial class MPlayer
	{
		public Vector2 posInRealWorld;
		public override void OnEnterWorld()
		{
			if (SubworldSystem.Current == null && posInRealWorld != Vector2.Zero)
			{
				Player.position = posInRealWorld;
				posInRealWorld = Vector2.Zero;
			}
		}
	}
}
