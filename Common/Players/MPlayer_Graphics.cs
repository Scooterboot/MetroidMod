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

//using MetroidModPorted.Content.NPCs;
//using MetroidModPorted.Content.Items;
//using MetroidModPorted.Common.Systems;

namespace MetroidModPorted.Common.Players
{
	public partial class MPlayer : ModPlayer  
	{
		public int style;
		public int r = 255;
		public int g = 0;
		public int b = 0;
		
		public Color chargeColor = Color.White;
		public int hyperColors = 0;
		public bool jet = false;
		private int tweak = 0;
		
		public bool isPowerSuit = false;
		public bool isLegacySuit = false;
		public bool thrusters = false;
		public bool visorGlow = false;
		public Color visorGlowColor = new(255, 255, 255);
		
		public float ballrot = 0f;
		public static int oldNumMax = 10;
		public Vector2[] oldPos = new Vector2[oldNumMax];
		
		public int psuedoScrewFlash = 0;
		public int shineChargeFlash = 0;
		private Rectangle jetFrame;
		private int jetFrameCounter = 1;
		private int currentFrame = 0;
		
		public void ResetEffects_Graphics()
		{
			chargeColor = Color.White;
			isPowerSuit = false;
			isLegacySuit = false;
			thrusters = false;

			visorGlow = false;
			visorGlowColor = new Color(255, 255, 255);
		}
		public void PreUpdate_Graphics()
		{
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
			
			bool trail = (!Player.dead && !Player.mount.Active && Player.grapCount == 0 && shineDirection == 0 && !shineActive && !ballstate);
			if(trail && ((Player.controlJump && Player.jump > 0 && (isPowerSuit || isLegacySuit)) || (grapplingBeam >= 0 && (Math.Abs(Player.velocity.X) >= 8.5f || Math.Abs(Player.velocity.Y) >= 8.5f)) || (spaceJump && somersault) || SMoveEffect > 0))
			{
				tweak++;
				if(tweak > 4)
				{
					tweak = 5;
					Player.armorEffectDrawShadow = true;
				}
			}
			else
			{
				tweak = 0;
			}
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			//Player P = drawInfo.drawPlayer;
			//MPlayer mPlayer = P.GetModPlayer<MPlayer>();

			if (drawInfo.shadow == 0f && !drawInfo.headOnlyRender)
			{
				for (int i = oldPos.Length - 1; i > 0; i--)
				{
					Vector2 vect = oldPos[1] - oldPos[0];
					oldPos[i] = oldPos[i - 1] - (vect * 0.5f);
				}
				oldPos[0] = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y);

				int width = 8;
				if (Vector2.Distance(oldPos[0], oldPos[1]) > width)
				{
					for (int i = 1; i < oldPos.Length; i++)
					{
						Vector2 pos = oldPos[i - 1] - oldPos[i];
						float len = pos.Length();

						len = (len - (float)width) / len;
						pos.X *= len;
						pos.Y *= len;
						oldPos[i] += pos;
					}
				}
			}

