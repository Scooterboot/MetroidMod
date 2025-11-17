using System.Collections.Generic;
using MetroidMod.Common.Configs;
using MetroidMod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;

namespace MetroidMod.Content.NPCs.Mobs.Metroid
{
	public class Mochtroid : MNPC
	{
		internal readonly float speed = 3.5F;
		internal readonly float acceleration = .04F;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
		}
		public override void SetDefaults()
		{
			NPC.scale = 1f;
			NPC.width = 46;
			NPC.height = 46;
			NPC.damage = 1;
			NPC.defense = 10;
			NPC.lifeMax = 80;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 1f;
			NPC.value = Item.buyPrice(0, 0, 1, 60);

			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = Sounds.NPCs.Mochtroid;

		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (MConfigMain.Instance.disablemobspawn == true)
			{
				return 0f;
			}
			float chance = SpawnCondition.Dungeon.Chance * 0.08f;
			if (Main.hardMode)
			{
				chance *= 0.5f;
			}
			return chance;
		}
		public override bool? CanFallThroughPlatforms()
		{
			return true;
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
				new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Mochtroid")
			});
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < 16; i++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.height, NPC.height, DustID.t_Slime, 0, 0, 120, Color.LimeGreen, 1.5f);
					d.velocity = NPC.DirectionTo(d.position) * 2;
				}
			}
		}
		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (STATE == (int)StateID.Frozen)
			{
				modifiers.ScalingArmorPenetration += 1f;
				modifiers.Knockback *= 0.5f;
				modifiers.SetCrit();
			}
			if (NPC.HasBuff(BuffID.Frostburn) || NPC.HasBuff(BuffID.Frostburn2) || item.type == ItemID.IceBlade || item.type == ItemID.Frostbrand)
			{
				modifiers.ScalingArmorPenetration += 1f;
			}
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (STATE == (int)StateID.Frozen || (projectile.Name.Contains("Missile") && projectile.Name.Contains("Stardust")))
			{
				modifiers.ScalingArmorPenetration += 1f;
				modifiers.Knockback *= 0.5f;
				modifiers.SetCrit();
				if (projectile.Name.Contains("Missile") || projectile.Name.Contains("Spreader") || projectile.Name.Contains("Combo"))
				{
					modifiers.SourceDamage *= 2;
					modifiers.DamageVariationScale *= 0;
				}
			}
			if (NPC.HasBuff(BuffID.Frostburn) || NPC.HasBuff(BuffID.Frostburn2) || projectile.coldDamage)
			{
				modifiers.ScalingArmorPenetration += 1f;
			}
		}

		private float STATE
		{
			get { return NPC.ai[0]; }
			set { NPC.ai[0] = value; }
		}
		private float AI_Counter
		{
			get { return NPC.ai[1]; }
			set { NPC.ai[1] = value; }
		}
		private enum StateID : int
		{
			Spawn,
			Idle,
			Aggroed,
			Frozen
		}
		private void SetStats()
		{
			NPC.scale = 0.5f + (Main.rand.NextFloat() * 0.5f);
			NPC.Size *= NPC.scale;
			NPC.defense = NPC.defDefense = (int)(NPC.defense * NPC.scale);
			NPC.damage = NPC.defDamage = (int)(NPC.damage * NPC.scale);
			NPC.life = NPC.lifeMax = (int)(NPC.life * NPC.scale);
			NPC.value = (int)(NPC.value * NPC.scale);
			NPC.npcSlots *= NPC.scale;
			NPC.knockBackResist *= 2f - NPC.scale;
			NPC.DeathSound = Sounds.NPCs.Mochtroid.WithVolumeScale(NPC.scale * 0.75f).WithPitchOffset(1f - NPC.scale);
		}
		public override void AI()
		{
			if (NPC.scale == 1)
			{
				SetStats();
				STATE = (int)StateID.Idle;
				NPC.netUpdate = true;
			}
			if (NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze)
			{
				STATE = (int)StateID.Frozen;
				NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().speedDecrease = 0;
			}
			if (STATE == (int)StateID.Frozen)
			{
				NPC.noGravity = false;
				if (NPC.velocity.Y == 0)
				{
					NPC.velocity.X *= 0.96f;
				}
				if (!NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze)
				{
					STATE = (int)StateID.Idle;
				}
			}
			else
			{
				NPC.noGravity = true;
				NPCUtils.TargetSearchResults results = NPCUtils.SearchForTarget(NPC, NPCUtils.TargetSearchFlag.Players,
					(Player p) => p.Distance(NPC.Center) < 600 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, p.position, p.width, p.height), null);
				if (results.FoundTarget)
				{
					NPC.targetRect = results.NearestTargetHitbox;
					STATE = (int)StateID.Aggroed;
				}
				else
				{
					if (AI_Counter > 120)
					{
						STATE = (int)StateID.Idle;
					}
				}
				AI_Counter++;

				if (STATE == (int)StateID.Idle)
				{
					if (AI_Counter > 130)
					{
						AI_Counter = 0;
						NPC.targetRect = new Rectangle((int)NPC.Center.X + (150 * NPC.direction), (int)NPC.Center.Y - (40 * NPC.directionY), 1, 1);
					}
					else
					{
						if (NPC.collideX)
						{
							NPC.direction *= -1;
							NPC.velocity.X = NPC.direction;
							AI_Counter = 120;
						}
						if (NPC.collideY)
						{
							NPC.directionY *= -1;
							NPC.velocity.Y = NPC.directionY;
							NPC.targetRect = new Rectangle((int)NPC.Center.X + (150 * NPC.direction), (int)NPC.Center.Y + (40 * NPC.directionY), 1, 1);
						}
						NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
						NPC.directionY = NPC.velocity.Y > 0 ? 1 : -1;
					}
				}

				Vector2 targetPos = new Vector2(NPC.targetRect.Center.X, NPC.targetRect.Y);
				if (Collision.SolidCollision(NPC.targetRect.TopLeft() + new Vector2(0, -20), NPC.targetRect.Width, 20))
				{
					targetPos = NPC.targetRect.Center();
				}
				//Dust.NewDustPerfect(targetPos, DustID.BlueFairy, Vector2.Zero);

				Vector2 targetVelocity = Vector2.Normalize(targetPos - NPC.Center) * speed;

				Rectangle collisionRect = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
				bool sucking = false;
				// Proximity damage.
				foreach (Player p in Main.ActivePlayers)
				{
					if (p.active && !p.dead && !p.immune && p.getRect().Intersects(collisionRect))
					{
						p.AddBuff(ModContent.BuffType<MochtroidSucc>(), 2);
						sucking = true;
					}
				}
				if (sucking)
				{
					if (NPC.life < NPC.lifeMax && AI_Counter % 2 == 0)
					{
						NPC.life++;
					}
					if (NPC.soundDelay <= 0)
					{
						NPC.soundDelay = 75;
						SoundEngine.PlaySound(Sounds.NPCs.Mochtroid.WithPitchOffset((Main.rand.NextFloat() * 0.25f) + (1f - NPC.scale)).WithVolumeScale(NPC.scale * 0.5f), NPC.Center);

					}
				}
				float acc = sucking ? acceleration * 2 : acceleration;
				float dec = sucking ? 0.95f : 0.98f;
				if (NPC.velocity.X < targetVelocity.X)
				{
					NPC.velocity.X += acc;
					if (NPC.velocity.X < 0)
						NPC.velocity.X *= dec;
				}
				else if (NPC.velocity.X > targetVelocity.X)
				{
					NPC.velocity.X -= acc;
					if (NPC.velocity.X > 0)
						NPC.velocity.X *= dec;
				}

				if (NPC.velocity.Y < targetVelocity.Y)
				{
					NPC.velocity.Y += acc;
					if (NPC.velocity.Y < 0)
						NPC.velocity.Y *= dec;
				}
				else if (NPC.velocity.Y > targetVelocity.Y)
				{
					NPC.velocity.Y -= acc;
					if (NPC.velocity.Y > 0)
						NPC.velocity.Y *= dec;
				}

				if (NPC.collideX)
				{
					NPC.netUpdate = true;
					NPC.velocity.X = NPC.oldVelocity.X * -.7F;
				}
				if (NPC.collideY)
				{
					NPC.netUpdate = true;
					NPC.velocity.Y = NPC.oldVelocity.Y * -.7F;
				}

				if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) || (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) && !NPC.justHit)
					NPC.netUpdate = true;

			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (STATE != (int)StateID.Frozen)
			{
				NPC.frameCounter++;
				NPC.rotation = NPC.velocity.X * 0.05f;
			}
			else
			{
				if (NPC.velocity.Y == 0)
				{
					NPC.rotation += NPC.velocity.X * 0.05f;
				}
				else
				{
					NPC.rotation += NPC.velocity.X * 0.03f;
				}
			}
			if (NPC.frameCounter >= 6)
			{
				NPC.frame.Y = NPC.frame.Y + (int)(frameHeight * NPC.localAI[0]);

				if (NPC.frame.Y == 2 * frameHeight)
					NPC.localAI[0] = -1;
				else if (NPC.frame.Y == 0)
					NPC.localAI[0] = 1;

				NPC.frameCounter = 0;
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			Texture2D tex = TextureAssets.Npc[NPC.type].Value;
			int frameHeight = tex.Height / Main.npcFrameCount[NPC.type];
			Vector2 origin = new Vector2(tex.Width * 0.5f, frameHeight * 0.5f);
			Rectangle rect = new Rectangle(NPC.frame.X, NPC.frame.Y, tex.Width, tex.Height / Main.npcFrameCount[NPC.type]);

			if (STATE == (int)StateID.Frozen)
			{
				drawColor = Lighting.GetColor(NPC.Center.ToTileCoordinates());
			}
			DrawData data = new DrawData(tex, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - (tex.Width * 0.5f) + origin.X, NPC.position.Y - Main.screenPosition.Y + NPC.height + ((origin.Y - frameHeight + 8) * NPC.scale)), new Rectangle?(rect), drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
			if (STATE == (int)StateID.Frozen)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

				MiscShaderData shaderData = GameShaders.Misc["MetroidModDualTint"];
				shaderData.UseColor(new Color(0f, 0.286f, 1f));
				shaderData.UseSecondaryColor(new Color(0f, 0.286f, 1f));
				shaderData.UseOpacity(0.2f);
				shaderData.UseSaturation(1f);
				shaderData.UseImage0(TextureAssets.Npc[NPC.type]);

				shaderData.Apply(data);
			}
			data.Draw(spriteBatch);
			if (STATE == (int)StateID.Frozen)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
			}
			return false;
		}
	}
}
