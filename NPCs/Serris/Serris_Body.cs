using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Serris
{
    public class Serris_Body : Serris
    {
		protected NPC head
		{
			get { return Main.npc[npc.realLife]; }
		}
		protected NPC start
		{
			get { return Main.npc[(int)npc.ai[1]]; }
		}
		protected NPC end
		{
			get { return Main.npc[(int)npc.ai[0]]; }
		}

		public override bool Autoload(ref string name)
		{
			return (true);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 10;
		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 20;
			npc.defense = 28;
			npc.lifeMax = 500;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
		}

		public override void AI()
		{
			Update_Worm();
			npc.damage = head.damage;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					int gore = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SerrisGore2"), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D texBody = mod.GetTexture("NPCs/Serris/Serris_Body"),
				texFins = mod.GetTexture("NPCs/Serris/Serris_Fins");
			Serris_Head serris_head = (Serris_Head)head.modNPC;

			Vector2 bodyOrig = new Vector2(32, 35),
				finsOrig = new Vector2(52, 31);
			int bodyHeight = texBody.Height / 10,
				finsHeight = texFins.Height / 15;

			float bodyRot = npc.rotation - 1.57f;
			float headRot = head.rotation - 1.57f;
			Color bodyColor = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));

			SpriteEffects effects = SpriteEffects.None;
			if (head.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipVertically;
				bodyOrig.Y = bodyHeight - bodyOrig.Y;
				finsOrig.Y = finsHeight - finsOrig.Y;
			}
			int frame = serris_head.state - 1;
			if (serris_head.state == 4)
				frame = serris_head.sbFrame + 3;

			// If it's the last body part before the head, draw the fins.
			int yFrame = frame * (bodyHeight * 2);
			if ((int)npc.ai[1] == head.whoAmI)
			{
				for (int j = 0; j < 3; j++)
				{
					int finFrame = finsHeight * j + frame * (finsHeight * 3);
					Vector2 finPos = new Vector2(4, -16);
					float bodyRot2 = bodyRot - (headRot - bodyRot);
					Vector2 finRotPos = bodyRot.ToRotationVector2();
					if (float.IsNaN(finRotPos.X) || float.IsNaN(finRotPos.Y))
					{
						finRotPos = -Vector2.UnitY;
					}
					if (j == 0)
					{
						finPos = new Vector2(-14, -14);
						finRotPos = Vector2.Normalize(Vector2.Lerp(finRotPos, bodyRot2.ToRotationVector2(), 0.5f));
					}
					if (j == 2)
					{
						finPos = new Vector2(20, -16);
						finRotPos = Vector2.Normalize(Vector2.Lerp(finRotPos, headRot.ToRotationVector2(), 0.5f));
					}
					if (head.spriteDirection == -1)
					{
						finPos.Y *= -1;
					}
					float finRot = finRotPos.ToRotation();
					finRot += (((float)Math.PI / 16) - ((float)Math.PI / 8) * (1f - serris_head.mouthFrame)) * 0.5f * head.spriteDirection;
					float finPosRot = finPos.ToRotation() + bodyRot;
					Vector2 finalFinPos = npc.Center + finPosRot.ToRotationVector2() * finPos.Length();
					sb.Draw(texFins, finalFinPos - Main.screenPosition, new Rectangle?(new Rectangle(0, finFrame, texFins.Width, finsHeight)),
					bodyColor, finRot, finsOrig, 1f, effects, 0f);
				}
			}
			else
				yFrame += bodyHeight;

			sb.Draw(texBody, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, yFrame, texBody.Width, bodyHeight)),
			bodyColor, bodyRot, bodyOrig, 1f, effects, 0f);

			return (false);
		}
	}
}
