using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Beetom : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = animation timer.
         * npc.ai[2] = latched player index.
         */
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14;
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 32;
            
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.HitSound = SoundID.NPCHit1;
        }

        public override bool PreAI()
        {
            if(npc.ai[0] == 0) // Grounded state.
            {
                npc.TargetClosest(true);

                npc.velocity.X = 0;                

                if(npc.ai[1]++ >= 90)
                {
                    npc.velocity.Y = -8;
                    npc.velocity.X = Main.rand.Next(3, 8) * npc.direction;

                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                }
            }
            else if(npc.ai[0] == 1) // Aired state.
            {
                if (npc.collideY && npc.velocity.Y > 0)
                    npc.ai[0] = 0;

                if (npc.collideX)
                    npc.velocity.X = -npc.velocity.X * .8F;
            }
            else if(npc.ai[0] == 2) // Latched state.
            {
                npc.Center = Main.player[(int)npc.ai[2]].Center;
            }

            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            npc.ai[2] = target.whoAmI;
            npc.ai[0] = 2;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.visualOffset = new Vector2(0, -2); // Temporary.
            if (npc.ai[0] == 0)
            {
                if(npc.ai[1] < 8) // Just landed, start land animation. Very ugly, I know :S
                {
                    if (npc.ai[1] < 4)
                        npc.frame.Y = 8 * frameHeight;
                    else if (npc.ai[1] <= 8)
                        npc.frame.Y = 7 * frameHeight;
                }
                else if(npc.ai[1] >= 82) // Close to jumping, start jump animation. Also very ugly. Refactor?
                {
                    if (npc.ai[1] < 86)
                        npc.frame.Y = 7 * frameHeight;
                    else if (npc.ai[1] <= 90)
                        npc.frame.Y = 8 * frameHeight;
                }
                else if (npc.frameCounter++ >= 4)
                {
                    npc.frame.Y = (npc.frame.Y + frameHeight) % (4 * frameHeight);
                    npc.frameCounter = 0;
                }
            }
            else if(npc.ai[0] == 1)
            {
                npc.frame.Y = 9 * frameHeight;

                npc.spriteDirection = npc.direction;
            }
            else if(npc.ai[0] == 2)
            {
                if (npc.frameCounter++ >= 4)
                {
                    npc.frame.Y += frameHeight;
                    if (npc.frame.Y >= 14 * frameHeight)
                        npc.frame.Y = 10 * frameHeight;
                    npc.frameCounter = 0;
                }
            }
        }
    }
}
