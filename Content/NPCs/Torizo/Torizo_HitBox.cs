using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Torizo
{
	public class Torizo_HitBox : ModNPC
	{
		public override string Texture => Mod.Name + "/Content/NPCs/Torizo/TorizoHand_Front";

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

			NPCID.Sets.SpecificDebuffImmunity[Type][31] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.IceFreeze>()] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.InstantFreeze>()] = true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Torizo>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			/*bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				new FlavorTextBestiaryInfoElement("Nothing mysterious at all about this thing.")
			});*/
		}
		public override void SetDefaults()
		{
			NPC.width = 68;
			NPC.height = 62;
			NPC.scale = 1f;
			NPC.damage = 10;
			NPC.defense = 10;
			NPC.lifeMax = 1500;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * balance);
			//NPC.damage = (int)(NPC.damage * 0.7f);
		}

		internal NPC Base => Main.npc[(int)NPC.ai[0]];

		public override bool PreAI()
		{
			//body
			if (NPC.ai[1] == 1f)
			{
				NPC.width = 56;
				NPC.height = 94;
			}
			//shoulder
			if (NPC.ai[1] == 2f)
			{
				NPC.width = 30;
				NPC.height = 30;
			}
			//arm
			if (NPC.ai[1] == 3f)
			{
				NPC.width = 30;
				NPC.height = 30;
			}
			//hand
			if (NPC.ai[1] == 4f)
			{
				NPC.width = 32;
				NPC.height = 32;
			}
			//thigh
			if (NPC.ai[1] == 5f)
			{
				NPC.width = 30;
				NPC.height = 30;
			}
			//calf
			if (NPC.ai[1] == 6f)
			{
				NPC.width = 28;
				NPC.height = 28;
			}
			return true;
		}
		public override void AI()
		{
			bool flag = (Base.alpha < 255);
			if (!Base.active)
			{
				NPC.life = 0;
				if (flag)
				{
					if (NPC.ai[1] == 1f)
					{
						SoundEngine.PlaySound((SoundStyle)NPC.DeathSound, NPC.Center);
					}
					NPC.HitEffect(0, 10.0);
				}
				NPC.active = false;
				return;
			}
			NPC.damage = Base.damage;
			NPC.defense = Base.defense;
			NPC.GivenName = Base.GivenName;
			NPC.direction = Base.direction;
			if (NPC.ai[1] > 1f)
			{
				NPC.dontTakeDamage = true;
				NPC.realLife = Base.whoAmI;
			}
			else
			{
				if (NPC.ai[1] == 1f)
				{
					NPC.realLife = Base.whoAmI;
				}
				NPC.dontTakeDamage = Base.dontTakeDamage;
			}
		}

		public override bool? CanBeHitByItem(Player player, Item item) => (NPC.ai[1] <= 1f) ? null : false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => (NPC.ai[1] <= 1f) ? null : false;

		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (NPC.ai[1] == 0f)
			{
				//Base.StrikeNPC(hit);
			}
		}
		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (NPC.ai[1] == 0f)
			{
				//Base.StrikeNPC(hit);
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.ai[1] <= 1f)
			{
				Base.HitEffect(hit);
			}

			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				Gore newGore;
				if (NPC.ai[1] == 0f)
				{
					for (int i = 0; i < 3; i++)
					{
						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoHeadGore1").Type)];
						newGore.timeLeft = 60;

						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoHeadGore" + (2 + i)).Type)];
						newGore.timeLeft = 60;
					}
				}
				if (NPC.ai[1] == 1f)
				{
					newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoBodyGore1").Type)];
					newGore.timeLeft = 60;

					for (int i = 0; i < 3; i++)
					{
						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoBodyGore2").Type)];
						newGore.timeLeft = 60;

						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoBodyGore3").Type)];
						newGore.timeLeft = 60;

						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoBodyGore4").Type)];
						newGore.timeLeft = 60;
					}
				}
				if (NPC.ai[1] >= 2f)
				{
					if (NPC.ai[1] == 2)
					{
						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoGore1").Type)];
						newGore.timeLeft = 60;
					}

					newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoGore" + NPC.ai[1]).Type)];
					newGore.timeLeft = 60;

					if (NPC.ai[1] == 6)
					{
						newGore = Main.gore[Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(0, 30), new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("TorizoGore7").Type)];
						newGore.timeLeft = 60;
					}
				}

				for (int num70 = 0; num70 < 10; num70++)
				{
					int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
					Main.dust[num71].velocity *= 1.4f;
					Main.dust[num71].noGravity = true;
					int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num72].velocity *= 1.4f;
					Main.dust[num72].noGravity = true;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => NPC.ai[1] <= 0f;
	}
}
