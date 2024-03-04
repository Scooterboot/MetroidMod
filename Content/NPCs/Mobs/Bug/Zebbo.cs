using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Bug
{
	public class Zebbo : MNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 4;
		}
		public override void SetDefaults()
		{
			NPC.width = 16; NPC.height = 16;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.aiStyle = -1;
			NPC.lifeMax = 150;
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
				new FlavorTextBestiaryInfoElement("A flying bug. They're quite plentiful and tend to appear whenever anything gets near the air pockets they inhabit.")
			});
		}

		public override bool PreAI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
				NPC.TargetClosest();

			float speedMultiplier = 6.5F;
			float speedBuildup = .051F;

			Vector2 npcCenter = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
			Vector2 targetVelocity = new Vector2(Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2),
											   Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2));
			targetVelocity.X -= npcCenter.X;
			targetVelocity.Y -= npcCenter.Y;
			float magnitude = (float)Math.Sqrt(targetVelocity.X * targetVelocity.X + targetVelocity.Y * targetVelocity.Y);
			magnitude = speedMultiplier / magnitude;

			targetVelocity.X *= magnitude;
			targetVelocity.Y *= magnitude;

			if (Main.player[NPC.target].dead)
			{
				targetVelocity.X = NPC.direction * speedMultiplier / 2;
				targetVelocity.Y = -speedMultiplier / 2;
			}

			if (NPC.velocity.X < targetVelocity.X)
				NPC.velocity.X += speedBuildup;
			else if (NPC.velocity.X > targetVelocity.X)
				NPC.velocity.X -= speedBuildup;

			if (NPC.velocity.Y < targetVelocity.Y)
				NPC.velocity.Y += speedBuildup;
			else if (NPC.velocity.Y > targetVelocity.Y)
				NPC.velocity.Y -= speedBuildup;

			if (NPC.collideX)
			{
				NPC.netUpdate = true;
				NPC.velocity.X = NPC.oldVelocity.X * -.7F;
			}
			if (NPC.collideY)
			{
				NPC.netUpdate = true;
				NPC.velocity.Y = NPC.oldVelocity.Y * -.7F;
			}

			if (NPC.wet) // Wet flight handling.
			{
				if (NPC.velocity.Y > 0)
					NPC.velocity.Y *= .95F;
				NPC.velocity.Y -= .5F;
				if (NPC.velocity.Y < -4)
					NPC.velocity.Y = -4;

				NPC.TargetClosest();
			}

			if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) || (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) && !NPC.justHit)
				NPC.netUpdate = true;

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 2)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
				NPC.frameCounter = 0;
			}

			// Visual NPC styling.
			if (NPC.velocity.X > 0f)
				NPC.spriteDirection = 1;
			if (NPC.velocity.X < 0f)
				NPC.spriteDirection = -1;
			NPC.rotation = NPC.velocity.X * 0.1f;
		}
	}
}
