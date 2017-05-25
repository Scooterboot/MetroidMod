using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
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
		public int boostCharge = 0;
		public int boostEffect = 0;
		public bool spiderball = false;
		public bool special = false;
		public int cooldownbomb = 0;
		public int bomb = 0;
		//public int powerbomb = 0;
		public bool Ibounce = true;
		public float velY = 0f;
		public bool mouseRight = false;
		public int soundDelay = 0;
		public SoundEffectInstance soundInstance;
		public bool trap = false;
		public bool executeChange = false;
	#endregion
		public Color currentMorphColor = Color.White;
		public Color currentMorphColor2 = Color.White;
        public Color boostGold = Color.FromNonPremultiplied(255, 255, 0, 6);
        public Color boostYellow = Color.FromNonPremultiplied(255, 215, 0, 6);
		public bool speedBoosting = false;
		int powerReChargeDelay = 0;
		public float statCharge = 0.0f;
		public static float maxCharge = 100.0f;
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
		//public bool xrayequipped = false;
		public bool speedBooster = false;
		int num = 0;
		int num2 = 0;
		//public int maxMissiles = 5;
		//public int statMissiles = 5;
		public Color morphColor = Color.White;
		public Color morphColorLights = Color.White;
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
		int shineDeActive = 0;
		float speedBuildUp = 0f;
		bool shineCharge = false;
		int shineDeCharge = 0;
		//int manaDelay = 0;
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
		//public float maxDist;
		//public int grapplingBeam = -1;
		public bool grappleBeamIsHooked = false;
		#endregion
		int counter = 0;
		//SoundEffectInstance concentrationSound;
		bool soundPlayed = false;
		//bool isConcentrating = false;
		public float maxOverheat = 100f;
		public float statOverheat = 0f;
		public float extraOverheat = 0f;
		public float overheatCost = 1f;
		public int overheatDelay = 0;
		public int specialDmg = 100;
		public bool phazonImmune = false;
		public int phazonRegen = 0;
		int tweak = 0;
		int tweak2 = 0;
		public double Time = 0;
		public override void PreUpdate()
		{
			oldPosition = player.position;
			speedBoosting = false;
		/*	if(grapplingBeam >= 0 && !Main.projectile[grapplingBeam].active)
			{
				grapplingBeam = -1;
			}*/
			isPowerSuit = false;
			phazonImmune = false;
			phazonRegen = 0;
			thrusters = false;
			spaceJump = false;
		//	xrayequipped = false;
			speedBooster = false;
			morphBall = false;
			if (player.ownedProjectileCounts[mod.ProjectileType("GrappleBeamShot")] <= 0)
			{
			grappleBeamIsHooked = false;
			}
			visorGlow = false;
			visorGlowColor = new Color(255, 255, 255);
			maxOverheat = 100f;
			overheatCost = 1f;
			specialDmg = 100;
			Player P = player;
			morphColor = (P.shirtColor.R+P.shirtColor.G+P.shirtColor.B < P.underShirtColor.R+P.underShirtColor.G+P.underShirtColor.B)?P.shirtColor:P.underShirtColor;
			morphColor.A = 255;
			morphColorLights = (P.shirtColor.R+P.shirtColor.G+P.shirtColor.B >= P.underShirtColor.R+P.underShirtColor.G+P.underShirtColor.B)?P.shirtColor:P.underShirtColor;
			morphColorLights.A = 255;
			somersault = (!P.dead && (SMoveEffect > 0 || canSomersault) && !P.mount.Active && P.velocity.Y != 0 && P.velocity.X != 0 && P.itemAnimation == 0 && P.releaseHook && P.grapCount == 0 && !grappleBeamIsHooked && shineDirection == 0 && !shineActive && !ballstate && (((P.wingsLogic != 0 || P.rocketBoots != 0 || P.carpet) && (!P.controlJump || (!P.canRocket && !P.rocketRelease && P.wingsLogic == 0) || (P.wingTime <= 0 && P.rocketTime <= 0 && P.carpetTime <= 0))) || (P.wingsLogic == 0 && P.rocketBoots == 0 && !P.carpet)) && !P.sandStorm);
			somersault &= !(P.rocketDelay <= 0 && P.wingsLogic > 0 && P.controlJump && P.velocity.Y > 0f && P.wingTime <= 0);
			//isConcentrating = false;
			if(!morphBall)
			{
				player.width = 20;
				//player.height = 42;
			}
		
		
		//int soundDelay2 = 42;

			//MPlayer mPlayer = player.GetModPlayer<MPlayer>(mod);
			if(shineActive || shineDirection != 0)
			{
				player.gravity = Player.defaultGravity;
			}
			//player.grabRange += (int)(statCharge * 1.6f);
			if(player.velocity.Y == 0 || player.sliding || (player.autoJump && player.justJumped) || player.grappling[0] >= 0 || grappleBeamIsHooked)
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
				if(extraOverheat <= 0 && statCharge <= 0 && shineDirection <= 0 && !shineActive && overheatDelay <= 0)
				{
					statOverheat -= 0.5f;
				}
			}
			else
			{
				statOverheat = 0f;
			}
			if(statOverheat > maxOverheat)
			{
				extraOverheat = statOverheat - maxOverheat;
				statOverheat = maxOverheat;
			}
			if(extraOverheat > 0)
			{
				if(statCharge <= 0 && shineDirection <= 0 && !shineActive && overheatDelay <= 0)
				{
					extraOverheat -= 0.5f;
				}
			}
			else
			{
				extraOverheat = 0;
			}
		/*	for (int i = 0; i < player.inventory.Length; i++)
			{
				Terraria.Item I = player.inventory[i];
				if(I.type == mod.ItemType("HyperBeamAddon").type)
				{
					I.color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
			}*/
			int colorcount = 17;
			if (style == 0)
			{
				g += colorcount;
				if (g >= 255)
				{
					g = 255;
					style++;
				}
				r -= colorcount;
				if (r <= 0)
				{
					r = 0;
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
				if (g <= 0)
				{
					g = 0;
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
				if (b <= 0)
				{
					b = 0;
				}
			}
			Time += 1.0;
			if(Time > 54000.0)
			{
				Time = 0;
			}
			if(player.statMana < 0)
			{
				player.statMana = 0;
			}
			if(currentMorphColor2 == Color.White)
			{
				currentMorphColor2 = morphColorLights;
			}
			if(currentMorphColor == Color.White)
			{
				currentMorphColor = morphColor;
			}

			bool trail = (!player.dead && !player.mount.Active && player.grapCount == 0 && !grappleBeamIsHooked) && shineDirection == 0 && !shineActive && !ballstate);
			if(trail && ((player.velocity.Y < 0f && player.gravDir == 1) || (player.velocity.Y > 0f && player.gravDir == -1)) && isPowerSuit)
			{
				tweak++;
				if(tweak > 4)
				{
					tweak = 5;
                     player.armorEffectDrawShadowSubtle = true;
				}
			}
			else
			{
				tweak = 0;
			}

			if(speedBooster)
			{
				if(player.velocity.X <= -5f && player.controlLeft)
				{
					if(player.velocity.Y == 0)
					{
						player.jumpSpeedBoost -= (player.velocity.X/5f);
					}
					if(player.controlJump)
					{
						if(player.velocity.Y == 0 && player.releaseJump)
						{
							num = 180;
						}
						if(num > 0)
						{
							if(!player.slowFall)
							{
								player.gravity /= 3f;
							}
						}
					}
					else
					{
						num = 0;
					}
				}
				else if(player.velocity.X >= 5f && player.controlRight)
				{
					if(player.velocity.Y == 0)
					{
						player.jumpSpeedBoost += (player.velocity.X/5f);
					}
					if(player.controlJump)
					{
						if(player.velocity.Y == 0 && player.releaseJump)
						{
							num = 180;
						}
						if(num > 0)
						{
							if(!player.slowFall)
							{
								player.gravity /= 3f;
							}
						}
					}
					else
					{
						num = 0;
					}
				}
			}
			if(num > 0)
			{
				num--;
			}
			/*if(visorGlow && !ballstate)
			{
                 Vector2 face = new Vector2((int)((float)player.Center.X/16f), (int)((float)(player.position.Y+8f)/16f));
				Lighting.AddLight(face, (visorGlowColor)*0.375f);
			}*/
			if(jet)
			{
				//Color flame = new Color(153, 99, 61);
				Lighting.AddLight((int)((float)player.Center.X/16f), (int)((float)player.Center.Y/16f), 0.6f, 0.38f, 0.24f);
			}
			if(somersault)
			{
				rotation += ((rotateCountX + rotateCountY) * player.direction * player.gravDir * sMoveDir);
				if(rotation > (Math.PI*2))
				{
					rotation -= (float)(Math.PI*2);
				}
				if(rotation < -(Math.PI*2))
				{
					rotation += (float)(Math.PI*2);
				}
				player.fullRotation = rotation;
				player.fullRotationOrigin = player.Center - player.position;
			}
			else if(shineDirection == 2 || shineDirection == 4)
			{
				rotation = 0.1f * player.direction * player.gravDir;
				player.fullRotation = rotation;
				player.fullRotationOrigin = player.Center - player.position;
			}
			else if(shineDirection == 1 || shineDirection == 3)
			{
				rotation = (6.28318531f/8f) * player.direction * player.gravDir;
				player.fullRotation = rotation;
				player.fullRotationOrigin = player.Center - player.position;
			}
			else
			{
				rotation = 0f;
				if(player.mount.Type != 6)
				{
					player.fullRotation = 0f;
				}
			}
			if(spaceJump && SMoveEffect <= 0)
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
						//int num5 = 0;
						//int type = (int)Main.tile[i, j].type;
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
        public override void PostUpdate()
		{
			Player P = player;
			Terraria.Item I = P.inventory[P.selectedItem];
		/*	if(I.type == mod.ItemType("PowerBeam").type || I.type == mod.ItemType("MissileLauncher").type)
			{
				MItem mi = I.GetSubClass<MItem>();
				if(mi.texture != null)
				{
					P.itemLocation.X = P.Center.X - (float)mi.texture.Width * 0.5f;
					P.itemLocation.Y = P.Center.Y - (float)mi.texture.Height * 0.5f;
					if(MBase.AltBeamSkins && mi.textureAlt != null)
					{
						P.itemLocation.X = P.Center.X - (float)mi.textureAlt.Width * 0.5f;
						P.itemLocation.Y = P.Center.Y - (float)mi.textureAlt.Height * 0.5f;
					}
				}
			}
			if(I.type == mod.ItemType("NovaLaser").type)
			{
				P.itemLocation.X = P.Center.X - (float)Main.itemTexture[I.type].Width * 0.5f;
				P.itemLocation.Y = P.Center.Y - (float)Main.itemTexture[I.type].Height * 0.5f;
			}*/

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
				shineCharge = false;
				shineDeCharge = 0;
				//manaDelay = 0;
				shineSound = 0;
				shineDirection = 0;
				shineDeActive = 0;
				shineActive = false;
			}
		}
        public float ballrot = 0f;
		public static int oldNumMax = 10;
		public Vector2[] oldPos = new Vector2[oldNumMax];
        public void DrawBallTexture(SpriteBatch sb, Texture2D mytex, Texture2D mytex2, Texture2D mytex3, Texture2D boosttex, Texture2D trail, Player drawPlayer)
		{
			//TAPI.PlayerLayer.ExtraDrawInfo edi = PlayerLayer.extraDrawInfo;
			//MPlayer mPlayer = drawPlayer.getModPlayer<Mplayer>(mod);
			//sb.Draw(tex, new Vector2((float)((int)(drawPlayer.position.X - Main.screenPosition.X - (float)(frame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawPlayer.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)frame.Height + 4f))) + drawPos + edi.vector2, new Rectangle?(frame), Color.White, rotation, edi.vector2, 1f, edi.effects, 0f);
			float thisx = (float)((int)(drawPlayer.position.X + (float)(drawPlayer.width / 2) - Main.screenPosition.X));
			float thisy = (float)((int)(drawPlayer.position.Y + (float)(drawPlayer.height / 2) - Main.screenPosition.Y));
			Vector2 ballDims = new Vector2(28f,28f);
			Vector2 thispos =  new Vector2(thisx,thisy);
			//int timez = (int)(Main.time%60)/10;
			//if(drawPlayer.shadow == 0f)
			//{
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
					ballrotoffset+= 0.05f*drawPlayer.velocity.X;
					else
					ballrotoffset += 0.25f*drawPlayer.direction;
				}
				else if (drawPlayer.velocity.X < 0f)
					ballrotoffset -= 0.2f;
				else if ( drawPlayer.velocity.X > 0f)
					ballrotoffset += 0.2f;
				//if(drawPlayer.hitTile > 0)
				//{
					if(drawPlayer.velocity.X != 0f)
					ballrotoffset+= 0.025f*drawPlayer.velocity.X;
					else
					ballrotoffset += 0.125f*drawPlayer.direction;
				//}
				ballrot+=ballrotoffset;
				if(ballrot > (float)(Math.PI)*2)
					ballrot -= (float)(Math.PI)*2;
				if(ballrot < -(float)(Math.PI)*2)
					ballrot += (float)(Math.PI)*2;
				Color brightColor = morphColorLights;
				Color darkColor = Lighting.GetColor((int)((double)drawPlayer.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawPlayer.position.Y + (double)drawPlayer.height * 0.5) / 16, morphColor);
				currentMorphColor = darkColor;
				currentMorphColor2 = brightColor;
				float scale = 0.57f;
				int offset = 4;
				for (int num46 = oldPos.Length - 1; num46 > 0; num46--)
				{
					Vector2 vect = oldPos[1] - oldPos[0];
					oldPos[num46] = oldPos[num46 - 1] - (vect*0.5f);
				}
				oldPos[0] = thispos + Main.screenPosition;
				//oldRot[0] = (float)Math.Atan2((double)drawPlayer.velocity.Y, (double)drawPlayer.velocity.X);
				if (ballstate && drawPlayer.active && !drawPlayer.dead)
				{
					if(drawPlayer.velocity.X != 0 || drawPlayer.velocity.Y != 0)
					{
						for (int num88 = 0; num88 < oldPos.Length; num88++)
						{
							Color color23 = brightColor;
							color23 *= (float)(oldPos.Length - (num88)) / 15f;
							sb.Draw(trail, oldPos[num88] - Main.screenPosition, new Rectangle?(new Rectangle(0,0,trail.Width, trail.Height)), color23, ballrot, ballDims/2, scale, effects, 0f);
						}
					}
					if(shineDirection != 0)
					{
						currentMorphColor = Color.Yellow;
						sb.Draw(mytex, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), Color.Yellow,ballrot,ballDims/2, scale, effects, 0f);
						sb.Draw(mytex2, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), brightColor,ballrot,ballDims/2, scale, effects, 0f);
					}
					else
					{
						if(speedBoosting && shineDirection == 0)
						{
							currentMorphColor = new Color(51,70,179);
							sb.Draw(mytex3, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), Color.White,ballrot,ballDims/2, scale, effects, 0f);
							sb.Draw(mytex2, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), brightColor,ballrot,ballDims/2, scale, effects, 0f);
						}
						else
						{
							sb.Draw(mytex, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), darkColor,ballrot,ballDims/2, scale, effects, 0f);
							sb.Draw(mytex2, thispos, new Rectangle?(new Rectangle(0,((int)ballDims.Y+offset)*timez,(int)ballDims.X, (int)ballDims.Y)), brightColor,ballrot,ballDims/2, scale, effects, 0f);
						}
					}
					if(boostEffect > 0)
					{
						for (int i = 0; i < boostEffect; i++)
						{
							sb.Draw(boosttex, thispos, new Rectangle?(new Rectangle(0,0,boosttex.Width,boosttex.Height)), boostGold * 0.5f,ballrot,ballDims/2, scale, effects, 0f);
						}
					}
					else if(boostCharge > 0)
					{
						for (int i = 0; i < boostCharge; i++)
						{
							sb.Draw(boosttex, thispos, new Rectangle?(new Rectangle(0,0,boosttex.Width,boosttex.Height)), boostYellow * 0.5f,ballrot,ballDims/2, scale, effects, 0f);
						}
					}
					Texture2D spiderTex = mod.GetTexture("Gore/Spiderball");
					if(spiderball)
					{
						sb.Draw(spiderTex, thispos, new Rectangle?(new Rectangle(0,0,spiderTex.Width,spiderTex.Height)), brightColor,ballrot,new Vector2(spiderTex.Width/2,spiderTex.Height/2), scale, effects, 0f);
					}
				}
			//}
		}
        const int shinyblock = 700;
        public void AddSpaceJumping(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			AddSpaceJump(player);
			if(mp.statSpaceJumps >= 15 && !mp.ballstate && player.grappling[0] == -1  && mp.spaceJumped && !player.jumpAgainCloud && !player.jumpAgainBlizzard && !player.jumpAgainSandstorm && !player.jumpAgainFart && player.jump == 0 && player.velocity.Y != 0f && player.rocketTime == 0 && player.wingTime == 0f && !player.mount.Active)
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
			//player.noFallDmg = true;
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			if(player.velocity.Y == 0f || player.sliding || (player.autoJump && player.justJumped) || player.grappling[0] >= 0 || grappleBeamIsHooked)
			{
				mp.spaceJumped = false;
				mp.canSomersault = false;
			}
			else if(!mp.ballstate)
			{
				if(player.controlJump && player.releaseJump && !mp.spaceJumped && player.grappling[0] == -1 && player.jump <= 0)
				{
					int num167 = player.height;
					if (player.gravDir == -1f)
					{
						num167 = 4;
					}
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
				}
				else if(System.Math.Abs(player.velocity.X) > 4.25f)
				{
					mp.canSomersault = true;
				}
				else if(!mp.spaceJumped)
				{
					mp.canSomersault = false;
				}
			}
		}
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
				mPlayer.DrawBallTexture(spriteBatch, tex, tex2, tex3, boost, trail, drawPlayer);
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
						mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyOrigin, drawInfo.bodyColor, drawInfo.bodyArmorShader);
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
			if (mPlayer.jet && !drawPlayer.sandStorm && drawPlayer.shadow == 0f && mPlayer.thrusters)
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
				float yfloat = 4f;
				if (drawPlayer.mount.Active)
				{
					yfloat -= drawPlayer.mount._data.yOffset/32 + drawPlayer.mount._data.playerYOffsets[drawPlayer.mount._frame];
				}
			DrawData item = new DrawData(tex, new Vector2((float)((int)(drawPlayer.position.X - Main.screenPosition.X - (float)(frame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawPlayer.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)frame.Height + yfloat))) + drawPos + origin, new Rectangle?(frame), color, rot, origin, 1f, effects, 0);
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
			float whyfloat = 4f;
			if (drawPlayer.mount.Active)
			{
				whyfloat -= drawPlayer.mount._data.yOffset/32 + drawPlayer.mount._data.playerYOffsets[drawPlayer.mount._frame];
			}
			Main.playerDrawData.Add(new DrawData(tex, new Vector2((float)((int)(drawPlayer.position.X - Main.screenPosition.X - (float)(jetFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawPlayer.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)jetFrame.Height + whyfloat))) + drawPos + drawInfo.bodyOrigin, new Rectangle?(jetFrame), Color.White, rot, drawInfo.bodyOrigin, 1f, effects, 0));
		}
		public Color GetAlpha(Color newColor, float alphaReduction)
		{
			float num = (float)(255) / 255f;
			if (alphaReduction > 0f)
			{
				num *= 1f - alphaReduction;
			}
			return Color.Multiply(newColor, num);
		}
		public static readonly PlayerLayer visorLayer = new PlayerLayer("MetroidMod", "visorLayer", PlayerLayer.Head, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>(mod);
			if (mPlayer.isPowerSuit && !mPlayer.ballstate)
			{
				Texture2D tex = mod.GetTexture("Gore/VisorGlow");
				mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headOrigin, mPlayer.visorGlowColor, 0);
			}
		});
		//public void DrawTexture(SpriteBatch sb, PlayerDrawInfo drawInfo, Texture2D tex, Player drawPlayer, Rectangle frame, float rot, Vector2 drawPos, Vector2 origin, Color color, int shader)
       public static readonly PlayerLayer screwAttackLayer = new PlayerLayer("MetroidMod", "screwAttackLayer", PlayerLayer.FrontAcc, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>(mod);
			if (mPlayer.somersault && mPlayer.screwAttack && P.shadow == 0f)
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
						float num100 = (float)(tex.Width - projectile.width) * 0.5f + (float)projectile.width * 0.5f;
						spriteBatch.Draw(tex, new Vector2(projectile.position.X - Main.screenPosition.X + num100, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, y9, tex.Width, num121 - 1)), alpha, -mPlayer.rotation, new Vector2(num100, (float)(projectile.height / 2)), projectile.scale, effects, 0);
						if(mPlayer.screwAttackSpeedEffect > 0)
						{
							mPlayer.color21 = alpha;
							if(mPlayer.screwAttackSpeedEffect <= 30)
							{
								mPlayer.color21 = mPlayer.GetAlpha(alpha, ((float)(30-mPlayer.screwAttackSpeedEffect)/30f));
							}
							spriteBatch.Draw(tex2, new Vector2(projectile.position.X - Main.screenPosition.X + num100, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, y9, tex2.Width, num121 - 1)), mPlayer.color21, -mPlayer.rotation, new Vector2(num100, (float)(projectile.height / 2)), projectile.scale, effects, 0);
							Texture2D tex3 = mod.GetTexture("Gore/ScrewAttack_YellowPlayerGlow");
							Main.playerDrawData.Add(new DrawData(tex3, new Vector2(projectile.position.X - Main.screenPosition.X + num100, projectile.position.Y - Main.screenPosition.Y + (float)(projectile.height / 2) + projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex3.Width, tex3.Height)), mPlayer.color21, 0f, new Vector2(num100, (float)(projectile.height / 2)), projectile.scale, effects, 0));
						}
					}
				}
			}
		});
		//public void DrawTexture(SpriteBatch sb, PlayerDrawInfo drawInfo, Texture2D tex, Player drawPlayer, Rectangle frame, float rot, Vector2 drawPos, Vector2 origin, Color color, int shader)
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			MPlayer mPlayer = player.GetModPlayer<MPlayer>(mod);
			Player P = player;
	//		PlayerLayer.Add(list, gunItemLayer, PlayerLayer.LayerHeldItem, false, false);
    			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k] == PlayerLayer.FrontAcc)
				{
					k++;
					layers.Insert(k + 1, ballLayer);
					k++;
					layers.Insert(k + 1, screwAttackLayer);
				}
					if (layers[k] == PlayerLayer.Body)
				{
					k++;
					layers.Insert(k + 1, thrusterLayer);
					k++;
					layers.Insert(k + 1, jetLayer);
					k++;
				}
				if (layers[k] == PlayerLayer.Head)
				{
					k++;
					layers.Insert(k + 1, visorLayer);

				}
			}
			Terraria.Item I = P.inventory[P.selectedItem];
			int frame = (int)(P.bodyFrame.Y/P.bodyFrame.Height);
	/*	if ((I.type == ItemDef.byName["MetroidMod:PowerBeam"].type || I.type == ItemDef.byName["MetroidMod:MissileLauncher"].type || I.type == ItemDef.byName["MetroidMod:NovaLaser"].type) && P.itemAnimation == 0 && (frame < 1 || frame > 4))
			{
				PlayerLayer.Add(list, gunLayer, PlayerLayer.LayerArmsFront, false, false);
			}*/
	//		PlayerLayer.Add(list, screwAttackLayer, PlayerLayer.LayerAccessoryFront, false, false);
			//TAPI.PlayerLayer.ExtraDrawInfo edi = PlayerLayer.extraDrawInfo;
			if(somersault)
			{
				//P.fullRotation = rotation;
				//P.fullRotationOrigin = P.Center - P.position;
				P.bodyFrame.Y = P.bodyFrame.Height * 6;
				P.legFrame.Y = P.legFrame.Height * 7;
				P.wingFrame = 1;
				if (P.wings == 4)
				{
					P.wingFrame = 3;
				}
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
				//rotation = 0.1f * P.direction * P.gravDir;
				//P.fullRotation = rotation;
				//P.fullRotationOrigin = P.Center - P.position;
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
				//rotation = (MBase.degree360/8f) * P.direction * P.gravDir;
				//P.fullRotation = rotation;
				//P.fullRotationOrigin = P.Center - P.position;
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
				
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (P.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				}
				float targetrotation = (float)Math.Atan2((MY-P.Center.Y)*P.direction,(MX-P.Center.X)*P.direction);
				if(/*(XRayScope.xray.XRayActive(P) && MBase.HeadTracking == "X-Ray") ||*/ (!ballstate && ((P.direction == 1 && MX >= P.Center.X) || (P.direction == -1 && MX <= P.Center.X))))
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
				}
				(grappleBeamIsHooked && P.itemAnimation <= 0)
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
				}*/
			}
			/*if(flashActive)
			{
				P.bodyFrame.Y = P.bodyFrame.Height * 6;
				P.legFrameCounter = 0.0;
				P.legFrame.Y = P.legFrame.Height * 7;
				P.head = -1;
				P.body = -1;
				P.legs = -1;
				gunLayer.visible = false;
				jetLayer.visible = false;
				thrusterLayer.visible = false;
				P.drawAura = true;
			}*/
			if(ballstate)
			{
				PlayerLayer.Arms.visible = false;
				PlayerLayer.Head.visible = false;
                PlayerLayer.Face.visible = false;
                PlayerLayer.Hair.visible = false;
                PlayerLayer.HairBack.visible = false;
                PlayerLayer.Skin.visible = false;
				PlayerLayer.Body.visible = false;
				PlayerLayer.Legs.visible = false;
				PlayerLayer.Wings.visible = false;
				PlayerLayer.BackAcc.visible = false;
				PlayerLayer.BalloonAcc.visible = false;
				PlayerLayer.ShoeAcc.visible = false;
                PlayerLayer.HandOnAcc.visible = false;
				PlayerLayer.HandOffAcc.visible = false;
				PlayerLayer.WaistAcc.visible = false;
				PlayerLayer.NeckAcc.visible = false;
				PlayerLayer.ShieldAcc.visible = false;
				//PlayerLayer.FrontAcc.visible = false;
				PlayerLayer.MountBack.visible = false;
                PlayerLayer.HeldItem.visible = false;
                P.shadow = 0f;
	///			visorLayer.visible = false;
	///			gunLayer.visible = false;
	///			jetLayer.visible = false;
	///			thrusterLayer.visible = false;
				//P.drawAura = false;
			}
			else
			{
				if (thrusters)
				{
					if((P.wings == 0 && P.back == -1) || P.velocity.Y == 0f || mPlayer.shineDirection != 0)
					{
						PlayerLayer.Wings.visible = false;
						PlayerLayer.BackAcc.visible = false;
					}
				}
				//if(!flashActive)
				//{
		///			visorLayer.visible = true;
		///			gunLayer.visible = true;
		///			jetLayer.visible = true;
		///			thrusterLayer.visible = true;
				//}
			}
			//TAPI.PlayerLayer.ExtraDrawHeadInfo edhi = PlayerLayer.extraDrawHeadInfo;
			
			if(!thrusters)
			{
				jet = false;
			}
			/*#region test
			if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.G) < 0)
			{
				P.bodyFrame.Y = P.bodyFrame.Height*6;
			}
			if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.Y) < 0)
			{
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (P.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				}
				float targetrotation = (float)Math.Atan2((MY-P.Center.Y)*P.direction,(MX-P.Center.X)*P.direction);
				rotation = targetrotation;
				P.fullRotation = rotation;
				P.fullRotationOrigin = P.Center - P.position;
				P.bodyFrame.Y = P.bodyFrame.Height*6;
			}
			#endregion*/
		}
        public void MorphBallBasic(Player player)
		{
			if (player.grappling[0] >= 0 || grappleBeamIsHooked)
						{
							ballstate = false;
						}
			if (ballstate)
			{
				if(!special && statCharge >= 100)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
					bomb = 90;
					int BombID = mod.ProjectileType("MBBomb");
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-5,-2,BombID,10,0,player.whoAmI);
					int b = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,-3,-4,BombID,10,0,player.whoAmI);
					int c = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,-5,BombID,10,0,player.whoAmI);
					int d = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,3,-4,BombID,10,0,player.whoAmI);
					int e = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,5,-2,BombID,10,0,player.whoAmI);
					Main.projectile[a].timeLeft = 60;
					Main.projectile[b].timeLeft = 60;
					Main.projectile[c].timeLeft = 60;
					Main.projectile[d].timeLeft = 60;
					Main.projectile[e].timeLeft = 60;
					player.statMana -= 10;
					player.manaRegenDelay = (int)player.maxRegenDelay;
					special = true;
					statCharge = 0;
				}
				else
				{
					statCharge = 0;
				}
				player.noItems = true;
				player.noFallDmg = true;
				player.scope = false;
				player.width = Math.Abs(player.velocity.X) >= 7f ? 20: 14;
				player.height = 14;
				player.position.Y += Player.defaultHeight - player.height;
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
				player.spikedBoots = 0;
				//player.drawAura = false;
				player.maxFallSpeed += 2.5f;
				if(player.velocity.Y == 0f)
				{
					player.runSlowdown *= 0.5f;
				}
				//Color brightColor = currentMorphColor;
				//Lighting.AddLight((int)((player.Center.X) / 16f), (int)((player.Center.Y) / 16f), (float)(brightColor.R/127), (float)(brightColor.G/127), (float)(brightColor.B/127));  

				int timez = (int)(Time%60)/10;
				Color brightColor = currentMorphColor2;
				Lighting.AddLight((int)((player.Center.X) / 16f), (int)((player.Center.Y) / 16f), (float)(brightColor.R/(shinyblock/(1+0.1*timez))), (float)(brightColor.G/(shinyblock/(1+0.1*timez))), (float)(brightColor.B/(shinyblock/(1+0.1*timez))));  
				//bool flag = false;
				if(bomb <= 0 && Main.mouseRight && !mouseRight && shineDirection == 0)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
					int BombID = mod.ProjectileType("MBBomb");
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,BombID,10,0,player.whoAmI);
					Main.projectile[a].aiStyle = 0;
				}
				mouseRight = Main.mouseRight;
				if(bomb > 0)
				{
					bomb--;
				}
				if(Ibounce && !player.controlDown)
				{
					Vector2 value2 = player.velocity;
					player.velocity = Collision.TileCollision(player.position, player.velocity, player.width, player.height, false, false);		
					if (value2 != player.velocity)
					{
						if (player.velocity.Y != value2.Y && Math.Abs((double)value2.Y) > 7f)
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
											//flag = true;
											if (Main.tile[i, j].slope() == 1 && (!Main.tile[i + 1, j].active() || !Main.tileSolid[(int)Main.tile[i + 1, j].type]))
											{
												player.velocity.X += velY;
												velY = 0f;
												if(!player.controlLeft)
												{
													if (player.velocity.X < 0f)
													{
														player.velocity.X = player.velocity.X * 0.9f;
													}
													if (player.velocity.X < 3f)
													{
														player.velocity.X = player.velocity.X + 0.2f;
													}
													else
													{
														player.velocity.X = player.velocity.X + 0.1f;
													}
													if (player.velocity.X > 8)
													{
														player.velocity.X = 8;
													}
													if ((double)player.velocity.X < -0.1 || (double)player.velocity.X > 0.1)
													{
														player.velocity.X = player.velocity.X * 1.1f;
													}
												}
											}
											else if (Main.tile[i, j].slope() == 2 && (!Main.tile[i - 1, j].active() || !Main.tileSolid[(int)Main.tile[i - 1, j].type]))
											{
												player.velocity.X -= velY;
												velY = 0f;
												if(!player.controlRight)
												{
													if (player.velocity.X > 0f)
													{
														player.velocity.X = player.velocity.X * 0.9f;
													}
													if (player.velocity.X > -3f)
													{
														player.velocity.X = player.velocity.X - 0.2f;
													}
													else
													{
														player.velocity.X = player.velocity.X - 0.1f;
													}
													if (player.velocity.X < -8)
													{
														player.velocity.X = -8;
													}
													if ((double)player.velocity.X < -0.1 || (double)player.velocity.X > 0.1)
													{
														player.velocity.X = player.velocity.X * 1.1f;
													}
												}
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
										//Ibounce = true;
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
				/*if(!flag)
				{
					player.moveSpeed += 0.5f;
				}*/
			}
			else
			{
				if(Math.Abs(player.gravDir) < 0.05)
				{
					player.gravDir = 1;
				}
				special = false;
			}
			if (MetroidMod.MorphBallKey.JustPressed && shineDirection == 0)
			{
				if(!trap)
				{
					executeChange = false;
					if(player.height == 14)
					{
						float playerposX = player.width/2 + player.position.X;
						float playerposY = player.height/2 + player.position.Y;
						int pPosX = (int)(playerposX / 16f);
						int pPosY = (int)(playerposY / 16f);
						if (Main.tile[pPosX, pPosY - 1] == null)
						{
							Main.tile[pPosX, pPosY - 1] = new Tile();
						}
						if (Main.tile[pPosX, pPosY - 2] == null)
						{
							Main.tile[pPosX, pPosY - 2] = new Tile();
						}
						bool Inval1 = Main.tile[pPosX, pPosY - 1].active() && Main.tileSolid[(int)Main.tile[pPosX, pPosY - 1].type] && !Main.tileSolidTop[(int)Main.tile[pPosX, pPosY - 1].type];
						bool Inval2 = Main.tile[pPosX, pPosY - 2].active() && Main.tileSolid[(int)Main.tile[pPosX, pPosY - 2].type] && !Main.tileSolidTop[(int)Main.tile[pPosX, pPosY - 2].type];
						if (!(Inval1 || Inval2))
						{
							executeChange = true;
						}
					}
					else
					{
						executeChange = true;
					}
					if(executeChange)
					{
						player.mount.Dismount(player);
						if(ballstate)
						{
							Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/MorphOut"));
						}
						else
						{
							Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/MorphIn"));
						}
						for (int i = 0; i < 25; i++)
						{
							int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 63, 0f, 0f, 100, currentMorphColor, 2f);
							Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
							Main.dust[num].scale *= 1.3f;
							Main.dust[num].noGravity = true;
							Main.dust[num].velocity += player.velocity * 0.8f;
							Main.dust[num].noLight = true;
						}
						for (int j = 0; j < 15; j++)
						{
							int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 63, 0f, 0f, 100, currentMorphColor2, 1f);
							Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
							Main.dust[num].scale *= 1.3f;
							Main.dust[num].noGravity = true;
							Main.dust[num].velocity += player.velocity * 0.8f;
							Main.dust[num].noLight = true;
						}
						ballstate = !ballstate;
						Vector2 PlayerDims = new Vector2(player.width,player.height);
						player.width = ballstate?14:20;
						player.height = ballstate?14:42;
						Vector2 NewDims = new Vector2(player.width,player.height);
						Vector2 TheDiff = PlayerDims - NewDims;
						player.position += new Vector2(0,ballstate?14:-14);
						player.position += TheDiff*0.5f;
					}
					trap = true;
				}
			}
			else
			{
				trap = false;
			}
		}
        const int TileSize = 16;
