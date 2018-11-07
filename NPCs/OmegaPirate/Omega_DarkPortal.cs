using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod;
using MetroidMod.NPCs.OmegaPirate;

namespace MetroidMod.NPCs.OmegaPirate
{
    public class Omega_DarkPortal : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Portal");
		}
		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 32;
			npc.scale = 0f;
			npc.damage = 0;
			npc.defense = 30;
			npc.lifeMax = 30000;
			npc.dontTakeDamage = true;
			npc.HitSound = null;
			npc.DeathSound = null;
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
		}

		bool init = false;
		NPC Base;
		public override bool PreAI()
		{
			if(!init)
			{
				npc.ai[1] = -1;
				Base = Main.npc[(int)npc.ai[0]];
				init = true;
			}
			return true;
		}
		
		public override void AI()
		{
			if(Base == null || !Base.active)
			{
				npc.ai[2] = 1;
			}
			if(npc.ai[2] == 1)
			{
				npc.scale -= 0.05f;
				if(npc.scale <= 0f)
				{
					npc.life = 0;
					npc.active = false;
					return;
				}
			}
			else
			{
				npc.scale = Math.Min(npc.scale + 0.05f, 1f);
			}
			
			npc.rotation += 0.25f;
			
			if(npc.ai[1] != -1)
			{
				NPC target = Main.npc[(int)npc.ai[1]];
				
				if(target != null && target.active && npc.scale >= 0.5f)
				{
					npc.localAI[0] += 1f;
					if(npc.localAI[0] > 10f)
					{
						float angle = (float)MetroidMod.ConvertToRadians(Main.rand.Next(360));
						Vector2 vel = angle.ToRotationVector2() * 15f;
						int p = Projectile.NewProjectile(npc.Center.X,npc.Center.Y,vel.X,vel.Y,mod.ProjectileType("Omega_PhazonParticle"),0,0f);
						Main.projectile[p].ai[0] = target.position.X + Main.rand.Next(target.width);
						Main.projectile[p].ai[1] = target.position.Y + Main.rand.Next(target.height);
						npc.localAI[0] = 0f;
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Texture2D tex = mod.GetTexture("NPCs/OmegaPirate/Omega_DarkPortal"),
			tex2 = mod.GetTexture("NPCs/OmegaPirate/Omega_DarkPortal2");
			
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color25 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			
			Vector2 vector60 = npc.Center - Main.screenPosition;
			Color alpha4 = npc.GetAlpha(color25);
			Vector2 origin8 = new Vector2((float)tex.Width, (float)tex.Height) / 2f;
			
			Color color57 = alpha4 * 0.8f;
			color57.A /= 2;
			Color color58 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color58.A = alpha4.A;
			float num279 = 0.95f + (npc.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color58 *= num279;
			float scale13 = 0.6f + npc.scale * 0.6f * num279;
			sb.Draw(tex2, vector60, null, color58, -npc.rotation * 2, origin8, npc.scale, spriteEffects, 0f);
			sb.Draw(Main.extraTexture[50], vector60, null, color58, -npc.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(Main.extraTexture[50], vector60, null, alpha4, -npc.rotation, origin8, npc.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex, vector60, null, color57, -npc.rotation * 0.7f, origin8, npc.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(Main.extraTexture[50], vector60, null, alpha4 * 0.8f, npc.rotation * 0.5f, origin8, npc.scale * 0.9f, spriteEffects, 0f);
			sb.Draw(tex, vector60, null, alpha4, npc.rotation, origin8, npc.scale, spriteEffects, 0f);
			return false;
		}
	}
}