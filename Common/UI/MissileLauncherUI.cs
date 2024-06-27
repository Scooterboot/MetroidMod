using System;
using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.MissileAddons;
using MetroidMod.Content.Items.MissileAddons.BeamCombos;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	public class MissileLauncherUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.chest == -1 && (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].type == ModContent.ItemType<MissileLauncher>() || Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].type == ModContent.ItemType<ArmCannon>() && Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].TryGetGlobalItem(out MGlobalItem ac) && !ac.isBeam);

		MissileLauncherPanel missileLauncherPanel;
		private MissileChangeButton mcButton;

		public override void OnInitialize()
		{
			missileLauncherPanel = new MissileLauncherPanel();
			missileLauncherPanel.Initialize();

			Append(missileLauncherPanel);
			mcButton = new MissileChangeButton();
			mcButton.Initialize();
			Append(mcButton);
		}
	}

	public class MissileLauncherPanel : DragableUIPanel
	{
		private Texture2D panelTexture;

		public MissileLauncherItemBox[] missileSlots;

		//public UIText[] textSlots;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.missileSlotAmount]
		{
			new Vector2(106, 94),
			new Vector2(38, 42),
			new Vector2(178, 42)
		};

		public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/MissileLauncher_Border", AssetRequestMode.ImmediateLoad).Value;

			this.SetPadding(0);
			this.Left.Pixels = 160;
			this.Top.Pixels = 260;
			this.Width.Pixels = panelTexture.Width;
			this.Height.Pixels = panelTexture.Height;

			this.Append(new MissileLauncherFrame());
			this.Append(new MissileLauncherLines());

			missileSlots = new MissileLauncherItemBox[MetroidMod.missileSlotAmount];
			//textSlots = new UIText[MetroidMod.missileSlotAmount];
			for (int i = 0; i < MetroidMod.missileSlotAmount; ++i)
			{
				missileSlots[i] = new MissileLauncherItemBox();
				missileSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				missileSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				missileSlots[i].missileSlotType = i;
				missileSlots[i].SetCondition();

				this.Append(missileSlots[i]);

				/*
				textSlots[i] = new UIText("0", Main.screenHeight / 1080f);
				textSlots[i].SetText(MissileLauncherLoader.GetAddonSlotName(i));
				textSlots[i].Top.Pixels = itemBoxPositionValues[i].Y + 44;
				textSlots[i].Left.Pixels = itemBoxPositionValues[i].X - 22;
				textSlots[i].IsWrapped = true;
				textSlots[i].Width.Pixels = 88;
				textSlots[i].Height.Pixels = 22;

				Append(textSlots[i]);
				*/
			}
		}

		public override void Update(GameTime gameTime)
		{
			enabled = MConfigClient.Instance.MissileLauncher.enabled;
			if (!enabled && MConfigClient.Instance.MissileLauncher.auto)
			{
				this.Left.Pixels = 160;
				this.Top.Pixels = 260;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					this.Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture, DrawRectangle, Color.White);
		}
	}

	public class MissileLauncherItemBox : UIPanel
	{
		private Texture2D itemBoxTexture;

		public Condition condition;

		public int changeSlotType;
		public int missileSlotType;

		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			itemBoxTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = itemBoxTexture.Width; Height.Pixels = itemBoxTexture.Height;
			this.OnLeftClick += ItemBoxClick;
		}

		public override void Update(GameTime gameTime)
		{
			// Ignore mouse input.
			if (base.IsMouseHovering)
				Main.LocalPlayer.mouseInterface = true;
		}

		public void SetCondition()
		{
			this.condition = delegate (Item addonItem) {
				//Mod mod = MetroidMod.Instance;
				if (addonItem.ModItem != null)// && addonItem.ModItem.Mod == mod)
				{
					MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					return addonItem.type <= ItemID.None || mItem.missileSlotType == missileSlotType;
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == mod);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented? also ugly --Dr
			if (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem.Type == ModContent.ItemType<MissileLauncher>())
			{
				if (Main.LocalPlayer.controlUseItem || Main.LocalPlayer.controlUseTile) { return; }

				MissileLauncher missileLauncherTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as MissileLauncher;
				if (missileLauncherTarget == null || missileLauncherTarget.MissileMods == null) { return; }

				if (missileLauncherTarget.MissileMods[missileSlotType] != null && !missileLauncherTarget.MissileMods[missileSlotType].IsAir)
				{
					if (Main.mouseItem.IsAir)
					{
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<HomingMissileAddon>())
						{
							missileLauncherTarget.MissileChange[0].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<SpazerComboAddon>())
						{
							missileLauncherTarget.MissileChange[1].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<SeekerMissileAddon>())
						{
							missileLauncherTarget.MissileChange[2].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<WavebusterAddon>())
						{
							missileLauncherTarget.MissileChange[3].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<IceSpreaderAddon>())
						{
							missileLauncherTarget.MissileChange[4].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<FlamethrowerAddon>())
						{
							missileLauncherTarget.MissileChange[5].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<PlasmaMachinegunAddon>())
						{
							missileLauncherTarget.MissileChange[6].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<DiffusionMissileAddon>())
						{
							missileLauncherTarget.MissileChange[7].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<NovaComboAddon>())
						{
							missileLauncherTarget.MissileChange[8].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<VortexComboAddon>())
						{
							missileLauncherTarget.MissileChange[9].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<StardustComboAddon>())
						{
							missileLauncherTarget.MissileChange[10].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<NebulaComboAddon>())
						{
							missileLauncherTarget.MissileChange[11].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<SolarComboAddon>())
						{
							missileLauncherTarget.MissileChange[12].TurnToAir();
						}
						SoundEngine.PlaySound(SoundID.Grab);
						Main.mouseItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();

						missileLauncherTarget.MissileMods[missileSlotType].TurnToAir();
					}
					else if (condition == null || (condition != null && condition(Main.mouseItem)))
					{
						SoundEngine.PlaySound(SoundID.Grab);

						if (Main.mouseItem.type == missileLauncherTarget.MissileMods[missileSlotType].type)
						{
							int stack = Main.mouseItem.stack + missileLauncherTarget.MissileMods[missileSlotType].stack;

							if (missileLauncherTarget.MissileMods[missileSlotType].maxStack >= stack)
							{
								missileLauncherTarget.MissileMods[missileSlotType].stack = stack;
								Main.mouseItem.TurnToAir();
							}
							else
							{
								int stackDiff = stack - missileLauncherTarget.MissileMods[missileSlotType].maxStack;
								missileLauncherTarget.MissileMods[missileSlotType].stack = missileLauncherTarget.MissileMods[missileSlotType].maxStack;
								Main.mouseItem.stack = stackDiff;
							}
						}
						else
						{
							Item tempBoxItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();
							Item tempMouseItem = Main.mouseItem.Clone();

							missileLauncherTarget.MissileMods[missileSlotType] = tempMouseItem;
							Main.mouseItem = tempBoxItem;
						}
					}
				}
				else if (!Main.mouseItem.IsAir)
				{
					if (condition == null || (condition != null && condition(Main.mouseItem)))
					{
						if (Main.mouseItem.type == ModContent.ItemType<HomingMissileAddon>())
						{
							missileLauncherTarget.MissileChange[0] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<SpazerComboAddon>())
						{
							missileLauncherTarget.MissileChange[1] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<SeekerMissileAddon>())
						{
							missileLauncherTarget.MissileChange[2] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<WavebusterAddon>())
						{
							missileLauncherTarget.MissileChange[3] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<IceSpreaderAddon>())
						{
							missileLauncherTarget.MissileChange[4] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<FlamethrowerAddon>())
						{
							missileLauncherTarget.MissileChange[5] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<PlasmaMachinegunAddon>())
						{
							missileLauncherTarget.MissileChange[6] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<DiffusionMissileAddon>())
						{
							missileLauncherTarget.MissileChange[7] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<NovaComboAddon>())
						{
							missileLauncherTarget.MissileChange[8] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<VortexComboAddon>())
						{
							missileLauncherTarget.MissileChange[9] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<StardustComboAddon>())
						{
							missileLauncherTarget.MissileChange[10] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<NebulaComboAddon>())
						{
							missileLauncherTarget.MissileChange[11] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<SolarComboAddon>())
						{
							missileLauncherTarget.MissileChange[12] = Main.mouseItem.Clone();
						}
						SoundEngine.PlaySound(SoundID.Grab);
						missileLauncherTarget.MissileMods[missileSlotType] = Main.mouseItem.Clone();
						Main.mouseItem.TurnToAir();
					}
				}
			}
			else if (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem.Type == ModContent.ItemType<ArmCannon>())
			{
				if (Main.LocalPlayer.controlUseItem || Main.LocalPlayer.controlUseTile) { return; }

				ArmCannon missileLauncherTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as ArmCannon;
				if (missileLauncherTarget == null || missileLauncherTarget.MissileMods == null) { return; }

				if (missileLauncherTarget.MissileMods[missileSlotType] != null && !missileLauncherTarget.MissileMods[missileSlotType].IsAir)
				{
					if (Main.mouseItem.IsAir)
					{
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<HomingMissileAddon>())
						{
							missileLauncherTarget.MissileChange[0].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<SpazerComboAddon>())
						{
							missileLauncherTarget.MissileChange[1].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<SeekerMissileAddon>())
						{
							missileLauncherTarget.MissileChange[2].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<WavebusterAddon>())
						{
							missileLauncherTarget.MissileChange[3].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<IceSpreaderAddon>())
						{
							missileLauncherTarget.MissileChange[4].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<FlamethrowerAddon>())
						{
							missileLauncherTarget.MissileChange[5].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<PlasmaMachinegunAddon>())
						{
							missileLauncherTarget.MissileChange[6].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<DiffusionMissileAddon>())
						{
							missileLauncherTarget.MissileChange[7].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<NovaComboAddon>())
						{
							missileLauncherTarget.MissileChange[8].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<VortexComboAddon>())
						{
							missileLauncherTarget.MissileChange[9].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<StardustComboAddon>())
						{
							missileLauncherTarget.MissileChange[10].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<NebulaComboAddon>())
						{
							missileLauncherTarget.MissileChange[11].TurnToAir();
						}
						if (missileLauncherTarget.MissileMods[missileSlotType].type == ModContent.ItemType<SolarComboAddon>())
						{
							missileLauncherTarget.MissileChange[12].TurnToAir();
						}
						SoundEngine.PlaySound(SoundID.Grab);
						Main.mouseItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();

						missileLauncherTarget.MissileMods[missileSlotType].TurnToAir();
					}
					else if (condition == null || (condition != null && condition(Main.mouseItem)))
					{
						SoundEngine.PlaySound(SoundID.Grab);

						if (Main.mouseItem.type == missileLauncherTarget.MissileMods[missileSlotType].type)
						{
							int stack = Main.mouseItem.stack + missileLauncherTarget.MissileMods[missileSlotType].stack;

							if (missileLauncherTarget.MissileMods[missileSlotType].maxStack >= stack)
							{
								missileLauncherTarget.MissileMods[missileSlotType].stack = stack;
								Main.mouseItem.TurnToAir();
							}
							else
							{
								int stackDiff = stack - missileLauncherTarget.MissileMods[missileSlotType].maxStack;
								missileLauncherTarget.MissileMods[missileSlotType].stack = missileLauncherTarget.MissileMods[missileSlotType].maxStack;
								Main.mouseItem.stack = stackDiff;
							}
						}
						else
						{
							Item tempBoxItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();
							Item tempMouseItem = Main.mouseItem.Clone();

							missileLauncherTarget.MissileMods[missileSlotType] = tempMouseItem;
							Main.mouseItem = tempBoxItem;
						}
					}
				}
				else if (!Main.mouseItem.IsAir)
				{
					if (condition == null || (condition != null && condition(Main.mouseItem)))
					{
						if (Main.mouseItem.type == ModContent.ItemType<HomingMissileAddon>())
						{
							missileLauncherTarget.MissileChange[0] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<SpazerComboAddon>())
						{
							missileLauncherTarget.MissileChange[1] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<SeekerMissileAddon>())
						{
							missileLauncherTarget.MissileChange[2] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<WavebusterAddon>())
						{
							missileLauncherTarget.MissileChange[3] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<IceSpreaderAddon>())
						{
							missileLauncherTarget.MissileChange[4] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<FlamethrowerAddon>())
						{
							missileLauncherTarget.MissileChange[5] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<PlasmaMachinegunAddon>())
						{
							missileLauncherTarget.MissileChange[6] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<DiffusionMissileAddon>())
						{
							missileLauncherTarget.MissileChange[7] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<NovaComboAddon>())
						{
							missileLauncherTarget.MissileChange[8] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<VortexComboAddon>())
						{
							missileLauncherTarget.MissileChange[9] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<StardustComboAddon>())
						{
							missileLauncherTarget.MissileChange[10] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<NebulaComboAddon>())
						{
							missileLauncherTarget.MissileChange[11] = Main.mouseItem.Clone();
						}
						if (Main.mouseItem.type == ModContent.ItemType<SolarComboAddon>())
						{
							missileLauncherTarget.MissileChange[12] = Main.mouseItem.Clone();
						}
						SoundEngine.PlaySound(SoundID.Grab);
						missileLauncherTarget.MissileMods[missileSlotType] = Main.mouseItem.Clone();
						Main.mouseItem.TurnToAir();
					}
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			if (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem.Type == ModContent.ItemType<MissileLauncher>())
			{
				MissileLauncher missileLauncherTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as MissileLauncher;

				spriteBatch.Draw(itemBoxTexture, DrawRectangle, Color.White);

				// Item drawing.
				if (missileLauncherTarget.MissileMods[missileSlotType].IsAir) return;

				Color itemColor = missileLauncherTarget.MissileMods[missileSlotType].GetAlpha(Color.White);
				Texture2D itemTexture = TextureAssets.Item[missileLauncherTarget.MissileMods[missileSlotType].type].Value;
				CalculatedStyle innerDimensions = base.GetDimensions();

				if (base.IsMouseHovering)
				{
					Main.hoverItemName = missileLauncherTarget.MissileMods[missileSlotType].Name;
					Main.HoverItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();
				}

				var frame = Main.itemAnimations[missileLauncherTarget.MissileMods[missileSlotType].type] != null
							? Main.itemAnimations[missileLauncherTarget.MissileMods[missileSlotType].type].GetFrame(itemTexture)
							: itemTexture.Frame(1, 1, 0, 0);

				float drawScale = 1f;
				if ((float)frame.Width > innerDimensions.Width || (float)frame.Height > innerDimensions.Width)
				{
					if (frame.Width > frame.Height)
						drawScale = innerDimensions.Width / (float)frame.Width;
					else
						drawScale = innerDimensions.Width / (float)frame.Height;
				}

				var unreflectedScale = drawScale;
				var tmpcolor = Color.White;

				ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, missileLauncherTarget.MissileMods[missileSlotType].type);

				Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

				drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
				drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);

				if (missileLauncherTarget.MissileMods[missileSlotType].stack > 1)
				{
					Utils.DrawBorderStringFourWay(
						spriteBatch,
						FontAssets.ItemStack.Value,
						Math.Min(9999, missileLauncherTarget.MissileMods[missileSlotType].stack).ToString(),
						innerDimensions.Position().X + 10f,
						innerDimensions.Position().Y + 26f,
						Color.White,
						Color.Black,
						Vector2.Zero,
						unreflectedScale * 0.8f);
				}
			}
			else if (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem.Type == ModContent.ItemType<ArmCannon>())
			{
				ArmCannon missileLauncherTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as ArmCannon;

				spriteBatch.Draw(itemBoxTexture, DrawRectangle, Color.White);

				// Item drawing.
				if (missileLauncherTarget.MissileMods[missileSlotType].IsAir) return;

				Color itemColor = missileLauncherTarget.MissileMods[missileSlotType].GetAlpha(Color.White);
				Texture2D itemTexture = TextureAssets.Item[missileLauncherTarget.MissileMods[missileSlotType].type].Value;
				CalculatedStyle innerDimensions = base.GetDimensions();

				if (base.IsMouseHovering)
				{
					Main.hoverItemName = missileLauncherTarget.MissileMods[missileSlotType].Name;
					Main.HoverItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();
				}

				var frame = Main.itemAnimations[missileLauncherTarget.MissileMods[missileSlotType].type] != null
							? Main.itemAnimations[missileLauncherTarget.MissileMods[missileSlotType].type].GetFrame(itemTexture)
							: itemTexture.Frame(1, 1, 0, 0);

				float drawScale = 1f;
				if ((float)frame.Width > innerDimensions.Width || (float)frame.Height > innerDimensions.Width)
				{
					if (frame.Width > frame.Height)
						drawScale = innerDimensions.Width / (float)frame.Width;
					else
						drawScale = innerDimensions.Width / (float)frame.Height;
				}

				var unreflectedScale = drawScale;
				var tmpcolor = Color.White;

				ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, missileLauncherTarget.MissileMods[missileSlotType].type);

				Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

				drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
				drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);

				if (missileLauncherTarget.MissileMods[missileSlotType].stack > 1)
				{
					Utils.DrawBorderStringFourWay(
						spriteBatch,
						FontAssets.ItemStack.Value,
						Math.Min(9999, missileLauncherTarget.MissileMods[missileSlotType].stack).ToString(),
						innerDimensions.Position().X + 10f,
						innerDimensions.Position().Y + 26f,
						Color.White,
						Color.Black,
						Vector2.Zero,
						unreflectedScale * 0.8f);
				}
			}
		}
	}

	/*
	 * The classes in the following section do not have any functionality besides visual aesthetics.
	 */
	public class MissileLauncherFrame : UIPanel
	{
		private Texture2D missileLauncherFrame;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public override void OnInitialize()
		{
			missileLauncherFrame = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/MissileLauncher_Frame", AssetRequestMode.ImmediateLoad).Value;

			this.Width.Pixels = missileLauncherFrame.Width;
			this.Height.Pixels = missileLauncherFrame.Height;

			// Hardcoded position values.
			this.Top.Pixels = 52;
			this.Left.Pixels = 118;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(missileLauncherFrame, drawRectangle, Color.White);
		}
	}
	public class MissileLauncherLines : UIPanel
	{
		private Texture2D missileLauncherLines;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public override void OnInitialize()
		{
			missileLauncherLines = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/MissileLauncher_Lines", AssetRequestMode.ImmediateLoad).Value;

			this.Width.Pixels = missileLauncherLines.Width;
			this.Height.Pixels = missileLauncherLines.Height;

			// Hardcoded position values.
			this.Top.Pixels = 0;
			this.Left.Pixels = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(missileLauncherLines, drawRectangle, Color.White);
		}
	}
	public class MissileChangeButton : DragableUIPanel
	{
		private Texture2D buttonTex, buttonTex_Hover, buttonTex_Click,
		buttonTexEnabled, buttonTexEnabled_Hover, buttonTexEnabled_Click;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			Left.Pixels = 112;
			Top.Pixels = 374; //274

			buttonTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceOff", AssetRequestMode.ImmediateLoad).Value;
			buttonTex_Hover = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceHover", AssetRequestMode.ImmediateLoad).Value;
			buttonTexEnabled = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceOn", AssetRequestMode.ImmediateLoad).Value;
			buttonTexEnabled_Hover = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceHover", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = 44; //buttonTex.Width
			Height.Pixels = 44;
			OnLeftClick += BCButtonClick;
		}

		public override void Update(GameTime gameTime)
		{
			if (IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			enabled = MConfigClient.Instance.MissileLauncher.enabled;
			if (!enabled)
			{
				Left.Pixels = 112; //112
				Top.Pixels = 374;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}

		private bool clicked = false;
		private void BCButtonClick(UIMouseEvent evt, UIElement e)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

			mp.missileChangeActive = !mp.missileChangeActive;
			//SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
			clicked = true;
			if (mp.missileChangeActive)
			{
				SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelect);
			}
			if (!mp.missileChangeActive)
			{
				SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
			}
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

			Texture2D tex = buttonTex, texH = buttonTex_Hover, texC = buttonTex_Click;
			if (mp.missileChangeActive)
			{
				tex = buttonTexEnabled;
				texH = buttonTexEnabled_Hover;
				texC = buttonTexEnabled_Click;
			}

			if (IsMouseHovering)
			{
				tex = texH;
				if (clicked)
				{
					tex = texC;
					clicked = false;
				}

				string psText = "Missile Interface: Disabled";
				if (mp.missileChangeActive)
				{
					psText = "Missile Interface: Enabled";
				}
				Main.hoverItemName = psText;
			}

			sb.Draw(tex, DrawRectangle, Color.White);
		}
	}
}
