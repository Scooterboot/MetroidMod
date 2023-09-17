using System;
using System.Linq;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
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
using MetroidMod.Content.MorphBallAddons;

//using MetroidMod.Content.NPCs;
//using MetroidMod.Content.Items;
//using MetroidMod.Common.Systems;

namespace MetroidMod.Common.Players
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
		private int unMorphDir = 0;
		
		public int bomb = 0;
		public int cooldownbomb = 0;
		public int bombDamage = 10;
		public bool special = false;
		public float statPBCh = 0.0f;
		public static float maxPBCh = 200.0f;
		private int powerReChargeDelay = 0;
		
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
		
		public void ResetEffects_MorphBall()
		{
			if(!Player.mount.Active || Player.mount.Type != ModContent.MountType<Content.Mounts.MorphBallMount>())
			{
				morphBall = false;
			}
		}
		public void PreUpdate_MorphBall()
		{
			Player P = Player;
			morphColor = P.shirtColor;
			morphColor.A = 255;
			morphColorLights = P.underShirtColor;
			morphColorLights.A = 255;
			morphItemColor = P.shirtColor;
			morphItemColor.A = 255;
			
			if(!morphBall)
			{
				Player.width = 20;
				//Player.height = 42;
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
		private bool mflag = false;
		public override void SetControls()
		{
			//ballstate = (Player.mount.Active && Player.mount.Type == mod.MountType("MorphBallMount"));
			if(Player.mount.Active && Player.mount.Type == ModContent.MountType<Content.Mounts.MorphBallMount>())
			{
				ballstate = true;
				unMorphDir = 0;
				if(CheckCollide(Player.position-new Vector2((20-morphSize)/2,42-morphSize),20,42))
				{
					if(!CheckCollide(Player.position-new Vector2((20-morphSize),42-morphSize),20,42))
					{
						unMorphDir = -1;
					}
					else if(!CheckCollide(Player.position-new Vector2(0,42-morphSize),20,42))
					{
						unMorphDir = 1;
					}
					else
					{
						Player.controlMount = false;
						Player.releaseMount = false;
						mflag = false;
					}
				}
			}
			else
			{
				ballstate = false;
			}
			
			//morph ball transformation tweaks and effects
			if((Player.miscEquips[3].type == ModContent.ItemType<Content.Items.Accessories.MorphBall>() || Player.mount.Type == ModContent.MountType<Content.Mounts.MorphBallMount>()) && Player.controlMount && !shineActive)
			{
				if(mflag)
				{
					if(ballstate)
					{
						unMorphDir = 0;
						SoundEngine.PlaySound(Sounds.Suit.MorphIn, Player.position);
					}
					else
					{
						SoundEngine.PlaySound(Sounds.Suit.MorphOut, Player.position);
					}
					for (int i = 0; i < 25; i++)
					{
						int num = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, DustID.WhiteTorch, 0f, 0f, 100, morphColor, 2f);
						Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
						Main.dust[num].scale *= 1.3f;
						Main.dust[num].noGravity = true;
						Main.dust[num].velocity += Player.velocity * 0.8f;
						Main.dust[num].noLight = true;
					}
					for (int j = 0; j < 15; j++)
					{
						int num = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, DustID.WhiteTorch, 0f, 0f, 100, morphColorLights, 1f);
						Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
						Main.dust[num].scale *= 1.3f;
						Main.dust[num].noGravity = true;
						Main.dust[num].velocity += Player.velocity * 0.8f;
						Main.dust[num].noLight = true;
					}
					int oldWidth = Player.width;
					Player.width = ballstate?morphSize:20;
					int newWidth = Player.width;
					float widthDiff = (float)(oldWidth - newWidth)*0.5f;
					Player.position.X += widthDiff - widthDiff*unMorphDir;
					
					rotation = 0f;
					Player.fullRotation = 0f;
					for(int i = 0; i < oldPos.Length; i++)
					{
						oldPos[i] = new Vector2(Player.position.X,Player.position.Y+Player.gfxOffY);
					}
					for(int i = 0; i < Player.shadowPos.Length; i++)
					{
						Player.shadowPos[i] = Player.position;
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
			if(Player.mount.Active && Player.mount.Type == ModContent.MountType<Content.Mounts.MorphBallMount>() && Player.grappling[0] == -1)
			{
				//temporarily trick the game into thinking the Player isn't on a mount so that the Player can use their original move speed and jump height
				Player.mount._active = false;
				ballstate = true;
				Player.canJumpAgain_Cloud = false;
				Player.canJumpAgain_Sandstorm = false;
				Player.canJumpAgain_Blizzard = false;
				Player.canJumpAgain_Fart = false;
				Player.canJumpAgain_Sail = false;
				Player.canJumpAgain_Unicorn = false;
				Player.pulley = false;
				Player.ropeCount = 10;
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
		/*public override void UpdateEquips()
		{
			if(Player.miscEquips[3].type == ModContent.ItemType<Content.Items.Accessories.MorphBall>())
			{
				Player.VanillaUpdateAccessory(Player.whoAmI, Player.miscEquips[3], Player.hideMisc[3], ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
			}
		}*/
		public override void PostUpdateEquips()
		{
			if(ballstate)
			{
				Player.spikedBoots = 0;
			}
		}
		public void PostUpdateRunSpeeds_MorphBall()
		{
			if(spiderball && CurEdge != Edge.None)
			{
				Player.moveSpeed = 0f;
				Player.maxRunSpeed = 0f;
				Player.accRunSpeed = 0f;
				Player.velocity.X = 0f;
			}
			
			if(ballstate)
			{
				//end morph ball mount trick
				Player.mount._active = true;
			}
			
			Player.altFunctionUse = ballstate ? 1 : Player.altFunctionUse;
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
		
		public void MorphBallBasic(Player Player)
		{
			for (int k = 0; k < 1000; k++)
			{
				if (Main.projectile[k].active && Main.projectile[k].owner == Player.whoAmI && Main.projectile[k].aiStyle == ProjAIStyleID.Hook)
				{
					Main.projectile[k].Kill();
				}
			}
			Player.controlHook = false;
			Player.controlUseItem = false;
			//Player.controlUseTile = false;
			Player.noFallDmg = true;
			Player.scope = false;
			if(ballstate)
			{
				Player.width = Math.Abs(Player.velocity.X) >= 10f ? 20: morphSize;
			}
			Player.hasJumpOption_Cloud = false;
			Player.canJumpAgain_Cloud = false;
			Player.isPerformingJump_Cloud = false;
			Player.hasJumpOption_Blizzard = false;
			Player.canJumpAgain_Blizzard = false;
			Player.isPerformingJump_Blizzard = false;
			Player.hasJumpOption_Sandstorm = false;
			Player.canJumpAgain_Sandstorm = false;
			Player.isPerformingJump_Sandstorm = false;
			Player.hasJumpOption_Fart = false;
			Player.canJumpAgain_Fart = false;
			Player.isPerformingJump_Fart = false;
			Player.rocketBoots = 0;
			Player.rocketTime = 0;
			Player.wings = 0;
			Player.wingTime = 0;
			Player.carpet = false;
			Player.carpetTime = 0;
			Player.canCarpet = false;
			if(Player.velocity.Y == 0f)
			{
				Player.runSlowdown *= 0.5f;
				Player.moveSpeed += 0.5f;
			}

			int shinyblock = 700;
			int timez = (int)(Time%60)/10;
			Color brightColor = morphColorLights;
			Lighting.AddLight((int)((Player.Center.X) / 16f), (int)((Player.Center.Y) / 16f), (float)(brightColor.R/(shinyblock/(1+0.1*timez))), (float)(brightColor.G/(shinyblock/(1+0.1*timez))), (float)(brightColor.B/(shinyblock/(1+0.1*timez))));  

			if(!spiderball)
			{
				Ibounce = true;
				if (Player.velocity.Y == 0)
				{
					int num2 = (int)MathHelper.Clamp((Player.position.X / 16f) - 1, 0, Main.maxTilesX-1);
					int num3 = (int)MathHelper.Clamp(((Player.position.X + (float)Player.width) / 16f) + 2, 0, Main.maxTilesX-1);
					int num4 = (int)MathHelper.Clamp((Player.position.Y / 16f) - 1, 0, Main.maxTilesY-1);
					int num5 = (int)MathHelper.Clamp(((Player.position.Y + (float)Player.height) / 16f) + 2, 0, Main.maxTilesY-1);
					for (int i = num2; i < num3; i++)
					{
						for (int j = num4; j < num5; j++)
						{
							if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && !Main.tile[i, j].IsActuated && (Main.tileSolid[(int)Main.tile[i, j].TileType] || (Main.tileSolidTop[(int)Main.tile[i, j].TileType] && Main.tile[i, j].TileFrameY == 0)))
							{
								Vector2 vector4;
								vector4.X = (float)(i * 16);
								vector4.Y = (float)(j * 16);
								int num6 = 16;
								if (Main.tile[i, j].IsHalfBlock)
								{
									vector4.Y += 8f;
									num6 -= 8;
								}
								if (Player.position.X + (float)Player.width >= vector4.X && Player.position.X <= vector4.X + 16f && Player.position.Y + (float)Player.height >= vector4.Y && Player.position.Y <= vector4.Y + (float)num6)
								{
									if(Main.tile[i, j].Slope > SlopeType.Solid)
									{
										if (Main.tile[i, j].Slope == SlopeType.SlopeDownLeft && (!Main.tile[i + 1, j].HasTile || !Main.tileSolid[(int)Main.tile[i + 1, j].TileType]))
										{
											Player.velocity.X += velY;
											velY = 0f;
											Ibounce = false;
										}
										else if (Main.tile[i, j].Slope == SlopeType.SlopeDownRight && (!Main.tile[i - 1, j].HasTile || !Main.tileSolid[(int)Main.tile[i - 1, j].TileType]))
										{
											Player.velocity.X -= velY;
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
				else if(Player.velocity.Y > 0)
				{
					velY = Player.velocity.Y * 0.75f;
				}
			}
			else
			{
				velY = 0f;
			}
			
			if(Ibounce && !Player.controlDown && !Player.controlJump)
			{
				Vector2 value2 = Player.velocity;
				Player.velocity = Collision.TileCollision(Player.position, Player.velocity, Player.width, Player.height, false, false);		
				if (value2 != Player.velocity)
				{
					if (Player.velocity.Y != value2.Y && value2.Y > 7f)//Math.Abs((double)value2.Y) > 7f)
					{
						Player.velocity.Y = value2.Y * -0.3f;
					}
					Player.fallStart = (int)(Player.position.Y / 16f);
				}
			}
		}
		public void Bomb(Player Player, int BombID, Item BombItem)
		{
			int bombCount = 0;
			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if(Main.projectile[i].active && Main.projectile[i].type == BombID && Main.projectile[i].owner == Player.whoAmI)
				{
					bombCount++;
				}
			}
			if (Player.whoAmI == Main.myPlayer && bomb <= 0 && bombCount < 3 && Player.controlUseTile && Player.releaseUseTile && !Player.tileInteractionHappened && Player.releaseUseItem && !Player.controlUseItem && !Player.mouseInterface && !CaptureManager.Instance.Active && !Main.HoveringOverAnNPC && !Main.SmartInteractShowingGenuine)
			{
				SoundEngine.PlaySound(Sounds.Suit.LayBomb, Player.position);
				int a = Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X,Player.Center.Y,0,0,BombID,bombDamage,4f,Player.whoAmI, 1);
				Main.projectile[a].aiStyle = 0;
				//bomb = 20;
			}

			if (Player.whoAmI == Main.myPlayer && !special && statCharge >= 100)
			{
				SoundEngine.PlaySound(Sounds.Suit.LayBomb, Player.position);
				bomb = 90;
				if(Player.controlLeft)
				{
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -6, -2, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -5.5f, -3.5f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 70;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -4.7f, -4.7f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 80;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -3.5f, -5.5f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 90;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -2, -6, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 100;
				}
				else if(Player.controlRight)
				{
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 6, -2, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 5.5f, -3.5f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 70;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 4.7f, -4.7f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 80;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 3.5f, -5.5f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 90;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 2, -6, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 100;
				}
				else if(Player.controlDown && Player.velocity.Y == 0)
				{
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, 0, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 40;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, -5, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 50;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, -7, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, -8.5f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 70;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, -10, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 80;
				}
				else if(Player.controlDown && Player.velocity.Y != 0)
				{
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, 0, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 2;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, 0, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 25;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, -4.5f, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 25;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 4, 2, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 25;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -4, 2, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 25;
				}
				else
				{
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -5, -2, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, -3, -4, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 0, -5, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 3, -4, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
					Main.projectile[Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X, Player.Center.Y, 5, -2, BombID, bombDamage, 4f, Player.whoAmI)].timeLeft = 60;
				}
				special = true;
			}

			if (bomb > 0)
			{
				bomb--;
			}
		}
		public void PowerBomb(Player Player, int type, int damage, Item BombItem)
		{
			if(Player.whoAmI == Main.myPlayer && statPBCh <= 0 && Systems.MSystem.PowerBombKey.JustPressed && shineDirection == 0)
			{
				SoundEngine.PlaySound(Sounds.Suit.LayPowerBomb, Player.position);
				statPBCh = 200;
				Projectile.NewProjectile(Player.GetSource_Accessory(BombItem), Player.Center.X,Player.Center.Y+4,0,0,type,damage,0,Player.whoAmI);
			}
		}
		public void Drill(Player p, int drill)
		{
			Item drills = p.GetBestPickaxe();
			bool noBuildFlag = false;
			if (Main.mouseLeft && !Player.mouseInterface)
			{
					drill = drills.pick;
				//p.controlUseItem = true;
			}
			for (int i = 0; i < p.buffType.Length; i++)
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
			
			if(!Player.mouseInterface && drill > 0 && p.position.X / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetX && (p.position.X + (float)p.width) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetX && p.position.Y / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetY && (p.position.Y + (float)p.height) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetY)
			{
				if(Main.mouseLeft)
				{
					if (p.runSoundDelay <= 0)
					{
						SoundEngine.PlaySound(SoundID.Item22, p.position);
						p.runSoundDelay = 30;
					}
					if (Main.rand.NextBool(6))
					{
						int num123 = Dust.NewDust(p.position + p.velocity * (float)Main.rand.Next(6, 10) * 0.1f, p.width, p.height, DustID.Smoke, 0f, 0f, 80, default(Color), 1.5f);
						Dust expr_5B99_cp_0 = Main.dust[num123];
						expr_5B99_cp_0.position.X -= 4f;
						Main.dust[num123].noGravity = true;
						Main.dust[num123].velocity *= 0.2f;
						Main.dust[num123].velocity.Y = (float)(-(float)Main.rand.Next(7, 13)) * 0.15f;
					}
				}
				if (cooldownbomb == 0 && Main.mouseLeft && (!Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile || (!Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].TileType] && !Main.tileSolid[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].TileType] && Main.tile[Player.tileTargetX, Player.tileTargetY].TileType != 314)))
				{
					p.poundRelease = false;
				}
				if (Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile && Main.tile[Player.tileTargetX, Player.tileTargetY].TileType != 26)
				{
					if (cooldownbomb == 0 && Main.mouseLeft)
					{
						if (drill > 0)
						{
							p.PickTile(Player.tileTargetX, Player.tileTargetY, drill);
						}
						cooldownbomb = (int)(drills.useTime * p.pickSpeed);
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
		public void BoostBall(Player Player)
		{

			if(Systems.MSystem.BoostBallKey.Current && Player.whoAmI == Main.myPlayer)
			{
				if(boostCharge <= 60)
				{
					boostCharge++;
				}
				if(soundDelay <= 0 && SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Suit.BoostBallStartup, Player.position), out ActiveSound result))
				{
					soundInstance = result.Sound;
				}
				if(soundDelay >= 306)
				{
					soundDelay = 210;
				}
				if(soundDelay == 210 && SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Suit.BoostBallLoop, Player.position), out result))
				{
					soundInstance = result.Sound;
				}
				soundDelay++;
			}
			else if(Player.whoAmI == Main.myPlayer)
			{
				if (soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				if(boostCharge > 20)
				{
					SoundEngine.PlaySound(Sounds.Suit.BoostBallSound, Player.position);
					
					float mult = Math.Max((float)boostCharge / 30f, 1.25f);
					
					if(spiderball && CurEdge != Edge.None)
					{
						if(CurEdge == Edge.Floor)
						{
							Player.velocity.Y -= 9f;
						}
						if(CurEdge == Edge.Ceiling)
						{
							Player.velocity.Y += 9f;
						}
						if(CurEdge == Edge.Left)
						{
							Player.velocity.X += 9f;
							Player.velocity.Y -= 1f;
						}
						if(CurEdge == Edge.Right)
						{
							Player.velocity.X -= 9f;
							Player.velocity.Y -= 1f;
						}
						CurEdge = Edge.None;
					}
					else
					{
						if(Player.velocity.X == 0 && Player.velocity.Y == 0)
						{
							float maxSpeed = Player.maxRunSpeed + Player.accRunSpeed + 4f*mult;
							float speedCap = Math.Max(maxSpeed-Math.Abs(Player.velocity.X),0f);
							Player.velocity.X += MathHelper.Clamp(4f*mult*Player.direction,-speedCap,speedCap);
						}
						else
						{
							Vector2 boostedVel = Vector2.Normalize(Player.velocity) * 4f*mult;
							float maxSpeed = Player.maxRunSpeed + Player.accRunSpeed + Math.Abs(boostedVel.X);
							float speedCap = Math.Max(maxSpeed-Math.Abs(Player.velocity.X),0f);
							Player.velocity.X += MathHelper.Clamp(boostedVel.X,-speedCap,speedCap);
							Player.velocity.Y += boostedVel.Y;
						}
					}
					boostEffect += boostCharge;
				}
				boostCharge = 0;
				soundDelay = 0;
			}
			if(boostEffect > 0)
			{
				Player.armorEffectDrawShadow = true;
				boostEffect--;
			}
		}
		
		// Using these so that I don't have to write out the entire method every time
		public bool CheckCollide(float offsetX, float offsetY)
		{
			return CheckCollide(Player.position+new Vector2(offsetX,offsetY), Player.width, Player.height);
		}
		public static bool CheckCollide(Vector2 Position, int Width, int Height)
		{
			return CollideMethods.CheckCollide(Position, Width, Height);
		}

		// current edge
		public Edge CurEdge = Edge.None;
		// get the edge the Player is currently on
		public Edge GetEdge(Player Player)
		{
			if (CheckCollide(0f,1.1f+Math.Sign(Player.velocity.Y)))
			{
				return Edge.Floor;
			}
			else if (CheckCollide(0f,-1.1f+Math.Sign(Player.velocity.Y)))
			{
				return Edge.Ceiling;
			}
			else if (CheckCollide(-1.1f+Math.Sign(Player.velocity.X),0f))
			{
				return Edge.Left;
			}
			else if (CheckCollide(1.1f+Math.Sign(Player.velocity.X),0f))
			{
				return Edge.Right;
			}
			
			return Edge.None;
		}
		private Vector2 spiderVelocity;
		public void DoFloor(Player Player)
		{
			SpiderMovement(Player);
			
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
		public void DoCeiling(Player Player)
		{
			SpiderMovement(Player);
			
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
		public void DoLeft(Player Player)
		{
			SpiderMovement(Player);
			
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
		public void DoRight(Player Player)
		{
			SpiderMovement(Player);
			
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
		internal float spiderSpeed = 0f;
		public void SpiderMovement(Player Player)
		{
			Player.velocity.X = 0f;
			Player.velocity.Y = 1E-05f;
			
			Player.position.X = (float)Math.Round(Player.position.X,2);
			Player.position.Y = (float)Math.Round(Player.position.Y,2);
			
			if(Player.controlLeft)
			{
				spiderSpeed = Math.Max(spiderSpeed-0.125f,-2f);
			}
			else if(Player.controlRight)
			{
				spiderSpeed = Math.Min(spiderSpeed+0.125f,2f);
			}
			else
			{
				if(spiderSpeed > 0)
				{
					spiderSpeed = Math.Max(spiderSpeed-0.125f,0f);
				}
				else
				{
					spiderSpeed = Math.Min(spiderSpeed+0.125f,0f);
				}
			}
			
			Vector2 velocity = new(0.125f,0f);
			Vector2 velocity2 = new(0f,0.125f);
			if(CurEdge == Edge.Right)
			{
				velocity = new(0f,-0.125f);
				velocity2 = new(0.125f,0f);
			}
			if(CurEdge == Edge.Left)
			{
				velocity = new Vector2(0f,0.125f);
				velocity2 = new Vector2(-0.125f,0f);
			}
			if(CurEdge == Edge.Ceiling)
			{
				velocity = new Vector2(-0.125f,0f);
				velocity2 = new Vector2(0f,-0.125f);
			}
			velocity *= Math.Sign(spiderSpeed);
			
			int num = (int)(Math.Abs(spiderSpeed) * 10f);
			while(!CheckCollide(velocity.X,velocity.Y) && num > 0)
			{
				Player.position.X += velocity.X;
				Player.position.Y += velocity.Y;
				num--;
			}
			spiderVelocity = velocity;// * spiderSpeed;
			
			int num2 = 10;
			while(!CheckCollide(velocity2.X,velocity2.Y) && num2 > 0)
			{
				Player.position.X += velocity2.X;
				Player.position.Y += velocity2.Y;
				num2--;
			}
			
			if(CheckCollide(0f,0f))
			{
				Player.position -= velocity2;
			}
		}
		public void SpiderBall(Player Player)
		{
			// disable spiderball when jumping
			if(Player.controlJump && Player.releaseJump)
			{
				CurEdge = Edge.None;
				spiderball = false;
			}

			if (Player.whoAmI == Main.myPlayer && Systems.MSystem.SpiderBallKey.JustPressed)
			{
				if (ballstate)
				{
					CurEdge = Edge.None;
					spiderball = !spiderball;
					SoundEngine.PlaySound(Sounds.Suit.SpiderActivate, Player.position);
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
						DoFloor(Player);
						break;
					case Edge.Ceiling:
						DoCeiling(Player);
						break;
					case Edge.Left:
						DoLeft(Player);
						break;
					case Edge.Right:
						DoRight(Player);
						break;
					case Edge.None:
						CurEdge = GetEdge(Player);
						break;
					default:
						break;
				}
				
				// if no solid tile is adjacent to the Player
				if (!CheckCollide(Player.position-new Vector2(3,3),Player.width+6,Player.height+6))
				{
					CurEdge = Edge.None;
				}
				// if the edge has changed, display the current edge

				if(CurEdge != Edge.None)
				{
					// render Player's default movements effortless
					Player.moveSpeed = 0f;
					Player.maxRunSpeed = 0f;
					Player.accRunSpeed = 0f;
					Player.gravity = 0f;
					Player.stairFall = true;
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
