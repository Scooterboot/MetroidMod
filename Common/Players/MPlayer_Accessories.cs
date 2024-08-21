using System;
using MetroidMod.Common.Configs;

//using MetroidMod.Content.NPCs;
//using MetroidMod.Content.Items;
using MetroidMod.Common.Systems;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.Items.Armors;
using MetroidMod.Content.Mounts;
using MetroidMod.Content.Tiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	public partial class MPlayer : ModPlayer
	{
		public bool powerGrip = false;
		public bool isGripping = false;
		public int reGripTimer = 0;
		public int gripDir = 1;

		public bool speedBooster = false;
		public bool speedBoosting = false;
		private float speedBuildUp = 0f;
		public bool shineActive = false;
		public int shineDirection = 0;
		public int shineCharge = 0;
		private int shineDischarge = 0;
		private int proj = -1;
		private int shineSound = 0;
		public int speedBoostDmg = 0;

		public bool somersault = false;
		public bool canSomersault = false;
		public bool disableSomersault = false;
		public float rotation = 0.0f;
		public float rotateSpeed = 0.05f;
		public float rotateSpeed2 = 50f;
		public float rotateCountX = 0.05f;
		public float rotateCountY = 0.05f;
		private int itemRotTweak = 0;

		public bool EnableWallJump = false;
		public bool canWallJump = false;

		public bool hiJumpBoost = false;
		/// <summary>
		/// Determines if you have a double-jump equipped.
		/// </summary>
		public bool spaceJumpBoots = false;
		///<summary>
		///If true, use the Spin Boost effects instead of Space Jump Boots.
		///</summary>
		public bool itsSpinBoost = true;
		public bool spaceJumped = false;
		public bool spaceJump = false;
		public static float maxSpaceJumps = 120;
		public float statSpaceJumps = maxSpaceJumps;
		public int spaceJumpsRegenDelay = 0;
		public bool insigniaActive = false;

		public bool screwAttack = false;
		public int screwAttackSpeedEffect = 0;
		public int screwSpeedDelay = 0;
		public int screwAttackDmg = 0;

		public bool Eyed = false;

		/// <summary>
		/// The amount of Life Reserve tanks a player has.
		/// </summary>
		public int reserveTanks = 0;
		/// <summary>
		/// The amount of hearts currently stored in a player's Life Reserve tanks.
		/// </summary>
		public int reserveHearts = 0;
		/// <summary>
		/// The current value of each heart in a player's Life Reserve Tanks.
		/// </summary>
		public int reserveHeartsValue = 20;

		/// <summary>
		/// Whether or not this player can open red hatches by right-clicking.
		/// </summary>
		public bool RedKeycard = false;
		/// <summary>
		/// Whether or not this player can open green hatches by right-clicking.
		/// </summary>
		public bool GreenKeycard = false;
		/// <summary>
		/// Whether or not this player can open yellow hatches by right-clicking.
		/// </summary>
		public bool YellowKeycard = false;

		public void ResetEffects_Accessories()
		{
			powerGrip = false;

			Eyed = false;
			speedBooster = false;
			speedBoosting = false;
			speedBoostDmg = 0;

			disableSomersault = false;
			EnableWallJump = false;

			hiJumpBoost = false;
			spaceJumpBoots = false;
			itsSpinBoost = true;
			spaceJump = false;

			insigniaActive = false;

			screwAttack = false;
			screwAttackDmg = 0;

			bool flag = false;
			for (int i = 0; i < Player.buffType.Length; i++)
			{
				if (Player.buffType[i] == ModContent.BuffType<Content.Buffs.EnergyRecharge>() && Player.buffTime[i] > 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				reserveTanks = 0;
				reserveHeartsValue = 20;
			}

			RedKeycard = false;
			GreenKeycard = false;
			YellowKeycard = false;
		}
		public void PreUpdate_Accessories()
		{
			Player P = Player;
			somersault = (!P.dead && !disableSomersault && (SMoveEffect > 0 || canSomersault) && !P.mount.Active && P.velocity.Y != 0 /*&& P.velocity.X != 0*/ && !P.sliding && !P.pulley && !isGripping && (P.itemAnimation == 0 || statCharge >= 30) && P.grappling[0] <= -1 && grapplingBeam <= -1 && shineDirection == 0 && !shineActive && !ballstate && (((P.wingsLogic != 0 || P.rocketBoots != 0 || P.carpet) && (!P.controlJump || (!P.canRocket && !P.rocketRelease && P.wingsLogic == 0) || (P.wingTime <= 0 && P.rocketTime <= 0 && P.carpetTime <= 0))) || (P.wingsLogic == 0 && P.rocketBoots == 0 && !P.carpet)) && !P.sandStorm);
			somersault &= !(P.rocketDelay <= 0 && P.wingsLogic > 0 && P.controlJump && P.velocity.Y > 0f && P.wingTime <= 0);

			if (Player.velocity.Y == 0 || Player.sliding || (Player.autoJump && Player.justJumped) || Player.grappling[0] >= 0 || grapplingBeam >= 0)
			{
				statSpaceJumps = maxSpaceJumps;
			}

			if (statSpaceJumps < maxSpaceJumps && spaceJumpsRegenDelay <= 0)
			{
				statSpaceJumps++;
			}
			else if (spaceJumpsRegenDelay <= 0)
			{
				statSpaceJumps = maxSpaceJumps;
			}
			if (spaceJumpsRegenDelay > 0)
			{
				spaceJumpsRegenDelay--;
			}
			if (reserveHearts > reserveTanks)
			{
				reserveHearts = reserveTanks;
			}

			if (!Player.mount.Active)
			{
				if (somersault)
				{
					float rotMax = (float)Math.PI / 8;
					if (spaceJump)// && SMoveEffect <= 0)
					{
						rotMax = (float)Math.PI / 4;
					}
					rotation += MathHelper.Clamp((rotateCountX + rotateCountY) * Player.direction * Player.gravDir, -rotMax, rotMax);
					if (rotation > (Math.PI * 2f))
					{
						rotation -= (float)(Math.PI * 2f);
					}
					if (rotation < -(Math.PI * 2f))
					{
						rotation += (float)(Math.PI * 2f);
					}
					Player.fullRotation = rotation;
					Player.fullRotationOrigin = new Vector2((float)Player.width / 2, (float)Player.height * 0.55f);
					if (Player.gravDir == -1)
					{
						Player.fullRotationOrigin.Y = (float)Player.height * 0.45f;
					}
					itemRotTweak = 2;

				}
				else if (shineDirection == 2 || shineDirection == 4) //right and up or left and up
				{
					rotation = 0.05f * Player.direction * Player.gravDir;
					Player.fullRotation = rotation;
					Player.fullRotationOrigin = Player.Center - Player.position;
				}
				else if (shineDirection == 1 || shineDirection == 3) //right or left
				{
					rotation = ((float)Math.PI / 4f) * Player.direction * Player.gravDir;
					Player.fullRotation = rotation;
					Player.fullRotationOrigin = Player.Center - Player.position;
				}
				else if (shineDirection == 6 || shineDirection == 7) // right and down or left and down
				{
					rotation = (float)Math.PI / 3f * Player.direction * Player.gravDir;
					Player.fullRotation = rotation;
					Player.fullRotationOrigin = Player.Center - Player.position;
				}
				else if (shineDirection == 8) //down
				{
					rotation = (float)Math.PI * Player.gravDir;
					Player.fullRotation = rotation;
					Player.fullRotationOrigin = Player.Center - Player.position;
				}
				else if (!P.sleeping.isSleeping) //up
				{
					rotation = 0f;
					Player.fullRotation = 0f;
				}
			}
			else
			{
				rotation = 0f;
				Player.fullRotation = 0f;
			}
			if (spaceJump)
			{
				rotateSpeed = 0.2f;
				rotateSpeed2 = 20f;
			}
			else
			{
				rotateSpeed = 0.08f;
				rotateSpeed2 = 40f;
			}
			if (Player.velocity.Y > 1)
			{
				rotateCountY = rotateSpeed + Player.velocity.Y / rotateSpeed2;
			}
			else if (Player.velocity.Y < -1)
			{
				rotateCountY = rotateSpeed + (Player.velocity.Y / rotateSpeed2) * (-1f);
			}
			else
			{
				rotateCountY = rotateSpeed;
			}
			if (Player.velocity.X > 1)
			{
				rotateCountX = rotateSpeed + Player.velocity.X / rotateSpeed2;
			}
			else if (Player.velocity.X < -1)
			{
				rotateCountX = rotateSpeed + (Player.velocity.X / rotateSpeed2) * (-1f);
			}
			else
			{
				rotateCountX = rotateSpeed;
			}
			if (reserveHearts > reserveTanks)
			{
				reserveHearts = reserveTanks;
			}
		}
		private bool sbFlag = false;
		public void PostUpdateMiscEffects_Accessories()
		{
			Player player = Main.LocalPlayer;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			//	These kinds of checks might be pretty messy, I think, so I think I'll consider reforming them later when I can think clearly.
			if (Configs.MConfigItems.Instance.enableLedgeClimbNoPowerSuit && !mp.IsPowerSuitBreastplate)
			{
				mp.powerGrip = true;
			}
			GripMovement();
			canWallJump = false;
			int wallJumpDir = 0;
			bool altJump = false;
			if (EnableWallJump)
			{
				if (!Player.mount.Active || Player.mount.Active && Configs.MConfigMain.Instance.enableMorphBallWallJump)
				{
					CheckWallJump(Player, ref wallJumpDir, ref altJump);
				}
			}

			if (speedBooster)
			{
				AddSpeedBoost(Player, speedBoostDmg);
				if (Player.controlJump)
				{
					if (Player.velocity.Y == 0)
					{
						sbFlag = true;
					}
				}
				else
				{
					sbFlag = false;
				}
				if (sbFlag)
				{
					if (Player.velocity.X <= -4f && Player.controlLeft)
					{
						Player.jumpSpeedBoost += Math.Abs(Player.velocity.X / 4f);
					}
					else if (Player.velocity.X >= 4f && Player.controlRight)
					{
						Player.jumpSpeedBoost += Math.Abs(Player.velocity.X / 4f);
					}
				}
			}
			else
			{
				sbFlag = false;
			}

			DoWallJump(Player, wallJumpDir, altJump);

			if (spaceJumpBoots || spaceJump || screwAttack)
			{
				EnableWallJump = true;
				AddSpaceJumpBoots(Player);
				if (spaceJump)
				{
					AddSpaceJump(Player);
				}
				if (screwAttack)
				{
					AddScrewAttack(Player, screwAttackDmg);
				}
			}
			else if (EnableWallJump)
			{
				if (Player.velocity.Y == 0f || Player.sliding || (Player.autoJump && Player.justJumped) || Player.grappling[0] >= 0 || grapplingBeam >= 0)
				{
					if (Player.velocity.X != 0 || Player.sliding)
					{
						canSomersault = true;
					}
					else if (!Player.sliding)
					{
						canSomersault = false;
					}
				}
			}

			if (shineActive || shineDirection != 0 || (spiderball && CurEdge != Edge.None))
			{
				//Player.gravity = 0f;
				float num3 = Player.gravity;
				if (Player.slowFall)
				{
					if (Player.controlUp)
					{
						num3 = Player.gravity / 10f * Player.gravDir;
					}
					else
					{
						num3 = Player.gravity / 3f * Player.gravDir;
					}
				}
				Player.velocity.Y -= num3;
			}
		}
		public void PostUpdateRunSpeeds_Accessories()
		{
			if (hiJumpBoost)
			{
				Player.jumpHeight += 5;
				Player.jumpSpeed += 1.5f;
			}
		}
		public void PostUpdate_Accessories()
		{
			if (Player.itemAnimation > 0)
			{
				if (itemRotTweak > 0)
				{
					float MY = Main.mouseY + Main.screenPosition.Y;
					float MX = Main.mouseX + Main.screenPosition.X;
					if (Player.gravDir == -1f)
					{
						MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
					Vector2 oPos = Player.RotatedRelativePoint(Player.MountedCenter, true);
					Player.ChangeDir(Math.Sign(MX - oPos.X));
					Player.itemRotation = (float)Math.Atan2((MY - oPos.Y) * Player.direction, (MX - oPos.X) * Player.direction) - Player.fullRotation;
					itemRotTweak--;
				}
			}
			else
			{
				itemRotTweak = 0;
			}

			if (!speedBooster)
			{
				speedBuildUp = 0;
				shineCharge = 0;
				shineSound = 0;
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
			}
			if (!spaceJumpBoots && !spaceJump && !screwAttack && !EnableWallJump)
			{
				canSomersault = false;
			}
			if (!screwAttack)
			{
				screwAttackSpeedEffect = 0;
				screwSpeedDelay = 0;
			}
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (reserveTanks > 0 && reserveHearts > 0)
			{
				if (Player.statLifeMax < reserveHearts * reserveHeartsValue)
				{
					Player.statLife = Player.statLifeMax;
					reserveHearts -= (int)Math.Ceiling((double)Player.statLifeMax / reserveHeartsValue);
				}
				else
				{
					Player.statLife = reserveHearts * reserveHeartsValue;
					reserveHearts = 0;
				}
				SoundEngine.PlaySound(Sounds.Suit.MissilesReplenished, Player.position);
				return false;
			}
			Player player = Main.LocalPlayer;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (mp.PrimeHunter)
			{
				bool wearingSuit = Player.armor[0].type == ModContent.ItemType<PowerSuitHelmet>() && Player.armor[1].type == ModContent.ItemType<PowerSuitBreastplate>() && Player.armor[2].type == ModContent.ItemType<PowerSuitGreaves>();
				if (!wearingSuit)
				{
					damageSource = PlayerDeathReason.ByCustomReason($"{Player.name} did not find an exploit");
				}
				else
				{
					damageSource = PlayerDeathReason.ByCustomReason("The Prime Hunter is dead!");
				}
				SoundEngine.PlaySound(Sounds.Suit.SamusDeath, Player.position);
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
		public void GripMovement()
		{
			gripDir = Player.direction;
			isGripping = false;
			reGripTimer--;
			if (reGripTimer <= 0 && powerGrip && !Player.mount.Active && ((!Player.controlRight && gripDir == -1) || (!Player.controlLeft && gripDir == 1)) && !Player.shimmering)
			{
				bool flag = false;
				float num = Player.position.X;
				if (gripDir == 1)
				{
					num += (float)Player.width;
				}
				num += (float)gripDir;
				float num2 = Player.position.Y + 8f;
				if (Player.gravDir < 0f)
				{
					num2 = Player.position.Y + (float)Player.height - 8f;
				}
				num = MathHelper.Clamp(num / 16f, 0, Main.maxTilesX - 1);
				num2 = MathHelper.Clamp(num2 / 16f, 0, Main.maxTilesY - 1);
				/*
				//Allow gripping onto non solid tiles
				if (Main.tile[(int)num, (int)num2].active() && !Main.tile[(int)num, (int)num2].inActive() && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.Rope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.SilkRope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.VineRope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.WebRope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.Chain && (Main.tile[(int)num, (int)num2 - (int)Player.gravDir].inActive() || !Main.tile[(int)num, (int)num2 - (int)Player.gravDir].active() || (Main.tile[(int)num, (int)num2 - 1].bottomSlope() && Player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].topSlope() && Player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num, (int)num2 - (int)Player.gravDir].type] || Main.tileSolidTop[Main.tile[(int)num, (int)num2 - (int)Player.gravDir].type] || (Main.tile[(int)num, (int)num2].halfBrick() && Player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].halfBrick() && Player.gravDir == -1) || Main.tile[(int)num, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
				{
					flag = true;
				}
				float num3 = Player.Center.X / 16f;
				if (Main.tile[(int)num3, (int)num2].active() && !Main.tile[(int)num3, (int)num2].inActive() && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.Rope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.SilkRope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.VineRope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.WebRope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.Chain && (Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].inActive() || !Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].active() || (Main.tile[(int)num3, (int)num2 - 1].bottomSlope() && Player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].topSlope() && Player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].type] || Main.tileSolidTop[Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].type] || (Main.tile[(int)num3, (int)num2].halfBrick() && Player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].halfBrick() && Player.gravDir == -1) || Main.tile[(int)num3, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
				{
					flag = true;
				}
				*/
				if (Main.tile[(int)num, (int)num2].HasTile && !Main.tile[(int)num, (int)num2].IsActuated && Main.tileSolid[Main.tile[(int)num, (int)num2].TileType] && !Main.tileSolidTop[Main.tile[(int)num, (int)num2].TileType] && (Main.tile[(int)num, (int)num2 - (int)Player.gravDir].IsActuated || !Main.tile[(int)num, (int)num2 - (int)Player.gravDir].HasTile || (Main.tile[(int)num, (int)num2 - 1].BottomSlope && Player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].TopSlope && Player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num, (int)num2 - (int)Player.gravDir].TileType] || Main.tileSolidTop[Main.tile[(int)num, (int)num2 - (int)Player.gravDir].TileType] || (Main.tile[(int)num, (int)num2].IsHalfBlock && Player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].IsHalfBlock && Player.gravDir == -1) || Main.tile[(int)num, (int)num2].TileType == TileID.MinecartTrack))
				{
					flag = true;
				}
				if (Main.tile[(int)num, (int)num2].TileType == ModContent.TileType<GripLedge>() && !Main.tile[(int)num, (int)num2].IsActuated && Main.tile[(int)num, (int)num2].HasTile)
				{
					flag = true;
				}
				bool crumble = (MSystem.mBlockType[(int)num, (int)num2] == BreakableTileID.CrumbleInstant || MSystem.mBlockType[(int)num, (int)num2] == BreakableTileID.CrumbleSpeed || MSystem.mBlockType[(int)num, (int)num2] == BreakableTileID.CrumbleSlow);
				if (crumble && Math.Sign(Player.velocity.Y) == Player.gravDir && Main.tile[(int)num, (int)num2].HasTile && !Main.tile[(int)num, (int)num2].IsActuated)
				{
					if (MSystem.mBlockType[(int)num, (int)num2] == BreakableTileID.CrumbleInstant) //CrumbleInstant
					{
						MSystem.AddRegenBlock((int)num, (int)num2, true);
						// Enforce SpeedBooster
						if (falling)
						{
							Player.velocity.X = 0;
							Player.oldVelocity.X = 0;
						}
						flag = false;
					}
					if (MSystem.mBlockType[(int)num, (int)num2] == BreakableTileID.CrumbleSpeed) //CrumbleSpeed
					{
						MSystem.nextTick.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 1, new Vector2((int)num, (int)num2)));
						flag = false;
					}
					if (MSystem.mBlockType[(int)num, (int)num2] == BreakableTileID.CrumbleSlow) //CrumbleSlow
					{
						MSystem.hit[(int)num, (int)num2] = true;
						MSystem.timers.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 60, new Vector2((int)num, (int)num2)));
						flag = false;
					}
				}
				float num3 = Player.Center.X / 16f;
				if (Main.tile[(int)num3, (int)num2].HasTile && !Main.tile[(int)num3, (int)num2].IsActuated && Main.tileSolid[Main.tile[(int)num3, (int)num2].TileType] && !Main.tileSolidTop[Main.tile[(int)num3, (int)num2].TileType] && (Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].IsActuated || !Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].HasTile || (Main.tile[(int)num3, (int)num2 - 1].BottomSlope && Player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].TopSlope && Player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].TileType] || Main.tileSolidTop[Main.tile[(int)num3, (int)num2 - (int)Player.gravDir].TileType] || (Main.tile[(int)num3, (int)num2].IsHalfBlock && Player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].IsHalfBlock && Player.gravDir == -1) || Main.tile[(int)num3, (int)num2].TileType == TileID.MinecartTrack))
				{
					flag = true;
				}
				if (Main.tile[(int)num3, (int)num2].TileType == ModContent.TileType<Content.Tiles.GripLedge>() && !Main.tile[(int)num3, (int)num2].IsActuated && Main.tile[(int)num3, (int)num2].HasTile)
				{
					flag = true;
				}

				crumble = (MSystem.mBlockType[(int)num3, (int)num2] == BreakableTileID.CrumbleInstant || MSystem.mBlockType[(int)num3, (int)num2] == BreakableTileID.CrumbleSpeed || MSystem.mBlockType[(int)num3, (int)num2] == 11);
				if (crumble && Math.Sign(Player.velocity.Y) == Player.gravDir && Main.tile[(int)num3, (int)num2].HasTile && !Main.tile[(int)num3, (int)num2].IsActuated)
				{
					if (MSystem.mBlockType[(int)num3, (int)num2] == BreakableTileID.CrumbleInstant) //CrumbleInstant
					{
						MSystem.AddRegenBlock((int)num3, (int)num2, true);
						// Enforce SpeedBooster
						if (falling)
						{
							Player.velocity.X = 0;
							Player.oldVelocity.X = 0;
						}
						flag = false;
					}
					if (MSystem.mBlockType[(int)num3, (int)num2] == BreakableTileID.CrumbleSpeed) //CrumbleSpeed
					{
						MSystem.nextTick.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 1, new Vector2((int)num3, (int)num2)));
						flag = false;
					}
					if (MSystem.mBlockType[(int)num3, (int)num2] == BreakableTileID.CrumbleSlow) //CrumbleSlow
					{
						MSystem.hit[(int)num3, (int)num2] = true;
						MSystem.timers.Enqueue(new Tuple<int, Vector2>((int)(MSystem.Timer) + 60, new Vector2((int)num3, (int)num2)));
						flag = false;
					}
				}

				if (flag && ((Player.velocity.Y > 0f && Player.gravDir == 1f) || (Player.velocity.Y < Player.gravity && Player.gravDir == -1f)))
				{
					if (!Player.controlDown)
					{
						reGripTimer = 0;
						Player.fullRotation = 0;
						Player.position.Y = ((int)num2 * 16) - 8;
						if (Player.gravDir == 1 && (Main.tile[(int)num, (int)num2].IsHalfBlock || Main.tile[(int)num, (int)num2].TileType == TileID.MinecartTrack || Main.tile[(int)num3, (int)num2].IsHalfBlock || Main.tile[(int)num3, (int)num2].TileType == TileID.MinecartTrack))
						{
							Player.position.Y += 8;
						}
						if (Player.gravDir == -1)
						{
							Player.position.Y -= 12;
						}
						float grav = Player.gravity;
						if (Player.slowFall)
						{
							if (Player.controlUp)
							{
								grav = Player.gravity / 10f * Player.gravDir;
							}
							else
							{
								grav = Player.gravity / 3f * Player.gravDir;
							}
						}
						if (Player.velocity.X > 2)
						{
							Player.velocity.X = 2;
						}
						if (Player.velocity.X < -2)
						{
							Player.velocity.X = -2;
						}
						Player.fallStart = (int)(Player.position.Y / 16f);
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
						if (Player.hasJumpOption_WallOfFleshGoat)
						{
							Player.canJumpAgain_WallOfFleshGoat = true;
						}
						if (Player.hasJumpOption_Santank)
						{
							Player.canJumpAgain_Santank = true;
						}
						*/
						Player.blockExtraJumps = false;
						if (Player.controlJump)
						{
							Player.velocity.Y = -Player.jumpSpeed * Player.gravDir;
							Player.jump = Player.jumpHeight;
							canSomersault = true;
							SoundEngine.PlaySound(Sounds.Suit.GripClimb, Player.position);
						}
						else if (Player.controlUp)
						{
							Player.velocity.Y = -6 * Player.gravDir;
							reGripTimer = 10;
							SoundEngine.PlaySound(Sounds.Suit.GripClimb, Player.position);
						}
						else
						{
							Player.velocity.Y = (-grav + 1E-05f) * Player.gravDir;
						}
					}
					isGripping = true;
				}
				if (isGripping && Player.controlDown)
				{
					isGripping = false;
					reGripTimer = 10;
				}
				bool gripledge = MSystem.mBlockType[(int)num, (int)num2] == ModContent.TileType<GripLedge>();
				if (isGripping && Player.controlRight && gripDir >= 1 && Player.releaseRight && !Player.mount.Active && Player.miscEquips[3].type == ModContent.ItemType<MorphBall>() && !gripledge && Main.keyState.IsKeyDown(Keys.LeftShift))
				{
					var ball = ModContent.MountType<MorphBallMount>();
					Player.QuickMount();
					isGripping = false;
					reGripTimer = 10;
					Player.position.X += 16f * gripDir;
					Player.position.Y -= 32f;
					SoundEngine.PlaySound(Sounds.Suit.MorphIn, Player.position);
				}
				if (isGripping && Player.controlLeft && gripDir <= -1 && Player.releaseLeft && !Player.mount.Active && Player.miscEquips[3].type == ModContent.ItemType<MorphBall>() && !gripledge && Main.keyState.IsKeyDown(Keys.LeftShift))
				{
					var ball = ModContent.MountType<MorphBallMount>();
					Player.QuickMount();
					isGripping = false;
					reGripTimer = 10;
					Player.position.X += 16f * gripDir;
					Player.position.Y -= 32f;
					SoundEngine.PlaySound(Sounds.Suit.MorphIn, Player.position);
				}
			}
		}
		public void CheckWallJump(Player Player, ref int dir, ref bool altJump)
		{
			canWallJump = false;
			altJump = false;
			dir = Player.controlLeft ? -1 : Player.controlRight ? 1 : Player.direction;
			if (dir != 0 && Player.velocity.Y != 0)
			{
				float margin = 6 + Math.Abs(Player.velocity.X); //Margin of error for Super Style wall jumping
				float xPos = Player.position.X;
				float xPosAlt = Player.position.X + Player.width + margin;
				if (dir == 1)
				{
					xPos += Player.width;
					xPosAlt = Player.position.X - margin;
				}
				xPos += dir * Math.Abs(Player.velocity.X + 1);
				float yPos = Player.position.Y + (float)Player.height - 1;
				if (Player.gravDir < 0f)
				{
					yPos = Player.position.Y + 1f;
				}
				xPos /= 16f;
				xPosAlt /= 16f;
				yPos /= 16f;
				if (WorldGen.SolidTile((int)xPos, (int)yPos) || WorldGen.SolidTile((int)xPos, (int)yPos - 1))
				{
					canWallJump = true;
				}
				if (WorldGen.SolidTile((int)xPosAlt, (int)yPos) || WorldGen.SolidTile((int)xPosAlt, (int)yPos - 1)) //Super Style wall jump
				{
					canWallJump = true;
					altJump = true;
					if (Player.controlLeft || Player.controlRight)
					{
						Player.fullRotation = dir * (float)Math.PI / 6;
						Player.fullRotationOrigin = new Vector2((float)Player.width / 2, (float)Player.height * 0.55f);
						if (Player.gravDir == -1)
						{
							Player.fullRotationOrigin.Y = (float)Player.height * 0.45f;
						}
						itemRotTweak = 2;
					}
				}
			}
		}
		public void DoWallJump(Player Player, int dir, bool altJump)
		{
			if (canWallJump && Player.controlJump && Player.releaseJump && (Player.controlLeft || Player.controlRight))
			{
				float xSpeed = 3f;  //3 for vanilla wall jump, 6 for restricting single wall jumping
				if (altJump)
				{
					dir *= -1;
					xSpeed = 6;
				}
				if (speedBoosting)
				{
					xSpeed = 11;
				}
				SoundEngine.PlaySound(Sounds.Suit.WallJump, Player.position);
				Player.jump = Player.jumpHeight;
				Player.velocity.Y = -Player.jumpSpeed * Player.gravDir;
				Player.velocity.X = xSpeed * -dir;
				Player.canRocket = false;
				Player.rocketRelease = false;
				Player.fallStart = (int)(Player.Center.Y / 16f);
				Player.autoJump = true;
				Player.justJumped = true;
				canSomersault = true;
			}
		}
		public static void AddSpaceJump(Player Player)
		{
			MPlayer mp = Player.GetModPlayer<MPlayer>();
			if (mp.statSpaceJumps >= 15 && Player.grappling[0] == -1 && mp.spaceJumped && !Player.GetJumpState(ExtraJump.CloudInABottle).Active && !Player.GetJumpState(ExtraJump.BlizzardInABottle).Active && !Player.GetJumpState(ExtraJump.SandstormInABottle).Active && !Player.GetJumpState(ExtraJump.FartInAJar).Active && Player.jump == 0 && Player.velocity.Y != 0f && Player.rocketTime == 0 && Player.wingTime == 0f && !Player.mount.Active)
			{
				if (Player.controlJump && Player.releaseJump && Player.velocity.Y != 0 && mp.spaceJumped)
				{
					Player.jump = Player.jumpHeight;
					Player.velocity.Y = -Player.jumpSpeed * Player.gravDir;
					if (!mp.insigniaActive)
					{
						mp.statSpaceJumps -= 15;
					}
					mp.spaceJumpsRegenDelay = 25;
				}
			}
		}
		public static void AddSpaceJumpBoots(Player Player)
		{
			MPlayer mp = Player.GetModPlayer<MPlayer>();
			if (Player.velocity.Y == 0f || Player.sliding || (Player.autoJump && Player.justJumped) || Player.grappling[0] >= 0 || mp.grapplingBeam >= 0)
			{
				mp.spaceJumped = false;
				if (Player.velocity.X != 0 || Player.sliding)
				{
					mp.canSomersault = true;
				}
				else if (!Player.sliding)
				{
					mp.canSomersault = false;
				}
			}
			else if ((!Player.mount.Active || !Player.mount.BlockExtraJumps) && Player.controlJump && Player.releaseJump && !mp.spaceJumped && Player.grappling[0] == -1 && mp.grapplingBeam <= -1 && Player.jump <= 0)
			{
				int num167 = Player.height;
				if (Player.gravDir == -1f)
				{
					num167 = 4;
				}
				if (mp.itsSpinBoost == true)  //Visual effects for specifically the spin boost. It's the special variable because SJB came first
				{
					SoundEngine.PlaySound(SoundID.Item21, Player.position);
					for (int num168 = 0; num168 < 16; num168++)
					{
						int jumpDust = 263;
						float scale2 = 2.5f;
						int alpha2 = 0;
						if (num168 % 2 == 1)
						{
							Dust jumpdust = Dust.NewDustPerfect(new Vector2(Player.position.X + 8f, Player.position.Y + (float)num167 - 10f), jumpDust, new Vector2((float)num168 / 3, 0f), alpha2, default(Color), scale2);
							jumpdust.noGravity = true; //cool thing about perfectdust: you don't have to break out main.dust[], it just works    -Z
							jumpdust.velocity.X = jumpdust.velocity.X * 1f - 2f;
							/*int jumpFX1 = Dust.NewDust(new Vector2(Player.position.X - num168, Player.position.Y + (float)num167 - 10f), 0, 0, jumpDust, 0f, 0f, alpha2, default(Color), scale2);
							Main.dust[jumpFX1].noGravity = true;
							Main.dust[jumpFX1].velocity.X = Main.dust[jumpFX1].velocity.X * 1f - 2f - Player.velocity.X * 0.6f;*/
							//Main.dust[jumpFX1].velocity.Y = Main.dust[jumpFX1].velocity.Y * 1f + 2f * Player.gravDir - Player.velocity.Y * 0.3f;
						}
						else
						{
							Dust jumpdust = Dust.NewDustPerfect(new Vector2(Player.position.X + 8f, Player.position.Y + (float)num167 - 10f), jumpDust, new Vector2(-(float)num168 / 3, 0f), alpha2, default(Color), scale2);
							jumpdust.noGravity = true;
							jumpdust.velocity.X = jumpdust.velocity.X * 1f + 2f;
							/*int jumpFX2 = Dust.NewDust(new Vector2(Player.position.X - num168, Player.position.Y + (float)num167 - 10f), 0, 0, jumpDust, 0f, 0f, alpha2, default(Color), scale2);
							Main.dust[jumpdust].noGravity = true;/
							Main.dust[jumpFX2].velocity.X = Main.dust[jumpFX2].velocity.X * 1f + 2f - Player.velocity.X * 0.6f;*/
							//Main.dust[jumpFX2].velocity.Y = Main.dust[jumpFX2].velocity.Y * 1f + 2f * Player.gravDir - Player.velocity.Y * 0.3f;
						}
					}
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Item20, Player.position);
					for (int num168 = 0; num168 < 8; num168++)
					{
						int type4 = 6;
						float scale2 = 2.5f;
						int alpha2 = 100;
						if (num168 <= 3)
						{
							int num169 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)num167 - 10f), 8, 8, type4, 0f, 0f, alpha2, default(Color), scale2);
							Main.dust[num169].noGravity = true;
							Main.dust[num169].velocity.X = Main.dust[num169].velocity.X * 1f - 2f - Player.velocity.X * 0.3f;
							Main.dust[num169].velocity.Y = Main.dust[num169].velocity.Y * 1f + 2f * Player.gravDir - Player.velocity.Y * 0.3f;
						}
						else
						{
							int num170 = Dust.NewDust(new Vector2(Player.position.X + (float)Player.width - 4f, Player.position.Y + (float)num167 - 10f), 8, 8, type4, 0f, 0f, alpha2, default(Color), scale2);
							Main.dust[num170].noGravity = true;
							Main.dust[num170].velocity.X = Main.dust[num170].velocity.X * 1f + 2f - Player.velocity.X * 0.3f;
							Main.dust[num170].velocity.Y = Main.dust[num170].velocity.Y * 1f + 2f * Player.gravDir - Player.velocity.Y * 0.3f;
						}
					}
				}
				mp.spaceJumped = true;
				mp.canSomersault = true;
				Player.jump = Player.jumpHeight;
				Player.velocity.Y = -Player.jumpSpeed * Player.gravDir;
				Player.canRocket = false;
				Player.rocketRelease = false;
				Player.fallStart = (int)(Player.Center.Y / 16f);
			}
			if (mp.spaceJumped)
			{
				mp.canSomersault = true;
			}
		}
		private int screwProj = -1;
		public void AddScrewAttack(Player Player, int damage)
		{
			if (somersault)
			{
				bool flag = false;
				Player.longInvince = true;
				int screwAttackID = ModContent.ProjectileType<Content.Projectiles.ScrewAttackProj>();
				foreach (Projectile P in Main.projectile)
				{
					if (P.active && P.owner == Player.whoAmI && P.type == screwAttackID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					screwProj = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.position.X + Player.width / 2, Player.position.Y + Player.height / 2, 0, 0, screwAttackID, damage, 0, Player.whoAmI);
				}
			}
			if (screwSpeedDelay <= 0 && !ballstate && Player.grappling[0] == -1 && Player.velocity.Y != 0f && !Player.mount.Active)
			{
				if (Player.controlJump && Player.releaseJump && System.Math.Abs(Player.velocity.X) > 2.5f)
				{
					screwSpeedDelay = 20;
				}
			}
			if (screwSpeedDelay > 0)
			{
				if (Player.jump > 1 && ((Player.velocity.Y < 0 && Player.gravDir == 1) || (Player.velocity.Y > 0 && Player.gravDir == -1)) && screwSpeedDelay >= 19 && somersault)
				{
					screwAttackSpeedEffect = 60;
				}
				screwSpeedDelay--;
			}
			if (screwAttackSpeedEffect > 0)
			{
				if (Player.controlLeft)
				{
					if (Player.velocity.X < -2 && Player.velocity.X > -8 * Player.moveSpeed)
					{
						Player.velocity.X -= 0.2f;
						Player.velocity.X -= (float)0.02 + ((Player.moveSpeed - 1f) / 10);
					}
				}
				else if (Player.controlRight)
				{
					if (Player.velocity.X > 2 && Player.velocity.X < 8 * Player.moveSpeed)
					{
						Player.velocity.X += 0.2f;
						Player.velocity.X += (float)0.02 + ((Player.moveSpeed - 1f) / 10);
					}
				}
				for (int i = 0; i < (screwAttackSpeedEffect / 20); i++)
				{
					if (screwProj != -1)
					{
						Projectile P = Main.projectile[screwProj];
						if (P.active && P.owner == Player.whoAmI && P.type == ModContent.ProjectileType<Content.Projectiles.ScrewAttackProj>())
						{
							Color color = new();
							int dust = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, DustID.Enchanted_Gold, -Player.velocity.X * 0.5f, -Player.velocity.Y * 0.5f, 100, color, 2f);
							Main.dust[dust].noGravity = true;
							if (i == ((screwAttackSpeedEffect / 20) - 1) && screwAttackSpeedEffect == 59)
							{
								SoundEngine.PlaySound(Sounds.Items.Weapons.ScrewAttackSpeed, Player.position);
							}
						}
					}
				}
				screwAttackSpeedEffect--;
			}
		}
		public void AddSpeedBoost(Player Player, int damage)
		{
			MPlayer mp = Player.GetModPlayer<MPlayer>();
			speedBoosting = (Math.Abs(Player.velocity.X) >= 6.85f || canWallJump) && speedBuildUp >= 120f && mp.SMoveEffect <= 0 && shineDirection == 0 && !(Player.mount.Active && !mp.morphBall);
			if ((Player.controlRight && Player.velocity.X > 0) || (Player.controlLeft && Player.velocity.X < 0))
			{
				speedBuildUp = Math.Min(speedBuildUp + 1f, 135f);
			}
			else if (!speedBoosting)
			{
				speedBuildUp = 0f;
			}
			Player.maxRunSpeed += (speedBuildUp * 0.06f);
			Player.runAcceleration *= 1.5f;
			Player.runSlowdown *= 1.5f;
			Player.accRunSpeed *= 1.5f;
			if (mp.speedBoosting)
			{
				Player.armorEffectDrawShadow = true;
				//MPlayer.jet = true;
				bool SpeedBoost = false;
				int SpeedBoostID = ModContent.ProjectileType<Content.Projectiles.SpeedBoost>();
				if (mp.ballstate)
				{
					SpeedBoostID = ModContent.ProjectileType<Content.Projectiles.SpeedBall>();
				}
				foreach (Projectile P in Main.projectile)
				{
					if (P.active && P.owner == Player.whoAmI && P.type == SpeedBoostID)
					{
						SpeedBoost = true;
						break;
					}
				}
				if (!SpeedBoost)
				{
					Projectile.NewProjectile(Player.GetSource_FromAI(), Player.position.X + Player.width / 2, Player.position.Y + Player.height / 2, 0, 0, SpeedBoostID, damage, 0, Player.whoAmI);
				}
			}
			#region shine-spark
			if (mp.speedBoosting)
			{
				if (Player.controlDown && Player.velocity.Y == 0)
				{
					shineCharge = 300;
					Player.velocity.X = 0;
					speedBuildUp = 0f;
				}
			}
			if (shineCharge > 0)
			{
				if (Player.controlJump && Player.releaseJump && !Player.controlRight && !Player.controlLeft && mp.statOverheat < mp.maxOverheat)
				{
					shineActive = true;
					if (!ballstate)
					{
						Player.mount.Dismount(Player);
					}
				}
				else
				{
					Lighting.AddLight(Player.Center, 1, 216 / 255, 0);
					shineSound++;
					if (shineSound > 11)
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.SpeedBoosterLoop, Player.position);
						shineSound = 0;
					}
				}
				shineCharge--;
			}
			if (shineActive)
			{
				shineSound = 0;
				Player.velocity.Y = 0;
				Player.maxFallSpeed = 0f;
				Player.velocity.X = 0;
				Player.moveSpeed = 0f;
				Player.maxRunSpeed = 0f;
				//Player.noItems = true;
				Player.controlUseItem = false;
				Player.controlUseTile = false;
				Player.controlMount = false;
				Player.releaseMount = false;
				Player.controlHook = false;
				Player.stairFall = true;
				if (Main.myPlayer == Player.whoAmI && !ballstate)
				{
					Player.mount.Dismount(Player);
				}
				for (int k = 0; k < 1000; k++)
				{
					if (Main.projectile[k].active && Main.projectile[k].owner == Player.whoAmI && Main.projectile[k].aiStyle == 7)
					{
						Main.projectile[k].Kill();
					}
				}
				//Player.controlJump = false;
				mp.rotation = 0;
				Player.armorEffectDrawShadow = true;
				if (shineDirection == 0)
				{
					shineDischarge++;
					Lighting.AddLight(Player.Center, 1, 216 / 255, 0);
				}
				if (CheckCollide(0f, 4f * Player.gravDir) && shineDischarge > 2)
				{
					Player.position.Y -= 2f * Player.gravDir;
				}
				//TODO this is clunky, is it not?
				if (shineDischarge >= 30 && mp.statOverheat < mp.maxOverheat)
				{
					shineCharge = 0;
					if (Player.controlRight && !Player.controlUp) //right
					{
						shineDirection = 1;
					}
					if (Player.controlRight && Player.controlUp) //right and up
					{
						shineDirection = 2;
					}
					if (Player.controlLeft && !Player.controlUp) //left
					{
						shineDirection = 3;
					}
					if (Player.controlLeft && Player.controlUp) //left and up
					{
						shineDirection = 4;
					}
					if (!Player.controlRight && !Player.controlLeft && !Player.controlDown) //default direction is up
					{
						shineDirection = 5;
					}
					if (Player.controlRight && Player.controlDown) //right and down
					{
						shineDirection = 6;
					}
					if (Player.controlLeft && Player.controlDown) //left and down
					{
						shineDirection = 7;
					}
					if (!Player.controlRight && !Player.controlLeft && Player.controlDown) //down
					{
						shineDirection = 8;
					}
				}
				Player.fallStart = (int)(Player.Center.Y / 16f);
			}

			switch (shineDirection)
			{
				case 1: //right
					Player.velocity.X = Math.Max(2.5f * Player.accRunSpeed, 20);
					Player.velocity.Y = 0;
					Player.maxFallSpeed = 0f;
					Player.direction = 1;
					shineDischarge = 0;
					Player.controlLeft = false;
					//Player.controlUp = true;
					break;

				case 2: //right and up
					Player.velocity.X = Math.Max(2.5f * Player.accRunSpeed, 20);
					Player.velocity.Y = Math.Min(-2.5f * Player.accRunSpeed * Player.gravDir, -20);
					Player.maxFallSpeed = 0f;
					Player.direction = 1;
					shineDischarge = 0;
					Player.controlLeft = false;
					break;

				case 3: //left
					Player.velocity.X = Math.Min(-2.5f * Player.accRunSpeed, -20);
					Player.velocity.Y = 0;
					Player.maxFallSpeed = 0f;
					Player.direction = -1;
					shineDischarge = 0;
					Player.controlRight = false;
					//Player.controlUp = true;
					break;

				case 4: //left and up
					Player.velocity.X = Math.Min(-2.5f * Player.accRunSpeed, -20);
					Player.velocity.Y = Math.Min(-2.5f * Player.accRunSpeed * Player.gravDir, -20);
					Player.maxFallSpeed = 0f;
					Player.direction = -1;
					shineDischarge = 0;
					Player.controlRight = false;
					break;

				case 5: //up
					Player.velocity.X = 0;
					Player.velocity.Y = Math.Min(-2.5f * Player.accRunSpeed * Player.gravDir, -20);
					Player.maxFallSpeed = 0f;
					shineDischarge = 0;
					if (Player.miscCounter % 5 == 0 && !ballstate)
					{
						Player.direction *= -1;
					}
					Player.controlLeft = false;
					Player.controlRight = false;
					break;

				case 6: //right and down
					Player.velocity.X = Math.Max(2.5f * Player.accRunSpeed, 20);
					Player.velocity.Y = Math.Max(2.5f * Player.accRunSpeed * Player.gravDir, 20);
					Player.maxFallSpeed = Math.Max(2.5f* Player.accRunSpeed, 20);
					Player.direction = 1;
					shineDischarge = 0;
					Player.controlLeft = false;
					Player.GoingDownWithGrapple = true;
					break;

				case 7: //left and down
					Player.velocity.X = Math.Min(-2.5f * Player.accRunSpeed, -20);
					Player.velocity.Y = Math.Max(2.5f * Player.accRunSpeed * Player.gravDir, 20);
					Player.maxFallSpeed = Math.Max(2.5f * Player.accRunSpeed, 20);
					Player.direction = -1;
					shineDischarge = 0;
					Player.controlRight = false;
					Player.GoingDownWithGrapple = true;
					break;

				case 8: //down
					Player.velocity.X = 0;
					Player.velocity.Y = Math.Max(2.5f * Player.accRunSpeed * Player.gravDir, 20);
					Player.maxFallSpeed = Math.Max(2.5f * Player.accRunSpeed, 20);
					shineDischarge = 0;
					if (Player.miscCounter % 4 == 0 && !ballstate)
					{
						Player.direction *= -1;
					}
					Player.controlLeft = false;
					Player.controlRight = false;
					Player.GoingDownWithGrapple = true;
					break;
			}
			if (shineDirection != 0)
			{
				mp.statOverheat += 0.5f;
				shineCharge = 0;
				bool shineSpark = false;
				int ShineSparkID = ModContent.ProjectileType<Content.Projectiles.ShineSpark>();
				if (mp.ballstate)
				{
					ShineSparkID = ModContent.ProjectileType<Content.Projectiles.ShineBall>();
				}
				foreach (Terraria.Projectile P in Main.projectile)
				{
					if (P.active && P.owner == Player.whoAmI && P.type == ShineSparkID)
					{
						shineSpark = true;
						break;
					}
				}
				if (!shineSpark)
				{
					proj = Projectile.NewProjectile(Player.GetSource_FromAI(), Player.position.X + Player.width / 2, Player.position.Y + Player.height / 2, 0, 0, ShineSparkID, damage, 0, Player.whoAmI);
				}
			}

			//cancel shine-spark
			//stop right movement
			if (shineDirection == 1 && (CheckCollide(Player.velocity.X, 0f) || mp.statOverheat >= mp.maxOverheat ||
			(Player.position.X + (float)Player.width) > (Main.rightWorld - 640f - 48f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop up and right movement
			if (shineDirection == 2 && (CheckCollide(Player.velocity.X, Player.velocity.Y) || CheckCollide(Player.velocity.X, 0f) || CheckCollide(0f, Player.velocity.Y) || mp.statOverheat >= mp.maxOverheat ||
			(Player.position.X + (float)Player.width) > (Main.rightWorld - 640f - 48f) || Player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop left movement
			if (shineDirection == 3 && (CheckCollide(Player.velocity.X, 0f) || mp.statOverheat >= mp.maxOverheat ||
			Player.position.X < (Main.leftWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop left and up movement
			if (shineDirection == 4 && (CheckCollide(Player.velocity.X, Player.velocity.Y) || CheckCollide(Player.velocity.X, 0f) || CheckCollide(0f, Player.velocity.Y) || mp.statOverheat >= mp.maxOverheat ||
			Player.position.X < (Main.leftWorld + 640f + 32f) || Player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop up movement
			if (shineDirection == 5 && (CheckCollide(0f, Player.velocity.Y) || mp.statOverheat >= mp.maxOverheat ||
			Player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop down and right movement
			if (shineDirection == 6 && (CheckCollide(Player.velocity.X, Player.velocity.Y) || CheckCollide(Player.velocity.X, 0f) || CheckCollide(0f, Player.velocity.Y) || mp.statOverheat >= mp.maxOverheat ||
			(Player.position.X + (float)Player.width) > (Main.rightWorld - 640f - 48f) || (Player.position.Y + Player.height) > (Main.bottomWorld - 640f - 48f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop down and left movement
			if (shineDirection == 7 && (CheckCollide(Player.velocity.X, Player.velocity.Y) || CheckCollide(Player.velocity.X, 0f) || CheckCollide(0f, Player.velocity.Y) || mp.statOverheat >= mp.maxOverheat ||
			Player.position.X < (Main.leftWorld + 640f + 32f) || (Player.position.Y + Player.height) > (Main.bottomWorld - 640f - 48f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop down movement
			if (shineDirection == 8 && (CheckCollide(0f, Player.velocity.Y) || mp.statOverheat >= mp.maxOverheat ||
			(Player.position.Y + Player.height) > (Main.bottomWorld - 640f - 48f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if (mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}

			//stop any movement
			if (shineDirection != 0 && Player.controlJump && Player.releaseJump)
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;

				if (Player.velocity.Y >= 0)
				{
					Player.velocity.Y = 1E-05f;
					Player.jump = 1;
				}
				if (Player.velocity.X != 0)
				{
					mp.canSomersault = true;
				}

				Player.releaseJump = false;
			}
			#endregion
		}
	}
}
