using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace MetroidMod.NPCs.Town
{
	[AutoloadHead]
	public class ChozoGhost : ModNPC
	{
		/*public override string Texture
		{
			get
			{
				return "MetroidMod/NPCs/Town/ChozoGhost";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "MetroidMod/NPCs/ExamplePerson_Alt_1" };
			}
		}*/

		public override bool Autoload(ref string name)
		{
			name = "ChozoGhost";
			return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
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
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			//npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			animationType = NPCID.Guide;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return MWorld.bossesDown.HasFlag(MetroidBossDown.downedTorizo);
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(5))
			{
				case 0:
					return "Grey Voice";
				case 1:
					return "Old Bird";
				case 2:
					return "Ou-Qua";
				case 3:
					return "Dryn";
				default:
					return "De'la";
			}
		}

		public override void FindFrame(int frameHeight)
		{
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			
			chat.Add("Greetings, "+Main.player[Main.myPlayer].name+"! How may I aid you?");
			chat.Add("Please, do not be frightened by my appearance. I may be a ghost, but I am not evil.");
			chat.Add("We may look different, but inside... we have the same 'heart' as you.");
			//chat.Add("Be cautious of my brethren in our ancient ruins, for they may put your mettle to the test.");

			int wdoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (wdoctor >= 0)
			{
				chat.Add(Main.npc[wdoctor].GivenName + " keeps shaking his witchcraft 'things' at me whenever I come near him. Could ask him to stop?", 0.5);
			}
			
			//chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			//chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = Language.GetTextValue("LegacyInterface.64");
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
			else
			{
				QuestChat(npc);
			}
		}
		void QuestChat(NPC npc)
		{
			Player player = Main.player[Main.myPlayer];
			
			string chat = 
			"You have defeated the Torizo, and in doing so, gained my attention. My name is "+npc.GivenName+", and I will aid you in any way I can.\n" + 
			"With your current materials, you should be able to craft a Power Beam, however, you may want an armor upgrade to go along with it.\n" +
			"Your next objective is to create a Power Suit. To do so, you will need a set of Chozite Armor and Energy Tanks." +
			"Use the spoils of the Torizo and the spoils of an evil beast to craft Energy Tanks.\n" +
			"Return to me once you have defeated the Eater of Worlds or the Brain of Cthulhu and I will further guide you through the process, should you need it." +
			"\n\n\n" +
			"Objective: Defeat the Eater of Worlds or Brain of Cthulhu.";
			
			if(NPC.downedBoss2)
			{
				chat = "Now that you have defeated the mighty beast, you can now craft Energy Tanks."+
				"Using these Energy Tanks, you can re-craft Chozite Armor into an adequate Power Suit.\n"+
				"Speak to me once you have equipped a full Power Suit, and I will guide you to your next objective." +
				"\n\n\n" +
				"Objective: Craft and equip a Power Suit";
			}
			if(player.armor[0].type == mod.ItemType("PowerSuitHelmet") && player.armor[1].type == mod.ItemType("PowerSuitBreastplate") && player.armor[2].type == mod.ItemType("PowerSuitGreaves"))
			{
				chat = "With that Power Suit, your potential knows no bounds. But your journey has only just begun.\n"+
				"If you have not already, I recommend you craft a Power Beam."+
				"You may also find a variety of Power Beam addons throughout the world, or craft them with a variety of materials.\n"+
				"Your next objective is to dig deep into the earth until you reach the ferocious Underworld."+
				"Pillage its resources to upgrade your Power Suit further." +
				"\n\n\n" +
				"Objective: Craft a Varia Suit using Hellstone Bars and equip it";
			}
			if(player.armor[0].type == mod.ItemType("VariaSuitHelmet") && player.armor[1].type == mod.ItemType("VariaSuitBreastplate") && player.armor[2].type == mod.ItemType("VariaSuitGreaves"))
			{
				chat = "You are now ready for your next challenge. Go to the Dungeon and free the old man there of his curse, "+
				"then you will gain entry to the Dungeon. Many spoils await you inside that will help you on your journey."+
				"In particular, the bones of the skeletons inside may prove useful." +
				"\n\n\n" +
				"Objective: Defeat Skeletron";
			}
			if(NPC.downedBoss3)
			{
				chat = "";
			}
			
			Main.npcChatText = chat;
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
			
			/*shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleItem"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("EquipMaterial"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("BossItem"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleWorkbench"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleChair"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleDoor"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleBed"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleChest"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExamplePickaxe"));
			nextSlot++;
			shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleHamaxe"));
			nextSlot++;
			if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>(mod).ZoneExample)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleWings"));
				nextSlot++;
			}
			if (Main.moonPhase < 2)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleSword"));
				nextSlot++;
			}
			else if (Main.moonPhase < 4)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleGun"));
				nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleBullet"));
				nextSlot++;
			}
			else if (Main.moonPhase < 6)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("ExampleStaff"));
				nextSlot++;
			}
			else
			{
			}
			// Here is an example of how your npc can sell items from other mods.
			if (ModLoader.GetLoadedMods().Contains("SummonersAssociation"))
			{
				shop.item[nextSlot].SetDefaults(ModLoader.GetMod("SummonersAssociation").ItemType("BloodTalisman"));
				nextSlot++;
			}*/
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
		public override void PostAI()
		{
			tFrameCounter++;
			if(tFrameCounter >= 6)
			{
				tFrame++;
				tFrameCounter = 0;
			}
			if(tFrame >= 4)
			{
				tFrame = 0;
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
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			npc.spriteDirection = npc.direction;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if(npc.direction == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Main.npcTexture[npc.type];
			int height = tex.Height / Main.npcFrameCount[npc.type];
			int cFrame = frame + tFrame;
			sb.Draw(tex, new Vector2((int)(npc.Center.X - Main.screenPosition.X),(int)(npc.Center.Y + (rise-4) - Main.screenPosition.Y)),new Rectangle?(new Rectangle(0,height*cFrame,tex.Width,height)),npc.GetAlpha(Color.White),npc.rotation,new Vector2(tex.Width/2,height/2),npc.scale,spriteEffects,0f);
			return false;
		}
	}
}