using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{
    public class Covern : ModNPC
    {
        /*
         * npc.ai[0] = state manager.
         * npc.ai[1] = glowmask opacity.
         * npc.ai[2] = bounce movement.
         * npc.ai[3] = state timer.
         */
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 30; npc.height = 32;

            /* Temporary NPC values */
            npc.scale = 2;
            npc.damage = 15;
            npc.defense = 5;
            npc.lifeMax = 150;
            npc.aiStyle = -1;
            npc.knockBackResist = 0;

            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override bool PreAI()
        {
            if(npc.ai[0] == 0)
            {
                npc.ai[0] = 1;
                npc.ai[1] = 1;
            }

            if(npc.ai[0] == 1)
            {
                if (npc.ai[1] > 0)
                    npc.ai[1] -= .06F;
                else
                    npc.ai[1] = 0;
                
                if(npc.ai[3]++ >= 180)
                {
                    npc.ai[0] = 2;
                    npc.ai[3] = 0;
                }
            }
            else if(npc.ai[0] == 2)
            {
                if (npc.ai[1] < 1)
                    npc.ai[1] += .06F;
                else
                {
                    npc.ai[1] = 1;

                    if(npc.ai[3]++ >= 60)
                    {
                        npc.TargetClosest(false);
                        Vector2 newPos = Main.player[npc.target].Center + new Vector2(Main.rand.Next(-160, 161), Main.rand.Next(-120, 40));

                        npc.position = newPos;
                        npc.ai[0] = 1;
                        npc.ai[3] = 0;
                    }
                }
            }

            npc.ai[2] += .2F;
            npc.velocity.Y = (float)Math.Sin(npc.ai[2]);
            npc.dontTakeDamage = npc.ai[1] >= 1 ? true : false;

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 6)
            {
                npc.frame.Y = (npc.frame.Y + frameHeight) % ((Main.npcFrameCount[npc.type]-1) * frameHeight);
                npc.frameCounter = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.ai[1] >= 1)
                return false;

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("MetroidMod/NPCs/Mobs/Covern_Glowmask");
            Vector2 drawPos = npc.position - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2, ((texture.Height / (Main.npcFrameCount[npc.type]-1)) / 2));
            drawPos += origin * npc.scale + new Vector2(0, 2 * npc.scale);

            if (npc.ai[1] < 1)
                spriteBatch.Draw(texture, drawPos, npc.frame, npc.GetAlpha(Color.White) * (float)npc.ai[1], npc.rotation, origin, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
