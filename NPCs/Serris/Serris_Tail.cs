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
    public class Serris_Tail : ModNPC
    {
        bool SpeedBoost = false;
		int sptDir = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[npc.type] = 3;
		}
		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 46;
			npc.damage = 20;
			npc.defense = 18;
			npc.lifeMax = 500;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 5, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.frameCounter = 0;
			npc.aiStyle = 6;
			npc.npcSlots = 1;
		}
		public override void AI()
		{
			if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].life <= (Main.npc[(int)npc.ai[1]].lifeMax/2))
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
			}
			foreach(NPC N in Main.npc) if (N!= null)
			{
				if(N.active && N.type == mod.NPCType("Serris_Head"))
				{
					sptDir = N.spriteDirection;
					if(N.dontTakeDamage)
					{
						SpeedBoost = true;
						return;
					}
					else
					{
						SpeedBoost = false;
					}
				}
			}
			if(SpeedBoost)
			{
				npc.damage = 60;
			}
			else
			{
				npc.damage = 20;
			}
		}
		/*public override void PostAI()
		{
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = 1;
			}
			else if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = -1;
			}
		}*/
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (sptDir == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			else
			{
				effects = SpriteEffects.None;
			}
			Color buffColor = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			Color alpha2 = npc.GetAlpha(buffColor);
			if (npc.behindTiles)
			{
				int num44 = (int)((npc.position.X - 8f) / 16f);
				int num45 = (int)((npc.position.X + (float)npc.width + 8f) / 16f);
				int num46 = (int)((npc.position.Y - 8f) / 16f);
				int num47 = (int)((npc.position.Y + (float)npc.height + 8f) / 16f);
				for (int m = num44; m <= num45; m++)
				{
					for (int n = num46; n <= num47; n++)
					{
						if (Lighting.Brightness(m, n) == 0f)
						{
							buffColor = Color.Black;
						}
					}
				}
			}
			Texture2D tex2 = Main.npcTexture[npc.type];
			Rectangle rect2 = new Rectangle((int)npc.frame.X, (int)npc.frame.Y, (tex2.Width/3), (tex2.Height/Main.npcFrameCount[npc.type]));
			Vector2 vector13 = new Vector2((float)((tex2.Width/3) / 2), (float)((tex2.Height/Main.npcFrameCount[npc.type]) / 2));
			sb.Draw(tex2, new Vector2(npc.position.X - Main.screenPosition.X + (float)(npc.width / 2) - (float)(tex2.Width/3) / 2f + vector13.X, npc.position.Y - Main.screenPosition.Y + (float)npc.height - (float)(tex2.Height / Main.npcFrameCount[npc.type]) + 4f + vector13.Y), new Rectangle?(rect2), alpha2, npc.rotation, vector13, 1f, effects, 0f);
			return false;
		}
		public override void FindFrame(int frameHeight)
		{
			
			int num = 1;
			if (!Main.dedServ)
			{
				num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			}
			npc.frameCounter += 1;
			if(SpeedBoost)
			{
				Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 1.0f, 1.0f, 1.0f);
				if(npc.frameCounter >= 0 && npc.frameCounter < 5)
				{
					npc.frame.Y = num;
				}
				if(npc.frameCounter >= 5 && npc.frameCounter < 10)
				{
					npc.frame.Y = num*2;
				}
				if(npc.frameCounter >= 10)
				{
					npc.frameCounter = 0;
				}
			}
			else
			{
				npc.frame.Y = 0;
				npc.frameCounter = 0;
			}
			int num2 = 1;
			if (!Main.dedServ)
			{
				num2 = Main.npcTexture[npc.type].Width / 3;
			}
			foreach(NPC N in Main.npc) if (N!= null)
			{
				if(N.active && N.type == mod.NPCType("Serris_Head"))
				{
					if(N.life <= (int)(N.lifeMax) && N.life >= (int)(N.lifeMax * 0.8f))
					{
						npc.frame.X = 0;
					}
					if(N.life < (int)(N.lifeMax * 0.8f) && N.life >= (int)(N.lifeMax * 0.6f))
					{
						npc.frame.X = num2;
					}
					if(N.life < (int)(N.lifeMax * 0.6f))
					{
						npc.frame.X = num2*2;
					}
				}
			}
			
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					int gore = Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("SerrisGore3"), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
	}
}