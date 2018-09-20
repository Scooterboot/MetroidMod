using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Skultera : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 9;
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 28;

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

        public override void AI()
        {
            if (npc.direction == 0)
                npc.TargetClosest();

            if(npc.wet)
            {
                bool followPlayer = false;
                npc.TargetClosest(false);
                if (Main.player[npc.target].wet && !Main.player[npc.target].dead)
                    followPlayer = true;

                if(followPlayer) // Follow target.
                {
                    npc.TargetClosest(true);
                    if (npc.direction != npc.oldDirection)
                        npc.ai[1]++;

                    npc.velocity.X += npc.direction * .15F;
                    npc.velocity.Y += npc.directionY * .15F;

                    npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -5, 5);
                    npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -3, 3);
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

                    if (Main.tile[npcTilePosX, npcTilePosY - 1] == null)
                        Main.tile[npcTilePosX, npcTilePosY] = new Tile();
                    if (Main.tile[npcTilePosX, npcTilePosY + 1] == null)
                        Main.tile[npcTilePosX, npcTilePosY] = new Tile();
                    if (Main.tile[npcTilePosX, npcTilePosY + 2] == null)
                        Main.tile[npcTilePosX, npcTilePosY] = new Tile();

                    if (Main.tile[npcTilePosX, npcTilePosY - 1].liquid > 128) // If the tile below the NPC is at least half full of liquid.
                    {
                        if (Main.tile[npcTilePosX, npcTilePosY + 1].active() || Main.tile[npcTilePosX, npcTilePosY + 2].active())
                            npc.ai[0] = -1;
                    }
                }
            }
            else
            {
                if(npc.velocity.Y == 0)
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
            npc.rotation = MathHelper.Clamp(npc.rotation, -.2F, .2F);
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] > 0) // NPC is turning (animation).
            {
                npc.ai[1]++;
                if (npc.ai[1] < 3)
                    npc.frame.Y = 4 * frameHeight;
                else if (npc.ai[1] < 6)
                    npc.frame.Y = 5 * frameHeight;
                else if (npc.ai[1] < 9)
                    npc.frame.Y = 6 * frameHeight;
                else if (npc.ai[1] < 12)
                    npc.frame.Y = 7 * frameHeight;
                else if (npc.ai[1] < 15)
                    npc.frame.Y = 8 * frameHeight;
                else
                {
                    npc.ai[1] = 0;
                    npc.frame.Y = 0;
                    npc.spriteDirection = npc.direction;
                }
            }
            else
            {
                if (npc.frameCounter++ >= 6) // NPC is swimming.
                {
                    npc.frame.Y = (npc.frame.Y + frameHeight) % (3 * frameHeight);
                    npc.frameCounter = 0;
                }
                npc.spriteDirection = npc.direction;
            }
        }
    }
}
