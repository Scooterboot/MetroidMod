using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Content.Hatches
{
	internal class HatchPassThroughSystem : ModSystem
	{
		private static IEnumerable<int> GetOpenHatchTileTypes()
		{
			foreach(HatchTile hatchTile in ModContent.GetContent<HatchTile>())
			{
				if(hatchTile.Open)
				{
					yield return hatchTile.Type;
				}
			}
		}

		public override void PreUpdateEntities()
		{
			foreach(int type in GetOpenHatchTileTypes())
			{
				Main.tileSolid[type] = false;
			}
		}

		public override void PostUpdateTime()
		{
			foreach (int type in GetOpenHatchTileTypes())
			{
				Main.tileSolid[type] = true;
			}
		}
	}
}
