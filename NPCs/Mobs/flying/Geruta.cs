using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.flying
{
    public class Geruta : MNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = timer.
         */

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 8;
        }
        public override void SetDefaults()
        {
            npc.width = 42; npc.height = 23;

            /* Temporary NPC values */
            npc.scale = 1.5F;
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
            if(npc.ai[0] == 0)
            {
                npc.velocity = Vector2.Zero;

                if(npc.ai[1]++ >= 30) // Jump countdown.
                {
                    npc.TargetClosest();

                    Vector2 targetVelocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center);

                    npc.velocity.X = targetVelocity.X * 3;
                    npc.velocity.Y = Main.rand.Next(6, 10);

                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                }
            }
            else
            {
                if(npc.collideX) // If collide on the X axis, turn the X velocity around and cut it a bit.
                {
                    npc.velocity.X *= -.8F;
                    npc.netUpdate = true;
                }

                if(npc.collideY) // If collide on the Y axis, see if we've landed or should bounce back.
                {
                    if(npc.oldVelocity.Y > 0)
                        npc.velocity.Y = 0;
                    else
                    {
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;

                        npc.velocity *= 0;
                        npc.netUpdate = true;

                        return false;
                    }
                }

                // If we're 'falling' upwards, make sure to time the fall so the fall isn't endless.
                if(npc.velocity.Y < 0)
                {
                    if (npc.oldVelocity.Y >= 0)
                        npc.ai[2] = 0;
                    
                    if(npc.ai[1]++ >= 120)
                    {
                        npc.ai[0] = 1;
                        npc.ai[1] = 1;
                        npc.ai[2] = 0;

                        npc.velocity.Y = Main.rand.Next(6, 10);
                        npc.netUpdate = true;
                    }

                    npc.ai[2]++;
                }
                else
                    npc.ai[2]++;

                npc.velocity.Y -= .2F;
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            bool spawnDusts = false;
            if (npc.ai[0] == 0)
                npc.frame.Y = 0;
            else
            {
                if (npc.velocity.Y < 0)
                {
                    if (npc.ai[1] >= 102)
                    {
                        if (npc.ai[2] < 108)
                            npc.frame.Y = 4 * frameHeight;
                        else if (npc.ai[2] < 114)
                            npc.frame.Y = 5 * frameHeight;
                        else
                        {
                            npc.frame.Y = 6 * frameHeight;
                            spawnDusts = true;
                        }
                    }
                    else
                    {
                        if (npc.ai[2] < 6)
                            npc.frame.Y = 4 * frameHeight;
                        else if (npc.ai[2] < 12)
                            npc.frame.Y = 5 * frameHeight;
                        else
                        {
                            npc.frame.Y = 6 * frameHeight;
                            spawnDusts = true;
                        }
                    }
                }
                else
                {
                    if (npc.velocity.Y <= 2)
                        npc.frame.Y = frameHeight;
                    else
                    {
                        if (npc.ai[2] < 8)
                            npc.frame.Y = frameHeight;
                        else
                            npc.frame.Y = 2 * frameHeight;
                    }
                    spawnDusts = true;
                }
            }

            /* Dust spawning */
            if (!spawnDusts) return;

            Vector2[] dustSides = new Vector2[2] { new Vector2(10, 0), new Vector2(22, 0) };
            dustSides[0].Y = dustSides[1].Y = npc.frame.Y == 6 * frameHeight ? 16 : -4;        
            for(int i = 0; i < dustSides.Length; ++i)
            {
                for(int j = 0; j < 5; ++j)
                {
                    int newDust = Dust.NewDust(npc.position + dustSides[i] * npc.scale, (int)(6 * npc.scale), (int)(6 * npc.scale), DustID.Fire, 0, -npc.velocity.Y * .3F);
                    Main.dust[newDust].noGravity = true;
                }
            }
        }
    }
}
