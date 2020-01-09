using System;

using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MetroidMod.NPCs.Mobs
{
	/*
	 *	Generic crawler class for all wall crawling mobs.
	 */
	public class Crawler : ModNPC
	{
		protected float crawlSpeed = 0;
		protected int rotationXOffset = 0;
		protected int rotationYOffset = 0;
		public override bool Autoload(ref string name)
		{
			return (false);
		}

		public override void AI()
		{
			if (npc.ai[0] == 0)
			{
				npc.TargetClosest();
				npc.directionY = 1;
				npc.ai[2] = npc.direction;

				npc.ai[0] = 1;
				npc.netUpdate = true;
			}

			if (npc.ai[1] == 0)
			{
				if (npc.collideY)
					npc.ai[0] = 2;

				if (!npc.collideY && npc.ai[0] == 2)
				{
					npc.direction = -npc.direction;
					npc.ai[0] = npc.ai[1] = 1;
				}

				if (npc.collideX)
				{
					npc.directionY = -npc.directionY;
					npc.ai[1] = 1;
				}
			}
			else
			{
				if (npc.collideX)
					npc.ai[0] = 2;

				if (!npc.collideX && npc.ai[0] == 2)
				{
					npc.directionY = -npc.directionY;
					npc.ai[0] = 1;
					npc.ai[1] = 0;
				}

				if (npc.collideY)
				{
					npc.direction = -npc.direction;
					npc.ai[1] = 0;
				}
			}

			npc.velocity.X = npc.direction * crawlSpeed;
			npc.velocity.Y = npc.directionY * crawlSpeed;
		}

		public override void FindFrame(int frameHeight)
		{
			if (npc.frameCounter++ >= 8)
			{
				npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
				npc.frameCounter = 0;
			}

			// Rotate the NPC correctly (visually).
			if (npc.direction == npc.ai[2])
			{
				if (npc.directionY == 1)
				{
					npc.rotation = 0;
					npc.visualOffset = new Vector2(0, 0);
				}
				else
				{
					npc.rotation = MathHelper.PiOver2 * -npc.ai[2];
					npc.visualOffset = new Vector2(rotationXOffset * npc.ai[2], 0);
				}
			}
			else
			{
				if (npc.directionY == 1)
				{
					npc.rotation = MathHelper.PiOver2 * npc.ai[2];
					npc.visualOffset = new Vector2(-rotationXOffset * npc.ai[2], 0);
				}
				else
				{
					npc.rotation = (float)Math.PI;
					npc.visualOffset = new Vector2(0, -rotationYOffset);
				}
			}
		}
	}
}
