using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.NPCs.Torizo
{
	public class OrbBomb : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo Orb");
		}
		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 26;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.knockBackResist = 0;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath3;
			NPC.value = 0;
			NPC.lavaImmune = true;
			NPC.behindTiles = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 0;
			NPC.buffImmune[20] = true;
			NPC.buffImmune[24] = true;
			NPC.buffImmune[31] = true;
			NPC.buffImmune[39] = true;
			NPC.buffImmune[44] = true;
			//NPC.buffImmune[mod.BuffType("PhazonDebuff")] = true;
			
			//NPC.dontTakeDamage = true;
			//NPC.noTileCollide = true;
			NPC.noGravity = true;
		}

		int damage = 30;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			damage *= 2;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Heart, 4));
		}
		
		public override void AI()
		{
			if(NPC.ai[0] >= 1)
			{
				if(NPC.velocity.X == 0 || NPC.velocity.Y == 0)
				{
					NPC.damage = damage;
					NPC.velocity *= 0f;
					
					NPC.position.X += NPC.width/2f;
					NPC.position.Y += NPC.height;
					NPC.width = 30;
					NPC.height = 60;
					NPC.position.X -= NPC.width/2f;
					NPC.position.Y -= NPC.height;
					
					if(NPC.ai[0] == 1)
					{
						for (int i = 0; i < 25; i++)
						{
							int newDust = Dust.NewDust(new Vector2(NPC.position.X+2,NPC.position.Y+NPC.height-4), NPC.width-2, 8, 55, 0f, 0f, 100, default(Color), 2f);
							Main.dust[newDust].velocity *= 0.5f;
							Main.dust[newDust].velocity.Y -= 4f;
							Main.dust[newDust].noGravity = true;

							newDust = Dust.NewDust(new Vector2(NPC.position.X+2,NPC.position.Y+NPC.height-4), NPC.width-2, 8, 30, 0f, 0f, 100, default(Color), 2f);
							Main.dust[newDust].velocity *= 0.5f;
							Main.dust[newDust].velocity.Y -= 4f;
							Main.dust[newDust].noGravity = true;
							
							newDust = Dust.NewDust(new Vector2(NPC.position.X+4,NPC.position.Y+NPC.height-10), NPC.width-4, 10, 55, 0f, 0f, 100, default(Color), 0.5f);
							Main.dust[newDust].velocity.X *= 0.5f;
							Main.dust[newDust].velocity.Y = -Main.rand.Next(30) * 0.1f;
							Main.dust[newDust].noGravity = true;
						}
					}
					SoundEngine.PlaySound(2,(int)NPC.Center.X,(int)NPC.Center.Y,14);
					
					NPC.ai[0]++;
				}
				else
				{
					NPC.velocity.Y += 0.1f;
				}
				if(NPC.ai[0] > 2)
				{
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
			}
			else
			{
				NPC.ai[0] = 1;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, -(Main.rand.Next(4)/2), 100, Color.White, 1.5f);
				Main.dust[dust].noGravity = true;
			}
			if(NPC.life <= 0)
			{
				for(int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 55, 0f, -(Main.rand.Next(3)/2), 100, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
