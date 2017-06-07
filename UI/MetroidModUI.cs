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

namespace MetroidMod
{
	public class MetroidModUI
	{
		public static int beamSlotAmount = 5;

		public bool ShowBeamUIButton = false;
        public bool BeamUIOpen = false;

		public UIButton beamButton;
        public UIObject beamUIObj;
		public UIItemSlot[] beamSlot = new UIItemSlot[beamSlotAmount];
		UILabel[] label = new UILabel[beamSlotAmount];
        public MetroidModUI()
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
				string tTip = "Slot Type: Charge";
				if(i == 1)
				{
					tTip = "Slot Type: Secondary";
				}
				if(i == 2)
				{
					tTip = "Slot Type: Utility";
				}
				if(i == 3)
				{
					tTip = "Slot Type: Primary A";
				}
				if(i == 4)
				{
					tTip = "Slot Type: Primary B";
				}
				beamSlot[i] = new UIItemSlot(new Vector2(10, 10+i*58), panel,
				delegate(Item item)
				{
					return (item.type <= 0 || (item.modItem != null && item.modItem.mod == mod /*&& item.ToolTip.ToString().Contains("Power Beam Addon") && item.ToolTip.ToString().Contains(tTip)*/));
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
				Color color = Color.White;// new Color((int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)), (int)((byte)((float)Main.mouseTextColor)));
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
}