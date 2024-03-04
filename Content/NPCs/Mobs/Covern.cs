using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs
{
	public class Covern : MNPC
	{
		/*
		 * NPC.ai[0] = state manager.
		 * NPC.ai[1] = glowmask opacity.
		 * NPC.ai[2] = bounce movement.
		 * NPC.ai[3] = state timer.
		 */
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 4;
		}
		public override void SetDefaults()
		{
			NPC.width = 30; NPC.height = 32;

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
			if (NPC.ai[0] == 0)
			{
				NPC.ai[0] = 1;
				NPC.ai[1] = 1;
			}

			if (NPC.ai[0] == 1)
			{
				if (NPC.ai[1] > 0)
					NPC.ai[1] -= .06F;
				else
					NPC.ai[1] = 0;

				if (NPC.ai[3]++ >= 180)
				{
					NPC.ai[0] = 2;
					NPC.ai[3] = 0;
				}
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.ai[1] < 1)
					NPC.ai[1] += .06F;
				else
				{
					NPC.ai[1] = 1;

					if (NPC.ai[3]++ >= 60)
					{
						NPC.TargetClosest(false);
						Vector2 newPos = Main.player[NPC.target].Center + new Vector2(Main.rand.Next(-160, 161), Main.rand.Next(-120, 40));

						NPC.position = newPos;
						NPC.ai[0] = 1;
						NPC.ai[3] = 0;
					}
				}
			}

			NPC.ai[2] += .2F;
			NPC.velocity.Y = (float)Math.Sin(NPC.ai[2]);
			NPC.dontTakeDamage = NPC.ai[1] >= 1 ? true : false;

			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ >= 6)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % ((Main.npcFrameCount[NPC.type] - 1) * frameHeight);
				NPC.frameCounter = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[1] >= 1)
				return false;

			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Mobs/Covern_Glowmask").Value;
			Vector2 drawPos = NPC.position - Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, ((texture.Height / (Main.npcFrameCount[NPC.type] - 1)) / 2));
			drawPos += origin * NPC.scale + new Vector2(0, 2 * NPC.scale);

			if (NPC.ai[1] < 1)
				spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(Color.White) * (float)NPC.ai[1], NPC.rotation, origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
	}
}
