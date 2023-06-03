using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Utility
{
	public class Powamp : MNPC
	{
		/*
		 * NPC.ai[0] = player proximity timer.
		 * NPC.ai[1] = NPC x velocity (cos)
		 * NPC.ai[2] = NPC y velocity (sin)
		 */
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 7;
		}
		public override void SetDefaults()
		{
			NPC.width = 14; NPC.height = 36;

			/* Temporary NPC values */
			NPC.scale = 1.5F;
			NPC.damage = 0;
			NPC.lifeMax = 1;
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
				new FlavorTextBestiaryInfoElement("An aquatic balloon-like creature that's useful as a grappling point.")
			});
		}

		public override bool PreAI()
		{
			NPC.TargetClosest(false);
			if (Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) <= 240)
				NPC.ai[0] += (NPC.ai[0] < 46 ? 1 : 0);
			else
				NPC.ai[0] -= (NPC.ai[0] > 0 ? 1 : 0);

			NPC.ai[1] += .06F;
			NPC.velocity.X = (float)Math.Cos(NPC.ai[1]) * .4F;

			NPC.ai[2] += .03F;
			NPC.velocity.Y = (float)Math.Sin(NPC.ai[2]) * .3F;
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] <= 20)
			{
				if (NPC.frameCounter++ >= 15)
				{
					NPC.frame.Y = (NPC.frame.Y + frameHeight) % (3 * frameHeight);
					NPC.frameCounter = 0;
				}
			}
			else if (NPC.ai[0] > 20 && NPC.ai[0] <= 35)
				NPC.frame.Y = 3 * frameHeight;
			else
			{
				if (NPC.frameCounter++ >= 15)
				{
					NPC.frame.Y = NPC.frame.Y + frameHeight;
					if (NPC.frame.Y >= 7 * frameHeight)
						NPC.frame.Y = 4 * frameHeight;
					NPC.frameCounter = 0;
				}
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				SoundEngine.PlaySound(SoundID.Item62, NPC.position);
				for (int i = 0; i < 30; i++)
				{
					int num727 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 31, 0f, 0f, 100, default(Color), 1.5f);
					Dust dust = Main.dust[num727];
					dust.velocity *= 1.4f;
				}
				for (int i = 0; i < 20; i++)
				{
					int num729 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 3.5f);
					Main.dust[num729].noGravity = true;
					Dust dust = Main.dust[num729];
					dust.velocity *= 7f;
					num729 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 1.5f);
					dust = Main.dust[num729];
					dust.velocity *= 3f;
				}
			}
		}
	}
}
