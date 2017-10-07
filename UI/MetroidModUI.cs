using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.UI;
using ReLogic.Graphics;
using ReLogic;

using MetroidMod.Items;

namespace MetroidMod
{
	public class BeamUI
	{
		public static int beamSlotAmount = 5;

		public bool ShowBeamUIButton = false;
        public bool BeamUIOpen = false;

		public UIButton beamButton;
        public UIObject beamUIObj;
		public UIItemSlot[] beamSlot = new UIItemSlot[beamSlotAmount];
		UILabel[] label = new UILabel[beamSlotAmount];
		public BeamUI()
        {
            Mod mod = ModLoader.GetMod(UIParameters.MODNAME);
			
			Player P = Main.player[Main.myPlayer];
			
			beamButton = new UIButton(new Vector2(250, 292), new Vector2(44, 44), delegate()
            {
                BeamUIOpen = !BeamUIOpen;
            }, null,
			mod.GetTexture("Textures/Buttons/BeamUIButton"),
			mod.GetTexture("Textures/Buttons/BeamUIButton_Hover"),
			mod.GetTexture("Textures/Buttons/BeamUIButton_Click"));
			
			UIPanel panel = new UIPanel(new Vector2(250,350), new Vector2(174, 310), null);
			
			for(int i = 0; i < beamSlot.Length; i++)
			{
				int k = i;
				beamSlot[i] = new UIItemSlot(new Vector2(10, 10+i*58), panel,
				delegate(Item item)
				{
					if(item.modItem != null && item.modItem.mod == mod)
					{
						MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
						return (item.type <= 0 || mItem.addonSlotType == k);
					}
					return (item.type <= 0 || (item.modItem != null && item.modItem.mod == mod));
				});
			}

			for(int i = 0; i < label.Length; i++)
			{
				string slotText = "Charge";
				if(i == 1)
				{
					slotText = "Secondary";
				}
				if(i == 2)
				{
					slotText = "Utility";
				}
				if(i == 3)
				{
					slotText = "Primary A";
				}
				if(i == 4)
				{
					slotText = "Primary B";
				}
				Color color = Color.White;
				label[i] = new UILabel(new Vector2(68, 24+i*58), Main.fontMouseText, new Vector2(200, 52), color, Color.Black, delegate()
				{
					return slotText;
				}, panel);
			}
			
			for(int i = 0; i < beamSlot.Length; i++)
			{
				panel.children.Add(beamSlot[i]);
			}
            for(int i = 0; i < label.Length; i++)
			{
				panel.children.Add(label[i]);
			}

            beamUIObj = panel;
        }
		bool labelHide = false;
		float labelAlpha = 1f;
        public void Draw(SpriteBatch sb)
        {
			if(Main.playerInventory && Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
			{
				beamButton.Draw(sb);
				if(BeamUIOpen)
				{
					beamUIObj.Draw(sb);
					for(int i = 0; i < beamSlotAmount; i++)
					{
						beamSlot[i].DrawItemText();

						label[i].borderColor.A = (byte)(255f*labelAlpha);
						label[i].color = new Color((int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)));
						
						if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(beamSlot[i].rectangle))
						{
							labelHide = true;
						}
					}

					if (labelHide)
					{
						labelAlpha -= 0.1f;
						if (labelAlpha < 0f)
						{
							labelAlpha = 0f;
						}
					}
					else
					{
						labelAlpha += 0.025f;
						if (labelAlpha > 1f)
						{
							labelAlpha = 1f;
						}
					}
					labelHide = false;
				}
				else
				{
					labelAlpha = 1f;
					labelHide = false;
				}
			}
			else
			{
				BeamUIOpen = false;
				labelAlpha = 1f;
				labelHide = false;
			}
        }
	}
	
	public class MissileUI
	{
		public static int missileSlotAmount = 2;

		public bool ShowMissileUIButton = false;
        public bool MissileUIOpen = false;

		public UIButton missileButton;
        public UIObject missileUIObj;
		public UIItemSlot[] missileSlot = new UIItemSlot[missileSlotAmount];
		UILabel[] label = new UILabel[missileSlotAmount];
		public UIItemSlot expansionSlot;
		UILabel expansionLabel;
		public MissileUI()
        {
            Mod mod = ModLoader.GetMod(UIParameters.MODNAME);
			
			Player P = Main.player[Main.myPlayer];
			
			missileButton = new UIButton(new Vector2(250, 292), new Vector2(44, 44), delegate()
            {
                MissileUIOpen = !MissileUIOpen;
            }, null,
			mod.GetTexture("Textures/Buttons/MissileUIButton"),
			mod.GetTexture("Textures/Buttons/MissileUIButton_Hover"),
			mod.GetTexture("Textures/Buttons/MissileUIButton_Click"));
			
			UIPanel panel = new UIPanel(new Vector2(250,350), new Vector2(220, 210), null);
			
			for(int i = 0; i < missileSlot.Length; i++)
			{
				int k = i;
				missileSlot[i] = new UIItemSlot(new Vector2(10, 10+i*58), panel,
				delegate(Item item)
				{
					if(item.modItem != null && item.modItem.mod == mod)
					{
						MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
						return (item.type <= 0 || mItem.missileSlotType == k);
					}
					return (item.type <= 0 || (item.modItem != null && item.modItem.mod == mod));
				});
			}

			for(int i = 0; i < label.Length; i++)
			{
				string slotText = "Charge";
				if(i == 1)
				{
					slotText = "Primary";
				}
				label[i] = new UILabel(new Vector2(68, 24+i*58), Main.fontMouseText, new Vector2(200, 52), Color.White, Color.Black, delegate()
				{
					return slotText;
				}, panel);
			}
			
			expansionSlot = new UIItemSlot(new Vector2(10, 150), panel,
			delegate(Item item)
			{
				return (item.type <= 0 || item.type == mod.ItemType("MissileExpansion"));
			});
			
			expansionLabel = new UILabel(new Vector2(68, 164), Main.fontMouseText, new Vector2(200, 52), Color.White, Color.Black, delegate()
			{
				return "Missile Expansion";
			}, panel);
			
			for(int i = 0; i < missileSlot.Length; i++)
			{
				panel.children.Add(missileSlot[i]);
			}
            for(int i = 0; i < label.Length; i++)
			{
				panel.children.Add(label[i]);
			}
			panel.children.Add(expansionSlot);
			panel.children.Add(expansionLabel);

            missileUIObj = panel;
        }
		bool labelHide = false;
		float labelAlpha = 1f;
        public void Draw(SpriteBatch sb)
        {
			if(Main.playerInventory && Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
			{
				missileButton.Draw(sb);
				if(MissileUIOpen)
				{
					missileUIObj.Draw(sb);
					for(int i = 0; i < missileSlotAmount; i++)
					{
						missileSlot[i].DrawItemText();

						label[i].borderColor.A = (byte)(255f*labelAlpha);
						label[i].color = new Color((int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)));
						
						if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(missileSlot[i].rectangle))
						{
							labelHide = true;
						}
					}
					
					expansionSlot.DrawItemText();

					expansionLabel.borderColor.A = (byte)(255f*labelAlpha);
					expansionLabel.color = new Color((int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)));
					
					if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(expansionSlot.rectangle))
					{
						labelHide = true;
					}

					if (labelHide)
					{
						labelAlpha -= 0.1f;
						if (labelAlpha < 0f)
						{
							labelAlpha = 0f;
						}
					}
					else
					{
						labelAlpha += 0.025f;
						if (labelAlpha > 1f)
						{
							labelAlpha = 1f;
						}
					}
					labelHide = false;
				}
				else
				{
					labelAlpha = 1f;
					labelHide = false;
				}
			}
			else
			{
				MissileUIOpen = false;
				labelAlpha = 1f;
				labelHide = false;
			}
        }
	}
	public class BallUI
	{
		public static int ballSlotAmount = 5;

		public bool ShowBallUIButton = false;
        public bool BallUIOpen = false;

		public UIButton ballButton;
        public UIObject ballUIObj;
		public UIItemSlot[] ballSlot = new UIItemSlot[ballSlotAmount];
		UILabel[] label = new UILabel[ballSlotAmount];
		public BallUI()
		{
			Mod mod = ModLoader.GetMod(UIParameters.MODNAME);
			
			Player P = Main.player[Main.myPlayer];
			
			ballButton = new UIButton(new Vector2(200, 292), new Vector2(44, 44), delegate()
            {
                BallUIOpen = !BallUIOpen;
            }, null,
			mod.GetTexture("Textures/Buttons/MorphBallUIButton"),
			mod.GetTexture("Textures/Buttons/MorphBallUIButton_Hover"),
			mod.GetTexture("Textures/Buttons/MorphBallUIButton_Click"));
			
			UIPanel panel = new UIPanel(new Vector2(200,350), new Vector2(150, 310), null);
			
			for(int i = 0; i < ballSlot.Length; i++)
			{
				int k = i;
				ballSlot[i] = new UIItemSlot(new Vector2(10, 10+i*58), panel,
				delegate(Item item)
				{
					if(item.modItem != null && item.modItem.mod == mod)
					{
						MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
						return (item.type <= 0 || mItem.ballSlotType == k);
					}
					return (item.type <= 0 || (item.modItem != null && item.modItem.mod == mod));
				});
			}

			for(int i = 0; i < label.Length; i++)
			{
				string slotText = "Drill";
				if(i == 1)
				{
					slotText = "Weapon";
				}
				if(i == 2)
				{
					slotText = "Special";
				}
				if(i == 3)
				{
					slotText = "Utility";
				}
				if(i == 4)
				{
					slotText = "Boost";
				}
				Color color = Color.White;
				label[i] = new UILabel(new Vector2(68, 24+i*58), Main.fontMouseText, new Vector2(200, 52), color, Color.Black, delegate()
				{
					return slotText;
				}, panel);
			}
			
			for(int i = 0; i < ballSlot.Length; i++)
			{
				panel.children.Add(ballSlot[i]);
			}
            for(int i = 0; i < label.Length; i++)
			{
				panel.children.Add(label[i]);
			}

            ballUIObj = panel;
		}
		bool labelHide = false;
		float labelAlpha = 1f;
        public void Draw(SpriteBatch sb)
        {
			if(Main.playerInventory && Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
			{
				ballButton.Draw(sb);
				if(BallUIOpen)
				{
					ballUIObj.Draw(sb);
					for(int i = 0; i < ballSlotAmount; i++)
					{
						ballSlot[i].DrawItemText();

						label[i].borderColor.A = (byte)(255f*labelAlpha);
						label[i].color = new Color((int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)), (int)((byte)((float)Main.mouseTextColor * labelAlpha)));
						
						if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(ballSlot[i].rectangle))
						{
							labelHide = true;
						}
					}

					if (labelHide)
					{
						labelAlpha -= 0.1f;
						if (labelAlpha < 0f)
						{
							labelAlpha = 0f;
						}
					}
					else
					{
						labelAlpha += 0.025f;
						if (labelAlpha > 1f)
						{
							labelAlpha = 1f;
						}
					}
					labelHide = false;
				}
				else
				{
					labelAlpha = 1f;
					labelHide = false;
				}
			}
			else
			{
				BallUIOpen = false;
				labelAlpha = 1f;
				labelHide = false;
			}
        }
	}
}
