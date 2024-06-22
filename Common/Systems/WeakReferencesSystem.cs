using System;
using System.Collections.Generic;
using MetroidMod.Common.Systems;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items.Boss;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Items.Vanity;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Kraid;
using MetroidMod.Content.NPCs.Nightmare;
using MetroidMod.Content.NPCs.OmegaPirate;
using MetroidMod.Content.NPCs.Phantoon;
using MetroidMod.Content.NPCs.Serris;
using MetroidMod.Content.NPCs.Torizo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod
{
	internal class WeakReferencesSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			DoBossChecklistSupport();
			DoRecipeBrowserSupport();
			DoHEROsModSupport();
			DoMusicDisplaySupport();
		}

		private Action<SpriteBatch, Rectangle, Color> BossChecklistRect(string tex, float mult = 1f) => (SpriteBatch sb, Rectangle rect, Color color) =>
			{
				Texture2D texture = ModContent.Request<Texture2D>(tex).Value;
				Rectangle centered = new(rect.X + (rect.Width / 2) - (int)Math.Floor(texture.Width * mult / 2f), rect.Y + (rect.Height / 2) - (int)Math.Floor(texture.Height * mult / 2f), (int)Math.Floor(texture.Width * mult), (int)Math.Floor(texture.Height * mult));
				sb.Draw(texture, centered, color);
			};

		private void DoBossChecklistSupport()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) || bossChecklist == null)
			{
				Mod.Logger.Info("Boss Checklist is not loaded.");
				return;
			}
			Mod.Logger.Info("Doing Boss Checklist compatibility");
			bossChecklist.Call("LogBoss",
				Mod,
				"Torizo",
				2.1f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo),
				new List<int>() { ModContent.NPCType<Torizo>(), ModContent.NPCType<Torizo_HitBox>() },
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<TorizoSummon>(),
					["collectibles"] = new List<int>() { ModContent.ItemType<TorizoTrophy>(), ModContent.ItemType<TorizoMask>(), ModContent.ItemType<TorizoMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/Torizo/Torizo_BossLog")
				}
			);
			bossChecklist.Call("LogBoss",
				Mod,
				"Serris",
				5.1f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedSerris),
				new List<int>() { ModContent.NPCType<Serris_Head>(), ModContent.NPCType<Serris_Body>(), ModContent.NPCType<Serris_Tail>() },
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<SerrisSummon>(),
					["collectibles"] = new List<int>() { ModContent.ItemType<SerrisTrophy>(), ModContent.ItemType<SerrisMask>(), ModContent.ItemType<SerrisMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/Serris/Serris_BossLog")
				}
			);
			bossChecklist.Call("LogBoss",
				Mod,
				"Kraid",
				8.1f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid),
				new List<int>() { ModContent.NPCType<Kraid_Head>(), ModContent.NPCType<Kraid_Body>(), ModContent.NPCType<Kraid_ArmFront>(), ModContent.NPCType<Kraid_ArmBack>() },
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<KraidSummon>(),
					["collectibles"] = new List<int>() { ModContent.ItemType<KraidTrophy>(), ModContent.ItemType<KraidMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/Kraid/Kraid_BossLog", 0.5f)
				}
			);
			bossChecklist.Call("LogBoss",
				Mod,
				"Phantoon",
				8.9f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedPhantoon),
				ModContent.NPCType<Phantoon>(),
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<PhantoonSummon>(),
					["collectibles"] = new List<int>() { ModContent.ItemType<PhantoonTrophy>(), ModContent.ItemType<PhantoonMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/Phantoon/Phantoon")
				}
			);
			bossChecklist.Call("LogBoss",
				Mod,
				"Nightmare",
				13f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedNightmare),
				new List<int>() { ModContent.NPCType<Nightmare>(), ModContent.NPCType<Nightmare_Body>(), ModContent.NPCType<Nightmare_ArmFront>(), ModContent.NPCType<Nightmare_ArmBack>() },
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<NightmareSummon>(),
					["collectibles"] = new List<int>() { /*ModContent.ItemType<NightmareTrophy>(), ModContent.ItemType<NightmareMask>(),*/ ModContent.ItemType<NightmareMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/Nightmare/Nightmare_BossLog")
				}
			);
			bossChecklist.Call("LogBoss",
				Mod,
				"OmegaPirate",
				13f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate),
				new List<int>() { ModContent.NPCType<OmegaPirate>(), ModContent.NPCType<OmegaPirate_HitBox>() },
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<OmegaPirateSummon>(),
					["collectibles"] = new List<int>() { /*ModContent.ItemType<OmegaPirateTrophy>(), ModContent.ItemType<OmegaPirateMask>(),*/ ModContent.ItemType<OmegaPirateMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/OmegaPirate/OmegaPirate_BossLog")
				}
			);
			bossChecklist.Call("LogBoss",
				Mod,
				"GoldenTorizo",
				15f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo),
				new List<int>() { ModContent.NPCType<GoldenTorizo>(), ModContent.NPCType<GoldenTorizo_HitBox>() },
				new Dictionary<string, object>()
				{
					["spawnItems"] = ModContent.ItemType<GoldenTorizoSummon>(),
					["collectibles"] = new List<int>() { /*ModContent.ItemType<GoldenTorizoTrophy>(), ModContent.ItemType<GoldenTorizoMask>(),*/ ModContent.ItemType<TorizoMusicBox>() },
					["customPortrait"] = BossChecklistRect("MetroidMod/Content/NPCs/GoldenTorizo/GoldenTorizo_BossLog")
				}
			);
		}

		private void DoRecipeBrowserSupport()
		{
			if (!ModLoader.TryGetMod("RecipeBrowser", out Mod recipeBrowser) || recipeBrowser == null)
			{
				Mod.Logger.Info("Recipe Browser is not loaded.");
				return;
			}
			Mod.Logger.Info("Doing Recipe Browser compatibility");
			if (!Main.dedServ)
			{
				recipeBrowser.Call("AddItemCategory",
					"Hunter",
					"Weapons",
					ModContent.Request<Texture2D>(
						$"{Mod.Name}/Assets/Textures/PowerBeamRecipeBrowser",
						AssetRequestMode.ImmediateLoad).Value,
					(Predicate<Item>)((Item item) => item.damage > 0 && item.DamageType.CountsAsClass<HunterDamageClass>()
					)
				);
			}
		}

		private void DoHEROsModSupport()
		{
			if (!ModLoader.TryGetMod("HEROsMod", out Mod herosMod) || herosMod == null)
			{
				Mod.Logger.Info("HERO's Mod is not loaded.");
				return;
			}
			Mod.Logger.Info("Doing HERO's Mod compatibility");
			herosMod.Call("AddItemCategory",
				"Hunter",
				"Weapons",
				(Predicate<Item>)((Item item) => item.damage > 0 && item.DamageType.CountsAsClass<HunterDamageClass>())
			);
			// TODO: Make MConfigClientMain only accept client changes if player is admin
			//Common.Configs.MConfigClientMain.Instance.condition = delegate (ModConfig pendingConfig, int whoAmI, ref string message) { return (bool)herosMod.Call("HasPermission", whoAmI, ""); };
			// TODO: Add extension for infinite energy and infinite overheat
			/*
			herosMod.Call("AddPermission",
				"CanHaveInfiniteEnergy",
				"Can enable infinite suit energy",
				(bool) => { return true; }
			);
			herosMod.Call("AddSimpleButton",
				"permName",

			);
			*/
		}
		private void DoMusicDisplaySupport()
		{
			if (!ModLoader.TryGetMod("MusicDisplay", out Mod musdisp) || musdisp == null)
			{
				Mod.Logger.Info("Music Display is not loaded.");
				return;
			}
			Mod.Logger.Info("Doing Music Display compatibility");

			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/ChozoRuinsActive"),
				Language.GetText("Mods.MetroidMod.BGMInfo.ChozoRuinsActive.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.ChozoRuinsActive.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/ChozoRuinsInactive"),
				Language.GetText("Mods.MetroidMod.BGMInfo.ChozoRuinsInactive.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.ChozoRuinsInactive.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/Kraid"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Kraid.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Kraid.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/Nightmare"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Nightmare.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Nightmare.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/OmegaPirate"),
				Language.GetText("Mods.MetroidMod.BGMInfo.OmegaPirate.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.OmegaPirate.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/Ridley"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Ridley.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Ridley.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/Serris"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Serris.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Serris.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/Title"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Title.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Title.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
			musdisp.Call("AddMusic",
				(short)MusicLoader.GetMusicSlot("MetroidMod/Assets/Music/Torizo"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Torizo.Name"),
				Language.GetText("Mods.MetroidMod.BGMInfo.Torizo.Author"),
				Language.GetText("Mods.MetroidMod.ModName")
				);
		}
	}
}
