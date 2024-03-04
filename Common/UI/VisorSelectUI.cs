using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidMod.Common.Players;

namespace MetroidMod.Common.UI
{
	public class VisorSelectUI : UIState
	{
		public static bool Visible => Main.LocalPlayer.GetModPlayer<MPlayer>().ShouldShowVisorUI;

		private VisorSelectPanel visorSelectPanel;
		public override void OnInitialize()
		{
			visorSelectPanel = new VisorSelectPanel();
			visorSelectPanel.Initialize();

			Append(visorSelectPanel);
		}
	}

	public class VisorSelectPanel : UIPanel
	{
		private Texture2D BackgroundTex;
		private Texture2D CombatIcon;
		private Texture2D ScanIcon;
		private Texture2D UtilityIcon;
		private Texture2D AltVisorIcon;

		private Rectangle backgroundRect => new(
			(int)(Left.Pixels + (Width.Pixels / 2) - (BackgroundTex.Width / 2)),
			(int)(Top.Pixels + (Height.Pixels / 2) - (BackgroundTex.Height / 2) + 16),
			BackgroundTex.Width,
			BackgroundTex.Height
		);
		private Rectangle combatRect => new(
			(int)(Left.Pixels + (Width.Pixels / 2) - (CombatIcon.Width / 2)),
			(int)(Top.Pixels + (Height.Pixels / 2) - (CombatIcon.Height / 2)),
			CombatIcon.Width,
			CombatIcon.Height
		);
		private Rectangle scanRect => new(
			(int)(Left.Pixels + (Width.Pixels / 2) - (ScanIcon.Width / 2)),
			(int)(Top.Pixels + (Height.Pixels / 2) - (ScanIcon.Height / 2) - 77),
			ScanIcon.Width,
			ScanIcon.Height
		);
		private Rectangle utilRect => new(
			(int)(Left.Pixels + (Width.Pixels / 2) - (UtilityIcon.Width / 2) - 75),
			(int)(Top.Pixels + (Height.Pixels / 2) - (UtilityIcon.Height / 2) + 22),
			UtilityIcon.Width,
			UtilityIcon.Height
		);
		private Rectangle altRect => new(
			(int)(Left.Pixels + (Width.Pixels / 2) - (AltVisorIcon.Width / 2) + 75),
			(int)(Top.Pixels + (Height.Pixels / 2) - (AltVisorIcon.Height / 2) + 22),
			AltVisorIcon.Width,
			AltVisorIcon.Height
		);
		public override void OnInitialize()
		{
			SetPadding(0);
			Width.Pixels = 100;
			Height.Pixels = 100;
			BackgroundTex = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/VisorSelectUI", AssetRequestMode.ImmediateLoad).Value;
			CombatIcon = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/CombatVisorIcon", AssetRequestMode.ImmediateLoad).Value;
			//base.OnInitialize();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			spriteBatch.Draw(BackgroundTex, backgroundRect, mp.HUDColor);
			spriteBatch.Draw(CombatIcon, combatRect, mp.HUDColor);
			if (ScanIcon != null)
			{
				spriteBatch.Draw(ScanIcon, scanRect, mp.HUDColor);
			}
			if (UtilityIcon != null)
			{
				spriteBatch.Draw(UtilityIcon, utilRect, mp.HUDColor);
			}
			if (AltVisorIcon != null)
			{
				spriteBatch.Draw(AltVisorIcon, altRect, mp.HUDColor);
			}
		}

		public override void OnActivate()
		{
			Left.Pixels = Main.mouseX - (Width.Pixels / 2);
			Top.Pixels = Main.mouseY - (Height.Pixels / 2);
			if (!Main.LocalPlayer.TryGetModPlayer(out MPlayer mp)) { return; }
			ModSuitAddon[] msa = MPlayer.GetVisorAddons(Main.LocalPlayer);
			Asset<Texture2D> tex;
			if (msa[0] != null && ModContent.RequestIfExists(msa[0].VisorSelectIcon, out tex, AssetRequestMode.ImmediateLoad)) { ScanIcon = tex.Value; } else { ScanIcon = null; }
			if (msa[1] != null && ModContent.RequestIfExists(msa[1].VisorSelectIcon, out tex, AssetRequestMode.ImmediateLoad)) { UtilityIcon = tex.Value; } else { UtilityIcon = null; }
			if (msa[2] != null && ModContent.RequestIfExists(msa[2].VisorSelectIcon, out tex, AssetRequestMode.ImmediateLoad)) { AltVisorIcon = tex.Value; } else { AltVisorIcon = null; }
		}

		public override void OnDeactivate()
		{
			if (!Main.LocalPlayer.TryGetModPlayer(out MPlayer mp)) { return; }
			Vector2 center = new(Left.Pixels + (Width.Pixels / 2), Top.Pixels + (Height.Pixels / 2));

			int oldVisor = mp.VisorInUse;

			if (ContainsPoint(new Vector2(Main.mouseX, Main.mouseY))) // Center
			{
				mp.VisorInUse = -1;
			}
			else
			{
				ModSuitAddon[] msa = MPlayer.GetVisorAddons(Main.LocalPlayer);

				float rot = (float)Math.Atan2(center.Y - Main.mouseY, center.X - Main.mouseX);
				if (rot > -Math.PI / 2 && rot < Math.Atan2(45, 45)) // Bottom left hand area
				{
					mp.VisorInUse = msa[1] != null ? msa[1].Type : -1;
				}
				else if (rot < -Math.PI / 2 || rot > Math.Atan2(45, -45)) // Bottom right hand area
				{
					mp.VisorInUse = msa[2] != null ? msa[2].Type : -1;
				}
				else if (rot > Math.Atan2(81, 186)) // Top area
				{
					mp.VisorInUse = msa[0] != null ? msa[0].Type : -1;
				}
			}
			if (mp.VisorInUse != oldVisor) { SoundEngine.PlaySound(mp.VisorInUse == -1 ? Sounds.UI.SwitchVisor2 : Sounds.UI.SwitchVisor); }
			MetroidMod.Instance.Logger.Debug($"Switched visor to Suit Addon type: {mp.VisorInUse}");
		}
	}
}
