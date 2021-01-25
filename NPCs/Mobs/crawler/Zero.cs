using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.crawler
{
	public class Zero : MNPC
	{
		internal readonly float[] speeds = { .05F, .15F, .25F, .35F };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zero");
			Main.npcFrameCount[npc.type] = 4;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.UndergroundJungle.Chance * 0.075f;
		}
		
		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 28;
			npc.aiStyle = 67;
			npc.damage = 18;
			npc.defense = 6;
			npc.lifeMax = 60;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 5, 0);
			npc.knockBackResist = 0.75f;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("ZeroBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
		}

		int frameNum = 1;
		public override bool PreAI()
		{
			npc.frameCounter++;
			if(npc.frameCounter > 16)
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
			
			if(npc.ai[3] != 0f)
			{
				npc.frame.Y = 0;
				frameNum = 1;
			}
			
			mNPC.crawlSpeed = speeds[(int)npc.frame.Y];
			mNPC.CrawlerAI(npc, mNPC.crawlSpeed, 0, true, true);

			return false;
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			mNPC.DrawCrawler(npc,sb,drawColor);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				Gore gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/ZeroGore0"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/ZeroGore1"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5, RandomVel.X, RandomVel.Y, 0, default(Color), 1.5f);
					dust.noGravity = false;
				}
			}
		}
	}
}
