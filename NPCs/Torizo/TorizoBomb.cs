using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Torizo
{
    public class TorizoBomb : ModNPC
    {
		int fall = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
		}
		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 26;
			npc.damage = 20;
			npc.defense = 0;
			npc.lifeMax = 1;
			npc.HitSound = SoundID.NPCHit3;
			npc.noGravity = true;
			npc.DeathSound = SoundID.NPCDeath3;
			npc.value = Item.buyPrice(0, 0, 0, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.behindTiles = true;
			npc.aiStyle = 9;
			npc.npcSlots = 0;
		}
		public override void AI()
		{
			fall++;
			if (fall > 60)
			{
			npc.velocity.Y += 0.15f;
			}
			if (Main.expertMode)
			{
				npc.damage = 35;
			}
			if (!Main.expertMode && (npc.velocity.X == 0 || npc.velocity.Y == 0))
			{
				npc.life = -1;
				npc.active = false;
			}
		}
	}
}