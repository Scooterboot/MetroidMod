using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.flying
{
    public class Waver : MNPC
    {
        /*
         * npc.ai[0] = Y turnaround timer.
         */
        internal readonly float xSpeed = 4;
		internal readonly float ySpeed = 3;

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 22; npc.height = 16;

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
            if (npc.direction == 0)
                npc.direction = 1;

            if (npc.collideX)
            {
                npc.direction *= -1;
                npc.netUpdate = true;
            }
            if(npc.collideY || npc.ai[0]++ >= 180)
            {
				npc.ai[0] = 0;

                npc.directionY *= -1;
                npc.netUpdate = true;
            }

            npc.velocity.X = npc.direction * xSpeed;
            npc.velocity.Y = npc.directionY * ySpeed;

            return (false);
        }

        public override void FindFrame(int frameHeight)
        {
			int frameCount;

			npc.frameCounter++;
			frameCount = Main.npcFrameCount[npc.type];
			if (npc.frameCounter >= 6)
			{
				if (npc.directionY == 1)
					npc.frame.Y = (npc.frame.Y - frameHeight < 0) ? (frameHeight * (frameCount - 1)) : (npc.frame.Y - frameHeight);
				else
					npc.frame.Y = (npc.frame.Y + frameHeight) % (frameHeight * frameCount);

				npc.frameCounter = 0;
			}
			npc.spriteDirection = -npc.direction;
        }
	}
}
