using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.crawler
{
	public class Sova : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sova");
			Main.npcFrameCount[npc.type] = 5;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Underworld.Chance * 0.25f;
		}
		
		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 28;
			npc.aiStyle = 67;
			npc.damage = 15;
			npc.defense = 10;
			npc.lifeMax = 50;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 1, 20);
			npc.knockBackResist = 0.75f;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("SovaBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
			
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
			
			Dust dust = Dust.NewDustDirect(npc.position,npc.width,npc.height,6, 0f,0f, 100, default(Color), 1f);
			dust.noGravity = true;
			
			Lighting.AddLight(npc.Center, 1f, 0.65f, 0.4f);
			
			return false;
		}
		
		public override Color? GetAlpha(Color drawColor)
		{
			drawColor.R = 255;
			drawColor.G = Utils.Clamp<byte>(drawColor.G, 175, 255);
			drawColor.B = Math.Min(drawColor.B, (byte)75);
			drawColor.A = 255;
			return drawColor;
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
				for (int i = 0; i < 10; i++)
				{
					int num727 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 31, 0f, 0f, 100, default(Color), 1.5f);
					Dust dust = Main.dust[num727];
					dust.velocity.Y = -Math.Abs(RandomVel.Y);
				}
				for (int i = 0; i < 20; i++)
				{
					int num729 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 3.5f);
					Dust dust = Main.dust[num729];
					dust.noGravity = true;
					dust.velocity.Y = -Math.Abs(RandomVel.Y);
					num729 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 1.5f);
					dust = Main.dust[num729];
					dust.noGravity = false;
					dust.velocity.Y = -Math.Abs(RandomVel.Y);
				}
			}
		}
	}
	public class Sova_85 : Sova
	{
		public override string Texture => "MetroidMod/NPCs/Mobs/crawler/Sova";
		public override void SetDefaults()
		{
			npc.scale = 0.85f;
			base.SetDefaults();
		}
	}
	public class Sova_75 : Sova
	{
		public override string Texture => "MetroidMod/NPCs/Mobs/crawler/Sova";
		public override void SetDefaults()
		{
			npc.scale = 0.75f;
			base.SetDefaults();
		}
	}
}
