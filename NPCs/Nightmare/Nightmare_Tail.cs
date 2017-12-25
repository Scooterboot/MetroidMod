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
		Projectile gravField;
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
			npc.dontTakeDamage = Head.dontTakeDamage;

			npc.Center = Head.Center + new Vector2(-76*Head.direction,88);
			npc.velocity *= 0f;
			
			if(npc.ai[1] == 1)
			{
				if(npc.ai[2] == 0)
				{
					// spawn gravity field projectile
					Vector2 spawnPos = new Vector2(npc.Center.X+26*Head.direction,npc.Center.Y+14);
					int gf = Projectile.NewProjectile(spawnPos.X,spawnPos.Y,0f,0f,mod.ProjectileType("NightmareGravityField"),0,0f);
					gravField = Main.projectile[gf];
					gravField.ai[0] = Head.whoAmI;
					gravField.ai[1] = npc.whoAmI;
					Main.PlaySound(SoundLoader.customSoundType, (int)gravField.Center.X, (int)gravField.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityField_Activate"));
					
					for (int num70 = 0; num70 < 15; num70++)
					{
						int num71 = Dust.NewDust(new Vector2(spawnPos.X-18f,spawnPos.Y-18f), 36, 36, 54, 0f, 0f, 100, default(Color), 1f+Main.rand.Next(3));
						//Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
					}
					
					npc.ai[2] = 1;
				}
				npc.ai[1] = 0;
			}
			if(gravField == null || !gravField.active)
			{
				npc.ai[2] = 0;
				npc.ai[3] = 0;
			}
			else if(npc.ai[2] == 1 && npc.ai[3] > 2000)
			{
				gravField.localAI[0] = 1;
			}
			//npc.dontTakeDamage = (npc.ai[2] == 0 || Head.dontTakeDamage);
		}
		
		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if(npc.ai[2] == 1)
			{
				npc.ai[3] += damage;
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(npc.ai[2] == 1)
			{
				npc.ai[3] += damage;
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
			if (Main.netMode != 2)
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
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
		}
	}
}