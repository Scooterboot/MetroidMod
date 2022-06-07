using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs.Aquatic
{
	public class Evir : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.
		 * NPC.ai[1] = timer.
		 * NPC.ai[2] = projectile index.
		 */
		internal readonly float shootSpeed = 10;
		internal readonly Vector2 projectilePosition = new Vector2(16, 35);
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 4;
		}
		public override void SetDefaults()
		{
			NPC.width = 30; NPC.height = 34;

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
				new FlavorTextBestiaryInfoElement("The spawn of a large creature that likes to trap its prey by getting it stuck to itself.")
			});
		}

		public override bool PreAI()
		{
			NPC.TargetClosest();

			if(NPC.ai[0] == 0)
			{
				if (NPC.ai[1]++ >= 120)
				{
					NPC.ai[0] = 1;
					NPC.ai[1] = 0;
					NPC.ai[2] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Mobs.EvirProjectile>(), NPC.damage, .3F, Main.myPlayer, NPC.whoAmI);
					Main.projectile[(int)NPC.ai[2]].scale = NPC.scale;
				}
			}
			else
			{
				Projectile projectile = Main.projectile[(int)NPC.ai[2]];
				projectile.position = NPC.position + (projectilePosition * NPC.scale) - new Vector2(NPC.direction == -1 ? projectile.width+2 : -2, projectile.height / 2);

				if(NPC.ai[1]++ >= 60)
				{
					Vector2 shootDirection = Vector2.Normalize(Main.player[NPC.target].Center - projectile.Center) * shootSpeed;
					projectile.velocity = shootDirection;

					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.ai[2] = -1;
				}
			}

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 12)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (4 * frameHeight);
				NPC.frameCounter = 0;
			}

			NPC.spriteDirection = -NPC.direction;
		}
	}
}
