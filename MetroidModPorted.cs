using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using ReLogic;
using ReLogic.Graphics;

using MetroidModPorted.Content.Items;
using MetroidModPorted.Common.UI;
using Terraria.Audio;
using MetroidModPorted.Common.Players;
using Terraria.ModLoader.IO;

namespace MetroidModPorted
{
	public enum MetroidMessageType : byte
	{
		SyncStartPlayerStats,
		SyncPlayerStats,
		PlaySyncedSound
	}

	public class MetroidModPorted : Mod
	{
		internal const int ballSlotAmount = 5;
		internal const int beamSlotAmount = 5;
		internal const int missileSlotAmount = 3;

		public static bool UseAltWeaponTextures;
		public static bool DragablePowerBeamUI;
		public static bool DragableMissileLauncherUI;
		public static bool DragableMorphBallUI;
		public static bool DragableSenseMoveUI;

		public static Color powColor = new(248, 248, 110);
		public static Color iceColor = new(0, 255, 255);
		public static Color waveColor = new(215, 0, 215);
		public static Color waveColor2 = new(239, 153, 239);
		public static Color plaRedColor = new(253, 221, 3);
		public static Color plaGreenColor = new(0, 248, 112);
		public static Color plaGreenColor2 = new(61, 248, 154);
		public static Color novColor = new(50, 255, 1);
		public static Color wideColor = new(255, 210, 255);
		public static Color lumColor = new(209, 255, 250);
		public static MetroidModPorted Instance { get; private set; }

		public static bool DebugDH;

		public int selectedItem = 0;
		public int oldSelectedItem = 0;

		public int[] FrozenStandOnNPCs;

		//public static int beamsRecipeGroupID;
		public static ushort unloadedItemID;

		public override void Load()
		{
			Instance = this;
			//OnHooks.Load();
			//ILHooks.Load();

			FrozenStandOnNPCs = new int[] { ModContent.NPCType<Content.NPCs.Mobs.Utility.Ripper>() };

			DebugDH = false;

			if (!Main.dedServ)
			{
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/Serris"), ModContent.ItemType<Content.Items.Tiles.SerrisMusicBox>(), ModContent.TileType<Content.Tiles.SerrisMusicBox>());
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/Kraid"), ModContent.ItemType<Content.Items.Tiles.KraidPhantoonMusicBox>(), ModContent.TileType<Content.Tiles.KraidPhantoonMusicBox>());
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/Ridley"), ModContent.ItemType<Content.Items.Tiles.RidleyMusicBox>(), ModContent.TileType<Content.Tiles.RidleyMusicBox>());
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/Torizo"), ModContent.ItemType<Content.Items.Tiles.TorizoMusicBox>(), ModContent.TileType<Content.Tiles.TorizoMusicBox>());
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/Nightmare"), ModContent.ItemType<Content.Items.Tiles.NightmareMusicBox>(), ModContent.TileType<Content.Tiles.NightmareMusicBox>());
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/OmegaPirate"), ModContent.ItemType<Content.Items.Tiles.OmegaPirateMusicBox>(), ModContent.TileType<Content.Tiles.OmegaPirateMusicBox>());
			}
		}

		public override void Unload()
		{
			Instance = null;
			//OnHooks.Unload();
			//ILHooks.Unload();
			BeamLoader.Unload();
			SuitAddonLoader.Unload();
		}

		/* NETWORK SYNICNG <<<<< WIP >>>>> */
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MetroidMessageType msgType = (MetroidMessageType)reader.ReadByte();
			switch (msgType)
			{
				case MetroidMessageType.SyncPlayerStats:
				case MetroidMessageType.SyncStartPlayerStats:
					byte playerID = reader.ReadByte();
					MPlayer targetPlayer = Main.player[playerID].GetModPlayer<MPlayer>();
					double statCharge = reader.ReadDouble();
					bool spiderBall = reader.ReadBoolean();
					int boostEffect = reader.ReadInt32();
					int boostCharge = reader.ReadInt32();
					Item[] SuitAddons = new Item[SuitAddonSlotID.Count];
					for (int i = 0; i < SuitAddons.Length; ++i)
					{
						SuitAddons[i] = ItemIO.Receive(reader);
					}

					targetPlayer.statCharge = (float)statCharge;
					targetPlayer.spiderball = spiderBall;
					targetPlayer.boostEffect = boostEffect;
					targetPlayer.boostCharge = boostCharge;
					targetPlayer.SuitAddons = SuitAddons;

					if (msgType == MetroidMessageType.SyncPlayerStats && Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.SyncPlayerStats);
						packet.Write(playerID);
						packet.Write(statCharge);
						packet.Write(spiderBall);
						packet.Write(boostEffect);
						packet.Write(boostCharge);
						for (int i = 0; i < SuitAddons.Length; ++i)
						{
							ItemIO.Send(SuitAddons[i], packet);
						}
						packet.Send(-1, playerID);
					}
					break;

				case MetroidMessageType.PlaySyncedSound:
					byte playerID2 = reader.ReadByte();
					Player targetPlayer2 = Main.player[playerID2];
					string sound = reader.ReadString();

					SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(this, "Assets/Sounds/" + sound), targetPlayer2.position);

					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.PlaySyncedSound);
						packet.Write(playerID2);
						packet.Write(sound);
						packet.Send(-1, whoAmI);
					}
					break;
			}
		}
		//public override void AddRecipeGroups() =>
		//beamsRecipeGroupID = RecipeGroup.RegisterGroup("LiquidLib:Buckets",
		//new RecipeGroup(() => "Any Buckets", ItemID.WaterBucket, ItemID.LavaBucket, ItemID.HoneyBucket) { IconicItemId = ItemID.WaterBucket });
	}
}