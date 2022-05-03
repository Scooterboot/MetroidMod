using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Utility
{
	public class Ripper : MNPC
	{
		/*
		 * npc.ai[0] = state manager.
		 * npc.ai[1] = Y velocity sinus helper.
		 */
		internal readonly float speed = 2.5F;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 3;
		}
		public override void SetDefaults()
		{
			NPC.width = 18; NPC.height = 12;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 10;
			NPC.lifeMax = 300;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("You can grapple onto this with the Grapple Beam! Not sure how that will help movement...")
			});
		}

		public override bool PreAI()
		{
			if (NPC.direction == 0)
				NPC.direction = 1;

			if (NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze)
			{
				NPC.damage = 0;
				NPC.position = NPC.oldPosition;
			}
			else
			{
				NPC.velocity.X = speed * NPC.direction;

				if (NPC.collideX)
				{
					NPC.velocity.X *= -1;
					NPC.netUpdate = true;
				}

				NPC.velocity.Y = (float)Math.Sin(NPC.ai[1] += .1F) * .5F;

				NPC.direction = NPC.velocity.X < 0 ? -1 : 1;
				NPC.damage = NPC.defDamage;
			}

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 6)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[Type] * frameHeight);
				NPC.frameCounter = 0;
			}

			NPC.spriteDirection = -NPC.direction;
		}
	}
}
