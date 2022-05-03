using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.Audio;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.Items.Weapons;
using MetroidModPorted.ID;

namespace MetroidModPorted.Common.UI
{
	public class SuitAddonsUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.GetModPlayer<MPlayer>().ShouldShowArmorUI && Main.EquipPage == 0;

		private SuitAddonsPanel suitAddonsPanel;
		public override void OnInitialize()
		{
			suitAddonsPanel = new SuitAddonsPanel();
			suitAddonsPanel.SetPadding(0);
			suitAddonsPanel.Top.Pixels = 300;
			suitAddonsPanel.Left.Pixels = Main.screenWidth - suitAddonsPanel.Width.Pixels - 200;

			suitAddonsPanel.addonSlots = new SuitUIItemBox[SuitAddonSlotID.Count];
			for (int i = 0; i < SuitAddonSlotID.Count; ++i)
			{
				suitAddonsPanel.addonSlots[i] = new SuitUIItemBox();
				suitAddonsPanel.addonSlots[i].Top.Pixels = suitAddonsPanel.itemBoxPositionValues[i].Y;
				suitAddonsPanel.addonSlots[i].Left.Pixels = suitAddonsPanel.itemBoxPositionValues[i].X;
				suitAddonsPanel.addonSlots[i].addonSlotType = i;
				suitAddonsPanel.addonSlots[i].SetCondition();

				suitAddonsPanel.Append(suitAddonsPanel.addonSlots[i]);
			}

			Append(suitAddonsPanel);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			/*suitAddonsPanel.Left.Pixels = Main.screenWidth - suitAddonsPanel.Width.Pixels - 200;
			suitAddonsPanel.Top.Pixels = 300;
			suitAddonsPanel.Width.Pixels = 256;
			suitAddonsPanel.Height.Pixels = 324;*/
			/*if (Main.LocalPlayer.chest != -1)// || Main.npcShop != 0)
			{
				Top.Pixels += 170;
			}*/
			suitAddonsPanel.Recalculate();
		}
	}

	public class SuitAddonsPanel : DragableUIPanel
	{
		//private Texture2D panelTexture;

		public SuitUIItemBox[] addonSlots;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[SuitAddonSlotID.Count]
		{
			new Vector2(32, 254),
			new Vector2(174, 254),
			new Vector2(98, 14),
			new Vector2(174, 14),
			new Vector2(32, 14),
			new Vector2(98, 94),
			new Vector2(32, 94),
			new Vector2(174, 94),
			new Vector2(98, 174),
			new Vector2(32, 174),
			new Vector2(174, 174)
		};

		/*public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/PowerBeam_Border").Value;

			SetPadding(0);
			Left.Pixels = 160;
			Top.Pixels = 260;
			Width.Pixels = panelTexture.Width;
			Height.Pixels = panelTexture.Height;
			enabled = MetroidModPorted.DragablePowerBeamUI;

			beamSlots = new PowerBeamItemBox[MetroidModPorted.beamSlotAmount];
			for (int i = 0; i < MetroidModPorted.beamSlotAmount; ++i)
			{
				beamSlots[i] = new PowerBeamItemBox();
				beamSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				beamSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				beamSlots[i].addonSlotType = i;
				beamSlots[i].SetCondition();

				Append(beamSlots[i]);
			}
			
			Append(new PowerBeamFrame());
			Append(new PowerBeamLines());
		}*/

		public override void Update(GameTime gameTime)
		{
			Width.Pixels = 256;
			Height.Pixels = 324;
			enabled = MetroidModPorted.DragableSenseMoveUI;
			if (!enabled)
			{
				Left.Pixels = Main.screenWidth - Width.Pixels - 200;
				Top.Pixels = 300;
				/*if (Main.LocalPlayer.chest != -1)// || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}*/
			}

			base.Update(gameTime);
		}

		/*protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture, DrawRectangle, Color.White);            
		}*/
	}
	public class SuitUIItemBox : UIPanel
	{
		//private Texture2D itemBoxTexture;

		public Condition condition;

		public int addonSlotType;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			//itemBoxTexture = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/ItemBox").Value;

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
				//Mod mod = ModLoader.GetMod("MetroidModPorted");
				if (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidModPorted.Instance)
				{
					//MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					if (addonItem.GetGlobalItem<MGlobalItem>().AddonType != AddonType.Suit) { return false; }
					ModSuitAddon mSuitAddon = ((SuitAddonItem)addonItem.ModItem).modSuitAddon;
					return addonItem.type <= ItemID.None || mSuitAddon.AddonSlot == addonSlotType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidModPorted.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			//PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[(MetroidModPorted.Instance).selectedItem].ModItem as PowerBeam;

			if (mp.SuitAddons[addonSlotType] != null && !mp.SuitAddons[addonSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = mp.SuitAddons[addonSlotType].Clone();

					mp.SuitAddons[addonSlotType].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = mp.SuitAddons[addonSlotType].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					mp.SuitAddons[addonSlotType] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					mp.SuitAddons[addonSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			//Item target = Main.LocalPlayer.inventory[MetroidModPorted.Instance.selectedItem];
			//if (target == null & target.type != ModContent.ItemType<PowerBeam>()) { return; }
			//PowerBeam powerBeamTarget = (PowerBeam)target.ModItem;

			//spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

			// Item drawing.
			//if (powerBeamTarget == null | powerBeamTarget.BeamMods[addonSlotType].IsAir) { return; }
			if (mp.SuitAddons[addonSlotType].IsAir) { return; }

			Color itemColor = mp.SuitAddons[addonSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[mp.SuitAddons[addonSlotType].type].Value;
			CalculatedStyle innerDimensions = GetDimensions();

			if (IsMouseHovering)
			{
				Main.hoverItemName = mp.SuitAddons[addonSlotType].Name;
				Main.HoverItem = mp.SuitAddons[addonSlotType].Clone();
			}

			Rectangle frame = Main.itemAnimations[mp.SuitAddons[addonSlotType].type] != null
						? Main.itemAnimations[mp.SuitAddons[addonSlotType].type].GetFrame(itemTexture)
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

			//float unreflectedScale = drawScale;
			Color tmpcolor = Color.White;

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, mp.SuitAddons[addonSlotType].type);

			Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (mp.SuitAddons[addonSlotType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																	   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
		}
	}
}
