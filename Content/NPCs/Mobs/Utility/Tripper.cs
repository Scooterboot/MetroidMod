using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Utility
{
	public class Tripper : MNPC
	{
		internal readonly float speed = 2F;
		internal readonly float acceleration = .06F;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 3;
		}
		public override void SetDefaults()
		{
			NPC.width = 28; NPC.height = 16;

			/* Temporary NPC values */
			NPC.scale = 1.5F;
			NPC.damage = 0;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override bool PreAI()
		{
			if (NPC.direction == 0)
				NPC.direction = 1;

			float desiredXVelocity = NPC.direction * speed;
			if (desiredXVelocity > 0)
			{
				if (NPC.velocity.X < desiredXVelocity)
					NPC.velocity.X += acceleration;
				else if (NPC.velocity.X > desiredXVelocity)
					NPC.velocity.X = desiredXVelocity;
			}
			else
			{
				if (NPC.velocity.X > desiredXVelocity)
					NPC.velocity.X -= acceleration;
				else if (NPC.velocity.X < desiredXVelocity)
					NPC.velocity.X = desiredXVelocity;
			}

			if (NPC.collideX)
			{
				NPC.velocity.X *= 0;
				NPC.direction *= -1;
				NPC.netUpdate = true;
			}

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.direction > 0)
			{
				if (NPC.velocity.X < speed / 3)
					NPC.frame.Y = 0;
				else if (NPC.velocity.X < (speed / 3) * 2)
					NPC.frame.Y = frameHeight;
				else
				{
					NPC.frameCounter++;
					NPC.frame.Y = frameHeight * ((NPC.frameCounter % 20 < 10) ? 2 : 1);
				}
			}
			else
			{
				if (NPC.velocity.X > -speed / 3)
					NPC.frame.Y = 0;
				else if (NPC.velocity.X > (-speed / 3) * 2)
					NPC.frame.Y = frameHeight;
				else
				{
					NPC.frameCounter++;
					NPC.frame.Y = frameHeight * ((NPC.frameCounter % 20 < 10) ? 2 : 1);
				}
			}

			NPC.spriteDirection = -NPC.direction;
		}
	}
}
