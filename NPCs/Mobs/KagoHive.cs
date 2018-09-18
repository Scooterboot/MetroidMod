using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class KagoHive : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            /* Temporary NPC values */
            npc.lifeMax = 800;
            npc.width = 32; npc.height = 32;
            npc.damage = 0;

            npc.HitSound = SoundID.NPCHit1;

            npc.knockBackResist = 0;

            npc.scale = 1.5F;
        }

        public override void FindFrame(int frameHeight)
        {
            if(npc.frameCounter++ >= 20)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            this.SpawnKago(player, damage);
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            this.SpawnKago(Main.player[projectile.owner], damage);
        }

        private void SpawnKago(Player player, int damage)
        {
            // Spawn one Kago per hit.
            NPC kago = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Kago"))];

            // Assign a random starting velocity to the newly created Kago to give some sort of 'punch-out' effect.
            kago.velocity = new Vector2(Main.rand.Next(6, 12) * player.direction, -2);
        }
    }
}
