using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Content.Hatches.Behavior;
using MetroidMod.Content.Hatches.Variants;
using MetroidMod.Content.NPCs.Mobs.Bug;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Content.Hatches
{
	internal class HatchTileEntity : ModTileEntity
	{
		public HatchTile Tile => ModContent.GetModTile(Main.tile[Position].TileType) as HatchTile;
		public ModHatch ModHatch => Tile.Hatch;
		public bool IsPhysicallyOpen => Tile.Open;


		public HatchState State = new();


		/// <summary>
		/// Call this method after calling any method on the State,
		/// this will let other multiplayer entities know what happened.
		/// </summary>
		public void SyncState()
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				DebugAssist.NewTextMP($"Hatch state sent: {State}");
				GetSyncPacket().Send();
			}
		}

		public ModPacket GetSyncPacket()
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)MetroidMessageType.ChangeHatchOpenState);
			packet.Write(Position.X);
			packet.Write(Position.Y);
			WriteState(packet);
			return packet;
		}

		public void WriteState(BinaryWriter writer)
		{
			writer.Write((byte)State.DesiredState);
			writer.Write((byte)State.LockStatus);
			writer.Write((byte)State.BlueConversion);
		}

		public override void Update()
		{
		}

		public override void SaveData(TagCompound tag)
		{
			if (State.DesiredState != default) tag["DesiredState"] = (int)State.DesiredState;
			if (State.BlueConversion != default) tag["BlueConversion"] = (int)State.BlueConversion;
			if (State.LockStatus != default) tag["LockStatus"] = (int)State.LockStatus;
		}

		public override void LoadData(TagCompound tag)
		{
			State.DesiredState = (HatchDesiredState)tag.Get<int>("DesiredState");
			State.BlueConversion = (HatchBlueConversionStatus)tag.Get<int>("BlueConversion");
			State.LockStatus = (HatchLockStatus)tag.Get<int>("LockStatus");
		}



		// Currently, the only usage of the NetSend and NetReceive methods is to
		// sync state of hatches when a player first joins, everything else is done via ModPackets.
		public override void NetSend(BinaryWriter writer)
		{
			WriteState(writer);
		}

		public override void NetReceive(BinaryReader reader)
		{
			State.DesiredState = (HatchDesiredState)reader.ReadByte();
			State.LockStatus = (HatchLockStatus)reader.ReadByte();
			State.BlueConversion = (HatchBlueConversionStatus)reader.ReadByte();
		}

		/// <summary>
		/// Get all of the hatch tile entities that exist in the world.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<HatchTileEntity> GetAll()
		{
			foreach(int id in ByID.Keys)
			{
				if (ByID[id] is HatchTileEntity hatch)
				{
					if (hatch.Tile != null)
					{
						yield return hatch;
					}
				}
			}
		}


		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.HasTile && ModContent.GetModTile(tile.TileType) is HatchTile;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 4, 4);

				// Sync the placement of the tile entity with other clients
				// The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
				NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
				return -1;
			}

			// ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
			int placedEntity = Place(i, j);
			return placedEntity;
		}

		public override void OnNetPlace()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
			}
		}
	}
}
