using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Scisor : Crawler
    {
		public override bool Autoload(ref string name)
		{
			return (true);
		}

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 32; npc.height = 24;

            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 20;
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;

            npc.noGravity = true;
            npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;

			this.crawlSpeed = 1.5f;
			this.rotationXOffset = this.rotationYOffset = 10;
        }
    }
}
