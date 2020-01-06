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
            npc.width = 32; npc.height = 32;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
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
		// Maybe just use the justHit boolean for kago spawning?

        private void SpawnKago(Player player, int damage)
        {
            if (Main.netMode == 1) return;

            // Spawn one Kago per hit.
            NPC kago = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Kago"))];

            // Assign a random starting velocity to the newly created Kago to give some sort of 'punch-out' effect.
            kago.velocity = new Vector2(Main.rand.Next(6, 12) * player.direction, -2);
			kago.netUpdate = true;
        }
    }
}
