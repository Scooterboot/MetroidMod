using System;
using MetroidMod.Common.Configs;
using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI.SuitAddons
{
	public class ReserveUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) && mp.ShouldShowArmorUI && mp.SuitAddonUIState == SuitAddonUIState.Breastplate && mp.SuitReserveTanks > 0 && Main.EquipPage == 0;

		public ReservePanel panel;

		public override void OnInitialize()
		{
			panel = new ReservePanel();
			panel.Initialize();

			Append(panel);
		}
	}

	public class ReservePanel : DragableUIPanel
	{
		private Asset<Texture2D> PanelTexture;

		private Asset<Texture2D> ModeTexture;

		private Asset<Texture2D> BarTexture;

		private Asset<Texture2D>[] NumberTextures;

		public ReserveButton modeButton;

		public ReserveButton reserveAmt;

		private bool reserveHoldingLClick;

		private bool reserveHoldingRClick;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);
		public Rectangle ModeAddRectangle => new((int)(DrawRectangle.Width * 90.0 / 216.0), (int)(DrawRectangle.Height * 6.0 / 66.0), (int)(DrawRectangle.Width * 110.0 / 216.0), (int)(DrawRectangle.Height * 10.0 / 66.0));

		public Rectangle GetNumRect(int i)
		{
			return i switch
			{
				0 => new((int)(DrawRectangle.Width * 154.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				1 => new((int)(DrawRectangle.Width * 170.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				2 => new((int)(DrawRectangle.Width * 186.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				//3 => new((int)(DrawRectangle.Width * 150.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				//4 => new((int)(DrawRectangle.Width * 166.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				//5 => new((int)(DrawRectangle.Width * 182.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				//6 => new((int)(DrawRectangle.Width * 198.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				//7 => new((int)(DrawRectangle.Width * 214.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 14.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0)),
				_ => new(),
			};
		}
		public Rectangle BarRect => new((int)(DrawRectangle.Width * 90.0 / 216.0), (int)(DrawRectangle.Height * 40.0 / 66.0), (int)(DrawRectangle.Width * 58.0 / 216.0), (int)(DrawRectangle.Height * 12.0 / 66.0));

		public Rectangle NumButtonRect => new((int)(DrawRectangle.Width * 94.0 / 216.0), (int)(DrawRectangle.Height * 38.0 / 66.0), (int)(DrawRectangle.Width * 134.0 / 216.0), (int)(DrawRectangle.Height * 14.0 / 66.0));

		public override void OnInitialize()
		{
			PanelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ReserveUI", AssetRequestMode.ImmediateLoad);
			ModeTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Reserve/MODE", AssetRequestMode.ImmediateLoad);
			BarTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Reserve/Bar", AssetRequestMode.ImmediateLoad);
			NumberTextures = new Asset<Texture2D>[10];
			for (int i = 0; i < 10; i++)
			{
				NumberTextures[i] = ModContent.Request<Texture2D>($"MetroidMod/Assets/Textures/UI/Reserve/{i}", AssetRequestMode.ImmediateLoad);
			}

			SetPadding(0);
			Left.Pixels = Main.screenWidth - PanelTexture.Width() - (Main.netMode == NetmodeID.MultiplayerClient ? 290 : 250);
			Top.Pixels = 400;
			Width.Pixels = PanelTexture.Width();
			Height.Pixels = PanelTexture.Height();

			modeButton = new();
			modeButton.Left.Pixels = ModeAddRectangle.Left;
			modeButton.Top.Pixels = ModeAddRectangle.Top;
			modeButton.Width.Pixels = ModeAddRectangle.Width;
			modeButton.Height.Pixels = ModeAddRectangle.Height;
			modeButton.OnUpdate += delegate {
				if (modeButton.IsMouseHovering)
				{
					Main.LocalPlayer.mouseInterface = true;
				}
			};
			modeButton.OnLeftClick += delegate {
				Main.LocalPlayer.MetroidPlayer().SuitReservesAuto = !Main.LocalPlayer.MetroidPlayer().SuitReservesAuto;
			};
			Append(modeButton);
			reserveAmt = new();
			reserveAmt.Left.Pixels = NumButtonRect.Left;
			reserveAmt.Top.Pixels = NumButtonRect.Top;
			reserveAmt.Width.Pixels = NumButtonRect.Width;
			reserveAmt.Height.Pixels = NumButtonRect.Height;
			reserveAmt.OnUpdate += delegate {
				if (reserveAmt.IsMouseHovering)
				{
					Main.LocalPlayer.mouseInterface = true;
				}
			};
			reserveAmt.OnLeftMouseDown += delegate { reserveHoldingLClick = true; };
			reserveAmt.OnLeftMouseUp += delegate { reserveHoldingLClick = false; };
			reserveAmt.OnRightMouseDown += delegate { reserveHoldingRClick = true; };
			reserveAmt.OnRightMouseUp += delegate { reserveHoldingRClick = false; };
			Append(reserveAmt);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			enabled = MConfigClient.Instance.Reserves.enabled;
			if (modeButton.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
				ModeTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Reserve/MODE_Hover", AssetRequestMode.ImmediateLoad);
			}
			else
			{
				ModeTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Reserve/MODE", AssetRequestMode.ImmediateLoad);
			}
			if (reserveAmt.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
				BarTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Reserve/Bar_Hover", AssetRequestMode.ImmediateLoad);
			}
			else
			{
				BarTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Reserve/Bar", AssetRequestMode.ImmediateLoad);
			}
			if (!enabled && MConfigClient.Instance.Reserves.auto)
			{
				Left.Pixels = Main.screenWidth - Width.Pixels - (Main.netMode == NetmodeID.MultiplayerClient ? 294 : 254);
				Top.Pixels = 398;
				if (!Main.mapFullscreen && Main.mapStyle == 1)
				{
					Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
				}
			}
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			if (reserveHoldingLClick)
			{
				if (mp.SuitReserves < mp.SuitReserveTanks * MConfigItems.Instance.reserveTankStoreCount && mp.Energy >= (mp.SuitReservesAuto ? 2 : 1))
				{
					mp.SuitReserves += 1;
					mp.Energy -= 1;
					mp.drainingReserves = false;
				}
			}
			if (reserveHoldingRClick)
			{
				if (mp.SuitReserves >= 1 && mp.Energy <= mp.MaxEnergy - 1)
				{
					mp.SuitReserves -= 1;
					mp.Energy += 1;
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			spriteBatch.Draw(PanelTexture.Value, DrawRectangle, Color.White);
			spriteBatch.Draw(ModeTexture.Value, new Rectangle(DrawRectangle.Left + ModeAddRectangle.Left, DrawRectangle.Top + ModeAddRectangle.Top, ModeAddRectangle.Width, ModeAddRectangle.Height), new(0, mp.SuitReservesAuto ? ModeTexture.Value.Height / 2 : 0, ModeTexture.Value.Width, ModeTexture.Value.Height / 2), Color.White);

			// number drawing
			int tanks = mp.SuitReserveTanks * MConfigItems.Instance.reserveTankStoreCount;
			int filled = mp.SuitReserves;
			int bars = Math.Min(mp.SuitReserveTanks, 4);

			// bar drawing
			Rectangle bRectFrame = new Rectangle(0, 0, 16 + (bars - 1) * 14, 12);

			Rectangle bRectFill = BarRect;
			bRectFill.X += 1;
			bRectFill.Width = (int)((float)filled / tanks * (bRectFrame.Width - 2));
			spriteBatch.Draw(
				BarTexture.Value,
				new Rectangle(DrawRectangle.Left + bRectFill.Left, DrawRectangle.Top + bRectFill.Top, bRectFill.Width, bRectFill.Height),
				new Rectangle?(new Rectangle(0, 12, 58, 12)),
				Main.MouseTextColorReal
				);

			spriteBatch.Draw(
				BarTexture.Value,
				new Rectangle(DrawRectangle.Left + BarRect.Left, DrawRectangle.Top + BarRect.Top, bRectFrame.Width, BarRect.Height),
				new Rectangle?(bRectFrame),
				Main.MouseTextColorReal
				);

			// make leading zeros
			string e = $"{filled:D3}";
			for (int i = 0; i < e.Length; i++)
			{
				Rectangle rect = GetNumRect(i);
				spriteBatch.Draw(
					NumberTextures[int.Parse(e[i].ToString())].Value,
					new Rectangle(DrawRectangle.Left + rect.Left, DrawRectangle.Top + rect.Top, rect.Width, rect.Height),
					Color.White
					);
			}

			//modeButton.SetText($"Mode: {(mp.SuitReservesAuto ? "Auto" : "Manual")}");
			//reserveAmt.SetText($"{mp.SuitReserves}/{mp.SuitReserveTanks * MConfigItems.Instance.reserveTankStoreCount}");
			//spriteBatch.Draw(tex.Value, new Vector2(reserveBars.Left.Pixels + Left.Pixels, reserveBars.Top.Pixels + Top.Pixels), new((int)reserveBars.Left.Pixels + (int)Left.Pixels, (int)reserveBars.Top.Pixels + (int)Top.Pixels, (int)((float)tex.Width() * ((float)mp.SuitReserves / 400f)), tex.Height()), Color.White);
		}
	}
	public class ReserveButton : UIPanel
	{
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			// lol don't draw self
		}
	}
}
