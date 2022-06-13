using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace MetroidModPorted.Content.NPCs.OmegaPirate
{
	public class OmegaPirate_HitBox : ModNPC
	{
		public override string Texture => Mod.Name + "/Content/NPCs/OmegaPirate/OmegaPirate_Body";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
			Main.npcFrameCount[NPC.type] = 2;
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					69,
					70,
					ModContent.BuffType<Buffs.PhazonDebuff>()
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}
		public override void SetDefaults()
		{
			NPC.width = 82;
			NPC.height = 126;
			NPC.scale = 1f;
			NPC.damage = 0;
			NPC.defense = 30;
			NPC.lifeMax = 30000;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<OmegaPirate>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("An experiment created by the space pirates. It is a hulking monster corrupted by a biomass known as Phazon. It's capable of absorbing projectiles with its hands and firing grenades from a distance. Get too close and it will react by smashing the ground to create an energy wave. Even if you smash its armor the creature will go invisible and attempt to absorb Phazon to repair its defenses.")
			});
		}

		NPC Base
		{
			get { return Main.npc[(int)NPC.ai[0]]; }
		}

		int oldLife = 0;
		bool initialized = false;

		public override bool PreAI()
		{
			//shoulder
			if(NPC.ai[1] == 1f)
			{
				NPC.width = 58;
				NPC.height = 58;
			}
			//elbow
			if(NPC.ai[1] == 2f)
			{
				NPC.width = 38;
				NPC.height = 38;
			}
			//hand
			if(NPC.ai[1] == 3f)
			{
				NPC.width = 60;
				NPC.height = 60;
			}
			//thigh
			if(NPC.ai[1] == 4f)
			{
				NPC.width = 44;
				NPC.height = 44;
			}
			//shin
			if(NPC.ai[1] == 5f)
			{
				NPC.width = 50;
				NPC.height = 50;
			}
			//foot
			if(NPC.ai[1] == 6f)
			{
				NPC.width = 40;
				NPC.height = 40;
			}
			//cannon
			if(NPC.ai[1] == 7f)
			{
				NPC.width = 48;
				NPC.height = 48;
			}

			if (!initialized)
			{
				oldLife = NPC.lifeMax;
				initialized = true;
			}

			return true;
		}

		public override void AI()
		{
			bool visible = (Base.alpha < 255);
			if (!Base.active)
			{
				Terraria.Audio.SoundEngine.PlaySound((Terraria.Audio.SoundStyle)NPC.DeathSound,NPC.Center);
				NPC.life = 0;
				if(visible)
					NPC.HitEffect(0, 10.0);
				NPC.active = false;
				return;
			}
			NPC.damage = Base.damage;
			NPC.defense = Base.defense;
			NPC.realLife = Base.whoAmI;
			if(NPC.ai[1] != 0f || Base.dontTakeDamage || Base.ai[0] != 2)
			{
				NPC.dontTakeDamage = true;
				for(int i = 0; i < NPC.buffTime.Length; i++)
				{
					NPC.buffTime[i] = 0;
				}
			}
			NPC.GivenName = Base.GivenName;
			NPC.chaseable = !NPC.dontTakeDamage;

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (Base != null && Base.active && Base.ai[0] == 2 && Base.ai[1] == 2 && NPC.ai[1] == 0f)
				{
					Base.ai[3] += (oldLife - NPC.life);
					oldLife = NPC.life;
					Base.netUpdate2 = true;
				}
			}
		}

		public override bool? CanBeHitByItem(Player player, Item item) => NPC.ai[1] == 0f;
		public override bool? CanBeHitByProjectile(Projectile projectile) => NPC.ai[1] == 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int m = 0; m < (NPC.life <= 0 ? 20 : 5); m++)
			{
				int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 68, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
				Main.dust[dustID].noGravity = true;
			}
				
			if(NPC.life <= 0 && Base.ai[0] == 3)
			{
				var entitySource = NPC.GetSource_Death();
				Gore newGore;
				if(NPC.ai[1] == 1f)
				{
					newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore1").Type)];
					newGore.timeLeft = 60;
				}
				else if(NPC.ai[1] == 3f)
				{
					newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore3").Type)];
					newGore.timeLeft = 60;

					newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore4").Type)];
					newGore.timeLeft = 60;

					newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore5").Type)];
					newGore.timeLeft = 60;

					newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore6").Type)];
					newGore.timeLeft = 60;
				}
				else if(NPC.ai[1] == 4f)
				{
					newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore2").Type)];
					newGore.timeLeft = 60;
				}
				else
				{
					for(int i = 0; i < NPC.width; i += 10)
					{
						newGore = Main.gore[Gore.NewGore(entitySource, NPC.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, Mod.Find<ModGore>("OmegaPirateGore" + Main.rand.Next(6,9)).Type)];
						newGore.timeLeft = 60;
					}
				}
			}
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(projectile.penetrate > 0 && projectile.aiStyle != 3)
			{
				projectile.penetrate = 0;
				projectile.netUpdate = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
	}
}
