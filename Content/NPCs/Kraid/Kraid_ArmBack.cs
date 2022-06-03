using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidModPorted.Content.NPCs.Kraid
{
    public class Kraid_ArmBack : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
			Main.npcFrameCount[NPC.type] = 6;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		Vector2 swipeVec = Vector2.Zero;
		Vector2[] swipeDestVec = new Vector2[9];
		float swipeFrame = 0f;

		public override void SetDefaults()
		{
			NPC.width = 60;
			NPC.height = 60;
			NPC.scale = 1f;
			NPC.damage = 50;
			NPC.defense = 500;
			NPC.lifeMax = 1000;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 4, 60);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			NPC.boss = true;

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
			NPC Head = Main.npc[(int)NPC.ai[0]];
			bool despawn = (Head.ai[3] > 1);
			if (!Head.active)
			{
				NPC.life = 0;
				NPC.active = false;
				if(!despawn)
				{
					NPC.HitEffect(0, 10.0);
				}
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
				NPC.timeLeft = 60;
			}

			if(NPC.ai[1] > 0)
			{
				NPC.ai[1]++;
			}
			if(NPC.ai[1] >= num2)
			{
				if(NPC.ai[1] <= num2+46)
				{
					swipeFrame = Math.Min(swipeFrame+0.17f,5f);
					NPC.frame.Y = Math.Max(NPC.frame.Y-1,0);
				}
				else
				{
					if(NPC.ai[1] <= num2+54)
					{
						swipeFrame = Math.Min(swipeFrame+0.25f,7f);
					}
					else if(NPC.ai[1] <= num2+70)
					{
						swipeFrame = Math.Min(swipeFrame+0.125f,9f);
					}
					NPC.frame.Y = Math.Min(NPC.frame.Y+1,5);
				}
				if(swipeFrame == 6f)
				{
					SoundEngine.PlaySound(Sounds.NPCs.KraidSwipe, NPC.Center);

					Vector2 clawPos = NPC.Center;
					float trot = (float)Math.Atan2(player.Center.Y-clawPos.Y,player.Center.X-clawPos.X);
					float speed = 8f;
					Vector2 clawVel = new Vector2((float)Math.Cos(trot)*speed,(float)Math.Sin(trot)*speed);

					int slash = Projectile.NewProjectile(NPC.GetSource_FromAI(),clawPos.X,clawPos.Y,clawVel.X,clawVel.Y,ModContent.ProjectileType<Projectiles.Boss.KraidSlash>(),30,8f);
					Main.projectile[slash].ai[0] = Head.whoAmI;
					Main.projectile[slash].spriteDirection = Head.direction;
				}
				swipeVec = Kraid_ArmFront.LerpArray(Vector2.Zero,swipeDestVec,swipeFrame);
				if(NPC.ai[1] >= num2+75)
				{
					swipeFrame = 0f;
					swipeVec = Vector2.Zero;
					NPC.ai[1] = 0;
					NPC.frame.Y = 4;
					bArmAnim = -1;
				}
				NPC.frameCounter = Math.Max(NPC.frameCounter-1,0);
			}
			else
			{
				num2 = 100;
				num2 -= 20*state;
				NPC.frameCounter += 1;
				if(NPC.frameCounter >= 10)
				{
					NPC.frame.Y += bArmAnim;
					if(NPC.frame.Y >= 4)
					{
						NPC.frame.Y = 4;
						bArmAnim = -1;
					}
					if(NPC.frame.Y <= 0)
					{
						NPC.frame.Y = 0;
						bArmAnim = 1;
					}
					NPC.frameCounter = 0;
				}
			}
			NPC.rotation = -(((float)Math.PI/4) * (Math.Max((swipeVec.X+swipeVec.Y)*-1,0)/192));

			if(NPC.frame.Y >= 4)
			{
				num = -1;
			}
			if(NPC.frame.Y <= 0)
			{
				num = 1;
			}
			anim = (1f/4)*NPC.frame.Y+((float)NPC.frameCounter/40)*num;
			anim = MathHelper.Clamp(anim,0f,1f);
			
			Vector2 vec = Vector2.Lerp(Vector2.Zero,new Vector2(animVec.X*Head.direction,animVec.Y),anim);
			NPC.Center = Head.Center + new Vector2(234*Head.direction,79) + vec + new Vector2(swipeVec.X*Head.direction,swipeVec.Y);
			NPC.velocity *= 0f;
			
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
				{
					Projectile P = Main.projectile[i];
					Rectangle projRect = new Rectangle((int)(P.position.X+P.velocity.X),(int)(P.position.Y+P.velocity.Y),P.width,P.height);
					Rectangle npcRect = new Rectangle((int)NPC.position.X,(int)NPC.position.Y,NPC.width,NPC.height);
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
				if (NPC.life <= 0)
				{
					for (int m = 0; m < 20; m++)
					{
						int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
						if (m % 2 == 0)
						{
							Main.dust[dustID].noGravity = true;
						}
					}
					int gore = Gore.NewGore(NPC.position,NPC.velocity,mod.GetGoreSlot("Gores/KraidGore4"),1f);
					Main.gore[gore].timeLeft = 60;
					gore = Gore.NewGore(NPC.position,NPC.velocity,mod.GetGoreSlot("Gores/KraidGore5"),1f);
					Main.gore[gore].timeLeft = 60;
					int gore = Gore.NewGore(NPC.position,NPC.velocity,mod.GetGoreSlot("Gores/KraidGore7"),1f);
					Main.gore[gore].timeLeft = 30;
					Main.PlaySound(4,(int)NPC.position.X,(int)NPC.position.Y,1);
					
					for (int num70 = 0; num70 < 20; num70++)
					{
						int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)NPC.position.X,(int)NPC.position.Y,14);
				}
			}
		}*/
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
	}
}