			bool pseudoScrew = (statCharge >= maxCharge && somersault);
			if (pseudoScrew)
			{
				if (drawInfo.shadow == 0f)
				{
					psuedoScrewFlash++;
				}
			}
			else
			{
				psuedoScrewFlash = 0;
			}
			if (shineCharge > 0)
			{
				if (drawInfo.shadow == 0f)
				{
					shineChargeFlash++;
				}
			}
			else
			{
				shineChargeFlash = 0;
			}
			if (hyperColors > 0 || speedBoosting || shineActive || (pseudoScrew && psuedoScrewFlash >= 3) || (shineCharge > 0 && shineChargeFlash >= 4))
			{
				int shader = GameShaders.Armor.GetShaderIdFromItemId(3558);
				if (drawInfo.drawPlayer.head > 0 && drawInfo.drawPlayer.cHead <= 0)
				{
					drawInfo.cHead = shader;
				}
				if (drawInfo.drawPlayer.body > 0 && drawInfo.drawPlayer.cBody <= 0)
				{
					drawInfo.cBody = shader;
				}
				if (drawInfo.drawPlayer.legs > 0 && drawInfo.drawPlayer.cLegs <= 0)
				{
					drawInfo.cLegs = shader;
				}

				if (drawInfo.shadow == 0f && hyperColors > 0)
				{
					hyperColors--;
				}
				if (psuedoScrewFlash >= 9)
				{
					psuedoScrewFlash = 0;
				}
				if (shineChargeFlash >= 6)
				{
					shineChargeFlash = 0;
				}
			}
			if (isGripping)
			{
				if (Player.position.X % 32 > 16 && Player.velocity.X != 0)
				{
					Player.bodyFrame.Y = Player.bodyFrame.Height * 1;
				}
				else
				{
					Player.bodyFrame.Y = Player.bodyFrame.Height * 2;
				}
			}
			if (Player.velocity.Y * Player.gravDir < 0 && reGripTimer > 0)
			{
				if (reGripTimer > 5)
				{
					Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
				}
				else
				{
					Player.bodyFrame.Y = Player.bodyFrame.Height * 4;
				}
			}
			if (somersault || (canWallJump && (drawInfo.drawPlayer.controlLeft || drawInfo.drawPlayer.controlRight) && !isGripping && !drawInfo.drawPlayer.sliding))
			{
				drawInfo.drawPlayer.bodyFrame.Y = drawInfo.drawPlayer.bodyFrame.Height * 6;
				drawInfo.drawPlayer.legFrame.Y = drawInfo.drawPlayer.legFrame.Height * 7;
				drawInfo.drawPlayer.wingFrame = 1;
				if (drawInfo.drawPlayer.wings == ArmorIDs.Wing.Jetpack)
				{
					drawInfo.drawPlayer.wingFrame = 3;
				}
			}
			else if (shineActive && shineDirection == 0 && shineDischarge > 0)
			{
				if (shineDischarge < 15)
				{
					drawInfo.drawPlayer.bodyFrame.Y = drawInfo.drawPlayer.bodyFrame.Height * 5;
				}
				else if (shineDischarge <= 30)
				{
					drawInfo.drawPlayer.bodyFrame.Y = drawInfo.drawPlayer.bodyFrame.Height * 6;
				}
				drawInfo.drawPlayer.legFrame.Y = drawInfo.drawPlayer.legFrame.Height * 5;
			}
			else if (shineDirection != 0)
			{
				drawInfo.drawPlayer.bodyFrame.Y = drawInfo.drawPlayer.bodyFrame.Height * 6;
				drawInfo.drawPlayer.legFrame.Y = drawInfo.drawPlayer.legFrame.Height * 7;
				if (shineDirection == 5 || shineDirection == 8)
				{
					drawInfo.drawPlayer.bodyFrame.Y = 0;
					drawInfo.drawPlayer.legFrameCounter = 0.0;
					drawInfo.drawPlayer.legFrame.Y = 0;
				}
				else
				{
					jet = true;
				}
			}
			ModifyDrawInfo_GetArmors(ref drawInfo);
		}
		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			Player p = drawInfo.drawPlayer;
			p.TryGetModPlayer(out MPlayer mPlayer);

