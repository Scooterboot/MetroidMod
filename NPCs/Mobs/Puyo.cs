﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Puyo : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 9;            
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 8;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = .2F;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public bool spawn = false;
        public override bool PreAI()
        {
            if (!spawn)
            {
                npc.scale = (Main.rand.Next(13, 21) * 0.1f);
                npc.defense = (int)((float)npc.defense * npc.scale);
                npc.damage = (int)((float)npc.damage * npc.scale);
                npc.life = (int)((float)npc.life * npc.scale);
                npc.lifeMax = npc.life;
                npc.value = (float)((int)(npc.value * npc.scale));
                npc.npcSlots *= npc.scale;
                npc.knockBackResist *= 2f - npc.scale;

                npc.frame.Width = 30;
                npc.frame.X = Main.rand.Next(0, 3) * npc.frame.Width;

                spawn = true;
            }
            return true;
        }

        public override void AI()
        {
            if (npc.velocity.Y == 0)
            {
                npc.TargetClosest();

                npc.ai[0]++;
                if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= 80)
                    npc.ai[0]++;

                if(npc.ai[0] >= 120)
                {
                    npc.velocity.X = Main.rand.Next(4, 8) * npc.direction;
                    npc.velocity.Y = Main.rand.Next(-7, -3);

                    npc.ai[0] = 0;
                    npc.ai[1] = Main.rand.Next(20, 61); // Next jump time.
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.visualOffset = new Vector2(60, 0);
            if (npc.velocity.Y == 0)
            {
                npc.frame.Y = (int)((Math.Round(npc.ai[0] / 10) % 3) * frameHeight);

                npc.frameCounter = 0;
            }
            else
            {
                npc.frameCounter++;
                if (npc.frameCounter < 5)
                    npc.frame.Y = 4 * frameHeight;
                else if (npc.frameCounter < 10)
                    npc.frame.Y = 5 * frameHeight;
                else if (npc.frameCounter < 15)
                    npc.frame.Y = 6 * frameHeight;
            }
        }
    }
}