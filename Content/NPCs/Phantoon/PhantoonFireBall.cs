using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidModPorted.Content.NPCs.Phantoon
{
	public class PhantoonFireBall : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Ball");
			Main.npcFrameCount[NPC.type] = 7;
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					39,
					44,
					ModContent.BuffType<Buffs.PhazonDebuff>()
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}
		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 28;
			NPC.damage = 0;//50;
			NPC.defense = 10;
			NPC.lifeMax = 50;
			NPC.knockBackResist = 0;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath3;
			NPC.value = 0;
			NPC.lavaImmune = true;
			NPC.behindTiles = false;
			NPC.aiStyle = -1;
			NPC.npcSlots = 0;
			
			NPC.dontTakeDamage = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Phantoon>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement("A ball of fire.")
			});
		}

		int damage = 66;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			damage *= 2;
		}
		
		Vector2 startPos;
		
		bool initialized = false;
		public override bool PreAI()
		{
			if(!initialized)
			{
				startPos = NPC.Center;
				initialized = true;

				// Play the spawn sound for the newly spawned NPC.
				SoundEngine.PlaySound(Sounds.NPCs.PhantoonFire, NPC.Center);
			}
			return true;
		}

		float dist = 0;//120;
		bool bounced = false;
		int bounceCounter = 0;
		int frameSet = 0;
		int currentFrame = 0;
		int timeLeft = 300;
		public override void AI()
		{
			NPC creator = Main.npc[(int)NPC.ai[0]];
			Player player = Main.player[NPC.target];

			// Phantoon spawn animation behavior
			if (NPC.ai[1] == -1)
			{
				NPC.active = creator.active;

				// Imported from original Phantoon code.
				if (creator.ai[0] > 300)
				{
					NPC.ai[3] = 1;
					if (creator.ai[0] > 460)
						NPC.ai[3] = 2;
				}

				if(NPC.ai[3] == 1 || NPC.ai[3] == 2)
				{
					NPC.ai[2] += (float)Math.PI / 120;
					
					if(NPC.ai[3] == 2)
					{
						dist -= 1f;
					}
				}
				else
				{
					dist = 120;
				}
				
				NPC.Center = creator.Center + new Vector2((float)Math.Cos(NPC.ai[2]) * dist, (float)Math.Sin(NPC.ai[2]) * dist);
				
				if(dist <= 10f)
				{
					NPC.alpha += 25;
					if(NPC.alpha >= 255)
					{
						NPC.active = false;
					}
				}
				NPC.frameCounter++;
				if(NPC.frameCounter > 4)
				{
					NPC.frame.Y++;
					NPC.frameCounter = 0;
				}
				if(NPC.frame.Y >= 3)
				{
					NPC.frame.Y = 0;
				}
			}
			else
			{
				if(timeLeft > 0)
				{
					if(NPC.dontTakeDamage)
					{
						NPC.alpha = 127;
					}
					else
					{
						NPC.alpha = 0;
					}
					timeLeft--;
				}
				else
				{
					NPC.alpha += 25;
					if(NPC.alpha >= 255)
					{
						NPC.active = false;
					}
				}
			}

			// Basic bounce behavior
			if (NPC.ai[1] == 0)
			{
				NPC.noTileCollide = false;
				
				NPC.frameCounter++;
				if(NPC.frameCounter > 4)
				{
					currentFrame++;
					NPC.frameCounter = 0;
				}
				if(currentFrame >= 3)
				{
					currentFrame = 0;
				}
				
				if(NPC.ai[2] <= 0f)
				{
					if(!bounced)
					{
						frameSet = 0;
						if(NPC.velocity.Y == 0f)
						{
							if(bounceCounter > 0)
							{
								currentFrame = 0;
								frameSet = 6;
								if(timeLeft > 0)
								{
									NPC.alpha = 0;
								}
								NPC.velocity = Vector2.Zero;
							}
							bounceCounter++;
						}
						if(bounceCounter > 10 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							bounced = true;
							NPC.netUpdate = true;
							NPC.velocity.Y = -(3 + Main.rand.Next(4));
							NPC.velocity.X = (10 - Main.rand.Next(21)) / 2;
						}
					}
					else
					{
						if(NPC.velocity.Y == 0f)
						{
							NPC.life = 0;
							NPC.HitEffect(0, 10.0);
							NPC.active = false;
						}
						frameSet = 3;
						NPC.dontTakeDamage = false;
						NPC.damage = damage;
					}
					NPC.velocity.Y += 0.1f;
				}
				else
				{
					NPC.ai[2] -= 1f;
					NPC.dontTakeDamage = false;
					NPC.damage = damage;
				}
				
				NPC.frame.Y = currentFrame + frameSet;
			}

			if(NPC.ai[1] == 1) // targeting behavior
			{
				NPC.frameCounter++;
				if(NPC.frameCounter > 4)
				{
					currentFrame++;
					NPC.frameCounter = 0;
				}
				if(currentFrame >= 3)
				{
					currentFrame = 0;
				}

				float targetRot = (float)Math.Atan2(player.Center.Y - NPC.Center.Y, player.Center.X - NPC.Center.X);
				if(NPC.ai[3] < 30)
				{
					if(creator.active)
					{
						startPos = creator.Center;
						NPC.Center = creator.Center + new Vector2((float)Math.Cos(NPC.ai[2]) * dist,(float)Math.Sin(NPC.ai[2]) * dist);
					}
					else
					{
						NPC.Center = startPos + new Vector2((float)Math.Cos(NPC.ai[2]) * dist,(float)Math.Sin(NPC.ai[2]) * dist);
					}
					
					if(dist < 120)
					{
						dist += 6f;
					}
					else
					{
						NPC.ai[3]++;
					}

					if(NPC.ai[3] == 30)
					{
						NPC.velocity = targetRot.ToRotationVector2()*12;
					}
					frameSet = 0;
					NPC.dontTakeDamage = true;
				}
				else
				{
					frameSet = 3;
					NPC.damage = damage;
					//NPC.velocity += targetRot.ToRotationVector2()*0.2f;
				}
				
				NPC.frame.Y = currentFrame + frameSet;
			}

			// Super fireball
			if (NPC.ai[1] == 2)
			{
				NPC.damage = damage;
				NPC.dontTakeDamage = false;
				float targetRot = (float)Math.Atan2(player.Center.Y - NPC.Center.Y, player.Center.X - NPC.Center.X);

				if(NPC.ai[2] == 0f)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						var entitySource = NPC.GetSource_FromAI();
						for (int i = 0; i < 4; i++)
						{
							NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PhantoonFireBall>(), NPC.whoAmI, NPC.whoAmI, 3, (float)Math.PI / 2 * i);
						}
					}
					NPC.ai[2] = 1f;
					NPC.velocity = targetRot.ToRotationVector2();
				}
				else
				{
					if(NPC.velocity.Length() < 16)
					{
						NPC.velocity *= 1.025f;
						if(Vector2.Distance(player.Center, NPC.Center) <= 600)
						{
							NPC.velocity += targetRot.ToRotationVector2()*0.5f;
						}
					}
				}
				
				NPC.frameCounter++;
				if(NPC.frameCounter > 4)
				{
					NPC.frame.Y++;
					NPC.frameCounter = 0;
				}
				if(NPC.frame.Y >= 6 || NPC.frame.Y < 3)
				{
					NPC.frame.Y = 3;
				}
			}

			// Super fireball #2
			if (NPC.ai[1] == 3)
			{
				if(creator.type == NPC.type && creator.active && creator.ai[1] == 2)
				{
					NPC.Center = creator.Center + new Vector2((float)Math.Cos(NPC.ai[2]) * 28, (float)Math.Sin(NPC.ai[2]) * 28) - creator.velocity;
					NPC.velocity = creator.velocity;
					NPC.ai[2] += (float)Math.PI / 60;
					
					NPC.frameCounter++;
					if(NPC.frameCounter > 4)
					{
						NPC.frame.Y++;
						NPC.frameCounter = 0;
					}
					if(NPC.frame.Y >= 3)
					{
						NPC.frame.Y = 0;
					}
					NPC.damage = damage;
				}
				else
				{
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.dontTakeDamage = true;
					NPC.damage = 0;
				}
			}

			// Behavior for when phantoon opens his eye
			if (NPC.ai[1] == 4)
			{
				NPC.Center = startPos + new Vector2((float)Math.Cos(NPC.ai[2]) * dist, (float)Math.Sin(NPC.ai[2]) * dist);
				dist += 8f;
				NPC.ai[2] += (float)Math.PI/60;
				
				NPC.damage = damage;
				NPC.dontTakeDamage = false;
				if(NPC.frameCounter++ > 4)
				{
					NPC.frame.Y++;
					NPC.frameCounter = 0;
				}
				if(NPC.frame.Y >= 3)
				{
					NPC.frame.Y = 0;
				}
			}

			float num = (255f - NPC.alpha) / 255f;
			Lighting.AddLight(NPC.Center, 0f,0.75f*num,1f*num);
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
			int texH = (tex.Height / 7);
			sb.Draw(tex,NPC.Center - Main.screenPosition,new Rectangle?(new Rectangle(0,texH*NPC.frame.Y,tex.Width,texH)),NPC.GetAlpha(Color.White),0f,new Vector2(tex.Width/2,texH-(NPC.height/2)-1),1f,SpriteEffects.None,0f);
			return false;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 59, 0f, -(Main.rand.Next(4)/2), 100, Color.White, 1.5f);
				Main.dust[dust].noGravity = true;
			}
			if(NPC.life <= 0)
			{
				for(int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 56, 0f, -(Main.rand.Next(3)/2), 100, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
