using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Content.Tiles.Hatch;
using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Content.Hatches
{
	internal class HatchPassThroughSystem : ModSystem
	{
		private static IEnumerable<int> GetOpenHatchTileTypes()
		{
			yield return ModContent.TileType<BlueHatchOpen>();
			yield return ModContent.TileType<BlueHatchOpenVertical>();
			yield return ModContent.TileType<RedHatchOpen>();
			yield return ModContent.TileType<RedHatchOpenVertical>();
			yield return ModContent.TileType<YellowHatchOpen>();
			yield return ModContent.TileType<YellowHatchOpenVertical>();
			yield return ModContent.TileType<GreenHatchOpen>();
			yield return ModContent.TileType<GreenHatchOpenVertical>();
			yield return ModContent.TileType<LockedHatchOpen>();
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
