using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria.ID;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MetroidMod.NPCs.Kraid
{
	[AutoloadBossHead]
    public class Kraid_Head : ModNPC
    {
		public override string BossHeadTexture => Texture + "_Head_Boss_1";
		public const string KraidHead = "MetroidMod/NPCs/Kraid/Kraid_Head_Head_Boss_";

		public override bool Autoload(ref string name)
		{
			for (int k = 0; k <= 3; k++)
			{
				mod.AddBossHeadTexture(KraidHead + k);
			}
			return base.Autoload(ref name);
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
			Main.npcFrameCount[npc.type] = 6;
		}
		public override void SetDefaults()
		{
			npc.width = 188;
			npc.height = 102;
			npc.scale = 1f;
			npc.damage = 40;
			npc.defense = 500;
			npc.lifeMax = 6000;
			npc.dontTakeDamage = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KraidRoarSound");//SoundID.NPCDeath5;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.value = Item.buyPrice(0, 0, 7, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.behindTiles = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 5;
			npc.boss = true;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			bossBag = mod.ItemType("KraidBag");
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Kraid");
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale) + 1;
			npc.damage = (int)(npc.damage * 0.7f);
		}
		public override void NPCLoot()
		{
			MWorld.bossesDown |= MetroidBossDown.downedKraid;
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KraidTissue"), Main.rand.Next(20, 31));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UnknownPlasmaBeam"));
				
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KraidPhantoonMusicBox"));
				}
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KraidMask"));
				}
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KraidTrophy"));
				}
			}
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
			npc.direction = 1;
			npc.spriteDirection = 1;

			int spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);
			int spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);
			int num11 = (int)(Main.player[npc.target].position.X / 16f) - spawnRangeX;
			int num12 = (int)(Main.player[npc.target].position.X / 16f) + spawnRangeX;
			int num13 = (int)(Main.player[npc.target].position.Y / 16f) - spawnRangeY;
			int num14 = (int)(Main.player[npc.target].position.Y / 16f) + spawnRangeY;
			Main.NewText("Spawning Kraid!");
			return NPC.NewNPC((int)MathHelper.Clamp(tileX,num11,num12) * 16 + 8, (int)MathHelper.Clamp(tileY,num13,num14) * 16, npc.type);
		}

		public override void AI()
		{
			if (npc.life < (int)(npc.lifeMax * 0.75f))
				state = 1;
			if (npc.life < (int)(npc.lifeMax * 0.5f))
				state = 2;
			if (npc.life < (int)(npc.lifeMax * 0.25f))
				state = 3;

			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			if (!player.dead)
			{
				npc.timeLeft = 60;
			}
			if (!player.active || player.dead)
			{
				npc.TargetClosest(true);
				player = Main.player[npc.target];
				if (!player.active || player.dead)
				{
					npc.position.Y += 10;
				}
			}

			// Just spawned, spawn limbs.
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (npc.ai[3] == 0)
				{					
					_body = NPC.NewNPC((int)(npc.position.X + 29 * npc.direction), (int)(npc.position.Y + 223), mod.NPCType("Kraid_Body"), npc.whoAmI);
					Body.position += new Vector2(0, (float)Body.height / 2);
					Body.realLife = npc.whoAmI;
					Body.ai[0] = npc.whoAmI;

					_armFront = NPC.NewNPC((int)(npc.position.X + 42 * npc.direction), (int)(npc.position.Y + 131), mod.NPCType("Kraid_ArmFront"), npc.whoAmI);
					ArmFront.position += new Vector2(0, (float)ArmFront.height / 2);
					ArmFront.realLife = npc.whoAmI;
					ArmFront.ai[0] = npc.whoAmI;

					_armBack = NPC.NewNPC((int)(npc.position.X + 234 * npc.direction), (int)(npc.position.Y + 79), mod.NPCType("Kraid_ArmBack"), npc.whoAmI);
					ArmBack.position += new Vector2((float)ArmBack.width / 2, (float)ArmBack.height);
					ArmBack.realLife = npc.whoAmI;
					ArmBack.ai[0] = npc.whoAmI;

					npc.ai[3] = 1;
					Body.netUpdate = ArmFront.netUpdate = ArmBack.netUpdate = true;
				}
			}

			npc.ai[1]++;
			if(npc.ai[1] >= 180 || npc.frameCounter > 0 || npc.frame.Y > 0 || roarCounter > 0 || mouthOpen)
			{
				npc.frameCounter += 1;
				if(npc.frameCounter >= 5)
				{
					npc.frame.Y += headAnim;
					if(npc.frame.Y >= 2)
					{
						npc.frame.Y = 2;
						if(roarCounter > 0 && !mouthOpen)
						{
							headAnim = 1;
						}
						else
						{
							headAnim = -1;
						}
					}
					if(npc.frame.Y <= 0)
					{
						npc.frame.Y = 0;
						headAnim = 1;
					}
					npc.frameCounter = 0;
				}
				npc.ai[1] = 0;
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


			bool flag = false;
			if(npc.ai[0] > 0)
			{
				npc.ai[0] += 1;
				if(npc.ai[0] <= 150)
				{
					flag = true;
				}
				else
				{
					npc.ai[0] = 0;
				}
			}
			this.Roar(flag);
			if(mouthOpen)
			{
				npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/KraidHitSound");
			}
			else
			{
				npc.HitSound = SoundID.NPCHit1;
			}

			int dir = 0;
			if(player.Center.X <= npc.Center.X+512 && player.Center.X >= npc.Center.X-512)
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
				dir *= npc.direction;
			}
			else
			{
				moveCounter = 0;
				if(player.Center.X > npc.Center.X)
				{
					dir = 1;
				}
				if(player.Center.X < npc.Center.X)
				{
					dir = -1;
				}
			}
			this.Move(dir);

			if(direction == 1 && player.Center.X < npc.position.X)
			{
				direction = -1;
			}
			if(direction == -1 && player.Center.X > npc.position.X+npc.width)
			{
				direction = 1;
			}
			npc.direction = direction;

			if(player.Center.Y < Body.position.Y)// && ((npc.direction == 1 && player.position.X <= (ArmBack.position.X+ArmBack.width)) || (npc.direction == -1 && (player.position.X+player.width) >= ArmBack.position.X)))
			{
				npc.ai[2] += 1f;
			}
			else
			{
				npc.ai[2] -= 0.5f;
			}
			if(npc.ai[2] > 200)
			{
				if(ArmBack != null && ArmBack.ai[1] <= 0)
				{
					ArmBack.ai[1]++;
				}
				npc.ai[2] = 0;
			}

			int heightOffset = 64;
			Vector2 position3 = new Vector2(Body.position.X, Body.position.Y + Body.height - heightOffset);
			//if (npc.position.X < player.position.X && npc.position.X + (float)npc.width > player.position.X + (float)player.width && npc.position.Y + (float)npc.height < player.position.Y + (float)player.height - 16f)
			if(player.position.Y > npc.position.Y+npc.height && Collision.SolidCollision(npc.position, npc.width, npc.height))
			{
				//npc.velocity.Y = npc.velocity.Y + 0.5f;
				if (npc.velocity.Y < 0f)
				{
					npc.velocity.Y = 0f;
				}
				if (npc.velocity.Y < 0.2f)
				{
					npc.velocity.Y = npc.velocity.Y + 0.025f;
				}
				else
				{
					npc.velocity.Y = npc.velocity.Y + 0.2f;
				}
				if (npc.velocity.Y > 2f)
				{
					npc.velocity.Y = 2f;
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
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = 0f;
					}
					if (npc.velocity.Y > -0.2f)
					{
						npc.velocity.Y = npc.velocity.Y - 0.025f;
					}
					else
					{
						npc.velocity.Y = npc.velocity.Y - 0.2f;
					}
					if (npc.velocity.Y < -2f)
					{
						npc.velocity.Y = -2f;
					}
				}
				else
				{
					if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = 0f;
					}
					if (npc.velocity.Y < 0.1f)
					{
						npc.velocity.Y = npc.velocity.Y + 0.025f;
					}
					else
					{
						npc.velocity.Y = npc.velocity.Y + 0.5f;
					}
				}
			}
			if (npc.velocity.Y > 10f)
			{
				npc.velocity.Y = 10f;
			}
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if((npc.direction == 1 && player.Center.X >= npc.Center.X) || (npc.direction == -1 && player.Center.X <= npc.Center.X))
			{
				if(npc.ai[0] <= 0 && roarCounter <= 0)
				{
					npc.ai[0] = 1;
				}
				if(mouthOpen)
				{
					if(ArmBack.ai[1] <= 0 && state > 0)
					{
						ArmBack.ai[1]++;
					}
				}
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if((npc.direction == 1 && projectile.Center.X >= npc.Center.X) || (npc.direction == -1 && projectile.Center.X <= npc.Center.X))
			{
				if(npc.ai[0] <= 0 && roarCounter <= 0)
				{
					npc.ai[0] = 1;
				}
				if(mouthOpen && projectile.Center.Y > npc.position.Y)
				{
					if(ArmBack.ai[1] <= 0 && state > 0)
					{
						ArmBack.ai[1]++;
					}
				}
			}
		}
		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			damage += (int)(npc.defense * 0.95f * 0.5f);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(mouthOpen && projectile.Center.Y > npc.position.Y && ((npc.direction == 1 && projectile.Center.X >= npc.Center.X) || (npc.direction == -1 && projectile.Center.X <= npc.Center.X)))
			{
				damage += (int)(npc.defense * 0.95f * 0.5f);
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
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/KraidRoarSound"));
				}
				if(roarCounter >= 30)
				{
					mouthOpen = true;
					npc.frame.X = 1;
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
				npc.frame.X = 0;
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
			Vector2 actualBLegPos = Body.Center + new Vector2(138*npc.direction,174) + bLegPos;
			Vector2 actualFLegPos = Body.Center + new Vector2(-68*npc.direction,174) + fLegPos;
			if(moveDir == 0)
			{
				npc.velocity.X = 0f;
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
							npc.velocity.X = 1f;
							fLegPos.Y = Math.Max(fLegPos.Y-1f,-8f);
						}
						else
						{
							npc.velocity.X = 0f;
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
							npc.velocity.X = 1f;
							bLegPos.Y = Math.Max(bLegPos.Y-1f,-8f);
						}
						else
						{
							npc.velocity.X = 0f;
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
							npc.velocity.X = -1f;
							fLegPos.Y = Math.Max(fLegPos.Y-1f,-8f);
						}
						else
						{
							npc.velocity.X = 0f;
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
							npc.velocity.X = -1f;
							bLegPos.Y = Math.Max(bLegPos.Y-1f,-8f);
						}
						else
						{
							npc.velocity.X = 0f;
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
			Main.PlaySound(2, (int)Body.Center.X, (int)Body.Center.Y,62, .5f);

			fullAnim = 6;
			fullOffset.Y = 2f;
		}

		Vector2[] gorePosition = new Vector2[12];

		Vector2 fullOffset = Vector2.Zero;
		
		int fullAnim = 0;
		
		float headRot = 0f;

		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Player player = Main.player[npc.target];

			npc.spriteDirection = npc.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (npc.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Color buffColor = Color.White;//Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			/*if (npc.behindTiles)
			{
				int num44 = (int)((Body.position.X - 8f) / 16f);
				int num45 = (int)((Body.position.X + (float)Body.width + 8f) / 16f);
				int num46 = (int)((npc.position.Y - 8f) / 16f);
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
			Color alpha2 = npc.GetAlpha(buffColor);

			Texture2D texHead = mod.GetTexture("NPCs/Kraid/Kraid_Head"),
				texJaw = mod.GetTexture("NPCs/Kraid/Kraid_Jaw"),
				texNeck = mod.GetTexture("NPCs/Kraid/Kraid_Neck"),
				texBody = mod.GetTexture("NPCs/Kraid/Kraid_Body"),
				texBodyOverlay = mod.GetTexture("NPCs/Kraid/Kraid_BodyOverlay"),
				texLegs = mod.GetTexture("NPCs/Kraid/Kraid_Legs"),
				texArm1 = mod.GetTexture("NPCs/Kraid/Kraid_Arm1"),
				texArm2 = mod.GetTexture("NPCs/Kraid/Kraid_Arm2"),
				texArmFront = mod.GetTexture("NPCs/Kraid/Kraid_ArmFront"),
				texArmBack = mod.GetTexture("NPCs/Kraid/Kraid_ArmBack");
			if(state > 0)
			{
				texHead = mod.GetTexture("NPCs/Kraid/Kraid_Head_"+state);
				texJaw = mod.GetTexture("NPCs/Kraid/Kraid_Jaw_"+state);
				texNeck = mod.GetTexture("NPCs/Kraid/Kraid_Neck_"+state);
				texBody = mod.GetTexture("NPCs/Kraid/Kraid_Body_"+state);
				texBodyOverlay = mod.GetTexture("NPCs/Kraid/Kraid_BodyOverlay_"+state);
				texLegs = mod.GetTexture("NPCs/Kraid/Kraid_Legs_"+state);
				texArm1 = mod.GetTexture("NPCs/Kraid/Kraid_Arm1_"+state);
				texArm2 = mod.GetTexture("NPCs/Kraid/Kraid_Arm2_"+state);
				texArmFront = mod.GetTexture("NPCs/Kraid/Kraid_ArmFront_"+state);
				texArmBack = mod.GetTexture("NPCs/Kraid/Kraid_ArmBack_"+state);
			}


			if (Body == null || ArmBack == null || ArmFront == null)
				return (false);

			Vector2 backArm1Pos = npc.Center + new Vector2(37*npc.direction,40) + (ArmBack.Center-(npc.Center+new Vector2(234*npc.direction,79)))*0.25f;
			sb.Draw(texArm1,backArm1Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm1.Width,texArm1.Height)),alpha2,0f,new Vector2(texArm1.Width/2,texArm1.Height/2),1f,effects,0f);
			gorePosition[0] = backArm1Pos;

			Vector2 bvec1 = new Vector2(-94,42);
			float bveclength = Vector2.Distance(Vector2.Zero,bvec1);
			float bvecrot = (float)Math.Atan2(bvec1.Y,bvec1.X)+ArmBack.rotation;
			Vector2 bvec2 = new Vector2((float)Math.Cos(bvecrot)*bveclength,(float)Math.Sin(bvecrot)*bveclength);
			Vector2 bArm2Pos1 = backArm1Pos+new Vector2(0f,30f),
					bArm2Pos2 = ArmBack.Center+new Vector2(bvec2.X*npc.direction,bvec2.Y);
			Vector2 backArm2Pos = Vector2.Lerp(bArm2Pos1,bArm2Pos2,0.5f);
			float bArmRot = (float)Math.Atan2((bArm2Pos2.Y-bArm2Pos1.Y)*npc.direction,(bArm2Pos2.X-bArm2Pos1.X)*npc.direction) - ((float)Math.PI*0.375f)*npc.direction;
			sb.Draw(texArm2,backArm2Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm2.Width,texArm2.Height)),alpha2,bArmRot,new Vector2(texArm2.Width/2,texArm2.Height/2),1f,effects,0f);
			gorePosition[1] = backArm2Pos;

			Vector2 bOrigin = new Vector2(109,80);
			if(npc.direction == -1)
			{
				bOrigin.X = (float)texArmBack.Width - bOrigin.X;
			}
			Vector2 armBackPos = fullOffset + ArmBack.Center + new Vector2(-(float)(ArmBack.width/2)*npc.direction,(float)ArmBack.height/2 - 14f);
			sb.Draw(texArmBack,armBackPos - Main.screenPosition,new Rectangle?(new Rectangle(0,(texArmBack.Height/6)*ArmBack.frame.Y,texArmBack.Width,texArmBack.Height/6)),alpha2,ArmBack.rotation*npc.direction,bOrigin,1f,effects,0f);
			gorePosition[2] = armBackPos;


			Vector2 backLegPos = Body.Center + new Vector2((62+texLegs.Width/4)*npc.direction,2) + bLegPos;
			sb.Draw(texLegs,backLegPos - Main.screenPosition,new Rectangle?(new Rectangle(texLegs.Width/2,0,texLegs.Width/2,texLegs.Height)),alpha2,0f,new Vector2(texLegs.Width/4,0),1f,effects,0f);
			gorePosition[3] = backLegPos;

			Vector2 bodyPos = fullOffset + Body.Center - new Vector2(117*npc.direction,38);
			sb.Draw(texBody,bodyPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texBody.Width,texBody.Height)),alpha2,0f,new Vector2(texBody.Width/2,texBody.Height/2),1f,effects,0f);
			gorePosition[4] = bodyPos;
			
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType("KraidBellySpike") && Main.projectile[i].ai[0] == npc.whoAmI && Main.projectile[i].localAI[0] <= 35)
				{
					Projectile projectile = Main.projectile[i];
					SpriteEffects effects2 = SpriteEffects.None;
					if (projectile.spriteDirection == -1)
					{
						effects2 = SpriteEffects.FlipHorizontally;
					}
					Texture2D tex = Main.projectileTexture[projectile.type];
					int num108 = tex.Height / Main.projFrames[projectile.type];
					int y4 = num108 * projectile.frame;
					sb.Draw(tex, new Vector2((float)((int)(projectile.Center.X - Main.screenPosition.X)), (float)((int)(projectile.Center.Y - Main.screenPosition.Y + projectile.gfxOffY))), new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2f, (float)projectile.height/2f), projectile.scale, effects2, 0f);
				}
			}
			
			Vector2 bodyOvPos = fullOffset + Body.Center + new Vector2(85*npc.direction,-7);
			sb.Draw(texBodyOverlay,bodyOvPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texBodyOverlay.Width,texBodyOverlay.Height)),alpha2,0f,new Vector2(texBodyOverlay.Width/2,texBodyOverlay.Height/2),1f,effects,0f);

			Vector2 frontLegPos = Body.Center + new Vector2((-144+texLegs.Width/4)*npc.direction,2) + fLegPos;
			sb.Draw(texLegs,frontLegPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texLegs.Width/2,texLegs.Height)),alpha2,0f,new Vector2(texLegs.Width/4,0),1f,effects,0f);
			gorePosition[5] = frontLegPos;

			bLegPrevPos = bLegPos;
			fLegPrevPos = fLegPos;


			float targetrotation = (float)Math.Atan2((player.Center.Y-npc.Center.Y)*npc.direction,(player.Center.X-npc.Center.X)*npc.direction);
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
			
			Vector2 hOffset = new Vector2(headOffset.X*npc.direction,headOffset.Y);

			Vector2 headPos = fullOffset + npc.Center + new Vector2((29-texNeck.Width/2)*npc.direction,-9) + new Vector2((float)Math.Max(Math.Ceiling(headOffset.X*0.5f),0f)*npc.direction,(float)Math.Floor(headOffset.Y*0.5f));
			if(mouthOpen)
			{
				headPos.X += roarFrame*npc.direction;
			}
			sb.Draw(texNeck,headPos - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texNeck.Width,texNeck.Height)),alpha2,0f,new Vector2(texNeck.Width/2,46),1f,effects,0f);
			gorePosition[6] = headPos;

			headPos = fullOffset + npc.Center + new Vector2(29*npc.direction,-9) + hOffset;
			Vector2 hpos1 = headPos;
			if(mouthOpen)
			{
				hpos1 += new Vector2(roarFrame*npc.direction,roarFrame);
			}
			sb.Draw(texJaw,hpos1 - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texJaw.Width,texJaw.Height)),alpha2,headRot,new Vector2(texJaw.Width/2,texJaw.Height/2),1f,effects,0f);
			gorePosition[7] = hpos1;
			Vector2 hpos2 = headPos;
			if(mouthOpen)
			{
				hpos2 += new Vector2(roarFrame*npc.direction,-roarFrame);
			}
			sb.Draw(texHead,hpos2 - Main.screenPosition,new Rectangle?(new Rectangle(0,(texHead.Height/6)*npc.frame.Y+(texHead.Height/2)*npc.frame.X,texHead.Width,texHead.Height/6)),alpha2,headRot,new Vector2(texHead.Width/2,texHead.Height/12),1f,effects,0f);
			gorePosition[8] = hpos2;


			Vector2 frontArm1Pos = npc.Center + new Vector2(-65*npc.direction,78) + (ArmFront.Center-(npc.Center+new Vector2(42*npc.direction,131)))*0.25f;
			sb.Draw(texArm1,frontArm1Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm1.Width,texArm1.Height)),alpha2,0f,new Vector2(texArm1.Width/2,texArm1.Height/2),1f,effects,0f);
			gorePosition[9] = frontArm1Pos;

			Vector2 vec1 = new Vector2(-92,62);
			float veclength = Vector2.Distance(Vector2.Zero,vec1);
			float vecrot = (float)Math.Atan2(vec1.Y,vec1.X)+ArmFront.rotation;
			Vector2 vec2 = new Vector2((float)Math.Cos(vecrot)*veclength,(float)Math.Sin(vecrot)*veclength);
			Vector2 fArm2Pos1 = frontArm1Pos,
					fArm2Pos2 = ArmFront.Center+new Vector2(vec2.X*npc.direction,vec2.Y);
			float fArmRot = (float)Math.Atan2((fArm2Pos2.Y-fArm2Pos1.Y)*npc.direction,(fArm2Pos2.X-fArm2Pos1.X)*npc.direction) - ((float)Math.PI/2)*npc.direction;
			Vector2 frontArm2Pos = Vector2.Lerp(fArm2Pos1,fArm2Pos2,0.5f);
			sb.Draw(texArm2,frontArm2Pos + fullOffset - Main.screenPosition,new Rectangle?(new Rectangle(0,0,texArm2.Width,texArm2.Height)),alpha2,fArmRot,new Vector2(texArm2.Width/2,texArm2.Height/2),1f,effects,0f);
			gorePosition[10] = frontArm2Pos;

			Vector2 fOrigin = new Vector2(106,63);
			if(npc.direction == -1)
			{
				fOrigin.X = (float)texArmFront.Width - fOrigin.X;
			}
			Vector2 armFrontPos = fullOffset + ArmFront.Center;
			sb.Draw(texArmFront,armFrontPos - Main.screenPosition,new Rectangle?(new Rectangle(0,(texArmFront.Height/5)*ArmFront.frame.Y,texArmFront.Width,texArmFront.Height/5)),alpha2,ArmFront.rotation*npc.direction,fOrigin,1f,effects,0f);
			gorePosition[11] = armFrontPos;
			
			if(fullAnim <= 0)
			{
				fullOffset = Vector2.Zero;
			}
			
			
			Texture2D rect = mod.GetTexture("Gore/Pixel");
			
			int num44 = (int)((Body.position.X - 240f) / 16f);
			int num45 = (int)((Body.position.X + (float)Body.width + 240f) / 16f);
			int num46 = (int)((npc.position.Y - 32f) / 16f);
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
					if (tile1 != null && tile1.active() && Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type] &&
						tile2 != null && tile2.active() && Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type] &&
						tile3 != null && tile3.active() && Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type] &&
						tile4 != null && tile4.active() && Main.tileSolid[(int)tile4.type] && !Main.tileSolidTop[(int)tile4.type] &&
						tile5 != null && tile5.active() && Main.tileSolid[(int)tile5.type] && !Main.tileSolidTop[(int)tile5.type] &&
						tile6 != null && tile6.active() && Main.tileSolid[(int)tile6.type] && !Main.tileSolidTop[(int)tile6.type] &&
						tile7 != null && tile7.active() && Main.tileSolid[(int)tile7.type] && !Main.tileSolidTop[(int)tile7.type] &&
						tile8 != null && tile8.active() && Main.tileSolid[(int)tile8.type] && !Main.tileSolidTop[(int)tile8.type] &&
						tile9 != null && tile9.active() && Main.tileSolid[(int)tile9.type] && !Main.tileSolidTop[(int)tile9.type])
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

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				for (int m = 0; m < (npc.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, npc.life <= 0 && m % 2 == 0 ? 3f : 1f);
					if (npc.life <= 0 && m % 2 == 0)
					{
						Main.dust[dustID].noGravity = true;
					}
				}

				if (npc.life <= 0)
				{
					int[] mapped_gore = new int[12] { 4, 5, 7, 9, 0, 8, 3, 2, 1, 4, 5, 6 };
					for(int i = 0; i < gorePosition.Length; i++)
					{
						if (i == 4) continue;

						string goreindex = "Gores/KraidGore" + mapped_gore[i];
						int gore = Gore.NewGore(gorePosition[i],new Vector2(Main.rand.Next(-5,5),Main.rand.Next(-5,5)),mod.GetGoreSlot(goreindex),1f);
						Main.gore[gore].timeLeft = 30;
						Main.gore[gore].rotation = 0;
					}
					Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,1);

					/*for (int num70 = 0; num70 < 25; num70++)
					{
						int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)npc.position.X,(int)npc.position.Y,14);*/
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
