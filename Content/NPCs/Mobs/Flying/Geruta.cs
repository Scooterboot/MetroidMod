using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Flying
{
	public class Geruta : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.
		 * NPC.ai[1] = timer.
		 */

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 8;
		}
		public override void SetDefaults()
		{
			NPC.width = 42; NPC.height = 23;

			/* Temporary NPC values */
			NPC.scale = 1.5F;
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
				new FlavorTextBestiaryInfoElement("An organism capable of flying up to ceilings or floors when crossing its path. Nothing more than a nuisance.")
			});
		}

		public override bool PreAI()
		{
			if(NPC.ai[0] == 0)
			{
				NPC.velocity = Vector2.Zero;

				if(NPC.ai[1]++ >= 30) // Jump countdown.
				{
					NPC.TargetClosest();

					Vector2 targetVelocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center);

					NPC.velocity.X = targetVelocity.X * 3;
					NPC.velocity.Y = Main.rand.Next(6, 10);

					NPC.ai[0] = 1;
					NPC.ai[1] = 0;
				}
			}
			else
			{
				if(NPC.collideX) // If collide on the X axis, turn the X velocity around and cut it a bit.
				{
					NPC.velocity.X *= -.8F;
					NPC.netUpdate = true;
				}

				if(NPC.collideY) // If collide on the Y axis, see if we've landed or should bounce back.
				{
					if(NPC.oldVelocity.Y > 0)
						NPC.velocity.Y = 0;
					else
					{
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;

						NPC.velocity *= 0;
						NPC.netUpdate = true;

						return false;
					}
				}

				// If we're 'falling' upwards, make sure to time the fall so the fall isn't endless.
				if(NPC.velocity.Y < 0)
				{
					if (NPC.oldVelocity.Y >= 0)
						NPC.ai[2] = 0;
					
					if(NPC.ai[1]++ >= 120)
					{
						NPC.ai[0] = 1;
						NPC.ai[1] = 1;
						NPC.ai[2] = 0;

						NPC.velocity.Y = Main.rand.Next(6, 10);
						NPC.netUpdate = true;
					}

					NPC.ai[2]++;
				}
				else
					NPC.ai[2]++;

				NPC.velocity.Y -= .2F;
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			bool spawnDusts = false;
			if (NPC.ai[0] == 0)
				NPC.frame.Y = 0;
			else
			{
				if (NPC.velocity.Y < 0)
				{
					if (NPC.ai[1] >= 102)
					{
						if (NPC.ai[2] < 108)
							NPC.frame.Y = 4 * frameHeight;
						else if (NPC.ai[2] < 114)
							NPC.frame.Y = 5 * frameHeight;
						else
						{
							NPC.frame.Y = 6 * frameHeight;
							spawnDusts = true;
						}
					}
					else
					{
						if (NPC.ai[2] < 6)
							NPC.frame.Y = 4 * frameHeight;
						else if (NPC.ai[2] < 12)
							NPC.frame.Y = 5 * frameHeight;
						else
						{
							NPC.frame.Y = 6 * frameHeight;
							spawnDusts = true;
						}
					}
				}
				else
				{
					if (NPC.velocity.Y <= 2)
						NPC.frame.Y = frameHeight;
					else
					{
						if (NPC.ai[2] < 8)
							NPC.frame.Y = frameHeight;
						else
							NPC.frame.Y = 2 * frameHeight;
					}
					spawnDusts = true;
				}
			}

			/* Dust spawning */
			if (!spawnDusts) return;

			Vector2[] dustSides = new Vector2[2] { new Vector2(10, 0), new Vector2(22, 0) };
			dustSides[0].Y = dustSides[1].Y = NPC.frame.Y == 6 * frameHeight ? 16 : -4;
			for(int i = 0; i < dustSides.Length; ++i)
			{
				for(int j = 0; j < 5; ++j)
				{
					int newDust = Dust.NewDust(NPC.position + dustSides[i] * NPC.scale, (int)(6 * NPC.scale), (int)(6 * NPC.scale), DustID.Torch, 0, -NPC.velocity.Y * .3F);
					Main.dust[newDust].noGravity = true;
				}
			}
		}
	}
}
