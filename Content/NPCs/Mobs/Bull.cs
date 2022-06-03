using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.NPCs.Mobs
{
	public class Bull : MNPC
	{
		/*
		 * NPC.ai[0] = dashing logic. 
		 */
		 
		internal readonly float speed = 1.2F;
		internal readonly float dashSpeed = 6F;
		internal readonly float acceleration = .08F;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 6;
		}
		public override void SetDefaults()
		{
			NPC.width = 24; NPC.height = 30;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override bool PreAI()
		{
			NPC.TargetClosest();

			Player p = Main.player[NPC.target];

			// Float logic.
			Vector2 targetVelocity = Vector2.Normalize(p.Center - NPC.Center) * speed;

			if (NPC.velocity.X < targetVelocity.X)
				NPC.velocity.X += acceleration;
			if (NPC.velocity.X > targetVelocity.X)
				NPC.velocity.X -= acceleration;

			if (NPC.velocity.Y < targetVelocity.Y)
				NPC.velocity.Y += acceleration;
			if (NPC.velocity.Y > targetVelocity.Y)
				NPC.velocity.Y -= acceleration;

			// Dash logic.
			if (Vector2.Distance(NPC.Center, p.Center) <= 170)
			{
				if (NPC.ai[0]++ >= 60)
				{
					NPC.velocity = targetVelocity * dashSpeed;
					NPC.ai[0] = 0;
				}
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 4)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (6 * frameHeight);
				NPC.frameCounter = 0;
			}
		}
	}
}
