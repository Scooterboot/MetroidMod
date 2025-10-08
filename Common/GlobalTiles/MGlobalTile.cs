using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using MetroidMod.Content.Tiles;

namespace MetroidMod.Common.GlobalTiles
{
	public class MGlobalTile : GlobalTile
	{
		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			if (Main.tile[i, j - 1].TileType == ModContent.TileType<MetroidDeepnestEntrance>() 
				|| Main.tile[i, j - 1].TileType == ModContent.TileType<MetroidDeepnestExit>())
			{
				if (Main.tile[i, j].TileType != ModContent.TileType<MetroidDeepnestEntrance>() 
					&& Main.tile[i, j].TileType != ModContent.TileType<MetroidDeepnestExit>())
				{
					return false;
				}
			}

			return base.CanKillTile(i, j, type, ref blockDamaged);
		}
	}
}
