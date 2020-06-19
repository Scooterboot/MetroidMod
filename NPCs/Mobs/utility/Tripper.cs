using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.utility
{
    public class Tripper : MNPC
    {
        internal readonly float speed = 2F;
        internal readonly float acceleration = .06F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 28; npc.height = 16;

            /* Temporary NPC values */
            npc.scale = 1.5F;
            npc.damage = 0;
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
                npc.direction = 1;

            float desiredXVelocity = npc.direction * speed;
            if (desiredXVelocity > 0)
            {
                if (npc.velocity.X < desiredXVelocity)
                    npc.velocity.X += acceleration;
                else if (npc.velocity.X > desiredXVelocity)
                    npc.velocity.X = desiredXVelocity;
            }
            else
            {
                if (npc.velocity.X > desiredXVelocity)
                    npc.velocity.X -= acceleration;
                else if (npc.velocity.X < desiredXVelocity)
                    npc.velocity.X = desiredXVelocity;
            }

            if (npc.collideX)
            {
                npc.velocity.X *= 0;
                npc.direction *= -1;
                npc.netUpdate = true;
            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.direction > 0)
            {
				if (npc.velocity.X < speed / 3)
					npc.frame.Y = 0;
				else if (npc.velocity.X < (speed / 3) * 2)
					npc.frame.Y = frameHeight;
				else
				{
					npc.frameCounter++;
					npc.frame.Y = frameHeight * ((npc.frameCounter % 20 < 10) ? 2 : 1);
				}
            }
            else
            {
                if (npc.velocity.X > -speed / 3)
                    npc.frame.Y = 0;
                else if (npc.velocity.X > (-speed / 3) * 2)
                    npc.frame.Y = frameHeight;
				else
				{
					npc.frameCounter++;
					npc.frame.Y = frameHeight * ((npc.frameCounter % 20 < 10) ? 2 : 1);
				}
			}

            npc.spriteDirection = -npc.direction;
        }
    }
}
