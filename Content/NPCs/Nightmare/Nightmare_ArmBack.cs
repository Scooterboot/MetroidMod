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

namespace MetroidModPorted.Content.NPCs.Nightmare
{
    public class Nightmare_ArmBack : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 48;
			NPC.scale = 1f;
			NPC.damage = 0;//50;
			NPC.defense = 30;
			NPC.lifeMax = 30000;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
		}

		int _laserBeam = 0;
		Projectile LaserBeam
		{
			get { return Main.projectile[_laserBeam]; }
		}

		public override void AI()
		{
			NPC Head = Main.npc[(int)NPC.ai[0]];
			bool flag = (Head.alpha < 255);
			if (!Head.active)
			{
				SoundEngine.PlaySound(NPC.DeathSound,NPC.Center);
				NPC.life = 0;
				if(flag)
				{
					NPC.HitEffect(0, 10.0);
				}
				NPC.active = false;
				return;
			}

			NPC.damage = Head.damage;
			NPC.velocity = Head.velocity;

			Vector2 laserPos = NPC.Center + new Vector2(17 * Head.direction, 15);
			if (NPC.ai[1] == 1)
			{
				laserPos = NPC.Center + new Vector2(17 * Head.direction, 16);
			}
			if (NPC.ai[1] == 2)
			{
				laserPos = NPC.Center + new Vector2(17 * Head.direction, 9);
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Spawn laser projectile
				if (NPC.ai[2] == 1)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), laserPos.X, laserPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.NightmareLaser>(), 25, 1f, Main.myPlayer, Head.whoAmI, NPC.whoAmI);
					NPC.ai[2] = 0;
				}
				// Charge laser beams
				if (NPC.ai[2] == 2)
				{
					_laserBeam = Projectile.NewProjectile(NPC.GetSource_FromAI(), laserPos.X, laserPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.NightmareLaserBeam>(), 50, 1f, Main.myPlayer, Head.whoAmI, NPC.whoAmI);
					NPC.ai[2] = 0;
				}
				// Fire laser beams
				if (NPC.ai[2] == 3)
				{
					if (LaserBeam != null && LaserBeam.active)
					{
						LaserBeam.localAI[0] = 1;
						LaserBeam.netUpdate2 = true;
					}
					NPC.ai[2] = 0;
				}

				// Spawn gravity orb
				if (NPC.ai[3] == 1)
				{
					NPC gOrb = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)laserPos.X, (int)laserPos.Y, ModContent.NPCType<GravityOrb>(), NPC.whoAmI, Head.whoAmI)];
					gOrb.position.Y += (float)gOrb.height / 2;
					gOrb.direction = Head.direction;
					gOrb.target = Head.target;
					gOrb.netUpdate = true;

					NPC.ai[3] = 0;
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
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (NPC.life <= 0)
				{
					for (int num70 = 0; num70 < 10; num70++)
					{
						int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					//Main.PlaySound(4,(int)NPC.position.X,(int)NPC.position.Y,14);
					
					int num = (int)NPC.ai[1];
					string path = "NightmareArmBackGore"+num;
					int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(0f,3f), Mod.Find<ModGore>(path).Type, 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
	}
}
