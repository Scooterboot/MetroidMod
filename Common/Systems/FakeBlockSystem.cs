using System.Collections.Generic;
using MetroidMod.Content.Items.Tiles.Destroyable;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.Systems
{
	public struct FakeBlockSystem : ITileData
	{
		private byte Data;
		public ushort Type;
		//public static ushort[,] mBlockType = new ushort[Main.maxTilesX, Main.maxTilesY];
		/// <summary>
		/// Checks and returns if a breakable tile is present at the given coodinates.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		/// 
		public bool ExistsAt
		{
			readonly get => TileDataPacking.GetBit(Data, offset: 0);
			set => Data = (byte)TileDataPacking.SetBit(value, Data, offset: 0);
		}
		//public static bool ExistsAt(int i, int j)
		//{
		//	return FakeBlockSystem.mBlockType[i, j] != BreakableTileID.None;
		//}
		///// <summary>
		///// Checks and returns if a breakable tile at the given coordinates is the given ID.
		///// </summary>
		///// <param name="i"></param>
		///// <param name="j"></param>
		///// <param name="placeType"></param>
		///// <returns></returns>
		//public static bool ExistsAt(int i, int j, ushort placeType)
		//{
		//	return FakeBlockSystem.mBlockType[i, j] == placeType;
		//}

		public bool SetRegen
		{
			readonly get => TileDataPacking.GetBit(Data, offset: 1);
			set => Data = (byte)TileDataPacking.SetBit(value, Data, offset: 1);
			//bool old = MSystem.dontRegen[i, j];
			//MSystem.dontRegen[i, j] = !regen;
			//return old != regen;
		}

		//public static bool Regens(int i, int j)
		//{
		//	return !MSystem.dontRegen[i, j];
		//}

		//why does this have a player overload
		//public static bool Place(Player player, int i, int j, ushort placeType)
		//{
		//	if (Main.tile[i, j].Get<FakeBlockSystem>().ExistsAt) return false;

		//	Vector2 position = new Vector2(i, j).ToWorldCoordinates(); //idk if this'll come up again but don't send this value around, it's adjusted for world coordinates. just use the vars		-Z
		//	FakeBlockSystem.mBlockType[i, j] = placeType;
		//	SoundEngine.PlaySound(SoundID.Dig, position);
		//	if (Main.netMode != NetmodeID.SinglePlayer)
		//	{
		//		//Packet to tell the server a breakable's been placed
		//		ModPacket placeBreakableBlock = ModContent.GetInstance<MetroidMod>().GetPacket();
		//		placeBreakableBlock.Write((byte)MetroidMessageType.BreakableBlockUpdate);
		//		placeBreakableBlock.Write7BitEncodedInt(i);
		//		placeBreakableBlock.Write7BitEncodedInt(j);
		//		placeBreakableBlock.Write(false); //Are we removing the tile? No? False then
		//		placeBreakableBlock.Write7BitEncodedInt(placeType); //why is the breakable type an unsigned short? can't send those in a packet
		//		placeBreakableBlock.Send(-1, player.whoAmI);
		//	}

		//	return true;
		//}
		public bool Place
		{
			readonly get => TileDataPacking.GetBit(Data, offset: 2);
			set => Data = (byte)TileDataPacking.SetBit(value, Data, offset: 2);
			//bool old = MSystem.dontRegen[i, j];
			//MSystem.dontRegen[i, j] = !regen;
			//return old != regen;
		}
	}
}
