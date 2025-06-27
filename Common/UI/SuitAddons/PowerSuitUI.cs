//using System;
//using MetroidMod.Common.Configs;
//using MetroidMod.Common.Players;
//using MetroidMod.Content.Items.Armors;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ReLogic.Content;
//using Terraria;
//using Terraria.Audio;
//using Terraria.GameContent;
//using Terraria.GameContent.UI.Elements;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.UI;

//namespace MetroidMod.Common.UI.SuitAddons
//{
//	public class PowerSuitUI : UIState
//	{
//		public static bool Visible => Main.playerInventory && Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) && mp.ShouldShowArmorUI && mp.SuitAddonUIState == SuitAddonUIState.PowerSuit && Main.EquipPage == 0;

//		public PowerSuitPanel panel;

//		public override void OnInitialize()
//		{
//			panel = new PowerSuitPanel();
//			panel.Initialize();

//			Append(panel);
//		}
//	}

//	public class PowerSuitPanel : DragableUIPanel
//	{
//		private Asset<Texture2D> PanelTexture;

//		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

//		public Vector2[] breastItemBoxPositionValues = new Vector2[4]
//		{
//			new Vector2(26, 88), // Primary
//			new Vector2(196, 88), // Barrier
//			new Vector2(36, 160), // Energy
//			new Vector2(186, 160) // Primary
//		};
//		public Vector2[] helmItemBoxPositionValues = new Vector2[3]
//		{
//			new Vector2(50, 16), // Scan
//			new Vector2(110, 16), // Alt
//			new Vector2(170, 16), // Utility
//		};
//		public BreastplateUIItemBox[] breastAddonSlots;
//		public HelmetUIItemBox[] helmAddonSlots;

//		public override void OnInitialize()
//		{
//			PanelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerSuit_Border", AssetRequestMode.ImmediateLoad);

//			SetPadding(0);
//			Left.Pixels = Main.screenWidth - PanelTexture.Width() - (Main.netMode == NetmodeID.MultiplayerClient ? 290 : 250);
//			Top.Pixels = 210;
//			Width.Pixels = PanelTexture.Width();
//			Height.Pixels = PanelTexture.Height();

//			//Append(new SuitUIBackground());
//			Append(new SuitUIFrame());
//			Append(new SuitUILines());

//			breastAddonSlots = new BreastplateUIItemBox[4];
//			for (int i = 0; i < breastAddonSlots.Length; i++)
//			{
//				breastAddonSlots[i] = new BreastplateUIItemBox();
//				breastAddonSlots[i].Top.Pixels = breastItemBoxPositionValues[i].Y;
//				breastAddonSlots[i].Left.Pixels = breastItemBoxPositionValues[i].X;
//				breastAddonSlots[i].addonSlotType = i;
//				breastAddonSlots[i].SetCondition();

//				Append(breastAddonSlots[i]);
//			}
//			helmAddonSlots = new HelmetUIItemBox[3];
//			for (int i = 0; i < helmAddonSlots.Length; i++)
//			{
//				helmAddonSlots[i] = new HelmetUIItemBox();
//				helmAddonSlots[i].Top.Pixels = helmItemBoxPositionValues[i].Y;
//				helmAddonSlots[i].Left.Pixels = helmItemBoxPositionValues[i].X;
//				helmAddonSlots[i].addonSlotType = i;
//				helmAddonSlots[i].SetCondition();

//				Append(helmAddonSlots[i]);
//			}
//		}

//		public override void Update(GameTime gameTime)
//		{
//			enabled = MConfigClient.Instance.BreastplateAddons.enabled;
//			if (IsMouseHovering)
//			{
//				Main.LocalPlayer.mouseInterface = true;
//			}
//			if (!enabled && MConfigClient.Instance.BreastplateAddons.auto)
//			{
//				Left.Pixels = Main.screenWidth - Width.Pixels - (Main.netMode == NetmodeID.MultiplayerClient ? 290 : 250);
//				Top.Pixels = 210;
//				if (!Main.mapFullscreen && Main.mapStyle == 1)
//				{
//					Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
//				}
//			}

//			base.Update(gameTime);
//		}

//		protected override void DrawSelf(SpriteBatch spriteBatch)
//		{
//			spriteBatch.Draw(PanelTexture.Value, DrawRectangle, Color.White);
//		}
//	}

//	/*
//	 * The classes in the following section do not have any functionality besides visual aesthetics.
//	 */
//	public class SuitUIBackground : UIPanel
//	{
//		private Asset<Texture2D> FrameTexture;

//		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

//		public override void OnInitialize()
//		{
//			FrameTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerSuit_Background", AssetRequestMode.ImmediateLoad);

//			Width.Pixels = FrameTexture.Width();
//			Height.Pixels = FrameTexture.Height();

//			// Hardcoded position values.
//			Top.Pixels = 0;
//			Left.Pixels = 0;
//		}

//		protected override void DrawSelf(SpriteBatch spriteBatch)
//		{
//			spriteBatch.Draw(FrameTexture.Value, DrawRectangle, Color.White);
//		}
//	}
//	public class SuitUIFrame : UIPanel
//	{
//		private Asset<Texture2D> FrameTexture;

//		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

//		public override void OnInitialize()
//		{
//			FrameTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerSuit_Frame", AssetRequestMode.ImmediateLoad);

//			Width.Pixels = FrameTexture.Width();
//			Height.Pixels = FrameTexture.Height();

//			// Hardcoded position values.
//			Top.Pixels = 120;
//			Left.Pixels = 110;
//		}

//		protected override void DrawSelf(SpriteBatch spriteBatch)
//		{
//			spriteBatch.Draw(FrameTexture.Value, DrawRectangle, Color.White);
//		}
//	}
//	public class SuitUILines : UIPanel
//	{
//		private Asset<Texture2D> LinesTexture;

//		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

//		public override void OnInitialize()
//		{
//			LinesTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/PowerSuit_Lines", AssetRequestMode.ImmediateLoad);

//			Width.Pixels = LinesTexture.Width();
//			Height.Pixels = LinesTexture.Height();

//			// Hardcoded position values.
//			Top.Pixels = 0;
//			Left.Pixels = 0;
//		}

//		protected override void DrawSelf(SpriteBatch spriteBatch)
//		{
//			spriteBatch.Draw(LinesTexture.Value, DrawRectangle, Color.White);
//		}
//	}
//}
