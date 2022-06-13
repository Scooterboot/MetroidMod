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

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.Items.Armors;
using MetroidModPorted.Content.Items.Weapons;
using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted.Common.UI
{
	public class SuitAddonsUI : UIState
	{
		public static bool Visible => Main.playerInventory && Main.LocalPlayer.GetModPlayer<MPlayer>().ShouldShowArmorUI && Main.EquipPage == 0;

		private SuitAddonsPanel suitAddonsPanel;

		public Vector2[] itemBoxBreastplatePositionValues = new Vector2[SuitAddonSlotID.Misc_Attack + 1]
		{
			new Vector2(32, 334),
			new Vector2(174, 334),
			new Vector2(98, 174),
			new Vector2(98, 94),
			new Vector2(32, 94),
			new Vector2(174, 94),
			new Vector2(32, 174),
			new Vector2(174, 174)
		};
		public Vector2[] itemBoxGreavesPositionValues = new Vector2[SuitAddonSlotID.Boots_Speed - SuitAddonSlotID.Misc_Attack]
		{
			new Vector2(98, 254),
			new Vector2(32, 254),
			new Vector2(174, 254)
		};
		public Vector2[] itemBoxHelmetPositionValues = new Vector2[SuitAddonSlotID.Visor_AltVision - SuitAddonSlotID.Boots_Speed]
		{
			new Vector2(32, 14),
			new Vector2(98, 14),
			new Vector2(174, 14)
		};
		public override void OnInitialize()
		{
			suitAddonsPanel = new SuitAddonsPanel();
			suitAddonsPanel.SetPadding(0);
			suitAddonsPanel.Top.Pixels = 300;
			suitAddonsPanel.Left.Pixels = Main.screenWidth - suitAddonsPanel.Width.Pixels - 200;

			suitAddonsPanel.addonSlots = new SuitUIItemBox[SuitAddonSlotID.Count];
			suitAddonsPanel.textSlots = new UIText[SuitAddonSlotID.Count];
			for (int i = 0; i < SuitAddonSlotID.Count; ++i)
			{
				suitAddonsPanel.addonSlots[i] = new SuitUIItemBox();
				if (i <= SuitAddonSlotID.Misc_Attack)
				{
					suitAddonsPanel.addonSlots[i].Top.Pixels = itemBoxBreastplatePositionValues[i].Y;
					suitAddonsPanel.addonSlots[i].Left.Pixels = itemBoxBreastplatePositionValues[i].X;
					suitAddonsPanel.addonSlots[i].bodyPart = 1;
					suitAddonsPanel.addonSlots[i].amtToSubtract = 0;
				}
				else if (i <= SuitAddonSlotID.Boots_Speed)
				{
					suitAddonsPanel.addonSlots[i].Top.Pixels = itemBoxGreavesPositionValues[i - SuitAddonSlotID.Boots_JumpHeight].Y;
					suitAddonsPanel.addonSlots[i].Left.Pixels = itemBoxGreavesPositionValues[i - SuitAddonSlotID.Boots_JumpHeight].X;
					suitAddonsPanel.addonSlots[i].bodyPart = 2;
					suitAddonsPanel.addonSlots[i].amtToSubtract = SuitAddonSlotID.Boots_JumpHeight;
				}
				else
				{
					suitAddonsPanel.addonSlots[i].Top.Pixels = itemBoxHelmetPositionValues[i - SuitAddonSlotID.Visor_Scan].Y;
					suitAddonsPanel.addonSlots[i].Left.Pixels = itemBoxHelmetPositionValues[i - SuitAddonSlotID.Visor_Scan].X;
					suitAddonsPanel.addonSlots[i].bodyPart = 0;
					suitAddonsPanel.addonSlots[i].amtToSubtract = SuitAddonSlotID.Visor_Scan;
				}
				suitAddonsPanel.addonSlots[i].addonSlotType = i;
				suitAddonsPanel.addonSlots[i].SetCondition();

				suitAddonsPanel.Append(suitAddonsPanel.addonSlots[i]);

				suitAddonsPanel.textSlots[i] = new UIText("0", Main.screenHeight / 1080f);
				suitAddonsPanel.textSlots[i].SetText(SuitAddonLoader.GetAddonSlotName(i));
				if (i <= SuitAddonSlotID.Misc_Attack)
				{
					suitAddonsPanel.textSlots[i].Top.Pixels = itemBoxBreastplatePositionValues[i].Y + 44;
					suitAddonsPanel.textSlots[i].Left.Pixels = itemBoxBreastplatePositionValues[i].X - 22;
				}
				else if (i <= SuitAddonSlotID.Boots_Speed)
				{
					suitAddonsPanel.textSlots[i].Top.Pixels = itemBoxGreavesPositionValues[i - SuitAddonSlotID.Boots_JumpHeight].Y + 44;
					suitAddonsPanel.textSlots[i].Left.Pixels = itemBoxGreavesPositionValues[i - SuitAddonSlotID.Boots_JumpHeight].X - 22;
				}
				else
				{
					suitAddonsPanel.textSlots[i].Top.Pixels = itemBoxHelmetPositionValues[i - SuitAddonSlotID.Visor_Scan].Y + 44;
					suitAddonsPanel.textSlots[i].Left.Pixels = itemBoxHelmetPositionValues[i - SuitAddonSlotID.Visor_Scan].X - 22;
				}
				suitAddonsPanel.textSlots[i].IsWrapped = true;
				suitAddonsPanel.textSlots[i].Width.Pixels = 88;
				suitAddonsPanel.textSlots[i].Height.Pixels = 22;

				suitAddonsPanel.Append(suitAddonsPanel.textSlots[i]);
			}

			suitAddonsPanel.SuitInfoSlots = new UIText[4];
			for (int i = 0; i < 4; i++)
			{
				suitAddonsPanel.SuitInfoSlots[i] = new UIText("0", Main.screenHeight / 1080f);
				suitAddonsPanel.SuitInfoSlots[i].Top.Pixels = 312 + (44 * (i / 2f));
				suitAddonsPanel.SuitInfoSlots[i].Left.Pixels = 98 - 22;
				suitAddonsPanel.SuitInfoSlots[i].IsWrapped = true;
				suitAddonsPanel.SuitInfoSlots[i].Width.Pixels = 88;
				suitAddonsPanel.SuitInfoSlots[i].Height.Pixels = 22;

				suitAddonsPanel.Append(suitAddonsPanel.SuitInfoSlots[i]);
			}

			//suitAddonsPanel.OpenReserveMenuButton = new UIImageButton(ModContent.Request<Texture2D>("ModLoader/UI/InfoDisplayPageArrow"));


			Append(suitAddonsPanel);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			suitAddonsPanel.SuitInfoSlots[0].SetText($"Eff: {Math.Floor(mp.EnergyDefenseEfficiency*1000)/10}%");
			suitAddonsPanel.SuitInfoSlots[1].SetText($"Res: {Math.Floor(mp.EnergyExpenseEfficiency * 1000)/10}%");
			suitAddonsPanel.SuitInfoSlots[2].SetText($"Tanks: {mp.FilledEnergyTanks}/{mp.EnergyTanks}");
			suitAddonsPanel.SuitInfoSlots[3].SetText($"Energy: {mp.EnergyRemainder}");
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

		public UIText[] textSlots;

		public UIText[] SuitInfoSlots;

		//public UIImageButton OpenReserveMenuButton;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

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
			Height.Pixels = 404;
			enabled = MetroidModPorted.DragableSenseMoveUI;
			if (!enabled)
			{
				Left.Pixels = Main.screenWidth - Width.Pixels - 200;
				Top.Pixels = 220;
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

		/// <summary>
		/// 0 for helmet, 1 for breastplate, 2 for boots.
		/// </summary>
		public int bodyPart;

		public int amtToSubtract;

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
				if (addonItem.ModItem != null)// && addonItem.ModItem.Mod == MetroidModPorted.Instance)
				{
					//MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					if (addonItem == null || !SuitAddonLoader.TryGetAddon(addonItem, out ModSuitAddon mSuitAddon)) { return false; }
					return addonItem.type <= ItemID.None || mSuitAddon.AddonSlot == addonSlotType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidModPorted.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			if (bodyPart == 0) { HelmetClick(evt, e); return; }
			if (bodyPart == 1) { BreastplateClick(evt, e); return; }
			if (bodyPart == 2) { GreavesClick(evt, e); return; }
		}
		private void HelmetClick(UIMouseEvent evt, UIElement e)
		{
			if (Main.LocalPlayer.armor[0].type != ModContent.ItemType<PowerSuitHelmet>()) { return; }
			PowerSuitHelmet target = Main.LocalPlayer.armor[0].ModItem as PowerSuitHelmet;

			if (target.SuitAddons[addonSlotType - amtToSubtract] != null && !target.SuitAddons[addonSlotType - amtToSubtract].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = target.SuitAddons[addonSlotType - amtToSubtract].Clone();

					target.SuitAddons[addonSlotType - amtToSubtract].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = target.SuitAddons[addonSlotType - amtToSubtract].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					target.SuitAddons[addonSlotType - amtToSubtract] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					target.SuitAddons[addonSlotType - amtToSubtract] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}
		private void BreastplateClick(UIMouseEvent evt, UIElement e)
		{
			if (Main.LocalPlayer.armor[1].type != ModContent.ItemType<PowerSuitBreastplate>()) { return; }
			PowerSuitBreastplate target = Main.LocalPlayer.armor[1].ModItem as PowerSuitBreastplate;

			if (target.SuitAddons[addonSlotType - amtToSubtract] != null && !target.SuitAddons[addonSlotType - amtToSubtract].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = target.SuitAddons[addonSlotType - amtToSubtract].Clone();

					target.SuitAddons[addonSlotType - amtToSubtract].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = target.SuitAddons[addonSlotType - amtToSubtract].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					target.SuitAddons[addonSlotType - amtToSubtract] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					target.SuitAddons[addonSlotType - amtToSubtract] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}
		private void GreavesClick(UIMouseEvent evt, UIElement e)
		{
			if (Main.LocalPlayer.armor[2].type != ModContent.ItemType<PowerSuitGreaves>()) { return; }
			PowerSuitGreaves target = Main.LocalPlayer.armor[2].ModItem as PowerSuitGreaves;

			if (target.SuitAddons[addonSlotType - amtToSubtract] != null && !target.SuitAddons[addonSlotType - amtToSubtract].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = target.SuitAddons[addonSlotType - amtToSubtract].Clone();

					target.SuitAddons[addonSlotType - amtToSubtract].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = target.SuitAddons[addonSlotType - amtToSubtract].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					target.SuitAddons[addonSlotType - amtToSubtract] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					target.SuitAddons[addonSlotType - amtToSubtract] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			//MPlayer mp = Main.LocalPlayer.GetModPlayer<MPlayer>();
			//Item target = Main.LocalPlayer.inventory[MetroidModPorted.Instance.selectedItem];
			//if (target == null & target.type != ModContent.ItemType<PowerBeam>()) { return; }
			//PowerBeam powerBeamTarget = (PowerBeam)target.ModItem;

			//spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

			// Item drawing.
			//if (powerBeamTarget == null | powerBeamTarget.BeamMods[addonSlotType].IsAir) { return; }
			if (Main.LocalPlayer.armor[bodyPart].IsAir) { return; }
			Item item = GetItem(Main.LocalPlayer.armor[bodyPart]);
			if (item == null || item.IsAir) { return; }

			Color itemColor = item.GetAlpha(Color.White);
			Texture2D itemTexture = TextureAssets.Item[item.type].Value;
			CalculatedStyle innerDimensions = GetDimensions();

			if (IsMouseHovering)
			{
				Main.hoverItemName = item.Name;
				Main.HoverItem = item.Clone();
			}

			Rectangle frame = Main.itemAnimations[item.type] != null
						? Main.itemAnimations[item.type].GetFrame(itemTexture)
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

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, item.type);

			Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (item.color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																	   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
			if (item.stack > 1)
			{
				Utils.DrawBorderStringFourWay(
					spriteBatch,
					FontAssets.ItemStack.Value,
					Math.Min(9999, item.stack).ToString(),
					innerDimensions.Position().X + 10f,
					innerDimensions.Position().Y + 26f,
					Color.White,
					Color.Black,
					Vector2.Zero,
					unreflectedScale * 0.8f);
			}
		}
		private Item GetItem(Item armor)
		{
			if (armor.IsAir | armor.ModItem == null) { return null; }
			if (bodyPart == 1)
			{
				if (armor.ModItem is not PowerSuitBreastplate) { return null; }
				PowerSuitBreastplate breastplate = armor.ModItem as PowerSuitBreastplate;
				return breastplate.SuitAddons[addonSlotType - amtToSubtract];
			}
			else if (bodyPart == 2)
			{
				if (armor.ModItem is not PowerSuitGreaves) { return null; }
				PowerSuitGreaves greaves = armor.ModItem as PowerSuitGreaves;
				return greaves.SuitAddons[addonSlotType - amtToSubtract];
			}
			else if (bodyPart == 0)
			{
				if (armor.ModItem is not PowerSuitHelmet) { return null; }
				PowerSuitHelmet helmet = armor.ModItem as PowerSuitHelmet;
				return helmet.SuitAddons[addonSlotType - amtToSubtract];
			}
			return null;
		}
	}
	public class ReserveMenu : DragableUIPanel
	{
		internal static bool _visible = false;
		public static bool Visible => SuitAddonsUI.Visible && _visible;

		public UIImage[] reserveBars;

		public UIText[] textSlots;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);
	}
}
