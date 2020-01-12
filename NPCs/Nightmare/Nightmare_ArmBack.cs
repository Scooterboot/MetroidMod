using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Nightmare
{
    public class Nightmare_ArmBack : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
		}
		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 48;
			npc.scale = 1f;
			npc.damage = 0;//50;
			npc.defense = 30;
			npc.lifeMax = 30000;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
		}

		int _laserBeam = 0;
		Projectile LaserBeam
		{
			get { return Main.projectile[_laserBeam]; }
		}

		public override void AI()
		{
			NPC Head = Main.npc[(int)npc.ai[0]];
			bool flag = (Head.alpha < 255);
			if (!Head.active)
			{
				Main.PlaySound(npc.DeathSound,npc.Center);
				npc.life = 0;
				if(flag)
				{
					npc.HitEffect(0, 10.0);
				}
				npc.active = false;
				return;
			}

			npc.damage = Head.damage;
			npc.velocity = Head.velocity;

			if (npc.ai[1] >= 1 && npc.ai[1] <= 3)
			{
				Vector2 laserPos = npc.Center + new Vector2(17 * Head.direction, 15);
				if (npc.ai[1] == 1)
				{
					laserPos = npc.Center + new Vector2(17 * Head.direction, 16);
				}
				if (npc.ai[1] == 2)
				{
					laserPos = npc.Center + new Vector2(17 * Head.direction, 9);
				}

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn laser projectile
					if (npc.ai[2] == 1)
					{
						Projectile.NewProjectile(laserPos.X, laserPos.Y, 0f, 0f, mod.ProjectileType("NightmareLaser"), 25, 1f, Main.myPlayer, Head.whoAmI, npc.whoAmI);
						npc.ai[2] = 0;
					}
					// Charge laser beams
					if (npc.ai[2] == 2)
					{
						_laserBeam = Projectile.NewProjectile(laserPos.X, laserPos.Y, 0f, 0f, mod.ProjectileType("NightmareLaserBeam"), 50, 1f, Main.myPlayer, Head.whoAmI, npc.whoAmI);
						npc.ai[2] = 0;
					}
					// Fire laser beams
					if (npc.ai[2] == 3)
					{
						if (LaserBeam != null && LaserBeam.active)
						{
							LaserBeam.localAI[0] = 1;
							LaserBeam.netUpdate2 = true;
						}
						npc.ai[2] = 0;
					}

					// Spawn gravity orb
					if (npc.ai[3] == 1)
					{
						NPC gOrb = Main.npc[NPC.NewNPC((int)laserPos.X, (int)laserPos.Y, mod.NPCType("GravityOrb"), npc.whoAmI, Head.whoAmI)];
						gOrb.position.Y += (float)gOrb.height / 2;
						gOrb.direction = Head.direction;
						gOrb.target = Head.target;
						gOrb.netUpdate = true;

						npc.ai[3] = 0;
					}
				}
			}

			if(!Head.dontTakeDamage)
			{
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
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					for (int num70 = 0; num70 < 10; num70++)
					{
						int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					//Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,14);
					
					int num = (int)npc.ai[1];
					string path = "Gores/NightmareArmBackGore"+num;
					int gore = Gore.NewGore(npc.position, new Vector2(0f,3f), mod.GetGoreSlot(path), 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
		}
	}
}