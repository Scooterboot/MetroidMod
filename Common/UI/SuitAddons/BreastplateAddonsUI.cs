using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ID;
using Terraria.ModLoader;

using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Armors;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Default;
using MetroidMod.ID;

namespace MetroidMod.Common.UI.SuitAddons
{
	public class BreastplateAddonsUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) && mp.ShouldShowArmorUI && mp.SuitAddonUIState == SuitAddonUIState.Breastplate && Main.EquipPage == 0;

		public BreastplateAddonsPanel panel;

		public override void OnInitialize()
		{
			panel = new BreastplateAddonsPanel();
			panel.Initialize();

			Append(panel);
		}
	}

	public class BreastplateAddonsPanel : DragableUIPanel
	{
		private Asset<Texture2D> PanelTexture;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[6]
		{
			new Vector2(32, 174), // Energy
			new Vector2(174, 174), // Reserve
			new Vector2(32, 14), // Barrier
			new Vector2(174, 14), // Primary
			new Vector2(32, 94), // Hand
			new Vector2(174, 94) // Attack
		};

		public BreastplateUIItemBox[] addonSlots;

		public override void OnInitialize()
		{
			PanelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/SuitBreastplate_Border", AssetRequestMode.ImmediateLoad);

			SetPadding(0);
			Left.Pixels = Main.screenWidth - PanelTexture.Width() - (Main.netMode == NetmodeID.MultiplayerClient ? 290 : 250);
			Top.Pixels = 210;
			Width.Pixels = PanelTexture.Width();
			Height.Pixels = PanelTexture.Height();

			Append(new BreastplateAddonsFrame());
			Append(new BreastplateAddonsLines());

			addonSlots = new BreastplateUIItemBox[6];
			for (int i = 0; i < addonSlots.Length; i++)
			{
				addonSlots[i] = new BreastplateUIItemBox();
				addonSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				addonSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				addonSlots[i].addonSlotType = i;
				addonSlots[i].SetCondition();

				Append(addonSlots[i]);
			}
		}

		public override void Update(GameTime gameTime)
		{
			enabled = MConfigClient.Instance.BreastplateAddons.enabled;
			if (IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			if (!enabled && MConfigClient.Instance.BreastplateAddons.auto)
			{
				Left.Pixels = Main.screenWidth - Width.Pixels - (Main.netMode == NetmodeID.MultiplayerClient ? 290 : 250);
				Top.Pixels = 210;
				if (!Main.mapFullscreen && Main.mapStyle == 1)
				{
					Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
				}
			}

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(PanelTexture.Value, DrawRectangle, Color.White);
		}
	}
	public class BreastplateUIItemBox : UIPanel
	{
		private Asset<Texture2D> ItemBoxTexture;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public Condition condition;

		public int addonSlotType;

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			ItemBoxTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox", AssetRequestMode.ImmediateLoad);

			Width.Pixels = 44; Height.Pixels = 44;
			OnClick += ItemBoxClick;
		}

		public override void Update(GameTime gameTime)
		{
			// Ignore mouse input.
			if (IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		public void SetCondition()
		{
			condition = delegate (Item addonItem) {
				//Mod mod = ModLoader.GetMod("MetroidMod");
				if (addonItem.ModItem != null)// && addonItem.ModItem.Mod == MetroidMod.Instance)
				{
					//MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					if (addonItem == null || !SuitAddonLoader.TryGetAddon(addonItem, out ModSuitAddon mSuitAddon)) { return false; }
					return addonItem.type <= ItemID.None || mSuitAddon.AddonSlot == addonSlotType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidMod.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			if (Main.LocalPlayer.controlUseItem || Main.LocalPlayer.controlUseTile) { return; }

			if (Main.LocalPlayer.armor[1].type != ModContent.ItemType<PowerSuitBreastplate>()) { return; }
			PowerSuitBreastplate target = Main.LocalPlayer.armor[1].ModItem as PowerSuitBreastplate;

			if (target.SuitAddons[addonSlotType] != null && !target.SuitAddons[addonSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = target.SuitAddons[addonSlotType].Clone();

					target.SuitAddons[addonSlotType].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = target.SuitAddons[addonSlotType].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					target.SuitAddons[addonSlotType] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					target.SuitAddons[addonSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);

			spriteBatch.Draw(ItemBoxTexture.Value, DrawRectangle, new Color(255, 255, 255));

			// Item drawing.
			if (Main.LocalPlayer.armor[1].IsAir) { return; }
			PowerSuitBreastplate breastplate = Main.LocalPlayer.armor[1].ModItem as PowerSuitBreastplate;

			Color itemColor = breastplate.SuitAddons[addonSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = TextureAssets.Item[breastplate.SuitAddons[addonSlotType].type].Value;
			CalculatedStyle innerDimensions = GetDimensions();

			if (IsMouseHovering)
			{
				Main.hoverItemName = breastplate.SuitAddons[addonSlotType].Name;
				Main.HoverItem = breastplate.SuitAddons[addonSlotType].Clone();
			}

			Rectangle frame = Main.itemAnimations[breastplate.SuitAddons[addonSlotType].type] != null
						? Main.itemAnimations[breastplate.SuitAddons[addonSlotType].type].GetFrame(itemTexture)
						: itemTexture.Frame(1, 1, 0, 0);

			float drawScale = 1f;
			if (frame.Width > innerDimensions.Width || frame.Height > innerDimensions.Width)
			{
				if (frame.Width > frame.Height)
				{
					drawScale = innerDimensions.Width / frame.Width;
				}
				else
				{
					drawScale = innerDimensions.Width / frame.Height;
				}
			}

			float unreflectedScale = drawScale;
			Color tmpcolor = Color.White;

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, breastplate.SuitAddons[addonSlotType].type);

			Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (breastplate.SuitAddons[addonSlotType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																	   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}

			if (breastplate.SuitAddons[addonSlotType].stack > 1)
			{
				Utils.DrawBorderStringFourWay(
					spriteBatch,
					FontAssets.ItemStack.Value,
					Math.Min(9999, breastplate.SuitAddons[addonSlotType].stack).ToString(),
					innerDimensions.Position().X + 10f,
					innerDimensions.Position().Y + 26f,
					Color.White,
					Color.Black,
					Vector2.Zero,
					unreflectedScale * 0.8f);
			}
		}
	}

	/*
	 * The classes in the following section do not have any functionality besides visual aesthetics.
	 */
	public class BreastplateAddonsFrame : UIPanel
	{
		private Asset<Texture2D> FrameTexture;

		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			FrameTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/SuitBreastplate_Frame", AssetRequestMode.ImmediateLoad);

			Width.Pixels = FrameTexture.Width();
			Height.Pixels = FrameTexture.Height();

			// Hardcoded position values.
			Top.Pixels = 120;
			Left.Pixels = 110;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(FrameTexture.Value, DrawRectangle, Color.White);
		}
	}
	public class BreastplateAddonsLines : UIPanel
	{
		private Asset<Texture2D> LinesTexture;

		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			LinesTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/SuitBreastplate_Lines", AssetRequestMode.ImmediateLoad);

			Width.Pixels = LinesTexture.Width();
			Height.Pixels = LinesTexture.Height();

			// Hardcoded position values.
			Top.Pixels = 0;
			Left.Pixels = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(LinesTexture.Value, DrawRectangle, Color.White);
		}
	}
}
