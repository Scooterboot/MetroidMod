using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.bug
{
    public class Kago : MNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 8; npc.height = 8;

            /* Temporary NPC values */
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = .2F;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }
        
        public bool spawn = false;
        public override bool PreAI()
        {
            if (!spawn)
            {
                npc.scale = (Main.rand.Next(15, 21) * 0.1f);
                npc.defense = (int)((float)npc.defense * npc.scale);
                npc.damage = (int)((float)npc.damage * npc.scale);
                npc.life = (int)((float)npc.life * npc.scale);
                npc.lifeMax = npc.life;
                npc.value = (float)((int)(npc.value * npc.scale));
                npc.npcSlots *= npc.scale;
                npc.knockBackResist *= 2f - npc.scale;
                npc.ai[1] = Main.rand.Next(20, 61);
                spawn = true;
            }
            return true;
        }

        public override void AI()
        {
            if (npc.velocity.Y == 0f)
            {
                npc.velocity = Vector2.Zero;
                if (npc.collideY && npc.oldVelocity.Y != 0f && Collision.SolidCollision(npc.position, npc.width, npc.height))
                    npc.position.X -= npc.velocity.X + (float)npc.direction;

                npc.ai[0]++;
                if(npc.ai[0] >= npc.ai[1])
                {
                    npc.TargetClosest();

                    npc.velocity.X = Main.rand.Next(2, 8) * npc.direction;
                    npc.velocity.Y = Main.rand.Next(-7, -3);

                    npc.ai[0] = 0;
                    npc.ai[1] = Main.rand.Next(20, 61); // Next jump time.
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if(npc.velocity.Y == 0)
            {
                // Just landed, revert to 'idle' frame.
                if(npc.frame.Y != 0)
                {
                    if (npc.frameCounter++ < 5)
                        npc.frame.Y = frameHeight;
                    else
                    {
                        npc.frame.Y = 0;
                        npc.frameCounter = 0;
                    }
                }
            }
            else
            {
                // Just aired, start launch animation
                if(npc.frame.Y != 2 * frameHeight)
                {
                    if (npc.frameCounter++ < 5)
                        npc.frame.Y = frameHeight;
                    else
                    {
                        npc.frame.Y = 2 * frameHeight;
                        npc.frameCounter = 0;
                    }
                }
            }
        }
    }
}
