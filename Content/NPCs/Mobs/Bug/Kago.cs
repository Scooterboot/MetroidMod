using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace MetroidMod.Content.NPCs.Mobs.Bug
{
	public class Kago : MNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 3;
		}
		public override void SetDefaults()
		{
			NPC.width = 8; NPC.height = 8;

			/* Temporary NPC values */
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = .2F;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A small creature that tends to latch onto victims in swarms when it's hive is attacked.")
			});
		}

		public bool spawn = false;
		public override bool PreAI()
		{
			if (!spawn)
			{
				NPC.scale = (Main.rand.Next(15, 21) * 0.1f);
				NPC.defense = (int)((float)NPC.defense * NPC.scale);
				NPC.damage = (int)((float)NPC.damage * NPC.scale);
				NPC.life = (int)((float)NPC.life * NPC.scale);
				NPC.lifeMax = NPC.life;
				NPC.value = (float)((int)(NPC.value * NPC.scale));
				NPC.npcSlots *= NPC.scale;
				NPC.knockBackResist *= 2f - NPC.scale;
				NPC.ai[1] = Main.rand.Next(20, 61);
				spawn = true;
			}
			return true;
		}

		public override void AI()
		{
			if (NPC.velocity.Y == 0f)
			{
				NPC.velocity = Vector2.Zero;
				if (NPC.collideY && NPC.oldVelocity.Y != 0f && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
					NPC.position.X -= NPC.velocity.X + (float)NPC.direction;

				NPC.ai[0]++;
				if (NPC.ai[0] >= NPC.ai[1])
				{
					NPC.TargetClosest();

					NPC.velocity.X = Main.rand.Next(2, 8) * NPC.direction;
					NPC.velocity.Y = Main.rand.Next(-7, -3);

					NPC.ai[0] = 0;
					NPC.ai[1] = Main.rand.Next(20, 61); // Next jump time.
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity.Y == 0)
			{
				// Just landed, revert to 'idle' frame.
				if (NPC.frame.Y != 0)
				{
					if (NPC.frameCounter++ < 5)
						NPC.frame.Y = frameHeight;
					else
					{
						NPC.frame.Y = 0;
						NPC.frameCounter = 0;
					}
				}
			}
			else
			{
				// Just aired, start launch animation
				if (NPC.frame.Y != 2 * frameHeight)
				{
					if (NPC.frameCounter++ < 5)
						NPC.frame.Y = frameHeight;
					else
					{
						NPC.frame.Y = 2 * frameHeight;
						NPC.frameCounter = 0;
					}
				}
			}
		}
	}
}
