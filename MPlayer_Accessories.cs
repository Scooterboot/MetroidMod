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

using MetroidMod.NPCs;
using MetroidMod.Items;
using MetroidMod.Common.Worlds;

namespace MetroidMod
{
	public partial class MPlayer : ModPlayer  
	{
		public bool powerGrip = false;
		public bool isGripping = false;
		public int reGripTimer = 0;
		public int gripDir = 1;
		
		public bool speedBooster = false;
		public bool speedBoosting = false;
		float speedBuildUp = 0f;
		public bool shineActive = false;
		public int shineDirection = 0;
		public int shineCharge = 0;
		int shineDischarge = 0;
		int proj = -1;
		int shineSound = 0;
		public int speedBoostDmg = 0;
		
		public bool somersault = false;
		public bool canSomersault = false;
		public bool disableSomersault = false;
		public float rotation = 0.0f;
		public float rotateSpeed = 0.05f;
		public float rotateSpeed2 = 50f;
		public float rotateCountX = 0.05f;
		public float rotateCountY = 0.05f;
		int itemRotTweak = 0;
		
		public bool hiJumpBoost = false;
		public bool spaceJumpBoots = false;
		public bool spaceJumped = false;
		public bool spaceJump = false;
		public static float maxSpaceJumps = 120;
		public float statSpaceJumps = maxSpaceJumps;
		public int spaceJumpsRegenDelay = 0;
		
		public bool screwAttack = false;
		public int screwAttackSpeedEffect = 0;
		public int screwSpeedDelay = 0;
		public int screwAttackDmg = 0;
		
		public int reserveTanks = 0;
		public int reserveHearts = 0;
		public int reserveHeartsValue = 20;
		
