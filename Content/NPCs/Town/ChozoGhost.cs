#region Using directives

using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidMod.Common.Systems;
using MetroidMod.Content.SuitAddons;
using MetroidMod.Content.Items.Armors;
using MetroidMod.Content.Items.Vanity;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Items.Tools;
using MetroidMod.ID;

#endregion

namespace MetroidMod.Content.NPCs.Town
{
	[AutoloadHead]
	public class ChozoGhost : ModNPC
	{
		public const string ShopName = "Chozo Shop";
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 16;

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;

			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;

			NPCID.Sets.HatOffsetY[Type] = 4;

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
			//chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue4"));

			int wdoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (wdoctor >= 0)
			{
				chat.Add(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.Dialogue.ChozoGhost.StandardDialogue3", Main.npc[wdoctor].GivenName), 0.5);
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
			if (player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>() && (player.armor[1].ModItem as PowerSuitBreastplate).SuitAddons[SuitAddonSlotID.Suit_Varia] == SuitAddonLoader.GetAddon<VariaSuitAddon>().Item)
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
			var npcShop = new NPCShop(Type, ShopName)
			.Add(ModContent.ItemType<Items.Boss.TorizoSummon>());

			if (MSystem.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo))
			{
				npcShop.Add(ModContent.ItemType<Items.Boss.GoldenTorizoSummon>());
			}

			if (Main.hardMode)
			{
				npcShop.Add(ModContent.ItemType<PowerBeam>());
				npcShop.Add(ModContent.ItemType<MissileLauncher>());

				if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
				{
					npcShop.Add(SuitAddonLoader.GetAddon<VariaSuitV2Addon>().ItemType);
				}
				else if (NPC.downedMechBossAny)
				{
					npcShop.Add(SuitAddonLoader.GetAddon<VariaSuitAddon>().ItemType);
				}
				else
				{
					npcShop.Add(ModContent.ItemType<PowerSuitHelmet>());
					npcShop.Add(ModContent.ItemType<PowerSuitBreastplate>());
					npcShop.Add(ModContent.ItemType<PowerSuitGreaves>());
				}

				if (NPC.downedPlantBoss)
				{
					npcShop.Add(SuitAddonLoader.GetAddon<GravitySuitAddon>().ItemType);
				}
			}

			npcShop.Add(ModContent.ItemType<VanityPack>());

			if (MSystem.bossesDown.HasFlag(MetroidBossDown.downedPhantoon))
			{
				npcShop.Add(ModContent.ItemType<VanityPack_Prime>());
			}
			if (NPC.downedMoonlord)
			{
				npcShop.Add(ModContent.ItemType<VanityPack_Lunar>());
			}

			if (NPC.downedMechBossAny)
			{
				npcShop.Add(ModContent.ItemType<RedKeycard>());
			}
			if (NPC.downedGolemBoss)
			{
				npcShop.Add(ModContent.ItemType<GreenKeycard>());
			}
			if (NPC.downedAncientCultist)
			{
				npcShop.Add(ModContent.ItemType<YellowKeycard>());
			}

			if(Main.hardMode && Main.bloodMoon)
			{
				npcShop.Add(ModContent.ItemType<Items.Tiles.MissileExpansion>());
			}
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
