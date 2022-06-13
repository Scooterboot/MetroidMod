using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidModPorted;
using MetroidModPorted.Content.NPCs.OmegaPirate;

namespace MetroidModPorted.Content.NPCs.OmegaPirate
{
	public class Omega_DarkPortal : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Portal");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 32;
			NPC.scale = 0f;
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
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<OmegaPirate>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("The Omega Pirate will occasionally slip into an alternate dimension. This portal is used to perform that act.")
			});
		}

		NPC Base
		{
			get { return Main.npc[(int)NPC.ai[3]]; }
		}
		
		public override void AI()
		{
			if(Base == null || !Base.active)
				NPC.ai[2] = 1;

			if(NPC.ai[2] == 1)
			{
				NPC.scale -= 0.05f;
				if(NPC.scale <= 0f)
				{
					NPC.life = 0;
					NPC.active = false;
					return;
				}
			}
			else
			{
				NPC.scale = Math.Min(NPC.scale + 0.05f, 1f);
			}
			
			NPC.rotation += 0.25f;
			
			if(NPC.scale >= 0.5f && NPC.ai[2] != 1)
			{
				NPC.localAI[0] += 1f;
				if(NPC.localAI[0] > 10f)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float angle = (float)Angle.ConvertToRadians(Main.rand.Next(360));
						Vector2 vel = angle.ToRotationVector2() * 15f;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, vel.X, vel.Y, ModContent.ProjectileType<Projectiles.Boss.Omega_PhazonParticle>(), 0, 0f, Main.myPlayer, NPC.ai[0], NPC.ai[1]);
					}
					NPC.localAI[0] = 0f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/Omega_DarkPortal").Value,
			tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/Omega_DarkPortal2").Value;
			
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color25 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
			
			Vector2 vector60 = NPC.Center - Main.screenPosition;
			Color alpha4 = NPC.GetAlpha(color25);
			Vector2 origin8 = new Vector2((float)tex.Width, (float)tex.Height) / 2f;
			
			Color color57 = alpha4 * 0.8f;
			color57.A /= 2;
			Color color58 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color58.A = alpha4.A;
			float num279 = 0.95f + (NPC.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color58 *= num279;
			float scale13 = 0.6f + NPC.scale * 0.6f * num279;
			sb.Draw(tex2, vector60, null, color58, -NPC.rotation * 2, origin8, NPC.scale, spriteEffects, 0f);
			sb.Draw(Terraria.GameContent.TextureAssets.Extra[50].Value, vector60, null, color58, -NPC.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(Terraria.GameContent.TextureAssets.Extra[50].Value, vector60, null, alpha4, -NPC.rotation, origin8, NPC.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex, vector60, null, color57, -NPC.rotation * 0.7f, origin8, NPC.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(Terraria.GameContent.TextureAssets.Extra[50].Value, vector60, null, alpha4 * 0.8f, NPC.rotation * 0.5f, origin8, NPC.scale * 0.9f, spriteEffects, 0f);
			sb.Draw(tex, vector60, null, alpha4, NPC.rotation, origin8, NPC.scale, spriteEffects, 0f);
			return false;
		}
	}
}
