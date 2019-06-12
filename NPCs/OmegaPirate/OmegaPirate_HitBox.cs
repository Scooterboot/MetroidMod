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

namespace MetroidMod.NPCs.OmegaPirate
{
    public class OmegaPirate_HitBox : ModNPC
    {
		public override string Texture
		{
			get
			{
				return mod.Name + "/NPCs/OmegaPirate/OmegaPirate_Body";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
			Main.npcFrameCount[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			npc.width = 82;
			npc.height = 126;
			npc.scale = 1f;
			npc.damage = 0;
			npc.defense = 30;
			npc.lifeMax = 30000;
			npc.dontTakeDamage = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[69] = true;
			npc.buffImmune[70] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;
		}
		public override bool PreAI()
		{
			//shoulder
			if(npc.ai[1] == 1f)
			{
				npc.width = 58;
				npc.height = 58;
			}
			//elbow
			if(npc.ai[1] == 2f)
			{
				npc.width = 38;
				npc.height = 38;
			}
			//hand
			if(npc.ai[1] == 3f)
			{
				npc.width = 60;
				npc.height = 60;
			}
			//thigh
			if(npc.ai[1] == 4f)
			{
				npc.width = 44;
				npc.height = 44;
			}
			//shin
			if(npc.ai[1] == 5f)
			{
				npc.width = 50;
				npc.height = 50;
			}
			//foot
			if(npc.ai[1] == 6f)
			{
				npc.width = 40;
				npc.height = 40;
			}
			//cannon
			if(npc.ai[1] == 7f)
			{
				npc.width = 48;
				npc.height = 48;
			}
			return true;
		}
		NPC Base;
		public override void AI()
		{
			Base = Main.npc[(int)npc.ai[0]];
			bool flag = (Base.alpha < 255);
			if (!Base.active)
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
			npc.damage = Base.damage;
			npc.defense = Base.defense;
			if(npc.ai[1] != 0f || Base.dontTakeDamage || Base.ai[0] != 2)
			{
				npc.dontTakeDamage = true;
				for(int i = 0; i < npc.buffTime.Length; i++)
				{
					npc.buffTime[i] = 0;
				}
			}
			npc.GivenName = Base.GivenName;
			npc.chaseable != npc.dontTakeDamage;
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return npc.ai[1] == 0f;
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return npc.ai[1] == 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				for (int m = 0; m < (npc.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 68, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, npc.life <= 0 && m % 2 == 0 ? 3f : 1f);
					Main.dust[dustID].noGravity = true;
				}
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(projectile.penetrate > 0 && projectile.aiStyle != 3)
			{
				projectile.penetrate = 0;
			}
			if(Base != null && Base.active && Base.ai[0] == 2 && Base.ai[1] == 2 && npc.ai[1] == 0f)
			{
				Base.ai[3] += damage;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
	}
}
