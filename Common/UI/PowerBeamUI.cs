using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Default;
using ReLogic.Content;

namespace MetroidMod.Common.UI
{
	/*
	 * The whole UI still feels a tad hacky, so it might need to get a bit of revamping here and there.
	 */
	public class PowerBeamUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.inventory[MetroidMod.Instance.selectedItem].type == ModContent.ItemType<PowerBeam>();

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
			new Vector2(174, 94)
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
			Height.Pixels = 164;
			enabled = MetroidMod.DragablePowerBeamUI;
			if (!enabled)
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

		public int addonSlotType;

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
			condition = delegate (Item addonItem)
			{
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
			// No failsafe. Should maybe be implemented?
			PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[MetroidMod.Instance.selectedItem].ModItem as PowerBeam;
			if (powerBeamTarget == null || powerBeamTarget.BeamMods == null) { return; }

			if (powerBeamTarget.BeamMods[addonSlotType] != null && !powerBeamTarget.BeamMods[addonSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = powerBeamTarget.BeamMods[addonSlotType].Clone();

					powerBeamTarget.BeamMods[addonSlotType].TurnToAir();
				}
				else if(condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = powerBeamTarget.BeamMods[addonSlotType].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					powerBeamTarget.BeamMods[addonSlotType] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if(!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					powerBeamTarget.BeamMods[addonSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			Item target = Main.LocalPlayer.inventory[MetroidMod.Instance.selectedItem];
			if (target == null || target.type != ModContent.ItemType<PowerBeam>()) { return; }
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

			//float unreflectedScale = drawScale;
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
			if(IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			enabled = MetroidMod.DragablePowerBeamUI;
			if (!enabled)
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
			if(mp.psuedoScrewActive)
			{
				tex = buttonTexEnabled;
				texH = buttonTexEnabled_Hover;
				texC = buttonTexEnabled_Click;
			}
			
			if(IsMouseHovering)
			{
				tex = texH;
				if(clicked)
				{
					tex = texC;
					clicked = false;
				}
				
				string psText = "Charge Somersault Attack: Disabled";
				if(mp.psuedoScrewActive)
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

			enabled = MetroidMod.DragablePowerBeamUI;
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
				tex = texH;
				if (clicked)
				{
					tex = texC;
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
			Top.Pixels = 354;
			
			iconTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ComboErrorIcon", AssetRequestMode.ImmediateLoad).Value;
			
			Width.Pixels = iconTex.Width;
			Height.Pixels = iconTex.Height;
		}
		
		public override void Update(GameTime gameTime)
		{
			if(IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			enabled = MetroidMod.DragablePowerBeamUI;
			if (!enabled)
			{
				Left.Pixels = 112;
				Top.Pixels = 354;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}
		
		protected override void DrawSelf(SpriteBatch sb)
		{
			PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[(MetroidMod.Instance).selectedItem].ModItem as PowerBeam;
			if(powerBeamTarget != null && (powerBeamTarget.comboError1 || powerBeamTarget.comboError2 || powerBeamTarget.comboError3 || powerBeamTarget.comboError4))
			{
				//MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
				
				if(IsMouseHovering)
				{
					string text = "Error: addon version mistmatch detected.\n"+
					"The following slots have had their addon effects disabled:";
					if(powerBeamTarget.comboError1)
					{
						text += "\nSecondary";
					}
					if(powerBeamTarget.comboError2)
					{
						text += "\nUtility";
					}
					if(powerBeamTarget.comboError3)
					{
						text += "\nPrimary A";
					}
					if(powerBeamTarget.comboError4)
					{
						text += "\nPrimary B";
					}
					text += "\n \n"+
					"Note: Addon stat bonuses are still applied.";
					
					Main.hoverItemName = text;
				}
				
				sb.Draw(iconTex, DrawRectangle, Color.White);
			}
		}
	}
}
