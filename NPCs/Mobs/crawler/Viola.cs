using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.crawler
{
	public class Viola : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viola");
			Main.npcFrameCount[npc.type] = 6;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Cavern.Chance + SpawnCondition.Underworld.Chance;
		}
		
		private bool spawn = false;
		private float speed = 1f;
		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.aiStyle = 67;
			npc.damage = 15;
			npc.defense = 4;
			npc.lifeMax = 45;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = Item.buyPrice(0, 0, 0, 20);
			npc.knockBackResist = 0.75f;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("ViolaBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
		}
		int frameNum = 1;
		public override bool PreAI()
		{
			if (!spawn)
			{
				spawn = true;
				npc.netUpdate = true;
			}
			
			mNPC.CrawlerAI(npc, speed*npc.scale, 1, false);
			
			npc.frameCounter++;
			if(npc.frameCounter >= 6)
			{
				npc.frame.Y += frameNum;
				if(npc.frame.Y >= Main.npcFrameCount[npc.type]-1)
				{
					npc.frame.Y = Main.npcFrameCount[npc.type]-1;
					frameNum = -1;
				}
				if(npc.frame.Y <= 0)
				{
					npc.frame.Y = 0;
					frameNum = 1;
				}
				npc.frameCounter = 0;
			}
			
			Lighting.AddLight(npc.Center, 0.4f, 1f, 0.65f);
			
			return false;
		}
		
		public override Color? GetAlpha(Color drawColor)
		{
			drawColor = Color.White;
			return drawColor;
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			mNPC.DrawCrawler(npc,sb);
			return false;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1.5f);
					dust.noGravity = true;
				}
			}
		}
	}
}
