using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.UI;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using MetroidMod.Content.Items.Weapons;
using Terraria.GameContent.UI.Elements;
using System.Security.Cryptography.X509Certificates;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using MetroidMod.Common.Players;
using ReLogic.Content;
using Terraria.GameContent.Biomes;
using Terraria.Chat.Commands;
using Terraria.UI.Chat;
using Terraria.Chat;
using Terraria.ModLoader.UI;
using Microsoft.CodeAnalysis;
using MetroidMod.Common.GlobalItems;
using MonoMod.Logs;

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

		//TODO: when done, make it not look like shit          -Z
		private ArmCannonPanel armCannonPanel;
		private UIText debugInfo;

		public override void OnInitialize()
		{
			armCannonPanel = new ArmCannonPanel();
			armCannonPanel.Initialize();
			armCannonPanel.VAlign = 0.3f;
			armCannonPanel.Left.Pixels = 62;
			Append(armCannonPanel);

			debugInfo = new UIText("Initializing.\nWait until Update(), numbnuts", 0.75f);
			debugInfo.VAlign = armCannonPanel.VAlign;
			debugInfo.MarginLeft = armCannonPanel.MarginLeft + armCannonPanel.Width.Pixels + 5;
			Append(debugInfo);

		}

		public override void Update(GameTime gameTime)
		{
			//I had to reconfigure the fuck out of this shit and I'm doing some really hacky shit now and I don't like it
			armCannonPanel.Update(gameTime);
			if (Visible)
			{
				target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;
				debugInfo.SetText("SLOT INFO:" +
								  "\nPrimary (charge): " + target.BeamAddonAccess[0].Name +
								  "\nAbility (ice): " + target.BeamAddonAccess[1].Name +
								  "\nIon (wave): " + target.BeamAddonAccess[2].Name +
								  "\nSpread (spazer): " + target.BeamAddonAccess[3].Name +
								  "\nSecondary (plasma): " + target.BeamAddonAccess[4].Name +
								  "\nAmmo (ua): " + target.BeamAddonAccess[5].Name +
								  "\nCurrent Holdfire: Slot " + target.HoldFireSlot +
								  "\nHolding fire? " + Main.LocalPlayer.controlUseItem);
			}
		}
	}
	public class ArmCannonPanel : DragableUIPanel
	{
		//Why is this its own separate class? Idfk
		private Texture2D panelTexture;

		/// <summary>
		/// The slots that hold addons, stored in an array to save space.
		/// </summary>
		private AddonSlot[] addonSlots;
		/// <summary>
		/// Holds titles and missile/UA ammo count.
		/// </summary>
		private UIText[] info;
		/// <summary>
		/// The Arm Cannon this instance of the UI affects.
		/// </summary>
		private ArmCannon target;

		public Rectangle DrawRectangle => new Rectangle((int)GetDimensions().Position().X, (int)GetDimensions().Position().Y, (int)Width.Pixels, (int)Height.Pixels);

		//The exact coordinate positions of every addon slot in the UI.
		public Vector2[] slotPositions = new Vector2[BeamAddonSlotID.Count + MissileAddonSlotID.Count]
		{
			//Beam addons:
			new(78, 8), //Primary
			new(76, 175), //Ability
			new(244, 175), //Ion
			new(76, 108), //Spread
			new(244, 108), //Secondary
			new(130, 265), //Ammo
			//Missile addons:
			new(160, 16), //Charge
			new(244, 8), //Primary
			new(190, 265) //Tank
		};

		public override void OnInitialize()
		{
			//Set the textures for the UI.
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Border", AssetRequestMode.ImmediateLoad).Value;

			//Carve out the ol' spot where the UI shows up.
			Width.Pixels = panelTexture.Width;
			Height.Pixels = panelTexture.Height;

			//Add the fancy visuals
			Append(new ArmCannonFrame());
			Append(new ArmCannonLines());

			//Begin placing the slots
			addonSlots = new AddonSlot[slotPositions.Length];
			UIText[] addonLabels = new UIText[addonSlots.Length];
			for (int i = 0; i < addonSlots.Length; ++i)
			{
				addonSlots[i] = new AddonSlot();
				addonSlots[i].Top.Pixels = slotPositions[i].Y;
				addonSlots[i].Left.Pixels = slotPositions[i].X;
				if (i < BeamAddonSlotID.Count)
				{
					addonSlots[i].isBeam = true;
					addonSlots[i].slotType = i;
					if (i != 5)
					{
						addonLabels[i] = new UIText(i.ToString(), 0.75f); //TODO: Dictionary for slot names
						addonLabels[i].TextColor = Color.Orange;
					}
					else
					{
						addonLabels[i] = new UIText("", 0.01f);
					}
				}
				else
				{
					addonSlots[i].isBeam = false;
					addonSlots[i].slotType = (i - BeamAddonSlotID.Count);
					if (i != 8)
					{
						addonLabels[i] = new UIText((i).ToString(), 0.75f); //TODO: Dictionary for slot names
						addonLabels[i].TextColor = Color.Orange;
					}
					else
					{
						addonLabels[i] = new UIText("", 0.01f);
					}
				}
				addonLabels[i].Top.Pixels = addonSlots[i].Top.Pixels + addonSlots[i].Height.Pixels - 4;
				addonLabels[i].Left.Pixels = addonSlots[i].Left.Pixels + (addonSlots[i].Width.Pixels / 2 - 9);

				addonSlots[i].ItemRead = new Item();

				Append(addonSlots[i]);
				Append(addonLabels[i]);
			}

			info = new UIText[6];
			//All of the non-slot related labels go here
			info[0] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.ACTitle"), 0.75f);
			info[0].HAlign = 0.5f;
			info[0].Top.Pixels = -11;

			info[1] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.ACAmmoTitle"), 0.5f);
			info[1].HAlign = info[0].HAlign;
			info[1].Top.Pixels = 247;

			info[2] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.UALabel"), 0.75f);
			info[2].HAlign = 0.2f;
			info[2].Top.Pixels = 260;

			info[3] = new UIText(Language.GetTextValue("Mods.MetroidMod.UILabelling.MissileLabel"), 0.75f);
			info[3].HAlign = 0.8f;
			info[3].Top.Pixels = 260;

			//These two show how much ammo the player has.
			info[4] = new UIText("  0/  0", 1f);
			info[4].HAlign = info[2].HAlign;
			info[4].Top.Pixels = 280;

			info[5] = new UIText("  0/  0", 1f);
			info[5].HAlign = info[3].HAlign;
			info[5].Top.Pixels = 280;

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
			MGlobalItem ac = (Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem]).GetGlobalItem<MGlobalItem>();
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
			info[0].Top.Pixels = -11;
			info[1].Top.Pixels = 247;
			info[1].SetText(Language.GetTextValue("Mods.MetroidMod.UILabelling.ACAmmoTitle"));
			info[2].HAlign = 0.2f;
			info[3].HAlign = 0.8f;
			info[4].HAlign = info[2].HAlign;
			info[4].Top.Pixels = info[5].Top.Pixels = 285;
			info[5].HAlign = info[3].HAlign;

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

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 realPosition = GetDimensions().Position();
			spriteBatch.Draw(panelTexture, new Rectangle((int)realPosition.X, (int)realPosition.Y, panelTexture.Width, panelTexture.Height), Color.White);
		}
	}

	public class ArmCannonFrame : UIPanel
	{
		private Texture2D armCannonFrame;
		public Rectangle DrawRectangle => new Rectangle((int)(Parent.GetDimensions().Position().X + Left.Pixels), (int)(Parent.GetDimensions().Position().Y + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);


		public override void OnInitialize()
		{
			// Set the textures for the UI.
			armCannonFrame = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Frame", AssetRequestMode.ImmediateLoad).Value;

			//Carve out the ol' spot where the UI shows up.
			Width.Pixels = armCannonFrame.Width;
			Height.Pixels = armCannonFrame.Height;
			
			//Hardcoded position values. This is a handcrafted sucker.
			Left.Pixels = 166;
			Top.Pixels = 120;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(armCannonFrame, DrawRectangle, Color.White);
		}
	}
	public class ArmCannonLines : UIPanel
	{
		private Texture2D armCannonLines;
		public Rectangle DrawRectangle => new Rectangle((int)(Parent.GetDimensions().Position().X + Left.Pixels), (int)(Parent.GetDimensions().Position().Y + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			armCannonLines = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ArmCannon_Lines", AssetRequestMode.ImmediateLoad).Value;

			//Carve out the ol' spot where the UI shows up.
			Width.Pixels = armCannonLines.Width;
			Height.Pixels = armCannonLines.Height;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(armCannonLines, DrawRectangle, Color.White);
		}
	}

	public class AddonSlot : UIItemSlot //I did not make UIItemSlot. I am simply the one that found it. or perhaps it found me
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
			/// The type of slot the slot is
			/// <br/>Only important for standard beam addons
			/// </summary>
			public int slotType;

			public AddonSlot()
			{
				backgroundTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox", AssetRequestMode.ImmediateLoad).Value;
			}
			public override void LeftMouseDown(UIMouseEvent evt)
			{
				Player player = Main.LocalPlayer;
				//This part does the calculation for if you clicking an item slot has an effect on its contents
				//this is gonna be real long and nesty, brace yourself
				MetroidMod.Instance.Logger.Info("Begin the clickening");
				if (!Main.mouseItem.IsAir)
				{
					MetroidMod.Instance.Logger.Info("You're definitely holding something");
					if (isBeam == true)
					{
						MetroidMod.Instance.Logger.Info("It's a beam addon slot");
						ModBeamAddon heldItem = BeamAddonLoader.GetAddon(Main.mouseItem);
						if (heldItem != null)
						{
							MetroidMod.Instance.Logger.Info("The held item IS a beam addon!\nThe addon in question: " + heldItem);
							if (heldItem.AddonSlot == slotType || isArray == true) //If it's an array then slot numbers don't matter
							{
								if ((Main.mouseItem.type == ItemRead.type) && (Main.mouseItem.stack + ItemRead.stack <= ItemRead.maxStack))
								{
									MetroidMod.Instance.Logger.Info("We stackin this shit");
									SlotMagic(true);
								} //Account for stacks
								else if (Main.mouseItem.type != ItemRead.type)
								{
									MetroidMod.Instance.Logger.Info("We NOT stackin this shit");
									SlotMagic(true);
								} //Items can't stack, check if they can swap
							}
						}
					}
					else
					{
						ModMissileAddon heldItem = MissileAddonLoader.GetAddon(Main.mouseItem);
						if (heldItem != null)
						{
							MetroidMod.Instance.Logger.Info("It's a missile addon slot");
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
					if (!ItemRead.IsAir)
					{
						MetroidMod.Instance.Logger.Info("erm.... it is TAKING.");
						SlotMagic(false);
					}
				}
			}

			public override void DarkMagic(Item ItemWrite, bool StackAttack)
			{
				ArmCannon target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;
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
							if (isBeam) { target.BeamAddonAccess[slotType] = ItemWrite; MetroidMod.Instance.Logger.Info("Addon inserted"); target.ArrayUpdate(); }
							else { target.MissileAddonAccess[slotType] = ItemWrite; target.ArrayUpdate(); }
						}
					}
				}
			}
		}
	}
