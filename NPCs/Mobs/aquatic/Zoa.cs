using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.aquatic
{
    public class Zoa : MNPC
    {
        /*
         * npc.ai[0] = idle/notarget velocity manager.
         * npc.ai[1] = dash timer.
         */

        internal readonly float dashSpeed = 8;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = npc.height = 16;

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
                npc.TargetClosest();

            if (npc.wet)
            {
                bool followPlayer = false;
                npc.TargetClosest(false);
                if (Main.player[npc.target].wet && !Main.player[npc.target].dead)
                    followPlayer = true;

                if (followPlayer) // Follow target.
                {
                    npc.TargetClosest(true);
                    
                    if(npc.ai[1]-- <= -30)
                    {
                        Vector2 dashVelocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * dashSpeed;
                        npc.velocity = dashVelocity;

                        // Setup dash dusts.
                        for(int i = 0; i < 8; ++i)
                        {
                            dashVelocity *= .5F;
                            int newDust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.BubbleBlock, -dashVelocity.X, -dashVelocity.Y);
                            Main.dust[newDust].noGravity = true;
                        }

                        npc.ai[1] = 50;
                    }

                    npc.velocity.X += npc.direction * .15F;
                    npc.velocity.Y += npc.directionY * .15F;

                    if (npc.ai[1] <= 0)
                    {
                        npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -5, 5);
                        npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -3, 3);
                    }
                }
                else // Idle/swim around; no target. 
                {
                    if (npc.collideX)
                    {
                        npc.velocity.X = -npc.velocity.X;
                        npc.direction *= -1;
                        npc.netUpdate = true;
                    }
                    if (npc.collideY)
                    {
                        npc.velocity.Y = -npc.velocity.Y;
                        npc.directionY = -npc.directionY;
                        npc.ai[0] = npc.direction;
                        npc.netUpdate = true;
                    }
                    npc.velocity.X += npc.direction * .1F;
                    if (npc.velocity.X < -1 || npc.velocity.X > 1)
                        npc.velocity.X *= .95F;

                    if (npc.ai[0] == -1)
                    {
                        npc.velocity.Y -= .01F;
                        if (npc.velocity.Y < -.3F)
                            npc.ai[0] = 1;
                    }
                    else
                    {
                        npc.velocity.Y += .01F;
                        if (npc.velocity.Y > .3F)
                            npc.ai[0] = -1;
                    }
                    if (npc.velocity.Y > .4F || npc.velocity.Y < -.4F)
                        npc.velocity.Y *= .95F;

                    /* Water check */
                    int npcTilePosX = (int)(npc.position.X + (npc.width / 2)) / 16;
                    int npcTilePosY = (int)(npc.position.Y + (npc.height / 2)) / 16;

                    for(int y = npcTilePosY - 1; y < npcTilePosY + 2; ++y)
                    {
                        if (Main.tile[npcTilePosX, y] == null)
                            Main.tile[npcTilePosX, y] = new Tile();
                    }

                    if (Main.tile[npcTilePosX, npcTilePosY - 1].liquid > 128) // If the tile below the NPC is at least half full of liquid.
                    {
                        if (Main.tile[npcTilePosX, npcTilePosY + 1].active() || Main.tile[npcTilePosX, npcTilePosY + 2].active())
                            npc.ai[0] = -1;
                    }
                }
            }
            else
            {
                if (npc.velocity.Y == 0)
                {
                    npc.velocity.X *= .94F;
                    if (npc.velocity.X > -.2F && npc.velocity.X < .2F)
                        npc.velocity.X = 0;
                }

                npc.velocity.Y += .3F;
                if (npc.velocity.Y > 10)
                    npc.velocity.Y = 10;
                npc.ai[0] = 1;
            }

            npc.rotation = npc.velocity.Y * npc.direction * .1F;
            npc.rotation = MathHelper.Clamp(npc.rotation, -.3F, .3F);
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] <= 0 && npc.frameCounter++ >= 4)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (3 * frameHeight);
                npc.frameCounter = 0;
            }
            else if (npc.ai[1] > 0)
                npc.frame.Y = 3 * frameHeight;

            npc.spriteDirection = -npc.direction;
        }
    }
}
