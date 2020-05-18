using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.GoldenTorizo
{
    public class OrbBomb_Golden : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Torizo Orb");
		}
		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 26;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.knockBackResist = 0;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath3;
			npc.value = 0;
			npc.lavaImmune = true;
			npc.behindTiles = true;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[44] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;
			
			//npc.dontTakeDamage = true;
			//npc.noTileCollide = true;
			npc.noGravity = true;
		}

		int damage = 60;//30;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			damage *= 2;
		}
		
		public override void NPCLoot()
		{
			Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
            MPlayer mp = player.GetModPlayer<MPlayer>();
			bool flag = false;
            for(int i = 0; i < player.inventory.Length; i++)
			{
				if(player.inventory[i].type == mod.ItemType("MissileLauncher"))
				{
					flag = true;
				}
			}
			int rand = Main.rand.Next(3);
			if(rand == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
			}
			if(rand == 1 && flag)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MissilePickup"), 1 + Main.rand.Next(5));
			}
		}
		
		public override void AI()
		{
			if(npc.ai[0] >= 1)
			{
				if(npc.velocity.X == 0 || npc.velocity.Y == 0)
				{
					npc.damage = damage;
					npc.velocity *= 0f;
					
					npc.position.X += npc.width/2f;
					npc.position.Y += npc.height;
					npc.width = 30;
					npc.height = 60;
					npc.position.X -= npc.width/2f;
					npc.position.Y -= npc.height;
					
					if(npc.ai[0] == 1)
					{
						for (int i = 0; i < 25; i++)
						{
							int newDust = Dust.NewDust(new Vector2(npc.position.X+2,npc.position.Y+npc.height-4), npc.width-2, 8, 55, 0f, 0f, 100, default(Color), 2f);
							Main.dust[newDust].velocity *= 0.5f;
							Main.dust[newDust].velocity.Y -= 4f;
							Main.dust[newDust].noGravity = true;

							newDust = Dust.NewDust(new Vector2(npc.position.X+2,npc.position.Y+npc.height-4), npc.width-2, 8, 30, 0f, 0f, 100, default(Color), 2f);
							Main.dust[newDust].velocity *= 0.5f;
							Main.dust[newDust].velocity.Y -= 4f;
							Main.dust[newDust].noGravity = true;
							
							newDust = Dust.NewDust(new Vector2(npc.position.X+4,npc.position.Y+npc.height-10), npc.width-4, 10, 55, 0f, 0f, 100, default(Color), 0.5f);
							Main.dust[newDust].velocity.X *= 0.5f;
							Main.dust[newDust].velocity.Y = -Main.rand.Next(30) * 0.1f;
							Main.dust[newDust].noGravity = true;
						}
					}
					Main.PlaySound(2,(int)npc.Center.X,(int)npc.Center.Y,14);
					
					npc.ai[0]++;
				}
				else
				{
					npc.velocity.Y += 0.1f;
				}
				if(npc.ai[0] > 2)
				{
					npc.life = 0;
					npc.HitEffect(0, 10.0);
					npc.active = false;
				}
			}
			else
			{
				npc.ai[0] = 1;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, -(Main.rand.Next(4)/2), 100, Color.White, 1.5f);
				Main.dust[dust].noGravity = true;
			}
			if(npc.life <= 0)
			{
				for(int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 55, 0f, -(Main.rand.Next(3)/2), 100, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}