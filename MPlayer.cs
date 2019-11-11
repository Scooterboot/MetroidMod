using System;
using System.Linq;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Capture;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MetroidMod;
using MetroidMod.NPCs;

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

    /* TODO:
     * Rework the Standing on NPC mechanic to accept a list of NPC types in PreUpdateMovement.
     */
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
		//public bool mouseRight = false;
		public int soundDelay = 0;
		public SoundEffectInstance soundInstance;
		public bool trap = false;
		public bool executeChange = false;
		int unMorphDir = 0;
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
		public int screwSpeedDelay = 0;
		public int screwAttack = 0;
		public bool isPowerSuit = false;
		public bool thrusters = false;
		public bool spaceJump = false;
		public bool spaceJumpBoots = false;
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
		public bool disableSomersault = false;
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
        public int reserveTanks = 0;
        public int reserveHearts = 0;
        public int reserveHeartsValue = 20;
		public int phazonRegen = 0;
		public bool powerGrip = false;
		public bool isGripping = false;
		public int reGripTimer = 0;
		public int gripDir = 1;
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
			reserveTanks = 0;
			reserveHeartsValue = 20;
			thrusters = false;
			spaceJump = false;
			spaceJumpBoots = false;
			speedBooster = false;
			screwAttack = 0;
			powerGrip = false;

			if(!player.mount.Active || player.mount.Type != mod.MountType("MorphBallMount"))
				morphBall = false;

			visorGlow = false;
			visorGlowColor = new Color(255, 255, 255);
			chargeColor = Color.White;
			maxOverheat = 100f;
			overheatCost = 1f;
			breathMult = 1f;
			disableSomersault = false;
		}
		float overheatCooldown = 0f;
		int itemRotTweak = 0;
		public override void PreUpdate()
		{
			ballstate = (player.mount.Active && player.mount.Type == mod.MountType("MorphBallMount"));
			if(ballstate)
			{
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
				unMorphDir = 0;
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
			somersault = (!P.dead && !disableSomersault && (SMoveEffect > 0 || canSomersault) && !P.mount.Active && P.velocity.Y != 0 /*&& P.velocity.X != 0*/ && !P.sliding && !P.pulley && !isGripping && (P.itemAnimation == 0 || statCharge >= 30) && P.grappling[0] <= -1 && grapplingBeam <= -1 && shineDirection == 0 && !shineActive && !ballstate && (((P.wingsLogic != 0 || P.rocketBoots != 0 || P.carpet) && (!P.controlJump || (!P.canRocket && !P.rocketRelease && P.wingsLogic == 0) || (P.wingTime <= 0 && P.rocketTime <= 0 && P.carpetTime <= 0))) || (P.wingsLogic == 0 && P.rocketBoots == 0 && !P.carpet)) && !P.sandStorm);
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

			if(visorGlow && !ballstate)
			{
				Lighting.AddLight((int)((float)player.Center.X/16f), (int)((float)(player.position.Y+8f)/16f), ((float)visorGlowColor.R/255)*0.375f,((float)visorGlowColor.G/255)*0.375f,((float)visorGlowColor.B/255)*0.375f);
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
					if(player.gravDir == -1)
					{
						player.fullRotationOrigin.Y = (float)player.height*0.45f;
					}
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
					player.AddBuff(mod.BuffType("PhazonDebuff"), 2, true);
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
		    int x1 = (int)(player.position.X + player.velocity.X - 1) / 16;
		    int x2 = (int)(player.position.X + player.velocity.X + player.width + 1) / 16;
		    int j = (int)(player.position.Y + player.height + 1) / 16;
		    if (x1 < 0)
		    {
			x1 = 0;
		    }
		    if (x2 > Main.maxTilesX)
		    {
			x2 = Main.maxTilesX;
		    }
		    if (j < 0)
		    {
			j = 0;
		    }
		    if (j > Main.maxTilesY)
		    {
			j = Main.maxTilesY;
		    }
		    for (int i = x1; i <= x2; i++)
		    {
			Vector2 pos = new Vector2(i * 16, j * 16);
			if (MWorld.mBlockType[i, j] == 1 && Main.tile[i, j].active() && !Main.tile[i, j].inActive())
			{
			    Wiring.DeActive(i, j);
			    if (Main.tile[i, j].inActive())
			    {
				Main.PlaySound(2, pos, 51);
				for (int d = 0; d < 4; d++)
				{
				    Dust.NewDust(pos, 16, 16, 1);
				}
			    }
			}
		    }
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
                num /= 16f;
                num2 /= 16f;
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

					/*Vector2 v = player.Center - projectile.Center;
					float dist = Vector2.Distance(player.Center, projectile.Center);
					if(soundDelay2 > 41)
					{
						Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/GrappleLoop"));
						soundDelay2 = 0;
					}
					soundDelay2++;
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
					}*/
					
					Vector2 vel = Vector2.Zero;
					
					float maxMaxDist = 400;
					Vector2 v = player.Center - projectile.Center;
					float dist = Vector2.Distance(player.Center, projectile.Center);
					bool up = (player.controlUp);
					bool down = (player.controlDown && maxDist < maxMaxDist);
					float reelSpeed = 11f;
					if (player.honeyWet && !player.ignoreWater)
					{
						reelSpeed *= 0.25f;
					}
					else if (player.wet && !player.merman && !player.ignoreWater)
					{
						reelSpeed *= 0.5f;
					}
					if (dist > maxDist || up)
					{
						player.maxRunSpeed = 15f;
						player.runAcceleration *= 3f;
						player.jump = 0;
						if(player.velocity.Y == 0f)
						{
							player.velocity.Y = 1E-05f;
						}
						float reel = 0f;
						if(up)
						{
							reel = Math.Max(-reelSpeed,-dist);
							maxDist = Math.Min(dist,maxMaxDist);
						}
						if(down)
						{
							reel = Math.Min(reelSpeed,maxMaxDist-dist);
							maxDist = Math.Min(dist,maxMaxDist);
						}
						float ndist = Vector2.Distance(player.Center + player.velocity, projectile.Center);
						float ddist = ndist - dist;
						v /= dist;
						player.velocity -= v * ddist;
						v *= (maxDist + reel);
						vel = (projectile.Center + v) - player.Center;
						vel = Collision.TileCollision(player.position, vel, player.width, player.height, player.controlDown, false);
						player.position += vel;
					}
					else if(down)
					{
						maxDist = Math.Min(maxDist+(reelSpeed/2),maxMaxDist);
					}

					if (player.controlJump)
					{
						if (player.releaseJump)
						{
							if (maxDist <= 20 && !player.controlDown)
							{
								player.velocity.Y = -Player.jumpSpeed;
								player.jump = Player.jumpHeight / 2;
							}
							else
							{
								player.velocity.Y = player.velocity.Y + 0.01f;
							}
							player.velocity += vel;
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
							player.releaseJump = false;

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
			if((player.miscEquips[3].type == mod.ItemType("MorphBall") || player.mount.Type == mod.MountType("MorphBallMount")) && player.controlMount && !shineActive)
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
		}
		public override void PostUpdateMiscEffects()
		{
			GripMovement();
			
			GrappleBeamMovement();

			if(speedBooster)
			{
				AddSpeedBoost(player);
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
			
			if(spaceJumpBoots || spaceJump || screwAttack > 0)
			{
				AddSpaceJumpBoots(player);
				if(spaceJump)
				{
					AddSpaceJump(player);
				}
				if(screwAttack > 0)
				{
					AddScrewAttack(player,screwAttack);
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
				player.ropeCount = 10;
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
				//mouseRight = false;
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
			if(!spaceJumpBoots && !spaceJump)
			{
				canSomersault = false;
			}
			if(screwAttack <= 0)
			{
				screwAttackSpeedEffect = 0;
				screwSpeedDelay = 0;
			}
			grapplingBeam = -1;
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
		public int psuedoScrewFlash = 0;
		public int shineChargeFlash = 0;
		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			Player P = player;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			
			if(drawInfo.shadow == 0f)
			{
				for (int i = oldPos.Length - 1; i > 0; i--)
				{
					Vector2 vect = oldPos[1] - oldPos[0];
					oldPos[i] = oldPos[i - 1] - (vect * 0.5f);
				}
				oldPos[0] = new Vector2((int)drawInfo.position.X, (int)drawInfo.position.Y);
				
				int width = 8;
				if(Vector2.Distance(oldPos[0],oldPos[1]) > width)
				{
					for(int i = 1; i < oldPos.Length; i++)
					{
						Vector2 pos = oldPos[i-1] - oldPos[i];
						float len = pos.Length();
						
						len = (len - (float)width) / len;
						pos.X *= len;
						pos.Y *= len;
						oldPos[i] += pos;
					}
				}
			}
			
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
			if (isGripping)
			{
				if (player.position.X % 32 > 16 && player.velocity.X != 0)
				{
				    player.bodyFrame.Y = player.bodyFrame.Height * 1;
				}
				else
				{
				    player.bodyFrame.Y = player.bodyFrame.Height * 2;
				}
			}
			if (player.velocity.Y * player.gravDir < 0 && reGripTimer > 0)
			{
				if (reGripTimer > 5)
				{
				    player.bodyFrame.Y = player.bodyFrame.Height * 3;
				}
				else
				{
				    player.bodyFrame.Y = player.bodyFrame.Height * 4;
				}
			}
		}
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			MPlayer mPlayer = player.GetModPlayer<MPlayer>();
			Player P = player;

    		for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k] == PlayerLayer.FrontAcc)
				{
					layers.Insert(k + 1, screwAttackLayer);
                    screwAttackLayer.visible = true;
                }
				if (layers[k] == PlayerLayer.Legs)
				{
					layers.Insert(k + 1, thrusterLayer);
					layers.Insert(k + 2, jetLayer);
                    thrusterLayer.visible = false;//true;
                    jetLayer.visible = true;
                }
				if (layers[k] == PlayerLayer.Head)
				{
					layers.Insert(k + 1, visorLayer);
                    visorLayer.visible = true;

                }
				if(layers[k] == PlayerLayer.Arms)
				{
					layers.Insert(k + 1, gunLayer);
                    gunLayer.visible = true;
                }
			}
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
                for(int i = 0; i < layers.Count; ++i)
                    layers[i].visible = false;
                layers.Add(ballLayer);
                ballLayer.visible = true;
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
				jet = false;
		}
        
        public override void PreUpdateMovement()
        {
            // 'Standing on NPC' mechanic. 
            // Might need some more work, but that's for something in the future.
            for (int i = 0; i < 200; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc.active && (((MetroidMod)MetroidMod.Instance).FrozenStandOnNPCs.Contains(npc.type) || npc.type == mod.NPCType("Tripper")))
                {
                    MNPC mnpc = npc.GetGlobalNPC<MNPC>();
                    if (!mnpc.froze && npc.type != mod.NPCType("Tripper")) continue;

                    if (player.position.X + player.width >= npc.position.X && player.position.X <= npc.position.X + npc.width &&
                        player.position.Y + player.height <= npc.position.Y && player.position.Y + player.velocity.Y + player.height >= npc.position.Y)
                    {
                        player.velocity.Y = 0;
                        player.position = player.oldPosition;

                        if (npc.type == mod.NPCType("Tripper"))
                        {
                            player.position.X = player.oldPosition.X + npc.velocity.X;
                        }
                    }
                }
            }
        }

        public static readonly PlayerLayer screwAttackLayer = new PlayerLayer("MetroidMod", "screwAttackLayer", delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (mPlayer.somersault && mPlayer.screwAttack > 0 && drawInfo.shadow == 0f && !mPlayer.ballstate)
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
						if (P.gravDir == -1f)
						{
							effects |= SpriteEffects.FlipVertically;
						}
						Color alpha = Lighting.GetColor((int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
						int num121 = tex.Height / Main.projFrames[projectile.type];
						int y9 = num121 * projectile.frame;
						spriteBatch.Draw(tex, drawInfo.position + P.fullRotationOrigin - Main.screenPosition, new Rectangle?(new Rectangle(0, y9, tex.Width, num121 - 1)), alpha, -mPlayer.rotation, new Vector2((float)(tex.Width / 2), (float)(num121 / 2)), projectile.scale, effects, 0);
						if(mPlayer.screwAttackSpeedEffect > 0)
						{
							Color color21 = alpha * ((float)Math.Min(mPlayer.screwAttackSpeedEffect,30)/30f);
							spriteBatch.Draw(tex2, drawInfo.position + P.fullRotationOrigin - Main.screenPosition, new Rectangle?(new Rectangle(0, y9, tex2.Width, num121 - 1)), color21, -mPlayer.rotation, new Vector2((float)(tex2.Width / 2), (float)(num121 / 2)), projectile.scale, effects, 0);
							Texture2D tex3 = mod.GetTexture("Gore/ScrewAttack_YellowPlayerGlow");
							Main.playerDrawData.Add(new DrawData(tex3, drawInfo.position + (P.Center-P.position) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex3.Width, tex3.Height)), color21, 0f, new Vector2((float)(tex3.Width / 2), (float)(tex3.Height / 2)), projectile.scale, effects, 0));
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
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.isPowerSuit && !mPlayer.ballstate)
			{
				Texture2D tex = mod.GetTexture("Gore/VisorGlowNew");
				mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headOrigin, drawPlayer.GetImmuneAlphaPure(mPlayer.visorGlowColor,drawInfo.shadow), 0);
			}
		});
		public static readonly PlayerLayer ballLayer = new PlayerLayer("MetroidMod", "ballLayer", delegate(PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            if (!drawPlayer.active || drawPlayer.outOfRange || Main.gameMenu) return;
            
			Texture2D tex = MetroidMod.Instance.GetTexture("Gore/Morphball");
			Texture2D tex3 = MetroidMod.Instance.GetTexture("Gore/Mockball");
			Texture2D boost = MetroidMod.Instance.GetTexture("Gore/Boostball");
            Texture2D tex2 = MetroidMod.Instance.GetTexture("Gore/Morphball_Light");
            Texture2D spiderTex = MetroidMod.Instance.GetTexture("Gore/Spiderball");
            Texture2D trail = MetroidMod.Instance.GetTexture("Gore/Morphball_Trail");
            MPlayer mp = drawPlayer.GetModPlayer<MPlayer>();

            float thisx = (int)(drawInfo.position.X + (drawPlayer.width / 2));
            float thisy = (int)(drawInfo.position.Y + (drawPlayer.height / 2));
            
            Vector2 ballDims = new Vector2(28f, 28f);
            Vector2 thispos = new Vector2(thisx, thisy) - Main.screenPosition;

            if (drawInfo.shadow == 0f)
            {
                int timez = (int)(mp.Time % 60) / 10;

                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                    effects = SpriteEffects.FlipHorizontally;
                if (drawPlayer.gravDir == -1f)
                    effects |= SpriteEffects.FlipVertically;

                float ballrotoffset = 0f;
                if (drawPlayer.velocity.Y != Vector2.Zero.Y)
                {
                    if (drawPlayer.velocity.X != 0f)
                        ballrotoffset += 0.05f * drawPlayer.velocity.X;
                    else
                        ballrotoffset += 0.25f * drawPlayer.direction;
                }
                else if (drawPlayer.velocity.X < 0f)
                    ballrotoffset -= 0.2f;
                else if (drawPlayer.velocity.X > 0f)
                    ballrotoffset += 0.2f;

                if (drawPlayer.velocity.X != 0f)
                    ballrotoffset += 0.025f * drawPlayer.velocity.X;
                else
                    ballrotoffset += 0.125f * drawPlayer.direction;

                if (mp.spiderball && mp.CurEdge != Edge.None)
                    mp.ballrot += mp.spiderSpeed * 0.085f;
                else
                    mp.ballrot += ballrotoffset;

                Color mColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, mp.morphColor), 0f);
                float scale = 0.57f;
                int offset = 4;
                if (mp.ballstate && drawPlayer.active && !drawPlayer.dead)
                {
                    DrawData data;
                    for (int i = 0; i < mp.oldPos.Length; i++)
                    {
                        Color color23 = mp.morphColorLights;
						if(mp.shineActive)// || (mp.shineCharge > 0 && mp.shineChargeFlash >= 4))
						{
                            color23 = new Color(255, 216, 0, 255);
						}
						else if(mp.speedBoosting)
						{
                            color23 = new Color(0, 200, 255, 255);
						}
						if(mp.boostEffect > 0)
						{
							Color gold = new Color(255,255,0,255);
							color23 = Color.Lerp(color23, gold, (float)mp.boostEffect/60f);
						}
                        color23 *= (mp.oldPos.Length - (i)) / 15f;

                        Vector2 drawPos = mp.oldPos[i] - Main.screenPosition + new Vector2((int)(drawPlayer.width / 2), (int)(drawPlayer.height / 2));
						
						if(drawPos != thispos)
						{
							data = new DrawData(trail, drawPos, new Rectangle?(new Rectangle(0, 0, trail.Width, trail.Height)), color23, mp.ballrot, ballDims / 2, scale, effects, 0);
							
							Main.playerDrawData.Add(data);
						}
                    }

                    data = new DrawData(tex, thispos, new Rectangle?(new Rectangle(0, ((int)ballDims.Y + offset) * timez, (int)ballDims.X, (int)ballDims.Y)), mColor, mp.ballrot, ballDims / 2, scale, effects, 0);
                    Main.playerDrawData.Add(data);

                    data = new DrawData(tex2, thispos, new Rectangle?(new Rectangle(0, ((int)ballDims.Y + offset) * timez, (int)ballDims.X, (int)ballDims.Y)), mp.morphColorLights, mp.ballrot, ballDims / 2, scale, effects, 0);
                    Main.playerDrawData.Add(data);
                    
                    for (int i = 0; i < mp.boostEffect; i++)
                    {
                        data = new DrawData(boost, thispos, new Rectangle?(new Rectangle(0, 0, boost.Width, boost.Height)), mp.boostGold * 0.5f, mp.ballrot, ballDims / 2, scale, effects, 0);
                        Main.playerDrawData.Add(data);
                    }
                    for (int i = 0; i < mp.boostCharge; i++)
                    {
                        data = new DrawData(boost, thispos, new Rectangle?(new Rectangle(0, 0, boost.Width, boost.Height)), mp.boostYellow * 0.5f, mp.ballrot, ballDims / 2, scale, effects, 0);
                        Main.playerDrawData.Add(data);
                    }

                    if (mp.spiderball)
                    {
                        data = new DrawData(spiderTex, thispos, new Rectangle?(new Rectangle(0, 0, spiderTex.Width, spiderTex.Height)), mp.morphColorLights * 0.5f, mp.ballrot, new Vector2(spiderTex.Width / 2, spiderTex.Height / 2), scale, effects, 0);
                        Main.playerDrawData.Add(data);
                    }
                }
            }
        });
		public static readonly PlayerLayer gunLayer = new PlayerLayer("MetroidMod", "gunLayer", PlayerLayer.Arms, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
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
                    if (tex != null)
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
					if (P.gravDir == -1f)
					{
						effects |= SpriteEffects.FlipVertically;
						pos.Y -= 2;
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
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
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
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.jet && !drawPlayer.sandStorm && drawInfo.shadow == 0f && mPlayer.thrusters)
			{
				if((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
				{
					Texture2D tex = mod.GetTexture("Gore/thrusterFlameNew");
					if(mPlayer.shineDirection != 0)
					{
						tex = mod.GetTexture("Gore/thrusterFlameNew_Spark");
					}
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
			if (drawPlayer.gravDir == -1f)
			{
				effects |= SpriteEffects.FlipVertically;
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
			if (drawPlayer.gravDir == -1f)
			{
				effects |= SpriteEffects.FlipVertically;
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

        public void SenseMove(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
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
		public void AddScrewAttack(Player player, int damageMult)
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
					screwProj = Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,screwAttackID,specialDmg*damageMult,0,player.whoAmI);
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
		
		// Using these so that I don't have to write out the entire method every time
		public bool CheckCollide(float offsetX, float offsetY)
		{
			return CheckCollide(player.position+new Vector2(offsetX,offsetY), player.width, player.height);
		}
		public bool CheckCollide(Vector2 Position, int Width, int Height)
		{
			return CollideMethods.CheckCollide(Position,Width,Height);
		}

        public void AddSpeedBoost(Player player)
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
		
		public void Bomb(Player player)
		{
			if(bomb <= 0 && player.controlUseTile && !player.tileInteractionHappened && player.releaseUseItem && !player.controlUseItem && !player.mouseInterface && !CaptureManager.Instance.Active && !Main.HoveringOverAnNPC && !Main.SmartInteractShowingGenuine)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayBomb"));
				int BombID = mod.ProjectileType("MBBomb");
				int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,BombID,bombDamage,0,player.whoAmI);
				Main.projectile[a].aiStyle = 0;
				bomb = 20;
			}
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
		public void PowerBomb(Player player, int type)
		{
			if(statPBCh <= 0 && MetroidMod.PowerBombKey.JustPressed && shineDirection == 0)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)player.position.X, (int)player.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/LayPowerBomb"));
				statPBCh = 200;
				int a = Terraria.Projectile.NewProjectile(player.Center.X,player.Center.Y+4,0,0,type,specialDmg/4,0,player.whoAmI);
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
        
        /* NETWORK SYNCING. <<<<<< WIP >>>>>> */

        // Using Initialize to make sure every player has his/her own instance.
        public override void Initialize()
        {
            oldPos = new Vector2[oldNumMax];

            spiderball = false;

            statCharge = 0;
        }

        public override void clientClone(ModPlayer clientClone)
        {
            MPlayer clone = clientClone as MPlayer;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MetroidMessageType.SyncPlayerStats);
            packet.Write((byte)player.whoAmI);
            packet.Write((double)statCharge);
            packet.Write(spiderball);
            packet.Write(boostEffect);
            packet.Write(boostCharge);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            MPlayer mp = clientPlayer as MPlayer;
            if (mp != null)
            {
                if(mp.statCharge != statCharge || mp.spiderball != spiderball || mp.boostEffect != boostEffect || mp.boostCharge != boostCharge)
                {
                    ModPacket packet = mod.GetPacket();
                    packet.Write((byte)MetroidMessageType.SyncPlayerStats);
                    packet.Write((byte)player.whoAmI);
                    packet.Write((double)statCharge);
                    packet.Write(spiderball);
                    packet.Write(boostEffect);
                    packet.Write(boostCharge);
                    packet.Send();
                }
            }
        }
    }
}
