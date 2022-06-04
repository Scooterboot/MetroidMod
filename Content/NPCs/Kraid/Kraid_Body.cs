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
	public class Kraid_Body : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 302;
			NPC.height = 348;
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
		}
		int state = 0;
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

			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (!player.dead)
			{
				NPC.timeLeft = 60;
			}

			NPC.Center = Head.Center + new Vector2(29*Head.direction,223);
			NPC.velocity *= 0f;
			
			if(++NPC.ai[1] >= 180 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int spike = Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center.X+(126*Head.direction),NPC.Center.Y+59,4f*Head.direction,0f,ModContent.ProjectileType<Projectiles.Boss.KraidBellySpike>(),NPC.damage/2,4f);
				Main.projectile[spike].ai[0] = Head.whoAmI;
				Main.projectile[spike].ai[1] = Head.target;
				Main.projectile[spike].spriteDirection = Head.direction;
				Main.projectile[spike].frame = state;
				Main.projectile[spike].netUpdate = true;
				NPC.ai[1] = 0;
			}
			if(++NPC.ai[2] >= 300 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int spike = Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center.X+(126*Head.direction),NPC.Center.Y-19,4f*Head.direction,0f,ModContent.ProjectileType<Projectiles.Boss.KraidBellySpike>(),NPC.damage/2,4f);
				Main.projectile[spike].ai[0] = Head.whoAmI;
				Main.projectile[spike].ai[1] = Head.target;
				Main.projectile[spike].spriteDirection = Head.direction;
				Main.projectile[spike].frame = state;

				NPC.netUpdate = true;
				NPC.ai[2] = Main.rand.Next(state*40,121);
			}
			if(++NPC.ai[3] >= 420 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				int spike = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X+(102*Head.direction),NPC.Center.Y-87,4f*Head.direction,0f,ModContent.ProjectileType<Projectiles.Boss.KraidBellySpike>(),NPC.damage/2,4f);
				Main.projectile[spike].ai[0] = Head.whoAmI;
				Main.projectile[spike].ai[1] = Head.target;
				Main.projectile[spike].spriteDirection = Head.direction;
				Main.projectile[spike].frame = state;

				NPC.netUpdate = true;
				NPC.ai[3] = Main.rand.Next(state*80,241);
			}

			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
				{
					Projectile P = Main.projectile[i];
					Rectangle projRect = new Rectangle((int)(P.position.X+P.velocity.X),(int)(P.position.Y+P.velocity.Y),P.width,P.height);
					Rectangle npcRect1 = new Rectangle((int)NPC.position.X,(int)NPC.position.Y+40,NPC.width,NPC.height-40);
					Rectangle npcRect2 = new Rectangle((int)NPC.position.X,(int)NPC.position.Y,NPC.width-60,40);
					if(Head.direction == -1)
					{
						npcRect2.X = (int)NPC.position.X+60;
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
				if (NPC.life <= 0)
				{
					for (int m = 0; m < 50; m++)
					{
						int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
						if (m % 2 == 0)
						{
							Main.dust[dustID].noGravity = true;
						}
					}
					SoundEngine.PlaySound(SoundID.NPCDeath1,NPC.position);
					/*for (int num70 = 0; num70 < 65; num70++)
					{
						int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)NPC.position.X,(int)NPC.position.Y,14);*/
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
	}
}
