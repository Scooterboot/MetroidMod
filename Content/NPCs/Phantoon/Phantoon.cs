using System;
using System.Collections.Generic;
using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Phantoon
{
	[AutoloadBossHead]
	public class Phantoon : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phantoon");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCID.Sets.SpecificDebuffImmunity[Type][20] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][24] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][31] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][39] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][44] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.PhazonDebuff>()] = true;
		}
		int damage = 130;//65;
		int oldLife = 0;
		public override void SetDefaults()
		{
			NPC.width = 92;
			NPC.height = 180;
			NPC.damage = 0;
			NPC.defense = 50;
			NPC.lifeMax = 15000;
			NPC.dontTakeDamage = true;
			NPC.alpha = 255;
			NPC.scale = 1f;
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath10;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 7, 0);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = false;
			NPC.aiStyle = -1;
			NPC.npcSlots = 5;
			if (Main.netMode != NetmodeID.Server) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Kraid"); }
			//bossBag = mod.ItemType("PhantoonBag");
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement("An interdimensional anomaly drawn in by places of great negative emotions. Usually shipwrecks with lingering energy. The creature's only weakness is the eye in its mouth. It is capable of dematerializing at will... It's capable of spawning flaming eyes and rage hands to attack anything that gets in its way! To think that this is only the creature's head... it bears a resemblance to a certain deity...")
			});
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * balance + 1);
			NPC.damage = 0;//(int)(NPC.damage * 0.7f);
			damage *= 2;
			damage = (int)(damage * 0.7f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void OnKill()
		{
			MSystem.bossesDown |= MetroidBossDown.downedPhantoon;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.PhantoonBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.GravityFlare>(), 1));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.KraidPhantoonMusicBox>(), 6));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.PhantoonMask>(), 8));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.PhantoonTrophy>(), 11));

			npcLoot.Add(notExpertRule);
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < 20; i++)
				{
					Dust newDust = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 50, default(Color), 1.5f)];
					newDust.velocity *= 2f;
					newDust.noGravity = true;
				}
				var entitySource = NPC.GetSource_Death();
				Gore.NewGore(entitySource, new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2(/*hitDirection,*/ 0f) * .3f, 61, NPC.scale);
				Gore.NewGore(entitySource, new Vector2(NPC.position.X, NPC.position.Y + (NPC.height / 2) - 15f), new Vector2(0f) * .3f, 62, NPC.scale);
				Gore.NewGore(entitySource, new Vector2(NPC.position.X, NPC.position.Y + NPC.height - 20f), new Vector2(0f) * .3f, 63, NPC.scale);
			}
		}

		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(BossHeadTexture);
			if (NPC.alpha > 192)
			{
				index = -1;
			}
		}

		bool initialized = false;
		public override bool PreAI()
		{
			if (!initialized)
			{
				initialized = true;
				NPC.netUpdate = true;
				NPC.TargetClosest(true);
				NPC.Center = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 200);
				oldLife = NPC.life;
			}
			return true;
		}

		int frameNum = 1;

		int eyeOpen = 0;
		int eyeFrame = 0;
		int eyeFrameCounter = 0;

		int state = 0;

		bool initialTeleport = false;

		public override void AI()
		{
			if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 2000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
			{
				NPC.TargetClosest(true);
				if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 2000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
				{
					NPC.ai[2] = 3f;
					eyeOpen = 0;
					NPC.alpha += 3;
					if (NPC.alpha > 255)
					{
						NPC.active = false;
					}
				}
			}

			state = 0;
			if (NPC.life <= (int)(NPC.lifeMax * 0.8f))
			{
				state = 1;
			}
			if (NPC.life <= (int)(NPC.lifeMax * 0.55f))
			{
				state = 2;
			}
			if (NPC.life <= (int)(NPC.lifeMax * 0.3f))
			{
				state = 3;
			}

			int[,] fireBallRand = new int[4, 2];
			fireBallRand[0, 0] = 15;
			fireBallRand[1, 0] = 30;
			fireBallRand[2, 0] = 40;
			fireBallRand[3, 0] = 50;

			fireBallRand[0, 1] = 25;
			fireBallRand[1, 1] = 50;

			fireBallRand[2, 1] = 60;

			if (state == 1)
			{
				fireBallRand[2, 0] = 45;
				fireBallRand[3, 0] = 60;

				fireBallRand[0, 1] = 30;
				fireBallRand[1, 1] = 60;
			}

			if (state == 2)
			{
				fireBallRand[2, 0] = 50;
				fireBallRand[3, 0] = 70;

				fireBallRand[0, 1] = 40;
				fireBallRand[1, 1] = 70;

				fireBallRand[2, 1] = 75;
			}

			if (state == 2)
			{
				fireBallRand[2, 0] = 55;
				fireBallRand[3, 0] = 80;

				fireBallRand[0, 1] = 50;
				fireBallRand[1, 1] = 80;

				fireBallRand[2, 1] = 75;
			}

			// Spawn animation
			if (NPC.ai[0] < 660)
			{
				NPC.ai[0]++;
				// Summon fire balls
				if (NPC.ai[0] > 30 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (NPC.ai[0] <= 270 && (NPC.ai[0] - 60) % 30 == 0)
					{
						int index = (int)(NPC.ai[0] - 60) / 30;
						float dist = 120;
						float rot = -((float)Math.PI / 2) + (((float)Math.PI / 4) * index);
						Vector2 pos = NPC.Center + new Vector2((float)Math.Cos(rot) * dist, (float)Math.Sin(rot) * dist);

						spawnFireBall(pos.X, pos.Y, true, -1, rot);
					}
				}

				// Fade into existence
				if (NPC.ai[0] > 550)
					NPC.alpha = Math.Max(NPC.alpha - 3, 127);
				NPC.ai[2] = 0f;
			}
			else // AI
			{
				// Basic movement and fireball spawning
				if (NPC.ai[2] == 0f)
				{
					NPC.ai[1]++;

					// Spawn fireball every 20 frames
					if (NPC.ai[1] % 20 == 0f && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int rand = Main.rand.Next(100);

						// Spawn only super and targeting fireballs when too far from the player
						if (Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) >= (600 - 100 * state))
						{
							if (rand < fireBallRand[0, 1])
							{
								// Spawn super fireball
								spawnFireBall(NPC.Center.X, NPC.Center.Y, true, 2);
							}
							else if (rand < fireBallRand[1, 1])
							{
								// Spawn targeting fireballs
								for (int i = 0; i < 2 + 2 * state; i++)
								{
									spawnFireBall(NPC.Center.X, NPC.Center.Y, true, 1, -((float)Math.PI / 2) + (((float)Math.PI / 4) * Main.rand.Next(8)));
								}
							}
						}
						else // spawn bounce or targeting fireballs when close to the player
						{
							if (rand < fireBallRand[0, 0])
							{
								// Spawn basic bounce fireball
								spawnFireBall(NPC.Center.X, NPC.Center.Y + 46);
							}
							else if (rand < fireBallRand[1, 0])
							{
								// Spawn bundle of bounce fireballs
								for (int i = 0; i < 5; i++)
								{
									spawnFireBall(NPC.Center.X, NPC.Center.Y + 46);
								}
							}
							else if (rand < fireBallRand[2, 0])
							{
								// Spawn targeting fireballs
								for (int i = 0; i < 2 + state; i++)
								{
									spawnFireBall(NPC.Center.X, NPC.Center.Y, true, 1, -((float)Math.PI / 2) + (((float)Math.PI / 4) * Main.rand.Next(8)));
								}
							}
							else if (rand < fireBallRand[3, 0])
							{
								// Spawn super fireball
								spawnFireBall(NPC.Center.X, NPC.Center.Y, true, 2);
							}
						}
					}

					eyeOpen = 0;
					NPC.alpha = Math.Max(NPC.alpha - 3, 127);

					if (NPC.ai[1] >= 360f && Main.netMode != NetmodeID.MultiplayerClient) // open eye
					{
						NPC.ai[1] = Main.rand.Next(241) + 20 * state;
						NPC.ai[2] = 1f;
						NPC.ai[3] = 2f;
						NPC.netUpdate = true;
						NPC.TargetClosest(true);

						for (int i = 0; i < 8; i++)
						{
							int fire = spawnFireBall(NPC.Center.X, NPC.Center.Y, true, 4, -((float)Math.PI / 2) + (((float)Math.PI / 4) * i));
						}
					}

					// movement
					if (NPC.Center.X > Main.player[NPC.target].Center.X + 150f)
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X = NPC.velocity.X * 0.98f;
						}
						NPC.velocity.X -= 0.1f;
						if (NPC.velocity.X > 8f)
						{
							NPC.velocity.X = 8f;
						}
					}
					else if (NPC.Center.X < Main.player[NPC.target].Center.X - 150f)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X * 0.98f;
						}
						NPC.velocity.X += 0.1f;
						if (NPC.velocity.X < -8f)
						{
							NPC.velocity.X = -8f;
						}
					}
					if (NPC.Center.Y > Main.player[NPC.target].Center.Y - 150f)
					{
						if (NPC.velocity.Y > 0f)
						{
							NPC.velocity.Y = NPC.velocity.Y * 0.98f;
						}
						NPC.velocity.Y -= 0.1f;
						if (NPC.velocity.Y > 5f)
						{
							NPC.velocity.Y = 5f;
						}
					}
					else if (NPC.Center.Y < Main.player[NPC.target].Center.Y - 250f)
					{
						if (NPC.velocity.Y < 0f)
						{
							NPC.velocity.Y = NPC.velocity.Y * 0.98f;
						}
						NPC.velocity.Y += 0.1f;
						if (NPC.velocity.Y < -5f)
						{
							NPC.velocity.Y = -5f;
						}
					}
				}
				if (NPC.ai[2] == 1f) // eye open - teleporting phase
				{
					NPC.ai[3]++;
					if (NPC.ai[3] < 60f)
					{
						if (NPC.ai[3] == 1f)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								NPC.Center = new Vector2(Main.player[NPC.target].Center.X + (100 + Main.rand.Next(101)) * ((Main.rand.NextBool(2)) ? 1 : -1), Main.player[NPC.target].Center.Y - 100 - Main.rand.Next(201));

								if (Main.rand.Next(100) < fireBallRand[2, 1])
								{
									for (int i = 0; i < 8; i++)
									{
										spawnFireBall(NPC.Center.X, NPC.Center.Y, true, 1, -((float)Math.PI / 2) + (((float)Math.PI / 4) * i));
									}
								}
								else
								{
									int num = (NPC.Center.X > Main.player[NPC.target].Center.X) ? -1 : 1;

									for (int i = 0; i < 8; i++)
									{
										float xpos = 64 + 36 * i;
										spawnFireBall(NPC.Center.X + xpos * num, NPC.Center.Y - 40f, true, 0, 1 + 5 * i);
									}
								}
								NPC.netUpdate = true;
							}
							initialTeleport = true;
						}
						NPC.alpha = Math.Max(NPC.alpha - 25, 0);
						eyeOpen = 1;
					}
					else
					{
						NPC.alpha = Math.Min(NPC.alpha + 8, 255);
						eyeOpen = 0;
						if (NPC.ai[3] > 100f)
						{
							NPC.ai[3] = 0f;
						}
					}

					if (NPC.justHit && initialTeleport) // change phase after being hit
					{
						eyeOpen = 2;
						NPC.ai[2] = 2f;
						NPC.ai[3] = 0f;
						NPC.netUpdate = true;
						initialTeleport = false;
					}

					// movement
					NPC.velocity.X = NPC.velocity.X * 0.9f;
					if (NPC.velocity.X > 0f)
					{
						NPC.velocity.X -= 0.25f;
						if (NPC.velocity.X > 4f)
						{
							NPC.velocity.X = 4f;
						}
					}
					else if (NPC.velocity.X < 0f)
					{
						NPC.velocity.X += 0.25f;
						if (NPC.velocity.X < -4f)
						{
							NPC.velocity.X = -4f;
						}
					}
					NPC.velocity.Y = NPC.velocity.Y * 0.9f;
					if (NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y -= 0.25f;
						if (NPC.velocity.Y > 4f)
						{
							NPC.velocity.Y = 4f;
						}
					}
					else if (NPC.velocity.Y < 0f)
					{
						NPC.velocity.Y += 0.25f;
						if (NPC.velocity.Y < -4f)
						{
							NPC.velocity.Y = -4f;
						}
					}
				}

				// Eye open - chase player phase
				if (NPC.ai[2] == 2f)
				{
					NPC.ai[3]++;

					// Change back to main phase after enough damage is recieved
					if (NPC.ai[3] >= (1000 - 75 * state))
					{
						eyeOpen = 0;
						NPC.alpha = Math.Min(NPC.alpha + 10, 255);
						if (NPC.alpha >= 255 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.ai[2] = 0f;
							NPC.ai[3] = 0f;
							NPC.netUpdate = true;
							NPC.velocity = new Vector2(5f - Main.rand.Next(11), 5f - Main.rand.Next(11));
							NPC.Center = new Vector2(Main.player[NPC.target].Center.X - 200 + Main.rand.Next(401), Main.player[NPC.target].Center.Y - 100 - Main.rand.Next(201));
						}
					}
					else
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC.ai[3] += (oldLife - NPC.life) / 2;
							oldLife = NPC.life;
						}

						NPC.alpha = Math.Max(NPC.alpha - 25, 0);

						// movement
						float speed = 0.2f + 0.05f * state;
						if (NPC.Center.X > Main.player[NPC.target].Center.X)
						{
							NPC.velocity.X -= speed;
						}
						else if (NPC.Center.X < Main.player[NPC.target].Center.X)
						{
							NPC.velocity.X += speed;
						}
						if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
						{
							NPC.velocity.Y -= speed;
						}
						else if (NPC.Center.Y < Main.player[NPC.target].Center.Y)
						{
							NPC.velocity.Y += speed;
						}
						float speedmax = 8f + 2f * state;
						if (NPC.velocity.X < -speedmax)
						{
							NPC.velocity.X = -speedmax;
						}
						if (NPC.velocity.X > speedmax)
						{
							NPC.velocity.X = speedmax;
						}
						if (NPC.velocity.Y < -speedmax)
						{
							NPC.velocity.Y = -speedmax;
						}
						if (NPC.velocity.Y > speedmax)
						{
							NPC.velocity.Y = speedmax;
						}
					}
				}
			}

			NPC.rotation = MathHelper.Clamp(NPC.velocity.X, -4f, 4f) / 30f;

			if (eyeOpen > 0)
			{
				NPC.damage = damage;
				NPC.dontTakeDamage = false;

				if ((eyeOpen == 1 && eyeFrame < 2) || (eyeOpen == 2 && eyeFrame < 3))
				{
					eyeFrameCounter++;
					if (eyeFrameCounter > 4)
					{
						eyeFrame++;
						eyeFrameCounter = 0;
					}
				}
				else if (eyeFrame >= 3)
				{
					if (eyeOpen == 1)
					{
						eyeFrame = 2;
					}
					else
					{
						float targetRot = (float)Math.Atan2(Main.player[NPC.target].Center.Y - (NPC.Center.Y + 22), Main.player[NPC.target].Center.X - NPC.Center.X);
						if (targetRot >= (float)(Math.PI * 2))
						{
							targetRot -= (float)(Math.PI * 2);
						}
						if (targetRot < 0)
						{
							targetRot += (float)(Math.PI * 2);
						}
						eyeFrame = (int)(3 + (float)Math.Round(7 * (targetRot / (float)(Math.PI * 2))));
					}
				}
			}
			else
			{
				NPC.dontTakeDamage = true;
				NPC.damage = 0;

				if (eyeFrame > 0)
				{
					if (eyeFrame > 2)
					{
						eyeFrame = 2;
					}
					eyeFrameCounter++;
					if (eyeFrameCounter > 4)
					{
						eyeFrame--;
						eyeFrameCounter = 0;
					}
				}
			}

			NPC.frame.Y = eyeFrame;

			NPC.frameCounter += 1;
			if (NPC.frameCounter > 8)
			{
				NPC.frame.X += frameNum;
				if (NPC.frame.X <= 0f)
				{
					frameNum = 1;
				}
				if (NPC.frame.X >= 2f)
				{
					frameNum = -1;
				}
				NPC.frameCounter = 0;
			}
		}

		int spawnFireBall(float posX, float posY, bool playSound = true, float ai1 = 0, float ai2 = 0, float ai3 = 0)
		{
			return NPC.NewNPC(NPC.GetSource_FromAI(), (int)posX, (int)posY, ModContent.NPCType<PhantoonFireBall>(), NPC.whoAmI, NPC.whoAmI, ai1, ai2, ai3, NPC.target);
		}

		/*public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if(NPC.ai[2] == 2f)
			{
				NPC.ai[3] += damage / 2;
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(NPC.ai[2] == 2f)
			{
				NPC.ai[3] += damage / 2;
			}
		}*/

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Player player = Main.player[NPC.target];

			NPC.direction = 1;
			NPC.spriteDirection = NPC.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Color color = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));

			Texture2D texMain = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Phantoon/Phantoon_Main").Value,
					texBottom = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Phantoon/Phantoon_Bottom").Value;

			// draw base
			int texH = (texMain.Height / 11);
			sb.Draw(texMain, new Vector2((int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y - Main.screenPosition.Y)), new Rectangle?(new Rectangle(0, texH * NPC.frame.Y, texMain.Width, texH)), color, NPC.rotation, new Vector2(texMain.Width / 2, 96), 1f, effects, 0f);

			// draw bottom
			int texBH = (texBottom.Height / 3);
			sb.Draw(texBottom, new Vector2((int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y - Main.screenPosition.Y)), new Rectangle?(new Rectangle(0, texBH * NPC.frame.X, texBottom.Width, texBH)), color, NPC.rotation, new Vector2(texBottom.Width / 2, -54), 1f, effects, 0f);

			return false;
		}
	}
}
