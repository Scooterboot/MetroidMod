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
		
		private bool spawn = false;
		private float newScale = -1;
		private float speed = 0.75f;
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
			
			if (Main.rand != null && Main.netMode != NetmodeID.MultiplayerClient)
			{
				newScale = Main.rand.Next(7, 10) * 0.1f;
				speed = Main.rand.Next(10, 15) * 0.1f;
			}
		}
		private void SetStats()
		{
			npc.scale = newScale;
			int newWidth = (int)((float)npc.width * npc.scale);
			int newHeight = (int)((float)npc.height * npc.scale);
			if (newWidth != npc.width)
			{
				npc.position.X += (float)(npc.width / 2) - (float)newWidth;
				npc.width = newWidth;
			}
			if (newHeight != npc.height)
			{
				npc.position.Y += (float)npc.height - (float)newHeight;
				npc.height = newHeight;
			}
			npc.defense = (int)((float)npc.defense * npc.scale);
			npc.damage = (int)((float)npc.damage * npc.scale);
			npc.life = (int)((float)npc.life * npc.scale);
			npc.lifeMax = npc.life;
			npc.value = (float)((int)(npc.value * npc.scale));
			npc.npcSlots *= npc.scale;
			npc.knockBackResist *= 2f - npc.scale;
		}
		public override bool PreAI()
		{
			if (!spawn && newScale != -1)
			{
				SetStats();
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
}