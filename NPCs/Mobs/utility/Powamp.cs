using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.utility
{
    public class Powamp : MNPC
    {
        /*
         * npc.ai[0] = player proximity timer.
         * npc.ai[1] = npc x velocity (cos)
         * npc.ai[2] = npc y velocity (sin)
         */
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.width = 14; npc.height = 36;

            /* Temporary NPC values */
            npc.scale = 1.5F;
            npc.damage = 0;
            npc.lifeMax = 1;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            npc.TargetClosest(false);
            if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= 240)
                npc.ai[0] += (npc.ai[0] < 46 ? 1 : 0);
            else
                npc.ai[0] -= (npc.ai[0] > 0 ? 1 : 0);

            npc.ai[1] += .06F;
            npc.velocity.X = (float)Math.Cos(npc.ai[1]) * .4F;

            npc.ai[2] += .03F;
            npc.velocity.Y = (float)Math.Sin(npc.ai[2]) * .3F;
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[0] <= 20)
            {
                if (npc.frameCounter++ >= 15)
                {
                    npc.frame.Y = (npc.frame.Y + frameHeight) % (3 * frameHeight);
                    npc.frameCounter = 0;
                }
            }
            else if (npc.ai[0] > 20 && npc.ai[0] <= 35)
                npc.frame.Y = 3 * frameHeight;
            else
            {
                if (npc.frameCounter++ >= 15)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    if (npc.frame.Y >= 7 * frameHeight)
                        npc.frame.Y = 4 * frameHeight;
                    npc.frameCounter = 0;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Main.PlaySound(SoundID.Item62, npc.position);
                for (int i = 0; i < 30; i++)
                {
                    int num727 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Dust dust = Main.dust[num727];
                    dust.velocity *= 1.4f;
                }
                for (int i = 0; i < 20; i++)
                {
                    int num729 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 3.5f);
                    Main.dust[num729].noGravity = true;
                    Dust dust = Main.dust[num729];
                    dust.velocity *= 7f;
                    num729 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    dust = Main.dust[num729];
                    dust.velocity *= 3f;
                }
            }
        }
    }
}
