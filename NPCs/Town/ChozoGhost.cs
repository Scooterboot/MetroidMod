#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidMod.Common.Worlds;

#endregion

namespace MetroidMod.NPCs.Town
{
	[AutoloadHead]
	public class ChozoGhost : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 16;

			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;

			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;

			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 18;
			npc.height = 40;

			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.knockBackResist = 0f;

			npc.townNPC = true;
			npc.friendly = true;

			npc.aiStyle = 7;
			animationType = NPCID.Guide;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;	
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
			=> MWorld.bossesDown.HasFlag(MetroidBossDown.downedTorizo);

		public override string TownNPCName()
		{
			switch (Main.rand.Next(5))
			{
				case 1:
					return "Old Bird";
				case 2:
					return "Ou-Qua";
				case 3:
					return "Dryn";
				case 4:
					return "De'la";
				default:
					return "Grey Voice";
			}
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			
			chat.Add("Greetings, " + Main.LocalPlayer.name + "! How may I aid you?");
			chat.Add("Please, do not be frightened by my appearance. I may be a ghost, but I am not evil.");
			chat.Add("We may look different, but inside... We have the same 'heart' as you.");
			//chat.Add("Be cautious of my brethren in our ancient ruins, for they may put your mettle to the test.");

			int wdoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (wdoctor >= 0)
			{
				chat.Add(Main.npc[wdoctor].GivenName + " keeps shaking his witchcraft 'things' at me whenever I come near him. Could ask him to stop?", 0.5);
			}
			
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			//button2 = Language.GetTextValue("LegacyInterface.64");
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
			/*else
			{
				Main.npcChatText = QuestChat(npc);
			}*/
		}
		string QuestChat(NPC npc)
		{
			Player player = Main.player[Main.myPlayer];
			
			string boss2 = "Eater of Worlds";
			if(WorldGen.crimson)
			{
				boss2 = "Brain of Cthulhu";
			}
			
			string chat = 
			"Now that you have defeated the Torizo, your next objective is to create a Power Suit and Power Beam. "+
			"You should be able to craft a Power Beam with the materials you have now, however a Power Suit will require fully built Energy Tanks.\n"+
			"Defeat the "+boss2+" and use its spoils to craft Energy Tanks, then return to me for further guidance.\n"+
			" \n"+
			"Objective: Defeat the "+boss2;
			
			if(NPC.downedBoss2)
			{
				chat = "Now that you have defeated the mighty beast, you can now craft Energy Tanks."+
				"Using these Energy Tanks, you can re-craft Chozite Armor into an adequate Power Suit.\n"+
				"Speak to me once you have equipped a full Power Suit, and I will guide you to your next objective.\n" +
				" \n" +
				"Objective: Craft and equip a Power Suit";
			}
			if(player.armor[0].type == mod.ItemType("PowerSuitHelmet") && player.armor[1].type == mod.ItemType("PowerSuitBreastplate") && player.armor[2].type == mod.ItemType("PowerSuitGreaves"))
			{
				chat = "With that Power Suit, your potential knows no bounds. But your journey has only just begun.\n"+
				"You may also find a variety of Power Beam addons throughout the world, or craft them with a variety of materials.\n"+
				"Your next objective is to dig deep into the earth until you reach the ferocious Underworld.\n"+
				"Pillage its resources to upgrade your Power Suit further." +
				" \n" +
				"Objective: Craft a Varia Suit using Hellstone Bars and equip it";
			}
			if(player.armor[0].type == mod.ItemType("VariaSuitHelmet") && player.armor[1].type == mod.ItemType("VariaSuitBreastplate") && player.armor[2].type == mod.ItemType("VariaSuitGreaves"))
			{
				chat = "You are now ready for your next challenge. Go to the Dungeon and free the old man there of his curse, "+
				"then you will gain entry to the Dungeon. Many spoils await you inside that will help you on your journey."+
				"In particular, the bones of the skeletons inside may prove useful.\n" +
				" \n" +
				"Objective: Defeat Skeletron";
			}
			if(NPC.downedBoss3)
			{
				chat = "[Quest System WIP]";
			}
			
			return chat;
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(mod.ItemType("TorizoSummon"));
			nextSlot++;
			
			if(MWorld.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo))
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("GoldenTorizoSummon"));
				nextSlot++;
			}
			
			if(Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("PowerBeam"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("MissileLauncher"));
				nextSlot++;
				
				if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
				{
					shop.item[nextSlot].SetDefaults(mod.ItemType("VariaSuitV2Helmet"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("VariaSuitV2Breastplate"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("VariaSuitV2Greaves"));
					nextSlot++;
				}
				else if(NPC.downedMechBossAny)
				{
					shop.item[nextSlot].SetDefaults(mod.ItemType("VariaSuitHelmet"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("VariaSuitBreastplate"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("VariaSuitGreaves"));
					nextSlot++;
				}
				else
				{
					shop.item[nextSlot].SetDefaults(mod.ItemType("PowerSuitHelmet"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("PowerSuitBreastplate"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("PowerSuitGreaves"));
					nextSlot++;
				}
				
				if(NPC.downedPlantBoss)
				{
					shop.item[nextSlot].SetDefaults(mod.ItemType("GravitySuitHelmet"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("GravitySuitBreastplate"));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType("GravitySuitGreaves"));
					nextSlot++;
				}
			}
			
			shop.item[nextSlot].SetDefaults(mod.ItemType("VanityPack"));
			nextSlot++;
			
			if(MWorld.bossesDown.HasFlag(MetroidBossDown.downedPhantoon))
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("VanityPack_Prime"));
				nextSlot++;
			}
			if(NPC.downedMoonlord)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("VanityPack_Lunar"));
				nextSlot++;
			}
		}

		/*public override void NPCLoot()
		{
			Item.NewItem(npc.getRect(), mod.ItemType<Items.Armor.ExampleCostume>());
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = mod.ProjectileType("SparklingBall");
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}*/
		
		int frame = 0;
		int tFrame = 0;
		int tFrameCounter = 0;
		int rise = 0;
		int riseNum = -1;
		public override void FindFrame(int frameHeight)
		{
			if(++tFrameCounter >= 6)
			{
				tFrameCounter = 0;
				tFrame = (tFrame + 1) % 4;
			}
			
			if((tFrame == 0 || tFrame == 2) && tFrameCounter == 0)
			{
				rise += riseNum;
				if(rise < -6)
				{
					riseNum = 1;
				}
				if(rise >= 0)
				{
					riseNum = -1;
				}
			}
			
			if(!Main.dedServ)
			{
				// 'Loading' the NPC to make sure its texture is properly populated in the Main.npcTexture array.
				Main.instance.LoadNPC(NPCID.Guide);

				int gFrame = (npc.frame.Y / Main.npcTexture[NPCID.Guide].Height) * Main.npcFrameCount[NPCID.Guide];
				
				if(gFrame == 16)
				{
					frame = 4;
				}
				else if(gFrame == 17)
				{
					frame = 8;
				}
				else if(gFrame >= 21 && gFrame <= 25)
				{
					frame = 12;
				}
				else
				{
					frame = 0;
				}
			}

			npc.spriteDirection = npc.direction;
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];

			SpriteEffects spriteEffects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Rectangle drawFrame = tex.Frame(1, Main.npcFrameCount[npc.type], 0, frame + tFrame);
			Vector2 origin = drawFrame.Size() / 2;

			sb.Draw(tex, new Vector2((int)(npc.Center.X - Main.screenPosition.X), (int)(npc.Center.Y + (rise - 4) - Main.screenPosition.Y)), drawFrame, npc.GetAlpha(Color.White), npc.rotation, origin, npc.scale, spriteEffects, 0f);
			
			return (false);
		}
	}
}