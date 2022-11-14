using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using MetroidMod.Common.Systems;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.Items.Boss;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items.Miscellaneous;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Items.Vanity;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Kraid;
using MetroidMod.Content.NPCs.Nightmare;
using MetroidMod.Content.NPCs.OmegaPirate;
using MetroidMod.Content.NPCs.Phantoon;
using MetroidMod.Content.NPCs.Serris;
using MetroidMod.Content.NPCs.Torizo;
using MetroidMod.Content.NPCs.Town;

namespace MetroidMod
{
	internal class WeakReferencesSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			DoBossChecklistSupport();
			DoCensusModSupport();
			DoRecipeBrowserSupport();
			DoHEROsModSupport();
		}

		private Action<SpriteBatch, Rectangle, Color> BossChecklistRect(string tex) => (SpriteBatch sb, Rectangle rect, Color color) =>
			{
				Texture2D texture = ModContent.Request<Texture2D>(tex).Value;
				Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
				sb.Draw(texture, centered, color);
			};

		private void DoBossChecklistSupport()
		{
			if(!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) || bossChecklist == null)
			{
				Mod.Logger.Info("Boss Checklist is not loaded.");
				return;
			}
			Mod.Logger.Info("Doing Boss Checklist compatibility");
			bossChecklist.Call("AddBoss",
				Mod,
				"Torizo",
				new List<int>() { ModContent.NPCType<Torizo>(), ModContent.NPCType<Torizo_HitBox>() },
				2.1f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo),
				() => true,
				new List<int>() { ModContent.ItemType<TorizoTrophy>(), ModContent.ItemType<TorizoMask>(), ModContent.ItemType<TorizoMusicBox>() },
				ModContent.ItemType<TorizoSummon>(),
				"Guaranteed spawn in the Chozo Ruins found in the Desert. Upon defeat, a Chozo Ghost town NPC will move in, allowing you to purchase its summoning item.",
				null, BossChecklistRect("MetroidMod/Content/NPCs/Torizo/Torizo_BossLog")
			);
			bossChecklist.Call("AddBoss",
				Mod,
				"Serris",
				new List<int>() { ModContent.NPCType<Serris_Head>(), ModContent.NPCType<Serris_Body>(), ModContent.NPCType<Serris_Tail>() },
				5.1f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedSerris),
				() => true,
				new List<int>() { ModContent.ItemType<SerrisTrophy>(), ModContent.ItemType<SerrisMask>(), ModContent.ItemType<SerrisMusicBox>() },
				ModContent.ItemType<SerrisSummon>(),
				"Summoning item can only be used at the Ocean.",
				null, BossChecklistRect("MetroidMod/Content/NPCs/Serris/Serris_BossLog")
			);
			bossChecklist.Call("AddBoss",
				Mod,
				"Kraid",
				new List<int>() { ModContent.NPCType<Kraid_Head>(), ModContent.NPCType<Kraid_Body>(), ModContent.NPCType<Kraid_ArmFront>(), ModContent.NPCType<Kraid_ArmBack>() },
				8.1f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid),
				() => true,
				new List<int>() { ModContent.ItemType<KraidTrophy>(), ModContent.ItemType<KraidMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
				ModContent.ItemType<KraidSummon>(),
				null,
				null, BossChecklistRect("MetroidMod/Content/NPCs/Kraid/Kraid_BossLog")
			);
			bossChecklist.Call("AddBoss",
				Mod,
				"Phantoon",
				ModContent.NPCType<Phantoon>(),
				8.9f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedPhantoon),
				() => true,
				new List<int>() { ModContent.ItemType<PhantoonTrophy>(), ModContent.ItemType<PhantoonMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
				ModContent.ItemType<PhantoonSummon>(),
				"Summoning item can only be used at night.",
				null, BossChecklistRect("MetroidMod/Content/NPCs/Phantoon/Phantoon")
			);
			bossChecklist.Call("AddBoss",
				Mod,
				"Nightmare",
				new List<int>() { ModContent.NPCType<Nightmare>(), ModContent.NPCType<Nightmare_Body>(), ModContent.NPCType<Nightmare_ArmFront>(), ModContent.NPCType<Nightmare_ArmBack>() },
				13f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedNightmare),
				() => true,
				new List<int>() { /*ModContent.ItemType<NightmareTrophy>(), ModContent.ItemType<NightmareMask>(),*/ ModContent.ItemType<NightmareMusicBox>() },
				ModContent.ItemType<NightmareSummon>(),
				"Summoning item can only be used at night.",
				null, BossChecklistRect("MetroidMod/Content/NPCs/Nightmare/Nightmare_BossLog")
			);
			bossChecklist.Call("AddBoss",
				Mod,
				"Omega Pirate",
				new List<int>() { ModContent.NPCType<OmegaPirate>(), ModContent.NPCType<OmegaPirate_HitBox>() },
				13f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate),
				() => true,
				new List<int>() { /*ModContent.ItemType<OmegaPirateTrophy>(), ModContent.ItemType<OmegaPirateMask>(),*/ ModContent.ItemType<OmegaPirateMusicBox>() },
				ModContent.ItemType<OmegaPirateSummon>(),
				null,
				null, BossChecklistRect("MetroidMod/Content/NPCs/OmegaPirate/OmegaPirate_BossLog")
			);
			bossChecklist.Call("AddBoss",
				Mod,
				"Golden Torizo",
				new List<int>() { ModContent.NPCType<GoldenTorizo>(), ModContent.NPCType<GoldenTorizo_HitBox>() },
				15f,
				() => MSystem.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo),
				() => true,
				new List<int>() { /*ModContent.ItemType<GoldenTorizoTrophy>(), ModContent.ItemType<GoldenTorizoMask>(),*/ ModContent.ItemType<TorizoMusicBox>() },
				ModContent.ItemType<GoldenTorizoSummon>(),
				"Guaranteed spawn in the Chozo Ruins after the Golem has been defeated. Upon defeat, the Chozo Ghost will sell you its summoning item.",
				null, BossChecklistRect("MetroidMod/Content/NPCs/GoldenTorizo/GoldenTorizo_BossLog")
			);
		}

		private void DoCensusModSupport()
		{
			if (!ModLoader.TryGetMod("Census", out Mod census) || census == null)
			{
				Mod.Logger.Info("Census is not loaded.");
				return;
			}
			Mod.Logger.Info("Doing Census compatibility");
			census.Call("TownNPCCondition", ModContent.NPCType<ChozoGhost>(), $"Defeat Torizo, an ancient guardian found in the [c/ffff00:Chozo Ruins]");
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
	}
}
