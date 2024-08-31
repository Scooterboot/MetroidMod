using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace MetroidMod.Content.NPCs.Mobs.Metroid
{
	public class Mochtroid : MNPC
	{
		/*
		 * NPC.ai[0] = animation manager (pingpong).
		 */

		internal readonly float speed = 3.5F;
		internal readonly float acceleration = .04F;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
		}
		public override void SetDefaults()
		{
			NPC.width = 28; NPC.height = 20;

			/* Temporary NPC values */
			NPC.scale = 1.5F;
			NPC.damage = 0;
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Mochtroid")
			});
		}
		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float chance = SpawnCondition.Corruption.Chance + SpawnCondition.Crimson.Chance;
			if(Main.hardMode)
			{
				chance *= 0.5f;
			}
			return chance;
		}*/

		public override bool PreAI()
		{
			if (NPC.ai[0] == 0)
				NPC.ai[0] = 1;

			NPC.TargetClosest();

			Vector2 targetVelocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * speed;

			if (NPC.velocity.X < targetVelocity.X)
			{
				NPC.velocity.X += acceleration;
				if (NPC.velocity.X < 0)
					NPC.velocity.X *= .98F;
			}
			else if (NPC.velocity.X > targetVelocity.X)
			{
				NPC.velocity.X -= acceleration;
				if (NPC.velocity.X > 0)
					NPC.velocity.X *= .98F;
			}

			if (NPC.velocity.Y < targetVelocity.Y)
			{
				NPC.velocity.Y += acceleration;
				if (NPC.velocity.Y < 0)
					NPC.velocity.Y *= .98F;
			}
			else if (NPC.velocity.Y > targetVelocity.Y)
			{
				NPC.velocity.Y -= acceleration;
				if (NPC.velocity.Y > 0)
					NPC.velocity.Y *= .98F;
			}

			if (NPC.collideX)
			{
				NPC.netUpdate = true;
				NPC.velocity.X = NPC.oldVelocity.X * -.7F;
			}
			if (NPC.collideY)
			{
				NPC.netUpdate = true;
				NPC.velocity.Y = NPC.oldVelocity.Y * -.7F;
			}

			if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) || (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) && !NPC.justHit)
				NPC.netUpdate = true;

			Rectangle collisionRect = new Rectangle(
				(int)NPC.position.X - 32, (int)NPC.position.Y - 32, NPC.width + 64, NPC.height + 64);
			// Proximity damage.
			for (int i = 0; i < 255; ++i)
			{
				if (!Main.player[i].active || Main.player[i].dead) continue;

				if (Main.player[i].getRect().Intersects(collisionRect) &&
					Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("could not get away"), 5, 0, false, false, 0, false) != 0)
				{
					Main.player[i].hurtCooldowns[0] = 15;
				}
			}

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 6)
			{
				NPC.frame.Y = NPC.frame.Y + (int)(frameHeight * NPC.localAI[0]);

				if (NPC.frame.Y == 2 * frameHeight)
					NPC.localAI[0] = -1;
				else if (NPC.frame.Y == 0)
					NPC.localAI[0] = 1;

				NPC.frameCounter = 0;
			}

			NPC.rotation = NPC.velocity.X * 0.2f;
		}
	}
}
