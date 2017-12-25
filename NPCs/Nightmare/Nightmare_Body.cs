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
    public class Nightmare_Body : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
		}
		public override void SetDefaults()
		{
			npc.width = 212;
			npc.height = 122;
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
			npc.Center = Head.Center + new Vector2(-44*Head.direction,-5);
			npc.velocity *= 0f;

			if(!Head.dontTakeDamage)
			{
				for(int i = 0; i < Main.maxProjectiles; i++)
				{
					if(Main.projectile[i].active && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
					{
						Projectile P = Main.projectile[i];
						Rectangle projRect = new Rectangle((int)(P.position.X+P.velocity.X),(int)(P.position.Y+P.velocity.Y),P.width,P.height);
						
						Rectangle npcRect1 = new Rectangle((int)npc.Center.X-102,(int)npc.Center.Y-59,106,104);
						Rectangle npcRect2 = new Rectangle((int)npc.Center.X-102,(int)npc.Center.Y+45,202,16);
						if(Head.direction == -1)
						{
							npcRect1.X = (int)npc.Center.X-4;
							npcRect2.X = (int)npc.Center.X-100;
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
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (npc.life <= 0)
				{
					for (int num70 = 0; num70 < 70; num70++)
					{
						int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					//Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,14);
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
		}
	}
}