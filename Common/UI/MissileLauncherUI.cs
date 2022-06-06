using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.Items;
using MetroidModPorted.Content.Items.Weapons;

namespace MetroidModPorted.Common.UI
{
	public class MissileLauncherUI : UIState
	{
		public static bool Visible
		{
			get { return Main.playerInventory && Main.LocalPlayer.inventory[MetroidModPorted.Instance.selectedItem].type == ModContent.ItemType<MissileLauncher>(); }
		}

		MissileLauncherPanel missileLauncherPanel;

		public override void OnInitialize()
		{
			missileLauncherPanel = new MissileLauncherPanel();
			missileLauncherPanel.Initialize();

			Append(missileLauncherPanel);
		}
	}

	public class MissileLauncherPanel : DragableUIPanel
	{
		//Texture2D panelTexture;

		public MissileLauncherItemBox[] missileSlots;

		public UIText[] textSlots;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidModPorted.missileSlotAmount]
		{
			new Vector2(106, 94),
			new Vector2(38, 42),
			new Vector2(178, 42)
		};

		public override void OnInitialize()
		{
			//panelTexture = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/MissileLauncher_Border").Value;

			this.SetPadding(0);
			this.Left.Pixels = 160;
			this.Top.Pixels = 260;
			this.Width.Pixels = 256;//panelTexture.Width;
			this.Height.Pixels = 164;//panelTexture.Height;
			enabled = MetroidModPorted.DragableMissileLauncherUI;

			missileSlots = new MissileLauncherItemBox[MetroidModPorted.missileSlotAmount];
			textSlots = new UIText[MetroidModPorted.missileSlotAmount];
			for (int i = 0; i < MetroidModPorted.missileSlotAmount; ++i)
			{
				missileSlots[i] = new MissileLauncherItemBox();
				missileSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				missileSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				missileSlots[i].missileSlotType = i;
				missileSlots[i].SetCondition();

				this.Append(missileSlots[i]);

				textSlots[i] = new UIText("0", Main.screenHeight / 1080f);
				textSlots[i].SetText(MissileLauncherLoader.GetAddonSlotName(i));
				textSlots[i].Top.Pixels = itemBoxPositionValues[i].Y + 44;
				textSlots[i].Left.Pixels = itemBoxPositionValues[i].X - 22;
				textSlots[i].IsWrapped = true;
				textSlots[i].Width.Pixels = 88;
				textSlots[i].Height.Pixels = 22;

				Append(textSlots[i]);
			}

			this.Append(new MissileLauncherFrame());
			this.Append(new MissileLauncherLines());
		}

		public override void Update(GameTime gameTime)
		{
			enabled = MetroidModPorted.DragableMissileLauncherUI;
			if (!enabled)
			{
				this.Left.Pixels = 160;
				this.Top.Pixels = 260;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					this.Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}

		/*protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture, DrawRectangle, Color.White);
		}*/
	}

	public class MissileLauncherItemBox : UIPanel
	{
		Texture2D itemBoxTexture;

		public Condition condition;

		public int missileSlotType;

		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			//itemBoxTexture = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/ItemBox", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = 44; Height.Pixels = 44;//Width.Pixels = itemBoxTexture.Width; Height.Pixels = itemBoxTexture.Height;
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
				//Mod mod = MetroidModPorted.Instance;
				if (addonItem.ModItem != null)// && addonItem.ModItem.Mod == mod)
				{
					MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					return addonItem.type <= ItemID.None || mItem.missileSlotType == missileSlotType;
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == mod);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			MissileLauncher missileLauncherTarget = Main.LocalPlayer.inventory[MetroidModPorted.Instance.selectedItem].ModItem as MissileLauncher;
			if (missileLauncherTarget == null || missileLauncherTarget.MissileMods == null) { return; }

