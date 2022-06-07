using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Utility
{
	public class Squeept : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.
		 * NPC.ai[1] = jump timer + animation helper.
		 * NPC.ai[2] = 'landing' animation helper.
		 */
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 9;
		}
		public override void SetDefaults()
		{
			NPC.width = 30; NPC.height = 32;

			/* Temporary NPC values */
			NPC.scale = 1.2F;
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
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A useful step stool when frozen. Found in burning hot places.")
			});
		}

		public override bool PreAI()
		{
			if(NPC.ai[0] == 0) // Idle/grounded phase.
			{
				if (NPC.localAI[2] > 0) NPC.localAI[2]--;

				if(NPC.ai[1]++ >= 60)
				{
					NPC.velocity.Y = -Main.rand.Next(9, 15);

					NPC.ai[0] = 1;
					NPC.ai[1] = 0;
					NPC.localAI[1] = 0;
					NPC.localAI[2] = 0;
					NPC.netUpdate = true;
				}
			}
			else // Jumping/falling phase.
			{
				if (NPC.velocity.Y >= 0)
					NPC.localAI[1]++;

				if(NPC.collideY && NPC.oldVelocity.Y >= 0)
				{
					NPC.velocity.Y = 0;

					NPC.ai[0] = 0;
					NPC.localAI[1] = 0;
					NPC.localAI[2] = 8;
					NPC.netUpdate = true;
				}
			}

			NPC.velocity.X = 0;

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if(NPC.ai[0] == 0)
			{
				if (NPC.localAI[2] > 0)
				{
					if (NPC.localAI[2] > 4)
						NPC.frame.Y = 6 * frameHeight;
					else
						NPC.frame.Y = 5 * frameHeight;
				}
				else
				{
					if (NPC.ai[1] < 50)
						NPC.frame.Y = 4 * frameHeight;
					else if (NPC.ai[1] < 55)
						NPC.frame.Y = 3 * frameHeight;
					else
						NPC.frame.Y = frameHeight;
				}
			}
			else
			{
				if(NPC.velocity.Y < 0)
				{
					if (NPC.velocity.Y < -5)
						NPC.frame.Y = 2 * frameHeight;
					else
						NPC.frame.Y = 0;
				}
				else
				{
					if (NPC.localAI[1] < 3)
						NPC.frame.Y = 3 * frameHeight;
					else if (NPC.localAI[1] < 6)
						NPC.frame.Y = 4 * frameHeight;
					else if (NPC.localAI[1] < 9)
						NPC.frame.Y = 5 * frameHeight;
					else if (NPC.localAI[1] < 12)
						NPC.frame.Y = 6 * frameHeight;
					else
					{
						if(NPC.frameCounter++ % 5 == 0)
						{
							NPC.frame.Y += frameHeight;
							if (NPC.frame.Y >= 9 * frameHeight)
								NPC.frame.Y = 7 * frameHeight;
						}
					}
				}
			}
		}
	}
}
