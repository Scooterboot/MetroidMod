using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MetroidMod.Content.NPCs.Nightmare
{
	public class Nightmare_Tail : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nightmare");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 80;
			NPC.height = 64;
			NPC.scale = 1f;
			NPC.damage = 0;//50;
			NPC.defense = 50;
			NPC.lifeMax = 10000;
			//NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Nightmare>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement("Nightmare's Tail.")
			});
		}

		int _gravityField = 0;
		Projectile GravityField
		{
			get { return Main.projectile[_gravityField]; }
		}

		int oldLife = 0;
		bool initialized = false;

		public override bool PreAI()
		{
			if (!initialized)
			{
				oldLife = NPC.life;
				initialized = true;
			}
			return (true);
		}

		public override void AI()
		{
			NPC Head = Main.npc[(int)NPC.ai[0]];
			bool flag = (Head.alpha < 255);
			if (!Head.active)
			{
				SoundEngine.PlaySound((SoundStyle)NPC.DeathSound, NPC.Center);
				NPC.life = 0;
				if (flag)
				{
					NPC.HitEffect(0, 10.0);
				}
				NPC.active = false;
				return;
			}

			NPC.velocity *= 0f;
			NPC.damage = Head.damage;
			NPC.dontTakeDamage = Head.dontTakeDamage;
			NPC.Center = Head.Center + new Vector2(-76 * Head.direction, 88);

			if (NPC.ai[1] == 1)
			{
				if (NPC.ai[2] == 0)
				{
					// Spawn gravity field projectile
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 spawnPos = new Vector2(NPC.Center.X + 26 * Head.direction, NPC.Center.Y + 14);
						_gravityField = Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, Vector2.Zero, ModContent.ProjectileType<Projectiles.Boss.NightmareGravityField>(), 0, 0f, Main.myPlayer, Head.whoAmI, NPC.whoAmI);
					}

					NPC.ai[2] = 1;
				}
				NPC.ai[1] = 0;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// If the Tail has an active GravityField projectile, we want to keep track of lost health to update its state.
				if (NPC.ai[2] == 1)
				{
					if (NPC.justHit)
					{
						NPC.ai[3] += oldLife - NPC.life;
						oldLife = NPC.life;
					}
				}

				if (GravityField == null || !GravityField.active)
				{
					NPC.ai[2] = 0;
					NPC.ai[3] = 0;
					NPC.netUpdate = true;
				}
				else if (NPC.ai[2] == 1 && NPC.ai[3] > 2000)
				{
					GravityField.localAI[0] = 1f;
					GravityField.netUpdate2 = true;
				}
			}
		}
		
		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if(NPC.ai[2] == 0)
			{
				modifiers.FinalDamage /= (int)(item.damage * 10f);
			}
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if(NPC.ai[2] == 0)
			{
				modifiers.FinalDamage /= (int)(projectile.damage * 10f);
			}
		}
		
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				for (int num70 = 0; num70 < 15; num70++)
				{
					int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
					Main.dust[num71].velocity *= 1.4f;
					Main.dust[num71].noGravity = true;
					int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num72].velocity *= 1.4f;
					Main.dust[num72].noGravity = true;
				}
				//Main.PlaySound(4,(int)NPC.position.X,(int)NPC.position.Y,14);
					
				int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(0f,3f), Mod.Find<ModGore>("NightmareTailGore").Type, 1f);
				Main.gore[gore].velocity *= 0.4f;
				Main.gore[gore].timeLeft = 60;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return (false);
		}
	}
}
