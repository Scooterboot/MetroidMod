using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs
{

	public class LarvalMetroid : ModNPC
    {
		public float movingSpeed = 0;
		public bool movingUp = false;
		public bool grappled = false;
		public bool frozen = false;
		public bool spawn = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Larval Metroid");
			Main.npcFrameCount[npc.type] = 3;
		}
		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 38;
			npc.damage = 20;
			npc.defense = 23;
			npc.lifeMax = 100;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 1, 60);
			npc.knockBackResist = 0.75f;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("MetroidBanner");
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.CursedInferno] = true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return (spawnInfo.player.ZoneCorrupt || spawnInfo.player.ZoneCrimson) && NPC.downedBoss2 ? 0.03f : 0f;
		}
		public override bool PreAI()
		{
			if (!spawn)
			{
				npc.netUpdate = true;
				npc.lifeMax = npc.life;
				npc.npcSlots *= npc.scale;
				npc.knockBackResist *= 2f - npc.scale;
				npc.scale = (Main.rand.Next(5, 10) * 0.1f);
				npc.life = (int)((float)npc.life * npc.scale);
				npc.value = (float)((int)(npc.value * npc.scale));
				npc.damage = (int)((float)npc.damage * npc.scale);
				npc.defense = (int)((float)npc.defense * npc.scale);

				spawn = true;
			}
			return true;
		}
		public override void AI()
		{
			frozen = npc.GetGlobalNPC<MNPC>(mod).froze;
			if (grappled)
			{
				if (Main.player[npc.target].dead || !Main.player[npc.target].active || frozen)
				{
					grappled = false;
					return;
				}
				npc.rotation = 0;
				npc.position = new Vector2(Main.player[npc.target].Center.X-(npc.width/2),Main.player[npc.target].Center.Y-(npc.height/2)-16);
				Main.player[npc.target].velocity.X *= 0.95f;
			}
			else if(!frozen)
			{
				npc.TargetClosest();
				
				if (Main.player[npc.target].Center.X < npc.Center.X)
				{
					if (npc.velocity.X > -2) {npc.velocity.X -= 0.2f;}
				}
				else if (Main.player[npc.target].Center.X > npc.Center.X)
				{
					if (npc.velocity.X < 2) {npc.velocity.X += 0.2f;}
				}
				if (Main.player[npc.target].Center.Y < npc.Center.Y)
				{
					if (npc.velocity.Y > -2) npc.velocity.Y -= 0.2f;
				}
				else if (Main.player[npc.target].Center.Y > npc.Center.Y)
				{
					if (npc.velocity.Y < 2) npc.velocity.Y += 0.2f;
				}

				if (movingUp)
				{
					movingSpeed -= 0.02f;
				}
				else
				{
					movingSpeed += 0.02f;
				}
				if (movingSpeed <= -0.20f)
				{
					movingUp = false;
				}
				if (movingSpeed >= 0.20f)
				{
					movingUp = true;
				}
				npc.velocity.Y += movingSpeed;
				
				Vector2 vector = npc.velocity;
				npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height, false, false);
				if (npc.velocity.X != vector.X)
				{
					npc.velocity.X = -vector.X;
				}
				if (npc.velocity.Y != vector.Y)
				{
					npc.velocity.Y = -vector.Y;
				}
				
				Player player = Main.player[npc.target];
				if (Vector2.Distance(npc.Center, player.Center) <= 25f)
				{
					grappled = true;
				}
                		npc.noGravity = true;
			}
			if(frozen)
			{
				npc.damage = 0;
                		npc.frame.Y = 0;
                		npc.noGravity = false;
				npc.rotation += npc.velocity.X * 0.1f;
				if (npc.velocity.Y == 0f)
				{
					npc.velocity.X = npc.velocity.X * 0.98f;
					if ((double)npc.velocity.X > -0.01 && (double)npc.velocity.X < 0.01)
					{
						npc.velocity.X = 0f;
					}
				}
			}
			
			if (Main.netMode == 2 && npc.whoAmI < 200)
			{
				NetMessage.SendData(23, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0);
			}
		}
		
		/*public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if(grappled)
			{
				hitDir = 0;
				player.knockbackResist = 0f;
			}
		}*/
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if(frozen && damage >= 20)
			{
				damage = (int)((double)(damage * (2 - (double)npc.scale)) + (double)npc.defense * 0.5);
			}
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore1"), npc.scale);
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore1"), npc.scale);
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore2"), npc.scale);
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore2"), npc.scale);
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (projectile.type == mod.ProjectileType("MBBomb") || projectile.type == mod.ProjectileType("Powerbomb"))
			{
				grappled = false;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			int num = 1;
			if (!Main.dedServ)
			{
				num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			}
			if(!frozen)
			{
				if (!grappled) npc.rotation = npc.velocity.X * 0.1f;
				npc.frameCounter += 1.0;
				if (npc.frameCounter >= 8.0)
				{
					npc.frame.Y = npc.frame.Y + num;
					npc.frameCounter = 0.0;
				}
				if (npc.frame.Y >= num * Main.npcFrameCount[npc.type])
				{
					npc.frame.Y = 0;
				}
			}
			else
			{
				npc.frame.Y = num;
			}
		}
		
    }
}
