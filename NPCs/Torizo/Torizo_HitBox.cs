using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Torizo
{
    public class Torizo_HitBox : ModNPC
    {
		public override string Texture => mod.Name + "/NPCs/Torizo/TorizoHand_Front";
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[npc.type] = 3;
		}
		public override void SetDefaults()
		{
			npc.width = 68;
			npc.height = 62;
			npc.scale = 1f;
			npc.damage = 10;
			npc.defense = 10;
			npc.lifeMax = 1500;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
			npc.buffImmune[31] = true;
			npc.buffImmune[mod.BuffType("IceFreeze")] = true;
			npc.buffImmune[mod.BuffType("InstantFreeze")] = true;
		}
		
		NPC Base
		{
			get { return Main.npc[(int)npc.ai[0]]; }
		}
		
		public override bool PreAI()
		{
			//body
			if(npc.ai[1] == 1f)
			{
				npc.width = 56;
				npc.height = 94;
			}
			//shoulder
			if(npc.ai[1] == 2f)
			{
				npc.width = 30;
				npc.height = 30;
			}
			//arm
			if(npc.ai[1] == 3f)
			{
				npc.width = 30;
				npc.height = 30;
			}
			//hand
			if(npc.ai[1] == 4f)
			{
				npc.width = 32;
				npc.height = 32;
			}
			//thigh
			if(npc.ai[1] == 5f)
			{
				npc.width = 30;
				npc.height = 30;
			}
			//calf
			if(npc.ai[1] == 6f)
			{
				npc.width = 28;
				npc.height = 28;
			}
			return true;
		}
		public override void AI()
		{
			bool flag = (Base.alpha < 255);
			if (!Base.active)
			{
				npc.life = 0;
				if(flag)
				{
					if(npc.ai[1] == 1f)
					{
						Main.PlaySound(npc.DeathSound,npc.Center);
					}
					npc.HitEffect(0, 10.0);
				}
				npc.active = false;
				return;
			}
			npc.damage = Base.damage;
			npc.defense = Base.defense;
			npc.GivenName = Base.GivenName;
			npc.direction = Base.direction;
			if(npc.ai[1] > 1f)
			{
				npc.dontTakeDamage = true;
				npc.realLife = Base.whoAmI;
			}
			else
			{
				if(npc.ai[1] == 1f)
				{
					npc.realLife = Base.whoAmI;
				}
				npc.dontTakeDamage = Base.dontTakeDamage;
			}
		}
		
		public override bool? CanBeHitByItem(Player player, Item item) => (npc.ai[1] <= 1f);
		public override bool? CanBeHitByProjectile(Projectile projectile) => (npc.ai[1] <= 1f);
		
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(npc.ai[1] == 0f)
			{
				Base.StrikeNPC((int)(damage*0.25f),0f,Base.direction);
			}
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.ai[1] <= 1f)
			{
				Base.HitEffect(hitDirection,damage);
			}
			
			if(npc.life <= 0)
			{
				Gore newGore;
				if(npc.ai[1] == 0f)
				{
					for(int i = 0; i < 3; i++)
					{
						newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoHeadGore1"))];
						newGore.timeLeft = 60;
						
						newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoHeadGore" + (2+i)))];
						newGore.timeLeft = 60;
					}
				}
				if(npc.ai[1] == 1f)
				{
					newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoBodyGore1"))];
					newGore.timeLeft = 60;
					
					for(int i = 0; i < 3; i++)
					{
						newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoBodyGore2"))];
						newGore.timeLeft = 60;
						
						newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoBodyGore3"))];
						newGore.timeLeft = 60;
						
						newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoBodyGore4"))];
						newGore.timeLeft = 60;
					}
				}
				if(npc.ai[1] >= 2f)
				{
					if(npc.ai[1] == 2)
					{
						newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoGore1"))];
						newGore.timeLeft = 60;
					}
					
					newGore = Main.gore[Gore.NewGore(npc.Center, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoGore" + npc.ai[1]))];
					newGore.timeLeft = 60;
					
					if(npc.ai[1] == 6)
					{
						newGore = Main.gore[Gore.NewGore(npc.Center+new Vector2(0,30), new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f, mod.GetGoreSlot("Gores/TorizoGore7"))];
						newGore.timeLeft = 60;
					}
				}
				
				for (int num70 = 0; num70 < 10; num70++)
				{
					int num71 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 5f);
					Main.dust[num71].velocity *= 1.4f;
					Main.dust[num71].noGravity = true;
					int num72 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num72].velocity *= 1.4f;
					Main.dust[num72].noGravity = true;
				}
			}
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => (npc.ai[1] <= 0f);
	}
}