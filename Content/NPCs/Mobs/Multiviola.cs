using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidMod.Content.NPCs.Mobs
{
	public class Multiviola : MNPC
	{
		/*
		 * NPC.ai[0] = animation state (ping-pong) & start state.
		 */

		internal readonly float speed = 3;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Underworld.Chance * 0.15f;
		}
		public override void SetDefaults()
		{
			NPC.width = 12; NPC.height = 12;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 20;
			NPC.knockBackResist = 0f;

			NPC.noGravity = true;
			NPC.behindTiles = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement("A fiery Viola.")
			});
		}

		public override bool PreAI()
		{
			if(NPC.ai[0] == 0)
			{
				NPC.velocity = (Main.rand.NextFloat((float)Math.PI * 2).ToRotationVector2()) * speed;
				NPC.ai[0] = 1;
			}

			if (NPC.collideX)
			{
				NPC.netUpdate = true;
				NPC.velocity.X = -NPC.oldVelocity.X;
			}
			if (NPC.collideY)
			{
				NPC.netUpdate = true;
				NPC.velocity.Y = -NPC.oldVelocity.Y;
			}

			Lighting.AddLight(NPC.Center, .97F, .2F, 0);

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 6)
			{
				NPC.frame.Y = NPC.frame.Y + (int)(frameHeight * NPC.localAI[0]);

				if (NPC.frame.Y == 3 * frameHeight)
					NPC.localAI[0] = -1;
				else if (NPC.frame.Y == 0)
					NPC.localAI[0] = 1;

				NPC.frameCounter = 0;
			}
		}
	}
}
