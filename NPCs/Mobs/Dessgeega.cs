using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Dessgeega : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 28;

            /* Temporary NPC values */
            npc.scale = 1.5F;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = .3F;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if (npc.ai[0] == 0) // Aired
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.ai[0] = 1;
                    npc.TargetClosest(true);
                }

                if (npc.velocity.X != 0f && Collision.SolidCollision(npc.position + new Vector2(npc.velocity.X, 0f), npc.width, npc.height) && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                    npc.velocity.X *= -1f;
            }
            else
            {
                npc.direction = Math.Sign(Main.player[npc.target].position.X - npc.position.X);
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity.X *= 0.1f;
                    if (Math.Abs(npc.velocity.X) < 1f)
                        npc.velocity.X = 0f;
                }

                if (npc.ai[1]++ > 40)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        npc.velocity.Y = -8f;
                        npc.velocity.X += 3 * npc.direction;
                    }
                    else
                    {
                        npc.velocity.Y = -6f;
                        npc.velocity.X += 2 * npc.direction;
                    }
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                }
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if(npc.velocity.Y != 0)
                npc.frame.Y = 2 * frameHeight;
            else
            {
                if(npc.frameCounter++ >= 5)
                {
                    npc.frame.Y = (npc.frame.Y + frameHeight) % (2 * frameHeight);
                    npc.frameCounter = 0;
                }
            }
        }
    }
}
