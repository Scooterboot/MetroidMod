using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace MetroidMod.Common.UI
{
	public abstract class UIItemSlot : UIElement
	{
		public Texture2D backgroundTexture;
		internal float scale;
		internal Item item;
		internal event Func<bool> CanClick;

		public UIItemSlot(float scale = 1f)
		{
			this.scale = scale;
			item = new Item();
			backgroundTexture = TextureAssets.InventoryBack4.Value;
			Width.Set(backgroundTexture.Width * scale, 0f);
			Height.Set(backgroundTexture.Height * scale, 0f);
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			Player player = Main.LocalPlayer;

			if (player.itemAnimation == 0 && player.itemTime == 0)
			{

				if (CanClick?.Invoke() ?? true)
				{

					Utils.Swap(ref item, ref Main.mouseItem);
					if (item.type == 0 || item.stack < 1)
					{
						item = new Item();
					}

					if (Main.mouseItem.type == item.type)
					{
						Utils.Swap(ref item.favorited, ref Main.mouseItem.favorited);
						if (item.stack != item.maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
						{
							if (Main.mouseItem.stack + item.stack <= Main.mouseItem.maxStack)
							{
								item.stack += Main.mouseItem.stack;
								Main.mouseItem.stack = 0;
							}
							else
							{
								int giveAmount = Main.mouseItem.maxStack - item.stack;
								item.stack += giveAmount;
								Main.mouseItem.stack -= giveAmount;
							}
						}
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}

					if (Main.mouseItem.type > 0 || item.type > 0)
					{
						Recipe.FindRecipes();
						SoundEngine.PlaySound(SoundID.Grab);
					}
					base.MouseDown(evt);
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 position = GetInnerDimensions().Position();
			spriteBatch.Draw(backgroundTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			if (item != null && !item.IsAir)
			{
				Texture2D itemTexture = TextureAssets.Item[item.type].Value;
				Rectangle textureFrame = Main.itemAnimations[item.type]?.GetFrame(itemTexture) ?? itemTexture.Bounds;

				Color newColor = Color.White;
				float pulseScale = 1f;
				ItemSlot.GetItemLight(ref newColor, ref pulseScale, item, false);
				int height = textureFrame.Height;
				int width = textureFrame.Width;
				float drawScale = 1f;
				float availableWidth = 32; // defaultBackgroundTexture.Width * scale;
				if (width > availableWidth || height > availableWidth)
				{
					drawScale = availableWidth / (width > height ? width : height);
				}
				drawScale *= scale;
				Vector2 itemPosition = position + backgroundTexture.Size() * scale / 2f - textureFrame.Size() * drawScale / 2f;
				Vector2 itemOrigin = textureFrame.Size() * (pulseScale / 2f - 0.5f);
				if (ItemLoader.PreDrawInInventory(item, spriteBatch, itemPosition, textureFrame, item.GetAlpha(newColor),
					item.GetColor(Color.White), itemOrigin, drawScale * pulseScale))
				{
					spriteBatch.Draw(itemTexture, itemPosition, textureFrame, item.GetAlpha(newColor), 0f, itemOrigin, drawScale * pulseScale, SpriteEffects.None, 0f);
					if (item.color != Color.Transparent)
					{
						spriteBatch.Draw(itemTexture, itemPosition, textureFrame, item.GetColor(Color.White), 0f, itemOrigin, drawScale * pulseScale, SpriteEffects.None, 0f);
					}
				}
				ItemLoader.PostDrawInInventory(item, spriteBatch, itemPosition, textureFrame, item.GetAlpha(newColor),
					item.GetColor(Color.White), itemOrigin, drawScale * pulseScale);
				if (ItemID.Sets.TrapSigned[item.type])
				{
					spriteBatch.Draw(TextureAssets.Wire.Value, position + new Vector2(40f) * scale, new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
				}

				if (item.stack > 1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), position + new Vector2(10f, 26f) * scale, Color.White, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);
				}

				if (IsMouseHovering)
				{
					Main.HoverItem = item.Clone();
					Main.hoverItemName = Main.HoverItem.Name;
				}
			}
		}
	}
}