			if (missileLauncherTarget.MissileMods[missileSlotType] != null && !missileLauncherTarget.MissileMods[missileSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();

					missileLauncherTarget.MissileMods[missileSlotType].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					if (Main.mouseItem.type == missileLauncherTarget.MissileMods[missileSlotType].type)
					{
						int stack = Main.mouseItem.stack + missileLauncherTarget.MissileMods[missileSlotType].stack;

						if(missileLauncherTarget.MissileMods[missileSlotType].maxStack >= stack)
						{
							missileLauncherTarget.MissileMods[missileSlotType].stack = stack;
							Main.mouseItem.TurnToAir();
						}
						else
						{
							int stackDiff = stack - missileLauncherTarget.MissileMods[missileSlotType].maxStack;
							missileLauncherTarget.MissileMods[missileSlotType].stack = missileLauncherTarget.MissileMods[missileSlotType].maxStack;
							Main.mouseItem.stack = stackDiff;
						}
					}
					else
					{
						Item tempBoxItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();
						Item tempMouseItem = Main.mouseItem.Clone();

						missileLauncherTarget.MissileMods[missileSlotType] = tempMouseItem;
						Main.mouseItem = tempBoxItem;
					}
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					missileLauncherTarget.MissileMods[missileSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			MissileLauncher missileLauncherTarget = Main.LocalPlayer.inventory[MetroidModPorted.Instance.selectedItem].ModItem as MissileLauncher;

			//spriteBatch.Draw(itemBoxTexture, DrawRectangle, Color.White);

			// Item drawing.
			if (missileLauncherTarget.MissileMods[missileSlotType].IsAir) return;

			Color itemColor = missileLauncherTarget.MissileMods[missileSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = TextureAssets.Item[missileLauncherTarget.MissileMods[missileSlotType].type].Value;
			CalculatedStyle innerDimensions = base.GetDimensions();

			if (base.IsMouseHovering)
			{
				Main.hoverItemName = missileLauncherTarget.MissileMods[missileSlotType].Name;
				Main.HoverItem = missileLauncherTarget.MissileMods[missileSlotType].Clone();
			}

			var frame = Main.itemAnimations[missileLauncherTarget.MissileMods[missileSlotType].type] != null
						? Main.itemAnimations[missileLauncherTarget.MissileMods[missileSlotType].type].GetFrame(itemTexture)
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

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, missileLauncherTarget.MissileMods[missileSlotType].type);

			Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if(missileLauncherTarget.MissileMods[missileSlotType].stack > 1)
			{
				Utils.DrawBorderStringFourWay(
					spriteBatch,
					FontAssets.ItemStack.Value,
					Math.Min(9999, missileLauncherTarget.MissileMods[missileSlotType].stack).ToString(),
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
	public class MissileLauncherFrame : UIPanel
	{
		Texture2D missileLauncherFrame;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public override void OnInitialize()
		{
			missileLauncherFrame = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/MissileLauncher_Frame"/*, ReLogic.Content.AssetRequestMode.ImmediateLoad*/).Value;

			this.Width.Pixels = missileLauncherFrame.Width;
			this.Height.Pixels = missileLauncherFrame.Height;

			// Hardcoded position values.
			this.Top.Pixels = 52;
			this.Left.Pixels = 118;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(missileLauncherFrame, drawRectangle, Color.White);
		}
	}
	public class MissileLauncherLines : UIPanel
	{
		Texture2D missileLauncherLines;

		public Rectangle drawRectangle
		{
			get { return new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels); }
		}

		public override void OnInitialize()
		{
			missileLauncherLines = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/MissileLauncher_Lines"/*, ReLogic.Content.AssetRequestMode.ImmediateLoad*/).Value;

			this.Width.Pixels = missileLauncherLines.Width;
			this.Height.Pixels = missileLauncherLines.Height;

			// Hardcoded position values.
			this.Top.Pixels = 0;
			this.Left.Pixels = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(missileLauncherLines, drawRectangle, Color.White);
		}
	}
}
