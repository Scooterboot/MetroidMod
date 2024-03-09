using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace MetroidMod.Content.NPCs.Mobs.Utility
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
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A flying creature that can be useful as a grappling point and a stepstool when frozen.")
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
