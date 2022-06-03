using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidModPorted.Content.NPCs.Mobs.Crawler
{
	public class Owtch : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Owtch");
			Main.npcFrameCount[Type] = 3;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Ocean.Chance * 0.2f + SpawnCondition.DesertCave.Chance * 0.2f;
		}
		
		private float speed = 0.5f;
		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 30;
			NPC.aiStyle = 67;
			NPC.damage = 15;
			NPC.defense = 30;
			NPC.lifeMax = 20;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 0, 75);
			NPC.knockBackResist = 0.75f;
			NPC.npcSlots = 1;
			//banner = NPC.type;
			//bannerItem = mod.ItemType("OwtchBanner");
			NPC.noGravity = true;
			NPC.behindTiles = true;
			
			mNPC.crawlSpeed = 0.5f;
		}
		public override bool PreAI()
		{
			mNPC.CrawlerAI(NPC,mNPC.crawlSpeed*NPC.scale);
			
			NPC.frameCounter++;
			if(NPC.frameCounter >= 4)
			{
				NPC.frame.Y += 1;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type])
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
			
			return false;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			mNPC.DrawCrawler(NPC,sb,screenPos,drawColor);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				var entitySource = NPC.GetSource_Death();
				Gore gore = Gore.NewGoreDirect(entitySource, NPC.Center, RandomVel, Mod.Find<ModGore>("OwtchGore").Type, NPC.scale);
				gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width,Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
				gore.timeLeft = 60;
				
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PinkCrystalShard, RandomVel.X, RandomVel.Y, 0, default(Color), 1f);
					dust.noGravity = false;
				}
			}
		}
	}
}
