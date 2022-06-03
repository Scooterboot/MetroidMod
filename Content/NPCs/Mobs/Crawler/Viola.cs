using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidModPorted.Content.NPCs.Mobs.Crawler
{
	public class Viola : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viola");
			Main.npcFrameCount[Type] = 6;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return (spawnInfo.SpawnTileY > WorldGen.lavaLine ? SpawnCondition.Cavern.Chance * 0.1f : 0) + SpawnCondition.Underworld.Chance * 0.15f;
		}
		
		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.aiStyle = 67;
			NPC.damage = 15;
			NPC.defense = 4;
			NPC.lifeMax = 45;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = Item.buyPrice(0, 0, 0, 20);
			NPC.knockBackResist = 0.75f;
			NPC.npcSlots = 1;
			//banner = NPC.type;
			//bannerItem = mod.ItemType("ViolaBanner");
			NPC.noGravity = true;
			NPC.behindTiles = true;
			NPC.lavaImmune = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			
			mNPC.crawlSpeed = 1f;
		}
		int frameNum = 1;
		public override bool PreAI()
		{
			mNPC.CrawlerAI(NPC, mNPC.crawlSpeed*NPC.scale, 1, false);
			
			NPC.frameCounter++;
			if(NPC.frameCounter >= 6)
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
			
			Lighting.AddLight(NPC.Center, 0.4f, 1f, 0.65f);
			
			return false;
		}
		
		public override Color? GetAlpha(Color drawColor)
		{
			drawColor = Color.White;
			return drawColor;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			mNPC.DrawCrawler(NPC,sb,screenPos,drawColor);
			return false;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && Main.netMode != 2)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 68, 0f, 0f, 100, default(Color), 1.5f);
					dust.noGravity = true;
				}
			}
		}
	}
}
