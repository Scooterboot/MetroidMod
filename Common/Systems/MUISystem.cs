using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Common.Systems
{
	internal class MUISystem : ModSystem
	{
		public static MUISystem Instance { get; private set; }
		internal static UserInterface pbUserInterface;
		internal static UserInterface miUserInterface;
		internal static UserInterface mbUserInterface;
		internal static UserInterface suitUserInterface;
		internal static UserInterface visorUserInterface;
		//internal static UI.PowerBeamUI powerBeamUI;
		//internal static UI.AddonsUI addonsUI;

		//internal static UserInterface mlUserInterface;
		//internal static UI.MissileLauncherUI missileLauncherUI;

		//internal static UI.MorphBallUI morphBallUI;

		//internal static UserInterface smUserInterface;
		//internal static UI.SenseMoveUI senseMoveUI;

		internal bool isPBInit = false;
		internal bool isMIInit = false;
		internal bool isMBInit = false;
		internal bool isSUInit = false;
		internal bool isVIInit = false;

		internal bool isVisorBGAudioPlaying = false;
		internal ActiveSound VisorBGAudio;
		internal int oldVisorID = -1;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				//addonsUI = new UI.AddonsUI();
				pbUserInterface = new UserInterface();
				miUserInterface = new UserInterface();
				mbUserInterface = new UserInterface();
				suitUserInterface = new UserInterface();
				visorUserInterface = new UserInterface();

				/*powerBeamUI = new UI.PowerBeamUI();
				powerBeamUI.Activate();
				pbUserInterface = new UserInterface();
				pbUserInterface.SetState(powerBeamUI);*/

				/*missileLauncherUI = new UI.MissileLauncherUI();
				missileLauncherUI.Activate();
				mlUserInterface = new UserInterface();
				mlUserInterface.SetState(missileLauncherUI);

				morphBallUI = new UI.MorphBallUI();
				morphBallUI.Activate();
				mbUserInterface = new UserInterface();
				mbUserInterface.SetState(morphBallUI);

				senseMoveUI = new UI.SenseMoveUI();
				senseMoveUI.Activate();
				smUserInterface = new UserInterface();
				smUserInterface.SetState(senseMoveUI);*/
			}
		}

		public override void Unload()
		{
			pbUserInterface = null;
			miUserInterface = null;
			mbUserInterface = null;
			suitUserInterface = null;
			visorUserInterface = null;
			//powerBeamUI = null;
			//addonsUI = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (isVIInit == false)
			{
				visorUserInterface.SetState(new UI.VisorSelectUI());
				isVIInit = true;
			}
			if (isSUInit == false)
			{
				suitUserInterface.SetState(new UI.SuitAddonsUI());
				isSUInit = true;
			}
			if (isMBInit == false)
			{
				mbUserInterface.SetState(new UI.MorphBallUI());
				isMBInit = true;
			}
			if (isMIInit == false)
			{
				miUserInterface.SetState(new UI.MissileLauncherUI());
				isMIInit = true;
			}
			if (isPBInit == false)
			{
				pbUserInterface.SetState(new UI.PowerBeamUI());
				isPBInit = true;
			}
			if (visorUserInterface != null && UI.VisorSelectUI.Visible)
			{
				visorUserInterface.Update(gameTime);
			}
			if (suitUserInterface != null && UI.SuitAddonsUI.Visible)
			{
				suitUserInterface.Update(gameTime);
			}
			if (mbUserInterface != null && UI.MorphBallUI.Visible)
			{
				mbUserInterface.Update(gameTime);
			}
			if (miUserInterface != null && UI.MissileLauncherUI.Visible)
			{
				miUserInterface.Update(gameTime);
			}
			if (pbUserInterface != null && UI.PowerBeamUI.Visible)
			{
				pbUserInterface.Update(gameTime);
			}
		}

		private static int z = 0;
		private bool coordcheck = false;
		private List<Vector2> itemCoords = new();
		public override void PostDrawInterface(SpriteBatch sb)
		{
			Mod mod = Mod;
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			Item item = P.inventory[P.selectedItem];

			z = 0;
			for (int i = 0; i < P.buffType.Length; i += 11)
			{
				if (P.buffType[i] > 0) { z += 50; }
			}

			// (debug) draw npc hitboxes
			if (MetroidModPorted.DebugDH)
			{
				for (int j = 0; j < Main.maxNPCs; j++)
				{
					NPC npc = Main.npc[j];
					if (npc.active && npc.life > 0)
					{
						Color color = new(0, 255, 0);
						if (npc.dontTakeDamage)
						{
							color = new Color(255, 0, 0);
						}
						color *= 0.125f;
						sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, new Rectangle((int)(npc.position.X - Main.screenPosition.X), (int)(npc.position.Y - Main.screenPosition.Y), npc.width, npc.height), color);
						sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, new Rectangle((int)(npc.position.X - Main.screenPosition.X), (int)(npc.position.Y - Main.screenPosition.Y), npc.width, 1), color);
						sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, new Rectangle((int)(npc.position.X - Main.screenPosition.X), (int)(npc.position.Y - Main.screenPosition.Y), 1, npc.height), color);
						sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, new Rectangle((int)(npc.position.X - Main.screenPosition.X), (int)(npc.position.Y + npc.height - 1 - Main.screenPosition.Y), npc.width, 1), color);
						sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, new Rectangle((int)(npc.position.X + npc.width - 1 - Main.screenPosition.X), (int)(npc.position.Y - Main.screenPosition.Y), 1, npc.height), color);
					}
				}
			}

			// (debug) markers for statue items (performance will tank on world load)
			if (MetroidModPorted.DebugDSI)
			{
				if (!coordcheck)
				{
					for (int i = 0; i < Main.maxTilesX; i++)
					{
						for (int j = 0; j < Main.maxTilesY; j++)
						{
							if (!Main.tile[i, j].HasTile) { continue; }
							if (SuitAddonLoader.IsASuitTile(Main.tile[i, j]) /*|| BeamLoader.IsABeamTile(Main.tile[i, j])*/ || MBAddonLoader.IsAMorphTile(Main.tile[i, j]))
							{
								itemCoords.Add(new Vector2(i, j));
							}
						}
					}
					coordcheck = true;
				}
				for (int i = 0; i < itemCoords.Count; i++)
				{
					Tile tile = Main.tile[(int)itemCoords[i].X, (int)itemCoords[i].Y];
					if (tile != null && tile.HasTile)
					{
						Texture2D tex = Terraria.GameContent.TextureAssets.Tile[tile.TileType].Value;

						Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2;

						Vector2 pos = itemCoords[i] * 16f;
						float rot = (float)Math.Atan2(pos.Y - screenCenter.Y, pos.X - screenCenter.X);
						float dist = Math.Min(Vector2.Distance(pos, screenCenter), Main.screenHeight / 2 - 32);

						Vector2 drawPos = screenCenter + rot.ToRotationVector2() * dist - Main.screenPosition;
						sb.Draw(tex, drawPos, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), Color.White, 0, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 0f);
					}
				}
			}
			if (Main.netMode != NetmodeID.Server)
			{
				if (SuitAddonLoader.TryGetAddon(mp.VisorInUse, out ModSuitAddon addon))
				{
					addon.DrawVisor(P);
					if (addon.Type != oldVisorID && isVisorBGAudioPlaying)
					{
						VisorBGAudio.Sound.Stop(true);
						isVisorBGAudioPlaying = false;
					}
					if (addon.VisorBackgroundNoise != null && !isVisorBGAudioPlaying)
					{
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound((SoundStyle)addon.VisorBackgroundNoise), out VisorBGAudio);
						isVisorBGAudioPlaying = true;
					}
					oldVisorID = addon.Type;
				}
				else
				{
					if (isVisorBGAudioPlaying)
					{
						VisorBGAudio.Sound.Stop(true);
						isVisorBGAudioPlaying = false;
					}
				}
				//Filters.Scene.Activate("FilterName");

				// Updating a filter
				//Filters.Scene["FilterName"].GetShader().UseProgress(progress);

				//Filters.Scene["FilterName"].Deactivate();
			}
		}

		//int lastSeenScreenWidth;
		//int lastSeenScreenHeight;
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int TargetIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
			if (TargetIndex != -1)
			{
				layers.Insert(TargetIndex + 1, new LegacyGameInterfaceLayer(
					"MetroidMod: Seeker Targets",
					delegate {
						DrawSeekerTargets(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			int MapIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Map / Minimap"));
			if (MapIndex != -1)
			{
				layers.Insert(MapIndex + 1, new LegacyGameInterfaceLayer(
					"MetroidMod: Charge Meter",
					delegate {
						if (!Main.playerInventory && Main.npcChatText == "" && Main.player[Main.myPlayer].sign < 0 && !Main.ingameOptionsWindow)
						{
							DrawChargeBar(Main.spriteBatch);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(MapIndex + 1, new LegacyGameInterfaceLayer(
					"MetroidMod: Space Jump Meter",
					delegate {
						if (!Main.playerInventory && Main.npcChatText == "" && Main.player[Main.myPlayer].sign < 0 && !Main.ingameOptionsWindow)
						{
							DrawSpaceJumpBar(Main.spriteBatch);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			int ResourceIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (ResourceIndex != -1)
			{
				layers.Insert(ResourceIndex + 1, new LegacyGameInterfaceLayer(
					"MetroidMod: Reserve Tanks",
					delegate {
						DrawReserveHearts(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(ResourceIndex + 2, new LegacyGameInterfaceLayer(
					"MetroidMod: Suit Energy Bar",
					delegate {
						DrawEnergyBar(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			// Draws the Music Player UI.
			int index = layers.FindIndex((GameInterfaceLayer layer) => layer.Name.Equals("Vanilla: Mouse Text"));
			if (index != -1)
			{
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Power Beam UI",
					delegate {
						if (UI.PowerBeamUI.Visible)// && !Main.recBigList)
						{
							if (Main.hasFocus) { pbUserInterface.Recalculate(); }
							pbUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Missile Launcher UI",
					delegate {
						if (UI.MissileLauncherUI.Visible)// && !Main.recBigList)
						{
							if (Main.hasFocus) { miUserInterface.Recalculate(); }
							miUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Morph Ball UI",
					delegate {
						if (UI.MorphBallUI.Visible)
						{
							if (Main.hasFocus) { mbUserInterface.Recalculate(); }
							mbUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Suit Addons UI",
					delegate {
						if (UI.SuitAddonsUI.Visible)
						{
							if (Main.hasFocus) { suitUserInterface.Recalculate(); }
							suitUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Visor Select UI",
					delegate {
						if (UI.VisorSelectUI.Visible)
						{
							if (MSystem.VisorUIKey.JustPressed)
							{
								visorUserInterface.CurrentState.Activate();
							}
							visorUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
						if (MSystem.VisorUIKey.JustReleased)
						{
							visorUserInterface.CurrentState.Deactivate();
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
				/*layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Missile Launcher UI",
					delegate {
						if (UI.MissileLauncherUI.visible)
						{
							if (Main.hasFocus) { mlUserInterface.Recalculate(); }
							mlUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);*/
				/*layers.Insert(index, new LegacyGameInterfaceLayer(
					"MetroidModPorted: Missile Launcher UI",
					delegate {
						if (UI.MorphBallUI.visible)
						{
							if (Main.hasFocus) { mbUserInterface.Recalculate(); }
							mbUserInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);*/
			}
		}
		float tRot = 0f;
		float[] tScale = { 1f, 1f, 1f, 1f, 1f };
		public void DrawSeekerTargets(SpriteBatch sb)
		{
			Mod mod = Mod;
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			Item item = P.inventory[P.selectedItem];

			if (item.type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>())
			{
				tRot += 0.05f;
				MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
				if (mi.numSeekerTargets > 0)
				{
					for (int i = 0; i < mi.seekerTarget.Length; i++)
					{
						if (mi.seekerTarget[i] > -1)
						{
							int frame = 0;
							bool flag = true;
							for (int j = 0; j < mi.seekerTarget.Length; j++)
							{
								if (i != j)
								{
									if (mi.seekerTarget[i] == mi.seekerTarget[j])
									{
										flag = false;
										frame += 1;
										tScale[i] = Math.Min(0.5f, tScale[i]);
									}
								}
								else
								{
									flag = true;
								}
							}
							if (flag)
							{
								tScale[i] = Math.Max(tScale[i] - 0.1f, 0f);
								NPC npc = Main.npc[mi.seekerTarget[i]];
								Texture2D tTex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Targeting_retical").Value;
								Color color = new Color(255, 255, 255, 50);
								color *= (1f - tScale[i]) * 0.9f;
								int height = tTex.Height / 5;
								int yFrame = height * frame;

								Vector2 pos = Vector2.Transform(npc.Center - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix);
								pos /= Main.UIScale;

								sb.Draw(tTex, pos, new Rectangle?(new Rectangle(0, yFrame, tTex.Width, height)), color, tRot, new Vector2((float)tTex.Width / 2f, (float)height / 2f), npc.scale * 1.5f * (1f + tScale[i]), SpriteEffects.None, 0f);
							}
						}
						else
						{
							tScale[i] = 1f;
						}
					}
				}
				else
				{
					for (int i = 0; i < tScale.Length; i++)
					{
						tScale[i] = 1f;
					}
				}

				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].active && Main.npc[i].lifeMax > 5 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)// && !Main.npc[i].immortal)
					{
						bool flag = false;
						for (int j = 0; j < Main.maxProjectiles; j++)
						{
							/*if (Main.projectile[j].active && Main.projectile[j].type == mod.ProjectileType("VortexComboShot") && Main.projectile[j].owner == P.whoAmI && Main.projectile[j].ai[1] == i)
							{
								flag = true;
							}*/
						}
						if (flag)
						{
							NPC npc = Main.npc[i];
							Texture2D tTex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Targeting_retical_Vortex").Value;
							Color color = new Color(255, 255, 255, 10);

							Vector2 pos = Vector2.Transform(npc.Center - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix);
							pos /= Main.UIScale;

							sb.Draw(tTex, pos, new Rectangle?(new Rectangle(0, 0, tTex.Width, tTex.Height)), color, tRot, new Vector2((float)tTex.Width / 2f, (float)tTex.Height / 2f), npc.scale * 1.5f, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}
		public static int chStyle;
		public static int chR = 255;
		public static int chG = 0;
		public static int chB = 0;
		public void DrawChargeBar(SpriteBatch sb)
		{
			Mod mod = Mod;
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			Item item = P.inventory[P.selectedItem];
			if (P.whoAmI == Main.myPlayer && P.active && !P.dead && !P.ghost)
			{
				Texture2D texBar = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ChargeBar").Value,
					texBarBorder = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ChargeBarBorder").Value,
					texBarBorder2 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ChargeBarBorder2").Value;
				if (item.type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() || item.type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>() || mp.ballstate)
				{
					int ch = (int)mp.statCharge, chMax = (int)MPlayer.maxCharge;
					int pb = (int)mp.statPBCh, pbMax = (int)MPlayer.maxPBCh;
					float x = 22, y = 78 + z;
					int times = (int)Math.Ceiling(texBar.Height / 2f);
					float chpercent = chMax == 0 ? 0f : 1f * ch / chMax;
					float pbpercent = pbMax == 0 ? 0f : 1f * pb / pbMax;
					int w = (int)(Math.Floor(texBar.Width / 2f * chpercent) * 2);
					int w2 = (int)(Math.Floor(texBar.Width / 2f * pbpercent) * 2);

					//Color c = chpercent < 1f ? new Color(chR,chG,chB) : Color.Gold;
					Color c = chpercent < 1f ? MColor.HsvColor(300.0 - chpercent * 240, 0.5, 1.0) : Color.Gold;

					Color p = pbpercent < 1f ? Color.Crimson : Color.Gray;
					chStyle = chpercent <= 0f ? 0 : (chpercent <= .5f ? 1 : (chpercent <= .75f ? 2 : (chpercent <= .99f ? 3 : 0)));
					float offsetX = 2, offsetY = 2;
					sb.Draw(texBarBorder2, new Vector2(x, y), new Rectangle(0, 0, texBarBorder2.Width, texBarBorder2.Height), Color.White);
					if (pb > 0)
					{
						for (int i = 0; i < times; i++)
						{
							int ww = w2 - (i * 2);
							if (ww > 0)
							{
								sb.Draw(texBar, new Vector2(x + offsetX, y + offsetY + i * 2), new Rectangle(0, i * 2, ww, 2), p);
							}
						}
					}
					if (ch > 9)
					{
						for (int i = 0; i < times; i++)
						{
							int ww = w - (i * 2);
							if (ww > 0)
							{
								sb.Draw(texBar, new Vector2(x + offsetX, y + offsetY + i * 2), new Rectangle(0, i * 2, ww, 2), c);
							}
						}
					}
					if (mp.hyperColors > 0)
					{
						sb.Draw(texBar, new Vector2(x + offsetX, y + offsetY), new Rectangle(0, 0, texBar.Width, texBar.Height), new Color(mp.r, mp.g, mp.b));
					}
					sb.Draw(texBarBorder, new Vector2(x, y), new Rectangle(0, 0, texBarBorder.Width, texBarBorder.Height), Color.White);

					if (item.type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>())
					{
						MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
						int num = Math.Min(mi.statMissiles, mi.maxMissiles);
						string text = num.ToString("000");
						Vector2 vect = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(text);
						Color color = new Color((int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)));
						sb.DrawString(Terraria.GameContent.FontAssets.MouseText.Value, text, new Vector2(x + 38 - (vect.X / 2), y), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
					}
				}
				if (item.type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>() || mp.shineDirection != 0 || mp.shineActive)
				{
					Texture2D overheatBar = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/OverheatBar").Value,
					overheatBorder = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/OverheatBorder").Value;
					int ovh = (int)Math.Min(mp.statOverheat, mp.maxOverheat), ovhMax = (int)mp.maxOverheat;
					float x2 = 22, y2 = 120 + z;
					int times2 = (int)Math.Ceiling(overheatBar.Height / 2f);
					float ovhpercent = ovhMax == 0 ? 0f : 1f * ovh / ovhMax;
					int wo = (int)(Math.Floor(overheatBar.Width * ovhpercent));
					Color colorheat = new Color((int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor * 0.25f)), (int)((byte)((float)Main.mouseTextColor * 0.1f)), (int)((byte)((float)Main.mouseTextColor)));
					Color o = ovhpercent < 1f ? Color.Gold : colorheat;
					sb.Draw(overheatBorder, new Vector2(x2, y2), new Rectangle(0, 0, overheatBorder.Width, overheatBorder.Height), Color.White);
					if (ovh > 0)
					{
						for (int i = 0; i < times2; i++)
						{
							int ww = wo - (i * 2);
							if (ww > 0 && ovh <= ovhMax)
							{
								sb.Draw(overheatBar, new Vector2(x2 + 6, y2 + 2 + i * 2), new Rectangle(0, i * 2, ww, 2), o);
							}
						}
					}
					string text = (int)Math.Round((double)mp.statOverheat) + "/" + ovhMax;
					Vector2 vect = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(text);
					Color color = new Color((int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)));
					sb.DrawString(Terraria.GameContent.FontAssets.MouseText.Value, text, new Vector2(x2 + 2, y2 + overheatBorder.Height + 2), color, 0f, default(Vector2), 0.75f, SpriteEffects.None, 0f);
				}
				int num4 = (int)((float)30 % 255);
				if (chStyle == 1)
				{
					chG += num4;
					if (chG >= 255)
					{
						chG = 255;
						chStyle++;
					}
					chR -= num4;
					if (chR <= 0)
					{
						chR = 0;
					}
				}
				else if (chStyle == 2)
				{
					chB += num4;
					if (chB >= 255)
					{
						chB = 255;
						chStyle++;
					}
					chG -= num4;
					if (chG <= 196)
					{
						chG = 196;
					}
				}
				else if (chStyle == 3)
				{
					chR += num4;
					if (chR >= 255)
					{
						chR = 255;
						chStyle = 0;
					}
					chB -= num4;
					if (chB <= 0)
					{
						chB = 0;
					}
					if (chB <= 196)
					{
						chG -= num4;
						if (chG <= 0)
						{
							chG = 0;
						}
					}
				}
				else if (chStyle == 0 || mp.statCharge <= 0)
				{
					chR = 255;
					chG = 0;
					chB = 0;
				}
			}
		}

		public void DrawSpaceJumpBar(SpriteBatch sb)
		{
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if (mp.shineDirection == 0 && mp.spaceJump && mp.spaceJumped && P.velocity.Y != 0 && !mp.ballstate)
			{
				Texture2D texBar = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/SpaceJumpBar").Value, texBarBorder = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/SpaceJumpBarBorder").Value;
				if (P.whoAmI == Main.myPlayer && P.active && !P.dead && !P.ghost)
				{
					int sj = (int)mp.statSpaceJumps, sjMax = (int)MPlayer.maxSpaceJumps;
					float x = 160, y = 98 + z;
					int times = (int)Math.Ceiling(texBar.Height / 2f);
					float sjpercent = sjMax == 0 ? 0f : 1f * sj / sjMax;
					int w = (int)(Math.Floor(texBar.Width / 2f * sjpercent) * 2);
					Color s = sjpercent < 1f ? Color.Cyan : Color.SkyBlue;
					sb.Draw(texBarBorder, new Vector2(x, y), new Rectangle(0, 0, texBarBorder.Width, texBarBorder.Height), Color.White);
					sb.Draw(texBar, new Vector2(x + 2, y + 2), new Rectangle(0, 0, w, texBar.Height), s);
				}
			}
		}
		public void DrawReserveHearts(SpriteBatch sb)
		{
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if (mp.reserveTanks > 0)
			{
				Texture2D texHeart = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ReserveHeart").Value;
				if (P.whoAmI == Main.myPlayer && P.active && !P.dead && !P.ghost)
				{
					float lifePerHeart = 20f;
					int num = Main.player[Main.myPlayer].statLifeMax / 20;
					int num2 = (Main.player[Main.myPlayer].statLifeMax - 400) / 5;
					if (num2 < 0)
					{
						num2 = 0;
					}
					if (num2 > 0)
					{
						num = Main.player[Main.myPlayer].statLifeMax / (20 + num2 / 4);
						lifePerHeart = (float)Main.player[Main.myPlayer].statLifeMax / 20f;
					}
					int num3 = Main.player[Main.myPlayer].statLifeMax2 - Main.player[Main.myPlayer].statLifeMax;
					lifePerHeart += (float)(num3 / num);
					int num4 = (int)((float)Main.player[Main.myPlayer].statLifeMax2 / lifePerHeart);
					if (num4 >= 10)
					{
						num4 = 10;
					}
					for (int i = 1; i < mp.reserveHearts + 1; i++)
					{
						float num5 = 1f;
						bool flag = false;
						int num6;
						if ((float)Main.player[Main.myPlayer].statLife >= (float)i * lifePerHeart)
						{
							num6 = 255;
							if ((float)Main.player[Main.myPlayer].statLife == (float)i * lifePerHeart)
							{
								flag = true;
							}
						}
						else
						{
							float num7 = ((float)Main.player[Main.myPlayer].statLife - (float)(i - 1) * lifePerHeart) / lifePerHeart;
							num6 = (int)(30f + 225f * num7);
							if (num6 < 30)
							{
								num6 = 30;
							}
							num5 = num7 / 4f + 0.75f;
							if ((double)num5 < 0.75)
							{
								num5 = 0.75f;
							}
							if (num7 > 0f)
							{
								flag = true;
							}
						}
						if (flag)
						{
							num5 += Main.cursorScale - 1f;
						}
						int num8 = 0;
						int num9 = 0;
						if (i > 10)
						{
							num8 -= 260;
							num9 += 26;
						}
						int a = (int)((double)((float)num6) * 0.9);
						if (mp.reserveHeartsValue >= 25)
						{
							texHeart = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/ReserveHeart2").Value;
						}
						sb.Draw(texHeart, new Vector2((float)(500 + 26 * (i - 1) + num8 + (Main.screenWidth - 800) + Terraria.GameContent.TextureAssets.Heart.Value.Width / 2), 32f + ((float)Terraria.GameContent.TextureAssets.Heart.Value.Height - (float)Terraria.GameContent.TextureAssets.Heart.Value.Height * num5) / 2f + (float)num9 + (float)(Terraria.GameContent.TextureAssets.Heart.Value.Height / 2)), new Rectangle?(new Rectangle(0, 0, texHeart.Width, texHeart.Height)), new Color(num6, num6, num6, a), 0f, new Vector2((float)(texHeart.Width / 2), (float)(texHeart.Height / 2)), num5, SpriteEffects.None, 0f);
					}
				}
			}
		}
		public void DrawEnergyBar(SpriteBatch sb)
		{
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if (mp.ShouldShowArmorUI)
			{
				// number
				int num0 = (int)Math.Floor(mp.Energy / 10f);
				int num1 = num0 - (int)Math.Floor(num0 / 10f) * 10;
				int num2 = mp.Energy - (num0 * 10);
				Texture2D tex1 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/EnergyTextures/{num1}").Value;
				Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/EnergyTextures/{num2}").Value;
				Vector2 center = new(Main.screenWidth / 2, tex1.Height);
				center += new Vector2(0, 20);
				sb.Draw(tex1, center + new Vector2(-100 - tex1.Width * 2 - 16, - tex1.Height / 2), new Rectangle?(new Rectangle(0, 0, tex1.Width, tex1.Height)), Color.Aqua, 0f, new Vector2((float)(tex1.Width / 2), (float)(tex1.Height / 2)), 2f, SpriteEffects.None, 0f);
				sb.Draw(tex2, center + new Vector2(-100 - tex1.Width - 4, - tex1.Height / 2), new Rectangle?(new Rectangle(0, 0, tex2.Width, tex2.Height)), Color.Aqua, 0f, new Vector2((float)(tex2.Width / 2), (float)(tex2.Height / 2)), 2f, SpriteEffects.None, 0f);

				// bar
				Texture2D value = Terraria.GameContent.TextureAssets.MagicPixel.Value;
				Rectangle rectangle = Utils.CenteredRectangle(center, new Vector2(200f, 10f));
				Rectangle destinationRectangle = rectangle;
				Rectangle destinationRectangle2 = rectangle;
				destinationRectangle2.Width = (int)((float)destinationRectangle2.Width * ((mp.Energy - (Math.Floor(mp.Energy / 100f) * 100f)) / 99f));
				Rectangle value2 = new Rectangle(0, 0, 1, 1);
				sb.Draw(value, destinationRectangle, value2, Color.White * 0.6f);
				sb.Draw(value, rectangle, value2, Color.Black * 0.6f);
				sb.Draw(value, destinationRectangle2, value2, Color.Aqua * 0.5f);

				// boxes
				int totalBoxes = mp.EnergyTanks;
				int boxCount = mp.FilledEnergyTanks;
				Texture2D boxTex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/EnergyTextures/Box2").Value;
				for (int i = 0; i < totalBoxes; i++)
				{
					sb.Draw(boxTex, center + new Vector2(-100 + (tex1.Width * i) + 8 + (4 * i), - boxTex.Height / 2), new Rectangle?(new Rectangle(0, 0, boxTex.Width / 2, boxTex.Height / 2)), i < boxCount ? Color.Aqua : Color.DarkSlateGray, 0f, new Vector2((float)(tex1.Width / 2), (float)(tex1.Height / 2)), 1.5f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}
