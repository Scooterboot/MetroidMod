using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.GameContent.UI.Elements;

using MetroidMod.Items;
using MetroidMod.Items.weapons;
using MetroidMod.Items.accessories;

namespace MetroidMod.NewUI
{
	public class MorphBallUI : UIState
	{
		public static bool visible
		{
			get { return Main.LocalPlayer.miscEquips[3].type == ModLoader.GetMod("MetroidMod").ItemType("MorphBall") && Main.EquipPage == 2; }
		}

		MorphBallPanel morphBallPanel;

		public override void OnInitialize()
		{
			morphBallPanel = new MorphBallPanel();
			morphBallPanel.Initialize();

			this.Append(morphBallPanel);
		}
	}

	public class MorphBallPanel : DragableUIPanel
	{
		Texture2D panelTexture;

		public MorphBallItemBox[] ballSlots;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)this.Left.Pixels, (int)this.Top.Pixels, (int)this.Width.Pixels, (int)this.Height.Pixels); }
		}

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.ballSlotAmount]
		{
			new Vector2(26, 86),
			new Vector2(182, 86),
			new Vector2(184, 22),
			new Vector2(26, 22),
			new Vector2(104, 86)
		};

		public override void OnInitialize()
		{
			panelTexture = ModContent.GetTexture("MetroidMod/Textures/UI/MorphBall_Border");

			this.SetPadding(0);
			this.Width.Pixels = panelTexture.Width;
			this.Height.Pixels = panelTexture.Height;
			this.Left.Pixels = Main.screenWidth - this.Width.Pixels - 180;
			this.Top.Pixels = 240;
			enabled = ModContent.GetInstance<MConfig>().DragableMorphBallUI;

			ballSlots = new MorphBallItemBox[MetroidMod.beamSlotAmount];
			for (int i = 0; i < MetroidMod.beamSlotAmount; ++i)
			{
				ballSlots[i] = new MorphBallItemBox();
				ballSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				ballSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				ballSlots[i].morphBallSlotType = i;
				ballSlots[i].SetCondition();

				this.Append(ballSlots[i]);
			}

			this.Append(new MorphBallFrame());
			this.Append(new MorphBallLines());
		}

		public override void Update(GameTime gameTime)
		{
			enabled = ModContent.GetInstance<MConfig>().DragableMorphBallUI;
			if (!enabled)
			{
				this.Left.Pixels = Main.screenWidth - this.Width.Pixels - 180;
				this.Top.Pixels = 240;
				if (!Main.mapFullscreen && Main.mapStyle == 1)
					this.Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
			}

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture, drawRectangle, Color.White);
		}
	}

	public class MorphBallItemBox : UIPanel
	{
		Texture2D itemBoxTexture;

		public Condition condition;

		public int morphBallSlotType;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			itemBoxTexture = ModContent.GetTexture("MetroidMod/Textures/UI/ItemBox");

			Width.Pixels = itemBoxTexture.Width; Height.Pixels = itemBoxTexture.Height;
			this.OnClick += ItemBoxClick;
		}

		public override void Update(GameTime gameTime)
		{
			// Ignore mouse input.
			if (base.IsMouseHovering)
				Main.LocalPlayer.mouseInterface = true;
		}

		public void SetCondition()
		{
			this.condition = delegate (Item addonItem)
			{
				Mod mod = ModLoader.GetMod("MetroidMod");
				if (addonItem.modItem != null && addonItem.modItem.mod == mod)
				{
					MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					return (addonItem.type <= 0 || mItem.ballSlotType == this.morphBallSlotType);
				}
				return (addonItem.type <= 0 || (addonItem.modItem != null && addonItem.modItem.mod == mod));
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			MorphBall morphBallTarget = Main.LocalPlayer.miscEquips[3].modItem as MorphBall;

			if (morphBallTarget.ballMods[morphBallSlotType] != null && !morphBallTarget.ballMods[morphBallSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					Main.PlaySound(SoundID.Grab);
					Main.mouseItem = morphBallTarget.ballMods[morphBallSlotType].Clone();

					morphBallTarget.ballMods[morphBallSlotType].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					Main.PlaySound(SoundID.Grab);

					Item tempBoxItem = morphBallTarget.ballMods[morphBallSlotType].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					morphBallTarget.ballMods[morphBallSlotType] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					Main.PlaySound(SoundID.Grab);
					morphBallTarget.ballMods[morphBallSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			MorphBall morphBallTarget = Main.LocalPlayer.miscEquips[3].modItem as MorphBall;

			spriteBatch.Draw(itemBoxTexture, drawRectangle, Color.White);

			// Item drawing.
			if (morphBallTarget.ballMods[morphBallSlotType].IsAir) return;

			Color itemColor = morphBallTarget.ballMods[morphBallSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = Main.itemTexture[morphBallTarget.ballMods[morphBallSlotType].type];
			CalculatedStyle innerDimensions = base.GetDimensions();

			if (base.IsMouseHovering)
			{
				Main.hoverItemName = morphBallTarget.ballMods[morphBallSlotType].Name;
				Main.HoverItem = morphBallTarget.ballMods[morphBallSlotType].Clone();
			}

			var frame = Main.itemAnimations[morphBallTarget.ballMods[morphBallSlotType].type] != null
						? Main.itemAnimations[morphBallTarget.ballMods[morphBallSlotType].type].GetFrame(itemTexture)
						: itemTexture.Frame(1, 1, 0, 0);

			float drawScale = 1f;
			if ((float)frame.Width > innerDimensions.Width || (float)frame.Height > innerDimensions.Width)
			{
				if (frame.Width > frame.Height)
					drawScale = innerDimensions.Width / (float)frame.Width;
				else
					drawScale = innerDimensions.Width / (float)frame.Height;
			}

			var unreflectedScale = drawScale;
			var tmpcolor = Color.White;

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, morphBallTarget.ballMods[morphBallSlotType].type);

			Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (morphBallTarget.ballMods[morphBallSlotType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
		}
	}

	/*
	 * The classes in the following section do not have any functionality besides visual aesthetics.
	 */
	public class MorphBallFrame : UIPanel
	{
		Texture2D morphBallFrame;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public override void OnInitialize()
		{
			morphBallFrame = ModContent.GetTexture("MetroidMod/Textures/UI/MorphBall_Frame");

			this.Width.Pixels = morphBallFrame.Width;
			this.Height.Pixels = morphBallFrame.Height;

			// Hardcoded position values.
			this.Top.Pixels = 34;
			this.Left.Pixels = 108;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(morphBallFrame, drawRectangle, Color.White);
		}
	}
	public class MorphBallLines : UIPanel
	{
		Texture2D morphBallLines;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public override void OnInitialize()
		{
			morphBallLines = ModContent.GetTexture("MetroidMod/Textures/UI/MorphBall_Lines");

			this.Width.Pixels = morphBallLines.Width;
			this.Height.Pixels = morphBallLines.Height;

			// Hardcoded position values.
			this.Top.Pixels = 0;
			this.Left.Pixels = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(morphBallLines, drawRectangle, Color.White);
		}
	}
}
