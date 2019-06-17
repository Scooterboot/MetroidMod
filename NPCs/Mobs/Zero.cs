using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Zero : ModNPC
    {
        internal readonly float[] speeds = new float[6] { .05F, .15F, .25F, .35F, .25F, .15F };

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }
        public override void SetDefaults()
        {
            npc.width = 32; npc.height = 16;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.noGravity = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {

            if (npc.ai[0] == 0)
            {
                npc.TargetClosest();
                npc.directionY = 1;

                npc.ai[0] = 1;
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

            npc.ai[2] += .06F;
            npc.velocity = new Vector2(npc.direction, npc.directionY) * speeds[(int)npc.ai[2] % 6];

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = ((int)npc.ai[2] * frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);

			// Rotate the NPC correctly (visually).
			if (npc.direction == 1)
			{
				if (npc.directionY == 1)
				{
					npc.rotation = 0;
					npc.visualOffset = new Vector2(0, 0);
				}
				else
				{
					npc.rotation = -MathHelper.PiOver2;
					npc.visualOffset = new Vector2(20, 0);
				}
			}
			else
			{
				if (npc.directionY == 1)
				{
					npc.rotation = MathHelper.PiOver2;
					npc.visualOffset = new Vector2(-20, 0);
				}
				else
				{
					npc.rotation = (float)Math.PI;
					npc.visualOffset = new Vector2(0, -8);
				}
			}
		}
    }
}
