using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Waver : ModNPC
    {
        /*
         * npc.ai[0] = animation helper. 
         * npc.ai[1] = Y turnaround timer.
         */
        internal readonly float speed = 4;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 28; npc.height = 20;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if (npc.direction == 0)
                npc.direction = 1;

            if (npc.collideX)
            {
                npc.direction *= -1;
                npc.netUpdate = true;
            }
            if(npc.collideY || npc.ai[1]++ >= 180)
            {
                npc.ai[0] = 16;
                npc.ai[1] = 0;

                npc.directionY *= -1;
                npc.netUpdate = true;
            }

            if (npc.ai[0] > 0) npc.ai[0]--;

            npc.velocity.X = npc.direction * speed;
            npc.velocity.Y = npc.directionY * speed;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[0] <= 0)
                npc.frame.Y = 0;
            else
            {
                if (npc.ai[0] > 12)
                    npc.frame.Y = 1 * frameHeight;
                else if (npc.ai[0] > 8)
                    npc.frame.Y = 2 * frameHeight;
                else if (npc.ai[0] > 4)
                    npc.frame.Y = 3 * frameHeight;
                else
                    npc.frame.Y = 4 * frameHeight;
            }
        }
    }
}
