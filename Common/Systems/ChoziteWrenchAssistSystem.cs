using System.Collections.Generic;
using MetroidMod.Content.Items.Tiles.Destroyable;
using MetroidMod.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common.Systems
{
	internal class ChoziteWrenchAssistSystem : ModSystem
	{
		private readonly HashSet<Point16> hitLocations = [];

		public bool HitTile(int i, int j)
		{
			if (hitLocations.Add(new(i, j)))
			{
				if (MSystem.mBlockType[i, j] != BreakableTileID.None)
				{
					FakeBlock.SetRegen(i, j, !FakeBlock.Regens(i, j));
					return true;
				}
			}

			return false;
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
