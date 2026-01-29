using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static MetroidMod.Sounds;

namespace MetroidMod.Content.NPCs.Metroids.Alpha
{
	public class AlphaMetroid : MNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 3;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.UsesNewTargetting[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 44;
			NPC.damage = 40;
			NPC.defense = 400;
			NPC.lifeMax = 640;
			NPC.HitSound = null;
			NPC.DeathSound = Sounds.NPCs.Metroid;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 1, 99, 1);
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			NPC.npcSlots = 2;
		}
		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (STATE == (int)StateID.Coccoon && AI_Counter < 100)
			{
				AI_Counter = 100;
			}
		}
		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (STATE == (int)StateID.Coccoon && AI_Counter < 100)
			{
				AI_Counter = 100;
			}
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			if (STATE == (int)StateID.Coccoon)
			{
				return false;
			}
			return base.CanHitPlayer(target, ref cooldownSlot);
		}
		public override bool CanHitNPC(NPC target)
		{
			if (STATE == (int)StateID.Coccoon)
			{
				return false;
			}
			return base.CanHitNPC(target);
		}
		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (STATE == (int)StateID.Coccoon)
			{
				modifiers.DisableKnockback();
				SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
			}
			else
			{
				if (player.Distance(WeakpointHurtbox().Center()) < player.Distance(NPC.Center))
				{
					modifiers.ArmorPenetration += 370;
					SoundEngine.PlaySound(SoundID.NPCHit1, NPC.Center);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Tink, NPC.Center);
					modifiers.Knockback *= 0.5f;
				}
			}
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (STATE == (int)StateID.Coccoon)
			{
				modifiers.DisableKnockback();
				SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
			}
			else
			{
				if (projectile.Colliding(projectile.Hitbox, WeakpointHurtbox()))
				{
					modifiers.ArmorPenetration += 370;
					SoundEngine.PlaySound(SoundID.NPCHit1, NPC.Center);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Tink, NPC.Center);
					modifiers.DisableKnockback();
				}
			}
		}
		private Rectangle WeakpointHurtbox()
		{
			Rectangle hurtbox = new Rectangle(0, 0, 44, 44);
			Vector2 centerPoint = NPC.Center + new Vector2(-1 * NPC.direction, 22).RotatedBy(NPC.rotation);
			hurtbox.X = (int)(centerPoint.X - hurtbox.Width / 2);
			hurtbox.Y = (int)(centerPoint.Y - hurtbox.Height / 2);
			return hurtbox;
		}
		public ref float STATE => ref NPC.ai[0];
		public ref float AI_Counter => ref NPC.ai[1];
		public ref float Wiggle => ref NPC.localAI[0];
		private enum StateID : int
		{
			Coccoon,
			Idle,
			Aggroed
		}
		public override void AI()
		{
			if (STATE == (int)StateID.Coccoon)
			{
				NPC.noGravity = false;
				NPC.velocity.X = 0;

				NPC.TargetClosest(false);
				Player p = Main.player[NPC.target];
				NPC.targetRect = p.Hitbox;
				if (AI_Counter > 0)
				{
					AI_Counter++;
				}
				else if (NPC.HasValidTarget && p.Distance(NPC.Center) < 200 && Collision.CanHitLine(p.Center, 1, 1, NPC.Center, 1, 1))
				{
					AI_Counter++;
				}
				if (AI_Counter == 150 || AI_Counter == 220)
				{
					for (int i = 0; i < 20; i++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silver, 0, -2, 0, new Color(168, 176, 128));
					}
					Wiggle = 16;
					if (AI_Counter == 50)
					{
						SoundEngine.PlaySound(SoundID.Item49.WithPitchOffset(-0.3f), NPC.Center);
					}
					else
					{
						SoundEngine.PlaySound(SoundID.Item48.WithPitchOffset(-0.25f), NPC.Center);
					}
				}
				if (Wiggle > 0)
				{
					Wiggle--;
				}
				if (AI_Counter >= 300)
				{
					AI_Counter = 0;
					STATE = (int)StateID.Aggroed;
					NPC.velocity.Y = -6;
					int index = Item.NewItem(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-4, 0), Vector2.Zero, ModContent.ItemType<HuskLarva>());
					Main.item[index].velocity = new Vector2(0, -1);
					SoundEngine.PlaySound(SoundID.Item51.WithPitchOffset(-0.5f), NPC.Center);
					for (int i = 0; i < 40; i++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silver, 0, -4 - Main.rand.Next(3), 0, new Color(168, 176, 128), 1f + Main.rand.NextFloat() * 0.5f);
					}
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-4, -3), NPC.velocity, Mod.Find<ModGore>("AlphaCoccoonShard1").Type, NPC.scale);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(4, -3), NPC.velocity, Mod.Find<ModGore>("AlphaCoccoonShard2").Type, NPC.scale);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(3, -4), NPC.velocity, Mod.Find<ModGore>("AlphaCoccoonShard3").Type, NPC.scale);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(3, -4), NPC.velocity, Mod.Find<ModGore>("AlphaCoccoonShard4").Type, NPC.scale);
				}
			}
			if (STATE == (int)StateID.Aggroed)
			{
				NPC.noGravity = true;
				NPC.velocity *= 0.95f;
				NPC.rotation += MathHelper.ToRadians(0.5f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection > 0)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Texture2D texBody = (Texture2D)ModContent.Request<Texture2D>($"{Texture}");
			Texture2D texCoccoon = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_Coccoon");

			int frameHeightBody = texBody.Height / 3;
			int frameHeightCoccoon = texCoccoon.Height / 3;

			int hatchFrame = AI_Counter >= 220 ? 1 : 0;

			Rectangle rectBody = new Rectangle(NPC.frame.X, NPC.frame.Y, texBody.Width, texBody.Height / 3);
			Rectangle rectCoccoon = new Rectangle(0, hatchFrame * frameHeightCoccoon, texCoccoon.Width, texCoccoon.Height / 3);

			Vector2 originBody = new Vector2(texBody.Width * 0.5f, frameHeightBody * 0.5f);
			Vector2 originCoccoon = new Vector2(rectCoccoon.Width * 0.5f, frameHeightCoccoon * 0.5f);

			Vector2 totalOffset = new Vector2(0, 0);
			Vector2 coccoonOffset = new Vector2(4 * NPC.spriteDirection, 4).RotatedBy(NPC.rotation);
			if (Wiggle > 0)
			{
				float range = 4;
				float w = (16 + range / 2) - Wiggle;
				totalOffset.X = -(range / 2) + Math.Abs((w + range) % (range * 2) - range);
			}

			Vector2 bodyPos = new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texBody.Width * 0.5f + originBody.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightBody + originBody.Y) + totalOffset;
			Vector2 coccoonPos = new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texCoccoon.Width * 0.5f + originCoccoon.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightCoccoon + originCoccoon.Y) + totalOffset + coccoonOffset;

			DrawData bodyData = new DrawData(texBody, bodyPos, new Rectangle?(rectBody), drawColor, NPC.rotation, originBody, NPC.scale, effects);
			DrawData coccoonData = new DrawData(texCoccoon, coccoonPos, new Rectangle?(rectCoccoon), drawColor, NPC.rotation, originCoccoon, NPC.scale, effects);

			bodyData.Draw(spriteBatch);
			if (STATE == (int)StateID.Coccoon)
			{
				coccoonData.Draw(spriteBatch);
			}
			else
			{
				Texture2D texDebug = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_DebugHurtbox");
				Rectangle debugRect = Rectangle.Intersect(WeakpointHurtbox(), NPC.Hitbox);

				Vector2 debugPos = new Vector2(debugRect.X - Main.screenPosition.X, debugRect.Y - Main.screenPosition.Y);

				DrawData debugData = new DrawData(texDebug, debugPos, new Rectangle?(debugRect), drawColor * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None);
				debugData.Draw(spriteBatch);
			}

			return false;
		}
	}
}
