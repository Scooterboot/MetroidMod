using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class SpacePirate : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * 
         */
        internal readonly float speed = 3;
        internal readonly float acceleration = .5F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 16;
        }
        public override void SetDefaults()
        {
            npc.width = 18; npc.height = 60;

            /* Temporary NPC values */
            npc.scale = 1;
            npc.damage = 25;
            npc.defense = 5;
            npc.lifeMax = 1000;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if (npc.ai[0] == 0) // Grounded/movement.
            {
                bool idle = false;
                if (npc.velocity.X == 0)
                    idle = true;
                if (npc.justHit)
                    idle = false;

                bool moving = false;

                // If NPC is grounded and moving.
                if (npc.velocity.Y == 0 && (npc.velocity.X > 0 && npc.direction < 0) || (npc.velocity.X < 0 && npc.direction > 0))
                    moving = true;

                npc.TargetClosest(true);

                if (npc.velocity.X < -speed || npc.velocity.X > speed)
                {
                    if (npc.velocity.Y == 0)
                    {
                        npc.velocity.X *= .8F;
                        npc.velocity.Y *= .8F;
                    }
                }
                else if (npc.velocity.X < speed && npc.direction == 1)
                {
                    npc.velocity.X += acceleration;
                    if (npc.velocity.X > speed)
                        npc.velocity.X = speed;
                }
                else if (npc.velocity.X > -speed && npc.direction == -1)
                {
                    npc.velocity.X -= acceleration;
                    if (npc.velocity.X < -speed)
                        npc.velocity.X = -speed;
                }

                bool grounded = false;

                if(npc.velocity.Y == 0) // Grounded.
                {
                    int npcTileXLeft = (int)npc.position.X / 16;
                    int npcTileXRight = (int)(npc.position.X + npc.width) / 16;
                    int npcTileY = (int)(npc.position.Y + npc.height + 7) / 16;

                    for(int i = npcTileXLeft; i < npcTileXRight; ++i)
                    {
                        if(Main.tile[i, npcTileY] == null) return false;
                        if(Main.tile[i, npcTileY].nactive() && Main.tileSolid[Main.tile[i, npcTileY].type])
                        {
                            grounded = true;
                            break;
                        }
                    }
                }

                if(npc.velocity.Y >= 0) // 'Falling' check for sloped tiles.
                {
                    int dir = npc.velocity.X == 0 ? 0 : npc.velocity.X > 0 ? 1 : -1;
                    float posX = npc.position.X + npc.velocity.X;
                    float posY = npc.position.Y;

                    int tileX = (int)(posX + (npc.width / 2) + (npc.width / 2 + 1) * dir) / 16;
                    int tileY = (int)(posY + npc.height - 1) / 16;

                    // Tile null check failsafe.
                    for(int y = tileY-3; y < tileY+1; ++y)
                        if (Main.tile[tileX, y] == null)
                    if (Main.tile[tileX - dir, tileY - 3] == null)
                        Main.tile[tileX - dir, tileY - 3] = new Tile();

                    // Gruesome if statement incomming, please refactor if you can, it drives me nuts.
                    if ((tileX * 16) < posX + npc.width && (tileX * 16 + 16) > posX && (Main.tile[tileX, tileY].nactive() && !Main.tile[tileX, tileY].topSlope() && (!Main.tile[tileX, tileY - 1].topSlope() && Main.tileSolid[(int)Main.tile[tileX, tileY].type]) && !Main.tileSolidTop[(int)Main.tile[tileX, tileY].type] || Main.tile[tileX, tileY - 1].halfBrick() && Main.tile[tileX, tileY - 1].nactive()) && (!Main.tile[tileX, tileY - 1].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 1].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 1].type] || Main.tile[tileX, tileY - 1].halfBrick() && (!Main.tile[tileX, tileY - 4].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 4].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 4].type])) && ((!Main.tile[tileX, tileY - 2].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 2].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 2].type]) &&
                        (!Main.tile[tileX, tileY - 3].nactive() ||
                        !Main.tileSolid[(int)Main.tile[tileX, tileY - 3].type] ||
                        Main.tileSolidTop[(int)Main.tile[tileX, tileY - 3].type]) &&
                        (!Main.tile[tileX - dir, tileY - 3].nactive() ||
                        !Main.tileSolid[(int)Main.tile[tileX - dir, tileY - 3].type])))
                    {
                        float y = tileY * 16;
                        if (Main.tile[tileX, tileY].halfBrick())
                            y += 8;
                        if (Main.tile[tileX, tileY - 1].halfBrick())
                            y -= 8;

                        float yFinalPos = posY + npc.height - y;
                        if (y < posY + npc.height && yFinalPos <= 16.1F)
                        {
                            npc.gfxOffY += npc.position.Y + npc.height - y;
                            npc.position.Y = y - npc.height;
                            npc.stepSpeed = yFinalPos >= 9.0F ? 2 : 1;
                            grounded = true;
                        }
                    }
                }

                if (grounded)
                {
                    int tileX = (int)(npc.position.X + (npc.width / 2) + (15 * npc.direction)) / 16;
                    int tileY = (int)(npc.position.Y + npc.height - 15) / 16;

                    for (int x = tileX - 1; x < tileX + 1; ++x)
                        for (int y = tileY - 3; y < tileY + 1; y++)
                            if (Main.tile[x, y] == null)
                                Main.tile[x, y] = new Tile();

                    Main.tile[tileX, tileY + 1].halfBrick();

                    if (npc.velocity.X < 0 && npc.direction == -1 || npc.velocity.X > 0 && npc.direction == 1)
                    {
                        // Jump required, determine jump height.
                        if (npc.height >= 32 && Main.tile[tileX, tileY - 2].nactive() && Main.tileSolid[Main.tile[tileX, tileY-2].type])
                        {
                            if (Main.tile[tileX, tileY - 3].nactive() && Main.tileSolid[Main.tile[tileX, tileY - 3].type])
                                npc.velocity.Y = -8;
                            else
                                npc.velocity.Y = -7;
                            
                            npc.netUpdate = true;
                        }
                        else if(Main.tile[tileX, tileY-1].nactive() && Main.tileSolid[Main.tile[tileX, tileY - 1].type])
                        {
                            npc.velocity.Y = -6;
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + npc.height - (tileY * 16) > 20 && Main.tile[tileX, tileY].nactive() && (!Main.tile[tileX, tileY].topSlope() && Main.tileSolid[(int)Main.tile[tileX, tileY].type]))
                        {
                            npc.velocity.Y = -5;
                            npc.netUpdate = true;
                        }
                        else if (npc.directionY < 0 && (!Main.tile[tileX, tileY + 1].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY + 1].type]) && (!Main.tile[tileX + npc.direction, tileY + 1].nactive() || !Main.tileSolid[(int)Main.tile[tileX + npc.direction, tileY + 1].type]))
                        {
                            npc.velocity.Y = -8;
                            npc.velocity.X = npc.velocity.X * 1.5F;
                            npc.netUpdate = true;
                        }

                        // Rework the following a bit. If the NPCs position has been static for a little while, jump.
                        //if (npc.velocity.Y == 0 && idle)
                        //    npc.velocity.Y = -5;
                    }

                    // If no jump has been initiated yet.
                    if(npc.velocity.Y == 0)
                    {
                        // TEMPORARY.
                        if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                        {
                            if (Main.rand.Next(45) == 0 && Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= 180)
                            {
                                npc.ai[0] = 3;
                            }
                            else if (Main.rand.Next(45) == 0 && Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= 140)
                            {
                                npc.ai[0] = 2;
                            }
                        }
                    }
                }

                npc.ai[1] = grounded ? 1 : 0;
            }
            if(npc.ai[0] == 1) // Shooting. Do we even want this?
            {

            }
            if(npc.ai[0] == 2) // Dropkick.
            {
                // Start the dropkick off with a small jump (almost) straight up.
                if(npc.ai[2] == 0)
                {
                    if (npc.ai[1] == 0)
                    {
                        npc.velocity.Y = -10;
                        npc.velocity.X *= 0;
                    }
                    npc.ai[1]++;
                    npc.velocity.Y *= .98F;

                    if (npc.ai[1] >= 40 || npc.velocity.Y >= 0)
                    {
                        npc.TargetClosest();
                        Vector2 newVelocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 7;
                        npc.velocity = newVelocity;

                        npc.ai[1] = 0;
                        npc.ai[2] = 1;
                    }
                }
                else
                {
                    if(npc.velocity.Y == 0)
                    {
                        npc.velocity.X = 0;
                        if(npc.ai[1] == 0)
                        {
                            // projectile spawning?
                            for(int i = 0; i < 4; ++i)
                            {
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-40, 41) * .1F, -5), mod.ProjectileType("SkreeRock"), npc.damage, 1.2F);
                            }
                        }
                        if(npc.ai[1]++ >= 40)
                        {
                            npc.ai[0] = 0;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                        }
                    }
                }
            }
            if(npc.ai[0] == 3) // Overhead jump + projectiles.
            {
                // This part is... Slightly hacky to make animations smooth. Feel free to refactor.
                npc.ai[1]++;
                if (npc.ai[1] <= 12)
                {
                    if (npc.velocity.X < -1 || npc.velocity.X > 1)
                        npc.velocity.X *= .95F;
                    if (npc.ai[1] == 12)
                    {
                        npc.velocity.Y = -9;
                        npc.velocity.X = 8 * npc.direction;
                    }
                }
                else if(npc.ai[1] >= 13)
                {
                    npc.rotation += .5F * npc.direction;

                    if (npc.ai[2]++ >= 20 && Main.netMode != 1)
                    {
                        Vector2 projVelocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 6;

                        Projectile.NewProjectile(npc.Center, projVelocity, mod.ProjectileType("SpacePirateClaw"), npc.damage, 1, Main.myPlayer);

                        npc.ai[2] = 0;
                    }

                    if (npc.velocity.Y == 0)
                    {
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                    }
                }
            }
            else
                npc.rotation = 0;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if(npc.ai[0] == 0)
            {
                if (npc.velocity.Y != 0)
                    npc.frame.Y = 14 * frameHeight;
                else
                {
                    npc.frameCounter += Math.Abs(npc.velocity.X * .45F);
                    if (npc.frameCounter >= 6) { 
                        npc.frame.Y = (npc.frame.Y + frameHeight) % (4 * frameHeight);
                        npc.frameCounter = 0;
                    }
                }
            }
            else if(npc.ai[0] == 1)
            {

            }
            else if(npc.ai[0] == 2)
            {
                if (npc.ai[2] == 0)
                    npc.frame.Y = 14 * frameHeight;
                else
                    npc.frame.Y = 15 * frameHeight;
            }
            else if(npc.ai[0] == 3)
            {
                if (npc.ai[1] < 4)
                    npc.frame.Y = 9 * frameHeight;
                else if (npc.ai[1] < 8)
                    npc.frame.Y = 10 * frameHeight;
                else if (npc.ai[1] < 12)
                    npc.frame.Y = 11 * frameHeight;
                else
                    npc.frame.Y = 12 * frameHeight;
            }

            npc.spriteDirection = npc.direction;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            for(int i = 0; i < 5; ++i)
            {
                Vector2 drawPos = npc.oldPos[i] + new Vector2(npc.width / 2, npc.height / 2) - Main.screenPosition;
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, npc.frame, Color.White * (.9F - .1F * i));
            }
            return true;
        }
    }
}
