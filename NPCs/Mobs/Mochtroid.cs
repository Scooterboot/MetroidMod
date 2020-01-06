using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Mochtroid : ModNPC
    {
        /*
         * npc.ai[0] = animation manager (pingpong).
         */ 

        internal readonly float speed = 3.5F;
        internal readonly float acceleration = .04F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 28; npc.height = 20;

            /* Temporary NPC values */
            npc.scale = 1.5F;
            npc.damage = 0;
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
                npc.ai[0] = 1;

            npc.TargetClosest();

            Vector2 targetVelocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * speed;

            if(npc.velocity.X < targetVelocity.X)
            {
                npc.velocity.X += acceleration;
                if (npc.velocity.X < 0)
                    npc.velocity.X *= .98F;
            }
            else if(npc.velocity.X > targetVelocity.X)
            {
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > 0)
                    npc.velocity.X *= .98F;
            }

            if (npc.velocity.Y < targetVelocity.Y)
            {
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y *= .98F;
            }
            else if (npc.velocity.Y > targetVelocity.Y)
            {
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > 0)
                    npc.velocity.Y *= .98F;
            }

            if (npc.collideX)
            {
                npc.netUpdate = true;
                npc.velocity.X = npc.oldVelocity.X * -.7F;
            }
            if (npc.collideY)
            {
                npc.netUpdate = true;
                npc.velocity.Y = npc.oldVelocity.Y * -.7F;
            }

            if (((npc.velocity.X > 0 && npc.oldVelocity.X < 0) || (npc.velocity.X < 0 && npc.oldVelocity.X > 0) || (npc.velocity.Y > 0 && npc.oldVelocity.Y < 0) || (npc.velocity.Y < 0 && npc.oldVelocity.Y > 0)) && !npc.justHit)
                npc.netUpdate = true;

            Rectangle collisionRect = new Rectangle(
                (int)npc.position.X - 32, (int)npc.position.Y - 32, npc.width + 64, npc.height + 64);
            // Proximity damage.
            for(int i = 0; i < 255; ++i)
            {
                if (!Main.player[i].active || Main.player[i].dead) continue;

                if(Main.player[i].getRect().Intersects(collisionRect) &&
                    Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("could not get away"), 5, 0, false, false, false, 0) != 0)
                {
                    Main.player[i].hurtCooldowns[0] = 15;
                }
            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 6)
            {
                npc.frame.Y = npc.frame.Y + (int)(frameHeight * npc.localAI[0]);

                if (npc.frame.Y == 2 * frameHeight)
                    npc.localAI[0] = -1;
                else if (npc.frame.Y == 0)
                    npc.localAI[0] = 1;

                npc.frameCounter = 0;
            }

            npc.rotation = npc.velocity.X * 0.2f;
        }
    }
}
