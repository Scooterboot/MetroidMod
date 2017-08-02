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
    public class Kraid_ArmBack : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
			Main.npcFrameCount[npc.type] = 6;
		}

		Vector2 swipeVec = Vector2.Zero;
		Vector2[] swipeDestVec = new Vector2[9];
		float swipeFrame = 0f;

		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 60;
			npc.scale = 1f;
			npc.damage = 50;
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

			swipeDestVec[0] = new Vector2(-12f,-10f);
			swipeDestVec[1] = new Vector2(-22f,-36f);
			swipeDestVec[2] = new Vector2(-40f,-52f);
			swipeDestVec[3] = new Vector2(-48f,-98f);
			swipeDestVec[4] = new Vector2(-60f,-102f);
			swipeDestVec[5] = new Vector2(12f,-132f);
			swipeDestVec[6] = new Vector2(26f,-46f);
			swipeDestVec[7] = new Vector2(-28f,22f);
			swipeDestVec[8] = Vector2.Zero;
		}

		int bArmAnim = 1;
		int num = 1;
		float anim = 0;
		Vector2 animVec = new Vector2(-14f,8f);

		int state = 0;
		int num2 = 100;
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

			Player player = Main.player[Head.target];
			if (!player.dead)
			{
				npc.timeLeft = 60;
			}

			if(npc.ai[1] > 0)
			{
				npc.ai[1]++;
			}
			if(npc.ai[1] >= num2)
			{
				if(npc.ai[1] <= num2+46)
				{
					swipeFrame = Math.Min(swipeFrame+0.17f,5f);
					npc.frame.Y = Math.Max(npc.frame.Y-1,0);
				}
				else
				{
					if(npc.ai[1] <= num2+54)
					{
						swipeFrame = Math.Min(swipeFrame+0.25f,7f);
					}
					else if(npc.ai[1] <= num2+70)
					{
						swipeFrame = Math.Min(swipeFrame+0.125f,9f);
					}
					npc.frame.Y = Math.Min(npc.frame.Y+1,5);
				}
				if(swipeFrame == 6f)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/KraidSwipeSound"));

					Vector2 clawPos = npc.Center;
					float trot = (float)Math.Atan2(player.Center.Y-clawPos.Y,player.Center.X-clawPos.X);
					float speed = 8f;
					Vector2 clawVel = new Vector2((float)Math.Cos(trot)*speed,(float)Math.Sin(trot)*speed);

					int slash = Projectile.NewProjectile(clawPos.X,clawPos.Y,clawVel.X,clawVel.Y,mod.ProjectileType("KraidSlash"),40,8f);
					Main.projectile[slash].ai[0] = Head.whoAmI;
					Main.projectile[slash].spriteDirection = Head.direction;
				}
				swipeVec = Kraid_ArmFront.LerpArray(Vector2.Zero,swipeDestVec,swipeFrame);
				if(npc.ai[1] >= num2+75)
				{
					swipeFrame = 0f;
					swipeVec = Vector2.Zero;
					npc.ai[1] = 0;
					npc.frame.Y = 4;
					bArmAnim = -1;
				}
				npc.frameCounter = Math.Max(npc.frameCounter-1,0);
			}
			else
			{
				num2 = 100;
				num2 -= 20*state;
				npc.frameCounter += 1;
				if(npc.frameCounter >= 10)
				{
					npc.frame.Y += bArmAnim;
					if(npc.frame.Y >= 4)
					{
						npc.frame.Y = 4;
						bArmAnim = -1;
					}
					if(npc.frame.Y <= 0)
					{
						npc.frame.Y = 0;
						bArmAnim = 1;
					}
					npc.frameCounter = 0;
				}
			}
			npc.rotation = -(((float)Math.PI/4) * (Math.Max((swipeVec.X+swipeVec.Y)*-1,0)/192));

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
			npc.Center = Head.Center + new Vector2(234*Head.direction,79) + vec + new Vector2(swipeVec.X*Head.direction,swipeVec.Y);
			npc.velocity *= 0f;
			
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
				{
					Projectile P = Main.projectile[i];
					Rectangle projRect = new Rectangle((int)(P.position.X+P.velocity.X),(int)(P.position.Y+P.velocity.Y),P.width,P.height);
					Rectangle npcRect = new Rectangle((int)npc.position.X,(int)npc.position.Y,npc.width,npc.height);
					if(projRect.Intersects(npcRect))
					{
						if (Main.projectile[i].penetrate > 0)
						{
							Main.projectile[i].penetrate--;
							if (Main.projectile[i].penetrate == 0)
							{
								break;
							}
						}
					}
				}
			}
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
					int gore = Gore.NewGore(npc.position,npc.velocity,mod.GetGoreSlot("Gores/KraidGore7"),1f);
					Main.gore[gore].timeLeft = 30;
					Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,1);
					
					for (int num70 = 0; num70 < 20; num70++)
					{
						int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
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
	}
}
