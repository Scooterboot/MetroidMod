using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Puyo : ModNPC
    {
		private int newXFrame = -1;
		private float newScale = -1;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 9;            
        }
        public override void SetDefaults()
        {
            npc.width = 12; npc.height = 6;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = .2F;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;

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
				npc.netUpdate = true;
			}
            return true;
        }

        public override void AI()
        {
            if (npc.GetGlobalNPC<MNPC>().froze)
            {
                npc.position = npc.oldPosition;
                return;
            }

            if (npc.velocity.Y == 0)
            {
                npc.TargetClosest();

                npc.ai[0]++;
                if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= 80)
                    npc.ai[0]++;

                if (npc.velocity.X != 0)
                {
                    npc.velocity.X *= .1F;
                    if (npc.velocity.X >= -.1F && npc.velocity.X <= .1F)
                        npc.velocity.X = 0;
                }

                if (npc.ai[0] >= 120)
                {
                    npc.velocity.X = Main.rand.Next(4, 8) * npc.direction;
                    npc.velocity.Y = Main.rand.Next(-7, -3);

                    npc.ai[0] = 0;
                    npc.ai[1] = Main.rand.Next(20, 61); // Next jump time.
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.visualOffset = new Vector2(60, 0);

            if (npc.GetGlobalNPC<MNPC>().froze) return;

            if (npc.velocity.Y == 0)
            {
                npc.frame.Y = (int)((Math.Round(npc.ai[0] / 10) % 3) * frameHeight);

                npc.frameCounter = 0;
            }
            else
            {
                npc.frameCounter++;
                if (npc.frameCounter < 5)
                    npc.frame.Y = 4 * frameHeight;
                else if (npc.frameCounter < 10)
                    npc.frame.Y = 5 * frameHeight;
                else if (npc.frameCounter < 15)
                    npc.frame.Y = 6 * frameHeight;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            SpriteEffects effects = npc.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 origin = new Vector2((texture.Width / 3) / 2, (texture.Height / Main.npcFrameCount[npc.type]) / 2);

            Vector2 drawPos = npc.Center - Main.screenPosition;
            drawPos -= new Vector2(texture.Width, (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2;
            drawPos += origin * npc.scale;

            spriteBatch.Draw(texture, drawPos, npc.frame, npc.GetAlpha(drawColor), npc.rotation, origin, npc.scale, effects, 0);

            return false;
        }

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			position.X -= 45;
			return (true);
		}

		private void SetStats()
		{
			npc.scale = newScale;
			npc.defense = (int)(npc.defense * npc.scale);
			npc.damage = (int)(npc.damage * npc.scale);
			npc.life = (int)(npc.life * npc.scale);
			npc.lifeMax = npc.life;
			npc.value = ((int)(npc.value * npc.scale));
			npc.npcSlots *= npc.scale;
			npc.knockBackResist *= 2f - npc.scale;

			npc.frame.Width = 30;
			npc.frame.X = newXFrame;
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
