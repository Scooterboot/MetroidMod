using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Bull : ModNPC
    {
        /*
         * npc.ai[0] = dashing logic. 
         */
         
        internal readonly float speed = 1.2F;
        internal readonly float dashSpeed = 6F;
        internal readonly float acceleration = .08F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }
        public override void SetDefaults()
        {
            npc.width = 24; npc.height = 30;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            npc.TargetClosest();

            Player p = Main.player[npc.target];

            // Float logic.
            Vector2 targetVelocity = Vector2.Normalize(p.Center - npc.Center) * speed;

            if (npc.velocity.X < targetVelocity.X)
                npc.velocity.X += acceleration;
            if (npc.velocity.X > targetVelocity.X)
                npc.velocity.X -= acceleration;

            if (npc.velocity.Y < targetVelocity.Y)
                npc.velocity.Y += acceleration;
            if (npc.velocity.Y > targetVelocity.Y)
                npc.velocity.Y -= acceleration;

            // Dash logic.
            if (Vector2.Distance(npc.Center, p.Center) <= 170)
            {
                if (npc.ai[0]++ >= 60)
                {
                    npc.velocity = targetVelocity * dashSpeed;
                    npc.ai[0] = 0;
                }
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 4)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (6 * frameHeight);
                npc.frameCounter = 0;
            }
        }
    }
}
