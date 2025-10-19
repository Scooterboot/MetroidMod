using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Common.Players;
using MetroidMod.Content.Hatches;
using MetroidMod.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
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
		ChangeHatchOpenState
	}

	[LegacyName("MetroidModPorted")]
	public class MetroidMod : Mod
	{
		public static MetroidMod Instance { get; private set; }
		public MetroidMod() { Instance = this; }


		public static bool DebugDH;
		public static bool DebugDSI;
		public static bool DisplayDebugValues;


		#region Addon array sizes
		internal const int ballSlotAmount = 5;
		internal const int beamSlotAmount = 6;
		internal const int beamChangeSlotAmount = 8;
		internal const int missileChangeSlotAmount = 8;
		internal const int missileSlotAmount = 3;
		#endregion

		public static bool UseAltWeaponTextures; //delete later			-Z

		#region Fallback Assets
		//With systems as complicated as the addon ones, things are gonna go wrong.
		//If the system fails to find any particular asset for an addon, it'll default to one of these suckers.

		#region Power Beam fallbacks
		/// <summary>
		/// The backup texture for a Beam shot. Identical to default Power Beam.
		/// </summary>
		public static Asset<Texture2D> PowerBeamFallbackTexture => ModContent.Request<Texture2D>($"MetroidMod/Assets/Textures/BeamAddons/PowerBeam/Shot");
		/// <summary>
		/// The backup sound effect for firing the Power Beam. Uses the Super Metroid hurt SFX to make it clear something is wrong.
		/// </summary>
		public static SoundStyle BeamShotFallbackSFX => new SoundStyle($"MetroidMod/Assets/Sounds/ArmCannon/ShotMissing");
		/// <summary>
		/// The backup sound effect for charging a shot. Identical to default Charge Beam charging sound.
		/// </summary>
		public static SoundStyle BeamChargeFallbackSFX => new SoundStyle($"MetroidMod/Assets/Sounds/ArmCannon/BeamChargingSound");

		/// <summary>
		/// The backup sound effect for Beam shot impact. Since I'm pretty sure most addons won't bother changing this, it's a lot more subtle than all the others.
		/// </summary>
		public static SoundStyle BeamImpactFallbackSFX => new SoundStyle($"MetroidMod/Assets/Sounds/ArmCannon/BeamImpactSound");

		#endregion

		#region Missile Launcher
		/// <summary>
		/// The backup sound effect for Missile shot impact. Since I'm pretty sure most addons won't bother changing this, it's a lot more subtle than all the others.
		/// </summary>
		/// 		/// <summary>
		/// The backup texture for a Beam shot. Identical to default Power Beam.
		/// </summary>
		public static Asset<Texture2D> MissileFallbackTexture => ModContent.Request<Texture2D>($"MetroidMod/Assets/Textures/MissileAddons/Expansion/Shot");
		/// <summary>
		/// The backup sound effect for a basic missile shot. Identical to default missile shot sound.
		/// </summary>
		public static SoundStyle MissileShotFallbackSFX => new SoundStyle($"MetroidMod/Assets/Sounds/ArmCannon/MissileShot");
		public static SoundStyle MissileImpactFallbackSFX => new SoundStyle($"MetroidMod/Assets/Sounds/ArmCannon/MissileImpact");
		/// <summary>
		/// The backup sound effect for charging a shot. Identical to default Missile charging sound.
		/// </summary>
		public static SoundStyle MissileChargeFallbackSFX => new SoundStyle($"MetroidMod/Assets/Sounds/ArmCannon/MissileChargingSound");
		#endregion

		#endregion

		#region Beam color values
		public static Color powColor = new(248, 248, 104);
		public static Color powSecondaryColor = new(248, 168, 0);
		public static Color iceColor = new(0, 176, 248);
		public static Color iceSecondaryColor = new(0, 56, 168);
		public static Color waveColor = new(255, 115, 255);
		public static Color waveSecondaryColor = new(176, 0, 176);
		public static Color waveColor2 = new(239, 153, 239);
		public static Color waveSecondaryColor2 = new(158, 79, 158);
		public static Color plaRedColor = new(253, 221, 3);
		public static Color plaRedSecondaryColor = new(184, 58, 24);
		public static Color plaGreenColor = new(0, 248, 112);
		public static Color plaGreenSecondaryColor = new(0, 160, 72);
		public static Color plaGreenColor2 = new(61, 248, 154);
		public static Color plaGreenSecondaryColor2 = new(40, 136, 80);
		public static Color novColor = new(50, 255, 1);
		public static Color novSecondaryColor = new(24, 184, 67);
		public static Color wideColor = new(255, 210, 255);
		public static Color lumColor = new(209, 255, 250);
		public static Color lumSecondaryColor = new(45, 105, 76);
		public static Color lumColor2 = new(229, 218, 186);
		public static Color lumSecondaryColor2 = new(164, 101, 124);
		#endregion

		public int[] FrozenStandOnNPCs;

		#region Recipe Groups
		//public static int beamsRecipeGroupID;
		/// <summary>
		/// A recipe group that includes all vanilla pre-Hardmode grappling hooks.
		/// </summary>
		public static int PreHMhooksRecipeID;
		/// <summary>
		/// A recipe group that includes all Morph Ball Bombs.
		/// </summary>
		public static int MorphBallBombsRecipeGroupID;
		/// <summary>
		/// A recipe group that includes Copper and Tin Bars.
		/// </summary>
		public static int T1PHMBarRecipeGroupID;
		/// <summary>
		/// Self-explanatory.
		/// </summary>
		public static int GoldPlatinumBarRecipeGroupID;
		/// <summary>
		/// A recipe group that includes Demonite and Crimtane Bars.
		/// </summary>
		public static int EvilBarRecipeGroupID;
		/// <summary>
		/// A recipe group that contains Shadow Scales and Tissue Samples.
		/// </summary>
		public static int EvilMaterialRecipeGroupID;
		/// <summary>
		/// A recipe group that includes Cursed Flames and Ichor.
		/// </summary>
		public static int EvilHMMaterialRecipeGroupID;
		/// <summary>
		/// A recipe group that includes Cobalt and Palladium.
		/// </summary>
		public static int T1HMBarRecipeGroupID;
		/// <summary>
		/// A recipe group that contains Mythril and Orichalcum.
		/// </summary>
		public static int T2HMBarRecipeGroupID;
		/// <summary>
		/// A recipe group that includes Adamantite and Titanium.
		/// </summary>
		public static int T3HMBarRecipeGroupID;
		#endregion

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

			if (Main.netMode != NetmodeID.Server)
			{
				Asset<Effect> projShaderRef = this.Assets.Request<Effect>("Assets/Effects/ProjectileShaders");

				GameShaders.Misc["MetroidModLaserBeam"] = new MiscShaderData(projShaderRef, "LaserShaderPass");
				GameShaders.Misc["MetroidModDualTint"] = new MiscShaderData(projShaderRef, "DualTintShaderPass");
				GameShaders.Misc["MetroidModPaletteShader"] = new MiscShaderData(projShaderRef, "PaletteShaderPass");
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
			BeamAddonLoader.Unload();
			MissileAddonLoader.Unload();
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

					targetPlayer.ReadPacketData(reader);

					if (msgType == MetroidMessageType.SyncPlayerStats && Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = GetPacket();
						packet.Write((byte)MetroidMessageType.SyncPlayerStats);
						packet.Write(playerID);
						targetPlayer.WritePacketData(packet);
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
				case MetroidMessageType.ChangeHatchOpenState:
					short i = reader.ReadInt16();
					short j = reader.ReadInt16();

					HatchState state = new();
					state.DesiredState = (HatchDesiredState)reader.ReadByte();
					state.LockStatus = (HatchLockStatus)reader.ReadByte();
					state.BlueConversion = (HatchBlueConversionStatus)reader.ReadByte();
					DebugAssist.NewTextMP($"Hatch state received: {state}");

					if (TileUtils.TryGetTileEntityAs(i, j, out HatchTileEntity hatch))
					{
						hatch.State.DesiredState = state.DesiredState;
						hatch.State.LockStatus = state.LockStatus;
						hatch.State.BlueConversion = state.BlueConversion;

						if (Main.netMode == NetmodeID.Server)
						{
							ModPacket packet = hatch.GetSyncPacket();
							packet.Send(-1, whoAmI);
						}
					}

					break;
			}
		}
	}
}
