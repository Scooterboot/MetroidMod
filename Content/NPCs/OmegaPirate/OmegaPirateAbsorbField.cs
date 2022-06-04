using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidModPorted.Content.NPCs.OmegaPirate;

namespace MetroidModPorted.Content.NPCs.OmegaPirate
{
	public class OmegaPirateAbsorbField : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Absorb Field");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 70;
			NPC.height = 70;
			NPC.scale = 1f;
			NPC.damage = 0;
			NPC.defense = 30;
			NPC.lifeMax = 30000;
			NPC.dontTakeDamage = true;
			NPC.HitSound = null;
			NPC.DeathSound = null;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			NPC.localAI = new float[5];
		}

		NPC Base
		{
			get { return Main.npc[(int)NPC.ai[0]]; }
		}

		int maxDamage = 200;
		int soundCounter = 0;
		bool soundPlayed = false;
		public override void AI()
		{
			if(Base == null || !Base.active)
				NPC.ai[3] = 1;

			NPC.realLife = Base.whoAmI;

			if(NPC.ai[3] == 1)
			{
				NPC.ai[1] -= 0.05f;
				if(NPC.ai[1] <= 0f)
				{
					NPC.life = 0;
					NPC.active = false;
					return;
				}
			}
			else
			{
				if(NPC.ai[1] <= 1f)
				{
					NPC.ai[1] = Math.Min(NPC.ai[1] + 0.05f, 1f);
				}
				else
				{
					NPC.ai[1] = Math.Max(NPC.ai[1] - 0.05f, 1f);
				}
			}
			
			if(NPC.ai[2] > maxDamage)
			{
				NPC.ai[2] = maxDamage;
			}
			
			if(NPC.ai[1] > 1f && NPC.ai[1] <= 1.05f)
			{
				Color color = Color.Lerp(OmegaPirate.minGlowColor,OmegaPirate.maxGlowColor,(NPC.ai[2] / maxDamage));
				CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.position.Y, 1, 1), color, (int)NPC.ai[2], false, false);
			}
			
			if(NPC.ai[1] > 1f)
			{
				if(!soundPlayed)
				{
					SoundEngine.PlaySound(Sounds.NPCs.ElitePirate_AbsorbProjectileSound, NPC.position);
					soundPlayed = true;
				}
			}
			else
			{
				soundPlayed = false;
			}
			
			NPC.position.X += NPC.width / 2f;
			NPC.position.Y += NPC.height / 2f;
			NPC.width = (int)(70 * NPC.ai[1]);
			NPC.height = (int)(70 * NPC.ai[1]);
			NPC.position.X -= NPC.width / 2f;
			NPC.position.Y -= NPC.height / 2f;
			
			NPC.scale = 0.5f + ((float)Main.rand.Next(6)/10f) + (NPC.ai[2] / 400f);
			
			for(int i = 0; i < NPC.maxAI; i++)
			{
				NPC.localAI[i] = (i + 2f + (float)Main.rand.Next(10)) / 10f;
			}
			
			int dust1 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 87, 0f, 0f, 100, Color.White, (1.5f + (NPC.ai[2] / 400f))*NPC.ai[1]);
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].velocity *= 0.3f;
			
			NPC.rotation += 0.5f * Base.direction;
			
			if(soundCounter <= 0)
			{
				SoundEngine.PlaySound(Sounds.NPCs.ElitePirate_AbsorbSound, NPC.position);
			}
			soundCounter++;
			if(soundCounter > 30)
			{
				soundCounter = 0;
			}
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateAbsorbField").Value,
			tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateAbsorbField2").Value;
			
			Color color = new Color(255,255,255,100);
			sb.Draw(tex, new Vector2((float)((int)(NPC.Center.X - Main.screenPosition.X)), (float)((int)(NPC.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), color, NPC.rotation, new Vector2((float)tex.Width/2f, (float)tex.Height/2f), NPC.scale * NPC.ai[1], SpriteEffects.None, 0f);
			
			for(int i = 0; i < NPC.maxAI; i++)
			{
				Color color2 = new Color(255,255,255,100);
				color2 *= ((2f - NPC.localAI[i]) / 1.5f);
				sb.Draw(tex2, new Vector2((float)((int)(NPC.Center.X - Main.screenPosition.X)), (float)((int)(NPC.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, tex2.Width, tex2.Height)), color2, NPC.rotation, new Vector2((float)tex2.Width/2f, (float)tex2.Height/2f), (NPC.localAI[i] * NPC.ai[1]) / 2f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
