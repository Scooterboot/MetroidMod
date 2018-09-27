using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Evir : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = timer.
         * npc.ai[2] = projectile index.
         */
        internal readonly float shootSpeed = 10;
        internal readonly Vector2 projectilePosition = new Vector2(16, 35);
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 34;

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
            npc.TargetClosest();

            if(npc.ai[0] == 0)
            {
                if (npc.ai[1]++ >= 120)
                {
                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                    npc.ai[2] = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("EvirProjectile"), npc.damage, .3F, Main.myPlayer, npc.whoAmI);
                    Main.projectile[(int)npc.ai[2]].scale = npc.scale;
                }
            }
            else
            {
                Projectile projectile = Main.projectile[(int)npc.ai[2]];
                projectile.position = npc.position + (projectilePosition * npc.scale) - new Vector2(npc.direction == -1 ? projectile.width+2 : -2, projectile.height / 2);

                if(npc.ai[1]++ >= 60)
                {
                    Vector2 shootDirection = Vector2.Normalize(Main.player[npc.target].Center - projectile.Center) * shootSpeed;
                    projectile.velocity = shootDirection;

                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                    npc.ai[2] = -1;
                }
            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 12)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (4 * frameHeight);
                npc.frameCounter = 0;
            }

            npc.spriteDirection = -npc.direction;
        }
    }
}
