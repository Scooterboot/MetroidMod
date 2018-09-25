using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Sova : ModNPC
    {
        internal readonly float speed = 1.5F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 22; npc.height = 18;

            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 20;
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;

            npc.noGravity = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                npc.TargetClosest();
                npc.directionY = 1;

                npc.ai[0] = 1;
                npc.ai[2] = 1;
            }

            if (npc.ai[1] == 0)
            {
                if (npc.collideY)
                    npc.ai[0] = 2;

                if (!npc.collideY && npc.ai[0] == 2)
                {
                    npc.direction = -npc.direction;
                    npc.ai[0] = npc.ai[1] = 1;
                }

                if (npc.collideX)
                {
                    npc.directionY = -npc.directionY;
                    npc.ai[1] = 1;
                }
            }
            else
            {
                if (npc.collideX)
                    npc.ai[0] = 2;

                if (!npc.collideX && npc.ai[0] == 2)
                {
                    npc.directionY = -npc.directionY;
                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                }

                if (npc.collideY)
                {
                    npc.direction = -npc.direction;
                    npc.ai[1] = 0;
                }
            }

            npc.velocity.X = npc.direction * speed;
            npc.velocity.Y = npc.directionY * speed;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 4)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }

            // Rotate the NPC correctly (visually).
            if (npc.direction == 1)
            {
                if (npc.directionY == 1)
                {
                    npc.rotation = -MathHelper.PiOver2;
                    npc.visualOffset = new Vector2(8, 0);
                }
                else
                {
                    npc.rotation = -(float)Math.PI;
                    npc.visualOffset = new Vector2(0, -6);
                }
            }
            else
            {
                if (npc.directionY == 1)
                {
                    npc.rotation = 0;
                    npc.visualOffset = new Vector2(0, 0);
                }
                else
                {
                    npc.rotation = MathHelper.PiOver2;
                    npc.visualOffset = new Vector2(-8, 0);
                }
            }

            npc.spriteDirection = -npc.direction;
        }
    }
}
