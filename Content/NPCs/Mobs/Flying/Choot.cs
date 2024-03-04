using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Flying
{
	public class Choot : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.
		 * NPC.ai[1] = ai timer.
		 */
		public bool spawn = false;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 5;
		}
		public override void SetDefaults()
		{
			NPC.width = 32; NPC.height = 16;

			/* Temporary NPC values */
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
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
				new FlavorTextBestiaryInfoElement("A jumping creature that spits acidic saliva on potential meals. Useful as platforms when frozen.")
			});
		}

		public override bool PreAI()
		{
			if (!spawn)
			{
				NPC.scale = (Main.rand.Next(13, 21) * 0.1f);
				NPC.defense = (int)((float)NPC.defense * NPC.scale);
				NPC.damage = (int)((float)NPC.damage * NPC.scale);
				NPC.life = (int)((float)NPC.life * NPC.scale);
				NPC.lifeMax = NPC.life;
				NPC.value = (float)((int)(NPC.value * NPC.scale));
				NPC.npcSlots *= NPC.scale;
				NPC.knockBackResist *= 2f - NPC.scale;
				spawn = true;
			}
			return true;
		}
		public override void AI()
		{
			if(NPC.ai[0] == 0) // Dormant/idle ai.
			{
				NPC.TargetClosest(false);
				NPC.velocity = Vector2.Zero;

				if(Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) <= 64)
				{
					NPC.velocity.Y = -8;
					NPC.ai[0] = 1;
				}
			}
			else if(NPC.ai[0] == 1) // Jump.
			{
				// Animation timer.
				NPC.ai[1]++;

				NPC.velocity.Y += 0.15F;
				//NPC.velocity.Y *= 0.97F;
				if (NPC.velocity.Y >= -1)
				{
					NPC.ai[0] = 2;
					NPC.ai[1] = 0;
				}
			}
			else if(NPC.ai[0] == 2) // Downfall
			{
				// Animation timer.
				NPC.ai[1]++;

				NPC.velocity.Y = 1.5F;
				NPC.ai[2] += 0.06F;

				NPC.velocity.X = (float)Math.Cos(NPC.ai[2]) * 3;

				if (NPC.collideY) // Reset to idle state.
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] == 0)
				NPC.frame.Y = 0;
			else if (NPC.ai[0] == 1)
			{
				if (NPC.ai[1] < 10)
					NPC.frame.Y = frameHeight;
				else
					NPC.frame.Y = 2 * frameHeight;
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.ai[1] < 10)
					NPC.frame.Y = 3 * frameHeight;
				else
					NPC.frame.Y = 4 * frameHeight;
			}
		}
	}
}
