using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	/// <summary>
	/// Utility class for managing hatch tiles. Given that open and closed hatches
	/// are currently completely separate tiles, this class can also be used for toggling
	/// between those two states.
	/// </summary>
	internal static class HatchTilePlacement
	{
		/// <summary>
		/// Insert a hatch with the specified tile type at the provided location.
		/// It will replace all tiles in the target area without doing any checks.
		/// This method should NOT be used to toggle between hatch states, as the old
		/// hatch state will likely be discarded.
		/// </summary>
		/// <param name="tileType">The tile type of the hatch. It determines its type, open state and orientation.</param>
		/// <param name="x">Top-left tile X of the hatch.</param>
		/// <param name="y">Top-left tile Y of the hatch.</param>
		public static void PlaceHatchAt(int tileType, int x, int y)
		{

			if(TileUtils.TryGetTileEntityAs(x, y, out HatchTileEntity tileEntity))
			{
				// If we are replacing an existing hatch, make sure we remove the old tile entity
				// this way we can create a new one with fresh state.
				tileEntity.Kill(x, y);
			}

			SetHatchTilesAt(tileType, x, y);
			ModContent.GetInstance<HatchTileEntity>().Place(x, y);
		}

		/// <summary>
		/// Replace the specified area with hatch tiles of the specified type.
		/// This method should only be used when there's a hatch at the specified location,
		/// say, for toggling between open and closed hatch tiles.
		/// </summary>
		/// <param name="tileType">The tile type of the hatch. It determines its type, open state and orientation.</param>
		/// <param name="x">Top-left tile X of the hatch.</param>
		/// <param name="y">Top-left tile Y of the hatch.</param>
		public static void SetHatchTilesAt(int tileType, int x, int y)
		{
			for (int j = 0; j < 4; j++)
			{
				for (int i = 0; i < 4; i++)
				{
					Tile tile = Main.tile[x + i, y + j];

					tile.HasTile = true;
					tile.TileType = (ushort)tileType;
					tile.TileFrameX = (short)(i * 18);
					tile.TileFrameY = (short)(j * 18);
				}
			}

			// TODO is this needed?
			// NetMessage.SendTileSquare(-1, x, y, 4, 4, TileChangeType.None);
		}
	}
}
