using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidModPorted.Content.NPCs.Nightmare
{
    public class GravityOrb : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Orb");
			Main.npcFrameCount[Type] = 4;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					39,
					44
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}

		public override void SetDefaults()
		{
			NPC.width = 25;
			NPC.height = 25;
			NPC.scale = 0.5f;
			NPC.damage = 50;
			NPC.defense = 20;
			NPC.lifeMax = 150;
			NPC.dontTakeDamage = false;
			NPC.HitSound = null;//SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath55;//mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityOrbHit");
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = false;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			NPC.ai[0] = -1;
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
					int num71 = Dust.NewDust(new Vector2(NPC.position.X - 12.5f, NPC.position.Y - 12.5f), 25, 25, 57, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num71].velocity *= 1.4f;
					Main.dust[num71].noGravity = true;
					int num72 = Dust.NewDust(new Vector2(NPC.position.X - 12.5f, NPC.position.Y - 12.5f), 25, 25, 62, 0f, 0f, 100, default(Color), 5f);
					Main.dust[num72].velocity *= 1.4f;
					Main.dust[num72].noGravity = true;
				}
				//Main.PlaySound(SoundLoader.customSoundType, (int)laserPos.X, (int)laserPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Nightmare_GravityOrbShot"));

				this.initialized = true;
				Terraria.Audio.SoundEngine.PlaySound(2, (int)NPC.position.X, (int)NPC.position.Y, 8);
			}

			NPC.position.X += NPC.width / 2f;
			NPC.position.Y += NPC.height / 2f;
			NPC.scale = Math.Min(NPC.scale + 0.015f,1f);
			NPC.width = (int)(50 * NPC.scale);
			NPC.height = (int)(50 * NPC.scale);
			NPC.position.X -= NPC.width / 2f;
			NPC.position.Y -= NPC.height / 2f;
			
			
			rotation += 0.25f*NPC.direction;
			NPC.rotation = rotation;
			NPC.frame.Y++;
			if(NPC.frame.Y > 3)
			{
				NPC.frame.Y = 0;
			}
			
			Player player = Main.player[NPC.target];
			NPC Head = Main.npc[(int)NPC.ai[0]];
			
			float targetRot = (float)Math.Atan2(player.Center.Y-NPC.Center.Y,player.Center.X-NPC.Center.X);
			if(NPC.ai[2] == 0f)
			{
				NPC.velocity = targetRot.ToRotationVector2();
				NPC.ai[2] = 1f;
			}
			else
			{
				if(NPC.velocity.Length() <= 12)
				{
					NPC.velocity *= 1.025f;
					if(Vector2.Distance(player.Center,NPC.Center) <= 600)
					{
						NPC.velocity += targetRot.ToRotationVector2()*0.33f;
					}
				}
			}
			
			if(timeLeft <= 0)
			{
				NPC.damage--;
				if(NPC.damage < 0)
				{
					NPC.damage = 0;
				}
				NPC.alpha += 10;
				if(NPC.alpha >= 255)
				{
					NPC.active = false;
				}
			}
			timeLeft--;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			if(!Main.npc[(int)NPC.ai[0]].active || NPC.ai[0] <= -1)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
				SpriteEffects effects = SpriteEffects.None;
				if (NPC.direction == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				int height = (int)(tex.Height / Main.npcFrameCount[NPC.type]);
				sb.Draw(tex, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, height*NPC.frame.Y, tex.Width, height)), NPC.GetAlpha(Color.White), NPC.rotation, new Vector2((float)tex.Width/2f, (float)height/2f), NPC.scale, effects, 0f);
			}
			return false;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, Color.White, 3f);
				Main.dust[dust].noGravity = true;
			}
			if(NPC.life <= 0)
			{
				for(int i = 0; i < 15; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, Color.White, 5f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
