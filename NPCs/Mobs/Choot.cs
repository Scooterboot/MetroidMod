using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Choot : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = ai timer.
         */
        public bool spawn = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 32; npc.height = 16;

            /* Temporary NPC values */
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
            if (!spawn)
            {
                npc.scale = (Main.rand.Next(13, 21) * 0.1f);
                npc.defense = (int)((float)npc.defense * npc.scale);
                npc.damage = (int)((float)npc.damage * npc.scale);
                npc.life = (int)((float)npc.life * npc.scale);
                npc.lifeMax = npc.life;
                npc.value = (float)((int)(npc.value * npc.scale));
                npc.npcSlots *= npc.scale;
                npc.knockBackResist *= 2f - npc.scale;
                spawn = true;
            }
            return true;
        }
        public override void AI()
        {
            if(npc.ai[0] == 0) // Dormant/idle ai.
            {
                npc.TargetClosest(false);
                npc.velocity = Vector2.Zero;

                if(Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= 64)
                {
                    npc.velocity.Y = -8;
                    npc.ai[0] = 1;
                }
            }
            else if(npc.ai[0] == 1) // Jump.
            {
                // Animation timer.
                npc.ai[1]++;

                npc.velocity.Y += 0.15F;
                //npc.velocity.Y *= 0.97F;
                if (npc.velocity.Y >= -1)
                {
                    npc.ai[0] = 2;
                    npc.ai[1] = 0;
                }
            }
            else if(npc.ai[0] == 2) // Downfall
            {
                // Animation timer.
                npc.ai[1]++;

                npc.velocity.Y = 1.5F;
                npc.ai[2] += 0.06F;

                npc.velocity.X = (float)Math.Cos(npc.ai[2]) * 3;

                if (npc.collideY) // Reset to idle state.
                {
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[0] == 0)
                npc.frame.Y = 0;
            else if (npc.ai[0] == 1)
            {
                if (npc.ai[1] < 10)
                    npc.frame.Y = frameHeight;
                else
                    npc.frame.Y = 2 * frameHeight;
            }
            else if (npc.ai[0] == 2)
            {
                if (npc.ai[1] < 10)
                    npc.frame.Y = 3 * frameHeight;
                else
                    npc.frame.Y = 4 * frameHeight;
            }
        }
    }
}
