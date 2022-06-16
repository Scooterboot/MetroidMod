using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Flying
{
	public class Waver : MNPC
	{
		/*
		 * NPC.ai[0] = Y turnaround timer.
		 */
		internal readonly float xSpeed = 4;
		internal readonly float ySpeed = 3;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 5;
		}
		public override void SetDefaults()
		{
			NPC.width = 22; NPC.height = 16;

			/* Temporary NPC values */
			NPC.scale = 2;
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("A floating creature that attacks any victim by flying into them. Unlike the Skree, they do not self-destruct.")
			});
		}

		public override bool PreAI()
		{
			if (NPC.direction == 0)
				NPC.direction = 1;

			if (NPC.collideX)
			{
				NPC.direction *= -1;
				NPC.netUpdate = true;
			}
			if(NPC.collideY || NPC.ai[0]++ >= 180)
			{
				NPC.ai[0] = 0;

				NPC.directionY *= -1;
				NPC.netUpdate = true;
			}

			NPC.velocity.X = NPC.direction * xSpeed;
			NPC.velocity.Y = NPC.directionY * ySpeed;

			return (false);
		}

		public override void FindFrame(int frameHeight)
		{
			int frameCount;

			NPC.frameCounter++;
			frameCount = Main.npcFrameCount[NPC.type];
			if (NPC.frameCounter >= 6)
			{
				if (NPC.directionY == 1)
					NPC.frame.Y = (NPC.frame.Y - frameHeight < 0) ? (frameHeight * (frameCount - 1)) : (NPC.frame.Y - frameHeight);
				else
					NPC.frame.Y = (NPC.frame.Y + frameHeight) % (frameHeight * frameCount);

				NPC.frameCounter = 0;
			}
			NPC.spriteDirection = -NPC.direction;
		}
	}
}
