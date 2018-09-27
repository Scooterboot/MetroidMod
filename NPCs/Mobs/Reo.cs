using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Reo : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = timer.
         * npc.ai[2] = dash cooldown.
         */
        internal readonly float speed = 3.5F;
        internal readonly float acceleration = .08F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 26; npc.height = 18;

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
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
                npc.TargetClosest();

            Player target = Main.player[npc.target];
            
            float tempSpeed = speed * (npc.ai[1] > 0 ? 2.5F : 1);
            float tempAcceleration = acceleration * (npc.ai[1] > 0 ? 2 : 1);

            Vector2 targetVelocity = Vector2.Normalize(target.Center - npc.Center) * tempSpeed;

            if (npc.velocity.X < targetVelocity.X)
            {
                npc.velocity.X += tempAcceleration;
                if (npc.velocity.X < 0)
                    npc.velocity.X *= .98F;
            }
            else if (npc.velocity.X > targetVelocity.X)
            {
                npc.velocity.X -= tempAcceleration;
                if (npc.velocity.X > 0)
                    npc.velocity.X *= .98F;
            }

            if (npc.velocity.Y < targetVelocity.Y)
            {
                npc.velocity.Y += tempAcceleration;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y *= .98F;
            }
            else if (npc.velocity.Y > targetVelocity.Y)
            {
                npc.velocity.Y -= tempAcceleration;
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

            if(npc.ai[2] <= 0 && Vector2.Distance(target.Center, npc.Center) <= 200)
            {
                npc.ai[1] = 120;
                npc.ai[2] = 300;
            }

            if (npc.ai[1] > 0)
                npc.ai[1]--;
            if (npc.ai[2] > 0)
                npc.ai[2]--;

            // Net update.
            if (((npc.velocity.X > 0 && npc.oldVelocity.X < 0) || (npc.velocity.X < 0 && npc.oldVelocity.X > 0) || (npc.velocity.Y > 0 && npc.oldVelocity.Y < 0) || (npc.velocity.Y < 0 && npc.oldVelocity.Y > 0)) && !npc.justHit)
                npc.netUpdate = true;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[1] <= 0)
            {
                if (npc.frameCounter++ >= 6)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    if (npc.frame.Y >= 3 * frameHeight)
                        npc.frame.Y = 0;
                    npc.frameCounter = 0;
                }
            }
            else
            {
                if (npc.frameCounter++ >= 6)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    if (npc.frame.Y >= 5 * frameHeight)
                        npc.frame.Y = 3 * frameHeight;
                    npc.frameCounter = 0;
                }
            }

            npc.rotation = (npc.ai[1] > 0 ? -npc.velocity.X : npc.velocity.X) * .2F;
        }
    }
}
