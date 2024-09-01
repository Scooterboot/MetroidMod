using System.Collections.Generic;
using MetroidMod.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidMod.Content.NPCs.Mobs.Crawler
{
	public class Zeela : MNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Zeela");
			Main.npcFrameCount[Type] = 5;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (MConfigMain.Instance.disablemobspawn == true)
			{
				return 0f;
			}
			return SpawnCondition.UndergroundJungle.Chance * 0.25f;
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
			//bannerItem = mod.ItemType("ZeelaBanner");
			NPC.noGravity = true;
			NPC.behindTiles = true;

			mNPC.crawlSpeed = 0.75f;

			SetStats();
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Zeela")
			});
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
			if (NPC.ai[0] == 0)
			{
				mNPC.crawlSpeed = Main.rand.Next(10, 15) * 0.1f * NPC.scale;
				NPC.netUpdate = true;
			}
			mNPC.CrawlerAI(NPC, mNPC.crawlSpeed);

			NPC.frameCounter++;
			if (NPC.frameCounter >= 4)
			{
				NPC.frame.Y += 1;
				if (NPC.frame.Y >= Main.npcFrameCount[NPC.type])
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}

			return false;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			mNPC.DrawCrawler(NPC, sb, screenPos, drawColor);
			return false;
		}

		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				var entitySource = NPC.GetSource_Death();
				Gore gore = Gore.NewGoreDirect(entitySource, NPC.Center, RandomVel, Mod.Find<ModGore>("ZeelaGore0").Type, NPC.scale);
				gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width, Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
				gore.timeLeft = 60;
				gore = Gore.NewGoreDirect(entitySource, NPC.Center, RandomVel, Mod.Find<ModGore>("ZeelaGore1").Type, NPC.scale);
				gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width, Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
				gore.timeLeft = 60;
				for (int i = 0; i < 4; i++)
				{
					gore = Gore.NewGoreDirect(entitySource, NPC.Center, RandomVel, Mod.Find<ModGore>("ZeelaGore2").Type, NPC.scale);
					gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width, Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
					gore.timeLeft = 60;
					if (i < 2)
					{
						gore = Gore.NewGoreDirect(entitySource, NPC.Center, RandomVel, Mod.Find<ModGore>("ZeelaGore3").Type, NPC.scale);
						gore.position -= new Vector2(Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Width, Terraria.GameContent.TextureAssets.Gore[gore.type].Value.Height) / 2;
						gore.timeLeft = 60;
					}
				}
			}
		}
	}
	public class Zeela_85 : Zeela
	{
		public override string Texture => "MetroidMod/Content/NPCs/Mobs/Crawler/Zeela";
		public override void SetDefaults()
		{
			NPC.scale = 0.85f;
			base.SetDefaults();
		}
	}
	public class Zeela_75 : Zeela
	{
		public override string Texture => "MetroidMod/Content/NPCs/Mobs/Crawler/Zeela";
		public override void SetDefaults()
		{
			NPC.scale = 0.75f;
			base.SetDefaults();
		}
	}
}
