using System.Collections.Generic;
using MetroidMod.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common.Systems
{
	internal class ChoziteWrenchAssistSystem : ModSystem
	{
		private readonly HashSet<Point16> hitLocations = [];

		public void HitTile(int i, int j)
		{
			if (hitLocations.Add(new(i, j)))
			{
				if (MSystem.mBlockType[i, j] != BreakableTileID.None)
				{
					MSystem.dontRegen[i, j] = !MSystem.dontRegen[i, j];
					Wiring.ReActive(i, j);
				}
			}
		}

		public override void PostUpdateEverything()
		{
			bool stoppedUsingWrench = !Main.LocalPlayer.ItemAnimationActive;
			
			if(stoppedUsingWrench)
			{
				hitLocations.Clear();
			}
		}
	}
}
