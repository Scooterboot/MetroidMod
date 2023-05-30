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

using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Armors;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Default;
using MetroidMod.ID;

namespace MetroidMod.Common.UI
{
	public class BeamChangeUI : UIState
	{
		public static bool Visible => Main.LocalPlayer.GetModPlayer<MPlayer>().beamChangeActive == true && Main.playerInventory && Main.LocalPlayer.inventory[MetroidMod.Instance.selectedItem].type == ModContent.ItemType<PowerBeam>();

		public BeamChangePanel panel;
		public override void OnInitialize()
		{
			panel = new BeamChangePanel();
			panel.Initialize();

			Append(panel);
		}
	}

	public class BeamChangePanel : DragableUIPanel
	{
		private Asset<Texture2D> panelTexture;

		public BeamUIItemBox[] addonSlots;

		public UIText[] textSlots;

		public UIText[] BeamInfoSlots;

		public UIImageButton OpenReserveMenuButton;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[10]
		{
			new Vector2(100, 0), //32,334
			new Vector2(81, 59), //174, 334
			new Vector2(31, 95), //98,174
			new Vector2(-31, 95), //98,94
			new Vector2(-81, 59), //32,94
			new Vector2(-100, 0), //174,94
			new Vector2(-81, -59), //32,174
			new Vector2(-31, -95), //174,174
			new Vector2(31, -95), //98,254
			new Vector2(81, -59) //32,254
		};
		public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/Side", AssetRequestMode.ImmediateLoad); //the "background.stupid"
			SetPadding(0);
			Top.Pixels = Main.screenHeight / 2;
			Left.Pixels = Main.screenWidth / 2;
			Width.Pixels = panelTexture.Width();
			Height.Pixels = panelTexture.Height();

			addonSlots = new BeamUIItemBox[10];
			for (int i = 0; i < BeamChangeSlotID.Count; ++i)
			{
				addonSlots[i] = new BeamUIItemBox();
				addonSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				addonSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				addonSlots[i].addonSlotType = i;
				addonSlots[i].SetCondition();

				Append(addonSlots[i]);
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture.Value, DrawRectangle, Color.White);
		}
	}
	public class BeamUIItemBox : UIPanel
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
			//OnClick += ItemBoxClick;
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
}
