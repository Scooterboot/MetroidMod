using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	/// <summary>
	/// The UI for the arm cannon
	/// <br/> jazz this desc up later
	/// </summary>
	public class ArmCannonUI : UIState
	{
		//Let it be known that I have zero clue what I'm doing        -Z
		/// <summary>
		/// The <see cref="ArmCannon"/> the UI is currently accessing.
		/// </summary>
		private ArmCannon target;
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.chest == -1 && (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].type == ModContent.ItemType<ArmCannon>());

		private UIDraggableBase baseBoard;
		private ArmCannonPanel armCannonPanel;
		private ArrayPanel beamArrayPanel;
		private ArrayPanel missileArrayPanel;

		private Asset<Texture2D> barTex => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/SuperMetroidUI_Border");
		private Asset<Texture2D> bgTex => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/SuperMetroidUI_BG");
		private UIPanelTileBackground buttonTab;


		#region buttons
		//TODO: make class for fancy buttons
		private UIImageButton beamArrayToggle;
		private bool beamPanelOpen = false;
		private UIImageButton missileArrayToggle;
		private bool missilePanelOpen = false;

		private UIImageButton pseudoScrewToggle;
		private Asset<Texture2D> psButtonOff => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PseudoScrewUIButton");
		private Asset<Texture2D> psButtonOffHover => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PseudoScrewUIButton_Hover");
		private Asset<Texture2D> psButtonOffClick => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PseudoScrewUIButton_Click");
		private Asset<Texture2D> psButtonOn => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PseudoScrewUIButton_Enabled");
		private Asset<Texture2D> psButtonOnHover => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/PseudoScrewUIButton_Enabled_Hover");
		#endregion

		private UIPanelTileBackground testPanel;
		private UIText debugInfo;

		public override void OnInitialize()
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			#region baseboard
			//The baseboard ensures that when the player deems fit to drag the UI around, all associated elements come with.
			baseBoard = new UIDraggableBase();
			//Hardcoded size values, perfectly tailored to the UI assets
			baseBoard.SetPadding(0);
			baseBoard.Width.Pixels = 492;
			baseBoard.Height.Pixels = 332;
			baseBoard.VAlign = 0.33f;
			baseBoard.Left.Pixels = 62;
			Append(baseBoard);
			#endregion

			#region buttons tab
			buttonTab = new UIPanelTileBackground(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/BackgroundSolid"), barTex, 10, 10);
			buttonTab.panelColor = Color.Black;
			buttonTab.SetPadding(0);
			buttonTab.Width.Pixels = 90;
			buttonTab.Height.Pixels = 106;
			baseBoard.Append(buttonTab);
			#endregion

			#region beam array panel
			beamArrayPanel = new ArrayPanel(bgTex, barTex, 10, 10);
			beamArrayPanel.SetPadding(0);

			beamArrayPanel.Top.Pixels = 6;
			beamArrayPanel.Left.Pixels = beamPanelOpen ? 310 : 144;

			beamArrayPanel.isBeam = true;

			beamArrayPanel.Initialize();
			baseBoard.Append(beamArrayPanel);
			#endregion

			#region missile array panel
			missileArrayPanel = new ArrayPanel(bgTex, barTex, 10, 10);
			missileArrayPanel.SetPadding(0);
			missileArrayPanel.isBeam = false;
			missileArrayPanel.Top.Pixels = 6 + beamArrayPanel.Height.Pixels;
			missileArrayPanel.Left.Pixels = beamPanelOpen ? 310 : 144;

			missileArrayPanel.Initialize();
			baseBoard.Append(missileArrayPanel);
			#endregion

			armCannonPanel = new ArmCannonPanel(bgTex, barTex, 10, 10);
			armCannonPanel.Initialize();
			armCannonPanel.Left.Pixels = 56;
			baseBoard.Append(armCannonPanel);

			#region buttons
			beamArrayToggle = new UIImageButton(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamArrayButton_Off"));
			beamArrayToggle.Left.Pixels = 8.5f;
			beamArrayToggle.Top.Pixels = 7.5f;
			beamArrayToggle.Width.Pixels = beamArrayToggle.Height.Pixels = 44;
			beamArrayToggle.SetVisibility(1f, 1f);
			beamArrayToggle.SetHoverImage(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/BeamArrayButton_Off_Hover"));
			beamArrayToggle.OnLeftClick += new MouseEvent(BeamButtonClicked);
			buttonTab.Append(beamArrayToggle);

			missileArrayToggle = new UIImageButton(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/ChargeComboButton_Off"));
			missileArrayToggle.Left.Pixels = 8.5f;
			missileArrayToggle.Top.Pixels = 52.5f;
			missileArrayToggle.Width.Pixels = missileArrayToggle.Height.Pixels = 44;
			missileArrayToggle.SetVisibility(1f, 1f);
			missileArrayToggle.SetHoverImage(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/Buttons/ChargeComboButton_Off_Hover"));
			missileArrayToggle.OnLeftClick += new MouseEvent(MissileButtonClicked);
			buttonTab.Append(missileArrayToggle);

			pseudoScrewToggle = new UIImageButton(mp.pseudoScrewActive ? psButtonOn : psButtonOff);
			pseudoScrewToggle.Left.Pixels = 8;
			pseudoScrewToggle.Top.Pixels = 110;
			pseudoScrewToggle.Width.Pixels = pseudoScrewToggle.Height.Pixels = 44;
			pseudoScrewToggle.SetVisibility(1f, 1f);
			pseudoScrewToggle.SetHoverImage(mp.pseudoScrewActive ? psButtonOnHover : psButtonOffHover);
			pseudoScrewToggle.OnLeftClick += new MouseEvent(PSAButtonClicked);
			baseBoard.Append(pseudoScrewToggle);
			#endregion



			debugInfo = new UIText("Initializing.\nWait until Update(), numbnuts", 0.75f);
			debugInfo.VAlign = baseBoard.VAlign * 2f;
			debugInfo.MarginLeft = baseBoard.MarginLeft + armCannonPanel.Width.Pixels + (buttonTab.Width.Pixels / 2);
			Append(debugInfo);

			testPanel = new UIPanelTileBackground(bgTex, barTex, 10, 10);
			testPanel.VAlign = 0.75f;
			testPanel.HAlign = 0.1f;
			testPanel.Width.Pixels = 100;
			testPanel.Height.Pixels = 100;
			Append(testPanel);
		}

		public override void Update(GameTime gameTime)
		{
			//I had to reconfigure the fuck out of this shit and I'm doing some really hacky shit now and I don't like it
			armCannonPanel.Update(gameTime);    //Don't forget to call Update()
			beamArrayPanel.Update(gameTime);    //So that the sub-elements can
			missileArrayPanel.Update(gameTime); //All properly update!
			if (Visible)
			{
				target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;
				debugInfo.SetText("CHOZO UNIVERSAL WEAPONS PLATFORM\nv0.8.0.1\nDEVELOPER MODE\nSLOT INFO:"
								  //+ "\nPrimary (charge): " + target.BeamAddonAccess[0].Name
								  //+ "\nAbility (ice): " + target.BeamAddonAccess[1].Name
								  //+ "\nIon (wave): " + target.BeamAddonAccess[2].Name
								  //+ "\nSpread (spazer): " + target.BeamAddonAccess[3].Name
								  //+ "\nSecondary (plasma): " + target.BeamAddonAccess[4].Name
								  //+ "\nAmmo (ua): " + target.BeamAddonAccess[5].Name
								  + "\nActive beam array slot: " + target.activeBeamArraySlot
								  + "\nActive missile array slot: " + target.activeMissileArray
								  + "\nCurrent Holdfire: Slot " + target.HoldFireSlot
								  + "\nHolding fire? " + Main.LocalPlayer.controlUseItem
								  + "\nVisual Dinners: [" + target.VisualDinners[0] + ", " + target.VisualDinners[1] + ", " + target.VisualDinners[2] + ", " + target.VisualDinners[3] + "]"
								  );
			}
		}

		private void PSAButtonClicked(UIMouseEvent evt, UIElement listingElement)
		{
			MetroidMod.Instance.Logger.Debug("Pseudo-screw toggle");
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			SoundEngine.PlaySound(SoundID.MenuTick);
			mp.pseudoScrewActive = !mp.pseudoScrewActive;
			pseudoScrewToggle.SetImage(mp.pseudoScrewActive ? psButtonOn : psButtonOff);
			pseudoScrewToggle.SetHoverImage(mp.pseudoScrewActive ? psButtonOnHover : psButtonOffHover);
			pseudoScrewToggle.Width.Pixels = pseudoScrewToggle.Height.Pixels = 44;
		}
		private void BeamButtonClicked(UIMouseEvent evt, UIElement listingElement)
		{
			beamPanelOpen = !beamPanelOpen;
			beamArrayPanel.Left.Pixels = beamPanelOpen ? 310 : 144;
			SoundEngine.PlaySound(SoundID.MenuTick);
		}
		private void MissileButtonClicked(UIMouseEvent evt, UIElement listingElement)
		{
			missilePanelOpen = !missilePanelOpen;
			missileArrayPanel.Left.Pixels = missilePanelOpen ? 310 : 144;
			SoundEngine.PlaySound(SoundID.MenuTick);
		}
	}
	public class ArmCannonPanel : UIPanelTileBackground
	{
		//The various lines and patterns drawn on top of the panel.
		private UIImage panelLines;
		/// <summary>
		/// The wireframe representation of the item this UI interfaces with.
		/// </summary>
		private UIImage panelWireframe;

		private UIImage panelTitleBox;

		/// <summary>
		/// The slots that hold addons, stored in an array to save space.
		/// </summary>
		private ArmCannonAddonSlot[] addonSlots;
		/// <summary>
		/// The cute little labels below addon slots that tell you what slot they are.
		/// </summary>
		private SlotLabel[] slotLabels;
		/// <summary>
		/// Holds titles and missile/UA ammo count.
		/// </summary>
		private UIText[] info;
		/// <summary>
		/// The Arm Cannon this instance of the UI affects.
		/// </summary>
		private ArmCannon target;

		//The exact coordinate positions of every addon slot in the UI.
		public Vector2[] slotPositions = new Vector2[BeamAddonSlotID.Count + MissileAddonSlotID.Count]
		{
			//Beam addons:
			new(35, 18), //[0]Primary
			new(34, 188), //[1]Ability
			new(202, 188), //[2]Ion
			new(34, 118), //[3]Spread
			new(202, 118), //[4]Secondary
			new(90, 276), //[5]Ammo
			//Missile addons:
			new(118, 28), //[6]Charge
			new(201, 18), //[7]Primary
			new(146, 276) //[8]Tank
		};

		public ArmCannonPanel(Asset<Texture2D> panel, Asset<Texture2D> border, int cornerSize = 12, int barSize = 4) : base(panel, border, cornerSize, barSize) { }

		public override void OnInitialize()
		{
			//Set the textures for the UI.

			Asset<Texture2D> labelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/BackgroundSolid");
			Color labelColor = new Color(248, 176, 0);

			//Set the size precisely so everything's in place!
			Width.Pixels = 280;
			Height.Pixels = 332;
			SetPadding(0);

			panelLines = new UIImage(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Lines"));
			Append(panelLines);
			panelWireframe = new UIImage(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Frame"));
			panelWireframe.Left.Pixels = 112;
			panelWireframe.Top.Pixels = 118;
			Append(panelWireframe);

			panelTitleBox = new UIImage(ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_TitleBar"));
			panelTitleBox.HAlign = 0.22f;
			panelTitleBox.Top.Pixels = -3;
			Append(panelTitleBox);

			//Begin placing the slots
			addonSlots = new ArmCannonAddonSlot[slotPositions.Length];
			//UIText[] addonLabels = new UIText[addonSlots.Length];
			slotLabels = new SlotLabel[addonSlots.Length];
			for (int i = 0; i < addonSlots.Length; ++i)
			{
				addonSlots[i] = new ArmCannonAddonSlot();
				addonSlots[i].Top.Pixels = slotPositions[i].Y;
				addonSlots[i].Left.Pixels = slotPositions[i].X;
				//Needed to center slot labels
				float slotCenterX = slotPositions[i].X + (addonSlots[i].Width.Pixels / 2) - 4;
				if (i < BeamAddonSlotID.Count)
				{
					addonSlots[i].isBeam = true;
					addonSlots[i].slotType = i;
					if (i != 5)
					{
						slotLabels[i] = new SlotLabel(labelTexture, labelColor, BeamAddonSlotID.GetSlotName(i).ToUpper(), 0.75f, Color.Black);
					}
				} //Power Beam addon slots
				else
				{
					addonSlots[i].isBeam = false;
					addonSlots[i].slotType = i - BeamAddonSlotID.Count;
					if (i != 8)
					{
						slotLabels[i] = new SlotLabel(labelTexture, labelColor, MissileAddonSlotID.GetSlotName(i - BeamAddonSlotID.Count).ToUpper(), 0.75f, Color.Black);
					}
				} //Missile Launcher addon slots

				addonSlots[i].ItemRead = new Item();

				Append(addonSlots[i]);

				if (i != 5 && i != 8)
				{
					float labelOffX = slotLabels[i].Width.Pixels / 2;
					slotLabels[i].Top.Pixels = (int)Math.Ceiling(addonSlots[i].Top.Pixels + addonSlots[i].Height.Pixels - 4);
					slotLabels[i].Left.Pixels = (int)Math.Ceiling(slotCenterX - labelOffX);
					Append(slotLabels[i]);
				}
			}

			info = new UIText[6];
			//All of the non-slot related labels go here
			//[0] - UI title ("ARM CANNON")
			//[1] - ammo section title ("A M M O")
			//[2] - UA label ("BEAM")
			//[3] - Missile label ("MISSILE")
			//[4] - UA ammo counter
			//[5] - Missile ammo counter

			info[0] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.ACTitle"), 0.75f);
			info[0].HAlign = 0.5f;

			info[1] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.ACAmmoTitle"), 0.57f);
			info[1].HAlign = info[0].HAlign;
			info[1].Top.Pixels = 258;

			info[2] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.UALabel"), 0.38f, true);
			info[2].HAlign = 0.11f;
			info[2].Top.Pixels = 278;

			info[3] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.MissileLabel"), 0.38f, true);
			info[3].HAlign = 0.92f;
			info[3].Top.Pixels = 278;

			//These two show how much ammo the player has.
			//UA
			info[4] = new UIText("  0/  0", 0.44f, true);
			info[4].HAlign = 0.06f;
			info[4].Top.Pixels = 300;
			//Missiles
			info[5] = new UIText("  0/  0", 0.44f, true);
			info[5].HAlign = 0.94f;
			info[5].Top.Pixels = 300;

			//Gonna append them in a for loop cause I'm too lazy to write 6 different appends
			for (int i = 0; i < info.Length; ++i)
			{
				Append(info[i]);
			}
		}

		public override void Update(GameTime gameTime)
		{
			// Ignore mouse input.
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;
			MGlobalItem ac = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].GetGlobalItem<MGlobalItem>();
			for (int i = 0; i < addonSlots.Length; ++i)
			{
				if (i < BeamAddonSlotID.Count)
				{
					addonSlots[i].ItemRead = target.BeamAddonAccess[i];
				}
				else
				{
					addonSlots[i].ItemRead = target.MissileAddonAccess[i - BeamAddonSlotID.Count];
				}
			}

			#region Hotload adjustments
			#endregion

			info[4].SetText(ac.statUA.ToString("000") + "/" + ac.maxUA.ToString("000"));
			if (ac.maxUA == ac.statUA && ac.maxUA != 0)
			{
				info[4].TextColor = Color.Gold;
			}
			else if (ac.statUA == 0 && ac.maxUA != 0)
			{
				info[4].TextColor = Color.Red;
			}
			else if (ac.maxUA == 0)
			{
				info[4].TextColor = info[2].TextColor = Color.Gray;
			}
			else { info[4].TextColor = info[2].TextColor = Color.White; }

			info[5].SetText(ac.statMissiles.ToString("000") + "/" + ac.maxMissiles.ToString("000"));
			if (ac.maxMissiles == ac.statMissiles && ac.maxMissiles != 0)
			{
				info[5].TextColor = Color.Gold;
			}
			else if (ac.statMissiles == 0 && ac.maxMissiles != 0)
			{
				info[5].TextColor = Color.Red;
			}
			else if (ac.maxMissiles == 0)
			{
				info[5].TextColor = info[3].TextColor = Color.Gray;
			}
			else { info[5].TextColor = info[3].TextColor = Color.White; }
		}
	}

	public class ArrayPanel : UIPanelTileBackground
	{
		//Okay so there's two ways I see myself going about this:
		//1: Each array slot is a fully-functional beam slot and they all have buttons under them, rendering the charge slot worthless.
		//2: Each array slot is a glorified button that just displays what's in that slot, and things can only be added or removed via the charge slot.
		//I could HYPOTHETICALLY do it kinda like it was before but I don't really want to because A) that feels like a recipe for accidentally deleting items and B) it's already hacky as fuck
		//I suppose only time will tell which I end up going with.

		private ArmCannon target;

		/// <summary>
		/// Is it the beam array or is it the missile array?
		/// <br/>This bool is the deciding factor, but only for <b>ArrayPanels</b>.
		/// </summary>
		public bool isBeam;
		public Asset<Texture2D> slotTex => ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox_Metal", AssetRequestMode.ImmediateLoad);
		public Asset<Texture2D> beamFrameTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_ArrayFrame_Beam");
		public Asset<Texture2D> missileFrameTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_ArrayFrame_Missile");

		private UIImage ArrayFrame;

		private ArmCannonAddonSlot[] arraySlots;
		public Vector2[] slotPositions = new Vector2[8]
		{
			new(80, 10), //Top
			new(128, 10), //Top-right
			new(128, 58), //Right
			new(128, 106), //Bottom-right
			new(80, 106), //Bottom
			new(32, 106), //Bottom-left
			new(32, 58), //Left
			new(32, 10) //Top-left
		};
		public ArrayPanel(Asset<Texture2D> panel, Asset<Texture2D> border, int cornerSize = 12, int barSize = 4) : base(panel, border, cornerSize, barSize) { }


		public override void OnInitialize()
		{
			Width.Pixels = 182;
			Height.Pixels = 160;

			arraySlots = new ArmCannonAddonSlot[8];
			//place the array slots in position
			for (int i = 0; i < arraySlots.Length; ++i)
			{
				arraySlots[i] = new ArmCannonAddonSlot(slotTex);
				arraySlots[i].Left.Pixels = slotPositions[i].X;
				arraySlots[i].Top.Pixels = slotPositions[i].Y;

				arraySlots[i].isArray = true;
				arraySlots[i].slotType = 0; //both arrays only take addons of their respective type 0
				arraySlots[i].slotNumber = i;
				arraySlots[i].isBeam = isBeam;

				Append(arraySlots[i]);
			}

			ArrayFrame = new UIImage(isBeam ? beamFrameTex : missileFrameTex);
			ArrayFrame.Width.Pixels = isBeam ? 28 : 20;
			ArrayFrame.Height.Pixels = isBeam ? 28 : 36;
			ArrayFrame.Left.Pixels = 12;
			ArrayFrame.HAlign = 0.5f;
			ArrayFrame.VAlign = 0.5f;
			Append(ArrayFrame);
		}
		public override void Update(GameTime gameTime)
		{
			//Currently selected array slot is colored yellow
			//All others default to Color.White
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;
			MGlobalItem ac = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].GetGlobalItem<MGlobalItem>();
			for (int i = 0; i < arraySlots.Length; ++i)
			{
				arraySlots[i].ItemRead = isBeam ? target.ChargeQuickSwapAccess[i] : target.ComboQuickChangeAccess[i];
				if ((target.activeBeamArraySlot == arraySlots[i].slotNumber && arraySlots[i].isBeam) || (target.activeMissileArray == arraySlots[i].slotNumber && !arraySlots[i].isBeam))
				{
					arraySlots[i].slotColor = new Color(252, 195, 0);
				}
				else
				{
					arraySlots[i].slotColor = Color.DarkGray;
				}
			}
		}
	}

	//Old experiment for array panels. Keeping it around just in case.
	//public class UIHidable : UIElement
	//{
	//	public bool Hidden;
	//	public override void Draw(SpriteBatch spriteBatch)
	//	{
	//		if (Hidden) return;
	//		base.Draw(spriteBatch);
	//	}
	//	public override void Update(GameTime gameTime)
	//	{
	//		if (Hidden) return;
	//		base.Update(gameTime);
	//	}
	//}

	public class ArmCannonAddonSlot : UIItemSlot //I did not make UIItemSlot. I am simply the one that found it. or perhaps it found me
	{
		public delegate bool Condition(Item item); //Idk the deal with these but I prolly have to use these because something something data security

		/// <summary>
		/// If <b>true</b>, it's a beam addon slot and not a missile addon slot.
		/// </summary>
		public bool isBeam;
		/// <summary>
		/// If <b>true</b>, it's an array slot.
		/// </summary>
		public bool isArray;
		/// <summary>
		/// The addon type this slot accepts.
		/// <br/>For non-Array slots, this also serves as its slot number.
		/// </summary>
		public int slotType;
		/// <summary>
		/// For array slots. The position in the array this slot maps to.
		/// </summary>
		public int slotNumber;

		private ArmCannon target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;

		public ArmCannonAddonSlot(Asset<Texture2D> slotTexture = null)
		{
			if (slotTexture == null) { slotTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox", AssetRequestMode.ImmediateLoad); }
			backgroundTexture = slotTexture.Value;
		}
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			Player player = Main.LocalPlayer;
			target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem; //failsafe
			bool isDupe = false; //To make sure you can't get more than one of the same nonstacking addon in an arm cannon.
								 //This part does the calculation for if you clicking an item slot has an effect on its contents
								 //this is gonna be real long and nesty, brace yourself
			MetroidMod.Instance.Logger.Info("Begin the clickening");
			if (!Main.mouseItem.IsAir)
			{
				//MetroidMod.Instance.Logger.Info("You're definitely holding something");
				if (isBeam)
				{
					MetroidMod.Instance.Logger.Info("It's a beam addon slot and you're holding something");
					ModBeamAddon heldItem = BeamAddonLoader.GetAddon(Main.mouseItem);
					if (heldItem != null && !isArray)
					{
						MetroidMod.Instance.Logger.Info("The held item IS a beam addon!\nThe addon in question: " + heldItem);
						if (heldItem.AddonSlot == slotType)
						{
							if (slotType == 0)
							{
								isDupe = BeamAddonLoader.ArrayDupeChecker(target.ChargeQuickSwapAccess, heldItem);
							}

							if ((Main.mouseItem.type == ItemRead.type) && (Main.mouseItem.stack + ItemRead.stack <= ItemRead.maxStack) && !isDupe)
							{
								MetroidMod.Instance.Logger.Info("We stackin this shit");
								SlotMagic(true);
							} //Account for stacks
							else if (Main.mouseItem.type != ItemRead.type && !isDupe)
							{
								MetroidMod.Instance.Logger.Info("We NOT stackin this shit");
								SlotMagic(true);
							} //Items can't stack, check if they can swap
							else if (isDupe)
							{
								SoundEngine.PlaySound(SoundID.Item16);
							}
						}
					}
				}
				else
				{
					ModMissileAddon heldItem = MissileAddonLoader.GetAddon(Main.mouseItem);
					if (heldItem != null)
					{
						MetroidMod.Instance.Logger.Info("It's a missile addon slot and you're holding something");
						if (heldItem.AddonSlot == slotType || isArray == true) //If it's an array then slot numbers don't matter
						{
							if ((Main.mouseItem.type == ItemRead.type) && (Main.mouseItem.stack + ItemRead.stack <= ItemRead.maxStack))
							{
								SlotMagic(true);
							} //Account for stacks
							else if (Main.mouseItem.type != ItemRead.type)
							{
								SlotMagic(true);
							} //Items can't stack, check if they can swap
						}
					}
				}
			}//Check for putting an item into a slot first, then if the mouse is empty
			else
			{
				MetroidMod.Instance.Logger.Info("Caught empty-handed boi");
				if (isBeam)
				{
					if (isArray)
					{
						SoundEngine.PlaySound(SoundID.MenuTick);
						target.activeBeamArraySlot = slotNumber;
						target.HoldFireSlot = -1;
						target.BeamAddonAccess[0] = target.ChargeQuickSwapAccess[slotNumber];
						target.ArrayUpdate();
					}
					else if (!ItemRead.IsAir)
					{
						MetroidMod.Instance.Logger.Info("erm.... it is TAKING.");
						SlotMagic(false);
					}
				}
				else
				{
					if (isArray)
					{
						SoundEngine.PlaySound(SoundID.MenuTick);
						target.activeMissileArray = slotNumber;
						target.MissileAddonAccess[0] = target.ComboQuickChangeAccess[slotNumber];
						target.ArrayUpdate();
					}
					else if (!ItemRead.IsAir)
					{
						MetroidMod.Instance.Logger.Info("erm.... it is TAKING.");
						SlotMagic(false);
					}
				}
			}
		}

		public override void DarkMagic(Item ItemWrite, bool StackAttack)
		{
			target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem; //failsafe
																												   //Takes the action attempted through SlotMagic and applies the effect to the addon array.
			if (StackAttack) //Player tried to stack stuff
			{
				if (ItemWrite == null) //Player removed a thing from the array
				{
					if (isArray) //Is it a quick-change menu?
					{
						SoundEngine.PlaySound(SoundID.Item16);
						ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Main.LocalPlayer.name + " tried to do something that shouldn't be physically possible."), Color.Red);
					}
					else
					{
						if (isBeam) { target.BeamAddonAccess[slotType].stack += ItemWrite.stack; target.ArrayUpdate(); }
						else { target.MissileAddonAccess[slotType].stack += ItemWrite.stack; target.ArrayUpdate(); }
					}
				}
			}
			else
			{
				if (ItemWrite == null) //Player removed a thing from the array
				{
					if (isArray) //Is it a quick-change menu?
					{
						if (isBeam) { target.ChargeQuickSwapAccess[slotType].TurnToAir(); target.ArrayUpdate(); }
						else { target.ComboQuickChangeAccess[slotType].TurnToAir(); target.ArrayUpdate(); }
					}
					else
					{
						if (isBeam) { target.BeamAddonAccess[slotType].TurnToAir(); target.ArrayUpdate(); }
						else { target.MissileAddonAccess[slotType].TurnToAir(); target.ArrayUpdate(); }
					}
				}
				else
				{
					if (isArray)
					{
						if (isBeam) { target.ChargeQuickSwapAccess[slotType] = ItemWrite; target.ArrayUpdate(); }
						else { target.ComboQuickChangeAccess[slotType] = ItemWrite; target.ArrayUpdate(); }
					}
					else
					{
						if (isBeam) { target.BeamAddonAccess[slotType] = ItemWrite; target.ArrayUpdate(); }
						else { target.MissileAddonAccess[slotType] = ItemWrite; target.ArrayUpdate(); }
					}
				}
			}
		}
	}

}
