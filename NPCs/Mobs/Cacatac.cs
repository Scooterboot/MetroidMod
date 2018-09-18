using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Cacatac : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         *  
         */

        public bool spawn = false;
        private float speed = .75F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 11;
        }
        public override void SetDefaults()
        {
            /* Temporary NPC values */
            npc.lifeMax = 20;
            npc.width = 24; npc.height = 34;
            npc.damage = 20;

            npc.knockBackResist = 0;
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
            if(npc.ai[0] == 0) // Movement stage.
            {
                npc.TargetClosest(true);

                Player p = Main.player[npc.target];
                if (Vector2.Distance(p.position, npc.position) <= 32 &&
                    Collision.CanHit(npc.position, npc.width, npc.height, p.position, p.width, p.height) &&
                    npc.velocity.Y == 0)
                {
                    npc.ai[0] = 1; // Set projectile stage for NPC.
                    return;
                }

                npc.velocity.X = npc.direction * speed;
            }
            if(npc.ai[0] == 1) // Projectile stage.
            {
                npc.velocity = Vector2.Zero;                

                if(npc.ai[1]++ >= 60)
                {
                    // Fire projectiles.
                    for (int i = 0; i < 5; ++i)
                    {
                        float value = (float)(Math.PI - ((Math.PI / 4) * i));
                        Vector2 v2 = new Vector2((float)Math.Cos(value), -(float)Math.Sin(value));
                        Projectile.NewProjectile(npc.Center, v2 * 6, mod.ProjectileType("CacatacSpike"), npc.damage, 0, Main.LocalPlayer.whoAmI, 0);
                    }

                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                }
            }

            return;
        }

        public override void FindFrame(int frameHeight)
        {
            if(!npc.collideX && !npc.collideY) // NPC is aired.
            {
                npc.frame.Y = 7 * frameHeight; // Idle.
            }
            else if(npc.ai[0] == 0)
            {
                npc.frameCounter += Math.Abs(npc.velocity.X) / 4;

                npc.frameCounter %= (Main.npcFrameCount[npc.type] - 4); // If the frameCounter exceeds the amount of frames available, reset it to 0.
                int frame = (int)(npc.frameCounter);
                npc.frame.Y = frame * frameHeight;
            }
            else if(npc.ai[0] == 1)
            {

                if (npc.ai[1] < 20)
                    npc.frame.Y = 8 * frameHeight;
                else if (npc.ai[1] < 40)
                    npc.frame.Y = 9 * frameHeight;
                else if (npc.ai[1] < 60)
                    npc.frame.Y = 10 * frameHeight;
            }

            npc.spriteDirection = npc.direction;
        }
    }
}
