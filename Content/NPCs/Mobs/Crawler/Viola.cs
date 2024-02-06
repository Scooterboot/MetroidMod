using System.Collections.Generic;
using MetroidMod.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.WorldBuilding;

namespace MetroidMod.Content.NPCs.Mobs.Crawler
{
	public class Viola : MNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Viola");
			Main.npcFrameCount[Type] = 6;
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (MConfigMain.Instance.disablemobspawn == true)
			{
				return 0f;
			}
			return (spawnInfo.SpawnTileY > GenVars.lavaLine ? SpawnCondition.Cavern.Chance * 0.1f : 0) + SpawnCondition.Underworld.Chance * 0.15f;
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

			mNPC.crawlSpeed = 1f;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement("A moving orb with a strange face... Glows in the dark and is useful for light sources.")
			});
		}
		int frameNum = 1;
		public override bool PreAI()
		{
			mNPC.CrawlerAI(NPC, mNPC.crawlSpeed * NPC.scale, 1, false);

			NPC.frameCounter++;
			if (NPC.frameCounter >= 6)
			{
				NPC.frame.Y += frameNum;
				if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] - 1)
				{
					NPC.frame.Y = Main.npcFrameCount[NPC.type] - 1;
					frameNum = -1;
				}
				if (NPC.frame.Y <= 0)
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
			mNPC.DrawCrawler(NPC, sb, screenPos, drawColor);
			return false;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
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
