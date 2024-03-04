using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs
{
	public abstract class MNPC : ModNPC
	{
		public MNPC mNPC;
		public MNPC()
		{
			mNPC = this;
		}

		protected int directionY = 1; // <= because 'TargetClosest' changes both normal direction and directionY, so we're defining our own
		public void HopperAI(NPC n, float maxSpeedX, float maxSpeedY)
		{
			if (n.ai[1] == 1f)
			{
				n.direction = Math.Sign(Main.player[n.target].position.X - n.position.X);
				if (n.velocity.Y == 0f)
				{
					n.velocity.X *= 0.1f;
					if (Math.Abs(n.velocity.X) < 1f)
					{
						n.velocity.X = 0f;
					}
				}
				n.ai[0]++;
				if (n.ai[0] > 40)
				{
					n.velocity.Y = -maxSpeedY * directionY;
					n.velocity.X = maxSpeedX * n.direction;
					if (Main.rand.NextBool(2))
					{
						n.velocity.Y *= 0.75f;
					}
					n.velocity *= n.scale;
					n.ai[1] = 0f;
					n.ai[0] = 1f;
					n.netUpdate = true;
				}
			}
			else
			{
				if (Math.Sign(n.velocity.Y) == -directionY && n.ai[0] == 1f)
				{
					n.velocity.X = maxSpeedX * n.direction;
				}
				else
				{
					n.ai[0] = 0f;
				}
				if (n.velocity.Y == 0f && n.ai[2] == 1f)
				{
					n.ai[1] = 1f;
					n.ai[0] = 0f;
					n.TargetClosest(true);
				}
			}

			if (n.ai[2] == 0f)
			{
				directionY = 1;
				if (Main.rand.NextBool(2))
				{
					directionY = -1;
				}
				n.ai[2] = 1f;
				n.netUpdate = true;
			}

			float maxFallSpeed = 10f;
			float gravity = 0.3f;

			float num = (float)(Main.maxTilesX / 4200);
			num *= num;
			float num2 = (float)((double)(n.position.Y / 16f - (60f + 10f * num)) / (Main.worldSurface / 6.0));
			if ((double)num2 < 0.25)
			{
				num2 = 0.25f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			gravity *= num2;
			if (n.wet)
			{
				if (n.honeyWet)
				{
					gravity = 0.1f;
					maxFallSpeed = 4f;
				}
				gravity = 0.2f;
				maxFallSpeed = 7f;
			}

			n.velocity.Y += gravity * directionY;
			if (directionY == -1)
			{
				if (n.velocity.Y < -maxFallSpeed)
				{
					n.velocity.Y = -maxFallSpeed;
				}

				if (Collision.SolidCollision(n.position + new Vector2(0f, n.velocity.Y), n.width, n.height))
				{
					if (n.velocity.Y < 0f)
					{
						n.velocity.Y = 0f;
					}
					if (n.velocity.Y > 0f)
					{
						n.velocity.Y = -0.01f;
					}
					n.ai[3] = 0f;
				}
				else if (n.velocity.Y <= -maxFallSpeed)
				{
					n.ai[3]++;
					if (n.ai[3] > 30f)
					{
						directionY = 1;
						n.ai[3] = 0f;
					}
				}
			}
			else
			{
				if (n.velocity.Y > maxFallSpeed)
				{
					n.velocity.Y = maxFallSpeed;
				}
			}
		}
		public void DrawHopper(NPC n, SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Npc[n.type].Value;//Main.npcTexture[n.type];
																				 //Color color = n.GetAlpha(Lighting.GetColor((int)n.Center.X / 16, (int)n.Center.Y / 16));
			Color color = n.GetAlpha(drawColor);
			int height = tex.Height / Main.npcFrameCount[n.type];

			if (directionY == -1)
			{
				sb.Draw(tex, new Vector2(n.Center.X, n.position.Y) - screenPos, new Rectangle?(new Rectangle(0, n.frame.Y * height, tex.Width, height)), color, 0f, new Vector2(tex.Width / 2, 2), n.scale, SpriteEffects.FlipVertically, 0f);
			}
			else
			{
				sb.Draw(tex, new Vector2(n.Center.X, n.position.Y + n.height) - screenPos, new Rectangle?(new Rectangle(0, n.frame.Y * height, tex.Width, height)), color, 0f, new Vector2(tex.Width / 2, height - 2), n.scale, SpriteEffects.None, 0f);
			}
		}

		protected float yOffset = 0f;
		protected float rotation = 0f;
		public float crawlSpeed = 0f;
		public void CrawlerAI(NPC n, float speed, int gravityType = 0, bool useRotation = true, bool fullSpeedOnSlopes = false)
		{
			// This AI is based on the vanilla snail AI (because it can crawl on all surfaces)
			// with some modifications that make it work better on slopes.
			// To make it work, the NPC needs the same aiStyle value of 67
			// which allows it to use the same slope collision checks as that AI.
			n.ai[2] = 2f; // <-- So don't remove this because of potential shinanigans with vanilla code.

			// We also have to define our own 'rotation' and 'directionY' variables, because the vanilla code screws with them on slopes.
			// Also make sure this is run in PreAI and return false so that it doesn't use the full extent of the vanilla AI.

			float speedCheck = Math.Max(speed, 1f);
			if (CollideMethods.CheckCollide(n.position + new Vector2(speedCheck * n.direction, 0f), n.width, n.height))
			{
				n.collideX = true;
			}
			if (CollideMethods.CheckCollide(n.position + new Vector2(0f, speedCheck * directionY), n.width, n.height))
			{
				n.collideY = true;
			}

			if (n.ai[0] == 0f)
			{
				n.TargetClosest(true);
				directionY = 1;
				n.ai[0] = 1f;

				n.direction = 1;
				if (Main.rand.NextBool(2))
				{
					n.direction = -1;
				}

				if (n.direction > 0)
				{
					n.spriteDirection = 1;
				}
				n.netUpdate = true;
			}
			bool flag = false;
			//if (Main.netMode != 1)
			//{
			if (!n.collideX && !n.collideY)
			{
				n.localAI[3] += 1f;
				if (n.localAI[3] > 5f)
				{
					n.ai[3] = 2f;
					//n.netUpdate = true;
				}
			}
			else
			{
				n.localAI[3] = 0f;
			}
			//}
			if (n.ai[3] > 0f)
			{
				n.ai[1] = 0f;
				n.ai[0] = 1f;
				if (n.velocity.Y > speed && useRotation)
				{
					rotation += (float)n.direction * 0.1f;
				}
				else
				{
					rotation = 0f;
				}
				n.spriteDirection = n.direction;
				n.velocity.X = speed * (float)n.direction;

				if (gravityType == 0)
				{
					directionY = 1;
					n.noGravity = false;
				}
				else if (gravityType == 1)
				{
					n.velocity.Y = speed * directionY;
				}

				if (n.collideX || n.collideY)
				{
					n.ai[3] -= 1f;
				}
			}
			if (n.ai[3] != 0f)
			{
				return;
			}

			n.noGravity = true;

			bool flag2 = false;
			if (CollideMethods.CheckCollide(n.position, n.width, n.height))
			{
				flag2 = true;

				bool flag3 = false;
				if (!CollideMethods.CheckCollide(n.position + new Vector2(n.width, 0f), 1, n.height))
				{
					n.direction = 1;
				}
				else if (!CollideMethods.CheckCollide(n.position + new Vector2(-1f, 0f), 1, n.height))
				{
					n.direction = -1;
				}
				else
				{
					n.direction *= -1;
					flag3 = true;
				}

				if (!CollideMethods.CheckCollide(n.position + new Vector2(0f, n.height), n.width, 1))
				{
					directionY = 1;
				}
				else if (CollideMethods.CheckCollide(n.position + new Vector2(0f, -1f), n.width, 1) || flag3)
				{
					directionY = -1;
				}
				else
				{
					directionY *= -1;
				}
			}
			else
			{
				if (n.ai[1] == 0f)
				{
					if (n.collideY)
					{
						if (n.ai[0] < 2f)
						{
							n.ai[0] = 2f;
						}
					}
					if (!n.collideY && n.ai[0] >= 2f)
					{
						n.ai[0] += 1f;
						if (n.ai[0] > 3f)
						{
							n.direction = -n.direction;
							n.ai[1] = 1f;
							n.ai[0] = 1f;
						}
					}
					if (n.collideX)
					{
						directionY = -directionY;
						n.ai[1] = 1f;
					}
				}
				else
				{
					if (n.collideX)
					{
						if (n.ai[0] < 2f)
						{
							n.ai[0] = 2f;
						}
					}
					if (!n.collideX && n.ai[0] >= 2f)
					{
						n.ai[0] += 1f;
						if (n.ai[0] > 3f)
						{
							directionY = -directionY;
							n.ai[1] = 0f;
							n.ai[0] = 1f;
						}
					}
					if (n.collideY)
					{
						n.direction = -n.direction;
						n.ai[1] = 0f;
					}
				}
			}

			if (!flag && useRotation)
			{
				float bottom_rot = 0f;
				float left_rot = 1.57f;
				float top_rot = 3.14f;
				float right_rot = 4.71f;

				float rot = rotation;
				if (directionY < 0)
				{
					if (n.direction < 0)
					{
						if (n.collideX)
						{
							rotation = left_rot;
							n.spriteDirection = -1;
						}
						else if (n.collideY)
						{
							rotation = top_rot;
							n.spriteDirection = 1;
						}
					}
					else if (n.collideY)
					{
						rotation = top_rot;
						n.spriteDirection = -1;
					}
					else if (n.collideX)
					{
						rotation = right_rot;
						n.spriteDirection = 1;
					}
				}
				else if (n.direction < 0)
				{
					if (n.collideY)
					{
						rotation = bottom_rot;
						n.spriteDirection = -1;
					}
					else if (n.collideX)
					{
						rotation = left_rot;
						n.spriteDirection = 1;
					}
				}
				else if (n.collideX)
				{
					rotation = right_rot;
					n.spriteDirection = -1;
				}
				else if (n.collideY)
				{
					rotation = bottom_rot;
					n.spriteDirection = 1;
				}

				float bl_slope_rot = 0.79f;
				float tl_slope_rot = 2.36f;
				float tr_slope_rot = 3.93f;
				float br_slope_rot = 5.50f;
				float offY = 0f;
				bool bottom = CollideMethods.CheckCollide(n.position + new Vector2(0f, 2f), n.width, n.height);
				bool left = CollideMethods.CheckCollide(n.position + new Vector2(-2f, 0f), n.width, n.height);
				bool top = CollideMethods.CheckCollide(n.position + new Vector2(0f, -2f), n.width, n.height);
				bool right = CollideMethods.CheckCollide(n.position + new Vector2(2f, 0f), n.width, n.height);
				if (left)
				{
					if (bottom)
					{
						rotation = bl_slope_rot;
						offY = 6f;
					}
					if (top)
					{
						rotation = tl_slope_rot;
						offY = 6f;
					}
				}
				if (right)
				{
					if (bottom)
					{
						rotation = br_slope_rot;
						offY = 6f;
					}
					if (top)
					{
						rotation = tr_slope_rot;
						offY = 6f;
					}
				}
				if (yOffset < offY)
				{
					yOffset = Math.Min(yOffset + 1f, offY);
				}
				else
				{
					yOffset = Math.Max(yOffset - 1f, offY);
				}

				float rot2 = rotation;
				rotation = rot;
				if ((double)rotation > 6.28)
				{
					rotation -= 6.28f;
				}
				if (rotation < 0f)
				{
					rotation += 6.28f;
				}
				float rot3 = Math.Abs(rotation - rot2);
				float rotRate = 0.1f;
				if (rotation > rot2)
				{
					if ((double)rot3 > 3.14)
					{
						rotation += rotRate;
					}
					else
					{
						rotation -= rotRate;
						if (rotation < rot2)
						{
							rotation = rot2;
						}
					}
				}
				if (rotation < rot2)
				{
					if ((double)rot3 > 3.14)
					{
						rotation -= rotRate;
					}
					else
					{
						rotation += rotRate;
						if (rotation > rot2)
						{
							rotation = rot2;
						}
					}
				}
			}
			float speed2 = speed;
			if (!CollideMethods.CheckCollide(n.position + new Vector2(speedCheck * n.direction, speedCheck * directionY), n.width, n.height) && !fullSpeedOnSlopes)
			{
				speed2 = (float)Math.Cos(Math.PI / 4) * speed;
			}
			n.velocity.X = speed2 * (float)n.direction;
			n.velocity.Y = speed2 * (float)directionY;
			if (flag2)
			{
				n.position += n.velocity;
			}
		}
		public void DrawCrawler(NPC n, SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Npc[n.type].Value;//Main.npcTexture[n.type];
																				 //Color color = n.GetAlpha(Lighting.GetColor((int)n.Center.X / 16, (int)n.Center.Y / 16));
			Color color = n.GetAlpha(drawColor);
			int height = tex.Height / Main.npcFrameCount[n.type];

			SpriteEffects effects = SpriteEffects.None;
			if (n.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			sb.Draw(tex, n.Center - screenPos, new Rectangle?(new Rectangle(0, n.frame.Y * height, tex.Width, height)), color, rotation, new Vector2(tex.Width / 2, height / 2 - yOffset), n.scale, effects, 0f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((short)directionY);
			writer.Write(crawlSpeed);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			directionY = reader.ReadInt16();
			crawlSpeed = reader.ReadSingle();
		}


		// ignore this. it was the old code i planned on using to globally control mob spawn chances, but i decided to do it in each mob's individual file
		// i'm only keeping this in case i change my mind
		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// metroids/corruption enemies
			if(npc.type == mod.NPCType("LarvalMetroid"))
			{
				if(Main.hardMode)
				{
					return (SpawnCondition.Corruption.Chance + SpawnCondition.Crimson.Chance + SpawnCondition.DungeonNormal.Chance) * 0.75f;
				}
				return SpawnCondition.DungeonNormal.Chance*0.5f;
			}
			if(npc.type == mod.NPCType("Mochtroid"))
			{
				float chance = SpawnCondition.Corruption.Chance + SpawnCondition.Crimson.Chance;
				if(Main.hardMode)
				{
					chance *= 0.5f;
				}
				return chance;
			}
			
			// pirates/dungeon enemies
			if(npc.type == mod.NPCType("SpacePirate") && Main.hardMode)
			{
				return SpawnCondition.DungeonNormal.Chance;
			}
			
			if(npc.type == mod.NPCType("Covern"))
			{
				if(Main.hardMode)
				{
					return SpawnCondition.DungeonNormal.Chance*0.75f;
				}
				return SpawnCondition.DungeonNormal.Chance*0.5f;
			}
			
			// aquatic enemies
			if(npc.type == mod.NPCType("Skultera") || npc.type == mod.NPCType("Zoa") || npc.type == mod.NPCType("Powamp"))
			{
				return SpawnCondition.CavePiranha.Chance + SpawnCondition.JungleWater.Chance + SpawnCondition.Ocean.Chance;
			}
			if(((npc.type == mod.NPCType("Evir") || npc.type == mod.NPCType("Scisor")) && Main.hardMode) || 
				npc.type == mod.NPCType("Owtch") || npc.type == mod.NPCType("Choot") || npc.type == mod.NPCType("Puyo") || npc.type == mod.NPCType("Bull"))
			{
				return SpawnCondition.Ocean.Chance + SpawnCondition.DesertCave.Chance;
			}
			
			// cave/jungle enemies
			if(npc.type == mod.NPCType("KagoHive") || npc.type == mod.NPCType("Zeb") || npc.type == mod.NPCType("Geemer") ||
				npc.type == mod.NPCType("Tripper") || npc.type == mod.NPCType("Ripper") || npc.type == mod.NPCType("Skree") || npc.type == mod.NPCType("Waver"))
			{
				return SpawnCondition.Cavern.Chance + SpawnCondition.UndergroundJungle.Chance;
			}
			if(npc.type == mod.NPCType("Beetom") || npc.type == mod.NPCType("Cacatac") || npc.type == mod.NPCType("Zebbo") || 
				npc.type == mod.NPCType("Zeela") || npc.type == mod.NPCType("Zero") || npc.type == mod.NPCType("Reo"))
			{
				return SpawnCondition.UndergroundJungle.Chance;
			}
			if(npc.type == mod.NPCType("Sidehopper"))
			{
				if(Main.hardMode)
				{
					return (SpawnCondition.Cavern.Chance + SpawnCondition.UndergroundJungle.Chance) * 0.5f;
				}
				return SpawnCondition.Cavern.Chance + SpawnCondition.UndergroundJungle.Chance;
			}
			
			// underworld enemies
			if(npc.type == mod.NPCType("Dessgeega") || npc.type == mod.NPCType("Viola") || npc.type == mod.NPCType("Multiviola") || 
				npc.type == mod.NPCType("Sova") || npc.type == mod.NPCType("Geruta") || npc.type == mod.NPCType("Squeept") || npc.type == mod.NPCType("Metaree"))
			{
				if(Main.hardMode && npc.type == mod.NPCType("Dessgeega"))
				{
					return SpawnCondition.Underworld.Chance * 0.5f;
				}
				return SpawnCondition.Underworld.Chance;
			}
			
			return 0f;
		}*/
	}
}
