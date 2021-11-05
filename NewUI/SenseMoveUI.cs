using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidMod.Items;
using MetroidMod.Items.armor;

namespace MetroidMod.NewUI
{
	public class SenseMoveUI : UIState
	{
		public static bool visible
		{
			get { return Main.playerInventory && Main.LocalPlayer.GetModPlayer<MPlayer>().senseMove && Main.EquipPage == 0; }
		}
		
		SenseMovePanel senseMovePanel;

		public override void OnInitialize()
		{
			senseMovePanel = new SenseMovePanel();
			senseMovePanel.Initialize();

			this.Append(senseMovePanel);
		}
	}
	
	public class SenseMovePanel : DragableUIPanel
	{
		Texture2D buttonTex, buttonTex_Hover, buttonTex_Click,
		buttonTexEnabled, buttonTexEnabled_Hover, buttonTexEnabled_Click;
		
		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}
		
		public override void OnInitialize()
		{
			buttonTex = ModContent.GetTexture("MetroidMod/Textures/Buttons/SenseMove_UIButton");
			buttonTex_Hover = ModContent.GetTexture("MetroidMod/Textures/Buttons/SenseMove_UIButton_Hover");
			buttonTex_Click = ModContent.GetTexture("MetroidMod/Textures/Buttons/SenseMove_UIButton_Click");
			
			buttonTexEnabled = ModContent.GetTexture("MetroidMod/Textures/Buttons/SenseMoveEnabled_UIButton");
			buttonTexEnabled_Hover = ModContent.GetTexture("MetroidMod/Textures/Buttons/SenseMoveEnabled_UIButton_Hover");
			buttonTexEnabled_Click = ModContent.GetTexture("MetroidMod/Textures/Buttons/SenseMoveEnabled_UIButton_Click");
			
			this.SetPadding(0);
			this.Width.Pixels = buttonTex.Width;
			this.Height.Pixels = buttonTex.Height;
			this.Left.Pixels = Main.screenWidth - this.Width.Pixels - 200;
			this.Top.Pixels = 300;
			enabled = MetroidMod.DragableSenseMoveUI;
			
			Width.Pixels = buttonTex.Width;
			Height.Pixels = buttonTex.Height;
			this.OnClick += SMButtonClick;
		}
		
		public override void Update(GameTime gameTime)
		{
			enabled = MetroidMod.DragableSenseMoveUI;
			if(base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			if (!enabled)
			{
				this.Left.Pixels = Main.screenWidth - this.Width.Pixels - 200;
				this.Top.Pixels = 300;
				if (!Main.mapFullscreen && Main.mapStyle == 1)
				{
					this.Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
				}
			}

			base.Update(gameTime);
		}
		
		bool clicked = false;
		private void SMButtonClick(UIMouseEvent evt, UIElement e)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			
			mp.senseMoveEnabled = !mp.senseMoveEnabled;
			Main.PlaySound(12);
			clicked = true;
		}
		
		protected override void DrawSelf(SpriteBatch sb)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			
			Texture2D tex = buttonTex, texH = buttonTex_Hover, texC = buttonTex_Click;
			if(mp.senseMoveEnabled)
			{
				tex = buttonTexEnabled;
				texH = buttonTexEnabled_Hover;
				texC = buttonTexEnabled_Click;
			}
			
			if(base.IsMouseHovering)
			{
				tex = texH;
				if(clicked)
				{
					tex = texC;
					clicked = false;
				}
				
				string smText = "Sense Move: Disabled";
				if(mp.senseMoveEnabled)
				{
					smText = "Sense Move: Enabled";
				}
				smText = smText + "\n" +
				"When enabled, double tap left or right to dodge\n" +
				"Gain 1/3 second of invulnerability while dodging\n" +
				"1 second cooldown\n" +
				"Only useable when grounded unless Space Jump is equipped";
				
				Main.hoverItemName = smText;
			}
			
			sb.Draw(tex, drawRectangle, Color.White);
		}
	}
}