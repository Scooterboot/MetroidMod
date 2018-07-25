using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace MetroidMod
{
    public class MPlayer : ModPlayer  
    {
        public int style;
		public int r = 255;
		public int g = 0;
		public int b = 0;
	#region Morph Ball variables
		public bool morphBall = false;
		public bool ballstate = false;
		public const int morphSize = 12;
		public int boostCharge = 0;
		public int boostEffect = 0;
		public bool spiderball = false;
		public bool special = false;
		public int cooldownbomb = 0;
		public int bomb = 0;
		public int bombDamage = 10;
		public bool Ibounce = true;
		public float velY = 0f;
		public bool mouseRight = false;
		public int soundDelay = 0;
		public SoundEffectInstance soundInstance;
		public bool trap = false;
		public bool executeChange = false;
	#endregion
        public Color boostGold = Color.FromNonPremultiplied(255, 255, 0, 6);
        public Color boostYellow = Color.FromNonPremultiplied(255, 215, 0, 6);
		public bool speedBoosting = false;
		int powerReChargeDelay = 0;
		public float statCharge = 0.0f;
		public static float maxCharge = 100.0f;
		public Color chargeColor = Color.White;
		public float statPBCh = 0.0f;
		public static float maxPBCh = 200.0f;
		public static float maxSpaceJumps = 120;
		public float statSpaceJumps = maxSpaceJumps;
		public int spaceJumpsRegenDelay = 0;
		public int screwAttackSpeedEffect = 0;
		public bool screwAttack = false;
		public bool isPowerSuit = false;
		public bool thrusters = false;
		public bool spaceJump = false;
		public bool visorGlow = false;
		public Color visorGlowColor = new Color(255, 255, 255);
		public Texture2D thrusterTexture;
		public bool speedBooster = false;
		public Color morphColor = Color.White;
		public Color morphColorLights = Color.White;
		public Color morphItemColor = Color.White;
		public Vector2 oldPosition;
		
		#region misc
		public bool somersault = false;
		public float rotation = 0.0f;
		public float rotateSpeed = 0.05f;
		public float rotateSpeed2 = 50f;
		public float rotateCountX = 0.05f;
		public float rotateCountY = 0.05f;
		public int hyperColors = 0;
		public int shineDirection = 0;
		public bool shineActive = false;
		int shineDischarge = 0;
		float speedBuildUp = 0f;
		public int shineCharge = 0;
		int proj = -1;
		int shineSound = 0;
		public bool jet = false;
		//public bool flashActive = false;
		public bool canSomersault = false;
		public bool spaceJumped = false;
		public bool isSenseMoving = false;
		public int SMoveEffect = 0;
		bool senseSound = false;
		public bool detect = false;
		public int sMoveDir = 1;
		//public Vector2 grappleVect = Vector2.Zero;
		public float grappleRotation = 0f;
		public float maxDist;
		public int grapplingBeam = -1;
		//int soundDelay2 = 42;
		//public bool grappleBeamIsHooked = false;
		public float breathMult = 1f;
		#endregion
		public float maxOverheat = 100f;
		public float statOverheat = 0f;
		public float overheatCost = 1f;
		public int overheatDelay = 0;
		public int specialDmg = 100;
		public bool phazonImmune = false;
        public bool hazardShield = false;
		public int phazonRegen = 0;
		int tweak = 0;
		bool tweak2 = false;
		public double Time = 0;
		public override void ResetEffects()
		{			
			speedBoosting = false;
			isPowerSuit = false;
			phazonImmune = false;
			phazonRegen = 0;
            hazardShield = false;
			thrusters = false;
			spaceJump = false;
			speedBooster = false;
			if(!player.mount.Active || player.mount.Type != mod.MountType("MorphBallMount"))
			{
				morphBall = false;
			}
			visorGlow = false;
			visorGlowColor = new Color(255, 255, 255);
			chargeColor = Color.White;
			maxOverheat = 100f;
			overheatCost = 1f;
			breathMult = 1f;
		}
		float overheatCooldown = 0f;
		int itemRotTweak = 0;
		public override void PreUpdate()
		{
			ballstate = (player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount"));
			if(ballstate)
			{
				if(CheckCollide(player.position-new Vector2((20-morphSize)/2,42-morphSize),20,42))
				{
					player.controlMount = false;
					player.releaseMount = false;
					mflag = false;
				}
			}
			else
			{
				boostCharge = 0;
				boostEffect = 0;
				spiderball = false;
			}
			
			UIParameters.oldState = UIParameters.newState;
            UIParameters.newState = Keyboard.GetState();
        	UIParameters.lastMouseState = UIParameters.mouseState;
        	UIParameters.mouseState = Mouse.GetState();
			oldPosition = player.position;
			Player P = player;
			specialDmg = (int)player.rangedDamage * 100;
			morphColor = P.shirtColor;
			morphColor.A = 255;
			morphColorLights = P.underShirtColor;
			morphColorLights.A = 255;
			morphItemColor = P.shirtColor;
			morphItemColor.A = 255;
			somersault = (!P.dead && (SMoveEffect > 0 || canSomersault) && !P.mount.Active && P.velocity.Y != 0 && P.velocity.X != 0 && (P.itemAnimation == 0 || statCharge >= 30) && P.grappling[0] <= -1 && grapplingBeam <= -1 && shineDirection == 0 && !shineActive && !ballstate && (((P.wingsLogic != 0 || P.rocketBoots != 0 || P.carpet) && (!P.controlJump || (!P.canRocket && !P.rocketRelease && P.wingsLogic == 0) || (P.wingTime <= 0 && P.rocketTime <= 0 && P.carpetTime <= 0))) || (P.wingsLogic == 0 && P.rocketBoots == 0 && !P.carpet)) && !P.sandStorm);
			somersault &= !(P.rocketDelay <= 0 && P.wingsLogic > 0 && P.controlJump && P.velocity.Y > 0f && P.wingTime <= 0);

			player.breathMax = (int)(200 * breathMult);
			if(!morphBall)
			{
				player.width = 20;
				//player.height = 42;
			}

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
			if(statPBCh > 0)
			{
				if(powerReChargeDelay <= 0)
				{
					statPBCh -= 1.0f;
					powerReChargeDelay = 6;
				}
				if(powerReChargeDelay > 0)
				{
					powerReChargeDelay--;
				}
			}
			if(statCharge >= maxCharge)
			{
				statCharge = maxCharge;
			}
			if(overheatDelay > 0)
			{
				overheatDelay--;
			}
			if(statOverheat > 0)
			{
				if(shineDirection <= 0 && !shineActive && overheatDelay <= 0)
				{
					statOverheat -= overheatCooldown;
					if(statCharge <= 0)
					{
						overheatCooldown += 0.025f;
					}
					else if(overheatCooldown < 0.25f)
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
		
			int colorcount = 16;
			if (style == 0)
			{
				g += colorcount;
				if (g >= 255)
				{
					g = 255;
					style++;
				}
				r -= colorcount;
				if (r <= 63)
				{
					r = 63;
				}
			}
			else if (style == 1)
			{
				b += colorcount;
				if (b >= 255)
				{
					b = 255;
					style++;
				}
				g -= colorcount;
				if (g <= 63)
				{
					g = 63;
				}
			}
			else
			{
				r += colorcount;
				if (r >= 255)
				{
					r = 255;
					style = 0;
				}
				b -= colorcount;
				if (b <= 63)
				{
					b = 63;
				}
			}
			Time += 1.0;
			if(Time > 54000.0)
			{
				Time = 0;
			}

			bool trail = (!player.dead && !player.mount.Active && player.grapCount == 0 && shineDirection == 0 && !shineActive && !ballstate);
			if(trail && ((player.controlJump && player.jump > 0 && isPowerSuit) || (grapplingBeam >= 0 && (Math.Abs(player.velocity.X) >= 8.5f || Math.Abs(player.velocity.Y) >= 8.5f)) || (spaceJump && somersault) || SMoveEffect > 0))
			{
				tweak++;
				if(tweak > 4)
				{
					tweak = 5;
                    player.armorEffectDrawShadow = true;
				}
			}
			else
			{
				tweak = 0;
			}

			if(jet)
			{
				Lighting.AddLight((int)((float)player.Center.X/16f), (int)((float)player.Center.Y/16f), 0.6f, 0.38f, 0.24f);
			}
			if(!player.mount.Active)
			{
				if(somersault)
				{
					float rotMax = (float)Math.PI/8;
					if(spaceJump && SMoveEffect <= 0)
					{
						rotMax = (float)Math.PI/4;
					}
					rotation += MathHelper.Clamp((rotateCountX + rotateCountY) * player.direction * player.gravDir * sMoveDir,-rotMax,rotMax);
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
					itemRotTweak = 2;
				}
				else if(shineDirection == 2 || shineDirection == 4)
				{
					rotation = 0.1f * player.direction * player.gravDir;
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
			if(!phazonImmune)
			{
				if(TouchTiles(player.position, player.width, player.height, mod.TileType("PhazonTile")))
				{
					player.AddBuff(mod.BuffType("PhazonDebuff"), 1, true);
				}
			}
			else
			{
				if(TouchTiles(player.position, player.width, player.height, mod.TileType("PhazonTile")) && phazonRegen > 0)
				{
					player.lifeRegen += phazonRegen;
				}
			}

            if (hazardShield)
            {
                List<int> debuffList = new List<int>() {20, 21, 22, 23, 24, 30, 31, 32, 33, 35, 36, 46, 47, 69, 70, 72, 80, 88, 94, 103, 120, 137, 144, 145, 148, 149, 153, 156, 164, 169, 195, 196, 197};

                for (int k = 0; k < 22; k++)
                {
                    int buff = P.buffType[k];
                    if(debuffList.Contains(buff))
                    {
                        if (P.body == mod.ItemType("HazardShieldBreastplate"))
                        {
                            P.buffTime[k] = Math.Max(P.buffTime[k] - 1, 0);
                        }
                        else if (P.body == mod.ItemType("StardustHazardShieldSuitBreastplate"))
                        {
                            P.buffTime[k] = Math.Max(P.buffTime[k] - 2, 0);
                        }
                    }
                }
            }
		}
        public static bool TouchTiles(Vector2 Position, int Width, int Height, int tileType)
		{
			Vector2 vector = Position;
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY)
			{
				num4 = Main.maxTilesY;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && Main.tile[i, j].slope() == 0 && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tile[i, j].type == tileType)
					{
						Vector2 vector2;
						vector2.X = (float)(i * 16);
						vector2.Y = (float)(j * 16);
						int num6 = 16;
						if (Main.tile[i, j].halfBrick())
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
		public void GrappleBeamMovement()
		{
			if(grapplingBeam >= 0)
			{
				Projectile projectile = Main.projectile[grapplingBeam];
				if(projectile.type == mod.ProjectileType("GrappleBeamShot") && projectile.owner == player.whoAmI && projectile.active)
				{
					float targetrotation = (float)Math.Atan2(((projectile.Center.Y-player.Center.Y)*player.direction),((projectile.Center.X-player.Center.X)*player.direction));
					grappleRotation = targetrotation;
					/*if (player.velocity.Y != 0 && player.itemAnimation == 0)
					{
						player.fullRotation = grappleRotation + (player.direction*(float)Math.PI/2);
						player.fullRotationOrigin = player.Center - player.position;
					}*/

					if (Main.myPlayer == player.whoAmI && player.mount.Active)
					{
						player.mount.Dismount(player);
					}
					player.canCarpet = true;
					player.carpetFrame = -1;
					player.wingFrame = 1;
					if (player.velocity.Y == 0f || (player.wet && (double)player.velocity.Y > -0.02 && (double)player.velocity.Y < 0.02))
					{
						player.wingFrame = 0;
					}
					if (player.wings == 4)
					{
						player.wingFrame = 3;
					}
					if (player.wings == 30)
					{
						player.wingFrame = 0;
					}
					player.wingTime = (float)player.wingTimeMax;
					player.rocketTime = player.rocketTimeMax;
					player.rocketDelay = 0;
					player.rocketFrame = false;
					player.canRocket = false;
					player.rocketRelease = false;
					player.fallStart = (int)(player.position.Y / 16f);

					Vector2 v = player.Center - projectile.Center;
					float dist = Vector2.Distance(player.Center, projectile.Center);
					/*if(soundDelay2 > 41)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/GrappleLoop"));
						soundDelay2 = 0;
					}
					soundDelay2++;*/
					bool up = (player.controlUp);
					bool down = (player.controlDown && maxDist < 400);
					float ndist = Vector2.Distance(player.Center + player.velocity, projectile.Center);
					float ddist = ndist - dist;
					float distdiff = (dist-maxDist);
					if(distdiff <= 0f)
					{
						distdiff = 0f;
					}
					if(distdiff > player.gravity)
					{
						distdiff = player.gravity;
					}
					float num4 = projectile.Center.X - player.Center.X;
					float num5 = projectile.Center.Y - player.Center.Y;
					float num6 = (float)System.Math.Sqrt((double)(num4 * num4 + num5 * num5));
					float num7 = ddist+player.gravity+distdiff;
					if(up)
					{
						num7 = 11;
						maxDist = dist;
					}
					if(down)
					{
						num7 = -11;
						maxDist = dist;
					}
					float num8;
					if (num6 > num7)
					{
						num8 = num7 / num6;
					}
					else
					{
						num8 = 1f;
					}
					num4 *= num8;
					num5 *= num8;
					Vector2 vect = new Vector2(num4, num5);
					if(up || down)
					{
						player.velocity = vect;
						tweak2 = true;
					}
					else
					{
						if (dist >= maxDist)
						{
							player.velocity += vect;
							player.maxRunSpeed = 15f;
							player.runAcceleration *= 3f;
						}
						if(tweak2)
						{
							player.velocity *= 0;
							tweak2 = false;
						}
					}

					if (player.controlJump)
					{
						if (player.releaseJump)
						{
							if ((player.velocity.Y == 0f || (player.wet && (double)player.velocity.Y > -0.02 && (double)player.velocity.Y < 0.02)) && !player.controlDown)
							{
								player.velocity.Y = -Player.jumpSpeed;
								player.jump = Player.jumpHeight / 2;
								player.releaseJump = false;
							}
							else
							{
								player.velocity.Y = player.velocity.Y + 0.01f;
								player.releaseJump = false;
							}
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

							grapplingBeam = -1;
							player.grappling[0] = -1;
							player.grapCount = 0;
							for (int k = 0; k < 1000; k++)
							{
								if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].aiStyle == 7)//type == projectile.type)
								{
									Main.projectile[k].Kill();
								}
							}
							return;
						}
					}
					else
					{
						player.releaseJump = true;
					}
				}
			}
			else
			{
				tweak2 = false;
				//soundDelay2 = 42;
			}
		}
		bool sbFlag = false;
		bool mflag = false;
		public override void SetControls()
		{
			ballstate = (player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount"));
			
			//morph ball transformation tweaks and effects
			if((player.miscEquips[3].type == mod.ItemType("MorphBall") || player.mount.Type == mod.MountType("MorphBallMount")) && player.controlMount)
			{
				if(mflag)
				{
					if(ballstate)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/MorphIn"));
					}
					else
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/MorphOut"));
					}
					for (int i = 0; i < 25; i++)
					{
						int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 63, 0f, 0f, 100, morphColor, 2f);
						Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
						Main.dust[num].scale *= 1.3f;
						Main.dust[num].noGravity = true;
						Main.dust[num].velocity += player.velocity * 0.8f;
						Main.dust[num].noLight = true;
					}
					for (int j = 0; j < 15; j++)
					{
						int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 63, 0f, 0f, 100, morphColorLights, 1f);
						Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
						Main.dust[num].scale *= 1.3f;
						Main.dust[num].noGravity = true;
						Main.dust[num].velocity += player.velocity * 0.8f;
						Main.dust[num].noLight = true;
					}
					int oldWidth = player.width;
					player.width = ballstate?morphSize:20;
					int newWidth = player.width;
					float widthDiff = (float)(oldWidth - newWidth)*0.5f;
					player.position.X += widthDiff;
					mflag = false;
				}
			}
			else
			{
				mflag = true;
			}
		}
		public override void PostUpdateMiscEffects()
		{
			this.GrappleBeamMovement();

			if(speedBooster)
			{
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
			
			if(player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount"))
			{
				//temporarily trick the game into thinking the player isn't on a mount so that the player can use their original move speed and jump height
				player.mount._active = false;
				ballstate = true;
				player.jumpAgainCloud = false;
				player.jumpAgainSandstorm = false;
				player.jumpAgainBlizzard = false;
				player.jumpAgainFart = false;
				player.jumpAgainSail = false;
				player.jumpAgainUnicorn = false;
				player.pulley = false;
				statCharge = 0;
			}
			else
			{
				ballstate = false;
			}
			
			if(!ballstate)
			{
				special = false;
			}
		}
		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
		{
			if(player.miscEquips[3].type == mod.ItemType("MorphBall"))
			{
				player.VanillaUpdateAccessory(player.whoAmI, player.miscEquips[3], player.hideMisc[3], ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
			}
		}
		public override void PostUpdateEquips()
		{
			if(ballstate)
			{
				player.spikedBoots = 0;
			}
		}
		public override void PostUpdateRunSpeeds()
		{
			if(spiderball && CurEdge != Edge.None)
			{
				player.moveSpeed = 0f;
				player.maxRunSpeed = 0f;
				player.accRunSpeed = 0f;
				player.velocity.X = 0f;
			}
			
			if(ballstate)
			{
				//end morph ball mount trick
				player.mount._active = true;
			}
		}
        public override void PostUpdate()
		{
			if(player.itemAnimation > 0)
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
			if(!morphBall)
			{
				ballstate = false;
				boostCharge = 0;
				boostEffect = 0;
				spiderball = false;
				special = false;
				cooldownbomb = 0;
				bomb = 0;
				Ibounce = true;
				velY = 0f;
				mouseRight = false;
				soundDelay = 0;
				trap = false;
				executeChange = false;
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
			grapplingBeam = -1;
		}
		public int psuedoScrewFlash = 0;
		public int shineChargeFlash = 0;
		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			Player P = player;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>(mod);
			
			bool pseudoScrew = (statCharge >= maxCharge && somersault);
			if(pseudoScrew)
			{
				if(drawInfo.shadow == 0f)
				{
					psuedoScrewFlash++;
				}
			}
			else
			{
				psuedoScrewFlash = 0;
			}
			if(shineCharge > 0)
			{
				if(drawInfo.shadow == 0f)
				{
					shineChargeFlash++;
				}
			}
			else
			{
				shineChargeFlash = 0;
			}
			if(hyperColors > 0 || speedBoosting || shineActive || (pseudoScrew && psuedoScrewFlash >= 3) || (shineCharge > 0 && shineChargeFlash >= 4))
			{
				byte shader = (byte)GameShaders.Armor.GetShaderIdFromItemId(3558);
				if(P.head > 0 && P.cHead <= 0)
				{
					drawInfo.headArmorShader = shader;
				}
				if(P.body > 0 && P.cBody <= 0)
				{
					drawInfo.bodyArmorShader = shader;
				}
				if(P.legs > 0 && P.cLegs <= 0)
				{
					drawInfo.legArmorShader = shader;
				}

				if(drawInfo.shadow == 0f && hyperColors > 0)
				{
					hyperColors--;
				}
				if(psuedoScrewFlash >= 9)
				{
					psuedoScrewFlash = 0;
				}
				if(shineChargeFlash >= 6)
				{
					shineChargeFlash = 0;
				}
			}
			ballLayer.Draw(ref drawInfo);
		}
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			MPlayer mPlayer = player.GetModPlayer<MPlayer>(mod);
			Player P = player;

    		for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k] == PlayerLayer.FrontAcc)
				{
					layers.Insert(k + 1, screwAttackLayer);
				}
				if (layers[k] == PlayerLayer.Body)
				{
					layers.Insert(k + 1, thrusterLayer);
					layers.Insert(k + 2, jetLayer);
				}
				if (layers[k] == PlayerLayer.Head)
				{
					layers.Insert(k + 1, visorLayer);
				}
				if(layers[k] == PlayerLayer.Arms)
				{
					layers.Insert(k + 1, gunLayer);
				}
			}
			layers.Add(ballLayer);
			if(somersault)
			{
				P.bodyFrame.Y = P.bodyFrame.Height * 6;
				P.legFrame.Y = P.legFrame.Height * 7;
				P.wingFrame = 1;
				if (P.wings == 4)
				{
					P.wingFrame = 3;
				}
			}
			else if(shineActive && shineDirection == 0 && shineDischarge > 0)
			{
				if(shineDischarge < 15)
				{
					P.bodyFrame.Y = P.bodyFrame.Height * 5;
				}
				else if(shineDischarge <= 30)
				{
					P.bodyFrame.Y = P.bodyFrame.Height * 6;
				}
				P.legFrame.Y = P.legFrame.Height * 5;
			}
			else if(shineDirection == 5)
			{
				P.bodyFrame.Y = 0;
				P.legFrameCounter = 0.0;
				P.legFrame.Y = 0;
				if(thrusters)
				{
					PlayerLayer.Wings.visible = false;
					PlayerLayer.BackAcc.visible = false;
				}
				else
				{
					P.wingFrame = 0;
					if (P.wings == 4)
					{
						P.wingFrame = 3;
					}
				}
			}
			else if(shineDirection == 2 || shineDirection == 4)
			{
				P.bodyFrame.Y = P.bodyFrame.Height * 6;
				P.legFrame.Y = P.legFrame.Height * 7;
				if(thrusters)
				{
					jet = true;
					PlayerLayer.Wings.visible = false;
					PlayerLayer.BackAcc.visible = false;
				}
				else
				{
					P.wingFrame = 2;
					if (P.wings == 4)
					{
						P.wingFrame = 3;
					}
				}
			}
			else if(shineDirection == 1 || shineDirection == 3)
			{
				P.bodyFrame.Y = P.bodyFrame.Height * 6;
				P.legFrame.Y = P.legFrame.Height * 7;
				if(thrusters)
				{
					jet = true;
					PlayerLayer.Wings.visible = false;
					PlayerLayer.BackAcc.visible = false;
				}
				else
				{
					P.wingFrame = 2;
					if (P.wings == 4)
					{
						P.wingFrame = 3;
					}
				}
			}
			else
			{
				/*float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (P.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				}
				float targetrotation = (float)Math.Atan2((MY-P.Center.Y)*P.direction,(MX-P.Center.X)*P.direction);
				if((XRayScope.xray.XRayActive(P) && MBase.HeadTracking == "X-Ray") || (!ballstate && ((P.direction == 1 && MX >= P.Center.X) || (P.direction == -1 && MX <= P.Center.X))))
				{
					P.headRotation = targetrotation * 0.3f;
					if ((double)P.headRotation < -0.3)
					{
						P.headRotation = -0.3f;
					}
					if ((double)P.headRotation > 0.3)
					{
						P.headRotation = 0.3f;
					}
				}
				else if(!P.merman && !P.dead)
				{
					P.headRotation = 0f;
				}*/
				if(grapplingBeam >= 0 && P.itemAnimation <= 0)
				{
					float num11 = grappleRotation * (float)P.direction;
					P.bodyFrame.Y = P.bodyFrame.Height * 3;
					if ((double)num11 < -0.75)
					{
						P.bodyFrame.Y = P.bodyFrame.Height * 2;
						if (P.gravDir == -1f)
						{
							P.bodyFrame.Y = P.bodyFrame.Height * 4;
						}
					}
					if ((double)num11 > 0.6)
					{
						P.bodyFrame.Y = P.bodyFrame.Height * 4;
						if (P.gravDir == -1f)
						{
							P.bodyFrame.Y = P.bodyFrame.Height * 2;
						}
					}
				}
			}
			if(ballstate)
			{
				PlayerLayer.HairBack.visible = false;
				PlayerLayer.MountBack.visible = false;
				PlayerLayer.MiscEffectsBack.visible = false;
				PlayerLayer.BackAcc.visible = false;
				PlayerLayer.Wings.visible = false;
				PlayerLayer.BalloonAcc.visible = false;
				PlayerLayer.Skin.visible = false;
				PlayerLayer.Legs.visible = false;
				PlayerLayer.ShoeAcc.visible = false;
				PlayerLayer.Body.visible = false;
				PlayerLayer.HandOffAcc.visible = false;
				PlayerLayer.WaistAcc.visible = false;
				PlayerLayer.NeckAcc.visible = false;
				PlayerLayer.Face.visible = false;
				PlayerLayer.Hair.visible = false;
				PlayerLayer.Head.visible = false;
				PlayerLayer.FaceAcc.visible = false;
				PlayerLayer.MountFront.visible = false;
				PlayerLayer.ShieldAcc.visible = false;
				PlayerLayer.SolarShield.visible = false;
				PlayerLayer.HeldProjBack.visible = false;
				PlayerLayer.HeldItem.visible = false;
				PlayerLayer.Arms.visible = false;
				PlayerLayer.HandOnAcc.visible = false;
				PlayerLayer.HeldProjFront.visible = false;
				PlayerLayer.FrontAcc.visible = false;
				PlayerLayer.MiscEffectsFront.visible = false;
			}
			else
			{
				if(somersault || shineActive)
				{
					PlayerLayer.HeldItem.visible = false;
				}
				if (thrusters)
				{
					if((P.wings == 0 && P.back == -1) || P.velocity.Y == 0f || mPlayer.shineDirection != 0)
					{
						PlayerLayer.Wings.visible = false;
						PlayerLayer.BackAcc.visible = false;
					}
				}
			}
			
			if(!thrusters)
			{
				jet = false;
			}
		}

		public static readonly PlayerLayer screwAttackLayer = new PlayerLayer("MetroidMod", "screwAttackLayer", PlayerLayer.FrontAcc, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>(mod);
			if (mPlayer.somersault && mPlayer.screwAttack && drawInfo.shadow == 0f && !mPlayer.ballstate)
			{
				Texture2D tex = mod.GetTexture("Projectiles/ScrewAttackProj");
				Texture2D tex2 = mod.GetTexture("Gore/ScrewAttack_Yellow");
				for(int i = 0; i < 255; i++)
				{
					Projectile projectile = Main.projectile[i];
					if(projectile.active && projectile.owner == P.whoAmI && projectile.type == mod.ProjectileType("ScrewAttackProj"))
					{
						SpriteEffects effects = SpriteEffects.None;
						if (projectile.spriteDirection == -1)
						{
							effects = SpriteEffects.FlipHorizontally;
						}
						Color alpha = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
						int num121 = tex.Height / Main.projFrames[projectile.type];
						int y9 = num121 * projectile.frame;
						//float num100 = (float)(tex.Width - projectile.width) * 0.5f + (float)projectile.width * 0.5f;
						//spriteBatch.Draw(tex, new Vector2(projectile.position.X - Main.screenPosition.X + num100, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, y9, tex.Width, num121 - 1)), alpha, -mPlayer.rotation, new Vector2(num100, (float)(projectile.height / 2)), projectile.scale, effects, 0);
						spriteBatch.Draw(tex, drawInfo.position + P.fullRotationOrigin - Main.screenPosition, new Rectangle?(new Rectangle(0, y9, tex.Width, num121 - 1)), alpha, -mPlayer.rotation, new Vector2((float)(projectile.width / 2), (float)(projectile.height / 2)), projectile.scale, effects, 0);
						if(mPlayer.screwAttackSpeedEffect > 0)
						{
							Color color21 = alpha * ((float)Math.Min(mPlayer.screwAttackSpeedEffect,30)/30f);
							//spriteBatch.Draw(tex2, new Vector2(projectile.position.X - Main.screenPosition.X + num100, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, y9, tex2.Width, num121 - 1)), color21, -mPlayer.rotation, new Vector2(num100, (float)(projectile.height / 2)), projectile.scale, effects, 0);
							spriteBatch.Draw(tex2, drawInfo.position + P.fullRotationOrigin - Main.screenPosition, new Rectangle?(new Rectangle(0, y9, tex2.Width, num121 - 1)), color21, -mPlayer.rotation, new Vector2((float)(projectile.width / 2), (float)(projectile.height / 2)), projectile.scale, effects, 0);
							Texture2D tex3 = mod.GetTexture("Gore/ScrewAttack_YellowPlayerGlow");
							//Main.playerDrawData.Add(new DrawData(tex3, new Vector2(projectile.position.X - Main.screenPosition.X + num100, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex3.Width, tex3.Height)), color21, 0f, new Vector2(num100, (float)(projectile.height / 2)), projectile.scale, effects, 0));
							Main.playerDrawData.Add(new DrawData(tex3, drawInfo.position + (P.Center-P.position) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex3.Width, tex3.Height)), color21, 0f, new Vector2((float)(projectile.width / 2), (float)(projectile.height / 2)), projectile.scale, effects, 0));
						}
					}
				}
			}
		});
		public static readonly PlayerLayer visorLayer = new PlayerLayer("MetroidMod", "visorLayer", PlayerLayer.Head, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>(mod);
			if (mPlayer.isPowerSuit && !mPlayer.ballstate)
			{
				Texture2D tex = mod.GetTexture("Gore/VisorGlow");
				mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headOrigin, drawPlayer.GetImmuneAlphaPure(mPlayer.visorGlowColor,drawInfo.shadow), 0);
			}
		});
		public static readonly PlayerLayer ballLayer = new PlayerLayer("MetroidMod", "ballLayer", PlayerLayer.FrontAcc, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			Texture2D tex = mod.GetTexture("Gore/Morphball");
			Texture2D tex2 = mod.GetTexture("Gore/Morphball_Light");
			Texture2D tex3 = mod.GetTexture("Gore/Mockball");
			Texture2D boost = mod.GetTexture("Gore/Boostball");
			Texture2D trail = mod.GetTexture("Gore/Morphball_Trail");
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>(mod);
			mPlayer.DrawBallTexture(spriteBatch, tex, tex2, tex3, boost, trail, drawPlayer, drawInfo);
		});
		public static readonly PlayerLayer gunLayer = new PlayerLayer("MetroidMod", "gunLayer", PlayerLayer.Arms, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>(mod);
			Item I = P.inventory[P.selectedItem];
			int frame = (int)(P.bodyFrame.Y/P.bodyFrame.Height);
			if ((I.type == mod.ItemType("PowerBeam") || I.type == mod.ItemType("MissileLauncher")) && ((P.itemAnimation == 0 && (frame < 1 || frame > 4)) || (mPlayer.statCharge > 0 && mPlayer.somersault)) && !P.dead)
			{
				Texture2D tex = Main.itemTexture[I.type];//I.GetTexture();
				/*MItem mi = I.GetSubClass<MItem>();
				if((I.type == ItemDef.byName["MetroidMod:PowerBeam"].type || I.type == ItemDef.byName["MetroidMod:MissileLauncher"].type) && mi.texture != null)
				{
					tex = mi.texture;
					if(MBase.AltBeamSkins && mi.textureAlt != null)
					{
						tex = mi.textureAlt;
					}
				}*/
				if(tex != null)
				{
					Vector2 origin = new Vector2(12f, (float)((int)(tex.Height/2)));
					if(P.direction == -1)
					{
						origin.X = tex.Width - 12;
					}
					Vector2 pos = new Vector2(0f,0f);
					float rot = 0f;
					float rotate = 0f;
					float posX = 0f;
					float posY = 0f;
					if(frame == 0)
					{
						rotate = 1.3625f;
						posX = -7f;
						posY = 13f;
					}
					else if(frame == 5)
					{
						rotate = -1.75f;
						posX = -8f;
						posY = -12f;
					}
					else if(frame == 6 || frame == 18 || frame == 19 || (frame >= 11 && frame <= 13))
					{
						posX = 0f;
						posY = 5f;
					}
					else if(frame >= 7 && frame <= 9)
					{
						posX = -2f;
						posY = 3f;
					}
					else if(frame == 10)
					{
						posX = -2f;
						posY = 5f;
					}
					else if(frame == 14)
					{
						posX = 2f;
						posY = 3f;
					}
					else if(frame == 15 || frame == 16)
					{
						posX = 4f;
						posY = 3f;
					}
					else if(frame == 17)
					{
						posX = 2f;
						posY = 5f;
					}
					rot = rotate*P.direction*P.gravDir;
					pos.X += ((float)P.bodyFrame.Width * 0.5f) + posX*P.direction;
					pos.Y += ((float)P.bodyFrame.Height * 0.5f) + 4f + posY*P.gravDir;

					SpriteEffects effects = SpriteEffects.None;
					if (P.direction == -1)
					{
						effects = SpriteEffects.FlipHorizontally;
					}
					Color color = Lighting.GetColor((int)((double)drawInfo.position.X + (double)P.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)P.height * 0.5) / 16);

					DrawData item = new DrawData(tex, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(P.bodyFrame.Width / 2) + (float)(P.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)P.height - (float)P.bodyFrame.Height + 4f))) + new Vector2((float)((int)pos.X),(float)((int)pos.Y)), new Rectangle?(new Rectangle(0,0,tex.Width,tex.Height)), drawInfo.middleArmorColor, rot, origin, I.scale, effects, 0);
 					item.shader = drawInfo.bodyArmorShader;
					Main.playerDrawData.Add(item);
				}
			}
		});
		public static readonly PlayerLayer thrusterLayer = new PlayerLayer("MetroidMod", "thrusterLayer", PlayerLayer.Body, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>(mod);
			if (mPlayer.thrusters)
			{
				if((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
				{
					if(mPlayer.thrusterTexture != null)
					{
						Texture2D tex = mPlayer.thrusterTexture;
						mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyOrigin, drawInfo.middleArmorColor, drawInfo.bodyArmorShader);
  					}
				}
			}
		});
		public static readonly PlayerLayer jetLayer = new PlayerLayer("MetroidMod", "jetLayer", PlayerLayer.Body, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>(mod);
			if (mPlayer.jet && !drawPlayer.sandStorm && drawInfo.shadow == 0f && mPlayer.thrusters)
			{
				if((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
				{
					Texture2D tex = mod.GetTexture("Gore/thrusterFlame");
					mPlayer.DrawThrusterJet(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyRotation, drawPlayer.bodyPosition);
				}
			}
		});
		public void DrawTexture(SpriteBatch sb, PlayerDrawInfo drawInfo, Texture2D tex, Player drawPlayer, Rectangle frame, float rot, Vector2 drawPos, Vector2 origin, Color color, int shader)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (drawPlayer.direction == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			DrawData item = new DrawData(tex, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(frame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)frame.Height + 4f))) + drawPos + origin, new Rectangle?(frame), color, rot, origin, 1f, effects, 0);
			item.shader = shader;
			Main.playerDrawData.Add(item);
		}
		Rectangle jetFrame;
		int jetFrameCounter = 1;
		int currentFrame = 0;
		public void DrawThrusterJet(SpriteBatch sb, PlayerDrawInfo drawInfo, Texture2D tex, Player drawPlayer, float rot, Vector2 drawPos)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (drawPlayer.direction == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			jetFrame.Width = 40;
			jetFrame.Height = 56;
			jetFrame.X = 0;
			jetFrame.Y = jetFrame.Height*currentFrame;
			jetFrameCounter++;
			int frame = 2;
			if(jetFrameCounter < frame)
			{
				currentFrame = 0;
			}
			else if(jetFrameCounter < frame * 2)
			{
				currentFrame = 1;
			}
			else if(jetFrameCounter < frame * 3)
			{
				currentFrame = 2;
			}
			else if(jetFrameCounter < frame * 4 - 1)
			{
				currentFrame = 1;
			}
			else
			{
				currentFrame = 1;
				jetFrameCounter = 0;
			}
			float yfloat = 4f;
			Main.playerDrawData.Add(new DrawData(tex, new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(jetFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)jetFrame.Height + yfloat))) + drawPos + drawInfo.bodyOrigin, new Rectangle?(jetFrame), Color.White, rot, drawInfo.bodyOrigin, 1f, effects, 0));
		}
		public float ballrot = 0f;
		public static int oldNumMax = 10;
		public Vector2[] oldPos = new Vector2[oldNumMax];
        public void DrawBallTexture(SpriteBatch sb, Texture2D mytex, Texture2D mytex2, Texture2D mytex3, Texture2D boosttex, Texture2D trail, Player drawPlayer, PlayerDrawInfo drawInfo)
		{
			float thisx = (float)((int)(drawInfo.position.X + (float)(drawPlayer.width / 2) - Main.screenPosition.X));
			float thisy = (float)((int)(drawInfo.position.Y + (float)(drawPlayer.height / 2) - Main.screenPosition.Y));
			Vector2 ballDims = new Vector2(28f,28f);
			Vector2 thispos =  new Vector2(thisx,thisy);
			if(drawInfo.shadow == 0f)
			{
				int timez = (int)(Time%60)/10;
				SpriteEffects effects = SpriteEffects.None;
				if (drawPlayer.direction == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				float ballrotoffset = 0f;
				if(drawPlayer.velocity.Y != Vector2.Zero.Y)
				{
					if(drawPlayer.velocity.X != 0f)
					{
						ballrotoffset += 0.05f*drawPlayer.velocity.X;
					}
					else
					{
						ballrotoffset += 0.25f*drawPlayer.direction;
					}
				}
				else if (drawPlayer.velocity.X < 0f)
				{
					ballrotoffset -= 0.2f;
				}
				else if ( drawPlayer.velocity.X > 0f)
				{
					ballrotoffset += 0.2f;
				}
				if(drawPlayer.velocity.X != 0f)
				{
					ballrotoffset += 0.025f*drawPlayer.velocity.X;
				}
				else
				{
					ballrotoffset += 0.125f*drawPlayer.direction;
				}
				if(spiderball && CurEdge != Edge.None)
				{
					ballrot += spiderSpeed*0.085f;
				}
				else
				{
					ballrot += ballrotoffset;
				}
				if(ballrot > (float)(Math.PI)*2)
				{
					ballrot -= (float)(Math.PI)*2;
				}
				if(ballrot < -(float)(Math.PI)*2)
				{
					ballrot += (float)(Math.PI)*2;
				}
				Color mColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, morphColor),0f);
				float scale = 0.57f;
				int offset = 4;
				for (int num46 = oldPos.Length - 1; num46 > 0; num46--)
				{
					Vector2 vect = oldPos[1] - oldPos[0];
					oldPos[num46] = oldPos[num46 - 1] - (vect*0.5f);
				}
				oldPos[0] = thispos + Main.screenPosition;
				if (ballstate && drawPlayer.active && !drawPlayer.dead)
				{
					for (int num88 = 0; num88 < oldPos.Length; num88++)
					{
						Color color23 = morphColorLights;
						color23 *= (float)(oldPos.Length - (num88)) / 15f;
						sb.Draw(trail, oldPos[num88] - Main.screenPosition, new Rectangle?(new Rectangle(0,0,trail.Width, trail.Height)), color23, ballrot, ballDims/2, scale, effects, 0f);
					}
					sb.Draw(mytex, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), mColor,ballrot,ballDims/2, scale, effects, 0f);
					sb.Draw(mytex2, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), morphColorLights,ballrot,ballDims/2, scale, effects, 0f);
					if(boostEffect > 0)
					{
						for (int i = 0; i < boostEffect; i++)
						{
							sb.Draw(boosttex, thispos, new Rectangle?(new Rectangle(0,0,boosttex.Width,boosttex.Height)), boostGold * 0.5f,ballrot,ballDims/2, scale, effects, 0f);
						}
					}
					if(boostCharge > 0)
					{
						for (int i = 0; i < boostCharge; i++)
						{
							sb.Draw(boosttex, thispos, new Rectangle?(new Rectangle(0,0,boosttex.Width,boosttex.Height)), boostYellow * 0.5f,ballrot,ballDims/2, scale, effects, 0f);
						}
					}
					Texture2D spiderTex = mod.GetTexture("Gore/Spiderball");
					if(spiderball)
					{
						sb.Draw(spiderTex, thispos, new Rectangle?(new Rectangle(0,0,spiderTex.Width,spiderTex.Height)), morphColorLights*0.5f,ballrot,new Vector2(spiderTex.Width/2,spiderTex.Height/2), scale, effects, 0f);
					}
				}
			}
		}

		public void SenseMove(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			int dist = 80;
			if(senseSound)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SenseMoveSound"));
			}
			Vector2 right = new Vector2(7.5f, -4.5f);
			Vector2 left = new Vector2(-7.5f, -4.5f);
			detect = false;
			float mult = Player.jumpSpeed - (Player.jumpHeight/Player.jumpSpeed) + player.gravity;
			float threshhold = Player.jumpSpeed*mult;
			float minimum = 2.5f;
			for(int k = 0; k < Main.npc.Length; k++)
			{
				NPC N = Main.npc[k];
				if(N.damage > 0 && !N.friendly && N.life > 0 && N.active)
				{
					for(int i = 1; i <= dist; i++)
					{
						Vector2 npcFuturePos = new Vector2(N.Center.X+(N.velocity.X*i),N.Center.Y+(N.velocity.Y*i));
						float npcDist = Vector2.Distance(P.Center, npcFuturePos);
						Vector2 pFuturePos = new Vector2(P.Center.X + (player.controlLeft ? left.X/3 * i: right.X/3 * i), P.Center.Y);
						float npcDist2 = Vector2.Distance(pFuturePos, npcFuturePos);
						if (npcDist <= (P.height + N.width) || npcDist <= (P.height + N.height) || npcDist2 <= (P.height + N.width) || npcDist2 <= (P.height + N.height))
						{
						    if (N.noTileCollide || Collision.CanHit(P.position, P.width, P.height, N.position, N.width, N.height))
						    {
								detect = true;
						    }
						}
					}
					if(detect)
					{
						if(N.Center.X > P.position.X + P.width)
						{
							right.X -= N.velocity.X;
							if(N.position.Y + N.height + N.velocity.Y < P.position.Y)
							{
								right.X += N.velocity.Y;
							}
							else if(N.position.Y + N.velocity.Y > P.position.Y + P.height)
							{
								right.Y += N.velocity.Y;
							}
							else
							{
								float height = (P.position.Y + P.height) - N.position.Y;
								right.Y -= (height/10);
							}
						}
						if(N.Center.X < P.position.X)
						{
							left.X -= N.velocity.X;
							if(N.position.Y + N.height + N.velocity.Y < P.position.Y)
							{
								left.X -= N.velocity.Y;
							}
							else if(N.position.Y + N.velocity.Y > P.position.Y + P.height)
							{
								left.Y += N.velocity.Y;
							}
							else
							{
								float height = (P.position.Y + P.height) - N.position.Y;
								left.Y -= (height/10);
							}
						}
					}
				}
			}
			for(int k = 0; k < Main.projectile.Length; k++)
			{
				Projectile N = Main.projectile[k];
				if(N.damage > 0 && !N.friendly && N.hostile && N.timeLeft > 0 && N.active)
				{
					for(int i = 1; i <= dist; i++)
					{
						Vector2 projFuturePos = new Vector2(N.Center.X+(N.velocity.X*i),N.Center.Y+(N.velocity.Y*i));
						float projDist = Vector2.Distance(P.Center, projFuturePos);
						Vector2 pFuturePos = new Vector2(P.Center.X + (player.controlLeft ? left.X/3 * i : right.X/3 * i), P.Center.Y);
						float projDist2 = Vector2.Distance(pFuturePos, projFuturePos);
						if (projDist <= (P.height + N.width) || projDist <= (P.height + N.height) || projDist2 <= (P.height + N.width) || projDist2 <= (P.height + N.height))
						{
						    if (!N.tileCollide || Collision.CanHit(P.position, P.width, P.height, N.position, N.width, N.height))
						    {
								detect = true;
						    }
						}
					}
					if(detect)
					{
						if(N.Center.X > P.position.X + P.width)
						{
							right.X -= N.velocity.X;
							if(N.position.Y + N.height + N.velocity.Y < P.position.Y)
							{
								right.X += N.velocity.Y;
							}
							else if(N.position.Y + N.velocity.Y > P.position.Y + P.height)
							{
								right.Y += N.velocity.Y;
							}
							else
							{
								float height = (P.position.Y + P.height) - N.position.Y;
								right.Y -= (height/10);
							}
						}
						if(N.Center.X < P.position.X)
						{
							left.X -= N.velocity.X;
							if(N.position.Y + N.height + N.velocity.Y < P.position.Y)
							{
								left.X -= N.velocity.Y;
							}
							else if(N.position.Y + N.velocity.Y > P.position.Y + P.height)
							{
								left.Y += N.velocity.Y;
							}
							else
							{
								float height = (P.position.Y + P.height) - N.position.Y;
								left.Y -= (height/10);
							}
						}
					}
				}
			}
			right.X =  Math.Abs(right.X) > threshhold ? threshhold : (Math.Abs(right.X) < minimum*3 ? minimum*3 : Math.Abs(right.X));
			right.Y = right.Y > -minimum ? -minimum : (right.Y < -threshhold ? -threshhold : right.Y);
			left.X = Math.Abs(left.X) > threshhold ? -threshhold : (Math.Abs(left.X) < minimum*3 ? -minimum*3 : -Math.Abs(left.X));
			left.Y = left.Y > -minimum ? -minimum : (left.Y < -threshhold ? -threshhold : left.Y);
			if(!P.mount.Active && (P.velocity.Y == 0f || (mp.spaceJumpsRegenDelay < 10 && mp.spaceJump && mp.statSpaceJumps >= 15 && P.velocity.Y*player.gravDir > 0)))
			{
				if(!isSenseMoving)
				{
				    if ((P.controlLeft || P.controlRight) && MetroidMod.SenseMoveKey.Current && P.velocity.Y != 0)
				    {
						if (!detect)
						{
							right.Y = -threshhold * 0.65f;
							left.Y = -threshhold * 0.65f;
							right.X = threshhold * 0.75f;
							left.X = -threshhold * 0.75f;
						}
						player.jump = Player.jumpHeight;
						mp.statSpaceJumps -= 15;
						mp.spaceJumpsRegenDelay = 25;
						player.fallStart = (int)(player.Center.Y / 16f);
						mp.spaceJumped = true;
						mp.canSomersault = true;
					}
					if(P.controlLeft && MetroidMod.SenseMoveKey.Current)
					{
						SMoveEffect = 40;
						senseSound = true;
						P.velocity.X = left.X;
						P.velocity.Y += left.Y * P.gravDir;
						P.direction = -1;
						isSenseMoving = true;
					}
					else if(P.controlRight && MetroidMod.SenseMoveKey.Current)
					{
						SMoveEffect = 40;
						senseSound = true;
						P.velocity.X = right.X;
						P.velocity.Y += right.Y * P.gravDir;
						P.direction = 1;
						isSenseMoving = true;
					}
					else
					{
						isSenseMoving = false;
						senseSound = false;
					}
				}
				else
				{
					isSenseMoving = false;
					senseSound = false;
				}
			}
			else
			{
				isSenseMoving = false;
				senseSound = false;
			}
			if(SMoveEffect > 0)
			{
				SMoveEffect--;
			}
			else
			{
				sMoveDir = 1;
			}
		}
        public void AddSpaceJumping(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			AddSpaceJump(player);
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
			mp.spaceJump = true;
		}
        public void AddSpaceJump(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
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
		}
		
		public bool CheckCollide(float offsetX, float offsetY)
		{
			return CheckCollide(player.position+new Vector2(offsetX,offsetY), player.width, player.height);
		}
		public bool CheckCollide(Vector2 Position, int Width, int Height)
		{
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
			num4 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tileSolidTop[(int)Main.tile[i, j].type])
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector.Y += 8f;
							num5 -= 8;
						}
						if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
						{
							if(Main.tile[i, j].slope() > 0)
							{
								if (Main.tile[i, j].slope() > 2)
								{
									if(Main.tile[i, j].slope() == 3 && Position.Y < vector.Y + (float)num5 - Math.Max(Position.X - vector.X, 0f))
									{
										return true;
									}
									if(Main.tile[i, j].slope() == 4 && Position.Y < vector.Y + (float)num5 - Math.Max((vector.X + 16f) - (Position.X + (float)Width), 0f))
									{
										return true;
									}
								}
								else
								{
									if(Main.tile[i, j].slope() == 1 && Position.Y + (float)Height > vector.Y + Math.Max(Position.X - vector.X, 0f))
									{
										return true;
									}
									if(Main.tile[i, j].slope() == 2 && Position.Y + (float)Height > vector.Y + Math.Max((vector.X + 16f) - (Position.X + (float)Width), 0f))
									{
										return true;
									}
								}
							}
							else
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

        public void AddSpeedBoost(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
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
					int SpBoost = Terraria.Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,SpeedBoostID,specialDmg/2,0,player.whoAmI);
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
				player.velocity.Y = 0f;
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
				player.controlJump = false;
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
				player.velocity.Y = 0f;
				player.maxFallSpeed = 0f;
				player.direction = 1;
				shineDischarge = 0;
				player.controlLeft = false;
				player.controlUp = true;
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
				player.velocity.Y = 0f;
				player.maxFallSpeed = 0f;
				player.direction = -1;
				shineDischarge = 0;
				player.controlRight = false;
				player.controlUp = true;
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
					proj = Terraria.Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,ShineSparkID,specialDmg,0,player.whoAmI);
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
		#endregion
		}
		
		public void Bomb(Player player)
		{
			if(bomb <= 0 && Main.mouseRight && !mouseRight && shineDirection == 0 && !player.mouseInterface)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
				int BombID = mod.ProjectileType("MBBomb");
				int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,BombID,bombDamage,0,player.whoAmI);
				Main.projectile[a].aiStyle = 0;
			}
			mouseRight = Main.mouseRight;
			if(bomb > 0)
			{
				bomb--;
			}
			if (!special && statCharge >= 100)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
				bomb = 90;
				int BombID = mod.ProjectileType("MBBomb");
				if(player.controlLeft)
				{
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-6,-2,BombID,bombDamage,0,player.whoAmI);
					int b = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-5.5f,-3.5f,BombID,bombDamage,0,player.whoAmI);
					int c = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-4.7f,-4.7f,BombID,bombDamage,0,player.whoAmI);
					int d = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-3.5f,-5.5f,BombID,bombDamage,0,player.whoAmI);
					int e = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-2,-6,BombID,bombDamage,0,player.whoAmI);
					Main.projectile[a].timeLeft = 60;
					Main.projectile[b].timeLeft = 70;
					Main.projectile[c].timeLeft = 80;
					Main.projectile[d].timeLeft = 90;
					Main.projectile[e].timeLeft = 100;
				}
				else if(player.controlRight)
				{
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,6,-2,BombID,bombDamage,0,player.whoAmI);
					int b = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,5.5f,-3.5f,BombID,bombDamage,0,player.whoAmI);
					int c = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,4.7f,-4.7f,BombID,bombDamage,0,player.whoAmI);
					int d = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,3.5f,-5.5f,BombID,bombDamage,0,player.whoAmI);
					int e = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,2,-6,BombID,bombDamage,0,player.whoAmI);
					Main.projectile[a].timeLeft = 60;
					Main.projectile[b].timeLeft = 70;
					Main.projectile[c].timeLeft = 80;
					Main.projectile[d].timeLeft = 90;
					Main.projectile[e].timeLeft = 100;
				}
				else if(player.controlDown && player.velocity.Y == 0)
				{
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y,0,0,BombID,bombDamage,0,player.whoAmI);
					int b = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-5f,BombID,bombDamage,0,player.whoAmI);
					int c = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-7,BombID,bombDamage,0,player.whoAmI);
					int d = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-8.5f,BombID,bombDamage,0,player.whoAmI);
					int e = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-10,BombID,bombDamage,0,player.whoAmI);
					Main.projectile[a].timeLeft = 40;
					Main.projectile[b].timeLeft = 50;
					Main.projectile[c].timeLeft = 60;
					Main.projectile[d].timeLeft = 70;
					Main.projectile[e].timeLeft = 80;
				}
				else if(player.controlDown && player.velocity.Y != 0)
				{
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+8,0,0,BombID,bombDamage,0,player.whoAmI);
					int b = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,BombID,bombDamage,0,player.whoAmI);
					int c = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-4.5f,BombID,bombDamage,0,player.whoAmI);
					int d = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,4,2,BombID,bombDamage,0,player.whoAmI);
					int e = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-4,2,BombID,bombDamage,0,player.whoAmI);
					Main.projectile[a].Kill();
					Main.projectile[b].aiStyle = 0;
					Main.projectile[b].timeLeft = 25;
					Main.projectile[c].timeLeft = 25;
					Main.projectile[d].timeLeft = 25;
					Main.projectile[e].timeLeft = 25;
				}
				else
				{
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-5,-2,BombID,bombDamage,0,player.whoAmI);
					int b = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-3,-4,BombID,bombDamage,0,player.whoAmI);
					int c = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-5,BombID,bombDamage,0,player.whoAmI);
					int d = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,3,-4,BombID,bombDamage,0,player.whoAmI);
					int e = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,5,-2,BombID,bombDamage,0,player.whoAmI);
					Main.projectile[a].timeLeft = 60;
					Main.projectile[b].timeLeft = 60;
					Main.projectile[c].timeLeft = 60;
					Main.projectile[d].timeLeft = 60;
					Main.projectile[e].timeLeft = 60;
				}
				special = true;
			}
		}
        public void MorphBallBasic(Player player)
		{
			for (int k = 0; k < 1000; k++)
			{
				if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].aiStyle == 7)
				{
					Main.projectile[k].Kill();
				}
			}
			//player.noItems = true;
			player.controlHook = false;
			player.controlUseItem = false;
			player.controlUseTile = false;
			player.noFallDmg = true;
			player.scope = false;
			if(ballstate)
			{
				player.width = Math.Abs(player.velocity.X) >= 10f ? 20: morphSize;
				//player.height = morphSize;
				//player.position.Y += Player.defaultHeight - player.height;
			}
			player.doubleJumpCloud = false;
			player.jumpAgainCloud = false;
			player.dJumpEffectCloud = false;
			player.doubleJumpBlizzard = false;
			player.jumpAgainBlizzard = false;
			player.dJumpEffectBlizzard = false;
			player.doubleJumpSandstorm = false;
			player.jumpAgainSandstorm = false;
			player.dJumpEffectSandstorm = false;
			player.doubleJumpFart = false;
			player.jumpAgainFart = false;
			player.dJumpEffectFart = false;
			player.rocketBoots = 0;
			player.rocketTime = 0;
			player.wings = 0;
			player.wingTime = 0;
			player.carpet = false;
			player.carpetTime = 0;
			player.canCarpet = false;
			if(player.gravity != 0f)
			{
				//player.maxFallSpeed += 2.5f;
			}
			if(player.velocity.Y == 0f)
			{
				player.runSlowdown *= 0.5f;
				player.moveSpeed += 0.5f;
			}

			int shinyblock = 700;
			int timez = (int)(Time%60)/10;
			Color brightColor = morphColorLights;
			Lighting.AddLight((int)((player.Center.X) / 16f), (int)((player.Center.Y) / 16f), (float)(brightColor.R/(shinyblock/(1+0.1*timez))), (float)(brightColor.G/(shinyblock/(1+0.1*timez))), (float)(brightColor.B/(shinyblock/(1+0.1*timez))));  

			if(Ibounce && !player.controlDown && !player.controlJump)
			{
				Vector2 value2 = player.velocity;
				player.velocity = Collision.TileCollision(player.position, player.velocity, player.width, player.height, false, false);		
				if (value2 != player.velocity)
				{
					if (player.velocity.Y != value2.Y && /*Math.Abs((double)value2.Y)*/value2.Y > 7f)
					{
						player.velocity.Y = value2.Y * -0.3f;
					}
				}
				player.fallStart = (int)(player.position.Y / 16f);
			}
			if(!spiderball)
			{
				int dis = 0;
				if (player.velocity.Y == 0)
				{
					int num2 = (int)(player.position.X / 16f) - 1;
					int num3 = (int)((player.position.X + (float)player.width) / 16f) + 2;
					int num4 = (int)(player.position.Y / 16f) - 1;
					int num5 = (int)((player.position.Y + (float)player.height) / 16f) + 2;
					if (num2 < 0)
					{
						num2 = 0;
					}
					if (num3 > Main.maxTilesX)
					{
						num3 = Main.maxTilesX;
					}
					if (num4 < 0)
					{
						num4 = 0;
					}
					if (num5 > Main.maxTilesY)
					{
						num5 = Main.maxTilesY;
					}
					for (int i = num2; i < num3; i++)
					{
						for (int j = num4; j < num5; j++)
						{
							if (Main.tile[i, j] != null && Main.tile[i, j].active() && !Main.tile[i, j].inActive() && (Main.tileSolid[(int)Main.tile[i, j].type] || (Main.tileSolidTop[(int)Main.tile[i, j].type] && Main.tile[i, j].frameY == 0)))
							{
								Vector2 vector4;
								vector4.X = (float)(i * 16);
								vector4.Y = (float)(j * 16);
								int num6 = 16;
								if (Main.tile[i, j].halfBrick())
								{
									vector4.Y += 8f;
									num6 -= 8;
								}
								if (player.position.X + (float)player.width >= vector4.X && player.position.X <= vector4.X + 16f && player.position.Y + (float)player.height >= vector4.Y && player.position.Y <= vector4.Y + (float)num6)
								{
									if(Main.tile[i, j].slope() > 0)
									{
										if (Main.tile[i, j].slope() == 1 && (!Main.tile[i + 1, j].active() || !Main.tileSolid[(int)Main.tile[i + 1, j].type]))
										{
											player.velocity.X += velY;
											velY = 0f;
										}
										else if (Main.tile[i, j].slope() == 2 && (!Main.tile[i - 1, j].active() || !Main.tileSolid[(int)Main.tile[i - 1, j].type]))
										{
											player.velocity.X -= velY;
											velY = 0f;
										}
									}
									else
									{
										dis++;
									}
								}
								else
								{
									dis++;
								}
								if(dis > 5)
								{
									velY = 0f;
									dis = 0;
								}
							}
						}
					}
				}
				else if(player.velocity.Y > 0)
				{
					velY = player.velocity.Y * 0.75f;
				}
			}
			else
			{
				velY = 0f;
			}
		}
        public enum Edge
		{
			Floor,
			Ceiling,
			Left,
			Right,
			None
		}

		// current edge
		static Edge CurEdge = Edge.None;

		// X is pressed
		static bool KeyX = false;

		// get the edge the player is currently on
		public Edge GetEdge(Player player)
		{
			if (CheckCollide(0f,1.1f+Math.Sign(player.velocity.Y)))
			{
				return Edge.Floor;
			}
			else if (CheckCollide(0f,-1.1f+Math.Sign(player.velocity.Y)))
			{
				return Edge.Ceiling;
			}
			else if (CheckCollide(-1.1f+Math.Sign(player.velocity.X),0f))
			{
				return Edge.Left;
			}
			else if (CheckCollide(1.1f+Math.Sign(player.velocity.X),0f))
			{
				return Edge.Right;
			}
			
			return Edge.None;
		}
		
		Vector2 spiderVelocity;

		public void DoFloor(Player player)
		{
			SpiderMovement(player);
			
			if(CheckCollide(spiderVelocity.X,0f))
			{
				if(spiderVelocity.X > 0f)
				{
					CurEdge = Edge.Right;
					return;
				}
				if(spiderVelocity.X < 0f)
				{
					CurEdge = Edge.Left;
					return;
				}
			}
			else if(!CheckCollide(0f,1f) && CheckCollide(-2f*Math.Sign(spiderVelocity.X),1f))
			{
				if(spiderVelocity.X > 0f)
				{
					CurEdge = Edge.Left;
					return;
				}
				if(spiderVelocity.X < 0f)
				{
					CurEdge = Edge.Right;
					return;
				}
			}
		}

		public void DoCeiling(Player player)
		{
			SpiderMovement(player);
			
			if(CheckCollide(spiderVelocity.X,0f))
			{
				if(spiderVelocity.X > 0f)
				{
					CurEdge = Edge.Right;
					return;
				}
				if(spiderVelocity.X < 0f)
				{
					CurEdge = Edge.Left;
					return;
				}
			}
			else if(!CheckCollide(0f,-1f) && CheckCollide(-2f*Math.Sign(spiderVelocity.X),-1f))
			{
				if(spiderVelocity.X > 0f)
				{
					CurEdge = Edge.Left;
					return;
				}
				if(spiderVelocity.X < 0f)
				{
					CurEdge = Edge.Right;
					return;
				}
			}
		}

		public void DoLeft(Player player)
		{
			SpiderMovement(player);
			
			if(CheckCollide(0f,spiderVelocity.Y))
			{
				if(spiderVelocity.Y > 0f)
				{
					CurEdge = Edge.Floor;
					return;
				}
				if(spiderVelocity.Y < 0f)
				{
					CurEdge = Edge.Ceiling;
					return;
				}
			}
			else if(!CheckCollide(-1f,0f) && CheckCollide(-1f,-2f*Math.Sign(spiderVelocity.Y)))
			{
				if(spiderVelocity.Y > 0f)
				{
					CurEdge = Edge.Ceiling;
					return;
				}
				if(spiderVelocity.Y < 0f)
				{
					CurEdge = Edge.Floor;
					return;
				}
			}
		}

		public void DoRight(Player player)
		{
			SpiderMovement(player);
			
			if(CheckCollide(0f,spiderVelocity.Y))
			{
				if(spiderVelocity.Y > 0f)
				{
					CurEdge = Edge.Floor;
					return;
				}
				if(spiderVelocity.Y < 0f)
				{
					CurEdge = Edge.Ceiling;
					return;
				}
			}
			else if(!CheckCollide(1f,0f) && CheckCollide(1f,-2f*Math.Sign(spiderVelocity.Y)))
			{
				if(spiderVelocity.Y > 0f)
				{
					CurEdge = Edge.Ceiling;
					return;
				}
				if(spiderVelocity.Y < 0f)
				{
					CurEdge = Edge.Floor;
					return;
				}
			}
		}
		
		float spiderSpeed = 0f;
		
		public void SpiderMovement(Player player)
		{
			player.velocity.X = 0f;
			player.velocity.Y = 1E-05f;
			
			player.position.X = (float)Math.Round(player.position.X,2);
			player.position.Y = (float)Math.Round(player.position.Y,2);
			
			if(player.controlLeft)
			{
				spiderSpeed = Math.Max(spiderSpeed-0.1f,-2f);
			}
			else if(player.controlRight)
			{
				spiderSpeed = Math.Min(spiderSpeed+0.1f,2f);
			}
			else
			{
				if(spiderSpeed > 0)
				{
					spiderSpeed = Math.Max(spiderSpeed-0.1f,0f);
				}
				else
				{
					spiderSpeed = Math.Min(spiderSpeed+0.1f,0f);
				}
			}
			
			Vector2 velocity = new Vector2(0.1f,0f);
			Vector2 velocity2 = new Vector2(0f,0.1f);
			if(CurEdge == Edge.Right)
			{
				velocity = new Vector2(0f,-0.1f);
				velocity2 = new Vector2(0.1f,0f);
			}
			if(CurEdge == Edge.Left)
			{
				velocity = new Vector2(0f,0.1f);
				velocity2 = new Vector2(-0.1f,0f);
			}
			if(CurEdge == Edge.Ceiling)
			{
				velocity = new Vector2(-0.1f,0f);
				velocity2 = new Vector2(0f,-0.1f);
			}
			velocity *= Math.Sign(spiderSpeed);
			
			int num = (int)(Math.Abs(spiderSpeed) * 10f);
			while(!CheckCollide(velocity.X,velocity.Y) && num > 0)
			{
				player.position.X += velocity.X;
				player.position.Y += velocity.Y;
				num--;
			}
			spiderVelocity = velocity;// * spiderSpeed;
			
			int num2 = 10;
			while(!CheckCollide(velocity2.X,velocity2.Y) && num2 > 0)
			{
				player.position.X += velocity2.X;
				player.position.Y += velocity2.Y;
				num2--;
			}
			
			if(CheckCollide(0f,0f))
			{
				player.position -= velocity2;
			}
		}

		public void SpiderBall(Player player)
		{
			// switch back/to spiderball if M is pressed
			if (MetroidMod.SpiderBallKey.JustPressed && !KeyX)
			{
				CurEdge = Edge.None;
				spiderball = !spiderball;
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpiderActivate"));
			}
			// disable spiderball when jumping
			if(player.controlJump && player.releaseJump)
			{
				CurEdge = Edge.None;
				spiderball = false;
			}
			
			// update KeyX so spiderball is only toggled once each key press
			KeyX = MetroidMod.SpiderBallKey.JustPressed;
	
			// get current edge
			Edge LastEdge = CurEdge;
			
			if (spiderball) // horizon
			{
				Ibounce = false;

				
				// edge action switch
				switch (CurEdge)
				{
					case Edge.Floor:
						DoFloor(player);
						break;
					case Edge.Ceiling:
						DoCeiling(player);
						break;
					case Edge.Left:
						DoLeft(player);
						break;
					case Edge.Right:
						DoRight(player);
						break;
					case Edge.None:
						CurEdge = GetEdge(player);
						break;
					default:
						break;
				}
				
				// if no solid tile is adjacent to the player
				if (!CheckCollide(player.position-new Vector2(3,3),player.width+6,player.height+6))
				{
					CurEdge = Edge.None;
				}
				// if the edge has changed, display the current edge

				if(CurEdge != Edge.None)
				{
					// render player's default movements effortless
					player.moveSpeed = 0f;
					player.maxRunSpeed = 0f;
					player.accRunSpeed = 0f;
					player.gravity = 0f;
					player.stairFall = true;
				}
				else
				{
					spiderVelocity = Vector2.Zero;
					spiderSpeed = 0f;
				}
			}
			else
			{
				spiderVelocity = Vector2.Zero;
				spiderSpeed = 0f;
				Ibounce = true;
			}
		}
		//int CFMoment = 0;
		public void PowerBomb(Player player)
		{
			if(statPBCh <= 0 && MetroidMod.PowerBombKey.JustPressed && shineDirection == 0)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayPowerBomb"));
				statPBCh = 200;
				int PBombID = mod.ProjectileType("PowerBomb");
				int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,PBombID,specialDmg/4,0,player.whoAmI);
			}
		}
		public void BoostBall(Player player)
		{
			if(MetroidMod.BoostBallKey.Current)
			{
				if(boostCharge <= 60)
				{
					boostCharge++;
				}
				if(soundDelay <= 0)
				{
					soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BoostBallStartup"));
				}
				if(soundDelay >= 306)
				{
					soundDelay = 210;
				}
				if(soundDelay == 210)
				{
					soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BoostBallLoop"));
				}
				soundDelay++;
			}
			else
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				if(boostCharge > 30)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BoostBallSound"));
					
					float mult = (float)boostCharge / 30f;
					
					if(spiderball && CurEdge != Edge.None)
					{
						if(CurEdge == Edge.Floor)
						{
							player.velocity.Y -= 9f;
						}
						if(CurEdge == Edge.Ceiling)
						{
							player.velocity.Y += 9f;
						}
						if(CurEdge == Edge.Left)
						{
							player.velocity.X += 9f;
							player.velocity.Y -= 1f;
						}
						if(CurEdge == Edge.Right)
						{
							player.velocity.X -= 9f;
							player.velocity.Y -= 1f;
						}
						CurEdge = Edge.None;
					}
					else
					{
						if(player.velocity.X == 0 && player.velocity.Y == 0)
						{
							player.velocity.X += 4f*mult * player.direction;
						}
						if(player.velocity.X > 0)
						{
							player.velocity.X += 4f*mult;
						}
						if(player.velocity.X < 0)
						{
							player.velocity.X -= 4f*mult;
						}
						if(player.velocity.Y > 0)
						{
							player.velocity.Y += 4f*mult;
						}
						if(player.velocity.Y < 0)
						{
							player.velocity.Y -= 4f*mult;
						}
					}
					boostEffect += boostCharge;
				}
				boostCharge = 0;
				soundDelay = 0;
			}
			if(boostEffect > 0)
			{
				player.armorEffectDrawShadow = true;
				boostEffect--;
			}
		}
		public void Drill(Player p, int drill)
		{
			if (!player.mouseInterface && drill > 0 && p.position.X / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetX && (p.position.X + (float)p.width) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetX && p.position.Y / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetY && (p.position.Y + (float)p.height) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetY)
			{
				if(Main.mouseLeft)
				{
					if (p.runSoundDelay <= 0)
					{
						Main.PlaySound(2, (int)p.position.X, (int)p.position.Y, 22);
						p.runSoundDelay = 30;
					}
					if (Main.rand.Next(6) == 0)
					{
						int num123 = Dust.NewDust(p.position + p.velocity * (float)Main.rand.Next(6, 10) * 0.1f, p.width, p.height, 31, 0f, 0f, 80, default(Color), 1.5f);
						Dust expr_5B99_cp_0 = Main.dust[num123];
						expr_5B99_cp_0.position.X = expr_5B99_cp_0.position.X - 4f;
						Main.dust[num123].noGravity = true;
						Main.dust[num123].velocity *= 0.2f;
						Main.dust[num123].velocity.Y = (float)(-(float)Main.rand.Next(7, 13)) * 0.15f;
					}
				}
				if (cooldownbomb == 0 && Main.mouseLeft && (!Main.tile[Player.tileTargetX, Player.tileTargetY].active() || (!Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && !Main.tileSolid[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 314)))
				{
					p.poundRelease = false;
				}
				if (Main.tile[Player.tileTargetX, Player.tileTargetY].active())
				{
					if (cooldownbomb == 0 && Main.mouseLeft)
					{
						if (drill > 0)
						{
							p.PickTile(Player.tileTargetX, Player.tileTargetY, drill);
						}
						cooldownbomb = 5;
					}
				}
			}
			if(cooldownbomb > 0)
			{
				if(!Main.mouseLeft)
				{
					p.poundRelease = true;
				}
				cooldownbomb--;
			}
		}
		
		public bool psuedoScrewActive = false;
		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"psuedoScrewAttackActive", psuedoScrewActive}
			};
		}
		public override void Load(TagCompound tag)
		{
			try
			{
				bool flag = tag.GetBool("psuedoScrewAttackActive");
				if(flag)
				{
					psuedoScrewActive = flag;
				}
			}
			catch{}
		}
    }
}
