using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Serris
{
    public class Serris_Tail : Serris_Body
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 15;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 32;
			npc.height = 32;
		}
		public override bool PreAI()
		{
			if(npc.localAI[0] == 1)
			{
				npc.width = 20;
				npc.height = 20;
			}
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					int gore = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisGore3"), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
	}
}
