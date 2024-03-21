using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	public class MissileChangeUI : UIState
	{
		public static bool Visible => Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) && mp.missileChangeActive == true && Main.LocalPlayer.inventory[mp.selectedItem].type == ModContent.ItemType<MissileLauncher>();

		public MissileChangePanel panel;
		public override void OnInitialize()
		{
			base.OnInitialize();
			panel = new MissileChangePanel();
			panel.Initialize();
			/*panel.addonSlots = new BeamUIItemBox[MetroidMod.missileChangeSlotAmount];
			for (int i = 0; i < MetroidMod.missileSlotAmount; ++i)
			{
				panel.addonSlots[i] = new BeamUIItemBox();
				panel.addonSlots[i].Top.Pixels = panel.itemBoxPositionValues[i].Y;
				panel.addonSlots[i].Left.Pixels = panel.itemBoxPositionValues[i].X;
				panel.addonSlots[i].missileChangeType = i;
				panel.addonSlots[i].SetCondition();

				panel.Append(panel.addonSlots[i]);
			}
			panel.Charge1 = new UIImageButton(ModContent.Request<Texture2D>($"{nameof(MetroidMod)}/Assets/Textures/Spiderball", AssetRequestMode.ImmediateLoad));
			panel.Charge1.Left.Pixels = 100;
			panel.Charge1.Top.Pixels = 0;*/
			/*suitAddonsPanel.OpenReserveMenuButton.OnUpdate += delegate { if (suitAddonsPanel.OpenReserveMenuButton.IsMouseHovering) { Main.LocalPlayer.mouseInterface = true; } };
			suitAddonsPanel.OpenReserveMenuButton.OnLeftClick += delegate { if (ReserveMenu._visible) { ReserveMenu._visible = false; } else { ReserveMenu._visible = true; } };*/
			Append(panel);
		}
	}

	public class MissileChangePanel : DragableUIPanel
	{
		private Asset<Texture2D> panelTexture;

		public MissileUIItemBox[] missileSlots;

		public UIText[] textSlots;

		public UIText[] MissileInfoSlots;


		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.missileChangeSlotAmount]
		{
			new Vector2(211, 115),
			new Vector2(188, 179),
			new Vector2(128, 214),
			new Vector2(61, 202),
			new Vector2(17, 149),
			new Vector2(17, 81),
			new Vector2(61, 28),
			new Vector2(128, 16),
			new Vector2(188, 51),
			new Vector2(136, 90),
			new Vector2(136, 140),
			new Vector2(86, 90),
			new Vector2(86, 140)
		};
		public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/bepisblank", AssetRequestMode.ImmediateLoad); //the "background.stupid"
			SetPadding(0);
			Top.Pixels = Main.instance.invBottom + 152;
			Left.Pixels = 160;
			Width.Pixels = panelTexture.Width();
			Height.Pixels = panelTexture.Height();

			missileSlots = new MissileUIItemBox[13];
			for (int i = 0; i < MissileChangeSlotID.Count; ++i)
			{
				missileSlots[i] = new MissileUIItemBox();
				missileSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				missileSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				missileSlots[i].missileChangeType = i;
				missileSlots[i].SetCondition();

				Append(missileSlots[i]);
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture.Value, DrawRectangle, Color.White);
		}
		public override void Update(GameTime gameTime)
		{
			Width.Pixels = panelTexture.Width();
			Height.Pixels = panelTexture.Height();
			if (Common.Systems.MSystem.SwitchKey.JustPressed)
			{
				Left.Pixels = Main.mouseX - (Width.Pixels / 2);
				Top.Pixels = Main.mouseY - (Height.Pixels / 2);
			}
			enabled = Configs.MConfigClient.Instance.MissileLauncher.enabled;
			if (!enabled)
			{
				/*if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}*/
			}

			base.Update(gameTime);
		}
	}
	public class MissileUIItemBox : UIPanel
	{
		private Texture2D itemBoxTexture;

		public Condition condition;

		public int missileChangeType;
		public int addonSlotType;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			itemBoxTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/BeamBorder", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = 44; Height.Pixels = 44;
			OnLeftClick += ItemBoxClick;
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
					MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					//if (addonItem.GetGlobalItem<MGlobalItem>().AddonType != AddonType.PowerBeam) { return false; }
					//ModBeam mBeam = ((BeamItem)addonItem.ModItem).modBeam;
					return addonItem.type <= ItemID.None || mItem.missileChangeType == missileChangeType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidMod.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			//TODO No failsafe. Should maybe be implemented?
			MissileLauncher missileTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as MissileLauncher;
			if (missileTarget == null || missileTarget.MissileChange == null) { return; }

			if (missileTarget.MissileChange[missileChangeType] != null && !missileTarget.MissileChange[missileChangeType].IsAir)
			{
				//pickup
				if (Main.mouseItem.IsAir && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = missileTarget.MissileChange[missileChangeType].Clone();

					missileTarget.MissileChange[missileChangeType].TurnToAir();
					if (Main.mouseItem.type == missileTarget.MissileMods[addonSlotType].type)
					{
						missileTarget.MissileMods[addonSlotType].TurnToAir();
					}
				}
				//activate
				if (Main.mouseItem.IsAir && !Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					missileTarget.MissileMods[addonSlotType] = missileTarget.MissileChange[missileChangeType].Clone();
				}
			}
			else if (!Main.mouseItem.IsAir || condition == null || (condition != null && condition(Main.mouseItem)))
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					//SoundEngine.PlaySound(SoundID.Grab);
					missileTarget.MissileChange[missileChangeType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			Item target = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem];
			if (target == null || target.type != ModContent.ItemType<MissileLauncher>()) { return; }
			MissileLauncher missileTarget = (MissileLauncher)target.ModItem;

			spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

			// Item drawing.
			if (missileTarget == null || missileTarget.MissileChange == null || missileTarget.MissileChange[missileChangeType].IsAir) { return; }

			Color itemColor = missileTarget.MissileChange[missileChangeType].GetAlpha(Color.White);
			Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[missileTarget.MissileChange[missileChangeType].type].Value;
			CalculatedStyle innerDimensions = GetDimensions();

			if (IsMouseHovering)
			{
				Main.hoverItemName = missileTarget.MissileChange[missileChangeType].Name;
				Main.HoverItem = missileTarget.MissileChange[missileChangeType].Clone();
			}

			Rectangle frame = Main.itemAnimations[missileTarget.MissileChange[missileChangeType].type] != null
						? Main.itemAnimations[missileTarget.MissileChange[missileChangeType].type].GetFrame(itemTexture)
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

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, missileTarget.MissileChange[missileChangeType].type);

			Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (missileTarget.MissileChange[missileChangeType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																	   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
		}
	}
}
