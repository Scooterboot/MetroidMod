using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Nightmare
{
	public class Nightmare_Body : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nightmare");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value); //this and above line hides the entity from the bestiary
		}
		public override void SetDefaults()
		{
			NPC.width = 212;
			NPC.height = 122;
			NPC.scale = 1f;
			NPC.damage = 0;//50;
			NPC.defense = 30;
			NPC.lifeMax = 30000;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Nightmare>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
		}
		public override void AI()
		{
			NPC Head = Main.npc[(int)NPC.ai[0]];
			bool flag = (Head.alpha < 255);
			if (!Head.active)
			{
				SoundEngine.PlaySound((SoundStyle)NPC.DeathSound, NPC.Center);
				NPC.life = 0;
				if (flag)
				{
					NPC.HitEffect(0, 10.0);
				}
				NPC.active = false;
				return;
			}

			NPC.damage = Head.damage;
			NPC.Center = Head.Center + new Vector2(-44 * Head.direction, -5);
			NPC.velocity *= 0f;

			if (!Head.dontTakeDamage)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].friendly && Main.projectile[i].damage > 0)
					{
						Projectile P = Main.projectile[i];
						Rectangle projRect = new Rectangle((int)(P.position.X + P.velocity.X), (int)(P.position.Y + P.velocity.Y), P.width, P.height);

						Rectangle npcRect1 = new Rectangle((int)NPC.Center.X - 102, (int)NPC.Center.Y - 59, 106, 104);
						Rectangle npcRect2 = new Rectangle((int)NPC.Center.X - 102, (int)NPC.Center.Y + 45, 202, 16);
						if (Head.direction == -1)
						{
							npcRect1.X = (int)NPC.Center.X - 4;
							npcRect2.X = (int)NPC.Center.X - 100;
						}
						if (projRect.Intersects(npcRect1) || projRect.Intersects(npcRect2))
						{
							if (Main.projectile[i].penetrate > 0)
							{
								Main.projectile[i].penetrate--;
								if (Main.projectile[i].penetrate == 0)
								{
									break;
								}
							}
						}
					}
				}
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (NPC.life <= 0)
				{
					for (int num70 = 0; num70 < 70; num70++)
					{
						int num71 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					//Main.PlaySound(4,(int)NPC.position.X,(int)NPC.position.Y,14);
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
	}
}
