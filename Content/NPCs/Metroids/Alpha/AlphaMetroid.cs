using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Projectiles.Metroids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
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
			NPC.knockBackResist = 0.25f;
			NPC.aiStyle = -1;
			NPC.npcSlots = 2;
		}
		int effectiveDefense = 40;
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			// Charging damage mult
			if (AI_Substate == (int)Aggro_Substate.Charge && AI_Counter > 60 && AI_Counter <= 84)
			{
				modifiers.SourceDamage *= 2;
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			// Charging damage mult
			if (AI_Substate == (int)Aggro_Substate.Charge && AI_Counter > 60 && AI_Counter <= 84)
			{
				modifiers.SourceDamage *= 2;
			}
		}
		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (STATE == (int)StateID.Coccoon && AI_Counter < 100)
			{
				AI_Counter = 100;
			}

			if (STATE == (int)StateID.Aggroed && AI_Substate == (int)Aggro_Substate.Charge) //Melee Counter during the charge
			{
				if (AI_Counter > 60 && AI_Counter < 84)
				{
					AI_Substate = (int)Aggro_Substate.Stun;
					AI_Counter = 90; //Stun Duration
				}
			}
		}
		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (STATE == (int)StateID.Coccoon && AI_Counter < 100)
			{
				AI_Counter = 100;
			}
			if (STATE == (int)StateID.Aggroed && AI_Substate == (int)Aggro_Substate.Charge) //Melee Counter during the charge
			{
				if (Main.player[projectile.owner].heldProj == projectile.whoAmI && AI_Counter > 60 && AI_Counter < 84)
				{
					AI_Substate = (int)Aggro_Substate.Stun;
					AI_Counter = 90; //Stun Duration
				}
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
			if (STATE == (int)StateID.Aggroed && AI_Substate == (int)Aggro_Substate.Charge)
			{
				if (AI_Counter > 60 && AI_Counter <= 85)
				{
					modifiers.SetCrit();
					modifiers.ArmorPenetration += (400 - effectiveDefense); 
					modifiers.Knockback += 5;
					SoundEngine.PlaySound(SoundID.NPCHit18, NPC.Center);
					return;
				}
				else
				{
					modifiers.DisableKnockback();
				}
			}
			if (STATE == (int)StateID.Coccoon)
			{
				modifiers.DisableKnockback();
				SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
			}
			else
			{
				if (player.Distance(WeakpointHurtbox().Center()) < player.Distance(NPC.Center))
				{
					modifiers.ArmorPenetration += (400 - effectiveDefense);
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
			if (STATE == (int)StateID.Aggroed && AI_Substate == (int)Aggro_Substate.Charge)
			{
				if (Main.player[projectile.owner].heldProj == projectile.whoAmI && AI_Counter > 60 && AI_Counter <= 85)
				{
					modifiers.SetCrit();
					modifiers.ArmorPenetration += (400 - effectiveDefense);
					SoundEngine.PlaySound(SoundID.NPCHit18, NPC.Center);
					return;
				}
				else
				{
					modifiers.DisableKnockback();
				}
			}
			if (STATE == (int)StateID.Coccoon)
			{
				modifiers.DisableKnockback();
				SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
			}
			else
			{
				if (projectile.Colliding(projectile.Hitbox, WeakpointHurtbox()))
				{
					modifiers.ArmorPenetration += (400 - effectiveDefense);
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
		public ref float AI_Substate => ref NPC.ai[2];
		public ref float Wiggle => ref NPC.localAI[0];
		private enum StateID : int
		{
			Coccoon,
			Idle,
			Aggroed,
			JustHatched
		}
		private enum Aggro_Substate : int
		{
			Neutral,
			Charge,
			Sparks,
			Stun
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
					Wiggle--; //Wiggle
				}
				if (AI_Counter >= 300) //5 Seconds
				{
					AI_Counter = 0;
					STATE = (int)StateID.JustHatched;
					NPC.noGravity = true;
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
			if (STATE == (int)StateID.JustHatched)
			{
				NPC.noGravity = true;
				AI_Counter++;
				NPC.velocity *= 0.95f;
				if (AI_Counter >= 60)
				{
					AI_Counter = 0;
					STATE = (int)StateID.Aggroed;
				}
			}
			if (STATE == (int)StateID.Aggroed)
			{
				NPC.noGravity = true;
				//NPC.velocity *= 0.95f;
				//NPC.rotation += MathHelper.ToRadians(0.5f);
				Vector2 targetVelocity = Vector2.Zero;
				if (!NPC.HasValidTarget)
				{
					STATE = (int)StateID.Idle;
					NPC.netUpdate = true;
				}
				else
				{
					Player p = Main.player[NPC.target];
					NPC.targetRect = p.Hitbox;
					targetVelocity = p.velocity;
				}

				if (AI_Substate == (int)Aggro_Substate.Neutral)
				{
					AI_Counter++;

					Vector2 accel = new Vector2(0.2f, 0.15f);
					Vector2 speed = new Vector2(7.5f, 5f);

					//Try to move above the player
					Vector2 targetPos = new Vector2(NPC.targetRect.Center.X, NPC.targetRect.Center.Y - 120) + targetVelocity * 20;

					//Move towards the side closest to the Metroid
					if (NPC.Center.X > NPC.targetRect.Center.X)
					{
						targetPos.X += 160;
					}
					else
					{
						targetPos.X -= 160;
					}
					NPC.direction = Math.Sign(NPC.DirectionTo(targetPos).X);
					NPC.directionY = Math.Sign(NPC.DirectionTo(targetPos).Y);

					//Speed up when about to charge
					if (AI_Counter > 150)
					{
						speed *= Math.Min(1.5f, AI_Counter / 150);
						accel *= Math.Min(1.5f, AI_Counter / 150);
					}

					//Charge attack when in position
					if (NPC.Distance(targetPos) < 60)
					{
						AI_Counter = 0;
						AI_Substate = (int)Aggro_Substate.Charge;
					}
					else 
					{
						//Main.NewText(Math.Abs(NPC.Distance(targetPos)));
						if (NPC.Distance(targetPos) < 80)
						{
							speed.Y *= NPC.Distance(targetPos) / 60;
						}
					}

					if (NPC.velocity.X * NPC.direction < speed.X)
					{
						NPC.velocity.X += NPC.direction * accel.X;
					}
					else
					{
						NPC.velocity.X = speed.X * NPC.direction;
					}

					//Only flip sprite if moving in that direction
					if (NPC.direction * Math.Sign(NPC.velocity.X) > 0)
					{
						NPC.spriteDirection = NPC.direction;
					}

					if (NPC.velocity.Y * NPC.directionY < speed.Y)
					{
						NPC.velocity.Y += NPC.directionY * accel.Y;
					}
					else
					{
						NPC.velocity.Y = speed.Y * NPC.directionY;
					}

					if (AI_Counter >= 350)
					{
						AI_Substate = (int)Aggro_Substate.Sparks;
						AI_Counter = 0;
					}
				}

				if (AI_Substate == (int)Aggro_Substate.Charge)
				{
					AI_Counter++;
					if (AI_Counter == 1)
					{
						NPC.FaceTarget();
						NPC.spriteDirection = NPC.direction;
					}
					float speed = 16.5f;
					//Set direction in normal mode
					float rot = NPC.direction < 0 ? MathHelper.ToRadians(110) : MathHelper.ToRadians(70);
					Vector2 targetPos = new Vector2(NPC.targetRect.Center.X, NPC.targetRect.Center.Y);
					if (Main.expertMode) //Tracking in expert mode
					{
						Vector2 trajectory = NPC.DirectionTo(targetPos);
						rot = trajectory.ToRotation() + MathHelper.ToRadians(35f) * NPC.direction;
					}
					if (AI_Counter < 60)
					{
						if (AI_Counter < 25) //Reel back
						{
							NPC.velocity.X = -1.25f * NPC.direction;
							NPC.velocity.Y = -1.25f;
							//NPC.rotation += MathHelper.ToRadians(3f) * NPC.direction;
							NPC.rotation = (NPC.direction < 0 ? rot - MathHelper.Pi : rot) * (AI_Counter / 25);
						}
						else //Dramatic wiggle
						{
							NPC.rotation = (NPC.direction < 0 ? rot - MathHelper.Pi : rot);
							NPC.velocity *= 0.92f;
							if (Wiggle > 0)
							{
								Wiggle--;
							}
							if (AI_Counter == 40)
							{
								Wiggle = 16;
							}
						}
						if (AI_Counter == 45) //Spark
						{
							SoundEngine.PlaySound(SoundID.MaxMana.WithPitchOffset(-0.15f));
							Dust.NewDustDirect(NPC.Center, 0, 0, DustID.TreasureSparkle, 0, 0, 0, default, 3f).noGravity = true; ;
						}
					}
					if (AI_Counter == 60) //The charge itself
					{
						//NPC.velocity.Y = 16;
						//NPC.velocity.X = 4 * NPC.direction;
						//NPC.velocity = trajectory.RotatedBy(MathHelper.ToRadians(35f) * NPC.direction);

						NPC.velocity = rot.ToRotationVector2() * speed;
						NPC.rotation = NPC.velocity.ToRotation() + (NPC.direction < 0 ? MathHelper.Pi : 0);
					}

					bool passedTarget = NPC.DirectionTo(targetPos).X * NPC.velocity.X < 0;
					if (AI_Counter > 60 && AI_Counter <= 84) //Steer up when passed the target
					{
						//NPC.velocity = NPC.velocity.RotatedBy(MathHelper.ToRadians(-4.5f) * NPC.direction);
						Vector2 projection = NPC.Center + (NPC.Distance(targetPos) / speed) * NPC.velocity;
						if (projection.Y > targetPos.Y || passedTarget || (NPC.Center.Y < targetPos.Y && NPC.velocity.Y < 0))
						{
							NPC.rotation += MathHelper.ToRadians(-4.5f) * NPC.direction;
						}
						NPC.velocity = NPC.rotation.ToRotationVector2() * NPC.direction * speed;
					}
					if (AI_Counter >= 85) //Decelerate
					{
						if (passedTarget || AI_Counter >= 100)
						{
							NPC.velocity *= 0.9f;
						}
					}
					if (AI_Counter >= 105)
					{
						NPC.rotation = MathHelper.WrapAngle(NPC.rotation) * 0.9f;
					}
					if (AI_Counter >= 125)
					{
						if (Main.rand.NextBool(2))
						{
							AI_Substate = (int)Aggro_Substate.Sparks;
							AI_Counter = 0;
						}
						else
						{
							AI_Substate = (int)Aggro_Substate.Neutral;
							AI_Counter = Main.rand.NextBool(2) ? 0 : 150; //Either reset to neutral or charge again
						}
						NPC.netUpdate = true;
						
						NPC.rotation = 0f;
					}
				}

				if (AI_Substate == (int)Aggro_Substate.Sparks)
				{
					AI_Counter++;
					//Try to move above the player
					Vector2 targetPos = new Vector2(NPC.targetRect.Center.X, NPC.targetRect.Center.Y - 120) + targetVelocity * 20;
					bool playerAbove = NPC.targetRect.Center.Y < NPC.Center.Y;

					Vector2 accel = new Vector2(0.2f, playerAbove ? 0.3f : 0.15f);
					Vector2 speed = new Vector2(5.5f, playerAbove ? 5.5f : targetPos.Y < NPC.Center.Y ? 3f : 1.5f);

					//Turn around when hitting an obstacle
					if (NPC.velocity.X == 0) 
					{
						NPC.direction *= -1;
					}

					//Turn around if more than 20 tiles from player
					if (NPC.Center.X > NPC.targetRect.Center.X + 320 || NPC.Center.X < NPC.targetRect.Center.X - 320)
					{
						NPC.direction = Math.Sign(NPC.DirectionTo(targetPos).X);
					}
					NPC.directionY = Math.Sign(NPC.DirectionTo(targetPos).Y);

					if (NPC.velocity.X * NPC.direction < speed.X)
					{
						NPC.velocity.X += NPC.direction * accel.X;
					}
					else
					{
						NPC.velocity.X = speed.X * NPC.direction;
					}

					//Only flip sprite if moving in that direction
					if (NPC.direction * Math.Sign(NPC.velocity.X) > 0)
					{
						NPC.spriteDirection = NPC.direction;
					}

					if (NPC.velocity.Y * NPC.directionY < speed.Y)
					{
						NPC.velocity.Y += NPC.directionY * accel.Y;
					}
					else
					{
						NPC.velocity.Y = speed.Y * NPC.directionY;
					}
					if (AI_Counter % 3 == 0)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0, 0, 20, default, 0.75f);
					}
					if (AI_Counter == 40 || //Single
						AI_Counter == 80 || AI_Counter == 90 || //Double
						(AI_Counter >= 130 && AI_Counter % 10 == 0 && (AI_Counter + 10) % 70 < 30)) //Triple
					{
						Vector2 ballVel = Vector2.Zero;
						if (playerAbove)
						{
							ballVel = new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-15, -9));
						}
						int damage = 30; //30 journey, 60 normal, 120 expert, 180 master
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, ballVel, ModContent.ProjectileType<ElectricBall>(), damage, 3f);
					}
					if (AI_Counter > 300)
					{
						AI_Counter = 0;
						AI_Substate = (int)Aggro_Substate.Neutral;
					}
				}

				if (AI_Substate == (int)Aggro_Substate.Stun) //Stun from being melee attacked during the charge
				{
					AI_Counter--;
					if (AI_Counter > 45)
					{
						float rotSpeed = 2 + (AI_Counter / 6);
						NPC.rotation += MathHelper.ToRadians(-rotSpeed) * NPC.direction;
						NPC.velocity *= 0.96f;
					}
					else
					{
						NPC.velocity *= 0.9f;
						NPC.rotation = MathHelper.WrapAngle(NPC.rotation) * 0.92f;
					}
					if (AI_Counter <= 0)
					{
						AI_Substate = (int)Aggro_Substate.Sparks;
						NPC.rotation = 0;
					}
				}

			}
		}
		public override void FindFrame(int frameHeight)
		{
			if (STATE == (int)StateID.Aggroed)
			{
				if (AI_Substate == (int)Aggro_Substate.Charge)
				{
					if (AI_Counter > 10 && AI_Counter < 60)
					{
						if (NPC.frame.Y < 2 * frameHeight)
						{
							NPC.frameCounter++;
							if (NPC.frameCounter > 6)
							{
								NPC.frameCounter = 0;
								NPC.frame.Y += frameHeight;
							}
						}
					}
					if (AI_Counter > 85)
					{
						if (NPC.frame.Y > 0)
						{
							NPC.frameCounter++;
							if (NPC.frameCounter > 6)
							{
								NPC.frameCounter = 0;
								NPC.frame.Y -= frameHeight;
							}
						}
					}
				}
				if (AI_Substate == (int)Aggro_Substate.Stun)
				{
					if (NPC.frame.Y > 0)
					{
						NPC.frameCounter++;
						if (NPC.frameCounter > 6)
						{
							NPC.frameCounter = 0;
							NPC.frame.Y -= frameHeight;
						}
					}
				}
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
			/*else
			{
				Texture2D texDebug = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_DebugHurtbox");
				Rectangle debugRect = Rectangle.Intersect(WeakpointHurtbox(), NPC.Hitbox);

				Vector2 debugPos = new Vector2(debugRect.X - Main.screenPosition.X, debugRect.Y - Main.screenPosition.Y);

				DrawData debugData = new DrawData(texDebug, debugPos, new Rectangle?(debugRect), drawColor * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None);
				debugData.Draw(spriteBatch);
			}*/

			return false;
		}
	}
}
