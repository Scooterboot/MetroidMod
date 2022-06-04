using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalNPCs;

namespace MetroidModPorted.Content.NPCs.Mobs
{
	public class Puyo : MNPC
	{
		private int newXFrame = -1;
		private float newScale = -1;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 9;
		}
		public override void SetDefaults()
		{
			NPC.width = 12; NPC.height = 6;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = .2F;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;

			if (Main.rand != null && Main.netMode != NetmodeID.MultiplayerClient)
			{
				newScale = (Main.rand.Next(13, 21) * 0.1f);
				newXFrame = Main.rand.Next(0, 3) * 30;
			}
		}

		public bool spawn = false;
		public override bool PreAI()
		{
			if (!spawn && newScale != -1 && newXFrame != -1)
			{
				SetStats();
				spawn = true;
				NPC.netUpdate = true;
			}
			return true;
		}

		public override void AI()
		{
			if (NPC.GetGlobalNPC<MGlobalNPC>().froze)
			{
				NPC.position = NPC.oldPosition;
				return;
			}

			if (NPC.velocity.Y == 0)
			{
				NPC.TargetClosest();

				NPC.ai[0]++;
				if (Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) <= 80)
					NPC.ai[0]++;

				if (NPC.velocity.X != 0)
				{
					NPC.velocity.X *= .1F;
					if (NPC.velocity.X >= -.1F && NPC.velocity.X <= .1F)
						NPC.velocity.X = 0;
				}

				if (NPC.ai[0] >= 120)
				{
					NPC.velocity.X = Main.rand.Next(4, 8) * NPC.direction;
					NPC.velocity.Y = Main.rand.Next(-7, -3);

					NPC.ai[0] = 0;
					NPC.ai[1] = Main.rand.Next(20, 61); // Next jump time.
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			//NPC.visualOffset = new Vector2(60, 0);

			if (NPC.GetGlobalNPC<MGlobalNPC>().froze) return;

			if (NPC.velocity.Y == 0)
			{
				NPC.frame.Y = (int)((Math.Round(NPC.ai[0] / 10) % 3) * frameHeight);

				NPC.frameCounter = 0;
			}
			else
			{
				NPC.frameCounter++;
				if (NPC.frameCounter < 5)
					NPC.frame.Y = 4 * frameHeight;
				else if (NPC.frameCounter < 10)
					NPC.frame.Y = 5 * frameHeight;
				else if (NPC.frameCounter < 15)
					NPC.frame.Y = 6 * frameHeight;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[Type].Value;
			SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 origin = new Vector2((texture.Width / 3) / 2, (texture.Height / Main.npcFrameCount[NPC.type]) / 2);

			Vector2 drawPos = NPC.Center - Main.screenPosition;
			drawPos -= new Vector2(texture.Width, (texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2;
			drawPos += origin * NPC.scale;

			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, effects, 0);

			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			position.X -= 45;
			return (true);
		}

		private void SetStats()
		{
			NPC.scale = newScale;
			NPC.defense = (int)(NPC.defense * NPC.scale);
			NPC.damage = (int)(NPC.damage * NPC.scale);
			NPC.life = (int)(NPC.life * NPC.scale);
			NPC.lifeMax = NPC.life;
			NPC.value = ((int)(NPC.value * NPC.scale));
			NPC.npcSlots *= NPC.scale;
			NPC.knockBackResist *= 2f - NPC.scale;

			NPC.frame.Width = 30;
			NPC.frame.X = newXFrame;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(newXFrame);
			writer.Write((double)newScale);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			newXFrame = reader.ReadInt32();
			newScale = (float)reader.ReadDouble();
		}
	}
}
