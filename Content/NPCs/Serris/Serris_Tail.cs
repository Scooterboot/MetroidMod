using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MetroidModPorted.Content.NPCs.Serris
{
    public class Serris_Tail : Serris_Body
    {
		private int tailType
		{
			get { return (int)NPC.ai[2]; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[Type] = 15;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 32;
			NPC.height = 32;
		}
		public override bool PreAI()
		{
			if(tailType > 0)
			{
				NPC.width = 20;
				NPC.height = 20;
			}
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (NPC.life <= 0)
				{
					int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SerrisGore3").Type, 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D texTail = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/Serris_Tail").Value;
			Serris_Head serris_head = (Serris_Head)head.ModNPC;

			float bRot = NPC.rotation - 1.57f;
			int tailHeight = texTail.Height / 15;
			Vector2 tailOrig = new Vector2(28, 29);
			Color bodyColor = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));

			SpriteEffects effects = SpriteEffects.None;
			if (head.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipVertically;
				tailOrig.Y = tailHeight - tailOrig.Y;
			}
			int frame = serris_head.state - 1;
			if (serris_head.state == 4)
				frame = serris_head.sbFrame + 3;

			int yFrame = frame * (tailHeight * 3) + (tailHeight * tailType);
			sb.Draw(texTail, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, yFrame, texTail.Width, tailHeight)),
			bodyColor, bRot, tailOrig, 1f, effects, 0f);
			return (false);
		}
	}
}
