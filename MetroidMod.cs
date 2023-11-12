using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ReLogic;
using ReLogic.Graphics;

using MetroidMod.Content.Items;
using MetroidMod.Common.UI;
using MetroidMod.Common.Players;
using MetroidMod.Content.Tiles.Hatch;
using MetroidMod.ID;

namespace MetroidMod
{
	public enum MetroidMessageType : byte
	{
		SyncStartPlayerStats,
		SyncPlayerStats,
		PlaySyncedSound,
		BestiaryUpdate,
		DoorClickSync
	}

	[LegacyName("MetroidModPorted")]
	public class MetroidMod : Mod
	{
		internal const int ballSlotAmount = 5;
		internal const int beamSlotAmount = 5;
		internal const int beamChangeSlotAmount = 12;
		internal const int missileChangeSlotAmount = 13;
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
		public static MetroidMod Instance { get; private set; }
		public MetroidMod() { Instance = this; }

		public static bool DebugDH;
		public static bool DebugDSI;

		public int selectedItem = 0;
		public int oldSelectedItem = 0;

		public int[] FrozenStandOnNPCs;

		//public static int beamsRecipeGroupID;
		public static int PreHMhooksRecipeID;
		public static int MorphBallBombsRecipeGroupID;
		public static int T1PHMBarRecipeGroupID;
		public static int GoldPlatinumBarRecipeGroupID;
		public static int EvilBarRecipeGroupID;
		public static int EvilMaterialRecipeGroupID;
		public static int EvilHMMaterialRecipeGroupID;
		public static int T1HMBarRecipeGroupID;
		public static int T2HMBarRecipeGroupID;
		public static int T3HMBarRecipeGroupID;
		public static ushort unloadedItemID;

		public override void Load()
		{
			ItemIL.Load();
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
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/ChozoRuinsActive"), ModContent.ItemType<Content.Items.Tiles.ChozoRuinsActiveMusicBox>(), ModContent.TileType<Content.Tiles.ChozoRuinsActiveMusicBox>());
				MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Assets/Music/ChozoRuinsInactive"), ModContent.ItemType<Content.Items.Tiles.ChozoRuinsInactiveMusicBox>(), ModContent.TileType<Content.Tiles.ChozoRuinsInactiveMusicBox>());
			}

			/*if (Main.netMode != NetmodeID.Server)
			{
				Ref<Effect> filterRef = new Ref<Effect>(GetEffect("Effects/MyFilters"));

				Filters.Scene["ThermalVisor"] = new Filter(new ScreenShaderData(filterRef, "PassName"), EffectPriority.High);
			}*/
		}

		public override void Unload()
		{
			Instance = null;
			ItemIL.Unload();
			//OnHooks.Unload();
			//ILHooks.Unload();
			//BeamLoader.Unload();
			SuitAddonLoader.Unload();
			MBAddonLoader.Unload();
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
					int energyTanks = reader.ReadInt32();
					int energy = reader.ReadInt32();
					int reserveTanks = reader.ReadInt32();
					int reserve = reader.ReadInt32();

					targetPlayer.statCharge = (float)statCharge;
					targetPlayer.spiderball = spiderBall;
					targetPlayer.boostEffect = boostEffect;
					targetPlayer.boostCharge = boostCharge;
					targetPlayer.EnergyTanks = energyTanks;
					targetPlayer.Energy = energy;
					targetPlayer.SuitReserveTanks = reserveTanks;
					targetPlayer.SuitReserves = reserve;

					if (msgType == MetroidMessageType.SyncPlayerStats && Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.SyncPlayerStats);
						packet.Write(playerID);
						packet.Write(statCharge);
						packet.Write(spiderBall);
						packet.Write(boostEffect);
						packet.Write(boostCharge);
						packet.Write(energyTanks);
						packet.Write(energy);
						packet.Write(reserveTanks);
						packet.Write(reserve);
						packet.Send(-1, playerID);
					}
					break;

				case MetroidMessageType.PlaySyncedSound:
					byte playerID2 = reader.ReadByte();
					Player targetPlayer2 = Main.player[playerID2];
					string sound = reader.ReadString();

					SoundEngine.PlaySound(new SoundStyle($"{Name}/Assets/Sounds/" + sound), targetPlayer2.position);

					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.PlaySyncedSound);
						packet.Write(playerID2);
						packet.Write(sound);
						packet.Send(-1, whoAmI);
					}
					break;

				case MetroidMessageType.BestiaryUpdate:
					int npcType = reader.ReadInt32();
					byte hostilityType = reader.ReadByte();
					NPC npc = new NPC();
					npc.SetDefaults(npcType);
					if (hostilityType == 1) { Main.BestiaryTracker.Chats.RegisterChatStartWith(npc); }
					else if (hostilityType == 2) { Main.BestiaryTracker.Sights.RegisterWasNearby(npc); }
					else if (hostilityType == 3) { Main.BestiaryTracker.Kills.RegisterKill(npc); }

					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.BestiaryUpdate);
						packet.Write(npcType);
						packet.Write(hostilityType);
						packet.Send(-1, whoAmI);
					}
					break;
				case MetroidMessageType.DoorClickSync:
					ushort type = reader.ReadUInt16();
					int i = reader.ReadInt32();
					int j = reader.ReadInt32();

					BlueHatch hatch = ModContent.GetModTile(type) as BlueHatch;
					hatch.HitWire(i, j);

					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.DoorClickSync);
						packet.Write(type);
						packet.Write(i);
						packet.Write(j);
						packet.Send(-1, whoAmI);
					}
					break;
			}
		}
	}
}
