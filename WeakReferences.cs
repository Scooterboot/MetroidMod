using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Common.Worlds;
using MetroidMod.Items.accessories;
using MetroidMod.Items.boss;
using MetroidMod.Items.damageclass;
using MetroidMod.Items.misc;
using MetroidMod.Items.tiles;
using MetroidMod.Items.vanity;
using MetroidMod.NPCs.GoldenTorizo;
using MetroidMod.NPCs.Kraid;
using MetroidMod.NPCs.Nightmare;
using MetroidMod.NPCs.OmegaPirate;
using MetroidMod.NPCs.Phantoon;
using MetroidMod.NPCs.Serris;
using MetroidMod.NPCs.Torizo;
using MetroidMod.NPCs.Town;

namespace MetroidMod
{
	internal class WeakReferences
	{
		private static Mod mod { get { return MetroidMod.Instance; } }
		public static void PerformModSupport()
		{
			DoFargoSupport();
			DoBossChecklistSupport();
			DoCensusModSupport();
			DoRecipeBrowserSupport();
			DoHEROsModSupport();
			DoBossAssistSupport();
		}

		private static void DoFargoSupport()
		{
			Mod mod = ModLoader.GetMod("Fargowiltas");
			if (mod != null)
			{
				mod.Call("AddSummon", 2.1f, WeakReferences.mod.Name, ModContent.ItemType<TorizoSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedTorizo)),
				1000);
				mod.Call("AddSummon", 5.1f, WeakReferences.mod.Name, ModContent.ItemType<SerrisSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedSerris)),
				1000);
				mod.Call("AddSummon", 6.1f, WeakReferences.mod.Name, ModContent.ItemType<KraidSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedKraid)),
				1000);
				mod.Call("AddSummon", 6.9f, WeakReferences.mod.Name, ModContent.ItemType<PhantoonSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedPhantoon)),
				1000);
				mod.Call("AddSummon", 11f, WeakReferences.mod.Name, ModContent.ItemType<NightmareSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedNightmare)),
				1000);
				mod.Call("AddSummon", 11f, WeakReferences.mod.Name, ModContent.ItemType<OmegaPirateSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate)),
				1000);
				mod.Call("AddSummon", 12f, WeakReferences.mod.Name, ModContent.ItemType<GoldenTorizoSummon>(),
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo)),
				1000);
			}
		}
		private static void DoBossChecklistSupport()
		{
			Mod mod = ModLoader.GetMod("BossChecklist");
			if (mod != null)
			{
				mod.Call("AddBoss", 2.1f,
				new List<int>() { ModContent.NPCType<Torizo>(), ModContent.NPCType<Torizo_HitBox>() }, MetroidMod.Instance, "Torizo",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedTorizo)), ModContent.ItemType<TorizoSummon>(),
				new List<int>() { ModContent.ItemType<TorizoTrophy>(), ModContent.ItemType<TorizoMask>(), ModContent.ItemType<TorizoMusicBox>() },
				new List<int>() { ModContent.ItemType<EnergyShard>(), ModContent.ItemType<TorizoBag>() },
				"Guaranteed spawn in the Chozo Ruins found in the Desert. Upon defeat, a Chozo Ghost town NPC will move in, allowing you to purchase its summoning item.",
				null, "MetroidMod/NPCs/Torizo/Torizo_BossLog");

				mod.Call("AddBoss", 5.1f,
				new List<int>() { ModContent.NPCType<Serris_Head>(), ModContent.NPCType<Serris_Body>(), ModContent.NPCType<Serris_Tail>() }, MetroidMod.Instance, "Serris",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedSerris)), ModContent.ItemType<SerrisSummon>(),
				new List<int>() { ModContent.ItemType<SerrisTrophy>(), ModContent.ItemType<SerrisMask>(), ModContent.ItemType<SerrisMusicBox>() },
				new List<int>() { ModContent.ItemType<SerrisCoreX>(), ModContent.ItemType<SerrisBag>() },
				"Summoning item can only be used at the Ocean.", null, "MetroidMod/NPCs/Serris/Serris_BossLog");

				mod.Call("AddToBossLoot", "Terraria", "WallofFlesh", new List<int>
				{
					ModContent.ItemType<HunterEmblem>()
				});

				mod.Call("AddBoss", 6.1f,
				new List<int>() { ModContent.NPCType<Kraid_Head>(), ModContent.NPCType<Kraid_Body>(), ModContent.NPCType<Kraid_ArmBack>(), ModContent.NPCType<Kraid_ArmFront>() }, MetroidMod.Instance, "Kraid",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedKraid)), ModContent.ItemType<KraidSummon>(),
				new List<int>() { ModContent.ItemType<KraidTrophy>(), ModContent.ItemType<KraidMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
				new List<int>() { ModContent.ItemType<KraidTissue>(), ModContent.ItemType<UnknownPlasmaBeam>(), ModContent.ItemType<KraidBag>() },
				null, null, "MetroidMod/NPCs/Kraid/Kraid_BossLog");

				mod.Call("AddBoss", 6.9f,
				ModContent.NPCType<Phantoon>(), MetroidMod.Instance, "Phantoon",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedPhantoon)), ModContent.ItemType<PhantoonSummon>(),
				new List<int>() { ModContent.ItemType<PhantoonTrophy>(), ModContent.ItemType<PhantoonMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
				new List<int>() { ModContent.ItemType<GravityGel>(), ModContent.ItemType<PhantoonBag>() },
				"Summoning item can only be used at night.", null, "MetroidMod/NPCs/Phantoon/Phantoon");

				mod.Call("AddBoss", 11f,
				new List<int>() { ModContent.NPCType<Nightmare>(), ModContent.NPCType<Nightmare_Body>(), ModContent.NPCType<Nightmare_ArmBack>(), ModContent.NPCType<Nightmare_ArmFront>() }, MetroidMod.Instance, "Nightmare",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedNightmare)), ModContent.ItemType<NightmareSummon>(),
				null,//new List<int>(){ModContent.ItemType<NightmareTrophy>(),ModContent.ItemType<NightmareMask>(), ModContent.ItemType<NightmareMusicBox>()},
				new List<int>() { ModContent.ItemType<NightmareCoreX>(), ModContent.ItemType<NightmareCoreXFragment>(), ModContent.ItemType<NightmareBag>() },
				"Summoning item can only be used at night.", null, "MetroidMod/NPCs/Nightmare/Nightmare_BossLog");

				mod.Call("AddBoss", 11f,
				new List<int>() { ModContent.NPCType<OmegaPirate>(), ModContent.NPCType<OmegaPirate_HitBox>() }, MetroidMod.Instance, "Omega Pirate",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate)), ModContent.ItemType<OmegaPirateSummon>(),
				null,//new List<int>(){ModContent.ItemType<OmegaPirateTrophy>(),ModContent.ItemType<OmegaPirateMask>(), ModContent.ItemType<OmegaPirateMusicBox>()},
				new List<int>() { ModContent.ItemType<PurePhazon>(), ModContent.ItemType<OmegaPirateBag>() },
				null, null, "MetroidMod/NPCs/OmegaPirate/OmegaPirate_BossLog");

				mod.Call("AddBoss", 12f,
				new List<int>() { ModContent.NPCType<GoldenTorizo>(), ModContent.NPCType<GoldenTorizo_HitBox>() }, MetroidMod.Instance, "Golden Torizo",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo)), ModContent.ItemType<GoldenTorizoSummon>(),
				new List<int>() {/*ModContent.ItemType<GoldenTorizoTrophy>(),ModContent.ItemType<GoldenTorizoMask>(),*/ ModContent.ItemType<TorizoMusicBox>() },
				new List<int>() { ModContent.ItemType<ScrewAttack>(), ModContent.ItemType<GoldenTorizoBag>() },
				"Guaranteed spawn in the Chozo Ruins after the Golem has been defeated. Upon defeat, the Chozo Ghost will sell you its summoning item.",
				null, "MetroidMod/NPCs/GoldenTorizo/GoldenTorizo_BossLog");
			}
		}
		private static void DoCensusModSupport()
		{
			Mod mod = ModLoader.GetMod("Census");
			if (mod != null)
			{
				mod.Call("TownNPCCondition", ModContent.NPCType<ChozoGhost>(), "Defeat [c/4dffb8:Torizo]");
			}
		}
		private static void DoRecipeBrowserSupport()
		{
			Mod mod = ModLoader.GetMod("RecipeBrowser");
			if (mod != null && !Main.dedServ)
			{
				mod.Call("AddItemCategory", "Hunter", "Weapons", ModContent.GetTexture("Items/accessories/HunterEmblem"), (Predicate<Item>)delegate (Item item)
				{
					if (item.damage > 0)
					{
						return item.modItem is HunterDamageItem;
					}
					return false;
				});
			}
		}
		private static void DoHEROsModSupport()
		{
			Mod mod = ModLoader.GetMod("HEROsMod");
			if (mod != null && mod.Version >= new Version(0, 3, 6, 2) && !Main.dedServ)
			{
				mod.Call("AddItemCategory", "Hunter", "Weapons", (Predicate<Item>)delegate (Item item)
				{
					if (item.damage > 0)
					{
						return item.modItem is HunterDamageItem;
					}
					return false;
				});
			}
		}
		private static void DoBossAssistSupport()
		{
			Mod mod = ModLoader.GetMod("BossAssist");
			if (mod != null)
			{
				mod.Call("AddStatPage", 2.1f, ModContent.NPCType<Torizo>(), WeakReferences.mod.Name, "Torizo",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedTorizo)), ModContent.ItemType<TorizoSummon>(),
				new List<int>() { ModContent.ItemType<TorizoTrophy>(), ModContent.ItemType<TorizoMask>(), ModContent.ItemType<TorizoMusicBox>() },
				new List<int>() { ModContent.ItemType<EnergyShard>(), ModContent.ItemType<TorizoBag>() },
				"MetroidMod/NPCs/Torizo/Torizo_BossLog");

				mod.Call("AddStatPage", 5.1f, ModContent.NPCType<Serris_Head>(), WeakReferences.mod.Name, "Serris",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedSerris)), ModContent.ItemType<SerrisSummon>(),
				new List<int>() { ModContent.ItemType<SerrisTrophy>(), ModContent.ItemType<SerrisMask>(), ModContent.ItemType<SerrisMusicBox>() },
				new List<int>() { ModContent.ItemType<SerrisCoreX>(), ModContent.ItemType<SerrisBag>() },
				"MetroidMod/NPCs/Serris/Serris_BossLog");

				mod.Call("AddStatPage", 6.1f, ModContent.NPCType<Kraid_Head>(), WeakReferences.mod.Name, "Kraid",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedKraid)), ModContent.ItemType<KraidSummon>(),
				new List<int>() { ModContent.ItemType<KraidTrophy>(), ModContent.ItemType<KraidMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
				new List<int>() { ModContent.ItemType<KraidTissue>(), ModContent.ItemType<UnknownPlasmaBeam>(), ModContent.ItemType<KraidBag>() },
				"MetroidMod/NPCs/Kraid/Kraid_BossLog");

				mod.Call("AddStatPage", 6.9f, ModContent.NPCType<Phantoon>(), WeakReferences.mod.Name, "Phantoon",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedPhantoon)), ModContent.ItemType<PhantoonSummon>(),
				new List<int>() { ModContent.ItemType<PhantoonTrophy>(), ModContent.ItemType<PhantoonMask>(), ModContent.ItemType<KraidPhantoonMusicBox>() },
				new List<int>() { ModContent.ItemType<GravityGel>(), ModContent.ItemType<PhantoonBag>() },
				"MetroidMod/NPCs/Phantoon/Phantoon");

				mod.Call("AddStatPage", 11f, ModContent.NPCType<Nightmare>(), WeakReferences.mod.Name, "Nightmare",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedNightmare)), ModContent.ItemType<NightmareSummon>(),
				null,//new List<int>(){ModContent.ItemType<NightmareTrophy>(),ModContent.ItemType<NightmareMask>(), ModContent.ItemType<NightmareMusicBox>()},
				new List<int>() { ModContent.ItemType<NightmareCoreX>(), ModContent.ItemType<NightmareCoreXFragment>(), ModContent.ItemType<NightmareBag>() },
				"MetroidMod/NPCs/Nightmare/Nightmare_BossLog");

				mod.Call("AddStatPage", 11f, ModContent.NPCType<OmegaPirate>(), WeakReferences.mod.Name, "Omega Pirate",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate)), ModContent.ItemType<OmegaPirateSummon>(),
				null,//new List<int>(){ModContent.ItemType<OmegaPirateTrophy>(),ModContent.ItemType<OmegaPirateMask>(), ModContent.ItemType<OmegaPirateMusicBox>()},
				new List<int>() { ModContent.ItemType<PurePhazon>(), ModContent.ItemType<OmegaPirateBag>() },
				"MetroidMod/NPCs/OmegaPirate/OmegaPirate_BossLog");

				mod.Call("AddStatPage", 12f, ModContent.NPCType<GoldenTorizo>(), WeakReferences.mod.Name, "Golden Torizo",
				(Func<bool>)(() => MWorld.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo)), ModContent.ItemType<GoldenTorizoSummon>(),
				new List<int>() {/*ModContent.ItemType<GoldenTorizoTrophy>(),ModContent.ItemType<GoldenTorizoMask>(),*/ ModContent.ItemType<TorizoMusicBox>() },
				new List<int>() { ModContent.ItemType<ScrewAttack>(), ModContent.ItemType<GoldenTorizoBag>() },
				"MetroidMod/NPCs/GoldenTorizo/GoldenTorizo_BossLog");
			}
		}
	}
}
