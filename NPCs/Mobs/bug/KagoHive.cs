using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.bug
{
    public class KagoHive : MNPC
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
		public override bool PreAI()
		{
			if (npc.ai[0] == 0)
				npc.ai[0] = npc.lifeMax;

			if (npc.justHit)
			{
				npc.TargetClosest(false);

				this.SpawnKago(Main.player[npc.target], (int)npc.ai[0] - npc.life);

				npc.ai[0] = npc.life;
			}

			return (true);
		}

        public override void FindFrame(int frameHeight)
        {
            if(npc.frameCounter++ >= 20)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }
        }
        private void SpawnKago(Player player, int damage)
        {
            if (Main.netMode == 1) return;

            // Spawn one Kago per hit.
            NPC kago = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Kago"))];

            // Assign a random starting velocity to the newly created Kago to give some sort of 'punch-out' effect.
            kago.velocity = new Vector2(Main.rand.Next(6, 12) * Math.Sign(npc.Center.X - player.Center.X), -2);
			kago.netUpdate = true;
        }
    }
}
