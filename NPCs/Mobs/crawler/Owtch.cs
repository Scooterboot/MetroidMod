using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.crawler
{
	public class Owtch : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Owtch");
			Main.npcFrameCount[npc.type] = 3;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Ocean.Chance + SpawnCondition.DesertCave.Chance;
		}
		
		private bool spawn = false;
		private float speed = 0.5f;
		public override void SetDefaults()
		{
			npc.width = 30;
			npc.height = 30;
			npc.aiStyle = 67;
			npc.damage = 15;
			npc.defense = 30;
			npc.lifeMax = 20;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 0, 75);
			npc.knockBackResist = 0.75f;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("OwtchBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
		}
		public override bool PreAI()
		{
			if (!spawn)
			{
				spawn = true;
				npc.netUpdate = true;
			}
			
			mNPC.CrawlerAI(npc, speed*npc.scale);
			
			npc.frameCounter++;
			if(npc.frameCounter >= 4)
			{
				npc.frame.Y += 1;
				if(npc.frame.Y >= Main.npcFrameCount[npc.type])
				{
					npc.frame.Y = 0;
				}
				npc.frameCounter = 0;
			}
			
			return false;
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			mNPC.DrawCrawler(npc,sb);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				Gore gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/OwtchGore"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 69, RandomVel.X, RandomVel.Y, 0, default(Color), 1f);
					dust.noGravity = false;
				}
			}
		}
	}
}