		public void ResetEffects_Accessories()
		{
			powerGrip = false;
			
			speedBooster = false;
			speedBoosting = false;
			speedBoostDmg = 0;
			
			disableSomersault = false;
			
			hiJumpBoost = false;
			spaceJumpBoots = false;
			spaceJump = false;
			
			screwAttack = false;
			screwAttackDmg = 0;
			
			reserveTanks = 0;
			reserveHeartsValue = 20;
		}
		public void PreUpdate_Accessories()
		{
			Player P = player;
			somersault = (!P.dead && !disableSomersault && (SMoveEffect > 0 || canSomersault) && !P.mount.Active && P.velocity.Y != 0 /*&& P.velocity.X != 0*/ && !P.sliding && !P.pulley && !isGripping && (P.itemAnimation == 0 || statCharge >= 30) && P.grappling[0] <= -1 && grapplingBeam <= -1 && shineDirection == 0 && !shineActive && !ballstate && (((P.wingsLogic != 0 || P.rocketBoots != 0 || P.carpet) && (!P.controlJump || (!P.canRocket && !P.rocketRelease && P.wingsLogic == 0) || (P.wingTime <= 0 && P.rocketTime <= 0 && P.carpetTime <= 0))) || (P.wingsLogic == 0 && P.rocketBoots == 0 && !P.carpet)) && !P.sandStorm);
			somersault &= !(P.rocketDelay <= 0 && P.wingsLogic > 0 && P.controlJump && P.velocity.Y > 0f && P.wingTime <= 0);
			
			if(player.velocity.Y == 0 || player.sliding || (player.autoJump && player.justJumped) || player.grappling[0] >= 0 || grapplingBeam >= 0)
			{
				statSpaceJumps = maxSpaceJumps;
			}

			if(statSpaceJumps < maxSpaceJumps && spaceJumpsRegenDelay <= 0)
			{
				statSpaceJumps++;
			}
			else if(spaceJumpsRegenDelay <= 0)
			{
				statSpaceJumps = maxSpaceJumps;
			}
			if(spaceJumpsRegenDelay > 0)
			{
				spaceJumpsRegenDelay--;
			}
			
			if(!player.mount.Active)
			{
				if(somersault)
				{
					float rotMax = (float)Math.PI/8;
					if(spaceJump)// && SMoveEffect <= 0)
					{
						rotMax = (float)Math.PI/4;
					}
					rotation += MathHelper.Clamp((rotateCountX + rotateCountY) * player.direction * player.gravDir,-rotMax,rotMax);
					if(rotation > (Math.PI*2))
					{
						rotation -= (float)(Math.PI*2);
					}
					if(rotation < -(Math.PI*2))
					{
						rotation += (float)(Math.PI*2);
					}
					player.fullRotation = rotation;
					player.fullRotationOrigin = new Vector2((float)player.width/2,(float)player.height*0.55f);
					if(player.gravDir == -1)
					{
						player.fullRotationOrigin.Y = (float)player.height*0.45f;
					}
					itemRotTweak = 2;
				}
				else if(shineDirection == 2 || shineDirection == 4)
				{
					rotation = 0.05f * player.direction * player.gravDir;
					player.fullRotation = rotation;
					player.fullRotationOrigin = player.Center - player.position;
				}
				else if(shineDirection == 1 || shineDirection == 3)
				{
					rotation = ((float)Math.PI/4f) * player.direction * player.gravDir;
					player.fullRotation = rotation;
					player.fullRotationOrigin = player.Center - player.position;
				}
				else
				{
					rotation = 0f;
					player.fullRotation = 0f;
				}
			}
			else
			{
				rotation = 0f;
				player.fullRotation = 0f;
			}
			if(spaceJump)
			{
				rotateSpeed = 0.2f;
				rotateSpeed2 = 20f;
			}
			else
			{
				rotateSpeed = 0.08f;
				rotateSpeed2 = 40f;
			}
			if(player.velocity.Y > 1)
			{
				rotateCountY = rotateSpeed + player.velocity.Y/rotateSpeed2;
			}
			else if(player.velocity.Y < -1)
			{
				rotateCountY = rotateSpeed + (player.velocity.Y/rotateSpeed2)*(-1f);
			}
			else
			{
				rotateCountY = rotateSpeed;
			}
			if(player.velocity.X > 1)
			{
				rotateCountX = rotateSpeed + player.velocity.X/rotateSpeed2;
			}
			else if(player.velocity.X < -1)
			{
				rotateCountX = rotateSpeed + (player.velocity.X/rotateSpeed2)*(-1f);
			}
			else
			{
				rotateCountX = rotateSpeed;
			}
		}
		bool sbFlag = false;
		public void PostUpdateMiscEffects_Accessories()
		{
			GripMovement();
			
			if(speedBooster)
			{
				AddSpeedBoost(player, speedBoostDmg);
				if(player.controlJump)
				{
					if(player.velocity.Y == 0)
					{
						sbFlag = true;
					}
				}
				else
				{
					sbFlag = false;
				}
				if(sbFlag)
				{
					if(player.velocity.X <= -4f && player.controlLeft)
					{
						player.jumpSpeedBoost += Math.Abs(player.velocity.X/4f);
					}
					else if(player.velocity.X >= 4f && player.controlRight)
					{
						player.jumpSpeedBoost += Math.Abs(player.velocity.X/4f);
					}
				}
			}
			else
			{
				sbFlag = false;
			}
			
			if(spaceJumpBoots || spaceJump || screwAttack)
			{
				AddSpaceJumpBoots(player);
				if(spaceJump)
				{
					AddSpaceJump(player);
				}
				if(screwAttack)
				{
					AddScrewAttack(player,screwAttackDmg);
				}
			}
			
			if(shineActive || shineDirection != 0 || (spiderball && CurEdge != Edge.None))
			{
				//player.gravity = 0f;
				float num3 = player.gravity;
				if (player.slowFall)
				{
					if (player.controlUp)
					{
						num3 = player.gravity / 10f * player.gravDir;
					}
					else
					{
						num3 = player.gravity / 3f * player.gravDir;
					}
				}
				player.velocity.Y -= num3;
			}
		}
		public void PostUpdateRunSpeeds_Accessories()
		{
			if(hiJumpBoost)
			{
				Player.jumpHeight += 5;
				Player.jumpSpeed += 1.5f;
			}
		}
		public void PostUpdate_Accessories()
		{
			if (player.itemAnimation > 0)
			{
				if(itemRotTweak > 0)
				{
					float MY = Main.mouseY + Main.screenPosition.Y;
					float MX = Main.mouseX + Main.screenPosition.X;
					if (player.gravDir == -1f)
					{
						MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
					Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
					player.ChangeDir(Math.Sign(MX-oPos.X));
					player.itemRotation = (float)Math.Atan2((MY-oPos.Y)*player.direction,(MX-oPos.X)*player.direction) - player.fullRotation;
					itemRotTweak--;
				}
			}
			else
			{
				itemRotTweak = 0;
			}
			
			if(!speedBooster)
			{
				speedBuildUp = 0;
				shineCharge = 0;
				shineSound = 0;
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
			}
			if(!spaceJumpBoots && !spaceJump)
			{
				canSomersault = false;
			}
			if(!screwAttack)
			{
				screwAttackSpeedEffect = 0;
				screwSpeedDelay = 0;
			}
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (reserveTanks > 0 && reserveHearts > 0)
			{
				if (player.statLifeMax < reserveHearts * reserveHeartsValue)
				{
					player.statLife = player.statLifeMax;
					reserveHearts -= (int)Math.Ceiling((double)player.statLifeMax / reserveHeartsValue);
				}
				else
				{
					player.statLife = reserveHearts * reserveHeartsValue;
					reserveHearts = 0;
				}
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/MissilesReplenished"));
				return false;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
		public void GripMovement()
		{
			gripDir = player.direction;
			isGripping = false;
			reGripTimer--;
			if (reGripTimer <= 0 && powerGrip && !player.mount.Active && ((!player.controlRight && gripDir == -1) || (!player.controlLeft && gripDir == 1)))
			{
				bool flag = false;
				float num = player.position.X;
				if (gripDir == 1)
				{
					num += (float)player.width;
				}
				num += (float)gripDir;
				float num2 = player.position.Y + 8f;
				if (player.gravDir < 0f)
				{
					num2 = player.position.Y + (float)player.height - 8f;
				}
				num = MathHelper.Clamp(num / 16f, 0, Main.maxTilesX-1);
				num2 = MathHelper.Clamp(num2 / 16f, 0, Main.maxTilesY-1);
				/*
				//Allow gripping onto non solid tiles
				if (Main.tile[(int)num, (int)num2].active() && !Main.tile[(int)num, (int)num2].inActive() && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.Rope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.SilkRope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.VineRope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.WebRope && Main.tile[(int)num, (int)num2].type != Terraria.ID.TileID.Chain && (Main.tile[(int)num, (int)num2 - (int)player.gravDir].inActive() || !Main.tile[(int)num, (int)num2 - (int)player.gravDir].active() || (Main.tile[(int)num, (int)num2 - 1].bottomSlope() && player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].topSlope() && player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num, (int)num2 - (int)player.gravDir].type] || Main.tileSolidTop[Main.tile[(int)num, (int)num2 - (int)player.gravDir].type] || (Main.tile[(int)num, (int)num2].halfBrick() && player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].halfBrick() && player.gravDir == -1) || Main.tile[(int)num, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
				{
					flag = true;
				}
				float num3 = player.Center.X / 16f;
				if (Main.tile[(int)num3, (int)num2].active() && !Main.tile[(int)num3, (int)num2].inActive() && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.Rope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.SilkRope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.VineRope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.WebRope && Main.tile[(int)num3, (int)num2].type != Terraria.ID.TileID.Chain && (Main.tile[(int)num3, (int)num2 - (int)player.gravDir].inActive() || !Main.tile[(int)num3, (int)num2 - (int)player.gravDir].active() || (Main.tile[(int)num3, (int)num2 - 1].bottomSlope() && player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].topSlope() && player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num3, (int)num2 - (int)player.gravDir].type] || Main.tileSolidTop[Main.tile[(int)num3, (int)num2 - (int)player.gravDir].type] || (Main.tile[(int)num3, (int)num2].halfBrick() && player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].halfBrick() && player.gravDir == -1) || Main.tile[(int)num3, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
				{
					flag = true;
				}
				*/
				if (Main.tile[(int)num, (int)num2].active() && !Main.tile[(int)num, (int)num2].inActive() && Main.tileSolid[Main.tile[(int)num, (int)num2].type] && !Main.tileSolidTop[Main.tile[(int)num, (int)num2].type] && (Main.tile[(int)num, (int)num2 - (int)player.gravDir].inActive() || !Main.tile[(int)num, (int)num2 - (int)player.gravDir].active() || (Main.tile[(int)num, (int)num2 - 1].bottomSlope() && player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].topSlope() && player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num, (int)num2 - (int)player.gravDir].type] || Main.tileSolidTop[Main.tile[(int)num, (int)num2 - (int)player.gravDir].type] || (Main.tile[(int)num, (int)num2].halfBrick() && player.gravDir == 1) || (Main.tile[(int)num, (int)num2 + 1].halfBrick() && player.gravDir == -1) || Main.tile[(int)num, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
				{
					flag = true;
				}
				if (Main.tile[(int)num, (int)num2].type == mod.TileType("GripLedge") && !Main.tile[(int)num, (int)num2].inActive() && Main.tile[(int)num, (int)num2].active())
				{
					flag = true;
				}

				if (MWorld.mBlockType[(int)num, (int)num2] == 1 && Main.tile[(int)num, (int)num2].active() && !Main.tile[(int)num, (int)num2].inActive())
				{
					Wiring.DeActive((int)num, (int)num2);
					Vector2 pos = new Vector2((int)num * 16, (int)num2 * 16);
					if (Main.tile[(int)num, (int)num2].inActive())
					{
						Main.PlaySound(2, pos, 51);
						for (int d = 0; d < 4; d++)
						{
							Dust.NewDust(pos, 16, 16, 1);
						}
						flag = false;
					}
				}
				float num3 = player.Center.X / 16f;
				if (Main.tile[(int)num3, (int)num2].active() && !Main.tile[(int)num3, (int)num2].inActive() && Main.tileSolid[Main.tile[(int)num3, (int)num2].type] && !Main.tileSolidTop[Main.tile[(int)num3, (int)num2].type] && (Main.tile[(int)num3, (int)num2 - (int)player.gravDir].inActive() || !Main.tile[(int)num3, (int)num2 - (int)player.gravDir].active() || (Main.tile[(int)num3, (int)num2 - 1].bottomSlope() && player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].topSlope() && player.gravDir == -1) || !Main.tileSolid[Main.tile[(int)num3, (int)num2 - (int)player.gravDir].type] || Main.tileSolidTop[Main.tile[(int)num3, (int)num2 - (int)player.gravDir].type] || (Main.tile[(int)num3, (int)num2].halfBrick() && player.gravDir == 1) || (Main.tile[(int)num3, (int)num2 + 1].halfBrick() && player.gravDir == -1) || Main.tile[(int)num3, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
				{
					flag = true;
				}
				if (Main.tile[(int)num3, (int)num2].type == mod.TileType("GripLedge") && !Main.tile[(int)num3, (int)num2].inActive() && Main.tile[(int)num3, (int)num2].active())
				{
					flag = true;
				}

				if (MWorld.mBlockType[(int)num3, (int)num2] == 1 && Main.tile[(int)num3, (int)num2].active() && !Main.tile[(int)num3, (int)num2].inActive())
				{
					Wiring.DeActive((int)num3, (int)num2);
					Vector2 pos = new Vector2((int)num3 * 16, (int)num2 * 16);
					if (Main.tile[(int)num3, (int)num2].inActive())
					{
						Main.PlaySound(2, pos, 51);
						for (int d = 0; d < 4; d++)
						{
							Dust.NewDust(pos, 16, 16, 1);
						}
						flag = false;
					}
				}
		
				if (flag && ((player.velocity.Y > 0f && player.gravDir == 1f) || (player.velocity.Y < player.gravity && player.gravDir == -1f)))
				{
					if (!player.controlDown)
					{
						reGripTimer = 0;
						player.fullRotation = 0;
						player.position.Y = ((int)num2 * 16) - 8;
						if (player.gravDir == 1 && (Main.tile[(int)num, (int)num2].halfBrick() || Main.tile[(int)num, (int)num2].type == Terraria.ID.TileID.MinecartTrack || Main.tile[(int)num3, (int)num2].halfBrick() || Main.tile[(int)num3, (int)num2].type == Terraria.ID.TileID.MinecartTrack))
						{
							player.position.Y += 8;
						}
						if (player.gravDir == -1)
						{
							player.position.Y -= 12;
						}
						float grav = player.gravity;
						if (player.slowFall)
						{
							if (player.controlUp)
							{
								grav = player.gravity / 10f * player.gravDir;
							}
							else
							{
								grav = player.gravity / 3f * player.gravDir;
							}
						}
						if (player.velocity.X > 2)
						{
							player.velocity.X = 2;
						}
						if (player.velocity.X < -2)
						{
							player.velocity.X = -2;
						}
						player.fallStart = (int)(player.position.Y / 16f);
						if (player.doubleJumpCloud)
						{
							player.jumpAgainCloud = true;
						}
						if (player.doubleJumpSandstorm)
						{
							player.jumpAgainSandstorm = true;
						}
						if (player.doubleJumpBlizzard)
						{
							player.jumpAgainBlizzard = true;
						}
						if (player.doubleJumpFart)
						{
							player.jumpAgainFart = true;
						}
						if (player.doubleJumpSail)
						{
							player.jumpAgainSail = true;
						}
						if (player.doubleJumpUnicorn)
						{
							player.jumpAgainUnicorn = true;
						}
						if (player.controlJump)
						{
							player.velocity.Y = -Player.jumpSpeed * player.gravDir;
							player.jump = Player.jumpHeight;
							canSomersault = true;
						}
						else if (player.controlUp)
						{
							player.velocity.Y = -6 * player.gravDir;
							reGripTimer = 10;
						}
						else
						{
							player.velocity.Y = (-grav + 1E-05f) * player.gravDir;
						}
					}
					isGripping = true;
				}
				if (isGripping && player.controlDown)
				{
					isGripping = false;
					reGripTimer = 10;
				}
			}
		}
		public void AddSpaceJump(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if(mp.statSpaceJumps >= 15 && player.grappling[0] == -1  && mp.spaceJumped && !player.jumpAgainCloud && !player.jumpAgainBlizzard && !player.jumpAgainSandstorm && !player.jumpAgainFart && player.jump == 0 && player.velocity.Y != 0f && player.rocketTime == 0 && player.wingTime == 0f && !player.mount.Active)
			{
				if(player.controlJump && player.releaseJump && player.velocity.Y != 0 && mp.spaceJumped)
				{
					player.jump = Player.jumpHeight;
					player.velocity.Y = -Player.jumpSpeed * player.gravDir;
					mp.statSpaceJumps -= 15;
					mp.spaceJumpsRegenDelay = 25;
				}
			}
		}
		public void AddSpaceJumpBoots(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if(player.velocity.Y == 0f || player.sliding || (player.autoJump && player.justJumped) || player.grappling[0] >= 0 || mp.grapplingBeam >= 0)
			{
				mp.spaceJumped = false;
				if(player.velocity.X != 0 || player.sliding)
				{
					mp.canSomersault = true;
				}
				else if(!player.sliding)
				{
					mp.canSomersault = false;
				}
			}
			else if((!player.mount.Active || !player.mount.BlockExtraJumps) && player.controlJump && player.releaseJump && !mp.spaceJumped && player.grappling[0] == -1 && mp.grapplingBeam <= -1 && player.jump <= 0)
			{
				int num167 = player.height;
				if (player.gravDir == -1f)
				{
					num167 = 4;
				}
				Main.PlaySound(2,(int)player.position.X,(int)player.position.Y,20);
				for (int num168 = 0; num168 < 8; num168++)
				{
					int type4 = 6;
					float scale2 = 2.5f;
					int alpha2 = 100;
					if (num168 <= 3)
					{
						int num169 = Dust.NewDust(new Vector2(player.position.X - 4f, player.position.Y + (float)num167 - 10f), 8, 8, type4, 0f, 0f, alpha2, default(Color), scale2);
						Main.dust[num169].noGravity = true;
						Main.dust[num169].velocity.X = Main.dust[num169].velocity.X * 1f - 2f - player.velocity.X * 0.3f;
						Main.dust[num169].velocity.Y = Main.dust[num169].velocity.Y * 1f + 2f * player.gravDir - player.velocity.Y * 0.3f;
					}
					else
					{
						int num170 = Dust.NewDust(new Vector2(player.position.X + (float)player.width - 4f, player.position.Y + (float)num167 - 10f), 8, 8, type4, 0f, 0f, alpha2, default(Color), scale2);
						Main.dust[num170].noGravity = true;
						Main.dust[num170].velocity.X = Main.dust[num170].velocity.X * 1f + 2f - player.velocity.X * 0.3f;
						Main.dust[num170].velocity.Y = Main.dust[num170].velocity.Y * 1f + 2f * player.gravDir - player.velocity.Y * 0.3f;
					}
				}
				mp.spaceJumped = true;
				mp.canSomersault = true;
				player.jump = Player.jumpHeight;
				player.velocity.Y = -Player.jumpSpeed * player.gravDir;
				player.canRocket = false;
				player.rocketRelease = false;
				player.fallStart = (int)(player.Center.Y / 16f);
			}
			if(mp.spaceJumped)
			{
				mp.canSomersault = true;
			}
		}
		int screwProj = -1;
		public void AddScrewAttack(Player player, int damage)
		{
			if(somersault)
			{
				bool flag = false;
				player.longInvince = true;
				int screwAttackID = mod.ProjectileType("ScrewAttackProj");
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == screwAttackID)
					{
						flag = true;
						break;
					}
				}
				if(!flag)
				{
					screwProj = Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,screwAttackID,damage,0,player.whoAmI);
				}
			}
			if(screwSpeedDelay <= 0 && !ballstate && player.grappling[0] == -1 && player.velocity.Y != 0f && !player.mount.Active)
			{
				if(player.controlJump && player.releaseJump && System.Math.Abs(player.velocity.X) > 2.5f)
				{
					screwSpeedDelay = 20;
				}
			}
			if(screwSpeedDelay > 0)
			{
				if(player.jump > 1 && ((player.velocity.Y < 0 && player.gravDir == 1) || (player.velocity.Y > 0 && player.gravDir == -1)) && screwSpeedDelay >= 19 && somersault)
				{
					screwAttackSpeedEffect = 60;
				}
				screwSpeedDelay--;
			}
			if(screwAttackSpeedEffect > 0)
			{
				if (player.controlLeft)
				{
					if (player.velocity.X < -2 && player.velocity.X > -8*player.moveSpeed)
					{
						player.velocity.X -= 0.2f;
						player.velocity.X -= (float) 0.02+((player.moveSpeed-1f)/10);
					}
				}
				else if (player.controlRight)
				{
					if (player.velocity.X > 2 && player.velocity.X < 8*player.moveSpeed)
					{
						player.velocity.X += 0.2f;
						player.velocity.X += (float) 0.02+((player.moveSpeed-1f)/10);
					}
				}
				for(int i = 0; i < (screwAttackSpeedEffect/20); i++)
				{
					if(screwProj != -1)
					{
						Projectile P = Main.projectile[screwProj];
						if(P.active && P.owner == player.whoAmI && P.type == mod.ProjectileType("ScrewAttackProj"))
						{
							Color color = new Color();
							int dust = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 57, -player.velocity.X * 0.5f, -player.velocity.Y * 0.5f, 100, color, 2f);
							Main.dust[dust].noGravity = true;
							if(i == ((screwAttackSpeedEffect/20)-1) && screwAttackSpeedEffect == 59)
							{
								Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/ScrewAttackSpeedSound"));
							}
						}
					}
				}
				screwAttackSpeedEffect--;
			}
		}
		public void AddSpeedBoost(Player player, int damage)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			speedBoosting = (Math.Abs(player.velocity.X) >= 6.85f && speedBuildUp >= 120f && mp.SMoveEffect <= 0 && shineDirection == 0);
			if((player.controlRight && player.velocity.X > 0) || (player.controlLeft && player.velocity.X < 0))
			{
				speedBuildUp = Math.Min(speedBuildUp + 1f, 135f);
			}
			else if(!speedBoosting)
			{
				speedBuildUp = 0f;
			}
			player.maxRunSpeed += (speedBuildUp*0.06f);
			if(mp.speedBoosting)
			{
				player.armorEffectDrawShadow = true;
				//MPlayer.jet = true;
				bool SpeedBoost = false;
				int SpeedBoostID = mod.ProjectileType("SpeedBoost");
				if(mp.ballstate)
				{
					SpeedBoostID = mod.ProjectileType("SpeedBall");
				}
				foreach(Terraria.Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == SpeedBoostID)
					{
						SpeedBoost = true;
						break;
					}
				}
				if(!SpeedBoost)
				{
					int SpBoost = Terraria.Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,SpeedBoostID,damage,0,player.whoAmI);
				}
			}
		#region shine-spark
			if(mp.speedBoosting)
			{
				if(player.controlDown && player.velocity.Y == 0)
				{
					shineCharge = 300;
					player.velocity.X = 0;
					speedBuildUp = 0f;
				}
			}
			if(shineCharge > 0)
			{
				if(player.controlJump && player.releaseJump && !player.controlRight && !player.controlLeft && mp.statOverheat < mp.maxOverheat)
				{
					shineActive = true;
					if(!ballstate)
					{
						player.mount.Dismount(player);
					}
				}
				else
				{
					Lighting.AddLight(player.Center, 1, 216/255, 0);
					shineSound++;
					if(shineSound > 11)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpeedBoosterLoop"));
						shineSound = 0;
					}
				}
				shineCharge--;
			}
			if(shineActive)
			{
				shineSound = 0;
				player.velocity.Y = 0;
				player.maxFallSpeed = 0f;
				player.velocity.X = 0;
				player.moveSpeed = 0f;
				player.maxRunSpeed = 0f;
				//player.noItems = true;
				player.controlUseItem = false;
				player.controlUseTile = false;
				player.controlMount = false;
				player.releaseMount = false;
				player.controlHook = false;
				player.stairFall = true;
				if (Main.myPlayer == player.whoAmI && !ballstate)
				{
					player.mount.Dismount(player);
				}
				for (int k = 0; k < 1000; k++)
				{
					if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].aiStyle == 7)
					{
						Main.projectile[k].Kill();
					}
				}
				//player.controlJump = false;
				mp.rotation = 0;
				player.armorEffectDrawShadow = true;
				if(shineDirection == 0)
				{
					shineDischarge++;
					Lighting.AddLight(player.Center, 1, 216/255, 0);
				}
				if(CheckCollide(0f,4f*player.gravDir) && shineDischarge > 2)
				{
					player.position.Y -= 2f*player.gravDir;
				}
				if(shineDischarge >= 30 && mp.statOverheat < mp.maxOverheat)
				{
					shineCharge = 0;
					if(player.controlRight && !player.controlUp) //right
					{
						shineDirection = 1;
					}
					if(player.controlRight && player.controlUp) //right and up
					{
						shineDirection = 2;
					}
					if(player.controlLeft && !player.controlUp) //left
					{
						shineDirection = 3;
					}
					if(player.controlLeft && player.controlUp) //left and up
					{
						shineDirection = 4;
					}
					if(!player.controlRight && !player.controlLeft) //default direction is up
					{
						shineDirection = 5;
					}
				}
				player.fallStart = (int)(player.Center.Y / 16f);
			}

			if(shineDirection == 1) //right
			{
				player.velocity.X = 20;
				player.velocity.Y = 0;
				player.maxFallSpeed = 0f;
				player.direction = 1;
				shineDischarge = 0;
				player.controlLeft = false;
				//player.controlUp = true;
			}
			if(shineDirection == 2) //right and up
			{
				player.velocity.X = 20;
				player.velocity.Y = -20f*player.gravDir;
				player.maxFallSpeed = 0f;
				player.direction = 1;
				shineDischarge = 0;
				player.controlLeft = false;
			}
			if(shineDirection == 3) //left
			{
				player.velocity.X = -20;
				player.velocity.Y = 0;
				player.maxFallSpeed = 0f;
				player.direction = -1;
				shineDischarge = 0;
				player.controlRight = false;
				//player.controlUp = true;
			}
			if(shineDirection == 4) //left and up
			{
				player.velocity.X = -20;
				player.velocity.Y = -20*player.gravDir;
				player.maxFallSpeed = 0f;
				player.direction = -1;
				shineDischarge = 0;
				player.controlRight = false;
			}
			if(shineDirection == 5) //up
			{
				player.velocity.X *= 0f;
				player.velocity.Y = -20*player.gravDir;
				player.maxFallSpeed = 0f;
				shineDischarge = 0;
				if (player.miscCounter % 4 == 0 && !ballstate)
				{
					player.direction *= -1;
				}
				player.controlLeft = false;
				player.controlRight = false;
			}

			if(shineDirection != 0)
			{
				mp.statOverheat += 0.5f;
				shineCharge = 0;
				bool shineSpark = false;
				int ShineSparkID = mod.ProjectileType("ShineSpark");
				if(mp.ballstate)
				{
					ShineSparkID = mod.ProjectileType("ShineBall");
				}
				foreach(Terraria.Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == ShineSparkID)
					{
						shineSpark = true;
						break;
					}
				}
				if(!shineSpark)
				{
					proj = Terraria.Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,ShineSparkID,damage,0,player.whoAmI);
				}
			}

		//cancel shine-spark
			//stop right movement
			if(shineDirection == 1 && (CheckCollide(player.velocity.X,0f) || mp.statOverheat >= mp.maxOverheat || 
			(player.position.X + (float)player.width) > (Main.rightWorld - 640f - 48f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop up and right movement
			if(shineDirection == 2 && (CheckCollide(player.velocity.X,player.velocity.Y) || CheckCollide(player.velocity.X,0f) || CheckCollide(0f,player.velocity.Y) || mp.statOverheat >= mp.maxOverheat || 
			(player.position.X + (float)player.width) > (Main.rightWorld - 640f - 48f) || player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop left movement
			if(shineDirection == 3 && (CheckCollide(player.velocity.X,0f) || mp.statOverheat >= mp.maxOverheat || 
			player.position.X < (Main.leftWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop left and up movement
			if(shineDirection == 4 && (CheckCollide(player.velocity.X,player.velocity.Y) || CheckCollide(player.velocity.X,0f) || CheckCollide(0f,player.velocity.Y) || mp.statOverheat >= mp.maxOverheat || 
			player.position.X < (Main.leftWorld + 640f + 32f) || player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop up movement
			if(shineDirection == 5 && (CheckCollide(0f,player.velocity.Y) || mp.statOverheat >= mp.maxOverheat || 
			player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			
			//stop any movement
			if(shineDirection != 0 && player.controlJump && player.releaseJump)
			{
				shineDirection = 0;
				shineDischarge = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				
				if(player.velocity.Y >= 0)
				{
					player.velocity.Y = 1E-05f;
					player.jump = 1;
				}
				if(player.velocity.X != 0)
				{
					mp.canSomersault = true;
				}
				
				player.releaseJump = false;
			}
		#endregion
		}
	}
}