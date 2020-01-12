using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MetroidMod.NPCs.Nightmare
{
    public class Nightmare_Tail : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare");
		}
		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 64;
			npc.scale = 1f;
			npc.damage = 0;//50;
			npc.defense = 50;
			npc.lifeMax = 10000;
			//npc.dontTakeDamage = true;
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

		int _gravityField = 0;
		Projectile GravityField
		{
			get { return Main.projectile[_gravityField]; }
		}

		int oldLife = 0;
		bool initialized = false;

		public override bool PreAI()
		{
			if (!initialized)
			{
				oldLife = npc.life;
				initialized = true;
			}
			return (true);
		}

		public override void AI()
		{
			NPC Head = Main.npc[(int)npc.ai[0]];
			bool flag = (Head.alpha < 255);
			if (!Head.active)
			{
				Main.PlaySound(npc.DeathSound, npc.Center);
				npc.life = 0;
				if (flag)
				{
					npc.HitEffect(0, 10.0);
				}
				npc.active = false;
				return;
			}

			npc.velocity *= 0f;
			npc.damage = Head.damage;
			npc.dontTakeDamage = Head.dontTakeDamage;
			npc.Center = Head.Center + new Vector2(-76 * Head.direction, 88);

			if (npc.ai[1] == 1)
			{
				if (npc.ai[2] == 0)
				{
					// Spawn gravity field projectile
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 spawnPos = new Vector2(npc.Center.X + 26 * Head.direction, npc.Center.Y + 14);
						_gravityField = Projectile.NewProjectile(spawnPos, Vector2.Zero, mod.ProjectileType("NightmareGravityField"), 0, 0f, Main.myPlayer, Head.whoAmI, npc.whoAmI);
					}

					npc.ai[2] = 1;
				}
				npc.ai[1] = 0;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				// If the Tail has an active GravityField projectile, we want to keep track of lost health to update its state.
				if (npc.ai[2] == 1)
				{
					if (npc.justHit)
					{
						npc.ai[3] += oldLife - npc.life;
						oldLife = npc.life;
					}
				}

				if (GravityField == null || !GravityField.active)
				{
					npc.ai[2] = 0;
					npc.ai[3] = 0;
					npc.netUpdate = true;
				}
				else if (npc.ai[2] == 1 && npc.ai[3] > 2000)
				{
					GravityField.localAI[0] = 1f;
					GravityField.netUpdate2 = true;
				}
			}
		}
		
		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if(npc.ai[2] == 0)
			{
				damage = (int)(damage * 0.1f);
			}
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(npc.ai[2] == 0)
			{
				damage = (int)(damage * 0.1f);
			}
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int num70 = 0; num70 < 15; num70++)
				{
					int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
					Main.dust[num71].velocity *= 1.4f;
					Main.dust[num71].noGravity = true;
					int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num72].velocity *= 1.4f;
					Main.dust[num72].noGravity = true;
				}
				//Main.PlaySound(4,(int)npc.position.X,(int)npc.position.Y,14);
					
				int gore = Gore.NewGore(npc.position, new Vector2(0f,3f), mod.GetGoreSlot("Gores/NightmareTailGore"), 1f);
				Main.gore[gore].velocity *= 0.4f;
				Main.gore[gore].timeLeft = 60;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return (false);
		}
	}
}