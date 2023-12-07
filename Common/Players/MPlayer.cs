using System;
using System.Linq;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Capture;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

using MetroidMod.Common.GlobalNPCs;
//using MetroidMod.Content.NPCs;
//using MetroidMod.Content.Items;
using MetroidMod.Common.Systems;
using MetroidMod.ID;
using MetroidMod.Content.Biomes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetroidMod.Common.Players
{
	public partial class MPlayer
	{
		public bool ZoneChozoRuins => ModContent.GetInstance<ChozoRuinsBiome>().IsBiomeActive(Player);

		public float statCharge = 0.0f;
		public static float maxCharge = 100.0f;

		public float maxOverheat = 100f;
		public float statOverheat = 0f;
		public float overheatCost = 1f;
		public int overheatDelay = 0;
		private float overheatCooldown = 0f;
		public float missileCost = 1f;
		public float maxParalyzerCharge = 100f;
		public float statParalyzerCharge = 0f;

		public bool senseMove = false;
		public bool senseMoveEnabled = true;
		public int SMoveEffect = 0;
		private bool senseSound = false;
		private int senseMoveCooldown = 0;
		private int dashTime = 0;

		public float grappleRotation = 0f;
		public float maxDist;
		public int grapplingBeam = -1;

		public bool phazonImmune = false;
		public bool canUsePhazonBeam = false;
		public int hazardShield = 0;
		public int phazonRegen = 0;

		public double Time = 0;
		public float breathMult = 1f;
		public Vector2 oldPosition;

		public bool falling;
		public int energyLowTimer = 0;

		public override void ResetEffects()
		{
			ResetEffects_Accessories();
			ResetEffects_SuitEnergy();
			ResetEffects_GetArmors();
			ResetEffects_MorphBall();
			ResetEffects_Graphics();


			maxOverheat = 100f;
			overheatCost = 1f;
			missileCost = 1f;
			maxParalyzerCharge = 100f;

			senseMove = false;

			phazonImmune = false;
			canUsePhazonBeam = false;
			hazardShield = 0;
			phazonRegen = 0;

			breathMult = 1f;

			HUDColor = Color.LightBlue;

			switch (Player.name.ToLower())
			{
				case "ed the terrarian":
				case "ed": // challenge name, tributary to a beta tester who broke my sanity for around 2 hours - DarkSamus49
					Player.statLifeMax2 /= 5;
					Player.statManaMax2 /= 4;
					Player.velocity.X /= 4f;
					Player.velocity.Y += Player.controlJump | Player.velocity.Y <= 0 ? 0 : 16f;
					Player.maxFallSpeed = 10000f;
					break;
				default:
					break;
			}
		}
		public override void PreUpdate()
		{
			PreUpdate_Accessories();
			PreUpdate_MorphBall();
			PreUpdate_Graphics();

			Player P = Player;

			if (statCharge >= maxCharge)
			{
				statCharge = maxCharge;
			}
			if (statParalyzerCharge >= maxParalyzerCharge)
			{
				statParalyzerCharge = maxParalyzerCharge;
			}
			if (overheatDelay > 0)
			{
				overheatDelay--;
			}
			if (statOverheat > 0)
			{
				if (shineDirection <= 0 && !shineActive && overheatDelay <= 0)
				{
					statOverheat -= overheatCooldown;
					if (statCharge <= 0)
					{
						overheatCooldown += 0.025f;
					}
					else if (overheatCooldown < 0.25f)
					{
						overheatCooldown += 0.0025f;
					}
				}
				else
				{
					overheatCooldown = 0f;
				}
			}
			else
			{
				overheatCooldown = 0f;
				statOverheat = 0f;
			}

			Time += 1.0;
			if (Time > 54000.0)
			{
				Time = 0;
			}
			P.breathMax = (int)(200 * breathMult);
			oldPosition = P.position;

			if (visorGlow && !ballstate)
			{
				Lighting.AddLight((int)((float)Player.Center.X / 16f), (int)((float)(Player.position.Y + 8f) / 16f), ((float)visorGlowColor.R / 255) * 0.375f, ((float)visorGlowColor.G / 255) * 0.375f, ((float)visorGlowColor.B / 255) * 0.375f);
			}
			if (jet)
			{
				Lighting.AddLight((int)((float)Player.Center.X / 16f), (int)((float)Player.Center.Y / 16f), 0.6f, 0.38f, 0.24f);
			}

			if (!phazonImmune)
			{
				if (TouchTiles(Player.position, Player.width, Player.height, ModContent.TileType<Content.Tiles.PhazonTile>()))
				{
					Player.AddBuff(ModContent.BuffType<Content.Buffs.PhazonDebuff>(), 2);
				}
			}
			else
			{
				if (TouchTiles(Player.position, Player.width, Player.height, ModContent.TileType<Content.Tiles.PhazonTile>()) && phazonRegen > 0)
				{
					Player.lifeRegen += phazonRegen;
				}
			}
			if (TouchTiles(Player.position, Player.width, Player.height, ModContent.TileType<Content.Tiles.PhazonCore>()))
			{
				Player.AddBuff(ModContent.BuffType<Content.Buffs.PhazonDebuff>(), 2);
			}

			if (hazardShield > 0)
			{
				List<int> debuffList = new() { 20, 21, 22, 23, 24, 30, 31, 32, 33, 35, 36, 46, 47, 69, 70, 72, 80, 88, 94, 103, 120, 137, 144, 145, 148, 149, 153, 156, 164, 169, 195, 196, 197 };

				for (int k = 0; k < P.buffType.Length; k++)
				{
					int buff = P.buffType[k];
					if (debuffList.Contains(buff))
					{
						P.buffTime[k] = Math.Max(P.buffTime[k] - hazardShield, 0);
					}
				}
			}
			int x1 = (int)MathHelper.Clamp((Player.position.X + Player.velocity.X) / 16, 0, Main.maxTilesX - 1);
			int x2 = (int)MathHelper.Clamp((Player.position.X + Player.velocity.X + Player.width - 1) / 16, 0, Main.maxTilesX - 1);
			int j = (int)MathHelper.Clamp((Player.position.Y + Player.height + 1) / 16, 0, Main.maxTilesY - 1);
			for (int i = x1; i <= x2; i++)
			{
				if (Main.tile[i, j].HasTile && !Main.tile[i, j].IsActuated)
				{
					if (MSystem.mBlockType[i, j] == BreakableTileID.CrumbleInstant) //CrumbleInstant
					{
						MSystem.AddRegenBlock(i, j, true);
						// Enforce SpeedBooster
						if (falling)
						{
							Player.velocity.X = 0;
							Player.oldVelocity.X = 0;
						}
					}
					if (MSystem.mBlockType[i, j] == BreakableTileID.CrumbleSpeed) //CrumbleSpeed
					{
						MSystem.nextTick.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 1, new Vector2(i, j)));
					}
					if (MSystem.mBlockType[i, j] == BreakableTileID.CrumbleSlow) //CrumbleSlow
					{
						MSystem.hit[i, j] = true;
						MSystem.timers.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 60, new Vector2(i, j)));
					}
				}
			}
			#region speedBoost & screwAttack
			int blockCheckWidth = 32;
			int blockCheckHeight = 48;
			x1 = (int)MathHelper.Clamp((Player.Center.X - blockCheckWidth / 2 + Math.Min(Player.velocity.X, 0)) / 16, 0, Main.maxTilesX - 1);
			x2 = (int)MathHelper.Clamp((Player.Center.X + blockCheckWidth / 2 + Math.Max(Player.velocity.X, 0)) / 16, 0, Main.maxTilesX - 1);
			int y1 = (int)MathHelper.Clamp((Player.Center.Y - blockCheckHeight / 2 + Math.Min(Player.velocity.Y, 0)) / 16, 0, Main.maxTilesY - 1);
			int y2 = (int)MathHelper.Clamp((Player.Center.Y + blockCheckHeight / 2 + Math.Max(Player.velocity.Y, 0)) / 16, 0, Main.maxTilesY - 1);
			for (int i = x1; i <= x2; i++)
			{
				for (int k = y1; k <= y2; k++)
				{
					MPlayer mp = Player.GetModPlayer<MPlayer>();
					if (mp.speedBoosting || mp.shineActive)
					{
						if (Main.tile[i, k].HasTile && !Main.tile[i, k].IsActuated)
						{
							if (MSystem.mBlockType[i, k] == BreakableTileID.Bomb) //BombBlock
							{
								MSystem.AddRegenBlock(i, k);
							}
							if (MSystem.mBlockType[i, k] == BreakableTileID.Fake) //FakeBlock
							{
								MSystem.AddRegenBlock(i, k);
							}
							if (MSystem.mBlockType[i, k] == BreakableTileID.Boost) //BoostBlock
							{
								MSystem.AddRegenBlock(i, k);
							}
							if (MSystem.mBlockType[i, k] == BreakableTileID.FakeHint) //FakeBlockHint
							{
								MSystem.AddRegenBlock(i, k);
							}
						}
					}
					if (mp.somersault && mp.screwAttack)
					{
						if (Main.tile[i, k].HasTile && !Main.tile[i, k].IsActuated)
						{
							if (MSystem.mBlockType[i, k] == BreakableTileID.Bomb) //BombBlock
							{
								MSystem.AddRegenBlock(i, k);
							}
							if (MSystem.mBlockType[i, k] == BreakableTileID.Fake) //FakeBlock
							{
								MSystem.AddRegenBlock(i, k);
							}
							if (MSystem.mBlockType[i, k] == BreakableTileID.ScrewAttack) //ScrewAttackBlock
							{
								MSystem.AddRegenBlock(i, k);
							}
							if (MSystem.mBlockType[i, k] == BreakableTileID.FakeHint) //FakeBlockHint
							{
								MSystem.AddRegenBlock(i, k);
							}
						}
					}
				}
			}
			#endregion
			//Is there a better workaround for this?
			falling = false;
			if (Math.Sign(Player.position.Y - Player.oldPosition.Y) == Player.gravDir)
			{
				falling = true;
			}
		}
		public static bool TouchTiles(Vector2 Position, int Width, int Height, int tileType)
		{
			Vector2 vector = Position;
			int num = (int)MathHelper.Clamp((Position.X / 16f) - 1, 0, Main.maxTilesX - 1);
			int num2 = (int)MathHelper.Clamp(((Position.X + (float)Width) / 16f) + 2, 0, Main.maxTilesX - 1);
			int num3 = (int)MathHelper.Clamp((Position.Y / 16f) - 1, 0, Main.maxTilesY - 1);
			int num4 = (int)MathHelper.Clamp(((Position.Y + (float)Height) / 16f) + 2, 0, Main.maxTilesY - 1);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i, j].HasTile && !Main.tile[i, j].IsActuated && Main.tile[i, j].TileType == tileType)
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num6 = 16;
						if (Main.tile[i, j].IsHalfBlock)
						{
							vector2.Y += 8f;
							num6 -= 8;
						}
						if (vector.X + (float)Width >= vector2.X && vector.X <= vector2.X + 16f && vector.Y + (float)Height >= vector2.Y && (double)vector.Y <= (double)(vector2.Y + (float)num6) + 0.01)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		public override void PreUpdateMovement()
		{
			// 'Standing on NPC' mechanic. 
			// Might need some more work, but that's for something in the future.
			// TODO: THE Player SLIDED OVER THE TOP OF THE TRIPPER WHEN IT CHANGES DIRECTION.
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				if (npc.active && ((MetroidMod.Instance).FrozenStandOnNPCs.Contains(npc.type) || npc.type == ModContent.NPCType<Content.NPCs.Mobs.Utility.Tripper>()))
				{
					MGlobalNPC mnpc = npc.GetGlobalNPC<MGlobalNPC>();
					if (!mnpc.froze && npc.type != ModContent.NPCType<Content.NPCs.Mobs.Utility.Tripper>()) { continue; }

					if (Player.position.X + Player.width >= npc.position.X && Player.position.X <= npc.position.X + npc.width &&
						Player.position.Y + Player.height <= npc.position.Y && Player.position.Y + Player.velocity.Y + Player.height >= npc.position.Y)
					{
						Player.velocity.Y = 0;
						Player.position = Player.oldPosition;

						if (npc.type == ModContent.NPCType<Content.NPCs.Mobs.Utility.Tripper>())
						{
							if ((npc.direction == 1 && npc.velocity.X < 2) || (npc.direction == -1 && npc.velocity.X > -2))
							{
								Player.position.X = Player.oldPosition.X + npc.velocity.X + (npc.direction * .08F);
							}
							else
							{
								Player.position.X = Player.oldPosition.X + npc.velocity.X;
							}
						}
					}
				}
			}
		}
		public override void PostUpdateMiscEffects()
		{
			PostUpdateMiscEffects_Accessories();
			PostUpdateMiscEffects_MorphBall();
			PostUpdateMiscEffects_Visors();

			if (senseMove && senseMoveEnabled)
			{
				SenseMove(Player);
			}

			GrappleBeamMovement();
			
			if (Energy <= 30 && ShouldShowArmorUI == true)
			{
				energyLowTimer--;
				if (energyLowTimer <= 0)
				{
					energyLowTimer = Common.Configs.MConfigClient.Instance.energyLowInterval;
					if (Common.Configs.MConfigClient.Instance.energyLow)
					{
						SoundEngine.PlaySound(Sounds.Suit.EnergyLow, Player.position);
					}
				}
			}
		}
		public override void PostUpdateRunSpeeds()
		{
			PostUpdateRunSpeeds_Accessories();
			PostUpdateRunSpeeds_MorphBall();

		}
		public override void PostUpdate()
		{
			PostUpdate_Accessories();
			PostUpdate_MorphBall();

			grapplingBeam = -1;

			if (SMoveEffect > 0)
			{
				SMoveEffect--;
			}
			if (senseMoveCooldown > 0)
			{
				senseMoveCooldown--;
			}
			if (dashTime > 0)
			{
				dashTime--;
			}
			if (dashTime < 0)
			{
				dashTime++;
			}
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			ModifyHurt_SuitEnergy(ref modifiers);
		}
		public override void PostHurt(Player.HurtInfo info)
		{
			PostHurt_SuitEnergy(info);
		}
		public override bool ConsumableDodge(Player.HurtInfo info)
		{
			if (SMoveEffect > 0)
			{
				return true;
			}
			return false;
		}

		public void SenseMove(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();

			if (P.mount.Active || mp.ballstate)
			{
				return;
			}

			if (mp.SMoveEffect > 0)
			{
				if ((!P.controlLeft || !(P.velocity.X < 0f)) && (!P.controlRight || !(P.velocity.X > 0f)))
				{
					P.velocity.X *= 0.95f;
				}
				if (P.velocity.Y == 0f || !mp.spaceJump)
				{
					P.velocity.X *= 0.98f;
				}
			}

			int num20 = 0;
			bool flag2 = false;
			if (mp.senseMoveCooldown <= 0 && (P.velocity.Y == 0f || mp.spaceJump))
			{
				if (P.controlRight && P.releaseRight && !mp.shineActive)//MetroidMod.SenseMoveKey.Current)
				{
					if (mp.dashTime > 0)
					{
						num20 = 1;
						flag2 = true;
						mp.dashTime = 0;
					}
					else
					{
						mp.dashTime = 15;
					}
				}
				else if (P.controlLeft && P.releaseLeft && !mp.shineActive)//MetroidMod.SenseMoveKey.Current)
				{
					if (mp.dashTime < 0)
					{
						num20 = -1;
						flag2 = true;
						mp.dashTime = 0;
					}
					else
					{
						mp.dashTime = -15;
					}
				}
			}
			if (flag2)
			{
				P.velocity.X = 14.5f * (float)num20;
				Point point3 = (P.Center + new Vector2(num20 * P.width / 2 + 2, P.gravDir * (float)(-P.height) / 2f + P.gravDir * 2f)).ToTileCoordinates();
				Point point4 = (P.Center + new Vector2(num20 * P.width / 2 + 2, 0f)).ToTileCoordinates();
				if (WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y))
				{
					P.velocity.X /= 2f;
				}
				P.velocity.Y -= 4.5f * P.gravDir;
				mp.SMoveEffect = 20;
				mp.senseMoveCooldown = 60;

				if (!mp.senseSound)
				{
					SoundEngine.PlaySound(Sounds.Suit.SenseMove, P.position);
					mp.senseSound = true;
				}
			}
			else
			{
				mp.senseSound = false;
			}
		}
		public void GrappleBeamMovement()
		{
			if (grapplingBeam >= 0)
			{
				Projectile projectile = Main.projectile[grapplingBeam];
				if ((projectile.type == ModContent.ProjectileType<Content.Projectiles.GrappleBeamShot>() || projectile.type == ModContent.ProjectileType<Content.Projectiles.GrappleBeamPlusShot>()) && projectile.owner == Player.whoAmI && projectile.active)
				{
					float targetrotation = (float)Math.Atan2(((projectile.Center.Y - Player.Center.Y) * Player.direction), ((projectile.Center.X - Player.Center.X) * Player.direction));
					grappleRotation = targetrotation;

					if (Main.myPlayer == Player.whoAmI && Player.mount.Active)
					{
						Player.mount.Dismount(Player);
					}
					Player.canCarpet = true;
					Player.carpetFrame = -1;
					Player.wingFrame = 1;
					if (Player.velocity.Y == 0f || (Player.wet && (double)Player.velocity.Y > -0.02 && (double)Player.velocity.Y < 0.02))
					{
						Player.wingFrame = 0;
					}
					if (Player.wings == 4)
					{
						Player.wingFrame = 3;
					}
					if (Player.wings == 30)
					{
						Player.wingFrame = 0;
					}
					Player.wingTime = (float)Player.wingTimeMax;
					Player.rocketTime = Player.rocketTimeMax;
					Player.rocketDelay = 0;
					Player.rocketFrame = false;
					Player.canRocket = false;
					Player.rocketRelease = false;
					Player.fallStart = (int)(Player.position.Y / 16f);

					Vector2 vel = Vector2.Zero;

					float maxMaxDist = 400;
					Vector2 v = Player.Center - projectile.Center;
					float dist = Vector2.Distance(Player.Center, projectile.Center);
					bool up = (Player.controlUp && maxDist > 3);
					bool down = (Player.controlDown && maxDist < maxMaxDist);
					float reelSpeed = 11f;
					if (Player.honeyWet && !Player.ignoreWater)
					{
						reelSpeed *= 0.25f;
					}
					else if (Player.wet && !Player.merman && !Player.ignoreWater)
					{
						reelSpeed *= 0.5f;
					}
					if (dist > maxDist || up)
					{
						Player.maxRunSpeed = 15f;
						Player.runAcceleration *= 3f;
						Player.jump = 0;
						if (Player.velocity.Y == 0f)
						{
							Player.velocity.Y = 1E-05f;
						}
						float reel = 0f;
						if (up)
						{
							reel = Math.Max(-reelSpeed, 2 - dist);
							maxDist = Math.Min(dist, maxMaxDist);
						}
						if (down)
						{
							reel = Math.Min(reelSpeed, maxMaxDist - dist);
							maxDist = Math.Min(dist, maxMaxDist);
						}
						float ndist = Vector2.Distance(Player.Center + Player.velocity, projectile.Center);
						float ddist = ndist - dist;
						v /= dist;
						Player.velocity -= v * ddist;
						v *= (maxDist + reel);
						vel = (projectile.Center + v) - Player.Center;
						vel = Collision.TileCollision(Player.position, vel, Player.width, Player.height, Player.controlDown, false);
						Player.position += vel;
					}
					else if (down)
					{
						maxDist = Math.Min(maxDist + (reelSpeed / 2), maxMaxDist);
					}

					if (Player.controlJump)
					{
						if (Player.releaseJump)
						{
							if (maxDist <= 20 && !Player.controlDown)
							{
								Player.velocity.Y = -Player.jumpSpeed;
								Player.jump = Player.jumpHeight / 2;
							}
							else
							{
								Player.velocity.Y = Player.velocity.Y + 0.01f;
							}
							Player.velocity += vel;
							/*
							if (Player.hasJumpOption_Cloud)
							{
								Player.canJumpAgain_Cloud = true;
							}
							if (Player.hasJumpOption_Sandstorm)
							{
								Player.canJumpAgain_Sandstorm = true;
							}
							if (Player.hasJumpOption_Blizzard)
							{
								Player.canJumpAgain_Blizzard = true;
							}
							if (Player.hasJumpOption_Fart)
							{
								Player.canJumpAgain_Fart = true;
							}
							if (Player.hasJumpOption_Sail)
							{
								Player.canJumpAgain_Sail = true;
							}
							if (Player.hasJumpOption_Unicorn)
							{
								Player.canJumpAgain_Unicorn = true;
							}
							if (Player.hasJumpOption_Basilisk)
							{
								Player.canJumpAgain_Basilisk = true;
							}
							if (Player.hasJumpOption_Santank)
							{
								Player.canJumpAgain_Santank = true;
							}
							if (Player.hasJumpOption_WallOfFleshGoat)
							{
								Player.canJumpAgain_WallOfFleshGoat = true;
							}
							*/
							Player.blockExtraJumps = false;
							Player.releaseJump = false;

							grapplingBeam = -1;
							Player.grappling[0] = -1;
							Player.grapCount = 0;
							for (int k = 0; k < 1000; k++)
							{
								if (Main.projectile[k].active && Main.projectile[k].owner == Player.whoAmI && Main.projectile[k].aiStyle == NPCAIStyleID.Passive)//type == projectile.type)
								{
									Main.projectile[k].Kill();
								}
							}
							return;
						}
					}
					else
					{
						Player.releaseJump = true;
					}
				}
			}
		}

		public bool psuedoScrewActive = false;
		public bool beamChangeActive = false;
		public bool missileChangeActive = false;
		public override void SaveData(TagCompound tag)
		{
			tag["psuedoScrewAttackActive"] = psuedoScrewActive;
			tag["senseMoveEnabled"] = senseMoveEnabled;
			tag["energy"] = Energy;
			tag["reserves"] = SuitReserves;
			tag["reserveAuto"] = SuitReservesAuto;
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				bool flag = tag.GetBool("psuedoScrewAttackActive");
				if (flag)
				{
					psuedoScrewActive = flag;
				}

				flag = tag.GetBool("senseMoveEnabled");
				if (!flag)
				{
					senseMoveEnabled = flag;
				}

				int energy = tag.GetInt("energy");
				if (energy > 0)
				{
					Energy = energy;
				}

				energy = tag.GetInt("reserves");
				if (energy > 0)
				{
					SuitReserves = energy;
				}

				flag = tag.GetBool("reserveAuto");
				if (flag)
				{
					SuitReservesAuto = flag;
				}
			}
			catch { }
		}

		/* NETWORK SYNCING. <<<<<< WIP >>>>>> */

		// Using Initialize to make sure every Player has his/her own instance.
		public override void Initialize()
		{
			oldPos = new Vector2[oldNumMax];

			spiderball = false;

			statCharge = 0;
			boostCharge = 0;
			boostEffect = 0;
			EnergyTanks = 0;
			Energy = 0;
		}

		public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
		{
			MPlayer clone = clientClone as MPlayer;

			clone.statCharge = statCharge;
			clone.spiderball = spiderball;
			clone.boostEffect = boostEffect;
			clone.boostCharge = boostCharge;
			clone.EnergyTanks = EnergyTanks;
			clone.Energy = Energy;
			clone.SuitReserveTanks = SuitReserveTanks;
			clone.SuitReserves = SuitReserves;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)MetroidMessageType.SyncStartPlayerStats);
			packet.Write((byte)Player.whoAmI);
			packet.Write((double)statCharge);
			packet.Write(spiderball);
			packet.Write(boostEffect);
			packet.Write(boostCharge);
			packet.Write(EnergyTanks);
			packet.Write(Energy);
			packet.Write(SuitReserveTanks);
			packet.Write(SuitReserves);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			MPlayer clone = clientPlayer as MPlayer;
			if (clone.statCharge != statCharge || clone.spiderball != spiderball || clone.boostEffect != boostEffect || clone.boostCharge != boostCharge)
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)MetroidMessageType.SyncPlayerStats);
				packet.Write((byte)Player.whoAmI);
				packet.Write((double)statCharge);
				packet.Write(spiderball);
				packet.Write(boostEffect);
				packet.Write(boostCharge);
				packet.Write(EnergyTanks);
				packet.Write(Energy);
				packet.Write(SuitReserveTanks);
				packet.Write(SuitReserves);
				packet.Send();
			}
		}
	}
}
