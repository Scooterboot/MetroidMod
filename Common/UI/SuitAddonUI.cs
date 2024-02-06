using System;
using MetroidMod.Common.Configs;
using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	public class SuitAddonUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.GetModPlayer<MPlayer>().ShouldShowArmorUI && Main.EquipPage == 0;

		SuitAddonPanel suitAddonPanel;

		public override void OnInitialize()
		{
			suitAddonPanel = new SuitAddonPanel();
			suitAddonPanel.Initialize();

			this.Append(suitAddonPanel);
		}
	}

	public class SuitAddonPanel : DragableUIPanel
	{
		Texture2D buttonTex, buttonTex_Hover, buttonTex_Click;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			buttonTex = ModContent.Request<Texture2D>($"{nameof(MetroidMod)}/Assets/Textures/Buttons/SuitAddonUIButton", AssetRequestMode.ImmediateLoad).Value;
			buttonTex_Hover = ModContent.Request<Texture2D>($"{nameof(MetroidMod)}/Assets/Textures/Buttons/SuitAddonUIButton_Hover", AssetRequestMode.ImmediateLoad).Value;
			buttonTex_Click = ModContent.Request<Texture2D>($"{nameof(MetroidMod)}/Assets/Textures/Buttons/SuitAddonUIButton_Click", AssetRequestMode.ImmediateLoad).Value;

			SetPadding(0);
			Width.Pixels = buttonTex.Width;
			Height.Pixels = buttonTex.Height;
			this.Left.Pixels = Main.screenWidth - Width.Pixels - (Main.netMode == NetmodeID.MultiplayerClient ? 240 : 200);
			Top.Pixels = 240;

			Width.Pixels = buttonTex.Width;
			Height.Pixels = buttonTex.Height;
			OnLeftClick += SAButtonClick;
		}

		public override void Update(GameTime gameTime)
		{
			enabled = MConfigClient.Instance.SuitMenu.enabled;
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			if (!enabled && MConfigClient.Instance.SuitMenu.auto)
			{
				this.Left.Pixels = Main.screenWidth - Width.Pixels - (Main.netMode == NetmodeID.MultiplayerClient ? 240 : 200);
				this.Top.Pixels = 240;
				if (!Main.mapFullscreen && Main.mapStyle == 1)
				{
					this.Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
				}
			}

			base.Update(gameTime);
		}

		private bool clicked = false;
		private void SAButtonClick(UIMouseEvent evt, UIElement e)
		{
			if (Main.LocalPlayer.TryGetModPlayer(out MPlayer mp))
			{
				if (mp.SuitAddonUIState < SuitAddonUIState.Reserves)
				{
					if (Main.keyState.IsKeyDown(Keys.LeftShift)) { mp.SuitAddonUIState -= 1; }
					else { mp.SuitAddonUIState += 1; }
					if (Main.keyState.IsKeyDown(Keys.LeftControl)) { mp.SuitAddonUIState = SuitAddonUIState.None; }
				}
				else
				{
					mp.SuitAddonUIState = SuitAddonUIState.None;
				}
				/*
				// i swear this is necessary ;-; - DarkSamus49
				switch (state)
				{
					default:
					case SuitAddonUIState.Reserves:
						mp.ShouldShowHelmetUI = false;
						mp.ShouldShowBreastplateUI = false;
						mp.ShouldShowGreavesUI = false;
						mp.ShouldShowReserveUI = false;
						state = SuitAddonUIState.None;
						break;
					case SuitAddonUIState.Greaves:
						mp.ShouldShowHelmetUI = false;
						mp.ShouldShowBreastplateUI = false;
						mp.ShouldShowGreavesUI = false;
						mp.ShouldShowReserveUI = true;
						state = SuitAddonUIState.Reserves;
						break;
					case SuitAddonUIState.Breastplate:
						mp.ShouldShowHelmetUI = false;
						mp.ShouldShowBreastplateUI = false;
						mp.ShouldShowGreavesUI = true;
						mp.ShouldShowReserveUI = false;
						state = SuitAddonUIState.Greaves;
						break;
					case SuitAddonUIState.Helmet:
						mp.ShouldShowHelmetUI = false;
						mp.ShouldShowBreastplateUI = true;
						mp.ShouldShowGreavesUI = false;
						mp.ShouldShowReserveUI = false;
						state = SuitAddonUIState.Breastplate;
						break;
					case SuitAddonUIState.None:
						mp.ShouldShowHelmetUI = true;
						mp.ShouldShowBreastplateUI = false;
						mp.ShouldShowGreavesUI = false;
						mp.ShouldShowReserveUI = false;
						state = SuitAddonUIState.Helmet;
						break;
				}
				Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
				*/
			}

			Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
			clicked = true;
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();

			Texture2D tex = buttonTex, texH = buttonTex_Hover, texC = buttonTex_Click;

			if (base.IsMouseHovering)
			{
				tex = texH;
				if (clicked)
				{
					tex = texC;
					clicked = false;
				}

				string smText = "Suit Addons\n" +
				"Press to show the Suit Addon Menu";

				Main.hoverItemName = smText;
			}

			sb.Draw(tex, DrawRectangle, Color.White);
		}
	}
}
