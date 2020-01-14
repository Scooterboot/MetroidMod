using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.NPCs.OmegaPirate;

namespace MetroidMod.NPCs.OmegaPirate
{
    public class OmegaPirateAbsorbField : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Absorb Field");
		}
		public override void SetDefaults()
		{
			npc.width = 70;
			npc.height = 70;
			npc.scale = 1f;
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
			npc.localAI = new float[5];
		}

		NPC Base
		{
			get { return Main.npc[(int)npc.ai[0]]; }
		}

		int maxDamage = 200;
		int soundCounter = 0;
		bool soundPlayed = false;
		public override void AI()
		{
			if(Base == null || !Base.active)
				npc.ai[3] = 1;

			npc.realLife = Base.whoAmI;

			if(npc.ai[3] == 1)
			{
				npc.ai[1] -= 0.05f;
				if(npc.ai[1] <= 0f)
				{
					npc.life = 0;
					npc.active = false;
					return;
				}
			}
			else
			{
				if(npc.ai[1] <= 1f)
				{
					npc.ai[1] = Math.Min(npc.ai[1] + 0.05f, 1f);
				}
				else
				{
					npc.ai[1] = Math.Max(npc.ai[1] - 0.05f, 1f);
				}
			}
			
			if(npc.ai[2] > maxDamage)
			{
				npc.ai[2] = maxDamage;
			}
			
			if(npc.ai[1] > 1f && npc.ai[1] <= 1.05f)
			{
				Color color = Color.Lerp(OmegaPirate.minGlowColor,OmegaPirate.maxGlowColor,(npc.ai[2] / maxDamage));
				CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.position.Y, 1, 1), color, (int)npc.ai[2], false, false);
			}
			
			if(npc.ai[1] > 1f)
			{
				if(!soundPlayed)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/ElitePirate_AbsorbProjectileSound"));
					soundPlayed = true;
				}
			}
			else
			{
				soundPlayed = false;
			}
			
			npc.position.X += npc.width / 2f;
			npc.position.Y += npc.height / 2f;
			npc.width = (int)(70 * npc.ai[1]);
			npc.height = (int)(70 * npc.ai[1]);
			npc.position.X -= npc.width / 2f;
			npc.position.Y -= npc.height / 2f;
			
			npc.scale = 0.5f + ((float)Main.rand.Next(6)/10f) + (npc.ai[2] / 400f);
			
			for(int i = 0; i < NPC.maxAI; i++)
			{
				npc.localAI[i] = (i + 2f + (float)Main.rand.Next(10)) / 10f;
			}
			
			int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 100, Color.White, (1.5f + (npc.ai[2] / 400f))*npc.ai[1]);
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].velocity *= 0.3f;
			
			npc.rotation += 0.5f * Base.direction;
			
			if(soundCounter <= 0)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/ElitePirate_AbsorbSound"));
			}
			soundCounter++;
			if(soundCounter > 30)
			{
				soundCounter = 0;
			}
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Texture2D tex = mod.GetTexture("NPCs/OmegaPirate/OmegaPirateAbsorbField"),
			tex2 = mod.GetTexture("NPCs/OmegaPirate/OmegaPirateAbsorbField2");
			
			Color color = new Color(255,255,255,100);
			sb.Draw(tex, new Vector2((float)((int)(npc.Center.X - Main.screenPosition.X)), (float)((int)(npc.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), color, npc.rotation, new Vector2((float)tex.Width/2f, (float)tex.Height/2f), npc.scale * npc.ai[1], SpriteEffects.None, 0f);
			
			for(int i = 0; i < NPC.maxAI; i++)
			{
				Color color2 = new Color(255,255,255,100);
				color2 *= ((2f - npc.localAI[i]) / 1.5f);
				sb.Draw(tex2, new Vector2((float)((int)(npc.Center.X - Main.screenPosition.X)), (float)((int)(npc.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, tex2.Width, tex2.Height)), color2, npc.rotation, new Vector2((float)tex2.Width/2f, (float)tex2.Height/2f), (npc.localAI[i] * npc.ai[1]) / 2f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}