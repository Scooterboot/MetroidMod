using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.Items.Weapons;

namespace MetroidModPorted.Common.UI
{
	public class AddonsUI : UIState
	{
		private SuitUI suitUI;
		public override void OnInitialize()
		{
			Main.hidePlayerCraftingMenu = true;
			suitUI = new SuitUI();
			suitUI.SetPadding(0);
			suitUI.Top.Pixels = Main.instance.invBottom + 10;
			suitUI.Left.Pixels = 65;

			Append(suitUI);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			suitUI.Left.Pixels = 160;
			suitUI.Top.Pixels = 260;
			suitUI.Width.Pixels = 256;
			suitUI.Height.Pixels = 164;
			if (Main.LocalPlayer.chest != -1)// || Main.npcShop != 0)
			{
				Top.Pixels += 170;
			}
			suitUI.Recalculate();
		}
	}
	public class SuitUI : UIPanel
	{
		private Texture2D panelTexture;
		//public Rectangle DrawRectangle => new Rectangle((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);
		public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/PowerBeam_Border").Value;

			SetPadding(0);
			//Left.Pixels = 160;
			//Top.Pixels = 260;
			Width.Pixels = panelTexture.Width;
			Height.Pixels = panelTexture.Height;
			
			/*beamSlots = new PowerBeamItemBox[MetroidModPorted.beamSlotAmount];
			for (int i = 0; i < MetroidModPorted.beamSlotAmount; ++i)
			{
				beamSlots[i] = new PowerBeamItemBox();
				beamSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				beamSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				beamSlots[i].addonSlotType = i;
				beamSlots[i].SetCondition();

				Append(beamSlots[i]);
			}*/

			//Append(new PowerBeamFrame());
			//Append(new PowerBeamLines());
		}
		/*public override void Update(GameTime gameTime)
		{
			Left.Pixels = 160;
			Top.Pixels = 260;
			if (Main.LocalPlayer.chest != -1)// || Main.npcShop != 0)
			{
				Top.Pixels += 170;
			}
			Recalculate();
		}*/

		/*protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			//spriteBatch.Draw(panelTexture, new Rectangle((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels), panelTexture.Bounds, Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0f);
		}*/
	}
	public class SuitItemBox : UIPanel
	{
		private Texture2D itemBoxTexture;

		public Condition condition;

		public int addonSlotType;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			itemBoxTexture = ModContent.Request<Texture2D>("MetroidModPorted/Assets/Textures/UI/ItemBox").Value;

			Width.Pixels = itemBoxTexture.Width; Height.Pixels = itemBoxTexture.Height;
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
					MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					ModBeam mBeam = ((BeamItem)addonItem.ModItem).modBeam;
					return addonItem.type <= ItemID.None || mBeam.AddonSlot == addonSlotType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidModPorted.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[(MetroidModPorted.Instance).selectedItem].ModItem as PowerBeam;

			if (powerBeamTarget.BeamMods[addonSlotType] != null && !powerBeamTarget.BeamMods[addonSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = powerBeamTarget.BeamMods[addonSlotType].Clone();

					powerBeamTarget.BeamMods[addonSlotType].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = powerBeamTarget.BeamMods[addonSlotType].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					powerBeamTarget.BeamMods[addonSlotType] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					powerBeamTarget.BeamMods[addonSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].ModItem as PowerBeam;

			spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

			// Item drawing.
			if (powerBeamTarget.BeamMods[addonSlotType].IsAir) { return; }

			Color itemColor = powerBeamTarget.BeamMods[addonSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[powerBeamTarget.BeamMods[addonSlotType].type].Value;
			CalculatedStyle innerDimensions = base.GetDimensions();

			if (base.IsMouseHovering)
			{
				Main.hoverItemName = powerBeamTarget.BeamMods[addonSlotType].Name;
				Main.HoverItem = powerBeamTarget.BeamMods[addonSlotType].Clone();
			}

			Rectangle frame = Main.itemAnimations[powerBeamTarget.BeamMods[addonSlotType].type] != null
						? Main.itemAnimations[powerBeamTarget.BeamMods[addonSlotType].type].GetFrame(itemTexture)
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

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, powerBeamTarget.BeamMods[addonSlotType].type);

			Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (powerBeamTarget.BeamMods[addonSlotType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
		}
	}
}
