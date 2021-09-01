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
	/* TODO:
	 * Rework the Standing on NPC mechanic to accept a list of NPC types in PreUpdateMovement.
	 */
	public partial class MPlayer : ModPlayer  
	{
		public float statCharge = 0.0f;
		public static float maxCharge = 100.0f;
		
		public float maxOverheat = 100f;
		public float statOverheat = 0f;
		public float overheatCost = 1f;
		public int overheatDelay = 0;
		float overheatCooldown = 0f;
		public float missileCost = 1f;
		
		public bool senseMove = false;
		public bool senseMoveEnabled = true;
		public int SMoveEffect = 0;
		bool senseSound = false;
		int senseMoveCooldown = 0;
		int dashTime = 0;
		
		public float grappleRotation = 0f;
		public float maxDist;
		public int grapplingBeam = -1;
		
		public bool phazonImmune = false;
		public bool canUsePhazonBeam = false;
		public bool hazardShield = false;
		public int phazonRegen = 0;
		
		public double Time = 0;
		public float breathMult = 1f;
		public Vector2 oldPosition;
		
        public bool falling;

		public override void ResetEffects()
		{
			ResetEffects_Accessories();
			ResetEffects_MorphBall();
			ResetEffects_Graphics();
			
			maxOverheat = 100f;
			overheatCost = 1f;
			missileCost = 1f;
			
			senseMove = false;
			
			phazonImmune = false;
			canUsePhazonBeam = false;
			hazardShield = false;
			phazonRegen = 0;
			
			breathMult = 1f;
		}
		public override void PreUpdate()
		{
			PreUpdate_Accessories();
			PreUpdate_MorphBall();
			PreUpdate_Graphics();
			
			Player P = player;
			
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
			
			Time += 1.0;
			if(Time > 54000.0)
			{
				Time = 0;
			}
			P.breathMax = (int)(200 * breathMult);
			oldPosition = P.position;
			
			if(visorGlow && !ballstate)
			{
				Lighting.AddLight((int)((float)player.Center.X/16f), (int)((float)(player.position.Y+8f)/16f), ((float)visorGlowColor.R/255)*0.375f,((float)visorGlowColor.G/255)*0.375f,((float)visorGlowColor.B/255)*0.375f);
			}
			if(jet)
			{
				Lighting.AddLight((int)((float)player.Center.X/16f), (int)((float)player.Center.Y/16f), 0.6f, 0.38f, 0.24f);
			}
			
			if(!phazonImmune)
			{
				if(TouchTiles(player.position, player.width, player.height, mod.TileType("PhazonTile")))
				{
					player.AddBuff(mod.BuffType("PhazonDebuff"), 2);
				}
			}
			else
			{
				if(TouchTiles(player.position, player.width, player.height, mod.TileType("PhazonTile")) && phazonRegen > 0)
				{
					player.lifeRegen += phazonRegen;
				}
			}
			if(TouchTiles(player.position, player.width, player.height, mod.TileType("PhazonCore")))
			{
				player.AddBuff(mod.BuffType("PhazonDebuff"), 2);
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
			int x1 = (int)MathHelper.Clamp((player.position.X + player.velocity.X - 1) / 16, 0, Main.maxTilesX-1);
			int x2 = (int)MathHelper.Clamp((player.position.X + player.velocity.X + player.width + 1) / 16, 0, Main.maxTilesX-1);
			int j = (int)MathHelper.Clamp((player.position.Y + player.height + 1) / 16, 0, Main.maxTilesY-1);
			for (int i = x1; i <= x2; i++)
			{
                if(Main.tile[i, j].active() && !Main.tile[i, j].inActive())
                {
                    if (MWorld.mBlockType[i, j] == 1) //CrumbleInstant
                    {
                        MWorld.AddRegenBlock(i, j, true);
                        // Enforce SpeedBooster
                        if(falling){
                            player.velocity.X = 0;
                            player.oldVelocity.X = 0;
                        }
                    }
                    if (MWorld.mBlockType[i, j] == 2) //CrumbleSpeed
                    {
                        MWorld.nextTick.Enqueue(new Tuple<int,Vector2>((int)(MWorld.Timer) + 1, new Vector2(i, j)));
                    }
                    if (MWorld.mBlockType[i, j] == 11) //CrumbleSlow
                    {
                        MWorld.hit[i, j] = true;
                        MWorld.timers.Enqueue(new Tuple<int,Vector2>((int)(MWorld.Timer) + 60, new Vector2(i, j)));
                    }
                }
			}
            #region speedBoost & screwAttack
			x1 = (int)MathHelper.Clamp((player.position.X + player.velocity.X - 3) / 16, 0, Main.maxTilesX-1);
			x2 = (int)MathHelper.Clamp((player.position.X + player.velocity.X + player.width + 3) / 16, 0, Main.maxTilesX-1);
			int y1 = (int)MathHelper.Clamp((player.position.Y + player.velocity.Y - 16) / 16, 0, Main.maxTilesY-1);
			int y2 = (int)MathHelper.Clamp((player.position.Y + player.velocity.Y + player.height + 3) / 16, 0, Main.maxTilesY-1);
			for (int i = x1; i <= x2; i++)
			{
                for (int k = y1; k <= y2; k++)
                {
                    var mp = player.GetModPlayer<MPlayer>();
                    if(mp.speedBoosting || mp.shineActive)
                    {
                        if(Main.tile[i, j].active() && !Main.tile[i, j].inActive())
                        {
                            if (MWorld.mBlockType[i, k] == 5) //FakeBlock
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                            if (MWorld.mBlockType[i, k] == 6) //BoostBlock
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                            if (MWorld.mBlockType[i, k] == 10) //FakeBlockHint
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                        }
                    }
                    if(mp.somersault && mp.screwAttack)
                    {
                        if(Main.tile[i, j].active() && !Main.tile[i, j].inActive())
                        {
                            if (MWorld.mBlockType[i, k] == 3) //BombBlock
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                            if (MWorld.mBlockType[i, k] == 5) //FakeBlock
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                            if (MWorld.mBlockType[i, k] == 9) //ScrewAttackBlock
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                            if (MWorld.mBlockType[i, k] == 10) //FakeBlockHint
                            {
                                MWorld.AddRegenBlock(i, k);
                            }
                        }
                    }
                }
            }
            #endregion
            //Is there a better workaround for this?
            falling = false;
            if(Math.Sign(player.position.Y - player.oldPosition.Y) == player.gravDir)
            {
                falling = true;
            }
		}
		public static bool TouchTiles(Vector2 Position, int Width, int Height, int tileType)
		{
			Vector2 vector = Position;
			int num = (int)MathHelper.Clamp((Position.X / 16f) - 1, 0, Main.maxTilesX-1);
			int num2 = (int)MathHelper.Clamp(((Position.X + (float)Width) / 16f) + 2, 0, Main.maxTilesX-1);
			int num3 = (int)MathHelper.Clamp((Position.Y / 16f) - 1, 0, Main.maxTilesY-1);
			int num4 = (int)MathHelper.Clamp(((Position.Y + (float)Height) / 16f) + 2, 0, Main.maxTilesY-1);
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
		public override void PreUpdateMovement()
		{
			// 'Standing on NPC' mechanic. 
			// Might need some more work, but that's for something in the future.
			// TODO: THE PLAYER SLIDED OVER THE TOP OF THE TRIPPER WHEN IT CHANGES DIRECTION.
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				if (npc.active && (((MetroidMod)MetroidMod.Instance).FrozenStandOnNPCs.Contains(npc.type) || npc.type == mod.NPCType("Tripper")))
				{
					MGlobalNPC mnpc = npc.GetGlobalNPC<MGlobalNPC>();
					if (!mnpc.froze && npc.type != mod.NPCType("Tripper")) continue;

					if (player.position.X + player.width >= npc.position.X && player.position.X <= npc.position.X + npc.width &&
						player.position.Y + player.height <= npc.position.Y && player.position.Y + player.velocity.Y + player.height >= npc.position.Y)
					{
						player.velocity.Y = 0;
						player.position = player.oldPosition;

						if (npc.type == mod.NPCType("Tripper"))
						{
							if ((npc.direction == 1 && npc.velocity.X < 2) || (npc.direction == -1 && npc.velocity.X > -2))
								player.position.X = player.oldPosition.X + npc.velocity.X + (npc.direction * .08F);
							else
								player.position.X = player.oldPosition.X + npc.velocity.X;
						}
					}
				}
			}
		}
		public override void PostUpdateMiscEffects()
		{
			PostUpdateMiscEffects_Accessories();
			PostUpdateMiscEffects_MorphBall();
			
			if(senseMove && senseMoveEnabled)
			{
				SenseMove(player);
			}
			
			GrappleBeamMovement();
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
			
			if(SMoveEffect > 0)
			{
				SMoveEffect--;
			}
			if(senseMoveCooldown > 0)
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
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if(SMoveEffect > 0)
			{
				return false;
			}
			return true;
		}
		
		public void SenseMove(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			
			if(P.mount.Active || mp.ballstate)
			{
				return;
			}
			
			if(mp.SMoveEffect > 0)
			{
				if ((!P.controlLeft || !(P.velocity.X < 0f)) && (!P.controlRight || !(P.velocity.X > 0f)))
				{
					P.velocity.X *= 0.95f;
				}
				if(P.velocity.Y == 0f || !mp.spaceJump)
				{
					P.velocity.X *= 0.98f;
				}
			}
			
			int num20 = 0;
			bool flag2 = false;
			if(mp.senseMoveCooldown <= 0 && (P.velocity.Y == 0f || mp.spaceJump))
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
				
				if(!mp.senseSound)
				{
					Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SenseMoveSound"));
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
			if(grapplingBeam >= 0)
			{
				Projectile projectile = Main.projectile[grapplingBeam];
				if(projectile.type == mod.ProjectileType("GrappleBeamShot") && projectile.owner == player.whoAmI && projectile.active)
				{
					float targetrotation = (float)Math.Atan2(((projectile.Center.Y-player.Center.Y)*player.direction),((projectile.Center.X-player.Center.X)*player.direction));
					grappleRotation = targetrotation;

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

					Vector2 vel = Vector2.Zero;
					
					float maxMaxDist = 400;
					Vector2 v = player.Center - projectile.Center;
					float dist = Vector2.Distance(player.Center, projectile.Center);
					bool up = (player.controlUp && maxDist > 3);
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
							reel = Math.Max(-reelSpeed,2-dist);
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
		}
		
		public bool psuedoScrewActive = false;
		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"psuedoScrewAttackActive", psuedoScrewActive},
				{"senseMoveEnabled", senseMoveEnabled}
			};
		}
		public override void Load(TagCompound tag)
		{
			try
			{
				bool flag = tag.GetBool("psuedoScrewAttackActive");
				if (flag)
				{
					psuedoScrewActive = flag;
				}
				
				flag = tag.GetBool("senseMoveEnabled");
				if(!flag)
				{
					senseMoveEnabled = flag;
				}
			}
			catch { }
		}

		/* NETWORK SYNCING. <<<<<< WIP >>>>>> */

		// Using Initialize to make sure every player has his/her own instance.
		public override void Initialize()
		{
			oldPos = new Vector2[oldNumMax];

			spiderball = false;

			statCharge = 0;
			boostCharge = 0;
			boostEffect = 0;
		}

		public override void clientClone(ModPlayer clientClone)
		{
			MPlayer clone = clientClone as MPlayer;

			clone.statCharge = statCharge;
			clone.spiderball = spiderball;
			clone.boostEffect = boostEffect;
			clone.boostCharge = boostCharge;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)MetroidMessageType.SyncStartPlayerStats);
			packet.Write((byte)player.whoAmI);
			packet.Write((double)statCharge);
			packet.Write(spiderball);
			packet.Write(boostEffect);
			packet.Write(boostCharge);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			MPlayer clone = clientPlayer as MPlayer;
			if (clone.statCharge != statCharge || clone.spiderball != spiderball || clone.boostEffect != boostEffect || clone.boostCharge != boostCharge)
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
