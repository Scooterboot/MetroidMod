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
using Microsoft.Build.Execution;
using Terraria.GameContent.Biomes;
using Terraria.Chat.Commands;
using Terraria.UI.Chat;
using Terraria.Chat;

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

		//TODO: when done, make it not look like shit
		private UIPanel panel;
		private UIText text;
		private UIText text2;
		private UIPanel pseudoScrewButton;
		private UIText psdText;
		private AddonSlot[] BeamSlot;
		private UIText debugInfo;
		//private AddonSlot[] ChargeSlot;
		//private AddonSlot[] MissileSlot;
		//private AddonSlot[] ComboSlot;
		//private UIPanel QuickSwapPanel;

		public override void OnInitialize()
		{
			panel = new UIPanel();
			panel.Width.Set(380, 0);
			panel.Height.Set(230, 0);
			panel.MarginTop = panel.VAlign = 0.305f;
			panel.MarginLeft = 66;
			panel.PaddingTop = 0;
			panel.BackgroundColor = new Color(0, 0, 0.25f, 0.5f);
			panel.BorderColor = Color.Teal;
			Append(panel);

			text = new UIText("CHOZO UNIVERSAL WEAPONS PLATFORM", 0.7f);
			text.HAlign = 0.5f;
			text.VAlign = 0.05f;
			text2 = new UIText("v.0.3.8.1 beta", 0.65f);
			text2.HAlign = text.HAlign;
			text2.VAlign = text.VAlign + 0.08f;
			UIText text3 = new UIText("DEVELOPER MODE", 0.5f);
			text3.TextColor = Color.Yellow;
			text3.VAlign = 1f;
			panel.Append(text);
			panel.Append(text2);
			panel.Append(text3);

			pseudoScrewButton = new UIPanel();
			pseudoScrewButton.Width.Set(75, 0);
			pseudoScrewButton.Height.Set(30, 0);
			pseudoScrewButton.HAlign = 0.97f;
			pseudoScrewButton.VAlign = 0.95f;
			pseudoScrewButton.BackgroundColor = Color.Red;
			pseudoScrewButton.OnLeftClick += OnPSBClick;
			psdText = new UIText("P.S.A");
			psdText.HAlign = psdText.VAlign = 0.5f;
			panel.Append(pseudoScrewButton);
			pseudoScrewButton.Append(psdText);

			BeamSlot = new AddonSlot[5];
			UIText[] labels = new UIText[5];
			for (int i = 0; i < 5; ++i) //do it like this for now because I'll figure out charge shit later
			{
				BeamSlot[i] = new AddonSlot();
				BeamSlot[i].VAlign = 0.34f;
				BeamSlot[i].HAlign = 0.07f + (0.22f * i);
				BeamSlot[i].isBeam = true;
				BeamSlot[i].isArray = false;
				BeamSlot[i].slotType = i;
				BeamSlot[i].ItemRead = new Item();

				labels[i] = new UIText(i.ToString(), 0.8f);
				labels[i].HAlign = 0.11f + (0.195f * i);
				labels[i].VAlign = BeamSlot[i].VAlign + 0.175f;

				panel.Append(BeamSlot[i]);
				panel.Append(labels[i]);

				MetroidMod.Instance.Logger.Info("Congrats, the fucking thing loaded!\nSlot: " + i);
			}
			debugInfo = new UIText("Initializing.\nWait until Update(), numbnuts", 0.75f);
			debugInfo.VAlign = panel.VAlign;
			debugInfo.MarginLeft = panel.MarginLeft + panel.Width.Pixels;
			Append(debugInfo);
		}

		public override void Update(GameTime gameTime)
		{
			//In the initialization the array writes itself onto the slots
			//In order for the slots to be able to actually change the contents,
			//during the update the item slots override the array
			//I hope this is how it works otherwise I'm gonna feel pretty stupid
			if (Visible)
			{
				target = (ArmCannon)Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem;
				for (int i = 0; i < 5; ++i)
				{
					BeamSlot[i].ItemRead = target.BeamAddonAccess[i];
				}
				debugInfo.SetText("SLOT INFO:" +
								  "\nPrimary (charge): " + target.BeamAddonAccess[0].Name +
								  "\nAbility (ice): " + target.BeamAddonAccess[1].Name +
								  "\nIon (wave): " + target.BeamAddonAccess[2].Name +
								  "\nSpread (spazer): " + target.BeamAddonAccess[3].Name +
								  "\nSecondary (plasma): " + target.BeamAddonAccess[4].Name +
								  "\nAmmo (ua): " + target.BeamAddonAccess[5].Name +
								  "\nHope you don't need anything else lmao");
			}
		}

		private void OnPSBClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			SoundEngine.PlaySound(SoundID.Item16);
			mp.pseudoScrewActive = !mp.pseudoScrewActive;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (pseudoScrewButton.ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
				psdText.TextColor = Color.Yellow;
			}
			else
			{
				psdText.TextColor = Color.White;
			}
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (mp.pseudoScrewActive)
			{
				pseudoScrewButton.BackgroundColor = Color.Green;
			}
			else
			{
				pseudoScrewButton.BackgroundColor = Color.Red;
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

			public override void Update(GameTime gameTime)
			{
				
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
							new ChatMessage(Main.LocalPlayer.name + " tried to do something that shouldn't be physically possible.");
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


}
