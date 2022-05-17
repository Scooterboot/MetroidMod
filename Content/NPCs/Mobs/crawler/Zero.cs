using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidModPorted.Content.NPCs.Mobs.Crawler
{
	public class Zero : MNPC
	{
		internal readonly float[] speeds = { .05F, .15F, .25F, .35F };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zero");
			Main.npcFrameCount[NPC.type] = 4;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.UndergroundJungle.Chance * 0.075f;
		}
		
		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 28;
			NPC.aiStyle = 67;
			NPC.damage = 18;
			NPC.defense = 6;
			NPC.lifeMax = 60;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 5, 0);
			NPC.knockBackResist = 0.75f;
			NPC.npcSlots = 1;
			//banner = NPC.type;
			//bannerItem = mod.ItemType("ZeroBanner");
			NPC.noGravity = true;
			NPC.behindTiles = true;
		}

		int frameNum = 1;
		public override bool PreAI()
		{
			NPC.frameCounter++;
			if(NPC.frameCounter > 16)
			{
				NPC.frame.Y += frameNum;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type]-1)
				{
					NPC.frame.Y = Main.npcFrameCount[NPC.type]-1;
					frameNum = -1;
				}
				if(NPC.frame.Y <= 0)
				{
					NPC.frame.Y = 0;
					frameNum = 1;
				}
				NPC.frameCounter = 0;
			}
			
			if(NPC.ai[3] != 0f)
			{
				NPC.frame.Y = 0;
				frameNum = 1;
			}
			
			mNPC.crawlSpeed = speeds[(int)NPC.frame.Y];
			mNPC.CrawlerAI(NPC, mNPC.crawlSpeed, 0, true, true);

			return false;
		}
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			mNPC.DrawCrawler(NPC,sb,drawColor);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && Main.netMode != 2)
			{
				Gore gore = Gore.NewGoreDirect(NPC.GetSource_Death(), NPC.Center, RandomVel, Mod.Find<ModGore>("ZeroGore0").Type, NPC.scale);
				gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width,Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
				gore.timeLeft = 60;
				gore = Gore.NewGoreDirect(NPC.GetSource_Death(), NPC.Center, RandomVel, Mod.Find<ModGore>("ZeroGore1").Type, NPC.scale);
				gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width,Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
				gore.timeLeft = 60;
				
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 5, RandomVel.X, RandomVel.Y, 0, default(Color), 1.5f);
					dust.noGravity = false;
				}
			}
		}
	}
}
