using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	public partial class MPlayer
	{
		public Vector2 posTransferXD;
		public override void OnEnterWorld()
		{
			if (posTransferXD != Vector2.Zero)
			{
				Player.position = posTransferXD;
				posTransferXD = Vector2.Zero;
			}
		}
		// public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		// {
			// TODO: teleport the player out on kill or on respawn? 1/2
			// if (SubworldLibrary.SubworldSystem.Current.FullName == $"{nameof(MetroidMod)}/{nameof(Content.Subworlds.MetroidDeepnest)}" && Player.SpawnX == -1 && Player.SpawnY == -1)
			// {
			// 	SubworldLibrary.SubworldSystem.Exit();
			// }
		// }
	}
}
