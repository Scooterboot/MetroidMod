using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Kraid
{
    public class Kraid_ArmFront : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
			Main.npcFrameCount[npc.type] = 5;
		}

		Vector2 swipeVec = Vector2.Zero;
		Vector2[] swipeDestVec = new Vector2[9];
		float swipeFrame = 0f;

		public override void SetDefaults()
		{
			npc.width = 2;
			npc.height = 2;
			npc.scale = 1f;
			npc.damage = 0;
			npc.defense = 500;
			npc.lifeMax = 1000;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 4, 60);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 1;

			swipeDestVec[0] = new Vector2(-20f,-2f);
			swipeDestVec[1] = new Vector2(-50f,-6f);
			swipeDestVec[2] = new Vector2(-78f,-42f);
			swipeDestVec[3] = new Vector2(-70f,-116f);
			swipeDestVec[4] = new Vector2(0f,-100f);
			swipeDestVec[5] = new Vector2(36f,-54f);
			swipeDestVec[6] = new Vector2(42f,0f);
			swipeDestVec[7] = new Vector2(6f,22f);
			swipeDestVec[8] = Vector2.Zero;
		}

		int fArmAnim = 1;
		int num = 1;
		float anim = 0;
		Vector2 animVec = new Vector2(14f,-8f);
		
		int num2 = 220;

		int state = 0;
		public override void AI()
		{
			NPC Head = Main.npc[(int)npc.ai[0]];
			if (!Head.active)
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
				return;
			}

			state = 0;
			if(Head.life < (int)(Head.lifeMax*0.75f))
			{
				state = 1;
			}
			if(Head.life < (int)(Head.lifeMax*0.5f))
			{
				state = 2;
			}
			if(Head.life < (int)(Head.lifeMax*0.25f))
			{
				state = 3;
			}

			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			if (!player.dead)
			{
				npc.timeLeft = 60;
			}

			npc.ai[1]++;
			if(npc.ai[1] >= num2)
			{
				if(npc.ai[1] <= num2+40)
				{
					swipeFrame = Math.Min(swipeFrame+0.125f,4f);
				}
				else if(npc.ai[1] <= num2+75)
				{
					swipeFrame = Math.Min(swipeFrame+0.25f,9f);
				}
				swipeVec = LerpArray(Vector2.Zero,swipeDestVec,swipeFrame);
				if(npc.ai[1] >= num2+80)
				{
					swipeFrame = 0f;
					swipeVec = Vector2.Zero;
					npc.ai[1] = 0;
				}
			}
			else
			{
				num2 = 220;
				num2 -= 30*state;
			}
			npc.rotation = -(((float)Math.PI/4) * (Math.Max((swipeVec.X+swipeVec.Y)*-1,0)/194));
			
			if(swipeFrame == 6f)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/KraidSwipeSound"));
				Vector2 clawPos = npc.Center+new Vector2(48*Head.direction,-36);
				float trot = (float)Math.Atan2(player.Center.Y-clawPos.Y,player.Center.X-clawPos.X);
				float speed = 4f;
				Vector2 clawVel = new Vector2((float)Math.Cos(trot)*speed,(float)Math.Sin(trot)*speed);

				for(int i = 0; i < 2; i++)
				{
					int spread = 15;
					float spreadMult = 0.05f;
					float vX = clawVel.X+(float)Main.rand.Next(-spread,spread+1) * spreadMult;
					float vY = clawVel.Y+(float)Main.rand.Next(-spread,spread+1) * spreadMult;
				 
					int c = NPC.NewNPC((int)clawPos.X,(int)clawPos.Y,mod.NPCType("KraidClaw"),npc.whoAmI);
					Main.npc[c].position.Y += (float)Main.npc[c].height/2;
					Main.npc[c].velocity = new Vector2(vX,vY);
					Main.npc[c].direction = Head.direction;
				}
			}
			
			npc.frameCounter += 1;
			if(npc.frameCounter >= 10)
			{
				npc.frame.Y += fArmAnim;
				if(npc.frame.Y >= 4 || npc.frame.Y <= 0)
				{
					fArmAnim *= -1;
				}
				npc.frameCounter = 0;
			}

			if(npc.frame.Y >= 4)
			{
				num = -1;
			}
			if(npc.frame.Y <= 0)
			{
				num = 1;
			}
			anim = (1f/4)*npc.frame.Y+((float)npc.frameCounter/40)*num;
			anim = MathHelper.Clamp(anim,0f,1f);
			
			Vector2 vec = Vector2.Lerp(Vector2.Zero,new Vector2(animVec.X*Head.direction,animVec.Y),anim);
			npc.Center = Head.Center + new Vector2(42*Head.direction,131) + vec + new Vector2(swipeVec.X*Head.direction,swipeVec.Y);
			npc.velocity *= 0f;
		}
		/*public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					for (int m = 0; m < 20; m++)
					{
						int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, npc.life <= 0 && m % 2 == 0 ? 3f : 1f);
						if (m % 2 == 0)
						{
							Main.dust[dustID].noGravity = true;
						}
					}
					int gore = Gore.NewGore(npc.position,npc.velocity,mod.GetGoreSlot("Gores/KraidGore4"),1f);
					Main.gore[gore].timeLeft = 60;
					gore = Gore.NewGore(npc.position,npc.velocity,mod.GetGoreSlot("Gores/KraidGore5"),1f);
					Main.gore[gore].timeLeft = 60;
					int gore = Gore.NewGore(npc.position,npc.velocity,mod.GetGoreSlot("Gores/KraidGore6"),1f);
					Main.gore[gore].timeLeft = 30;
					Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,1);
					
					for (int num70 = 0; num70 < 15; num70++)
					{
						int num71 = Dust.NewDust(new Vector2(npc.position.X-10,npc.position.Y-10), npc.width+10, npc.height+10, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(new Vector2(npc.position.X-10,npc.position.Y-10), npc.width+10, npc.height+10, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)npc.position.X,(int)npc.position.Y,14);
				}
			}
		}*/
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
		}
		
		public static Vector2 LerpArray(Vector2 value1, Vector2[] value2, float amount)
		{
			Vector2 result = value1;
			for(int i = 0; i < value2.Length; i++)
			{
				if((i+1) >= amount)
				{
					Vector2 firstValue = value1;
					Vector2 secondValue = value2[i];
					if(i > 0)
					{
						firstValue = value2[i-1];
					}
					float amt = amount-i;
					result = firstValue + (secondValue-firstValue)*amt;
					break;
				}
			}
			return result;
		}
	}
}