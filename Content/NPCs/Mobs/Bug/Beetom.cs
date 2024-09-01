﻿using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace MetroidMod.Content.NPCs.Mobs.Bug
{
	public class Beetom : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.
		 * NPC.ai[1] = animation timer.
		 * NPC.ai[2] = latched player index.
		 */
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 14;
		}
		public override void SetDefaults()
		{
			NPC.width = 30; NPC.height = 32;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Beetom")
			});
		}

		public override bool PreAI()
		{
			if (NPC.ai[0] == 0) // Grounded state.
			{
				NPC.TargetClosest(true);

				NPC.velocity.X = 0;

				if (NPC.ai[1]++ >= 90)
				{
					NPC.velocity.Y = -8;
					NPC.velocity.X = Main.rand.Next(3, 8) * NPC.direction;

					NPC.ai[0] = 1;
					NPC.ai[1] = 0;
				}
			}
			else if (NPC.ai[0] == 1) // Aired state.
			{
				if (NPC.collideY && NPC.velocity.Y > 0)
					NPC.ai[0] = 0;

				if (NPC.collideX)
					NPC.velocity.X = -NPC.velocity.X * .8F;
			}
			else if (NPC.ai[0] == 2) // Latched state.
			{
				NPC.Center = Main.player[(int)NPC.ai[2]].Center;
			}

			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			NPC.ai[0] = 2;
			NPC.ai[2] = target.whoAmI;
			NPC.netUpdate = true;
		}

		public override void FindFrame(int frameHeight)
		{
			//NPC.visualOffset = new Vector2(0, -2); // Temporary.
			if (NPC.ai[0] == 0)
			{
				if (NPC.ai[1] < 8) // Just landed, start land animation. Very ugly, I know :S
				{
					if (NPC.ai[1] < 4)
						NPC.frame.Y = 8 * frameHeight;
					else if (NPC.ai[1] <= 8)
						NPC.frame.Y = 7 * frameHeight;
				}
				else if (NPC.ai[1] >= 82) // Close to jumping, start jump animation. Also very ugly. Refactor?
				{
					if (NPC.ai[1] < 86)
						NPC.frame.Y = 7 * frameHeight;
					else if (NPC.ai[1] <= 90)
						NPC.frame.Y = 8 * frameHeight;
				}
				else if (NPC.frameCounter++ >= 4)
				{
					NPC.frame.Y = (NPC.frame.Y + frameHeight) % (4 * frameHeight);
					NPC.frameCounter = 0;
				}
			}
			else if (NPC.ai[0] == 1)
			{
				NPC.frame.Y = 9 * frameHeight;

				NPC.spriteDirection = NPC.direction;
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.frameCounter++ >= 4)
				{
					NPC.frame.Y += frameHeight;
					if (NPC.frame.Y >= 14 * frameHeight)
						NPC.frame.Y = 10 * frameHeight;
					NPC.frameCounter = 0;
				}
			}
		}
	}
}
