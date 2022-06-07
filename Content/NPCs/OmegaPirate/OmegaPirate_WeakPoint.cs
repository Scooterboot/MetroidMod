using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidModPorted.Content.NPCs.OmegaPirate
{
	public class OmegaPirate_WeakPoint : ModNPC
	{
		public override string Texture => $"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderRight";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					39,
					44,
					ModContent.BuffType<Buffs.PhazonDebuff>()
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}
		public override void SetDefaults()
		{
			NPC.width = 58;
			NPC.height = 58;
			NPC.scale = 1f;
			NPC.damage = 0;
			NPC.defense = 10;
			NPC.lifeMax = 5000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
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
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("An experiment created by the space pirates. It is a hulking monster corrupted by a biomass known as Phazon. It's capable of absorbing projectiles with its hands and firing grenades from a distance. Get too close and it will react by smashing the ground to create an energy wave. Even if you smash its armor the creature will go invisible and attempt to absorb Phazon to repair its defenses.")
			});
		}
		public override bool PreAI()
		{
			if(NPC.ai[1] == 1f)
			{
				NPC.width = 44;
				NPC.height = 44;
			}
			return true;
		}
		public override void AI()
		{
			NPC Base = Main.npc[(int)NPC.ai[0]];
			bool flag = (Base.alpha < 255);
			if (!Base.active)
			{
				SoundEngine.PlaySound((SoundStyle)NPC.DeathSound,NPC.Center);
				NPC.life = 0;
				if(flag)
				{
					NPC.HitEffect(0, 10.0);
				}
				NPC.active = false;
				return;
			}
			NPC.dontTakeDamage = true;
			if(Base.ai[0] == 1)
			{
				NPC.dontTakeDamage = false;
			}
			NPC.GivenName = Base.GivenName;
		}
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				for (int m = 0; m < (NPC.life <= 0 ? 20 : 5); m++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 68, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
					Main.dust[dustID].noGravity = true;
				}
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(projectile.penetrate > 0 && projectile.aiStyle != 3)
			{
				projectile.penetrate = 0;
				projectile.netUpdate = true;
			}
		}
	}
}
