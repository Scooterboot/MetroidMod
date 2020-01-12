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
    public class GravityOrb : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Orb");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 25;
			npc.height = 25;
			npc.scale = 0.5f;
			npc.damage = 50;
			npc.defense = 20;
			npc.lifeMax = 150;
			npc.dontTakeDamage = false;
			npc.HitSound = null;//SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath55;//mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityOrbHit");
			npc.noGravity = true;
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = false;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[44] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;
			npc.ai[0] = -1;
		}

		int timeLeft = 300;
		float rotation = 0f;
		bool initialized = false;
		public override void AI()
		{
			// Nothing needs to be set, just ported over the visual and audial effects.
			if (!initialized)
			{
				for (int num70 = 0; num70 < 10; num70++)
				{
					int num71 = Dust.NewDust(new Vector2(npc.position.X - 12.5f, npc.position.Y - 12.5f), 25, 25, 57, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num71].velocity *= 1.4f;
					Main.dust[num71].noGravity = true;
					int num72 = Dust.NewDust(new Vector2(npc.position.X - 12.5f, npc.position.Y - 12.5f), 25, 25, 62, 0f, 0f, 100, default(Color), 5f);
					Main.dust[num72].velocity *= 1.4f;
					Main.dust[num72].noGravity = true;
				}
				//Main.PlaySound(SoundLoader.customSoundType, (int)laserPos.X, (int)laserPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityOrbShot"));

				this.initialized = true;
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
			}

			npc.position.X += npc.width / 2f;
			npc.position.Y += npc.height / 2f;
			npc.scale = Math.Min(npc.scale + 0.015f,1f);
			npc.width = (int)(50 * npc.scale);
			npc.height = (int)(50 * npc.scale);
			npc.position.X -= npc.width / 2f;
			npc.position.Y -= npc.height / 2f;
			
			
			rotation += 0.25f*npc.direction;
			npc.rotation = rotation;
			npc.frame.Y++;
			if(npc.frame.Y > 3)
			{
				npc.frame.Y = 0;
			}
			
			Player player = Main.player[npc.target];
			NPC Head = Main.npc[(int)npc.ai[0]];
			
			float targetRot = (float)Math.Atan2(player.Center.Y-npc.Center.Y,player.Center.X-npc.Center.X);
			if(npc.ai[2] == 0f)
			{
				npc.velocity = targetRot.ToRotationVector2();
				npc.ai[2] = 1f;
			}
			else
			{
				if(npc.velocity.Length() <= 12)
				{
					npc.velocity *= 1.025f;
					if(Vector2.Distance(player.Center,npc.Center) <= 600)
					{
						npc.velocity += targetRot.ToRotationVector2()*0.33f;
					}
				}
			}
			
			if(timeLeft <= 0)
			{
				npc.damage--;
				if(npc.damage < 0)
				{
					npc.damage = 0;
				}
				npc.alpha += 10;
				if(npc.alpha >= 255)
				{
					npc.active = false;
				}
			}
			timeLeft--;
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			if(!Main.npc[(int)npc.ai[0]].active || npc.ai[0] <= -1)
			{
				Texture2D tex = Main.npcTexture[npc.type];
				SpriteEffects effects = SpriteEffects.None;
				if (npc.direction == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				int height = (int)(tex.Height / Main.npcFrameCount[npc.type]);
				sb.Draw(tex, npc.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, height*npc.frame.Y, tex.Width, height)), npc.GetAlpha(Color.White), npc.rotation, new Vector2((float)tex.Width/2f, (float)height/2f), npc.scale, effects, 0f);
			}
			return false;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 62, 0f, 0f, 100, Color.White, 3f);
				Main.dust[dust].noGravity = true;
			}
			if(npc.life <= 0)
			{
				for(int i = 0; i < 15; i++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 62, 0f, 0f, 100, Color.White, 5f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}