using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Flying
{
	public class Skree : MNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 28;

			/* Temporary NPC values */
			NPC.scale = 1.5F;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 20;
			NPC.knockBackResist = 0;

			NPC.npcSlots = .5F;
			NPC.behindTiles = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Skree")
			});
		}

		public override void AI()
		{
			NPC.noGravity = true;

			if (NPC.ai[0] == 0) // NPC is dormant/idle.
			{
				NPC.TargetClosest(true);

				Player p = Main.player[NPC.target];
				// Position and collision checks to initiate divebomb attack.
				if (p.Center.Y > NPC.Center.Y && p.position.X >= NPC.position.X - 100 && p.position.X <= NPC.position.X + 100 &&
					Collision.CanHit(NPC.position, NPC.width, NPC.height, p.position, p.width, p.height))
					NPC.ai[0] = 1;
			}

			if (NPC.ai[0] == 1) // NPC is flying down (divebomb).
			{
				NPC.TargetClosest(true);
				Player p = Main.player[NPC.target];


				if (NPC.position.X < p.position.X)
				{
					if (NPC.velocity.X < 0)
						NPC.velocity.X *= .98F;
					NPC.velocity.X += .3F;
				}
				else if (NPC.position.X > p.position.X)
				{
					if (NPC.velocity.X > 0)
						NPC.velocity.X *= .98F;
					NPC.velocity.X -= .3F;
				}
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -5, 5);

				NPC.velocity.Y = 6;
				if (NPC.collideY)
					NPC.ai[0] = 2;
			}

			if (NPC.ai[0] == 2) // NPC id burrowing.
			{
				NPC.noTileCollide = true;
				NPC.velocity.Y = .6F;
				NPC.velocity.X = 0;

				if (NPC.ai[1]++ <= 60) // 'Spew' projectiles.
				{
					if (Main.netMode != 1 && NPC.ai[1] % 15 == 0)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.Next(-40, 41) * .1F, -5), ModContent.ProjectileType<Projectiles.Mobs.SkreeRock>(), NPC.damage, 1.2F);
				}
				else
					NPC.active = false;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] == 0)
				NPC.frame.Y = 4 * frameHeight;
			else
			{
				if (NPC.frameCounter++ >= 3)
				{
					NPC.frame.Y = (NPC.frame.Y + frameHeight) % (4 * frameHeight);
					NPC.frameCounter = 0;
				}
			}
		}
	}
}
