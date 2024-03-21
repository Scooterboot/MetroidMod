using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items;
using MetroidMod.Content.Tiles.Hatch;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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
		public static bool DisplayDebugValues;

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

		public static List<int> hazardShieldDebuffList = new() { 20, 21, 22, 23, 24, 30, 31, 32, 33, 35, 36, 46, 47, 69, 70, 72, 80, 88, 94, 103, 120, 137, 144, 145, 148, 149, 153, 156, 164, 169, 195, 196, 197 };

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

		#region Calls
		public override object Call(params object[] args)
		{
			// Make absolutely, 100% certain that we have arguments.
			if (args is null) { throw new ArgumentNullException(nameof(args), "Arguments cannot be null!"); }
			if (args.Length == 0) { throw new ArgumentException("Arguments cannot be empty!"); }

			// Take first argument and treat it like a command.
			if (args[0] is string content)
			{
				switch (content.ToLower())
				{
					// AddHazardShieldDebuff: Makes the Hazard Shield more effective against debuff id stored in args[1]
					case "addhazardshielddebuff":
						if (args[1] is int id) { hazardShieldDebuffList.Add(id); return true; }
						else { throw new Exception($"Expected an argument of type int when adding to Hazard Shield debuff list, but got type {args[1].GetType().Name} instead."); }
				}
			}

			// Arguments didn't match any commands? Just return false.
			return false;
		}
		#endregion

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
					int capacity = reader.ReadInt32();
					bool canHyper = reader.ReadBoolean();
					int Hypercharge = reader.ReadInt32();
					int pbCh = reader.ReadInt32();

					targetPlayer.statCharge = (float)statCharge;
					targetPlayer.spiderball = spiderBall;
					targetPlayer.boostEffect = boostEffect;
					targetPlayer.boostCharge = boostCharge;
					targetPlayer.EnergyTanks = energyTanks;
					targetPlayer.Energy = energy;
					targetPlayer.SuitReserveTanks = reserveTanks;
					targetPlayer.SuitReserves = reserve;
					targetPlayer.tankCapacity = capacity;
					targetPlayer.canHyper = canHyper;
					targetPlayer.hyperCharge = Hypercharge;
					targetPlayer.statPBCh = pbCh;

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
						packet.Write(capacity);
						packet.Write(canHyper);
						packet.Write(Hypercharge);
						packet.Write(pbCh);
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
