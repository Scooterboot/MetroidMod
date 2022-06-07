using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Pirate
{
	public class SpacePirate : MNPC
	{
		/*
		** NPC.ai[0] = state manager.
		*/
		internal readonly float speed = 3;
		internal readonly float acceleration = .5F;

		internal int specialTimer = 0;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 16;
		}
		public override void SetDefaults()
		{
			NPC.width = 18; NPC.height = 60;

			/* Temporary NPC values */
			NPC.scale = 1;
			NPC.damage = 25;
			NPC.defense = 5;
			NPC.lifeMax = 1000;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			specialTimer = 0;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A race of creatures which were the product of various invasive species including the metroids.")
			});
		}

		public override bool PreAI()
		{
			if (NPC.ai[0] == 0) // Grounded/movement.
			{
				bool idle = false;
				if (NPC.velocity.X == 0)
					idle = true;
				if (NPC.justHit)
					idle = false;

				bool moving = false;

				// If NPC is grounded and moving.
				if (NPC.velocity.Y == 0 && (NPC.velocity.X > 0 && NPC.direction < 0) || (NPC.velocity.X < 0 && NPC.direction > 0))
					moving = true;

				NPC.TargetClosest(true);

				if (NPC.velocity.X < -speed || NPC.velocity.X > speed)
				{
					if (NPC.velocity.Y == 0)
					{
						NPC.velocity.X *= .8F;
						NPC.velocity.Y *= .8F;
					}
				}
				else if (NPC.velocity.X < speed && NPC.direction == 1)
				{
					NPC.velocity.X += acceleration;
					if (NPC.velocity.X > speed)
						NPC.velocity.X = speed;
				}
				else if (NPC.velocity.X > -speed && NPC.direction == -1)
				{
					NPC.velocity.X -= acceleration;
					if (NPC.velocity.X < -speed)
						NPC.velocity.X = -speed;
				}

				bool grounded = false;

				if(NPC.velocity.Y == 0) // Grounded.
				{
					int npcTileXLeft = (int)NPC.position.X / 16;
					int npcTileXRight = (int)(NPC.position.X + NPC.width) / 16;
					int npcTileY = (int)(NPC.position.Y + NPC.height + 7) / 16;

					for(int i = npcTileXLeft; i < npcTileXRight; ++i)
					{
						if(Main.tile[i, npcTileY] == null) return false;
						if(Main.tile[i, npcTileY].IsActuated && Main.tileSolid[Main.tile[i, npcTileY].TileType])
						{
							grounded = true;
							break;
						}
					}
				}

				if(NPC.velocity.Y >= 0) // 'Falling' check for sloped tiles.
				{
					int dir = NPC.velocity.X == 0 ? 0 : NPC.velocity.X > 0 ? 1 : -1;
					float posX = NPC.position.X + NPC.velocity.X;
					float posY = NPC.position.Y;

					int tileX = (int)(posX + (NPC.width / 2) + (NPC.width / 2 + 1) * dir) / 16;
					int tileY = (int)(posY + NPC.height - 1) / 16;

					// Tile null check failsafe.
					/*for(int y = tileY-3; y < tileY+1; ++y)
						if (Main.tile[tileX, y] == null)
					if (Main.tile[tileX - dir, tileY - 3] == null)
						Main.tile[tileX - dir, tileY - 3] = new Tile();*/

					// Gruesome if statement incomming, please refactor if you can, it drives me nuts.
					if ((tileX * 16) < posX + NPC.width && (tileX * 16 + 16) > posX && (Main.tile[tileX, tileY].IsActuated && !Main.tile[tileX, tileY].TopSlope && (!Main.tile[tileX, tileY - 1].TopSlope && Main.tileSolid[(int)Main.tile[tileX, tileY].TileType]) && !Main.tileSolidTop[(int)Main.tile[tileX, tileY].TileType] || Main.tile[tileX, tileY - 1].IsHalfBlock && Main.tile[tileX, tileY - 1].IsActuated) && (!Main.tile[tileX, tileY - 1].IsActuated || !Main.tileSolid[(int)Main.tile[tileX, tileY - 1].TileType] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 1].TileType] || Main.tile[tileX, tileY - 1].IsHalfBlock && (!Main.tile[tileX, tileY - 4].IsActuated || !Main.tileSolid[(int)Main.tile[tileX, tileY - 4].TileType] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 4].TileType])) && ((!Main.tile[tileX, tileY - 2].IsActuated || !Main.tileSolid[(int)Main.tile[tileX, tileY - 2].TileType] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 2].TileType]) &&
						(!Main.tile[tileX, tileY - 3].IsActuated ||
						!Main.tileSolid[(int)Main.tile[tileX, tileY - 3].TileType] ||
						Main.tileSolidTop[(int)Main.tile[tileX, tileY - 3].TileType]) &&
						(!Main.tile[tileX - dir, tileY - 3].IsActuated ||
						!Main.tileSolid[(int)Main.tile[tileX - dir, tileY - 3].TileType])))
					{
						float y = tileY * 16;
						if (Main.tile[tileX, tileY].IsHalfBlock)
							y += 8;
						if (Main.tile[tileX, tileY - 1].IsHalfBlock)
							y -= 8;

						float yFinalPos = posY + NPC.height - y;
						if (y < posY + NPC.height && yFinalPos <= 16.1F)
						{
							NPC.gfxOffY += NPC.position.Y + NPC.height - y;
							NPC.position.Y = y - NPC.height;
							NPC.stepSpeed = yFinalPos >= 9.0F ? 2 : 1;
							grounded = true;
						}
					}
				}

				if (grounded)
				{
					int tileX = (int)(NPC.position.X + (NPC.width / 2) + (15 * NPC.direction)) / 16;
					int tileY = (int)(NPC.position.Y + NPC.height - 15) / 16;

					/*for (int x = tileX - 1; x < tileX + 1; ++x)
						for (int y = tileY - 3; y < tileY + 1; y++)
							if (Main.tile[x, y] == null)
								Main.tile[x, y] = new Tile();*/

					if (NPC.velocity.X < 0 && NPC.direction == -1 || NPC.velocity.X > 0 && NPC.direction == 1)
					{
						// Jump required, determine jump height.
						if (NPC.height >= 32 && Main.tile[tileX, tileY - 2].IsActuated && Main.tileSolid[Main.tile[tileX, tileY-2].TileType])
						{
							if (Main.tile[tileX, tileY - 3].IsActuated && Main.tileSolid[Main.tile[tileX, tileY - 3].TileType])
								NPC.velocity.Y = -8;
							else
								NPC.velocity.Y = -7;
							
							NPC.netUpdate = true;
						}
						else if(Main.tile[tileX, tileY-1].IsActuated && Main.tileSolid[Main.tile[tileX, tileY - 1].TileType])
						{
							NPC.velocity.Y = -6;
							NPC.netUpdate = true;
						}
						else if (NPC.position.Y + NPC.height - (tileY * 16) > 20 && Main.tile[tileX, tileY].IsActuated && (!Main.tile[tileX, tileY].TopSlope && Main.tileSolid[(int)Main.tile[tileX, tileY].TileType]))
						{
							NPC.velocity.Y = -5;
							NPC.netUpdate = true;
						}
						else if (NPC.directionY < 0 && (!Main.tile[tileX, tileY + 1].IsActuated || !Main.tileSolid[(int)Main.tile[tileX, tileY + 1].TileType]) && (!Main.tile[tileX + NPC.direction, tileY + 1].IsActuated || !Main.tileSolid[(int)Main.tile[tileX + NPC.direction, tileY + 1].TileType]))
						{
							NPC.velocity.Y = -8;
							NPC.velocity.X = NPC.velocity.X * 1.5F;
							NPC.netUpdate = true;
						}

						// Rework the following a bit. If the NPCs position has been static for a little while, jump.
						//if (NPC.velocity.Y == 0 && idle)
						//    NPC.velocity.Y = -5;
					}

					specialTimer++;

					// If no jump has been initiated yet.
					if(NPC.velocity.Y == 0)
					{
						// TEMPORARY.
						if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) &&
							specialTimer >= 60 && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) <= 180)
						{
							if (Main.rand.NextBool(3))
								NPC.ai[0] = 2;
							else
								NPC.ai[0] = 3;
							specialTimer = 0;
						}
					}
				}

				NPC.ai[1] = grounded ? 1 : 0;
			}
			if(NPC.ai[0] == 1) // Shooting. Do we even want this?
			{

			}
			if(NPC.ai[0] == 2) // Dropkick.
			{
				// Start the dropkick off with a small jump (almost) straight up.
				if(NPC.ai[2] == 0)
				{
					NPC.ai[1] = 0;
					NPC.ai[2] = 1;
					NPC.velocity.Y = -14;
					NPC.velocity.X *= 0;
				}
				else if (NPC.ai[2] == 1)
				{
					NPC.velocity.Y *= .95F;

					if (NPC.ai[1]++ >= 40 || NPC.velocity.Y >= 0)
					{
						NPC.TargetClosest();
						Vector2 newVelocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 10;
						NPC.velocity = newVelocity;

						NPC.ai[1] = 0;
						NPC.ai[2] = 2;
					}
				}
				else
				{
					if(NPC.velocity.Y == 0)
					{
						NPC.velocity.X = 0;
						if(NPC.ai[1] == 0)
						{
							var entitySource = NPC.GetSource_FromAI();
							// projectile spawning?
							for(int i = 0; i < 4; ++i)
							{
								if (Main.netMode != 1)
									Projectile.NewProjectile(entitySource, NPC.Center, new Vector2(Main.rand.Next(-40, 41) * .1F, -5), ModContent.ProjectileType<Projectiles.Mobs.SkreeRock>(), NPC.damage, 1.2F);
							}
						}
						if(NPC.ai[1]++ >= 40)
						{
							NPC.ai[0] = 0;
							NPC.ai[1] = 0;
							NPC.ai[2] = 0;
						}
					}
				}
			}
			if(NPC.ai[0] == 3) // Overhead jump + projectiles.
			{
				// This part is... Slightly hacky to make animations smooth. Feel free to refactor.
				NPC.ai[1]++;
				if (NPC.ai[1] <= 12)
				{
					if (NPC.velocity.X < -1 || NPC.velocity.X > 1)
						NPC.velocity.X *= .95F;
					if (NPC.ai[1] == 12)
					{
						NPC.velocity.Y = -9;
						NPC.velocity.X = 8 * NPC.direction;
					}
				}
				else if(NPC.ai[1] >= 13)
				{
					NPC.rotation += .5F * NPC.direction;

					if (NPC.ai[2]++ >= 20 && Main.netMode != 1)
					{
						Vector2 projVelocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 6;

						var entitySource = NPC.GetSource_FromAI();
						Projectile.NewProjectile(entitySource, NPC.Center, projVelocity, ModContent.ProjectileType<Projectiles.Mobs.SpacePirateClaw>(), NPC.damage, 1, Main.myPlayer);

						NPC.ai[2] = 0;
					}

					if (NPC.velocity.Y == 0)
					{
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
					}
				}
			}
			else
				NPC.rotation = 0;

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if(NPC.ai[0] == 0)
			{
				if (NPC.velocity.Y != 0)
					NPC.frame.Y = 14 * frameHeight;
				else
				{
					NPC.frameCounter += Math.Abs(NPC.velocity.X * .45F);
					if (NPC.frameCounter >= 6) { 
						NPC.frame.Y = (NPC.frame.Y + frameHeight) % (4 * frameHeight);
						NPC.frameCounter = 0;
					}
				}
			}
			else if(NPC.ai[0] == 1)
			{

			}
			else if(NPC.ai[0] == 2)
			{
				if (NPC.ai[2] == 0)
					NPC.frame.Y = 14 * frameHeight;
				else
					NPC.frame.Y = 15 * frameHeight;
			}
			else if(NPC.ai[0] == 3)
			{
				if (NPC.ai[1] < 4)
					NPC.frame.Y = 9 * frameHeight;
				else if (NPC.ai[1] < 8)
					NPC.frame.Y = 10 * frameHeight;
				else if (NPC.ai[1] < 12)
					NPC.frame.Y = 11 * frameHeight;
				else
					NPC.frame.Y = 12 * frameHeight;
			}

			NPC.spriteDirection = NPC.direction;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			for(int i = 0; i < 5; ++i)
			{
				Vector2 drawPos = NPC.oldPos[i] + new Vector2(NPC.width / 2, NPC.height / 2) - Main.screenPosition;
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[Type].Value, drawPos, NPC.frame, Color.White * (.9F - .1F * i));
			}
			return true;
		}
	}
}
