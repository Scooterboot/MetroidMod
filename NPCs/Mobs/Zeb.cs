using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Zeb : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 16; npc.height = 14;

            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.aiStyle = -1;
            npc.lifeMax = 150;
            npc.knockBackResist = 0;

            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
        }

        public override bool PreAI()
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
                npc.TargetClosest();

            float speedMultiplier = 6.5F;
            float speedBuildup = .051F;

            Vector2 npcCenter = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
            Vector2 targetVelocity = new Vector2(Main.player[npc.target].position.X + (Main.player[npc.target].width / 2),
                                               Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2));
            targetVelocity.X -= npcCenter.X;
            targetVelocity.Y -= npcCenter.Y;
            float magnitude = (float)Math.Sqrt(targetVelocity.X * targetVelocity.X + targetVelocity.Y * targetVelocity.Y);            
            magnitude = speedMultiplier / magnitude;

            targetVelocity.X *= magnitude;
            targetVelocity.Y *= magnitude;

            if (Main.player[npc.target].dead)
            {
                targetVelocity.X = npc.direction * speedMultiplier / 2;
                targetVelocity.Y = -speedMultiplier / 2;
            }

            if (npc.velocity.X < targetVelocity.X)
                npc.velocity.X += speedBuildup;
            else if (npc.velocity.X > targetVelocity.X)
                npc.velocity.X -= speedBuildup;

            if (npc.velocity.Y < targetVelocity.Y)
                npc.velocity.Y += speedBuildup;
            else if (npc.velocity.Y > targetVelocity.Y)
                npc.velocity.Y -= speedBuildup;
            
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

            if (npc.wet) // Wet flight handling.
            {
                if (npc.velocity.Y > 0)
                    npc.velocity.Y *= .95F;
                npc.velocity.Y -= .5F;
                if (npc.velocity.Y < -4)
                    npc.velocity.Y = -4;

                npc.TargetClosest();
            }

            if (((npc.velocity.X > 0 && npc.oldVelocity.X < 0) || (npc.velocity.X < 0 && npc.oldVelocity.X > 0) || (npc.velocity.Y > 0 && npc.oldVelocity.Y < 0) || (npc.velocity.Y < 0 && npc.oldVelocity.Y > 0)) && !npc.justHit)
                npc.netUpdate = true;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 4)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }

            // Visual NPC styling.
            if (npc.velocity.X > 0f)
                npc.spriteDirection = 1;
            if (npc.velocity.X < 0f)
                npc.spriteDirection = -1;
            npc.rotation = npc.velocity.X * 0.1f;
        }
    }
}
