using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.OmegaPirate
{
    public class OmegaPirate_WeakPoint : ModNPC
    {
		public override string Texture
		{
			get
			{
				return mod.Name + "/NPCs/OmegaPirate/OmegaPirate_ArmShoulderRight";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			npc.width = 58;
			npc.height = 58;
			npc.scale = 1f;
			//npc.boss = true;
			npc.damage = 0;
			npc.defense = 20;
			npc.lifeMax = 10000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[44] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
		}
		public override bool PreAI()
		{
			if(npc.ai[1] == 1f)
			{
				npc.width = 44;
				npc.height = 44;
			}
			return true;
		}
		public override void AI()
		{
			NPC Base = Main.npc[(int)npc.ai[0]];
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
			npc.dontTakeDamage = true;
			if(Base.ai[0] == 1)
			{
				npc.dontTakeDamage = false;
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			return false;
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
			if(npc.life <= 0)
			{
				npc.boss = false;
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(projectile.penetrate > 0 && projectile.aiStyle != 3)
			{
				projectile.penetrate = 0;
			}
		}
	}
}