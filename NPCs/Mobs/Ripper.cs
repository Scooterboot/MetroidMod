using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Ripper : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = Y velocity sinus helper.
         */
        internal readonly float speed = 2.5F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 18; npc.height = 12;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 10;
            npc.lifeMax = 300;
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

            if (npc.GetGlobalNPC<MNPC>().froze)
            {
                npc.damage = 0;
                npc.position = npc.oldPosition;
            }
            else
            {
                npc.velocity.X = speed * npc.direction;

                if (npc.collideX)
                {
                    npc.velocity.X *= -1;
                    npc.netUpdate = true;
                }

                npc.velocity.Y = (float)Math.Sin(npc.ai[1] += .1F) * .5F;

                npc.direction = npc.velocity.X < 0 ? -1 : 1;
                npc.damage = npc.defDamage;
            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 6)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }

            npc.spriteDirection = -npc.direction;
        }
    }
}
