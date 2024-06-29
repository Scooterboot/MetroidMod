using System;
using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Content.Items.Addons.Hunters;
using MetroidMod.Content.Items.Addons.V2;
using MetroidMod.Content.Items.Addons.V3;
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
	/*
	 * The whole UI still feels a tad hacky, so it might need to get a bit of revamping here and there.
	 */
	public class PowerBeamUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.chest == -1 && (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].type == ModContent.ItemType<PowerBeam>() || Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].type == ModContent.ItemType<ArmCannon>() && Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].TryGetGlobalItem(out MGlobalItem ac) && ac.isBeam);

		private PowerBeamPanel powerBeamPanel;
		private PowerBeamScrewAttackButton pbsaButton;
		private PowerBeamChangeButton bcButton;
		private ComboError comboError;
		public override void OnInitialize()
		{
			base.OnInitialize();
			powerBeamPanel = new PowerBeamPanel();
			powerBeamPanel.Initialize();
			Append(powerBeamPanel);

			pbsaButton = new PowerBeamScrewAttackButton();
			pbsaButton.Initialize();
			Append(pbsaButton);

			comboError = new ComboError();
			comboError.Initialize();
			Append(comboError);

			bcButton = new PowerBeamChangeButton();
			bcButton.Initialize();
			Append(bcButton);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Main.hidePlayerCraftingMenu = true;
			/*powerBeamPanel.Left.Pixels = 160;
			powerBeamPanel.Top.Pixels = Main.instance.invBottom + 10;
			powerBeamPanel.Width.Pixels = 256;
			powerBeamPanel.Height.Pixels = 164;
			if (Main.LocalPlayer.chest != -1)// || Main.npcShop != 0)
			{
				powerBeamPanel.Top.Pixels += 170;
			}*/
			powerBeamPanel.Recalculate();
		}
	}

	public class PowerBeamPanel : DragableUIPanel
	{
		internal Texture2D panelTexture;

		public PowerBeamItemBox[] beamSlots;

		//public UIText[] textSlots;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.beamSlotAmount]
		{
			new Vector2(98, 14),
			new Vector2(174, 14),
			new Vector2(32, 14),
			new Vector2(32, 94),
			new Vector2(174, 94),
			new Vector2(98, 152)
		};

		public override void OnInitialize()
		{
			SetPadding(0);
			Top.Pixels = Main.instance.invBottom + 10;
			Left.Pixels = 65;
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerBeam_Border", AssetRequestMode.ImmediateLoad).Value;

			Append(new PowerBeamFrame());
			Append(new PowerBeamLines());

			beamSlots = new PowerBeamItemBox[MetroidMod.beamSlotAmount];
			for (int i = 0; i < MetroidMod.beamSlotAmount; ++i)
			{
				beamSlots[i] = new PowerBeamItemBox();
				beamSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				beamSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				beamSlots[i].addonSlotType = i;
				beamSlots[i].SetCondition();

				Append(beamSlots[i]);
			}
		}

		public override void Update(GameTime gameTime)
		{
			Width.Pixels = 256;
			Height.Pixels = 224;
			enabled = MConfigClient.Instance.PowerBeam.enabled;
			if (!enabled && MConfigClient.Instance.PowerBeam.auto)
			{
				Left.Pixels = 160;
				Top.Pixels = Main.instance.invBottom + 10;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture, DrawRectangle, Color.White);
		}
	}

	public class PowerBeamItemBox : UIPanel
	{
		private Texture2D itemBoxTexture;

		public Condition condition;
		public Condition condition2;

		public int addonSlotType;
		public int beamSlotType;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			itemBoxTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = 44; Height.Pixels = 44;
			OnLeftClick += ItemBoxClick;
		}

		public override void Update(GameTime gameTime)
		{
			// Ignore mouse input.
			if (IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		public void SetCondition()
		{
			condition = delegate (Item addonItem) {
				//Mod mod = ModLoader.GetMod("MetroidMod");
				if (addonItem.ModItem != null)// && addonItem.ModItem.Mod == MetroidMod.Instance)
				{
					MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					//if (addonItem.GetGlobalItem<MGlobalItem>().AddonType != AddonType.PowerBeam) { return false; }
					//ModBeam mBeam = ((BeamItem)addonItem.ModItem).modBeam;
					return addonItem.type <= ItemID.None || mItem.addonSlotType == addonSlotType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidMod.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			//TODO No failsafe. Should maybe be implemented?
			// How do I get BeamChange[beamSlotType] to not always equal 0 so it isnt this disguting trainwreck? --Dr
			if(Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem.Type == ModContent.ItemType<PowerBeam>())
			{
				PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as PowerBeam;
				if (powerBeamTarget == null || powerBeamTarget.BeamMods == null) { return; }

				if (powerBeamTarget.BeamMods[addonSlotType] != null && !powerBeamTarget.BeamMods[addonSlotType].IsAir)
				{
					//pickup
					if (Main.mouseItem.IsAir)
					{
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ChargeBeamAddon>())
						{
							powerBeamTarget.BeamChange[0].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<VoltDriverAddon>())
						{
							powerBeamTarget.BeamChange[1].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<MagMaulAddon>())
						{
							powerBeamTarget.BeamChange[2].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ImperialistAddon>())
						{
							powerBeamTarget.BeamChange[3].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<JudicatorAddon>())
						{
							powerBeamTarget.BeamChange[4].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ShockCoilAddon>())
						{
							powerBeamTarget.BeamChange[5].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<BattleHammerAddon>())
						{
							powerBeamTarget.BeamChange[6].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<OmegaCannonAddon>())
						{
							powerBeamTarget.BeamChange[7].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<HyperBeamAddon>())
						{
							powerBeamTarget.BeamChange[8].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<PhazonBeamAddon>())
						{
							powerBeamTarget.BeamChange[9].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ChargeBeamV2Addon>())
						{
							powerBeamTarget.BeamChange[10].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<LuminiteBeamAddon>())
						{
							powerBeamTarget.BeamChange[11].TurnToAir();
						}
						Main.mouseItem = powerBeamTarget.BeamMods[addonSlotType].Clone();

						powerBeamTarget.BeamMods[addonSlotType].TurnToAir();
						//powerBeamTarget.BeamChange[beamSlotType].TurnToAir();
					}
					else if (condition == null || (condition != null && condition(Main.mouseItem)) && addonSlotType != 0)
					{
						SoundEngine.PlaySound(SoundID.Grab);
						if (Main.mouseItem.type == powerBeamTarget.BeamMods[addonSlotType].type)
						{
							int stack = Main.mouseItem.stack + powerBeamTarget.BeamMods[addonSlotType].stack;

							if (powerBeamTarget.BeamMods[addonSlotType].maxStack >= stack)
							{
								powerBeamTarget.BeamMods[addonSlotType].stack = stack;
								Main.mouseItem.TurnToAir();
							}
							else
							{
								int stackDiff = stack - powerBeamTarget.BeamMods[addonSlotType].maxStack;
								powerBeamTarget.BeamMods[addonSlotType].stack = powerBeamTarget.BeamMods[addonSlotType].maxStack;
								Main.mouseItem.stack = stackDiff;
							}
						}
						else
						{
							Item tempBoxItem = powerBeamTarget.BeamMods[addonSlotType].Clone();
							Item tempMouseItem = Main.mouseItem.Clone();

							powerBeamTarget.BeamMods[addonSlotType] = tempMouseItem;
							Main.mouseItem = tempBoxItem;
						}
					}
				}
				//place
				else if (!Main.mouseItem.IsAir)
				{
					if (condition == null || (condition != null && condition(Main.mouseItem)))
					{
						if (Main.mouseItem.type == ModContent.ItemType<ChargeBeamAddon>())
						{
							powerBeamTarget.BeamChange[0] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<VoltDriverAddon>())
						{
							powerBeamTarget.BeamChange[1] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<MagMaulAddon>())
						{
							powerBeamTarget.BeamChange[2] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<ImperialistAddon>())
						{
							powerBeamTarget.BeamChange[3] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ImperialistLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<JudicatorAddon>())
						{
							powerBeamTarget.BeamChange[4] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<ShockCoilAddon>())
						{
							powerBeamTarget.BeamChange[5] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<BattleHammerAddon>())
						{
							powerBeamTarget.BeamChange[6] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.BattleHammerLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<OmegaCannonAddon>())
						{
							powerBeamTarget.BeamChange[7] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.OmegaCannonLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<HyperBeamAddon>())
						{
							powerBeamTarget.BeamChange[8] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamAquired);
						}
						if (Main.mouseItem.type == ModContent.ItemType<PhazonBeamAddon>())
						{
							powerBeamTarget.BeamChange[9] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamAquired);
						}
						if (Main.mouseItem.type == ModContent.ItemType<ChargeBeamV2Addon>())
						{
							powerBeamTarget.BeamChange[10] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<LuminiteBeamAddon>())
						{
							powerBeamTarget.BeamChange[11] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
						}
						SoundEngine.PlaySound(SoundID.Grab);
						powerBeamTarget.BeamMods[addonSlotType] = Main.mouseItem.Clone();
						Main.mouseItem.TurnToAir();
					}
				}
			}
			else if(Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem.Type == ModContent.ItemType<ArmCannon>())
			{
				ArmCannon powerBeamTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as ArmCannon;
				if (powerBeamTarget == null || powerBeamTarget.BeamMods == null) { return; }

				if (powerBeamTarget.BeamMods[addonSlotType] != null && !powerBeamTarget.BeamMods[addonSlotType].IsAir)
				{
					//pickup
					if (Main.mouseItem.IsAir)
					{
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ChargeBeamAddon>())
						{
							powerBeamTarget.BeamChange[0].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<VoltDriverAddon>())
						{
							powerBeamTarget.BeamChange[1].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<MagMaulAddon>())
						{
							powerBeamTarget.BeamChange[2].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ImperialistAddon>())
						{
							powerBeamTarget.BeamChange[3].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<JudicatorAddon>())
						{
							powerBeamTarget.BeamChange[4].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ShockCoilAddon>())
						{
							powerBeamTarget.BeamChange[5].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<BattleHammerAddon>())
						{
							powerBeamTarget.BeamChange[6].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<OmegaCannonAddon>())
						{
							powerBeamTarget.BeamChange[7].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<HyperBeamAddon>())
						{
							powerBeamTarget.BeamChange[8].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<PhazonBeamAddon>())
						{
							powerBeamTarget.BeamChange[9].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<ChargeBeamV2Addon>())
						{
							powerBeamTarget.BeamChange[10].TurnToAir();
						}
						if (powerBeamTarget.BeamMods[addonSlotType].type == ModContent.ItemType<LuminiteBeamAddon>())
						{
							powerBeamTarget.BeamChange[11].TurnToAir();
						}
						Main.mouseItem = powerBeamTarget.BeamMods[addonSlotType].Clone();

						powerBeamTarget.BeamMods[addonSlotType].TurnToAir();
						//powerBeamTarget.BeamChange[beamSlotType].TurnToAir();
					}
					else if (condition == null || (condition != null && condition(Main.mouseItem)) && addonSlotType != 0)
					{
						SoundEngine.PlaySound(SoundID.Grab);
						if (Main.mouseItem.type == powerBeamTarget.BeamMods[addonSlotType].type)
						{
							int stack = Main.mouseItem.stack + powerBeamTarget.BeamMods[addonSlotType].stack;

							if (powerBeamTarget.BeamMods[addonSlotType].maxStack >= stack)
							{
								powerBeamTarget.BeamMods[addonSlotType].stack = stack;
								Main.mouseItem.TurnToAir();
							}
							else
							{
								int stackDiff = stack - powerBeamTarget.BeamMods[addonSlotType].maxStack;
								powerBeamTarget.BeamMods[addonSlotType].stack = powerBeamTarget.BeamMods[addonSlotType].maxStack;
								Main.mouseItem.stack = stackDiff;
							}
						}
						else
						{
							Item tempBoxItem = powerBeamTarget.BeamMods[addonSlotType].Clone();
							Item tempMouseItem = Main.mouseItem.Clone();

							powerBeamTarget.BeamMods[addonSlotType] = tempMouseItem;
							Main.mouseItem = tempBoxItem;
						}
					}
				}
				//place
				else if (!Main.mouseItem.IsAir)
				{
					if (condition == null || (condition != null && condition(Main.mouseItem)))
					{
						if (Main.mouseItem.type == ModContent.ItemType<ChargeBeamAddon>())
						{
							powerBeamTarget.BeamChange[0] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<VoltDriverAddon>())
						{
							powerBeamTarget.BeamChange[1] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<MagMaulAddon>())
						{
							powerBeamTarget.BeamChange[2] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<ImperialistAddon>())
						{
							powerBeamTarget.BeamChange[3] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ImperialistLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<JudicatorAddon>())
						{
							powerBeamTarget.BeamChange[4] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<ShockCoilAddon>())
						{
							powerBeamTarget.BeamChange[5] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<BattleHammerAddon>())
						{
							powerBeamTarget.BeamChange[6] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.BattleHammerLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<OmegaCannonAddon>())
						{
							powerBeamTarget.BeamChange[7] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.OmegaCannonLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<HyperBeamAddon>())
						{
							powerBeamTarget.BeamChange[8] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamAquired);
						}
						if (Main.mouseItem.type == ModContent.ItemType<PhazonBeamAddon>())
						{
							powerBeamTarget.BeamChange[9] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.BeamAquired);
						}
						if (Main.mouseItem.type == ModContent.ItemType<ChargeBeamV2Addon>())
						{
							powerBeamTarget.BeamChange[10] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
						}
						if (Main.mouseItem.type == ModContent.ItemType<LuminiteBeamAddon>())
						{
							powerBeamTarget.BeamChange[11] = Main.mouseItem.Clone();
							SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
						}
						SoundEngine.PlaySound(SoundID.Grab);
						powerBeamTarget.BeamMods[addonSlotType] = Main.mouseItem.Clone();
						Main.mouseItem.TurnToAir();
					}
				}
			}
			
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			Item target = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem];
			if (target == null || (target.type != ModContent.ItemType<PowerBeam>() && target.type != ModContent.ItemType<ArmCannon>())) { return; }


			if (target.type == ModContent.ItemType<PowerBeam>())
			{
				PowerBeam powerBeamTarget = (PowerBeam)target.ModItem;
				spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

				// Item drawing.
				if (powerBeamTarget == null || powerBeamTarget.BeamMods == null || powerBeamTarget.BeamMods[addonSlotType].IsAir) { return; }

				Color itemColor = powerBeamTarget.BeamMods[addonSlotType].GetAlpha(Color.White);
				Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[powerBeamTarget.BeamMods[addonSlotType].type].Value;
				CalculatedStyle innerDimensions = GetDimensions();

				if (IsMouseHovering)
				{
					Main.hoverItemName = powerBeamTarget.BeamMods[addonSlotType].Name;
					Main.HoverItem = powerBeamTarget.BeamMods[addonSlotType].Clone();
				}

				Rectangle frame = Main.itemAnimations[powerBeamTarget.BeamMods[addonSlotType].type] != null
							? Main.itemAnimations[powerBeamTarget.BeamMods[addonSlotType].type].GetFrame(itemTexture)
							: itemTexture.Frame(1, 1, 0, 0);

				float drawScale = 1f;
				if (frame.Width > innerDimensions.Width || frame.Height > innerDimensions.Width)
				{
					if (frame.Width > frame.Height)
					{
						drawScale = innerDimensions.Width / frame.Width;
					}
					else
					{
						drawScale = innerDimensions.Width / frame.Height;
					}
				}

				float unreflectedScale = drawScale;
				Color tmpcolor = Color.White;

				ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, powerBeamTarget.BeamMods[addonSlotType].type);

				Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

				drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
				drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);

				if (powerBeamTarget.BeamMods[addonSlotType].color != default(Color))
				{
					spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																		   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
				}

				if (powerBeamTarget.BeamMods[addonSlotType].stack > 1)
				{
					Utils.DrawBorderStringFourWay(
						spriteBatch,
						FontAssets.ItemStack.Value,
						Math.Min(9999, powerBeamTarget.BeamMods[addonSlotType].stack).ToString(),
						innerDimensions.Position().X + 10f,
						innerDimensions.Position().Y + 26f,
						Color.White,
						Color.Black,
						Vector2.Zero,
						unreflectedScale * 0.8f);
				}
			}
			else if(target.type == ModContent.ItemType<ArmCannon>())
			{
				ArmCannon cannonTarget = (ArmCannon)target.ModItem;
				spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

				// Item drawing.
				if (cannonTarget == null || cannonTarget.BeamMods == null || cannonTarget.BeamMods[addonSlotType].IsAir) { return; }

				Color itemColor = cannonTarget.BeamMods[addonSlotType].GetAlpha(Color.White);
				Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[cannonTarget.BeamMods[addonSlotType].type].Value;
				CalculatedStyle innerDimensions = GetDimensions();

				if (IsMouseHovering)
				{
					Main.hoverItemName = cannonTarget.BeamMods[addonSlotType].Name;
					Main.HoverItem = cannonTarget.BeamMods[addonSlotType].Clone();
				}

				Rectangle frame = Main.itemAnimations[cannonTarget.BeamMods[addonSlotType].type] != null
							? Main.itemAnimations[cannonTarget.BeamMods[addonSlotType].type].GetFrame(itemTexture)
							: itemTexture.Frame(1, 1, 0, 0);

				float drawScale = 1f;
				if (frame.Width > innerDimensions.Width || frame.Height > innerDimensions.Width)
				{
					if (frame.Width > frame.Height)
					{
						drawScale = innerDimensions.Width / frame.Width;
					}
					else
					{
						drawScale = innerDimensions.Width / frame.Height;
					}
				}

				float unreflectedScale = drawScale;
				Color tmpcolor = Color.White;

				ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, cannonTarget.BeamMods[addonSlotType].type);

				Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

				drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
				drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);

				if (cannonTarget.BeamMods[addonSlotType].color != default(Color))
				{
					spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																		   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
				}

				if (cannonTarget.BeamMods[addonSlotType].stack > 1)
				{
					Utils.DrawBorderStringFourWay(
						spriteBatch,
						FontAssets.ItemStack.Value,
						Math.Min(9999, cannonTarget.BeamMods[addonSlotType].stack).ToString(),
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
	public class PowerBeamFrame : UIPanel
	{
		private Texture2D powerBeamFrame;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			powerBeamFrame = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerBeam_Frame", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = powerBeamFrame.Width;
			Height.Pixels = powerBeamFrame.Height;

			// Hardcoded position values.
			Top.Pixels = 80;
			Left.Pixels = 104;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(powerBeamFrame, DrawRectangle, Color.White);
		}
	}
	public class PowerBeamLines : UIPanel
	{
		private Asset<Texture2D> powerBeamLines;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			powerBeamLines = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerBeam_Lines", AssetRequestMode.ImmediateLoad);

			Width.Pixels = powerBeamLines.Value.Width;
			Height.Pixels = powerBeamLines.Value.Height;

			// Hardcoded position values.
			Top.Pixels = 0;
			Left.Pixels = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(powerBeamLines.Value, DrawRectangle, Color.White);
		}
	}

	// Charge Somersault attack toggle button
	public class PowerBeamScrewAttackButton : DragableUIPanel
	{
		private Texture2D buttonTex, buttonTex_Hover, buttonTex_Click,
		buttonTexEnabled, buttonTexEnabled_Hover, buttonTexEnabled_Click;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			Left.Pixels = 112;
			Top.Pixels = 274;

			buttonTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PsuedoScrewUIButton", AssetRequestMode.ImmediateLoad).Value;
			buttonTex_Hover = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PsuedoScrewUIButton_Hover", AssetRequestMode.ImmediateLoad).Value;
			buttonTex_Click = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PsuedoScrewUIButton_Click", AssetRequestMode.ImmediateLoad).Value;

			buttonTexEnabled = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PsuedoScrewUIButton_Enabled", AssetRequestMode.ImmediateLoad).Value;
			buttonTexEnabled_Hover = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PsuedoScrewUIButton_Enabled_Hover", AssetRequestMode.ImmediateLoad).Value;
			buttonTexEnabled_Click = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PsuedoScrewUIButton_Enabled_Click", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = buttonTex.Width;
			Height.Pixels = buttonTex.Height;
			OnLeftClick += SAButtonClick;
		}

		public override void Update(GameTime gameTime)
		{
			if (IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			enabled = MConfigClient.Instance.PsuedoScrewAttack.enabled;
			if (!enabled && MConfigClient.Instance.PsuedoScrewAttack.auto)
			{
				Left.Pixels = 112;
				Top.Pixels = 274;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}

		private bool clicked = false;
		private void SAButtonClick(UIMouseEvent evt, UIElement e)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

			mp.psuedoScrewActive = !mp.psuedoScrewActive;
			SoundEngine.PlaySound(SoundID.MenuTick);
			clicked = true;
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

			Texture2D tex = buttonTex, texH = buttonTex_Hover, texC = buttonTex_Click;
			if (mp.psuedoScrewActive)
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

				string psText = "Charge Somersault Attack: Disabled";
				if (mp.psuedoScrewActive)
				{
					psText = "Charge Somersault Attack: Enabled";
				}
				Main.hoverItemName = psText;
			}

			sb.Draw(tex, DrawRectangle, Color.White);
		}
	}
	public class PowerBeamChangeButton : DragableUIPanel
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
			//buttonTex_Click = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceClick", AssetRequestMode.ImmediateLoad).Value;
			buttonTexEnabled = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceOn", AssetRequestMode.ImmediateLoad).Value;
			buttonTexEnabled_Hover = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceHover", AssetRequestMode.ImmediateLoad).Value;
			//buttonTexEnabled_Click = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamInterfaceClick", AssetRequestMode.ImmediateLoad).Value;

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

			enabled = Configs.MConfigClient.Instance.PowerBeam.enabled;
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

			mp.beamChangeActive = !mp.beamChangeActive;
			//SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
			clicked = true;
			if (mp.beamChangeActive)
			{
				SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelect);
			}
			if (!mp.beamChangeActive)
			{
				SoundEngine.PlaySound(Sounds.Items.Weapons.BeamSelectFail);
			}
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

			Texture2D tex = buttonTex, texH = buttonTex_Hover, texC = buttonTex_Click;
			if (mp.beamChangeActive)
			{
				tex = buttonTexEnabled;
				texH = buttonTexEnabled_Hover;
				texC = buttonTexEnabled_Click;
			}

			if (IsMouseHovering)
			{
				// bug: buttonTex_Click isn't ever defined. what does this do?
				if (clicked)
				{
					texH = texC;
					clicked = false;
				}

				string psText = "Beam Interface: Disabled";
				if (mp.beamChangeActive)
				{
					psText = "Beam Interface: Enabled";
				}
				Main.hoverItemName = psText;
			}

			sb.Draw(tex, DrawRectangle, Color.White);
			if (IsMouseHovering)
			{
				sb.Draw(texH, DrawRectangle, Color.White);
			}
		}
	}

	// Combo Error messages
	public class ComboError : DragableUIPanel
	{
		private Texture2D iconTex;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			Left.Pixels = 420;
			Top.Pixels = 340; //354

			iconTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ComboErrorIcon", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = iconTex.Width;
			Height.Pixels = iconTex.Height;
		}

		public override void Update(GameTime gameTime)
		{
			if (IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			enabled = MConfigClient.Instance.PowerBeamError.enabled;
			if (!enabled && MConfigClient.Instance.PowerBeamError.auto)
			{
				Left.Pixels = 112;
				Top.Pixels = 340; //354
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as PowerBeam;
			ArmCannon cannonTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as ArmCannon;
			if (powerBeamTarget != null && (powerBeamTarget.comboError1 || powerBeamTarget.comboError2 || powerBeamTarget.comboError3 || powerBeamTarget.comboError4))
			{
				//MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

				if (IsMouseHovering)
				{
					string text = "Error: addon version mistmatch detected.\n" +
					"The following slots have had their addon effects disabled:";
					if (powerBeamTarget.comboError1)
					{
						text += "\nSecondary";
					}
					if (powerBeamTarget.comboError2)
					{
						text += "\nUtility";
					}
					if (powerBeamTarget.comboError3)
					{
						text += "\nPrimary A";
					}
					if (powerBeamTarget.comboError4)
					{
						text += "\nPrimary B";
					}
					text += "\n \n" +
					"Note: Addon stat bonuses are still applied.";

					Main.hoverItemName = text;
				}

				sb.Draw(iconTex, DrawRectangle, Color.White);
			}
			else if (cannonTarget != null && (cannonTarget.comboError1 || cannonTarget.comboError2 || cannonTarget.comboError3 || cannonTarget.comboError4))
			{
				if (IsMouseHovering)
				{
					string text = "Error: addon version mistmatch detected.\n" +
					"The following slots have had their addon effects disabled:";
					if (powerBeamTarget.comboError1)
					{
						text += "\nSecondary";
					}
					if (powerBeamTarget.comboError2)
					{
						text += "\nUtility";
					}
					if (powerBeamTarget.comboError3)
					{
						text += "\nPrimary A";
					}
					if (powerBeamTarget.comboError4)
					{
						text += "\nPrimary B";
					}
					text += "\n \n" +
					"Note: Addon stat bonuses are still applied.";

					Main.hoverItemName = text;
				}

				sb.Draw(iconTex, DrawRectangle, Color.White);
			}
		}
	}
}
