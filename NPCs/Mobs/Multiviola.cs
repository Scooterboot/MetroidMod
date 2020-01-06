using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Multiviola : ModNPC
    {
        /*
         * npc.ai[0] = animation state (ping-pong) & start state.
         */

        internal readonly float speed = 3;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 12; npc.height = 12;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 20;
            npc.knockBackResist = 0f;

            npc.noGravity = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if(npc.ai[0] == 0)
            {
                npc.velocity = (Main.rand.NextFloat((float)Math.PI * 2).ToRotationVector2()) * speed;
                npc.ai[0] = 1;
            }

            if (npc.collideX)
            {
                npc.netUpdate = true;
                npc.velocity.X = -npc.oldVelocity.X;
            }
            if (npc.collideY)
            {
                npc.netUpdate = true;
                npc.velocity.Y = -npc.oldVelocity.Y;
            }

            Lighting.AddLight(npc.Center, .97F, .2F, 0);

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 6)
            {
                npc.frame.Y = npc.frame.Y + (int)(frameHeight * npc.localAI[0]);

                if (npc.frame.Y == 3 * frameHeight)
                    npc.localAI[0] = -1;
                else if (npc.frame.Y == 0)
                    npc.localAI[0] = 1;

                npc.frameCounter = 0;
            }
        }
    }
}
