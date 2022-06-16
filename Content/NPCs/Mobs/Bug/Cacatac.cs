using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Bug
{
	public class Cacatac : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.  
		 */

		public bool spawn = false;
		internal readonly float speed = .75F;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 11;
		}
		public override void SetDefaults()
		{
			NPC.width = 24; NPC.height = 34;

			/* Temporary NPC values */
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A hostile cactus like creature that shoots spikes when attacked.")
			});
		}

		public override bool PreAI()
		{
			if (!spawn)
			{
				NPC.scale = (Main.rand.Next(13, 21) * 0.1f);
				NPC.defense = (int)((float)NPC.defense * NPC.scale);
				NPC.damage = (int)((float)NPC.damage * NPC.scale);
				NPC.life = (int)((float)NPC.life * NPC.scale);
				NPC.lifeMax = NPC.life;
				NPC.value = (float)((int)(NPC.value * NPC.scale));
				NPC.npcSlots *= NPC.scale;
				NPC.knockBackResist *= 2f - NPC.scale;
				spawn = true;
			}
			return true;
		}

		public override void AI()
		{
			if(NPC.ai[0] == 0) // Movement stage.
			{
				NPC.TargetClosest(true);

				Player p = Main.player[NPC.target];
				if (Vector2.Distance(p.position, NPC.position) <= 160 &&
					Collision.CanHit(NPC.position, NPC.width, NPC.height, p.position, p.width, p.height) &&
					NPC.velocity.Y == 0)
				{
					NPC.ai[0] = 1; // Set projectile stage for NPC.
					return;
				}

				NPC.velocity.X = NPC.direction * speed;
			}
			if(NPC.ai[0] == 1) // Projectile stage.
			{
				NPC.ai[1]++;
				NPC.velocity = Vector2.Zero;

				if(Main.netMode != 1 && NPC.ai[1] >= 60)
				{
					var entitySource = NPC.GetSource_FromAI();
					// Fire projectiles.
					for (int i = 0; i < 5; ++i)
					{
						float value = (float)(Math.PI - ((Math.PI / 4) * i));
						Vector2 v2 = new Vector2((float)Math.Cos(value), -(float)Math.Sin(value));
						Projectile.NewProjectile(entitySource, NPC.Center, v2 * 6, ModContent.ProjectileType<Projectiles.Mobs.CacatacSpike>(), NPC.damage, 0, Main.myPlayer);
					}

					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.netUpdate = true;
				}
			}

			return;
		}

		public override void FindFrame(int frameHeight)
		{
			if(!NPC.collideX && !NPC.collideY) // NPC is aired.
				NPC.frame.Y = 7 * frameHeight; // Idle.
			else if(NPC.ai[0] == 0)
			{
				NPC.frameCounter += Math.Abs(NPC.velocity.X) / 3.5F;

				NPC.frameCounter %= (Main.npcFrameCount[NPC.type] - 4); // If the frameCounter exceeds the amount of frames available, reset it to 0.
				int frame = (int)(NPC.frameCounter);
				NPC.frame.Y = frame * frameHeight;
			}
			else if(NPC.ai[0] == 1)
			{

				if (NPC.ai[1] < 20)
					NPC.frame.Y = 8 * frameHeight;
				else if (NPC.ai[1] < 40)
					NPC.frame.Y = 9 * frameHeight;
				else if (NPC.ai[1] < 60)
					NPC.frame.Y = 10 * frameHeight;
			}

			NPC.spriteDirection = NPC.direction;
		}
	}
}
