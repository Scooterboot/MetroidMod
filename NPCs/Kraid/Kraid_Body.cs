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
    public class Kraid_Body : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
		}
		public override void SetDefaults()
		{
			npc.width = 302;
			npc.height = 348;
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
			npc.boss = true;
		}
		int state = 0;
		public override void AI()
		{
			NPC Head = Main.npc[(int)npc.ai[0]];
			bool despawn = (Head.ai[3] > 1);
			if (!Head.active)
			{
				npc.life = 0;
				npc.active = false;
				if(!despawn)
				{
					npc.HitEffect(0, 10.0);
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

			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			if (!player.dead)
			{
				npc.timeLeft = 60;
			}

			npc.Center = Head.Center + new Vector2(29*Head.direction,223);
			npc.velocity *= 0f;
			
			if(++npc.ai[1] >= 180 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int spike = Projectile.NewProjectile(npc.Center.X+(126*Head.direction),npc.Center.Y+59,4f*Head.direction,0f,mod.ProjectileType("KraidBellySpike"),npc.damage/2,4f);
				Main.projectile[spike].ai[0] = Head.whoAmI;
				Main.projectile[spike].ai[1] = Head.target;
				Main.projectile[spike].spriteDirection = Head.direction;
				Main.projectile[spike].frame = state;
				Main.projectile[spike].netUpdate = true;
				npc.ai[1] = 0;
			}
			if(++npc.ai[2] >= 300 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int spike = Projectile.NewProjectile(npc.Center.X+(126*Head.direction),npc.Center.Y-19,4f*Head.direction,0f,mod.ProjectileType("KraidBellySpike"),npc.damage/2,4f);
				Main.projectile[spike].ai[0] = Head.whoAmI;
				Main.projectile[spike].ai[1] = Head.target;
				Main.projectile[spike].spriteDirection = Head.direction;
				Main.projectile[spike].frame = state;

				npc.netUpdate = true;
				npc.ai[2] = Main.rand.Next(state*40,121);
			}
			if(++npc.ai[3] >= 420 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int spike = Projectile.NewProjectile(npc.Center.X+(102*Head.direction),npc.Center.Y-87,4f*Head.direction,0f,mod.ProjectileType("KraidBellySpike"),npc.damage/2,4f);
				Main.projectile[spike].ai[0] = Head.whoAmI;
				Main.projectile[spike].ai[1] = Head.target;
				Main.projectile[spike].spriteDirection = Head.direction;
				Main.projectile[spike].frame = state;

				npc.netUpdate = true;
				npc.ai[3] = Main.rand.Next(state*80,241);
			}

			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
				{
					Projectile P = Main.projectile[i];
					Rectangle projRect = new Rectangle((int)(P.position.X+P.velocity.X),(int)(P.position.Y+P.velocity.Y),P.width,P.height);
					Rectangle npcRect1 = new Rectangle((int)npc.position.X,(int)npc.position.Y+40,npc.width,npc.height-40);
					Rectangle npcRect2 = new Rectangle((int)npc.position.X,(int)npc.position.Y,npc.width-60,40);
					if(Head.direction == -1)
					{
						npcRect2.X = (int)npc.position.X+60;
					}
					if(projRect.Intersects(npcRect1) || projRect.Intersects(npcRect2))
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
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					for (int m = 0; m < 50; m++)
					{
						int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, npc.life <= 0 && m % 2 == 0 ? 3f : 1f);
						if (m % 2 == 0)
						{
							Main.dust[dustID].noGravity = true;
						}
					}
					Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,1);
					/*for (int num70 = 0; num70 < 65; num70++)
					{
						int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)npc.position.X,(int)npc.position.Y,14);*/
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
		}
	}
}