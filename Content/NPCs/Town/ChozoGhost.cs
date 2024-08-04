#region Using directives

using System.Collections.Generic;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.Content.Items.Armors;
using MetroidMod.Content.Items.Miscellaneous;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Items.Tools;
using MetroidMod.Content.Items.Vanity;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Pets;
using MetroidMod.Content.SuitAddons;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

#endregion

namespace MetroidMod.Content.NPCs.Town
{
	[AutoloadHead]
	public class ChozoGhost : ModNPC
	{
		public const string ShopName = "Chozo Shop";

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 16; //24
			NPCID.Sets.ExtraFramesCount[Type] = 9; //13
			NPCID.Sets.AttackFrameCount[Type] = 4;//6
			NPCID.Sets.DangerDetectRange[Type] = 700;

			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;

			NPCID.Sets.HatOffsetY[Type] = 4;

			//NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			//NPCID.Sets.ShimmerTownTransform[Type] = true;

			NPC.Happiness
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate)
			;
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
			);
		}
		public override void Load()
		{
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

				new FlavorTextBestiaryInfoElement("A deceased member of the Gizzard tribe of Chozo who has lingering guilt. They are susceptible to phazon and prefer to avoid it.")
			});
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 40;

			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.knockBackResist = 0f;

			NPC.townNPC = true;
			NPC.friendly = true;

			NPC.aiStyle = 7;
			AnimationType = NPCID.Guide;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
			=> MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo);

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}
		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Old Bird",
				"Ou-Qua",
				"De'la",
				"Colorless",
				"Grey Voice"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue1", Main.LocalPlayer.name));
			chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue2"));
			chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue3"));
			chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue3"));
			//chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue4"));

			int wdoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (wdoctor >= 0)
			{
				chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.WitchDialogue", Main.npc[wdoctor].GivenName), 0.5);
			}
			if (Main.LocalPlayer.TryMetroidPlayer(out MPlayer mp) && mp.ShouldShowArmorUI) // if player is in suit, give them a tip on Energy
			{
				chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.EnergyHint"));
			}

			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			//button2 = Language.GetTextValue("LegacyInterface.64");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = ShopName; // Opens the shop
			}
		}
		string QuestChat(NPC npc)
		{
			Player player = Main.player[Main.myPlayer];

			string boss2 = "Eater of Worlds";
			if (WorldGen.crimson)
			{
				boss2 = "Brain of Cthulhu";
			}

			string chat =
			"Now that you have defeated the Torizo, your next objective is to create a Power Suit and Power Beam. " +
			"You should be able to craft a Power Beam with the materials you have now, however a Power Suit will require fully built Energy Tanks.\n" +
			"Defeat the " + boss2 + " and use its spoils to craft Energy Tanks, then return to me for further guidance.\n" +
			" \n" +
			"Objective: Defeat the " + boss2;

			if (NPC.downedBoss2)
			{
				chat = "Now that you have defeated the mighty beast, you can now craft Energy Tanks." +
				"Using these Energy Tanks, you can re-craft Chozite Armor into an adequate Power Suit.\n" +
				"Speak to me once you have equipped a full Power Suit, and I will guide you to your next objective.\n" +
				" \n" +
				"Objective: Craft and equip a Power Suit";
			}
			if (player.armor[0].type == ModContent.ItemType<PowerSuitHelmet>() && player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>() && player.armor[2].type == ModContent.ItemType<PowerSuitGreaves>())
			{
				chat = "With that Power Suit, your potential knows no bounds. But your journey has only just begun.\n" +
				"You may also find a variety of Power Beam addons throughout the world, or craft them with a variety of materials.\n" +
				"Your next objective is to dig deep into the earth until you reach the ferocious Underworld.\n" +
				"Pillage its resources to upgrade your Power Suit further." +
				" \n" +
				"Objective: Craft a Varia Suit using Hellstone Bars and equip it";
			}
			if (player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>() && (player.armor[1].ModItem as PowerSuitBreastplate).SuitAddons[SuitAddonSlotID.Suit_Barrier] == SuitAddonLoader.GetAddon<VariaSuitAddon>().Item)
			{
				chat = "You are now ready for your next challenge. Go to the Dungeon and free the old man there of his curse, " +
				"then you will gain entry to the Dungeon. Many spoils await you inside that will help you on your journey." +
				"In particular, the bones of the skeletons inside may prove useful.\n" +
				" \n" +
				"Objective: Defeat Skeletron";
			}
			if (NPC.downedBoss3)
			{
				chat = "[Quest System WIP]";
			}

			return chat;
		}

		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName);
			Condition Torizo = new Condition("Conditions.downedTorizo", () => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo));
			Condition Kraid = new Condition("Conditions.downedKraid", () => MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid));
			Condition Phantoon = new Condition("Conditions.downedPhantoon", () => MSystem.bossesDown.HasFlag(MetroidBossDown.downedPhantoon));
			Condition Nightmare = new Condition("Conditions.downedNightmare", () => MSystem.bossesDown.HasFlag(MetroidBossDown.downedNightmare));
			Condition Gold = new Condition("Conditions.downedGoldenTorizo", () => MSystem.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo));
			Condition Phazon = new Condition("Conditions.spawnedPhazon", () => MSystem.PhazonSpawn != true);
			npcShop.Add<PowerBeam>(Condition.Hardmode);
			npcShop.Add<ArmCannon>(Condition.Hardmode);
			npcShop.Add<MissileLauncher>(Condition.Hardmode);
			npcShop.Add<MissileExpansion>(Condition.Hardmode, Condition.BloodMoon);
			npcShop.Add(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType,Condition.Hardmode, Condition.BloodMoon);
			npcShop.Add<UAExpansion>(Condition.Hardmode, Condition.BloodMoon);
			npcShop.Add<PowerSuitHelmet>(Condition.Hardmode);
			npcShop.Add<PowerSuitBreastplate>(Condition.Hardmode);
			npcShop.Add<PowerSuitGreaves>(Condition.Hardmode);
			npcShop.Add(SuitAddonLoader.GetAddon<VariaSuitV2Addon>().ItemType, Condition.DownedMechBossAll);
			npcShop.Add(SuitAddonLoader.GetAddon<GravitySuitAddon>().ItemType, Condition.DownedPlantera);
			npcShop.Add<RedKeycard>(Condition.DownedMechBossAny);
			npcShop.Add<GreenKeycard>(Condition.DownedGolem);
			npcShop.Add<ChozoRuinsBag>(Torizo);
			npcShop.Add<BrinstarBag>(Kraid);
			npcShop.Add<NorfairBag>(Kraid);//Ridley); // no ridley yet
			npcShop.Add<Sector2TropicBag>(Kraid);//Zazabi); // no zazabi yet
			npcShop.Add<Sector5ArcticBag>(Nightmare);
			npcShop.Add<TourianBag>(Condition.DownedMoonLord);//MotherBrain // no mother brain yet
			npcShop.Add<VanityPack>();
			npcShop.Add<VanityPack_Prime>(Phantoon);
			npcShop.Add<VanityPack_Lunar>(Condition.DownedMoonLord);
			npcShop.Add<Items.Boss.TorizoSummon>();
			npcShop.Add<Items.Boss.GoldenTorizoSummon>(Gold);
			npcShop.Add<PhazonCore>(Condition.DownedPlantera, Phazon);
			npcShop.Add<AQAPlating>(Condition.IsNpcShimmered);
			npcShop.Add<ARCPlating>(Condition.IsNpcShimmered);
			npcShop.Add<PYRPlating>(Condition.IsNpcShimmered);
			npcShop.Add<SRXPlating>(Condition.IsNpcShimmered);
			npcShop.Add<ResearchCenterPlating>(Condition.IsNpcShimmered);
			npcShop.Add<CrocomireBone>(Condition.InUnderworld);
			npcShop.Register();
		}

		private int frame = 0;
		private int tFrame = 0;
		private int tFrameCounter = 0;
		private int rise = 0;
		private int riseNum = -1;
		public override void FindFrame(int frameHeight)
		{
			if (++tFrameCounter >= 6)
			{
				tFrameCounter = 0;
				tFrame = (tFrame + 1) % 4;
			}

			if ((tFrame == 0 || tFrame == 2) && tFrameCounter == 0)
			{
				rise += riseNum;
				if (rise < -6)
				{
					riseNum = 1;
				}
				if (rise >= 0)
				{
					riseNum = -1;
				}
			}

			if (!Main.dedServ)
			{
				// 'Loading' the NPC to make sure its texture is properly populated in the Main.npcTexture array.
				Main.instance.LoadNPC(NPCID.Guide);

				int gFrame = (NPC.frame.Y / Terraria.GameContent.TextureAssets.Npc[NPCID.Guide].Value.Height) * Main.npcFrameCount[NPCID.Guide];

				if (gFrame == 16)
				{
					frame = 4;
				}
				else if (gFrame == 17)
				{
					frame = 8;
				}
				else if (gFrame >= 21 && gFrame <= 25)
				{
					frame = 12;
				}
				else
				{
					frame = 0;
				}
			}

			NPC.spriteDirection = NPC.direction;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;

			SpriteEffects spriteEffects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Rectangle drawFrame = tex.Frame(1, Main.npcFrameCount[NPC.type], 0, frame + tFrame);
			Vector2 origin = drawFrame.Size() / 2;

			sb.Draw(tex, new Vector2((int)(NPC.Center.X - screenPos.X), (int)(NPC.Center.Y + (rise - 4) - screenPos.Y)), drawFrame, NPC.GetAlpha(Color.White), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

			return false;
		}
	}
}
