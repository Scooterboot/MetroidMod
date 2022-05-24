using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidModPorted.Content.NPCs.Mobs.Crawler
{
	public class Sova : MNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sova");
			Main.npcFrameCount[Type] = 5;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Underworld.Chance * 0.25f;
		}
		
		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 28;
			NPC.aiStyle = 67;
			NPC.damage = 15;
			NPC.defense = 10;
			NPC.lifeMax = 50;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 1, 20);
			NPC.knockBackResist = 0.75f;
			NPC.npcSlots = 1;
			//banner = NPC.type;
			//bannerItem = mod.ItemType("SovaBanner");
			NPC.noGravity = true;
			NPC.behindTiles = true;
			NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.CursedInferno] = true;
			
			mNPC.crawlSpeed = 0.75f;
			
			SetStats();
		}
		private void SetStats()
		{
			NPC.width = (int)((float)NPC.width * NPC.scale);
			NPC.height = (int)((float)NPC.height * NPC.scale);
			NPC.defense = (int)((float)NPC.defense * NPC.scale);
			NPC.damage = (int)((float)NPC.damage * NPC.scale);
			NPC.lifeMax = (int)((float)NPC.lifeMax * NPC.scale);
			NPC.value = (float)((int)(NPC.value * NPC.scale));
			NPC.npcSlots *= NPC.scale;
			NPC.knockBackResist *= 2f - NPC.scale;
		}
		public override bool PreAI()
		{
			if(NPC.ai[0] == 0)
			{
				mNPC.crawlSpeed = Main.rand.Next(10, 15) * 0.1f * NPC.scale;
				NPC.netUpdate = true;
			}
			mNPC.CrawlerAI(NPC, mNPC.crawlSpeed);
			
			NPC.frameCounter++;
			if(NPC.frameCounter >= 4)
			{
				NPC.frame.Y += 1;
				if(NPC.frame.Y >= Main.npcFrameCount[Type])
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
			
			Dust dust = Dust.NewDustDirect(NPC.position,NPC.width,NPC.height,6, 0f,0f, 100, default(Color), 1f);
			dust.noGravity = true;
			
			Lighting.AddLight(NPC.Center, 1f, 0.65f, 0.4f);
			
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

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			mNPC.DrawCrawler(NPC,sb,screenPos,drawColor);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && Main.netMode != 2)
			{
				for (int i = 0; i < 10; i++)
				{
					int num727 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 31, 0f, 0f, 100, default(Color), 1.5f);
					Dust dust = Main.dust[num727];
					dust.velocity.Y = -Math.Abs(RandomVel.Y);
				}
				for (int i = 0; i < 20; i++)
				{
					int num729 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 3.5f);
					Dust dust = Main.dust[num729];
					dust.noGravity = true;
					dust.velocity.Y = -Math.Abs(RandomVel.Y);
					num729 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 1.5f);
					dust = Main.dust[num729];
					dust.noGravity = false;
					dust.velocity.Y = -Math.Abs(RandomVel.Y);
				}
			}
		}
	}
	public class Sova_85 : Sova
	{
		public override string Texture => "MetroidModPorted/Content/NPCs/Mobs/Crawler/Sova";
		public override void SetDefaults()
		{
			NPC.scale = 0.85f;
			base.SetDefaults();
		}
	}
	public class Sova_75 : Sova
	{
		public override string Texture => "MetroidModPorted/Content/NPCs/Mobs/Crawler/Sova";
		public override void SetDefaults()
		{
			NPC.scale = 0.75f;
			base.SetDefaults();
		}
	}
}
