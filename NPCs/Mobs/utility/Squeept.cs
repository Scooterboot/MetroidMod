using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.utility
{
    public class Squeept : MNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = jump timer + animation helper.
         * npc.ai[2] = 'landing' animation helper.
         */
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 9;
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 32;

            /* Temporary NPC values */
            npc.scale = 1.2F;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if(npc.ai[0] == 0) // Idle/grounded phase.
            {
                if (npc.localAI[2] > 0) npc.localAI[2]--;

                if(npc.ai[1]++ >= 60)
                {
                    npc.velocity.Y = -Main.rand.Next(9, 15);

                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
					npc.localAI[1] = 0;
					npc.localAI[2] = 0;
                    npc.netUpdate = true;
                }
            }
            else // Jumping/falling phase.
            {
                if (npc.velocity.Y >= 0)
                    npc.localAI[1]++;

                if(npc.collideY && npc.oldVelocity.Y >= 0)
                {
                    npc.velocity.Y = 0;

                    npc.ai[0] = 0;
					npc.localAI[1] = 0;
					npc.localAI[2] = 8;
                    npc.netUpdate = true;
                }
            }

            npc.velocity.X = 0;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if(npc.ai[0] == 0)
            {
                if (npc.localAI[2] > 0)
                {
                    if (npc.localAI[2] > 4)
                        npc.frame.Y = 6 * frameHeight;
                    else
                        npc.frame.Y = 5 * frameHeight;
                }
                else
                {
                    if (npc.ai[1] < 50)
                        npc.frame.Y = 4 * frameHeight;
                    else if (npc.ai[1] < 55)
                        npc.frame.Y = 3 * frameHeight;
                    else
                        npc.frame.Y = frameHeight;
                }
            }
            else
            {
                if(npc.velocity.Y < 0)
                {
                    if (npc.velocity.Y < -5)
                        npc.frame.Y = 2 * frameHeight;
                    else
                        npc.frame.Y = 0;
                }
                else
                {
                    if (npc.localAI[1] < 3)
                        npc.frame.Y = 3 * frameHeight;
                    else if (npc.localAI[1] < 6)
                        npc.frame.Y = 4 * frameHeight;
                    else if (npc.localAI[1] < 9)
                        npc.frame.Y = 5 * frameHeight;
                    else if (npc.localAI[1] < 12)
                        npc.frame.Y = 6 * frameHeight;
                    else
                    {
                        if(npc.frameCounter++ % 5 == 0)
                        {
                            npc.frame.Y += frameHeight;
                            if (npc.frame.Y >= 9 * frameHeight)
                                npc.frame.Y = 7 * frameHeight;
                        }
                    }
                }
            }
        }
    }
}
