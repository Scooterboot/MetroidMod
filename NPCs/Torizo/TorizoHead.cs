using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Torizo
{
    public class TorizoHead : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[npc.type] = 3;
		}
		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 28;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 500;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath3;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 0, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.buffImmune[mod.BuffType("IceFreeze")] = true;
			npc.buffImmune[mod.BuffType("InstantFreeze")] = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TorizoHeadGore"), npc.scale);
			}
		}
		public override void AI()
		{
			if(npc.life <= (int)(npc.lifeMax * 0.8f) && npc.life >= (int)(npc.lifeMax * 0.6f))
				{
					npc.color = new Color(255, 220, 220);
				}
			if(npc.life < (int)(npc.lifeMax * 0.6f) && npc.life >= (int)(npc.lifeMax * 0.4f))
				{
					npc.color = new Color(255, 190, 190);
				}
			if(npc.life < (int)(npc.lifeMax * 0.6f) && npc.life >= (int)(npc.lifeMax * 0.4f))
				{
					npc.color = new Color(255, 150, 150);
				}
			if(npc.life < (int)(npc.lifeMax * 0.4f) && npc.life >= (int)(npc.lifeMax * 0.2f))
				{
					npc.color = new Color(255, 110, 110);
				}
				if(npc.life < (int)(npc.lifeMax * 0.2f))
				{
					npc.color = new Color(255, 70, 70);
				}

			npc.TargetClosest(true);
			Player P = Main.player[npc.target];
			int	bombCool = Main.expertMode ? 2000 : 3000;

			NPC N = Main.npc[(int)npc.ai[0]];
			if (!N.active || N.type != mod.NPCType("Torizo"))
			{
				npc.life = -1;
				npc.active = false;
			}
			float HX = N.position.X + (N.width - npc.width) + 8;
			float HY = N.position.Y - npc.height * 0.5f;
			npc.position.X = HX;
			npc.position.Y = HY;
			if (npc.ai[1] % 30 <= 5)
			{
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
			}
			npc.ai[1] += 1 + Main.rand.Next(5);
			if (npc.ai[1] >= bombCool)
			{
				npc.ai[2]++;
			}
			if (npc.ai[2] > 0 && npc.ai[2] % 20 == 0)
			{
				float targetX = P.Center.X;
                float targetY = P.Center.Y;
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 5);
				 NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-40, 40), (int)npc.Center.Y - 10, mod.NPCType("TorizoBomb"), 0, npc.whoAmI, targetX + Main.rand.Next(-150, 150), targetY+ Main.rand.Next(-150, 150));
				 NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 20, mod.NPCType("TorizoBomb"), 0, npc.whoAmI, targetX, targetY);
				 NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-40, 40), (int)npc.Center.Y + 10, mod.NPCType("TorizoBomb"), 0, npc.whoAmI, targetX+ Main.rand.Next(-150, 150), targetY + Main.rand.Next(-150, 150));
			}
			if (npc.ai[2] > 60)
			{
				npc.ai[1] = 0;
				npc.ai[2] = 0;
			}
/*			Vector2 dir = P.position - npc.position;
			float rot = (float)(Math.Atan2(dir.Y, dir.X) / (2 * Math.PI));
			if (rot < 30 && rot > -30)
			{
				npc.rotation = rot * npc.direction;
			}
			else
			{
				npc.rotation = 0;
			}*/

		}
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			if(npc.ai[2] > 0 && npc.ai[2] <= 15)
			{
				npc.frame.Y = 28;	
			}
			else if (npc.ai[2] > 15)
			{
				npc.frame.Y = 56;
			}
			else
			{
				npc.frame.Y = 0;	
			}
		}
	}
}