			/*if (somersault || (canWallJump && (p.controlLeft || p.controlRight) && !isGripping && !p.sliding))
			{
				p.bodyFrame.Y = p.bodyFrame.Height * 6;
				p.legFrame.Y = p.legFrame.Height * 7;
				p.wingFrame = 1;
				if (p.wings == ArmorIDs.Wing.Jetpack)
				{
					p.wingFrame = 3;
				}
			}
			else if (shineActive && shineDirection == 0 && shineDischarge > 0)
			{
				if (shineDischarge < 15)
				{
					p.bodyFrame.Y = p.bodyFrame.Height * 5;
				}
				else if (shineDischarge <= 30)
				{
					p.bodyFrame.Y = p.bodyFrame.Height * 6;
				}
				p.legFrame.Y = p.legFrame.Height * 5;
			}
			else if (shineDirection != 0)
			{
				p.bodyFrame.Y = p.bodyFrame.Height * 6;
				p.legFrame.Y = p.legFrame.Height * 7;
				if (shineDirection == 5 || shineDirection == 8)
				{
					p.bodyFrame.Y = 0;
					p.legFrameCounter = 0.0;
					p.legFrame.Y = 0;
				}
				else
				{
					jet = true;
				}
				if (thrusters)
				{
					p.wings = -1;
					p.back = -1;
					//PlayerLayer.Wings.visible = false;
					//PlayerLayer.BackAcc.visible = false;
				}
				else
				{
					p.wingFrame = 2;
					if (p.wings == ArmorIDs.Wing.Jetpack)
					{
						p.wingFrame = 3;
					}
					if (shineDirection == 5 || shineDirection == 8)
					{
						p.wingFrame = 0;
						if (p.wings == ArmorIDs.Wing.Jetpack)
						{
							p.wingFrame = 3;
						}
					}
				}
			}*/
			/*else
			{
				if (grapplingBeam >= 0 && p.itemAnimation <= 0)
				{
					float num11 = grappleRotation * (float)p.direction;
					p.bodyFrame.Y = p.bodyFrame.Height * 3;
					if ((double)num11 < -0.75)
					{
						p.bodyFrame.Y = p.bodyFrame.Height * 2;
						if (p.gravDir == -1f)
						{
							p.bodyFrame.Y = p.bodyFrame.Height * 4;
						}
					}
					if ((double)num11 > 0.6)
					{
						p.bodyFrame.Y = p.bodyFrame.Height * 4;
						if (p.gravDir == -1f)
						{
							p.bodyFrame.Y = p.bodyFrame.Height * 2;
						}
					}
				}
			}*/
			if (ballstate)
			{
				//for (int i = 0; i < layers.Count; ++i)
					//layers[i].visible = false;
				//layers.Add(ballLayer);
				//ballLayer.visible = true;
			}
			else
			{
				if (somersault || shineActive)
				{
					PlayerDrawLayers.HeldItem.Hide();
				}
				if (thrusters)
				{
					if ((drawInfo.drawPlayer.wings == 0 && drawInfo.drawPlayer.back == -1) || drawInfo.drawPlayer.velocity.Y == 0f || drawInfo.drawPlayer.GetModPlayer<MPlayer>().shineDirection != 0)
					{
						PlayerDrawLayers.Wings.Hide();
						PlayerDrawLayers.BackAcc.Hide();
					}
				}
			}
		}
		/*
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			MPlayer mPlayer = Player.GetModPlayer<MPlayer>();
			Player P = Player;

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
					thrusterLayer.visible = true;
					jetLayer.visible = true;
				}
				if (layers[k] == PlayerLayer.Head)
				{
					layers.Insert(k + 1, visorLayer);
					visorLayer.visible = true;

				}
				if(layers[k] == PlayerLayer.HandOnAcc)
				{
					layers.Insert(k + 1, gunLayer);
					gunLayer.visible = true;
				}
				if(layers[k] == PlayerLayer.HeldItem)
				{
					layers.Insert(k + 1, gunItemLayer);
					//gunItemLayer.visible = true;
				}
			}
			
			Item I = P.inventory[P.selectedItem];
			if (I.type == mod.ItemType("PowerBeam") || I.type == mod.ItemType("MissileLauncher") || I.type == mod.ItemType("NovaLaserDrill"))
			{
				gunItemLayer.visible = true;
				PlayerLayer.HeldItem.visible = false;
			}
			else
			{
				gunItemLayer.visible = false;
				PlayerLayer.HeldItem.visible = true;
			}
			
			if(somersault || (canWallJump && (player.controlLeft || player.controlRight) && !isGripping && !player.sliding)))
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
			else if(shineDirection != 0)
			{
				P.bodyFrame.Y = P.bodyFrame.Height * 6;
				P.legFrame.Y = P.legFrame.Height * 7;
				if(shineDirection == 5 || shineDirection == 8)
				{
					P.bodyFrame.Y = 0;
					P.legFrameCounter = 0.0;
					P.legFrame.Y = 0;
				}
				else
				{
					jet = true;
				}
				if(thrusters)
				{
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
					if(shineDirection == 5 || shineDirection == 8)
					{
						P.wingFrame = 0;
						if (P.wings == 4)
						{
							P.wingFrame = 3;
						}
					}
				}
			}
			else
			{
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
				for (int i = 0; i < layers.Count; ++i)
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
			
			if(!isPowerSuit && !isLegacySuit)
				jet = false;
		}*/
		/*public static readonly PlayerLayer screwAttackLayer = new PlayerLayer("MetroidMod", "screwAttackLayer", delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (mPlayer.somersault && mPlayer.screwAttack && drawInfo.shadow == 0f && !mPlayer.ballstate)
			{
				Texture2D tex = ModContent.Request<Texture2D>("Projectiles/ScrewAttackProj");
				Texture2D tex2 = ModContent.Request<Texture2D>("Gore/ScrewAttack_Yellow");
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
							Texture2D tex3 = ModContent.Request<Texture2D>("Gore/ScrewAttack_YellowPlayerGlow");
							Main.PlayerDrawData.Add(new DrawData(tex3, drawInfo.position + (P.Center-P.position) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex3.Width, tex3.Height)), color21, 0f, new Vector2((float)(tex3.Width / 2), (float)(tex3.Height / 2)), projectile.scale, effects, 0));
						}
					}
				}
			}
		});*/
		/*public static readonly PlayerLayer visorLayer = new PlayerLayer("MetroidMod", "visorLayer", PlayerLayer.Head, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.isLegacySuit && !mPlayer.ballstate)
			{
				Texture2D tex = ModContent.Request<Texture2D>("Gore/VisorGlow");
				mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.headRotation, drawPlayer.bodyPosition, drawInfo.headOrigin, drawPlayer.GetImmuneAlphaPure(mPlayer.visorGlowColor,drawInfo.shadow), 0);
			}
		});*/
		/*public static readonly PlayerLayer ballLayer = new PlayerLayer("MetroidMod", "ballLayer", delegate(PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!drawPlayer.active || drawPlayer.outOfRange || Main.gameMenu) return;
			
			Texture2D tex = MetroidMod.Instance.GetTexture("Gore/Morphball");
			Texture2D tex3 = MetroidMod.Instance.GetTexture("Gore/Morphball_Dye");
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

				if(Mount.currentShader > 0)
				{
					mp.morphColor = Color.White;
					tex = tex3;
				}
				Color mColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, mp.morphColor), 0f);
				float scale = 0.57f;
				int offset = 4;
				if (mp.ballstate && !drawPlayer.dead)
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
							
							Main.PlayerDrawData.Add(data);
						}
					}

					data = new DrawData(tex, thispos, new Rectangle?(new Rectangle(0, ((int)ballDims.Y + offset) * timez, (int)ballDims.X, (int)ballDims.Y)), mColor, mp.ballrot, ballDims / 2, scale, effects, 0);
					//data.shader = drawPlayer.mount.currentShader;
					data.shader = Mount.currentShader;
					Main.PlayerDrawData.Add(data);

					data = new DrawData(tex2, thispos, new Rectangle?(new Rectangle(0, ((int)ballDims.Y + offset) * timez, (int)ballDims.X, (int)ballDims.Y)), mp.morphColorLights, mp.ballrot, ballDims / 2, scale, effects, 0);
					Main.PlayerDrawData.Add(data);
					
					for (int i = 0; i < mp.boostEffect; i++)
					{
						data = new DrawData(boost, thispos, new Rectangle?(new Rectangle(0, 0, boost.Width, boost.Height)), mp.boostGold * 0.5f, mp.ballrot, ballDims / 2, scale, effects, 0);
						Main.PlayerDrawData.Add(data);
					}
					for (int i = 0; i < mp.boostCharge; i++)
					{
						data = new DrawData(boost, thispos, new Rectangle?(new Rectangle(0, 0, boost.Width, boost.Height)), mp.boostYellow * 0.5f, mp.ballrot, ballDims / 2, scale, effects, 0);
						Main.PlayerDrawData.Add(data);
					}

					if (mp.spiderball)
					{
						data = new DrawData(spiderTex, thispos, new Rectangle?(new Rectangle(0, 0, spiderTex.Width, spiderTex.Height)), mp.morphColorLights * 0.5f, mp.ballrot, new Vector2(spiderTex.Width / 2, spiderTex.Height / 2), scale, effects, 0);
						Main.PlayerDrawData.Add(data);
					}
				}
			}
		});
		public static readonly PlayerLayer gunItemLayer = new PlayerLayer("MetroidMod", "gunItemLayer", delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			Player P = drawInfo.drawPlayer;
			Item I = P.inventory[P.selectedItem];
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			if (drawInfo.shadow != 0f || P.frozen || ((P.itemAnimation <= 0 || I.useStyle == 0) && (I.holdStyle <= 0 || P.pulley)) || I.type <= 0 || P.dead || I.noUseGraphic || (P.wet && I.noWet) || mPlayer.somersault)
			{
				return;
			}
			
			if (I.type == mod.ItemType("PowerBeam") || I.type == mod.ItemType("MissileLauncher") || I.type == mod.ItemType("NovaLaserDrill"))
			{
				Texture2D tex = Main.itemTexture[I.type];
				MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
				if(mi.itemTexture != null)
				{
					tex = mi.itemTexture;
				}
				Color currentColor = Lighting.GetColor((int)((double)drawInfo.position.X + (double)P.width * 0.5) / 16, (int)(((double)drawInfo.position.Y + (double)P.height * 0.5) / 16.0));
				
				int num80 = 10;
				Vector2 vector7 = new Vector2(tex.Width / 2, tex.Height / 2);
				Vector2 vector8 = new Vector2(24f / 2, tex.Height / 2);
				num80 = (int)vector8.X;
				vector7.Y = vector8.Y;
				Vector2 origin4 = new Vector2(-num80, tex.Height / 2);
				if (P.direction == -1)
				{
					origin4 = new Vector2(tex.Width + num80, tex.Height / 2);
				}
				DrawData item2 = new DrawData(tex, new Vector2((int)(drawInfo.itemLocation.X - Main.screenPosition.X + vector7.X), (int)(drawInfo.itemLocation.Y - Main.screenPosition.Y + vector7.Y)), new Rectangle(0, 0, tex.Width, tex.Height), drawInfo.middleArmorColor, P.itemRotation, origin4, I.scale, drawInfo.spriteEffects, 0);
				item2.shader = drawInfo.bodyArmorShader;
				Main.PlayerDrawData.Add(item2);
			}
		});*/
		/*public static readonly PlayerLayer gunLayer = new PlayerLayer("MetroidMod", "gunLayer", PlayerLayer.HandOnAcc, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			Player P = drawInfo.drawPlayer;
			MPlayer mPlayer = P.GetModPlayer<MPlayer>();
			Item I = P.inventory[P.selectedItem];
			int frame = (int)(P.bodyFrame.Y/P.bodyFrame.Height);
			if ((I.type == mod.ItemType("PowerBeam") || I.type == mod.ItemType("MissileLauncher") || I.type == mod.ItemType("NovaLaserDrill")) && ((P.itemAnimation == 0 && (frame < 1 || frame > 4)) || (mPlayer.statCharge > 0 && mPlayer.somersault)) && !P.dead)
			{
				Texture2D tex = Main.itemTexture[I.type];
				MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
				if(mi.itemTexture != null)
				{
					tex = mi.itemTexture;
				}
				
				if (tex != null)
				{
					Vector2 origin = new Vector2(14f, (float)((int)(tex.Height/2)));
					if(P.direction == -1)
					{
						origin.X = tex.Width - 14;
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
					Main.PlayerDrawData.Add(item);
				}
			}
		});*/
		/*public static readonly PlayerLayer thrusterLayer = new PlayerLayer("MetroidMod", "thrusterLayer", PlayerLayer.Body, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			
			Item item = null;
			if(drawPlayer.armor[1] != null && !drawPlayer.armor[1].IsAir && drawPlayer.armor[1].modItem != null)
			{
				item = drawPlayer.armor[1];
			}
			if(drawPlayer.armor[11] != null && !drawPlayer.armor[11].IsAir && drawPlayer.armor[11].modItem != null)
			{
				item = drawPlayer.armor[11];
			}
			if (mPlayer.thrusters && item != null)
			{
				string name = item.modItem.Texture + "_Thrusters";
				if(ModContent.TextureExists(name) && name.Contains("MetroidMod"))
				{
					if((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
					{
						Texture2D tex = ModContent.GetTexture(name);
						mPlayer.DrawTexture(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyFrame, drawPlayer.bodyRotation, drawPlayer.bodyPosition, drawInfo.bodyOrigin, drawInfo.middleArmorColor, drawInfo.bodyArmorShader);
					}
				}
			}
		});*/
		/*public static readonly PlayerLayer jetLayer = new PlayerLayer("MetroidMod", "jetLayer", PlayerLayer.Body, delegate(PlayerDrawInfo drawInfo)
		{
			Mod mod = MetroidMod.Instance;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Player drawPlayer = drawInfo.drawPlayer;
			MPlayer mPlayer = drawPlayer.GetModPlayer<MPlayer>();
			if (mPlayer.jet && !drawPlayer.sandStorm && drawInfo.shadow == 0f)
			{
				if((drawPlayer.wings == 0 && drawPlayer.back == -1) || drawPlayer.velocity.Y == 0f || mPlayer.shineDirection != 0)
				{
					Texture2D tex = ModContent.Request<Texture2D>("Gore/thrusterFlameNew");
					if(mPlayer.shineDirection != 0 || mPlayer.SMoveEffect > 15)
					{
						tex = ModContent.Request<Texture2D>("Gore/thrusterFlameNew_Spark");
					}
					if(mPlayer.thrusters)
					{
						tex = ModContent.Request<Texture2D>("Gore/thrusterFlame");
					}
					mPlayer.DrawThrusterJet(spriteBatch, drawInfo, tex, drawPlayer, drawPlayer.bodyRotation, drawPlayer.bodyPosition);
				}
			}
		});*/
		/*public void DrawTexture(SpriteBatch sb, PlayerDrawInfo drawInfo, Texture2D tex, Player drawPlayer, Rectangle frame, float rot, Vector2 drawPos, Vector2 origin, Color color, int shader)
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
			Main.PlayerDrawData.Add(item);
		}*/
		public void DrawThrusterJet(ref PlayerDrawSet drawInfo, Texture2D tex, Player drawPlayer, float rot, Vector2 drawPos)
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
			drawInfo.DrawDataCache.Add(new DrawData(tex, new Vector2((float)((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(jetFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)jetFrame.Height + yfloat))) + drawPos + drawInfo.bodyVect, new Rectangle?(jetFrame), Color.White, rot, drawInfo.bodyVect, 1f, effects, 0));
		}
		public static void DrawTexture(ref PlayerDrawSet drawInfo, Texture2D tex, Player drawPlayer, Rectangle frame, float rot, Vector2 drawPos, Vector2 origin, Color color, int shader)
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
			DrawData item = new(tex, new Vector2((float)((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(frame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)frame.Height + 4f))) + drawPos + origin, new Rectangle?(frame), color, rot, origin, 1f, effects, 0);
			item.shader = shader;
			drawInfo.DrawDataCache.Add(item);
		}
	}
}
