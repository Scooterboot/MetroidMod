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
	public enum Edge
	{
		Floor,
		Ceiling,
		Left,
		Right,
		None
	}

	public partial class MPlayer : ModPlayer  
	{
		public bool morphBall = false;
		public bool ballstate = false;
		public const int morphSize = 12;
		public bool Ibounce = true;
		public float velY = 0f;
		public bool trap = false;
		public bool executeChange = false;
		int unMorphDir = 0;
		
		public int bomb = 0;
		public int cooldownbomb = 0;
		public int bombDamage = 10;
		public bool special = false;
		public float statPBCh = 0.0f;
		public static float maxPBCh = 200.0f;
		int powerReChargeDelay = 0;
		
		public int boostCharge = 0;
		public int boostEffect = 0;
		public int soundDelay = 0;
		public SoundEffectInstance soundInstance;
		
		public bool spiderball = false;
		
		public Color boostGold = Color.FromNonPremultiplied(255, 255, 0, 6);
		public Color boostYellow = Color.FromNonPremultiplied(255, 215, 0, 6);
		public Color morphColor = Color.White;
		public Color morphColorLights = Color.White;
		public Color morphItemColor = Color.White;
		
		float morphMaxRunSpeed = 3f;
		float morphAccRunSpeed = 3f;
		float morphRunAcceleration = 0.08f;
		int morphJumpHeight = 15;
		float morphJumpSpeed = 5.01f;
		
		public void ResetEffects_MorphBall()
		{
			if(!player.mount.Active || player.mount.Type != mod.MountType("MorphBallMount"))
			{
				morphBall = false;
			}
		}
		public void PreUpdate_MorphBall()
		{
			Player P = player;
			morphColor = P.shirtColor;
			morphColor.A = 255;
			morphColorLights = P.underShirtColor;
			morphColorLights.A = 255;
			morphItemColor = P.shirtColor;
			morphItemColor.A = 255;
			
			if(!morphBall)
			{
				player.width = 20;
				//player.height = 42;
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
		}
		bool mflag = false;
		public override void SetControls()
		{
			//ballstate = (player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount"));
			if(player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount"))
			{
				ballstate = true;
				unMorphDir = 0;
				if(CheckCollide(player.position-new Vector2((20-morphSize)/2,42-morphSize),20,42))
				{
					if(!CheckCollide(player.position-new Vector2((20-morphSize),42-morphSize),20,42))
					{
						unMorphDir = -1;
					}
					else if(!CheckCollide(player.position-new Vector2(0,42-morphSize),20,42))
					{
						unMorphDir = 1;
					}
					else
					{
						player.controlMount = false;
						player.releaseMount = false;
						mflag = false;
					}
				}
			}
			else
			{
				ballstate = false;
			}
			
			//morph ball transformation tweaks and effects
			if((player.miscEquips[3].type == mod.ItemType("MorphBall") || player.mount.Type == mod.MountType("MorphBallMount")) && player.controlMount && !shineActive)
			{
				if(mflag)
				{
					if(ballstate)
					{
						unMorphDir = 0;
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
					player.position.X += widthDiff - widthDiff*unMorphDir;
					
					rotation = 0f;
					player.fullRotation = 0f;
					for(int i = 0; i < oldPos.Length; i++)
					{
						oldPos[i] = new Vector2(player.position.X,player.position.Y+player.gfxOffY);
					}
					for(int i = 0; i < player.shadowPos.Length; i++)
					{
						player.shadowPos[i] = player.position;
					}
					
					mflag = false;
				}
			}
			else
			{
				mflag = true;
			}
			
			if(!ballstate)
			{
				unMorphDir = 0;
				boostCharge = 0;
				boostEffect = 0;
				spiderball = false;
			}
		}
		public void PostUpdateMiscEffects_MorphBall()
		{
			morphMaxRunSpeed = player.maxRunSpeed;
			morphAccRunSpeed = player.accRunSpeed;
			morphRunAcceleration = player.runAcceleration;
			
			if(player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount") && player.grappling[0] == -1)
			{
				/*//temporarily trick the game into thinking the player isn't on a mount so that the player can use their original move speed and jump height
				player.mount._active = false;
				ballstate = true;
				player.jumpAgainCloud = false;
				player.jumpAgainSandstorm = false;
				player.jumpAgainBlizzard = false;
				player.jumpAgainFart = false;
				player.jumpAgainSail = false;
				player.jumpAgainUnicorn = false;
				player.pulley = false;
				player.ropeCount = 10;
				statCharge = 0;*/
				
				ballstate = true;
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
		public void PostUpdateRunSpeeds_MorphBall()
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
				//player.mount._active = true;
				
				player.maxRunSpeed = morphMaxRunSpeed;
				player.accRunSpeed = morphAccRunSpeed;
				player.runAcceleration = morphRunAcceleration;
				Player.jumpHeight = morphJumpHeight;
				Player.jumpSpeed = morphJumpSpeed;
			}
			else
			{
				morphJumpHeight = Player.jumpHeight;
				morphJumpSpeed = Player.jumpSpeed;
			}
			
			player.altFunctionUse = ballstate ? -1 : player.altFunctionUse;
		}
		public void PostUpdate_MorphBall()
		{
			if(!morphBall)
			{
				ballstate = false;
				Ibounce = true;
				velY = 0f;
				trap = false;
				executeChange = false;
				bomb = 0;
				cooldownbomb = 0;
				special = false;
				boostCharge = 0;
				boostEffect = 0;
				soundDelay = 0;
				spiderball = false;
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
			player.controlHook = false;
			player.controlUseItem = false;
			//player.controlUseTile = false;
			player.noFallDmg = true;
			player.scope = false;
			if(ballstate)
			{
				player.width = Math.Abs(player.velocity.X) >= 10f ? 20: morphSize;
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
			if(player.velocity.Y == 0f)
			{
				player.runSlowdown *= 0.5f;
				player.moveSpeed += 0.5f;
			}

			int shinyblock = 700;
			int timez = (int)(Time%60)/10;
			Color brightColor = morphColorLights;
			Lighting.AddLight((int)((player.Center.X) / 16f), (int)((player.Center.Y) / 16f), (float)(brightColor.R/(shinyblock/(1+0.1*timez))), (float)(brightColor.G/(shinyblock/(1+0.1*timez))), (float)(brightColor.B/(shinyblock/(1+0.1*timez))));  

			if(!spiderball)
			{
				Ibounce = true;
				if (player.velocity.Y == 0)
				{
					int num2 = (int)MathHelper.Clamp((player.position.X / 16f) - 1, 0, Main.maxTilesX-1);
					int num3 = (int)MathHelper.Clamp(((player.position.X + (float)player.width) / 16f) + 2, 0, Main.maxTilesX-1);
					int num4 = (int)MathHelper.Clamp((player.position.Y / 16f) - 1, 0, Main.maxTilesY-1);
					int num5 = (int)MathHelper.Clamp(((player.position.Y + (float)player.height) / 16f) + 2, 0, Main.maxTilesY-1);
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
											Ibounce = false;
										}
										else if (Main.tile[i, j].slope() == 2 && (!Main.tile[i - 1, j].active() || !Main.tileSolid[(int)Main.tile[i - 1, j].type]))
										{
											player.velocity.X -= velY;
											velY = 0f;
											Ibounce = false;
										}
									}
								}
							}
						}
					}
					velY = 0f;
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
			
			if(Ibounce && !player.controlDown && !player.controlJump)
			{
				Vector2 value2 = player.velocity;
				player.velocity = Collision.TileCollision(player.position, player.velocity, player.width, player.height, false, false);		
				if (value2 != player.velocity)
				{
					if (player.velocity.Y != value2.Y && value2.Y > 7f)//Math.Abs((double)value2.Y) > 7f)
					{
						player.velocity.Y = value2.Y * -0.3f;
					}
					player.fallStart = (int)(player.position.Y / 16f);
				}
			}
		}
		public void Bomb(Player player, int BombID)
		{
			int bombCount = 0;
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].type == BombID && Main.projectile[i].owner == player.whoAmI)
				{
					bombCount++;
				}
			}
			if (player.whoAmI == Main.myPlayer && bomb <= 0 && bombCount < 3 && player.controlUseTile && player.releaseUseTile && !player.tileInteractionHappened && player.releaseUseItem && !player.controlUseItem && !player.mouseInterface && !CaptureManager.Instance.Active && !Main.HoveringOverAnNPC && !Main.SmartInteractShowingGenuine)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
				int a = Projectile.NewProjectile(player.Center.X,player.Center.Y,0,0,BombID,bombDamage,4f,player.whoAmI, 1);
				Main.projectile[a].aiStyle = 0;
				//bomb = 20;
			}

			if (player.whoAmI == Main.myPlayer && !special && statCharge >= 100)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
				bomb = 90;
				if(player.controlLeft)
				{
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -6, -2, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -5.5f, -3.5f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 70;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -4.7f, -4.7f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 80;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -3.5f, -5.5f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 90;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -2, -6, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 100;
				}
				else if(player.controlRight)
				{
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 6, -2, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 5.5f, -3.5f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 70;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 4.7f, -4.7f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 80;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 3.5f, -5.5f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 90;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 2, -6, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 100;
				}
				else if(player.controlDown && player.velocity.Y == 0)
				{
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 40;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -5, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 50;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -7, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -8.5f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 70;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -10, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 80;
				}
				else if(player.controlDown && player.velocity.Y != 0)
				{
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 2;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 25;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -4.5f, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 25;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 4, 2, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 25;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -4, 2, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 25;
				}
				else
				{
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -5, -2, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, -3, -4, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -5, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 3, -4, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(player.Center.X, player.Center.Y, 5, -2, BombID, bombDamage, 4f, player.whoAmI)].timeLeft = 60;
				}
				special = true;
			}

			if (bomb > 0)
				bomb--;
		}
		public void PowerBomb(Player player, int type, int damage)
		{
			if(player.whoAmI == Main.myPlayer && statPBCh <= 0 && MetroidMod.PowerBombKey.JustPressed && shineDirection == 0)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayPowerBomb"));
				statPBCh = 200;
				int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,type,damage,0,player.whoAmI);
			}
		}
		public void Drill(Player p, int drill)
		{
			bool noBuildFlag = false;
			for(int i = 0; i < p.buffType.Length; i++)
			{
				if(p.buffType[i] == BuffID.NoBuilding && p.buffTime[i] > 0)
				{
					noBuildFlag = true;
					break;
				}
			}
			if(p.noBuilding || noBuildFlag)
			{
				return;
			}
			
			if(!player.mouseInterface && drill > 0 && p.position.X / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetX && (p.position.X + (float)p.width) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetX && p.position.Y / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetY && (p.position.Y + (float)p.height) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetY)
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
				if (Main.tile[Player.tileTargetX, Player.tileTargetY].active() && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 26)
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
		public void BoostBall(Player player)
		{

			if(MetroidMod.BoostBallKey.Current && player.whoAmI == Main.myPlayer)
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
			else if(player.whoAmI == Main.myPlayer)
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				if(boostCharge > 20)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BoostBallSound"));
					
					float mult = Math.Max((float)boostCharge / 30f, 1.25f);
					
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
							float maxSpeed = player.maxRunSpeed + player.accRunSpeed + 4f*mult;
							float speedCap = Math.Max(maxSpeed-Math.Abs(player.velocity.X),0f);
							player.velocity.X += MathHelper.Clamp(4f*mult*player.direction,-speedCap,speedCap);
						}
						else
						{
							Vector2 boostedVel = Vector2.Normalize(player.velocity) * 4f*mult;
							float maxSpeed = player.maxRunSpeed + player.accRunSpeed + Math.Abs(boostedVel.X);
							float speedCap = Math.Max(maxSpeed-Math.Abs(player.velocity.X),0f);
							player.velocity.X += MathHelper.Clamp(boostedVel.X,-speedCap,speedCap);
							player.velocity.Y += boostedVel.Y;
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
		
		// Using these so that I don't have to write out the entire method every time
		public bool CheckCollide(float offsetX, float offsetY)
		{
			return CheckCollide(player.position+new Vector2(offsetX,offsetY), player.width, player.height);
		}
		public bool CheckCollide(Vector2 Position, int Width, int Height)
		{
			return CollideMethods.CheckCollide(Position,Width,Height);
		}
		
		// current edge
		public Edge CurEdge = Edge.None;
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
			// disable spiderball when jumping
			if(player.controlJump && player.releaseJump)
			{
				CurEdge = Edge.None;
				spiderball = false;
			}

			if (player.whoAmI == Main.myPlayer && MetroidMod.SpiderBallKey.JustPressed)
			{
				if (this.ballstate)
				{
					this.CurEdge = Edge.None;
					this.spiderball = !this.spiderball;
					Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/SpiderActivate"));
				}
			}

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
	}
}