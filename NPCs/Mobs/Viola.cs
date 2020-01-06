using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Viola : ModNPC
    {
        /*
         * npc.ai[0] & npc.ai[1] = state managers.
         * npc.ai[2] = animation state (ping-pong).
         */
        internal readonly float speed = 1.5F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }
        public override void SetDefaults()
        {
            npc.width = 12; npc.height = 12;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 20;
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;

            npc.noGravity = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                npc.TargetClosest();
                npc.directionY = 1;

                npc.ai[0] = 1;
                npc.ai[2] = 1;
            }

            if(npc.ai[1] == 0)
            {
                if (npc.collideY)
                    npc.ai[0] = 2;

                if(!npc.collideY && npc.ai[0] == 2)
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

            npc.velocity.X = npc.direction * speed;
            npc.velocity.Y = npc.directionY * speed;

            Lighting.AddLight(npc.Center, 0, .72F, .77F);
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 6)
            {
                npc.frame.Y = npc.frame.Y + (int)(frameHeight * npc.localAI[2]);

                if (npc.frame.Y == 5 * frameHeight)
                    npc.localAI[2] = -1;
                else if (npc.frame.Y == 0)
                    npc.localAI[2] = 1;

                npc.frameCounter = 0;
            }
        }
    }
}
