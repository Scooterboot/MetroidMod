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
    public class PhantoonUpperPart : ModNPC
    {public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantoon");
		}
		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 16;
			npc.damage = 0;
			npc.defense = 250;
			npc.lifeMax = 600;
			npc.alpha = 255;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath3;
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