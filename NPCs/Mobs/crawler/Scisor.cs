using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.crawler
{
	public class Scisor : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scisor");
			Main.npcFrameCount[npc.type] = 4;
		}
		
		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Ocean.Chance + SpawnCondition.DesertCave.Chance;
		}*/
		
		private bool spawn = false;
		private float speed = 1f;
		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 44;
			npc.aiStyle = 67;
			npc.damage = 15;
			npc.defense = 5;
			npc.lifeMax = 50;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 0, 75);
			npc.knockBackResist = 0.75f;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("ScisorBanner");
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
			if(npc.frameCounter >= 12)
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
				Gore gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/ScisorGore0"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/ScisorGore1"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/ScisorGore2"), npc.scale);
				gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
				gore.timeLeft = 60;
				for(int i = 0; i < 4; i++)
				{
					gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/ScisorGore3"), npc.scale);
					gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
					gore.timeLeft = 60;
				}
			}
		}
	}
}
