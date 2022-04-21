using System;
using Terraria.ModLoader;

namespace MetroidModPorted.Common.Systems
{
	public class MBiomesSystem : ModSystem
	{
		public int nesCrateriaBlockCount;
		public int nesBrinstarBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			nesCrateriaBlockCount = tileCounts[ModContent.TileType<Content.Tiles.NESCrateriaBlock>()];
			//nesBrinstarBlockCount = tileCounts[ModContent.TileType<Content.Tiles.NESBrinstarBlock>()];
		}
	}
}
