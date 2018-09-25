using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Owtch : ModNPC
    {
        internal readonly float idleSpeed = .4F;
        internal readonly float targetSpeed = .9F;
        internal readonly float acceleration = .04F;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 16; npc.height = 18;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = .4F;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            npc.TargetClosest(false);

            if(npc.velocity.Y == 0)
            {
                Player target = Main.player[npc.target];
                if(Math.Abs(target.position.Y - npc.position.Y) <= 32)
                {
                    if (target.Center.X < npc.Center.X)
                        npc.direction = -1;
                    else
                        npc.direction = 1;

                    if (npc.direction == 1)
                    {
                        if (npc.velocity.X < targetSpeed)
                            npc.velocity.X += acceleration;
                    }
                    else
                    {
                        if (npc.velocity.X > -targetSpeed)
                            npc.velocity.X -= acceleration;
                    }
                }
                else
                {
                    if(npc.direction == 1)
                    {
                        if (npc.velocity.X < idleSpeed)
                            npc.velocity.X += acceleration;
                        else
                            npc.velocity.X = idleSpeed;
                    }
                    else
                    {
                        if (npc.velocity.X > -idleSpeed)
                            npc.velocity.X -= acceleration;
                        else
                            npc.velocity.X = -idleSpeed;
                    }

                    if (npc.collideX)
                    {
                        npc.direction *= -1;
                        npc.velocity.X = 0;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 4)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }
        }
    }
}
