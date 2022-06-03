using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Flying
{
    public class Reo : MNPC
    {
        /*
         * NPC.ai[0] = state manager.
         * NPC.ai[1] = timer.
         * NPC.ai[2] = dash cooldown.
         */
        internal readonly float speed = 3.5F;
        internal readonly float acceleration = .08F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.width = 26; NPC.height = 18;

            /* Temporary NPC values */
            NPC.scale = 2;
            NPC.damage = 15;
            NPC.defense = 5;
            NPC.lifeMax = 150;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0;

            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
                NPC.TargetClosest();

            Player target = Main.player[NPC.target];
            
            float tempSpeed = speed * (NPC.ai[1] > 0 ? 2.5F : 1);
            float tempAcceleration = acceleration * (NPC.ai[1] > 0 ? 2 : 1);

            Vector2 targetVelocity = Vector2.Normalize(target.Center - NPC.Center) * tempSpeed;

            if (NPC.velocity.X < targetVelocity.X)
            {
                NPC.velocity.X += tempAcceleration;
                if (NPC.velocity.X < 0)
                    NPC.velocity.X *= .98F;
            }
            else if (NPC.velocity.X > targetVelocity.X)
            {
                NPC.velocity.X -= tempAcceleration;
                if (NPC.velocity.X > 0)
                    NPC.velocity.X *= .98F;
            }

            if (NPC.velocity.Y < targetVelocity.Y)
            {
                NPC.velocity.Y += tempAcceleration;
                if (NPC.velocity.Y < 0)
                    NPC.velocity.Y *= .98F;
            }
            else if (NPC.velocity.Y > targetVelocity.Y)
            {
                NPC.velocity.Y -= tempAcceleration;
                if (NPC.velocity.Y > 0)
                    NPC.velocity.Y *= .98F;
            }

            if (NPC.collideX)
            {
                NPC.netUpdate = true;
                NPC.velocity.X = NPC.oldVelocity.X * -.7F;
            }
            if (NPC.collideY)
            {
                NPC.netUpdate = true;
                NPC.velocity.Y = NPC.oldVelocity.Y * -.7F;
            }

            if(NPC.ai[2] <= 0 && Vector2.Distance(target.Center, NPC.Center) <= 200)
            {
                NPC.ai[1] = 120;
                NPC.ai[2] = 300;
            }

            if (NPC.ai[1] > 0)
                NPC.ai[1]--;
            if (NPC.ai[2] > 0)
                NPC.ai[2]--;

            // Net update.
            if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) || (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) && !NPC.justHit)
                NPC.netUpdate = true;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[1] <= 0)
            {
                if (NPC.frameCounter++ >= 6)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    if (NPC.frame.Y >= 3 * frameHeight)
                        NPC.frame.Y = 0;
                    NPC.frameCounter = 0;
                }
            }
            else
            {
                if (NPC.frameCounter++ >= 6)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    if (NPC.frame.Y >= 5 * frameHeight)
                        NPC.frame.Y = 3 * frameHeight;
                    NPC.frameCounter = 0;
                }
            }

            NPC.rotation = (NPC.ai[1] > 0 ? -NPC.velocity.X : NPC.velocity.X) * .2F;
        }
    }
}
