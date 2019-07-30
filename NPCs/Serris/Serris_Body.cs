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
    public class Serris_Body : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 10;
		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 20;
			npc.defense = 28;
			npc.lifeMax = 500;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.frameCounter = 0;
			npc.aiStyle = 6;
			npc.npcSlots = 1;
		}
		public override void AI()
		{
			NPC head = Main.npc[npc.realLife];
			NPC start = Main.npc[(int)npc.ai[1]];
			NPC end = Main.npc[(int)npc.ai[0]];
			if(missingSegment(head) || missingSegment(start) || (missingSegment(end) && npc.ai[0] > 0))
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
				return;
			}
			npc.damage = head.damage;
		}
		bool missingSegment(NPC npc)
		{
			return (npc == null || !npc.active || (npc.type != mod.NPCType("Serris_Head") && npc.type != mod.NPCType("Serris_Body") && npc.type != mod.NPCType("Serris_Tail")));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					int gore = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisGore2"), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
		}
	}
}
