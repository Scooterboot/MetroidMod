using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.crawler
{
	public class Geemer : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geemer");
			Main.npcFrameCount[npc.type] = 5;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Cavern.Chance * 0.3f + SpawnCondition.Underground.Chance * 0.3f;
		}
		
		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 28;
			npc.aiStyle = 67;
			npc.damage = 10;
			npc.defense = 3;
			npc.lifeMax = 30;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 0, 10);
			npc.knockBackResist = 0.75f;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("GeemerBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
			
			mNPC.crawlSpeed = 0.75f;
			
			SetStats();
		}
		private void SetStats()
		{
			npc.width = (int)((float)npc.width * npc.scale);
			npc.height = (int)((float)npc.height * npc.scale);
			npc.defense = (int)((float)npc.defense * npc.scale);
			npc.damage = (int)((float)npc.damage * npc.scale);
			npc.lifeMax = (int)((float)npc.lifeMax * npc.scale);
			npc.value = (float)((int)(npc.value * npc.scale));
			npc.npcSlots *= npc.scale;
			npc.knockBackResist *= 2f - npc.scale;
		}
		public override bool PreAI()
		{
			if(npc.ai[0] == 0)
			{
				mNPC.crawlSpeed = Main.rand.Next(10, 15) * 0.1f * npc.scale;
				npc.netUpdate = true;
			}
			mNPC.CrawlerAI(npc, mNPC.crawlSpeed);
			
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
			mNPC.DrawCrawler(npc,sb,drawColor);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				Gore gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/GeemerGore0"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/GeemerGore1"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				for(int i = 0; i < 4; i++)
				{
					gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/GeemerGore2"), npc.scale);
					gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
					gore.timeLeft = 60;
				}
			}
		}
	}
	public class Geemer_85 : Geemer
	{
		public override string Texture => "MetroidMod/NPCs/Mobs/crawler/Geemer";
		public override void SetDefaults()
		{
			npc.scale = 0.85f;
			base.SetDefaults();
		}
	}
	public class Geemer_75 : Geemer
	{
		public override string Texture => "MetroidMod/NPCs/Mobs/crawler/Geemer";
		public override void SetDefaults()
		{
			npc.scale = 0.75f;
			base.SetDefaults();
		}
	}
}