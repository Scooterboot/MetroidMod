using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Phantoon
{
    public class FireBall : ModNPC
    {public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireball");
			Main.npcFrameCount[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			npc.width = 14;
			npc.height = 28;
			npc.damage = 40;
			npc.defense = 25;
			npc.lifeMax = 5;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 0, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
		}
		
	}
}