using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

using MetroidMod.Common.Systems;

namespace MetroidMod.Content.NPCs.Kraid
{
	[AutoloadBossHead]
	public class Kraid_Head : ModNPC
	{
		public override string BossHeadTexture => Texture + "_Head_Boss_1";
		public const string KraidHead = "MetroidMod/Content/NPCs/Kraid/Kraid_Head_Head_Boss_";

		public override void Load()
		{
			for (int k = 0; k <= 3; k++)
			{
				if (k == 1) { continue; }
				Mod.AddBossHeadTexture(KraidHead + k);
			}
		}
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Kraid");
			Main.npcFrameCount[Type] = 6;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCID.Sets.SpecificDebuffImmunity[Type][20] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][24] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][31] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][39] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 188;
			NPC.height = 102;
			NPC.scale = 1f;
			NPC.damage = 40;
			NPC.defense = 500;
			NPC.lifeMax = 6000;
			NPC.dontTakeDamage = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = Sounds.NPCs.KraidRoar;//SoundID.NPCDeath5;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = Item.buyPrice(0, 0, 7, 0);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.behindTiles = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 5;
			NPC.boss = true;
			//BossBag = mod.ItemType("KraidBag");
			if (Main.netMode != NetmodeID.Server) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Kraid"); }
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				new FlavorTextBestiaryInfoElement("This invasive species made its way on this planet after the Gizzard tribe had brought it to the Terrarian Planet to train young warriors. It is extremely bulky and slow, but can shoot projectiles from its stomach. It's hide is almost impenetrable save for even the hottest lava. But these creatures are not indestructible on the inside. Give it a taste of pain when the mouth opens!")
			});
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * balance) + 1;
			NPC.damage = (int)(NPC.damage * 0.7f);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.KraidBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.KraidTissue>(), 1, 20, 31));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.UnknownPlasmaBeam>()));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.KraidPhantoonMusicBox>(), 6));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.KraidMask>(), 8));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.KraidTrophy>(), 11));

			npcLoot.Add(notExpertRule);
		}
		public override void OnKill()
		{
			MSystem.bossesDown |= MetroidBossDown.downedKraid;
			Main.BestiaryTracker.Kills.RegisterKill(ModContent.GetInstance<Kraid_ArmBack>().NPC);
			Main.BestiaryTracker.Kills.RegisterKill(ModContent.GetInstance<Kraid_ArmFront>().NPC);
			Main.BestiaryTracker.Kills.RegisterKill(ModContent.GetInstance<Kraid_Body>().NPC);
		}


		int state = 0;
		bool mouthOpen = false;
		int moveCounter = 0;
		int headAnim = 1;
		float roarFrame = 0f;
		int roarAnim = 1;
		int direction = 1;

		private int _body, _armFront, _armBack;
		NPC Body
		{
			get { return Main.npc[_body]; }
		}
		NPC ArmFront
		{
			get { return Main.npc[_armFront]; }
		}
		NPC ArmBack
		{
			get { return Main.npc[_armBack]; }
		}

		public override int SpawnNPC(int tileX, int tileY)
		{
			NPC.direction = 1;
			NPC.spriteDirection = 1;

			int spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);
			int spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);
			int num11 = (int)(Main.player[NPC.target].position.X / 16f) - spawnRangeX;
			int num12 = (int)(Main.player[NPC.target].position.X / 16f) + spawnRangeX;
			int num13 = (int)(Main.player[NPC.target].position.Y / 16f) - spawnRangeY;
			int num14 = (int)(Main.player[NPC.target].position.Y / 16f) + spawnRangeY;
			Main.NewText("Spawning Kraid!");
			return NPC.NewNPC(null, (int)MathHelper.Clamp(tileX,num11,num12) * 16 + 8, (int)MathHelper.Clamp(tileY,num13,num14) * 16, NPC.type);
		}

		public override void AI()
		{
			if (NPC.life < (int)(NPC.lifeMax * 0.75f))
				state = 1;
			if (NPC.life < (int)(NPC.lifeMax * 0.5f))
				state = 2;
			if (NPC.life < (int)(NPC.lifeMax * 0.25f))
				state = 3;

			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (!player.dead)
			{
				NPC.timeLeft = 60;
			}
			if (!player.active || player.dead)
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (!player.active || player.dead)
				{
					NPC.position.Y += 10;
					if(NPC.ai[3] > 0)
					{
						NPC.ai[3]++;
						if(NPC.ai[3] > 180)
						{
							NPC.life = 0;
							NPC.active = false;
						}
					}
				}
			}
			else if(NPC.ai[3] > 1)
			{
				NPC.ai[3] = 1;
			}

			// Just spawned, spawn limbs.
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (NPC.ai[3] == 0)
				{					
					_body = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + 29 * NPC.direction), (int)(NPC.position.Y + 223), ModContent.NPCType<Kraid_Body>(), NPC.whoAmI);
					Body.position += new Vector2(0, (float)Body.height / 2);
					Body.realLife = NPC.whoAmI;
					Body.ai[0] = NPC.whoAmI;

					_armFront = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + 42 * NPC.direction), (int)(NPC.position.Y + 131), ModContent.NPCType<Kraid_ArmFront>(), NPC.whoAmI);
					ArmFront.position += new Vector2(0, (float)ArmFront.height / 2);
					ArmFront.realLife = NPC.whoAmI;
					ArmFront.ai[0] = NPC.whoAmI;

					_armBack = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + 234 * NPC.direction), (int)(NPC.position.Y + 79), ModContent.NPCType<Kraid_ArmBack>(), NPC.whoAmI);
					ArmBack.position += new Vector2((float)ArmBack.width / 2, (float)ArmBack.height);
					ArmBack.realLife = NPC.whoAmI;
					ArmBack.ai[0] = NPC.whoAmI;

					NPC.ai[3] = 1;
					Body.netUpdate = ArmFront.netUpdate = ArmBack.netUpdate = true;
				}
			}

			NPC.ai[1]++;
			if(NPC.ai[1] >= 180 || NPC.frameCounter > 0 || NPC.frame.Y > 0 || roarCounter > 0 || mouthOpen)
			{
				NPC.frameCounter += 1;
				if(NPC.frameCounter >= 5)
				{
					NPC.frame.Y += headAnim;
					if(NPC.frame.Y >= 2)
					{
						NPC.frame.Y = 2;
						if(roarCounter > 0 && !mouthOpen)
						{
							headAnim = 1;
						}
						else
						{
							headAnim = -1;
						}
					}
					if(NPC.frame.Y <= 0)
					{
						NPC.frame.Y = 0;
						headAnim = 1;
					}
					NPC.frameCounter = 0;
				}
				NPC.ai[1] = 0;
			}
			
			if(mouthOpen)
			{
				roarFrame += roarAnim;
				if(roarFrame >= 4)
				{
					roarFrame = 4;
					roarAnim = -1;
				}
				if(roarFrame <= 0)
				{
					roarFrame = 0;
					roarAnim = 1;
				}
			}
			else
			{
				roarFrame = 0;
				roarAnim = 1;
			}

			if(fullAnim > 0)
			{
				fullAnim--;
			}


			if(NPC.justHit)
			{
				if((NPC.direction == 1 && player.Center.X >= NPC.Center.X) || (NPC.direction == -1 && player.Center.X <= NPC.Center.X))
				{
					if(NPC.ai[0] <= 0 && roarCounter <= 0)
					{
						NPC.ai[0] = 1;
						NPC.netUpdate = true;
					}
					if(mouthOpen)
					{
						if(ArmBack.ai[1] <= 0 && state > 0)
						{
							ArmBack.ai[1]++;
							ArmBack.netUpdate = true;
						}
					}
				}
			}
			
			bool flag = false;
			if(NPC.ai[0] > 0)
			{
				NPC.ai[0] += 1;
				if(NPC.ai[0] <= 150)
				{
					flag = true;
				}
				else
				{
					NPC.ai[0] = 0;
				}
			}
			this.Roar(flag);
			if(mouthOpen)
			{
				NPC.HitSound = Sounds.NPCs.KraidHit;
			}
			else
			{
				NPC.HitSound = SoundID.NPCHit1;
			}

			int dir = 0;
			if(player.Center.X <= NPC.Center.X+512 && player.Center.X >= NPC.Center.X-512)
			{
				moveCounter += 1;
				if(moveCounter > 180 && moveDir == 0)
				{
					dir = 1;
					if(Main.rand.Next(4) >= 1)
					{
						dir = -1;
					}
					moveCounter = Main.rand.Next(141);
				}
				dir *= NPC.direction;
			}
			else
			{
				moveCounter = 0;
				if(player.Center.X > NPC.Center.X)
				{
					dir = 1;
				}
				if(player.Center.X < NPC.Center.X)
				{
					dir = -1;
				}
			}
			this.Move(dir);

			if(direction == 1 && player.Center.X < NPC.position.X)
			{
				direction = -1;
			}
			if(direction == -1 && player.Center.X > NPC.position.X+NPC.width)
			{
				direction = 1;
			}
			NPC.direction = direction;

			if(player.Center.Y < Body.position.Y)// && ((NPC.direction == 1 && player.position.X <= (ArmBack.position.X+ArmBack.width)) || (NPC.direction == -1 && (player.position.X+player.width) >= ArmBack.position.X)))
			{
				NPC.ai[2] += 1f;
			}
			else
			{
				NPC.ai[2] -= 0.5f;
			}
			if(NPC.ai[2] > 200)
			{
				if(ArmBack != null && ArmBack.ai[1] <= 0)
				{
					ArmBack.ai[1]++;
				}
				NPC.ai[2] = 0;
			}

			int heightOffset = 64;
			Vector2 position3 = new Vector2(Body.position.X, Body.position.Y + Body.height - heightOffset);
			//if (NPC.position.X < player.position.X && NPC.position.X + (float)NPC.width > player.position.X + (float)player.width && NPC.position.Y + (float)NPC.height < player.position.Y + (float)player.height - 16f)
			if(player.position.Y > NPC.position.Y+NPC.height && Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
			{
				//NPC.velocity.Y = NPC.velocity.Y + 0.5f;
				if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y = 0f;
				}
				if (NPC.velocity.Y < 0.2f)
				{
					NPC.velocity.Y = NPC.velocity.Y + 0.025f;
				}
				else
				{
					NPC.velocity.Y = NPC.velocity.Y + 0.2f;
				}
				if (NPC.velocity.Y > 2f)
				{
					NPC.velocity.Y = 2f;
				}
			}
			else
			{
				int numTiles = 0;
				for(int i = 0; i < 20; i++)
				{
					Vector2 position4 = new Vector2(Body.position.X+((Body.width/20)*i),position3.Y);
					if(Collision.SolidCollision(position4, Body.width/20, heightOffset))
					{
						numTiles++;
					}
				}
				//if (Collision.SolidCollision(position3, Body.width, num897))
				if(numTiles >= 15)
				{
					if (NPC.velocity.Y > 0f)
					{
						NPC.velocity.Y = 0f;
					}
					if (NPC.velocity.Y > -0.2f)
					{
						NPC.velocity.Y = NPC.velocity.Y - 0.025f;
					}
					else
					{
						NPC.velocity.Y = NPC.velocity.Y - 0.2f;
					}
					if (NPC.velocity.Y < -2f)
					{
						NPC.velocity.Y = -2f;
					}
				}
				else
				{
					if (NPC.velocity.Y < 0f)
					{
						NPC.velocity.Y = 0f;
					}
					if (NPC.velocity.Y < 0.1f)
					{
						NPC.velocity.Y = NPC.velocity.Y + 0.025f;
					}
					else
					{
						NPC.velocity.Y = NPC.velocity.Y + 0.5f;
					}
				}
			}
			if (NPC.velocity.Y > 10f)
			{
				NPC.velocity.Y = 10f;
			}
		}

		/*public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if((NPC.direction == 1 && player.Center.X >= NPC.Center.X) || (NPC.direction == -1 && player.Center.X <= NPC.Center.X))
			{
				if(NPC.ai[0] <= 0 && roarCounter <= 0)
				{
					NPC.ai[0] = 1;
					NPC.netUpdate = true;
				}
				if(mouthOpen)
				{
					if(ArmBack.ai[1] <= 0 && state > 0)
					{
						ArmBack.ai[1]++;
						ArmBack.netUpdate = true;
					}
				}
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if((NPC.direction == 1 && projectile.Center.X >= NPC.Center.X) || (NPC.direction == -1 && projectile.Center.X <= NPC.Center.X))
			{
				if(NPC.ai[0] <= 0 && roarCounter <= 0)
				{
					NPC.ai[0] = 1;
					NPC.netUpdate = true;
				}
				if(mouthOpen && projectile.Center.Y > NPC.position.Y)
				{
					if(ArmBack.ai[1] <= 0 && state > 0)
					{
						ArmBack.ai[1]++;
						ArmBack.netUpdate = true;
					}
				}
			}
		}*/
		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage += (int)(NPC.defense * 0.95f * 0.5f);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if(mouthOpen && projectile.Center.Y > NPC.position.Y && ((NPC.direction == 1 && projectile.Center.X >= NPC.Center.X) || (NPC.direction == -1 && projectile.Center.X <= NPC.Center.X)))
			{
				modifiers.FinalDamage += (int)(NPC.defense * 0.95f * 0.5f);
			}
		}
		
		Vector2 headOffset = Vector2.Zero;
		int roarCounter = 0;
		void Roar(bool roaring)
		{
			if(roaring)
			{
				if(roarCounter < 10)
				{
					headOffset.X = Math.Max(headOffset.X-0.6f,-6f);
				}
				else if(roarCounter > 15)
				{
					headOffset.X = Math.Min(headOffset.X+1.7f,26f);
					headOffset.Y = Math.Max(headOffset.Y-0.7f,-10f);
				}
				if(roarCounter == 29)
				{
					SoundEngine.PlaySound(Sounds.NPCs.KraidRoar, NPC.Center);
				}
				if(roarCounter >= 30)
				{
					mouthOpen = true;
					NPC.frame.X = 1;
				}
				roarCounter = Math.Min(roarCounter+1,30);
			}
			else
			{
				if(headOffset.X > 0)
				{
					headOffset.X = Math.Max(headOffset.X-1.7f,0f);
				}
				if(headOffset.X < 0)
				{
					headOffset.X = Math.Min(headOffset.X+1.7f,0f);
				}
				if(headOffset.Y > 0)
				{
					headOffset.Y = Math.Max(headOffset.Y-0.7f,0f);
				}
				if(headOffset.Y < 0)
				{
					headOffset.Y = Math.Min(headOffset.Y+0.7f,0f);
				}
				roarCounter = Math.Max(roarCounter-1,0);
				mouthOpen = false;
				NPC.frame.X = 0;
			}
		}

		int moveDir = 0;
		int stepCounter = 0;

		Vector2 bLegPos = new Vector2(8f,0f);
		Vector2 fLegPos = new Vector2(-8f,0f);
		int currentLeg = 1;

		Vector2 bLegPrevPos = new Vector2(8f,0f);
		Vector2 fLegPrevPos = new Vector2(-8f,0f);

		void Move(int direction)
		{
			Vector2 actualBLegPos = Body.Center + new Vector2(138*NPC.direction,174) + bLegPos;
			Vector2 actualFLegPos = Body.Center + new Vector2(-68*NPC.direction,174) + fLegPos;
			if(moveDir == 0)
			{
				NPC.velocity.X = 0f;
				moveDir = direction;
			}
			else
			{
				stepCounter++;
				if(moveDir == 1)
				{
					if(currentLeg == 1)
					{
						if(fLegPos.X < 8f)
						{
							NPC.velocity.X = 1f;
							fLegPos.Y = Math.Max(fLegPos.Y-1f,-8f);
						}
						else
						{
							NPC.velocity.X = 0f;
							fLegPos.Y = Math.Min(fLegPos.Y+2f,0f);
						}
						if(fLegPos.Y == 0f && fLegPrevPos.Y < 0f)
						{
							this.stomp(actualFLegPos);
						}
						fLegPos.X = Math.Min(fLegPos.X+1f,8f);
						bLegPos.X = Math.Max(bLegPos.X-1f,-8f);
						if(bLegPos.X == -8f && bLegPos.Y == 0f && fLegPos.X == 8f && fLegPos.Y == 0f)
						{
							if(stepCounter >= 16)
							{
								currentLeg = -1;
								stepCounter = 0;
								moveDir = 0;
							}
						}
					}
					else if(currentLeg == -1)
					{
						if(bLegPos.X < 8f)
						{
							NPC.velocity.X = 1f;
							bLegPos.Y = Math.Max(bLegPos.Y-1f,-8f);
						}
						else
						{
							NPC.velocity.X = 0f;
							bLegPos.Y = Math.Min(bLegPos.Y+2f,0f);
						}
						if(bLegPos.Y == 0f && bLegPrevPos.Y < 0f)
						{
							this.stomp(actualBLegPos);
						}
						bLegPos.X = Math.Min(bLegPos.X+1f,8f);
						fLegPos.X = Math.Max(fLegPos.X-1f,-8f);
						if(fLegPos.X == -8f && fLegPos.Y == 0f && bLegPos.X == 8f && bLegPos.Y == 0f)
						{
							if(stepCounter >= 16)
							{
								currentLeg = 1;
								stepCounter = 0;
								moveDir = 0;
							}
						}
					}
				}
				else if(moveDir == -1)
				{
					if(currentLeg == 1)
					{
						if(fLegPos.X > -8f)
						{
							NPC.velocity.X = -1f;
							fLegPos.Y = Math.Max(fLegPos.Y-1f,-8f);
						}
						else
						{
							NPC.velocity.X = 0f;
							fLegPos.Y = Math.Min(fLegPos.Y+2f,0f);
						}
						if(fLegPos.Y == 0f && fLegPrevPos.Y < 0f)
						{
							this.stomp(actualFLegPos);
						}
						fLegPos.X = Math.Max(fLegPos.X-1f,-8f);
						bLegPos.X = Math.Min(bLegPos.X+1f,+8f);
						if(bLegPos.X == 8f && bLegPos.Y == 0f && fLegPos.X == -8f && fLegPos.Y == 0f)
						{
							if(stepCounter >= 16)
							{
								currentLeg = -1;
								stepCounter = 0;
								moveDir = 0;
							}
						}
					}
					else if(currentLeg == -1)
					{
						if(bLegPos.X > -8f)
						{
							NPC.velocity.X = -1f;
							bLegPos.Y = Math.Max(bLegPos.Y-1f,-8f);
						}
						else
						{
							NPC.velocity.X = 0f;
							bLegPos.Y = Math.Min(bLegPos.Y+2f,0f);
						}
						if(bLegPos.Y == 0f && bLegPrevPos.Y < 0f)
						{
							this.stomp(actualBLegPos);
						}
						bLegPos.X = Math.Max(bLegPos.X-1f,-8f);
						fLegPos.X = Math.Min(fLegPos.X+1f,8f);
						if(fLegPos.X == 8f && fLegPos.Y == 0f && bLegPos.X == -8f && bLegPos.Y == 0f)
						{
							if(stepCounter >= 16)
							{
								currentLeg = 1;
								stepCounter = 0;
								moveDir = 0;
							}
						}
					}
				}
			}
		}
		void stomp(Vector2 pos)
		{
			for (int num70 = 0; num70 < 25; num70++)
			{
				int dust = Dust.NewDust(new Vector2(pos.X-76f,pos.Y), 152, 4, 30, 0, 0, 100, default(Color), 2f);
				Main.dust[dust].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.Item1, Body.Center);

			fullAnim = 6;
			fullOffset.Y = 2f;
		}

		Vector2[] gorePosition = new Vector2[12];

		Vector2 fullOffset = Vector2.Zero;
		
		int fullAnim = 0;
		
		float headRot = 0f;

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Player player = Main.player[NPC.target];

			NPC.spriteDirection = NPC.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Color buffColor = Color.White;//Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
			/*if (NPC.behindTiles)
			{
				int num44 = (int)((Body.position.X - 8f) / 16f);
				int num45 = (int)((Body.position.X + (float)Body.width + 8f) / 16f);
				int num46 = (int)((NPC.position.Y - 8f) / 16f);
				int num47 = (int)((Body.position.Y + (float)Body.height + 8f) / 16f);
				for (int m = num44; m <= num45; m++)
				{
					for (int n = num46; n <= num47; n++)
					{
						if (Lighting.Brightness(m, n) == 0f)
						{
							buffColor = Color.Black;
						}
					}
				}
			}*/
			Color alpha2 = NPC.GetAlpha(buffColor);

			Texture2D texHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Head").Value,
				texJaw = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Jaw").Value,
				texNeck = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Neck").Value,
				texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Body").Value,
				texBodyOverlay = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_BodyOverlay").Value,
				texLegs = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Legs").Value,
				texArm1 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Arm1").Value,
				texArm2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Arm2").Value,
				texArmFront = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_ArmFront").Value,
				texArmBack = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_ArmBack").Value;
			if(state > 0)
			{
				texHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Head_"+state).Value;
				texJaw = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Jaw_"+state).Value;
				texNeck = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Neck_"+state).Value;
				texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Body_"+state).Value;
				texBodyOverlay = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_BodyOverlay_"+state).Value;
				texLegs = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Legs_"+state).Value;
				texArm1 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Arm1_"+state).Value;
				texArm2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_Arm2_"+state).Value;
				texArmFront = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_ArmFront_"+state).Value;
				texArmBack = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Kraid/Kraid_ArmBack_"+state).Value;
			}


			if (Body == null || ArmBack == null || ArmFront == null)
				return (false);

			Vector2 backArm1Pos = NPC.Center + new Vector2(37*NPC.direction,40) + (ArmBack.Center-(NPC.Center+new Vector2(234*NPC.direction,79)))*0.25f;
			sb.Draw(texArm1,backArm1Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm1.Width,texArm1.Height)),alpha2,0f,new Vector2(texArm1.Width/2,texArm1.Height/2),1f,effects,0f);
			gorePosition[0] = backArm1Pos;

			Vector2 bvec1 = new Vector2(-94,42);
			float bveclength = Vector2.Distance(Vector2.Zero,bvec1);
			float bvecrot = (float)Math.Atan2(bvec1.Y,bvec1.X)+ArmBack.rotation;
			Vector2 bvec2 = new Vector2((float)Math.Cos(bvecrot)*bveclength,(float)Math.Sin(bvecrot)*bveclength);
			Vector2 bArm2Pos1 = backArm1Pos+new Vector2(0f,30f),
					bArm2Pos2 = ArmBack.Center+new Vector2(bvec2.X*NPC.direction,bvec2.Y);
			Vector2 backArm2Pos = Vector2.Lerp(bArm2Pos1,bArm2Pos2,0.5f);
			float bArmRot = (float)Math.Atan2((bArm2Pos2.Y-bArm2Pos1.Y)*NPC.direction,(bArm2Pos2.X-bArm2Pos1.X)*NPC.direction) - ((float)Math.PI*0.375f)*NPC.direction;
			sb.Draw(texArm2,backArm2Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm2.Width,texArm2.Height)),alpha2,bArmRot,new Vector2(texArm2.Width/2,texArm2.Height/2),1f,effects,0f);
			gorePosition[1] = backArm2Pos;

			Vector2 bOrigin = new Vector2(109,80);
			if(NPC.direction == -1)
			{
				bOrigin.X = (float)texArmBack.Width - bOrigin.X;
			}
			Vector2 armBackPos = fullOffset + ArmBack.Center + new Vector2(-(float)(ArmBack.width/2)*NPC.direction,(float)ArmBack.height/2 - 14f);
			sb.Draw(texArmBack,armBackPos - Main.screenPosition,new Rectangle?(new Rectangle(0,(texArmBack.Height/6)*ArmBack.frame.Y,texArmBack.Width,texArmBack.Height/6)),alpha2,ArmBack.rotation*NPC.direction,bOrigin,1f,effects,0f);
			gorePosition[2] = armBackPos;


			Vector2 backLegPos = Body.Center + new Vector2((62+texLegs.Width/4)*NPC.direction,2) + bLegPos;
			sb.Draw(texLegs,backLegPos - Main.screenPosition,new Rectangle?(new Rectangle(texLegs.Width/2,0,texLegs.Width/2,texLegs.Height)),alpha2,0f,new Vector2(texLegs.Width/4,0),1f,effects,0f);
			gorePosition[3] = backLegPos;

			Vector2 bodyPos = fullOffset + Body.Center - new Vector2(117*NPC.direction,38);
			sb.Draw(texBody,bodyPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texBody.Width,texBody.Height)),alpha2,0f,new Vector2(texBody.Width/2,texBody.Height/2),1f,effects,0f);
			gorePosition[4] = bodyPos;
			
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<Projectiles.Boss.KraidBellySpike>() && Main.projectile[i].ai[0] == NPC.whoAmI && Main.projectile[i].localAI[0] <= 35)
				{
					Projectile projectile = Main.projectile[i];
					SpriteEffects effects2 = SpriteEffects.None;
					if (projectile.spriteDirection == -1)
					{
						effects2 = SpriteEffects.FlipHorizontally;
					}
					Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
					int num108 = tex.Height / Main.projFrames[projectile.type];
					int y4 = num108 * projectile.frame;
					sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)projectile.height/2f), projectile.scale, effects2, 0f);
				}
			}
			
			Vector2 bodyOvPos = fullOffset + Body.Center + new Vector2(85*NPC.direction,-7);
			sb.Draw(texBodyOverlay,bodyOvPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texBodyOverlay.Width,texBodyOverlay.Height)),alpha2,0f,new Vector2(texBodyOverlay.Width/2,texBodyOverlay.Height/2),1f,effects,0f);

			Vector2 frontLegPos = Body.Center + new Vector2((-144+texLegs.Width/4)*NPC.direction,2) + fLegPos;
			sb.Draw(texLegs,frontLegPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texLegs.Width/2,texLegs.Height)),alpha2,0f,new Vector2(texLegs.Width/4,0),1f,effects,0f);
			gorePosition[5] = frontLegPos;

			bLegPrevPos = bLegPos;
			fLegPrevPos = fLegPos;


			float targetrotation = (float)Math.Atan2((player.Center.Y-NPC.Center.Y)*NPC.direction,(player.Center.X-NPC.Center.X)*NPC.direction);
			if(player.active && !mouthOpen)
			{
				headRot = targetrotation * 0.3f;
				if (headRot < -0.15f)
				{
					headRot = -0.15f;
				}
				if (headRot > 0.15f)
				{
					headRot = 0.15f;
				}
			}
			/*else if(Math.Abs(headRot) > 0.05f)
			{
				headRot *= 0.9f;
			}*/
			else
			{
				headRot = 0f;
			}
			
			Vector2 hOffset = new Vector2(headOffset.X*NPC.direction,headOffset.Y);

			Vector2 headPos = fullOffset + NPC.Center + new Vector2((29-texNeck.Width/2)*NPC.direction,-9) + new Vector2((float)Math.Max(Math.Ceiling(headOffset.X*0.5f),0f)*NPC.direction,(float)Math.Floor(headOffset.Y*0.5f));
			if(mouthOpen)
			{
				headPos.X += roarFrame*NPC.direction;
			}
			sb.Draw(texNeck,headPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texNeck.Width,texNeck.Height)),alpha2,0f,new Vector2(texNeck.Width/2,46),1f,effects,0f);
			gorePosition[6] = headPos;

			headPos = fullOffset + NPC.Center + new Vector2(29*NPC.direction,-9) + hOffset;
			Vector2 hpos1 = headPos;
			if(mouthOpen)
			{
				hpos1 += new Vector2(roarFrame*NPC.direction,roarFrame);
			}
			sb.Draw(texJaw,hpos1 - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texJaw.Width,texJaw.Height)),alpha2,headRot,new Vector2(texJaw.Width/2,texJaw.Height/2),1f,effects,0f);
			gorePosition[7] = hpos1;
			Vector2 hpos2 = headPos;
			if(mouthOpen)
			{
				hpos2 += new Vector2(roarFrame*NPC.direction,-roarFrame);
			}
			sb.Draw(texHead,hpos2 - Main.screenPosition,new Rectangle?(new Rectangle(0,(texHead.Height/6)*NPC.frame.Y+(texHead.Height/2)*NPC.frame.X,texHead.Width,texHead.Height/6)),alpha2,headRot,new Vector2(texHead.Width/2,texHead.Height/12),1f,effects,0f);
			gorePosition[8] = hpos2;


			Vector2 frontArm1Pos = NPC.Center + new Vector2(-65*NPC.direction,78) + (ArmFront.Center-(NPC.Center+new Vector2(42*NPC.direction,131)))*0.25f;
			sb.Draw(texArm1,frontArm1Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm1.Width,texArm1.Height)),alpha2,0f,new Vector2(texArm1.Width/2,texArm1.Height/2),1f,effects,0f);
			gorePosition[9] = frontArm1Pos;

			Vector2 vec1 = new Vector2(-92,62);
			float veclength = Vector2.Distance(Vector2.Zero,vec1);
			float vecrot = (float)Math.Atan2(vec1.Y,vec1.X)+ArmFront.rotation;
			Vector2 vec2 = new Vector2((float)Math.Cos(vecrot)*veclength,(float)Math.Sin(vecrot)*veclength);
			Vector2 fArm2Pos1 = frontArm1Pos,
					fArm2Pos2 = ArmFront.Center+new Vector2(vec2.X*NPC.direction,vec2.Y);
			float fArmRot = (float)Math.Atan2((fArm2Pos2.Y-fArm2Pos1.Y)*NPC.direction,(fArm2Pos2.X-fArm2Pos1.X)*NPC.direction) - ((float)Math.PI/2)*NPC.direction;
			Vector2 frontArm2Pos = Vector2.Lerp(fArm2Pos1,fArm2Pos2,0.5f);
			sb.Draw(texArm2,frontArm2Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm2.Width,texArm2.Height)),alpha2,fArmRot,new Vector2(texArm2.Width/2,texArm2.Height/2),1f,effects,0f);
			gorePosition[10] = frontArm2Pos;

			Vector2 fOrigin = new Vector2(106,63);
			if(NPC.direction == -1)
			{
				fOrigin.X = (float)texArmFront.Width - fOrigin.X;
			}
			Vector2 armFrontPos = fullOffset + ArmFront.Center;
			sb.Draw(texArmFront,armFrontPos - Main.screenPosition,new Rectangle?(new Rectangle(0,(texArmFront.Height/5)*ArmFront.frame.Y,texArmFront.Width,texArmFront.Height/5)),alpha2,ArmFront.rotation*NPC.direction,fOrigin,1f,effects,0f);
			gorePosition[11] = armFrontPos;
			
			if(fullAnim <= 0)
			{
				fullOffset = Vector2.Zero;
			}
			
			
			Texture2D rect = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value;
			
			int num44 = (int)((Body.position.X - 240f) / 16f);
			int num45 = (int)((Body.position.X + (float)Body.width + 240f) / 16f);
			int num46 = (int)((NPC.position.Y - 32f) / 16f);
			int num47 = (int)((Body.position.Y + (float)Body.height + 16f) / 16f);
			for (int m = num44; m <= num45; m++)
			{
				for (int n = num46; n <= num47; n++)
				{
					Tile tile1 = Main.tile[m,n],
						tile2 = Main.tile[m,n-1],
						tile3 = Main.tile[m,n+1],
						tile4 = Main.tile[m-1,n],
						tile5 = Main.tile[m-1,n-1],
						tile6 = Main.tile[m-1,n+1],
						tile7 = Main.tile[m+1,n],
						tile8 = Main.tile[m+1,n-1],
						tile9 = Main.tile[m+1,n+1];
					if (tile1 != null && tile1.HasTile && Main.tileSolid[(int)tile1.TileType] && !Main.tileSolidTop[(int)tile1.TileType] &&
						tile2 != null && tile2.HasTile && Main.tileSolid[(int)tile2.TileType] && !Main.tileSolidTop[(int)tile2.TileType] &&
						tile3 != null && tile3.HasTile && Main.tileSolid[(int)tile3.TileType] && !Main.tileSolidTop[(int)tile3.TileType] &&
						tile4 != null && tile4.HasTile && Main.tileSolid[(int)tile4.TileType] && !Main.tileSolidTop[(int)tile4.TileType] &&
						tile5 != null && tile5.HasTile && Main.tileSolid[(int)tile5.TileType] && !Main.tileSolidTop[(int)tile5.TileType] &&
						tile6 != null && tile6.HasTile && Main.tileSolid[(int)tile6.TileType] && !Main.tileSolidTop[(int)tile6.TileType] &&
						tile7 != null && tile7.HasTile && Main.tileSolid[(int)tile7.TileType] && !Main.tileSolidTop[(int)tile7.TileType] &&
						tile8 != null && tile8.HasTile && Main.tileSolid[(int)tile8.TileType] && !Main.tileSolidTop[(int)tile8.TileType] &&
						tile9 != null && tile9.HasTile && Main.tileSolid[(int)tile9.TileType] && !Main.tileSolidTop[(int)tile9.TileType])
					{
						sb.Draw(rect,new Rectangle(m*16-(int)Main.screenPosition.X,n*16-(int)Main.screenPosition.Y,16,16),Color.Black);
					}
					
					float num = Lighting.Brightness(m, n);
					//if(num < 1f)
					if(num <= 0f)
					{
						Color color = Color.Black;
						color *= 1f-num;
						sb.Draw(rect,new Rectangle(m*16-(int)Main.screenPosition.X,n*16-(int)Main.screenPosition.Y,16,16),color);
					}
				}
			}

			return false;
		}
		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(KraidHead + state);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int m = 0; m < (NPC.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
					if (NPC.life <= 0 && m % 2 == 0)
					{
						Main.dust[dustID].noGravity = true;
					}
				}

				if (NPC.life <= 0)
				{
					int[] mapped_gore = new int[12] { 4, 5, 7, 9, 0, 8, 3, 2, 1, 4, 5, 6 };
					var entitySource = NPC.GetSource_Death();
					for(int i = 0; i < gorePosition.Length; i++)
					{
						if (i == 4) continue;

						string goreindex = "KraidGore" + mapped_gore[i];
						int gore = Gore.NewGore(entitySource, gorePosition[i],new Vector2(Main.rand.Next(-5,5),Main.rand.Next(-5,5)),Mod.Find<ModGore>(goreindex).Type,1f);
						Main.gore[gore].timeLeft = 30;
						Main.gore[gore].rotation = 0;
					}
					SoundEngine.PlaySound(SoundID.NPCDeath1,NPC.position);

					/*for (int num70 = 0; num70 < 25; num70++)
					{
						int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)NPC.position.X,(int)NPC.position.Y,14);*/
				}
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)this._body);
			writer.Write((byte)this._armBack);
			writer.Write((byte)this._armFront);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			this._body = reader.ReadByte();
			this._armBack = reader.ReadByte();
			this._armFront = reader.ReadByte();
		}
	}
}
