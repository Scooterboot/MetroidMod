using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Terraria;

namespace MetroidMod
{
	public static class TileUtils
	{
		/// <summary>
		/// Atttempts to find the top-left corner of a multitile at location (<paramref name="x"/>, <paramref name="y"/>)
		/// </summary>
		/// <param name="x">The tile X-coordinate</param>
		/// <param name="y">The tile Y-coordinate</param>
		/// <returns>The tile location of the multitile's top-left corner, or the input location if no tile is present or the tile is not part of a multitile</returns>
		public static Point16 GetTopLeftTileInMultitile(int x, int y)
		{
			Tile tile = Main.tile[x, y];

			int frameX = 0;
			int frameY = 0;

			if (tile.HasTile)
			{
				int style = 0, alt = 0;
				TileObjectData.GetTileInfo(tile, ref style, ref alt);
				TileObjectData data = TileObjectData.GetTileData(tile.TileType, style, alt);

				if (data != null)
				{
					int size = 16 + data.CoordinatePadding;

					frameX = tile.TileFrameX % (size * data.Width) / size;
					frameY = tile.TileFrameY % (size * data.Height) / size;
				}
			}

			return new Point16(x - frameX, y - frameY);
		}

		/// <summary>
		/// Uses <seealso cref="GetTopLeftTileInMultitile(int, int)"/> to try to get the entity bound to the multitile at (<paramref name="i"/>, <paramref name="j"/>).
		/// </summary>
		/// <typeparam name="T">The type to get the entity as</typeparam>
		/// <param name="i">The tile X-coordinate</param>
		/// <param name="j">The tile Y-coordinate</param>
		/// <param name="entity">The found <typeparamref name="T"/> instance, if there was one.</param>
		/// <returns><see langword="true"/> if there was a <typeparamref name="T"/> instance, or <see langword="false"/> if there was no entity present OR the entity was not a <typeparamref name="T"/> instance.</returns>
		public static bool TryGetTileEntityAs<T>(int i, int j, out T entity) where T : TileEntity
		{
			Point16 origin = GetTopLeftTileInMultitile(i, j);

			// TileEntity.ByPosition is a Dictionary<Point16, TileEntity> which contains all placed TileEntity instances in the world
			// TryGetValue is used to both check if the dictionary has the key, origin, and get the value from that key if it's there
			if (TileEntity.ByPosition.TryGetValue(origin, out TileEntity existing) && existing is T existingAsT)
			{
				entity = existingAsT;
				return true;
			}

			entity = null;
			return false;
		}
	}
}