public void SenseMove(Player P)
		{
				MPlayer mp = P.GetModPlayer<MPlayer>(mod);
				int dist = 80;
				if(senseSound)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SenseMoveSound"));
				}
				//int W = 20;
				//int H = 48;
				//Rectangle MB = new Rectangle((int)P.Center.X-W,(int)P.Center.Y-H,(W*2),(H*2));
				detect = false;
				for(int k = 0; k < Main.npc.Length; k++)
				{
					NPC N = Main.npc[k];
					if(N.damage > 0 && !N.friendly && N.life > 0 && N.active)
					{
						for(int i = 1; i <= dist; i++)
						{
							//Rectangle NB = new Rectangle((int)(N.position.X+(N.velocity.X*i)),(int)(N.position.Y+(N.velocity.Y*i)),N.width,N.height);
							Vector2 npcFuturePos = new Vector2(N.Center.X+(N.velocity.X*i),N.Center.Y+(N.velocity.Y*i));
							float npcDist = Vector2.Distance(P.Center, npcFuturePos);
							if((npcDist <= (P.height+N.width) || npcDist <= (P.height+N.height)) && P.velocity.Y == 0f)
							{
								if(N.velocity.X != 0f || N.velocity.Y != 0f)
								{
									if(N.noTileCollide || Collision.CanHit(P.position, P.width, P.height, N.position, N.width, N.height))
									{
										detect = true;
									}
								}
							}
						}
					}
				}
				for(int k = 0; k < Main.projectile.Length; k++)
				{
					Projectile Pr = Main.projectile[k];
					if(Pr.damage > 0 && !Pr.friendly && Pr.hostile && Pr.timeLeft > 0 && Pr.active)
					{
						for(int i = 1; i <= dist; i++)
						{
							//Rectangle PB = new Rectangle((int)(Pr.position.X+(Pr.velocity.X*i)),(int)(Pr.position.Y+(Pr.velocity.Y*i)),Pr.width,Pr.height);
							Vector2 projFuturePos = new Vector2(Pr.Center.X+(Pr.velocity.X*i),Pr.Center.Y+(Pr.velocity.Y*i));
							float projDist = Vector2.Distance(P.Center, projFuturePos);
							if((projDist <= (P.height+Pr.width) || projDist <= (P.height+Pr.height)) && P.velocity.Y == 0f)
							{
								if(Pr.velocity.X != 0f || Pr.velocity.Y != 0f)
								{
									if(!Pr.tileCollide || Collision.CanHit(P.position, P.width, P.height, Pr.position, Pr.width, Pr.height))
									{
										detect = true;
									}
								}
							}
						}
					}
				}
				if(detect && !mp.ballstate && !P.mount.Active && P.velocity.Y == 0f)
				{
					if(!isSenseMoving)
					{
						if(P.controlLeft && MetroidMod.SenseMoveKey.Current)
						{
							SMoveEffect = 40;
							senseSound = true;
							P.velocity.X = -7f;
							P.velocity.Y -= 4.5f * P.gravDir;
							if(P.direction == 1 && P.maxRunSpeed < -P.velocity.X)
							{
								sMoveDir = -1;
							}
							isSenseMoving = true;
						}
						else if(P.controlRight && MetroidMod.SenseMoveKey.Current)
						{
							SMoveEffect = 40;
							senseSound = true;
							P.velocity.X = 7f;
							P.velocity.Y -= 4.5f * P.gravDir;
							if(P.direction == -1 && P.maxRunSpeed < P.velocity.X)
							{
								sMoveDir = -1;
							}
							isSenseMoving = true;
						}
						else
						{
							isSenseMoving = false;
							senseSound = false;
							sMoveDir = 1;
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
		// check if there is a solid tile to the left of the left centre of the player
		public static bool CheckLeft(Player player)
		{
			float playerCentreY = player.position.Y + player.height * 0.5f;
			float playerLeft = player.position.X;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X <= Main.leftWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			int TX = (int)(playerLeft - 1) / TileSize;
			int TY = (int)playerCentreY / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && (!Main.tileSolidTop[(int)Main.tile[TX, TY].type] || (Main.tile[TX, TY].slope() != 0 && player.gravDir == 1)) && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		public static bool CheckLowerLeft(Player player)
		{
			float playerCentreY = player.position.Y + player.height * (0.5f + (0.25f * player.gravDir));
			float playerLeft = player.position.X;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X <= Main.leftWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			int TX = (int)(playerLeft - 1) / TileSize;
			int TY = (int)playerCentreY / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && (!Main.tileSolidTop[(int)Main.tile[TX, TY].type] || (Main.tile[TX, TY].slope() != 0 && player.gravDir == 1)) && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		// check if there is a solid tile right of the right centre of the player
		public static bool CheckRight(Player player)
		{
			float playerCentreY = player.position.Y + player.height * 0.5f;
			float playerRight = player.position.X + player.width;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X + (float)player.width >= Main.rightWorld - (float)(Lighting.offScreenTiles * 16) - 32f)
			{
				return true;
			}
			
			int TX = (int)(playerRight + 1) / TileSize;
			int TY = (int)playerCentreY / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && (!Main.tileSolidTop[(int)Main.tile[TX, TY].type] || (Main.tile[TX, TY].slope() != 0 && player.gravDir == 1)) && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		public static bool CheckLowerRight(Player player)
		{
			float playerCentreY = player.position.Y + player.height * (0.5f + (0.25f * player.gravDir));
			float playerRight = player.position.X + player.width;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X + (float)player.width >= Main.rightWorld - (float)(Lighting.offScreenTiles * 16) - 32f)
			{
				return true;
			}
			
			int TX = (int)(playerRight + 1) / TileSize;
			int TY = (int)playerCentreY / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && (!Main.tileSolidTop[(int)Main.tile[TX, TY].type] || (Main.tile[TX, TY].slope() != 0 && player.gravDir == 1)) && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		// check if there is solid tile below and left of the player
		public static bool CheckFloorLeft(Player player)
		{
			float playerBottom = (player.gravDir == 1?player.position.Y + player.height:player.position.Y);
			float playerLeft = player.position.X;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X <= Main.leftWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			// going to look at a tile outside the bounds of the map
			if (player.position.Y >= Main.bottomWorld - (float)(Lighting.offScreenTiles * 16) - 32f - (float)player.height)
			{
				return true;
			}
			
			int TX = (int)(playerLeft - 2) / TileSize;
			int TY = (int)(playerBottom + 2) / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && (!Main.tileSolidTop[(int)Main.tile[TX, TY].type] || (Main.tile[TX, TY].slope() != 0 && player.gravDir == 1)) && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		// check if there is solid tile below and right of the player
		public static bool CheckFloorRight(Player player)
		{
			float playerBottom = (player.gravDir == 1?player.position.Y + player.height:player.position.Y);
			float playerRight = player.position.X + player.width;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X + (float)player.width >= Main.rightWorld - (float)(Lighting.offScreenTiles * 16) - 32f)
			{
				return true;
			}
			
			// going to look at a tile outside the bounds of the map
			if (player.position.Y >= Main.bottomWorld - (float)(Lighting.offScreenTiles * 16) - 32f - (float)player.height)
			{
				return true;
			}
			
			int TX = (int)(playerRight + 2) / TileSize;
			int TY = (int)(playerBottom + 2) / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && (!Main.tileSolidTop[(int)Main.tile[TX, TY].type] || (Main.tile[TX, TY].slope() != 0 && player.gravDir == 1)) && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		// check if there is a solid tile above and left of the player
		public static bool CheckUpperLeft(Player player)
		{
			float playerTop = (player.gravDir == 1?player.position.Y:player.position.Y+player.height);
			float playerLeft = player.position.X;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X <= Main.leftWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			// going to look at a tile outside the bounds of the map
			if (player.position.Y <= Main.topWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			int TX = (int)(playerLeft - 1) / TileSize;
			int TY = (int)(playerTop - 1) / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && !Main.tileSolidTop[(int)Main.tile[TX, TY].type] && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		// check if there is solid tile above and right of the player
		public static bool CheckUpperRight(Player player)
		{
			float playerTop = (player.gravDir == 1?player.position.Y:player.position.Y+player.height);
			float playerRight = player.position.X + player.width;
			
			// going to look at a tile outside the bounds of the map
			if (player.position.X + (float)player.width >= Main.rightWorld - (float)(Lighting.offScreenTiles * 16) - 32f)
			{
				return true;
			}
			
			// going to look at a tile outside the bounds of the map
			if (player.position.Y <= Main.topWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			int TX = (int)(playerRight + 1) / TileSize;
			int TY = (int)(playerTop - 1) / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && !Main.tileSolidTop[(int)Main.tile[TX, TY].type] && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}

		// check if there is a solid tile above the top centre of the player
		public static bool CheckCeiling(Player player)
		{
			float playerCentreX = player.position.X + player.width * 0.5f;
			float playerTop = (player.gravDir == 1?player.position.Y:player.position.Y+player.height);
			
			// going to look at a tile outside the bounds of the map
			if (player.position.Y <= Main.topWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
			{
				return true;
			}
			
			int TX = (int)playerCentreX / TileSize;
			int TY = (int)(playerTop - 1) / TileSize;
			
			// tile is solid and active
			if (Main.tileSolid[(int)Main.tile[TX, TY].type] && !Main.tileSolidTop[(int)Main.tile[TX, TY].type] && Main.tile[TX, TY].active())
			{
				return true;
			}
			
			return false;
		}
        
        public void AddSpeedBoost(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			speedBoosting = (Math.Abs(player.velocity.X) >= 6.85f && speedBuildUp >= 120f && mp.SMoveEffect <= 0 && shineDirection == 0);
			if((player.controlRight && player.velocity.X > 0) || (player.controlLeft && player.velocity.X < 0))
			{
				if(player.velocity.Y == 0f && speedBuildUp < 135f)
				{
					speedBuildUp += 1f;
				}
				if(speedBuildUp >= 135f)
				{
					speedBuildUp = 135f;
				}
			}
			else if(!speedBoosting)
			{
				speedBuildUp = 0f;
			}
			player.maxRunSpeed += (speedBuildUp*0.036f);
			if(mp.speedBoosting)
			{
				player.armorEffectDrawShadowSubtle = true;
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
					int SpBoost = Terraria.Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,SpeedBoostID,specialDmg,0,player.whoAmI);
				}
			}
		#region shine-spark
			if(mp.speedBoosting)
			{
				if(player.controlDown && player.velocity.Y == 0 && !shineCharge)
				{
					shineCharge = true;
					player.velocity.X = 0;
				}
			}
			if(shineCharge)
			{
				shineDeCharge++;
				if(player.controlJump && player.releaseJump && !player.controlRight && !player.controlLeft && mp.statOverheat < mp.maxOverheat)
				{
					shineActive = true;
					player.mount.Dismount(player);
					//player.jump = 0;
					player.gravity = 0f;
				}
				else
				{
					Color color = new Color();
					int dust = Dust.NewDust(player.position, player.width, player.height, 64, 0, 0, 100, color, 2.0f);
					Main.dust[dust].noGravity = true;
					shineSound++;
					if(shineSound > 11)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpeedBoosterLoop"));

						shineSound = 0;
					}
				}
			}
			if(shineActive)
			{
				shineSound = 0;
				player.velocity.Y = -((player.gravity + 1E-06f) * player.gravDir);
				player.gravity *= 0f;
				player.maxFallSpeed = 0f;
				player.velocity.X = 0;
				player.moveSpeed = 0f;
				player.maxRunSpeed = 0f;
				player.noItems = true;
				player.controlJump = false;
				player.releaseJump = true;
				mp.rotation = 0;
				player.armorEffectDrawShadowSubtle = true;
				shineDeActive++;
				if(shineDeActive == 2)
				{
						player.position.Y -= 12f*player.gravDir;
				}
				if(shineDeActive > 29 && mp.statOverheat < mp.maxOverheat)
				{
					//player.statMana--;
					//player.manaRegenDelay = (int)player.maxRegenDelay;
					shineCharge = false;
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
			}

			if(shineDeCharge > 299 && !shineActive)
			{
				shineCharge = false;
				shineDeCharge = 0;
			}


			if(shineDirection == 1) //right
			{
				player.velocity.X = 20;
				player.velocity.Y = 0.00001f*player.gravDir;
				player.gravity *= 0f;
				player.maxFallSpeed = 0f;
				player.direction = 1;
				shineDeActive = 0;
				player.controlLeft = false;
			}
			if(shineDirection == 2) //right and up
			{
				player.velocity.X = 20;
				player.velocity.Y = -20f*player.gravDir;
				player.gravity *= 0f;
				player.maxFallSpeed = 0f;
				player.direction = 1;
				shineDeActive = 0;
				player.controlLeft = false;
			}
			if(shineDirection == 3) //left
			{
				player.velocity.X = -20;
				player.velocity.Y = 0.00001f*player.gravDir;
				player.gravity *= 0f;
				player.maxFallSpeed = 0f;
				player.direction = -1;
				shineDeActive = 0;
				player.controlRight = false;
			}
			if(shineDirection == 4) //left and up
			{
				player.velocity.X = -20;
				player.velocity.Y = -20*player.gravDir;
				player.gravity *= 0f;
				player.maxFallSpeed = 0f;
				player.direction = -1;
				shineDeActive = 0;
				player.controlRight = false;
			}
			if(shineDirection == 5) //up
			{
				player.velocity.X *= 0f;
				player.velocity.Y = -20*player.gravDir;
				player.gravity *= 0f;
				player.maxFallSpeed = 0f;
				shineDeActive = 0;
				if (player.miscCounter % 4 == 0 && !ballstate)
				{
					player.direction *= -1;
				}
				player.controlLeft = false;
				player.controlRight = false;
			}

			if(shineDirection != 0)
			{
				//manaDelay++;
				//if(manaDelay > 2)
				//{
					//player.statMana--;
					mp.statOverheat += 0.5f;
					//manaDelay = 0;
				//}
				//player.manaRegenDelay = (int)player.maxRegenDelay;
				shineDeCharge = 300;
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
			if(shineDirection == 1 && (CheckRight(player) || CheckFloorRight(player) || CheckLowerRight(player) || 
			CheckUpperRight(player) || mp.statOverheat >= mp.maxOverheat || 
			(player.position.X + (float)player.width) > (Main.rightWorld - 640f - 48f)))
			{
				shineDirection = 0;
				shineDeActive = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop up and right movement
			if(shineDirection == 2 && (CheckCeiling(player) || CheckUpperRight(player) || CheckRight(player) || mp.statOverheat >= mp.maxOverheat || 
			(player.position.X + (float)player.width) > (Main.rightWorld - 640f - 48f) || player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDeActive = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop left movement
			if(shineDirection == 3 && (CheckLeft(player) || CheckFloorLeft(player) || CheckLowerLeft(player) || 
			CheckUpperLeft(player) || mp.statOverheat >= mp.maxOverheat || 
			player.position.X < (Main.leftWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDeActive = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop left and up movement
			if(shineDirection == 4 && (CheckCeiling(player) || CheckUpperLeft(player) || CheckLeft(player) || mp.statOverheat >= mp.maxOverheat || 
			player.position.X < (Main.leftWorld + 640f + 32f) || player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDeActive = 0;
				shineActive = false;
				Main.projectile[proj].Kill();
				speedBuildUp = 135f;
				if(mp.statOverheat >= mp.maxOverheat)
				{
					mp.statOverheat += 10;
				}
			}
			//stop up movement
			if(shineDirection == 5 && (CheckCeiling(player) || CheckUpperLeft(player) || CheckUpperRight(player) || mp.statOverheat >= mp.maxOverheat || 
			player.position.Y < (Main.topWorld + 640f + 32f)))
			{
				shineDirection = 0;
				shineDeActive = 0;
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

		// size of a tile
		int TileSizeY = 16;


		// check if there is a solid tile below the bottom centre of the player
		public bool SpiderCheckFloor(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X+1,(int)player.position.Y+(int)player.height-1,player.width-2,2);
						Rectangle ball = new Rectangle((int)player.position.X,(int)player.position.Y+(int)player.height-1,player.width,2);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is a solid tile above the top centre of the player
		public bool SpiderCheckCeiling(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X+1,(int)player.position.Y-1,player.width-2,2);
						Rectangle ball = new Rectangle((int)player.position.X,(int)player.position.Y-1,player.width,2);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is a solid tile to the left of the left centre of the player
		public bool SpiderCheckLeft(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X-1,(int)player.position.Y+2,2,player.height-2);
						Rectangle ball = new Rectangle((int)player.position.X-1,(int)player.position.Y,2,player.height);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is a solid tile right of the right centre of the player
		public bool SpiderCheckRight(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X+(int)player.width-1,(int)player.position.Y+1,2,player.height-2);
						Rectangle ball = new Rectangle((int)player.position.X+(int)player.width-1,(int)player.position.Y,2,player.height);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is a solid tile above and left of the player
		public bool SpiderCheckCeilingLeft(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X-1,(int)player.position.Y-1,2,2);
						Rectangle ball = new Rectangle((int)player.position.X-1,(int)player.position.Y-1,1,1);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is solid tile above and right of the player
		public bool SpiderCheckCeilingRight(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X+(int)player.width-1,(int)player.position.Y-1,2,2);
						Rectangle ball = new Rectangle((int)player.position.X+(int)player.width,(int)player.position.Y-1,1,1);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is solid tile below and left of the player
		public bool SpiderCheckFloorLeft(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X-1,(int)player.position.Y+(int)player.height-1,2,2);
						Rectangle ball = new Rectangle((int)player.position.X-1,(int)player.position.Y+(int)player.height,1,1);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}

		// check if there is solid tile below and right of the player
		public bool SpiderCheckFloorRight(Player player)
		{
			int num = (int)((player.position.X) / 16f) - 5;
			int num2 = (int)((player.position.X + (float)player.width) / 16f) + 5;
			int num3 = (int)((player.position.Y) / 16f) - 5;
			int num4 = (int)((player.position.Y + (float)player.height) / 16f) + 5;
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
					if(Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type])
					{
						Rectangle tile = new Rectangle((i*16),(j*16),16,16);
						//Rectangle ball = new Rectangle((int)player.position.X+(int)player.width-1,(int)player.position.Y+(int)player.height-1,2,2);
						Rectangle ball = new Rectangle((int)player.position.X+(int)player.width,(int)player.position.Y+(int)player.height,1,1);
						if(Main.tile[i, j].halfBrick())
						{
							tile = new Rectangle((i*16),(j*16)+8,16,8);
						}
						if(ball.Intersects(tile))
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}
		/*public PlayerLayer testLayer = new PlayerLayer.Action("MetroidMod:test", (PlayerLayer layer, Player player, SpriteBatch sb) =>
		{
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			//floor
			mp.testRect[0] = new Rectangle((int)player.position.X,(int)player.position.Y+(int)player.height-1,player.width,2);
			//ceiling
			mp.testRect[1] = new Rectangle((int)player.position.X,(int)player.position.Y-1,player.width,2);
			//left
			mp.testRect[2] = new Rectangle((int)player.position.X-1,(int)player.position.Y,2,player.height);
			//right
			mp.testRect[3] = new Rectangle((int)player.position.X+(int)player.width-1,(int)player.position.Y,2,player.height);
			//up left
			mp.testRect2[0] = new Rectangle((int)player.position.X-1,(int)player.position.Y-1,1,1);
			//up right
			mp.testRect2[1] = new Rectangle((int)player.position.X+(int)player.width,(int)player.position.Y-1,1,1);
			//down left
			mp.testRect2[2] = new Rectangle((int)player.position.X-1,(int)player.position.Y+(int)player.height,1,1);
			//down right
			mp.testRect2[3] = new Rectangle((int)player.position.X+(int)player.width,(int)player.position.Y+(int)player.height,1,1);
			Texture2D tex = MBase.Instance.textures["Gore/TestRectangle"];
			for(int i = 0; i < 4; i++)
			{
				sb.Draw(tex, new Rectangle((int)(mp.testRect[i].X - Main.screenPosition.X), (int)(mp.testRect[i].Y - Main.screenPosition.Y), mp.testRect[i].Width, mp.testRect[i].Height), Color.Orange);
				sb.Draw(tex, new Rectangle((int)(mp.testRect2[i].X - Main.screenPosition.X), (int)(mp.testRect2[i].Y - Main.screenPosition.Y), mp.testRect2[i].Width, mp.testRect2[i].Height), Color.Blue);
			}
			sb.Draw(tex, new Rectangle((int)(player.Hitbox.X - Main.screenPosition.X), (int)(player.Hitbox.Y - Main.screenPosition.Y), player.Hitbox.Width, player.Hitbox.Height), Color.Gray);
		});*/

		// get the edge the player is currently on
		public Edge GetEdge(Player player)
		{
			if (SpiderCheckFloor(player))
			{
				return Edge.Floor;
			}
			else if (SpiderCheckCeiling(player))
			{
				return Edge.Ceiling;
			}
			else if (SpiderCheckLeft(player))
			{
				return Edge.Left;
			}
			else if (SpiderCheckRight(player))
			{
				return Edge.Right;
			}
			
			return Edge.None;
		}

		const float switchSpeed = 2f;
		const float moveSpeed = 2f;
		// actions if the player is on the floor
		public void DoFloor(Player player)
		{

			// push the player into their edge
			player.velocity.Y = (2.0f+player.gravity);
			player.maxFallSpeed = 0f;
			
			// move left
			if (player.controlLeft)
			{
				player.velocity.X = -moveSpeed;
			}
			// move right
			else if (player.controlRight)
			{
				player.velocity.X = moveSpeed;
			}
			// not moving
			else
			{
				player.velocity.X = 0f;
			}
			
			// moving left
			if (player.velocity.X < 0.0f)
			{
				if (CheckLeft(player))
				{
					CurEdge = Edge.Left;
					return;
				}
				
				// no floor
				if (!SpiderCheckFloor(player))
				{
					// no floor to the right
					if (!SpiderCheckFloorRight(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Right;
						player.position.X = (int)player.position.X / TileSize * TileSize;
						player.position.Y += TileSize - player.height;
						player.velocity.X = switchSpeed;
						return;
					}
				}
			}
			// moving right
			else if(player.velocity.X > 0.0f)
			{
				if (SpiderCheckRight(player))
				{
					CurEdge = Edge.Right;
					return;
				}
				
				// no floor
				if (!SpiderCheckFloor(player))
				{
					// no floor to the left
					if (!SpiderCheckFloorLeft(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Left;
						player.position.X = (int)player.position.X / TileSize * TileSize;
						player.position.Y += TileSize - player.height;
						player.velocity.X = -switchSpeed;
						return;
					}
				}
			}
		}

		// actions if the player is on the ceiling
		public void DoCeiling(Player player)
		{

			// push the player into their edge
			player.velocity.Y = -(2.0f+player.gravity);
			player.maxFallSpeed = 0f;
			
			// move right - upside down, reverse direction, classic spiderball style
			if (player.controlLeft)
			{
				player.velocity.X = moveSpeed;
			}
			// move left
			else if (player.controlRight)
			{
				player.velocity.X = -moveSpeed;
			}
			// not moving
			else
			{
				player.velocity.X = 0f;
			}
			
			// moving left
			if (player.velocity.X < 0.0f)
			{
				if (SpiderCheckLeft(player))
				{
					CurEdge = Edge.Left;
					return;
				}
				
				// no ceiling
				if (!SpiderCheckCeiling(player))
				{
					// no ceiling to the right
					if (!SpiderCheckCeilingRight(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Right;
						player.position.X = (int)player.position.X / TileSize * TileSize;
						player.position.Y += player.height - TileSize;
						player.velocity.X = switchSpeed;
						return;
					}
				}
			}
			else if(player.velocity.X > 0.0f)
			{
				if (SpiderCheckRight(player))
				{
					CurEdge = Edge.Right;
					return;
				}
				
				// no ceiling
				if (!SpiderCheckCeiling(player))
				{
					// no ceiling to the left
					if (!SpiderCheckCeilingLeft(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Left;
						player.position.X = (int)player.position.X / TileSize * TileSize;
						player.position.Y += player.height - TileSize;
						player.velocity.X = -switchSpeed;
						return;
					}
				}
			}
		}

		// actions if the player is on the left wall
		public void DoLeft(Player player)
		{

			// push the player into their edge
			player.velocity.X = -2.0f;
			
			// move up
			if (player.controlLeft)
			{
				player.velocity.Y = -(moveSpeed+player.gravity);
			}
			// move down
			else if (player.controlRight)
			{
				player.velocity.Y = moveSpeed;
			}
			else
			{
				// not moving
				player.velocity.Y = -player.gravity*player.gravDir;
				player.maxFallSpeed = 0f;
			}
			
			// moving up
			if (player.velocity.Y < 0.0f)
			{
				if (SpiderCheckCeiling(player))
				{
					CurEdge = Edge.Ceiling;
					return;
				}
				
				// no wall to the left
				if (!SpiderCheckLeft(player))
				{
					// now wall to the lower left
					if (!SpiderCheckFloorLeft(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Floor;
						player.position.Y = (int)player.position.Y / TileSize * TileSize;
						player.position.X += player.width - TileSize;
						player.velocity.Y = switchSpeed;
						return;
					}
				}
			}
			// moving down
			else if(player.velocity.Y > 0.0f)
			{
				if (SpiderCheckFloor(player))
				{
					CurEdge = Edge.Floor;
					return;
				}
			
				if (!SpiderCheckLeft(player))
				{
					// no wall to the upper left
					if (!SpiderCheckCeilingLeft(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Ceiling;
						player.position.Y = (int)player.position.Y / TileSize * TileSize;
						player.position.X += player.width - TileSize;
						player.velocity.Y = -switchSpeed;
						return;
					}
				}
			}
		}

		// actions if the player is on the right wall
		public void DoRight(Player player)
		{

			// push the player into their edge
			player.velocity.X = 2.0f;
			
			// move down
			if (player.controlLeft)
			{
				player.velocity.Y = moveSpeed;
			}
			// move up
			else if (player.controlRight)
			{
				player.velocity.Y = -(moveSpeed+player.gravity);
			}
			else
			{
				// not moving
				player.velocity.Y = -player.gravity*player.gravDir;
				player.maxFallSpeed = 0f;
			}
			
			// moving up
			if (player.velocity.Y < 0.0f)
			{
				if (SpiderCheckCeiling(player))
				{
					CurEdge = Edge.Ceiling;
					return;
				}
				
				// no wall to the right
				if (!SpiderCheckRight(player))
				{
					// no wall to the lower right
					if (!SpiderCheckFloorRight(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Floor;
						player.position.Y = (int)player.position.Y / TileSize * TileSize;
						player.position.X += TileSize - player.width;
						player.velocity.Y = switchSpeed;
						return;
					}
				}
			}
			// moving down
			else if(player.velocity.Y > 0.0f)
			{
				if (SpiderCheckFloor(player))
				{
					CurEdge = Edge.Floor;
					return;
				}
				
				// no wall to the right
				if (!SpiderCheckRight(player))
				{
					// no wall to the upper right
					if (!SpiderCheckCeilingRight(player))
					{
						// set player to the edge of the cliff edge tile
						CurEdge = Edge.Ceiling;
						player.position.Y = (int)player.position.Y / TileSize * TileSize;
						player.position.X += TileSize - player.width;
						player.velocity.Y = -switchSpeed;
						return;
					}
				}
			}
		}
		public void SpiderBall(Player player)
		{
	
			if(ballstate)
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
					if (!SpiderCheckFloor(player) && !SpiderCheckCeiling(player) && !SpiderCheckLeft(player) && !SpiderCheckRight(player) &&
						!SpiderCheckFloorLeft(player) && !SpiderCheckFloorRight(player) && !SpiderCheckCeilingLeft(player) && !SpiderCheckCeilingRight(player))
					{
						CurEdge = Edge.None;
					}
					// if the edge has changed, display the current edge

					if(CurEdge != Edge.None)
					{
						// render player's default movements effortless
						player.moveSpeed = 0f;
						player.maxRunSpeed = 0f;
						//player.gravity = 0f;
						// disable terraria's step-up feature
						if (player.velocity.Y == player.gravity && (!player.mount.Active || player.mount.Type != 6))
						{
							Collision.StepUp(ref player.position, ref player.velocity, player.width, player.height, ref player.stepSpeed, ref player.gfxOffY, (int)player.gravDir, player.waterWalk || player.waterWalk2);
						}
						if (player.gravDir == -1f)
						{
							if ((player.carpetFrame != -1 || player.velocity.Y <= player.gravity) && !player.controlUp)
							{
								Collision.StepDown(ref player.position, ref player.velocity, player.width, player.height, ref player.stepSpeed, ref player.gfxOffY, (int)player.gravDir, player.controlUp);
							}
						}
						else if ((player.carpetFrame != -1 || player.velocity.Y >= player.gravity) && !player.controlDown && (!player.mount.Active || player.mount.Type != 6))
						{
							Collision.StepDown(ref player.position, ref player.velocity, player.width, player.height, ref player.stepSpeed, ref player.gfxOffY, (int)player.gravDir, player.controlUp);
						}
					}
				}
				else
				{
					Ibounce = true;
				}
			}
		}
		//int CFMoment = 0;
		public void PowerBomb(Player player)
		{
	
			if(ballstate)
			{
				if(/*powerbomb == 0 && */statPBCh <= 0 && MetroidMod.PowerBombKey.JustPressed && shineDirection == 0)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
                    Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombCharge1"));
					statPBCh = 200;
					//powerbomb = 180;
					int PBombID = mod.ProjectileType("PowerBomb");
					int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,PBombID,25,0,player.whoAmI);
				}
			}
			/*if(powerbomb > 0)
			{
				powerbomb--;
			}*/
		}
		public void BoostBall(Player player)
		{
	
			if(ballstate)
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
						if(spiderball)
						{
							if(CurEdge == Edge.Floor)
							{
								CurEdge = Edge.None;
								player.velocity.Y -= 7f;
							}
							if(CurEdge == Edge.Ceiling)
							{
								CurEdge = Edge.None;
								player.velocity.Y += 7f;
							}
							if(CurEdge == Edge.Left)
							{
								CurEdge = Edge.None;
								player.velocity.X += 7f;
							}
							if(CurEdge == Edge.Right)
							{
								CurEdge = Edge.None;
								player.velocity.X -= 7f;
							}
						}
						if(!spiderball || CurEdge == Edge.None)
						{
							if(player.velocity.X == 0 && player.velocity.Y == 0)
							{
								player.velocity.X += 4f * player.direction;
							}
							if(player.velocity.X > 0)
							{
								player.velocity.X += 4f;
							}
							if(player.velocity.X < 0)
							{
								player.velocity.X -= 4f;
							}
							if(player.velocity.Y > 0)
							{
								player.velocity.Y += 4f;
							}
							if(player.velocity.Y < 0)
							{
								player.velocity.Y -= 4f;
							}
						}
						boostEffect += boostCharge;
						boostCharge -= 30;
					}
					else
					{
						boostCharge = 0;
					}
					soundDelay = 0;
				}
				if(boostEffect > 0)
				{
					 player.armorEffectDrawShadowSubtle = true;
					boostEffect--;
				}
			}
			else
			{
				boostCharge = 0;
				boostEffect = 0;
			}
		}
		public void Drill(Player p, int drill)
		{
			
			if(ballstate)
			{
				if (drill > 0 && p.position.X / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetX && (p.position.X + (float)p.width) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetX && p.position.Y / 16f - Player.tileRangeX - 3f <= (float)Player.tileTargetY && (p.position.Y + (float)p.height) / 16f + Player.tileRangeX + 2f >= (float)Player.tileTargetY)
				{
					int num165 = 0;
					if(Main.mouseLeft)
					{
						if (p.runSoundDelay <= 0)
						{
							Main.PlaySound(2, (int)p.position.X, (int)p.position.Y, 22);
							p.runSoundDelay = 30;
						}
						if (Main.rand.Next(6) == 0)
						{
							int num123 = Dust.NewDust(p.position + p.velocity * (float)Main.rand.Next(6, 10) * 0.1f, p.width, p.height, 31, 0f, 0f, 80, default(Color), 1.4f);
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
							int tileId = p.hitTile.HitObject(Player.tileTargetX, Player.tileTargetY, 1);
							if (Main.tileNoFail[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
							{
								num165 = 100;
							}
							if (drill > 0)
							{
							/*	if (Main.tileDungeon[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 25 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 58 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 117 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 203)
								{
									num165 += drill / 2;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 48 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 232)
								{
									num165 += drill / 4;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 226)
								{
									num165 += drill / 4;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 107 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 221)
								{
									num165 += drill / 2;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 108 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 222)
								{
									num165 += drill / 3;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 111 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 223)
								{
									num165 += drill / 4;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 211)
								{
									num165 += drill / 5;
								}
								else
								{
									num165 += drill;
								}
								if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 211 && drill < 200)
								{
									num165 = 0;
								}
								if ((Main.tile[Player.tileTargetX, Player.tileTargetY].type == 25 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 203) && drill < 65)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 117 && drill < 65)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 37 && drill < 50)
								{
									num165 = 0;
								}
								else if ((Main.tile[Player.tileTargetX, Player.tileTargetY].type == 22 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 204) && (double)Player.tileTargetY > Main.worldSurface && drill < 55)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 56 && drill < 65)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 58 && drill < 65)
								{
									num165 = 0;
								}
								else if ((Main.tile[Player.tileTargetX, Player.tileTargetY].type == 226 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 237) && drill < 210)
								{
									num165 = 0;
								}
								else if (Main.tileDungeon[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && drill < 65)
								{
									if ((double)Player.tileTargetX < (double)Main.maxTilesX * 0.35 || (double)Player.tileTargetX > (double)Main.maxTilesX * 0.65)
									{
										num165 = 0;
									}
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 107 && drill < 100)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 108 && drill < 110)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 111 && drill < 150)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 221 && drill < 100)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 222 && drill < 110)
								{
									num165 = 0;
								}
								else if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 223 && drill < 150)
								{
									num165 = 0;
								}
								if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 147 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 40 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 53 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 57 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 59 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 123 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 224)
								{
									num165 += drill;
								}
								if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 165 || Main.tileRope[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 199 || Main.tileMoss[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
								{
									num165 = 100;
								}
								if (drill < Main.tileMinPick[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
								{
									num165 = 0;
								}
								if (p.hitTile.AddDamage(tileId, num165, false) >= 100 && (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 2 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 23 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 60 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 70 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 109 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 71 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 199 || Main.tileMoss[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
								{
									num165 = 0;
								}
								if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 128 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 269)
								{
									if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 18 || Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 54)
									{
										Player.tileTargetX--;
										p.hitTile.UpdatePosition(tileId, Player.tileTargetX, Player.tileTargetY);
									}
									if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX >= 100)
									{
										num165 = 0;
										Main.blockMouse = true;
									}
								}
								if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 334)
								{
									if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameY == 0)
									{
										Player.tileTargetY++;
										p.hitTile.UpdatePosition(tileId, Player.tileTargetX, Player.tileTargetY);
									}
									if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameY == 36)
									{
										Player.tileTargetY--;
										p.hitTile.UpdatePosition(tileId, Player.tileTargetX, Player.tileTargetY);
									}
									int num166 = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].frameX;
									bool flag11 = num166 >= 5000;
									bool flag12 = false;
									if (!flag11)
									{
										int num167 = num166 / 18;
										num167 %= 3;
										Player.tileTargetX -= num167;
										if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX >= 5000)
										{
											flag11 = true;
										}
									}
									if (flag11)
									{
										num166 = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].frameX;
										int num168 = 0;
										while (num166 >= 5000)
										{
											num166 -= 5000;
											num168++;
										}
										if (num168 != 0)
										{
											flag12 = true;
										}
									}
									if (flag12)
									{
										num165 = 0;
										Main.blockMouse = true;
									}
								}
								if (p.hitTile.AddDamage(tileId, num165, true) >= 100)
								{
									p.hitTile.Clear(tileId);
									if (Main.netMode == 1 && Main.tile[Player.tileTargetX, Player.tileTargetY].type == 21)
									{
										WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true, false, false);
										NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0f);
										NetMessage.SendData(34, -1, -1, "", 1, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0f);
									}
									else
									{
										int num169 = Player.tileTargetY;
										WorldGen.KillTile(Player.tileTargetX, num169, false, false, false);
										if (Main.netMode == 1)
										{
											NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)num169, 0f, 0f);
										}
									}
								}
								else
								{
									WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true, false, false);
									if (Main.netMode == 1)
									{
										NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0f);
									}
								}
								if (num165 != 0)
								{
									p.hitTile.Prune();
								}*/
								//p.itemTime = (int)((float)this.inventory[this.selectedItem].useTime * this.pickSpeed);
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
		}


    }
}
