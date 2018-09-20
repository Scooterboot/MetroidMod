using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Metaree : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 26;
            npc.height = 28;

            /* Temporary NPC values */
            npc.scale = 1.5F;
            npc.damage = 36;
            npc.defense = 35;
            npc.lifeMax = 20;
            npc.knockBackResist = 0;

            npc.npcSlots = .5F;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            npc.noGravity = true;

            if (npc.ai[0] == 0) // NPC is dormant/idle.
            {
                npc.TargetClosest(true);

                Player p = Main.player[npc.target];
                // Position and collision checks to initiate divebomb attack.
                if (p.Center.Y > npc.Center.Y && p.position.X >= npc.position.X - 100 && p.position.X <= npc.position.X + 100 &&
                    Collision.CanHit(npc.position, npc.width, npc.height, p.position, p.width, p.height))
                    npc.ai[0] = 1;
            }

            if (npc.ai[0] == 1) // NPC is flying down (divebomb).
            {
                npc.TargetClosest(true);
                Player p = Main.player[npc.target];


                if (npc.position.X < p.position.X)
                {
                    if (npc.velocity.X < 0)
                        npc.velocity.X *= .98F;
                    npc.velocity.X += .3F;
                }
                else if (npc.position.X > p.position.X)
                {
                    if (npc.velocity.X > 0)
                        npc.velocity.X *= .98F;
                    npc.velocity.X -= .3F;
                }
                npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -5, 5);

                npc.velocity.Y = 6;
                if (npc.collideY)
                    npc.ai[0] = 2;
            }

            if (npc.ai[0] == 2) // NPC id burrowing.
            {
                npc.noTileCollide = true;
                npc.velocity.Y = .6F;
                npc.velocity.X = 0;

                if (npc.ai[1]++ <= 60) // 'Spew' projectiles.
                {
                    if (Main.netMode != 1 && npc.ai[1] % 15 == 0)
                        Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-40, 41) * .1F, -5), mod.ProjectileType("MetareeRock"), npc.damage, 1.2F);
                }
                else
                    npc.active = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[0] == 0)
                npc.frame.Y = 4 * frameHeight;
            else
            {
                if (npc.frameCounter++ >= 3)
                {
                    npc.frame.Y = (npc.frame.Y + frameHeight) % (4 * frameHeight);
                    npc.frameCounter = 0;
                }
            }
        }
    }
}
