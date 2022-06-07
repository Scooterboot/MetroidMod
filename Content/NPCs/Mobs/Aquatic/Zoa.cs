using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Aquatic
{
	public class Zoa : MNPC
	{
		/*
		 * NPC.ai[0] = idle/notarget velocity manager.
		 * NPC.ai[1] = dash timer.
		 */

		internal readonly float dashSpeed = 8;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 4;
		}
		public override void SetDefaults()
		{
			NPC.width = NPC.height = 16;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("An aquatic fish that is rather small.")
			});
		}

		public override bool PreAI()
		{
			if (NPC.direction == 0)
				NPC.TargetClosest();

			if (NPC.wet)
			{
				bool followPlayer = false;
				NPC.TargetClosest(false);
				if (Main.player[NPC.target].wet && !Main.player[NPC.target].dead)
					followPlayer = true;

				if (followPlayer) // Follow target.
				{
					NPC.TargetClosest(true);
					
					if(NPC.ai[1]-- <= -30)
					{
						Vector2 dashVelocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * dashSpeed;
						NPC.velocity = dashVelocity;

						// Setup dash dusts.
						for(int i = 0; i < 8; ++i)
						{
							dashVelocity *= .5F;
							int newDust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.BubbleBlock, -dashVelocity.X, -dashVelocity.Y);
							Main.dust[newDust].noGravity = true;
						}

						NPC.ai[1] = 50;
					}

					NPC.velocity.X += NPC.direction * .15F;
					NPC.velocity.Y += NPC.directionY * .15F;

					if (NPC.ai[1] <= 0)
					{
						NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -5, 5);
						NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -3, 3);
					}
				}
				else // Idle/swim around; no target. 
				{
					if (NPC.collideX)
					{
						NPC.velocity.X = -NPC.velocity.X;
						NPC.direction *= -1;
						NPC.netUpdate = true;
					}
					if (NPC.collideY)
					{
						NPC.velocity.Y = -NPC.velocity.Y;
						NPC.directionY = -NPC.directionY;
						NPC.ai[0] = NPC.direction;
						NPC.netUpdate = true;
					}
					NPC.velocity.X += NPC.direction * .1F;
					if (NPC.velocity.X < -1 || NPC.velocity.X > 1)
						NPC.velocity.X *= .95F;

					if (NPC.ai[0] == -1)
					{
						NPC.velocity.Y -= .01F;
						if (NPC.velocity.Y < -.3F)
							NPC.ai[0] = 1;
					}
					else
					{
						NPC.velocity.Y += .01F;
						if (NPC.velocity.Y > .3F)
							NPC.ai[0] = -1;
					}
					if (NPC.velocity.Y > .4F || NPC.velocity.Y < -.4F)
						NPC.velocity.Y *= .95F;

					/* Water check */
					int npcTilePosX = (int)(NPC.position.X + (NPC.width / 2)) / 16;
					int npcTilePosY = (int)(NPC.position.Y + (NPC.height / 2)) / 16;

					/*for(int y = npcTilePosY - 1; y < npcTilePosY + 2; ++y)
					{
						if (Main.tile[npcTilePosX, y] == null)
							Main.tile[npcTilePosX, y] = new Tile();
					}*/

					if (Main.tile[npcTilePosX, npcTilePosY - 1].LiquidAmount > 128) // If the tile below the NPC is at least half full of liquid.
					{
						if (Main.tile[npcTilePosX, npcTilePosY + 1].HasTile || Main.tile[npcTilePosX, npcTilePosY + 2].HasTile)
							NPC.ai[0] = -1;
					}
				}
			}
			else
			{
				if (NPC.velocity.Y == 0)
				{
					NPC.velocity.X *= .94F;
					if (NPC.velocity.X > -.2F && NPC.velocity.X < .2F)
						NPC.velocity.X = 0;
				}

				NPC.velocity.Y += .3F;
				if (NPC.velocity.Y > 10)
					NPC.velocity.Y = 10;
				NPC.ai[0] = 1;
			}

			NPC.rotation = NPC.velocity.Y * NPC.direction * .1F;
			NPC.rotation = MathHelper.Clamp(NPC.rotation, -.3F, .3F);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[1] <= 0 && NPC.frameCounter++ >= 4)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (3 * frameHeight);
				NPC.frameCounter = 0;
			}
			else if (NPC.ai[1] > 0)
				NPC.frame.Y = 3 * frameHeight;

			NPC.spriteDirection = -NPC.direction;
		}
	}
}
