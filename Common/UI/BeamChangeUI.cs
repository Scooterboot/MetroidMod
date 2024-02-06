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
using MetroidMod.Content.Items.Addons.Hunters;
using MetroidMod.Content.Items.Addons.V2;
using MetroidMod.Content.Items.Addons.V3;
using MetroidMod.Content.Items.Addons;

namespace MetroidMod.Common.UI
{
	public class BeamChangeUI : UIState
	{
		public static bool Visible => Main.LocalPlayer.TryGetModPlayer(out MPlayer mp) && mp.beamChangeActive == true && Main.playerInventory && Main.LocalPlayer.inventory[mp.selectedItem].type == ModContent.ItemType<PowerBeam>();

		public BeamChangePanel panel;
		public override void OnInitialize()
		{
			base.OnInitialize();
			panel = new BeamChangePanel();
			panel.Initialize();
			/*panel.addonSlots = new BeamUIItemBox[MetroidMod.beamChangeSlotAmount];
			for (int i = 0; i < MetroidMod.beamSlotAmount; ++i)
			{
				panel.addonSlots[i] = new BeamUIItemBox();
				panel.addonSlots[i].Top.Pixels = panel.itemBoxPositionValues[i].Y;
				panel.addonSlots[i].Left.Pixels = panel.itemBoxPositionValues[i].X;
				panel.addonSlots[i].beamSlotType = i;
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

	public class BeamChangePanel : DragableUIPanel
	{
		private Asset<Texture2D> panelTexture;

		public BeamUIItemBox[] addonSlots;

		public UIText[] textSlots;

		public UIText[] BeamInfoSlots;


		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.beamChangeSlotAmount]
		{
			new Vector2(211, 110),
			new Vector2(192, 169),
			new Vector2(142, 205),
			new Vector2(80, 205),
			new Vector2(30, 169),
			new Vector2(11, 110),
			new Vector2(30, 51),
			new Vector2(80, 15),
			new Vector2(142, 15),
			new Vector2(192, 51),
			new Vector2(80, 110),
			new Vector2(142, 110)
		};
		public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/bepis", AssetRequestMode.ImmediateLoad); //the "background.stupid"
			SetPadding(0);
			Top.Pixels = Main.instance.invBottom + 152;
			Left.Pixels = 160;
			Width.Pixels = panelTexture.Width();
			Height.Pixels = panelTexture.Height();

			addonSlots = new BeamUIItemBox[12];
			for (int i = 0; i < BeamChangeSlotID.Count; ++i)
			{
				addonSlots[i] = new BeamUIItemBox();
				addonSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				addonSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				addonSlots[i].beamSlotType = i;
				addonSlots[i].SetCondition();

				Append(addonSlots[i]);
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
			enabled = Configs.MConfigClient.Instance.PowerBeam.enabled;
			if (!enabled)
			{
				Left.Pixels = 160;
				Top.Pixels = Main.instance.invBottom + 174;
				if (Main.LocalPlayer.chest != -1 || Main.npcShop != 0)
				{
					Top.Pixels += 170;
				}
			}

			base.Update(gameTime);
		}
	}
	public class BeamUIItemBox : UIPanel
	{
		private Texture2D itemBoxTexture;

		public Condition condition;

		public int beamSlotType;
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
					return addonItem.type <= ItemID.None || mItem.beamSlotType == beamSlotType;
					//return (addonItem.type <= 0 || mItem.addonSlotType == this.addonSlotType);
				}
				return addonItem.type <= ItemID.None;// || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidMod.Instance);
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			//TODO No failsafe. Should maybe be implemented?
			PowerBeam powerBeamTarget = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem].ModItem as PowerBeam;
			if (powerBeamTarget == null || powerBeamTarget.BeamChange == null) { return; }

			if (powerBeamTarget.BeamChange[beamSlotType] != null && !powerBeamTarget.BeamChange[beamSlotType].IsAir)
			{
				//pickup
				if (Main.mouseItem.IsAir && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = powerBeamTarget.BeamChange[beamSlotType].Clone();

					powerBeamTarget.BeamChange[beamSlotType].TurnToAir();
					if(Main.mouseItem.type == powerBeamTarget.BeamMods[addonSlotType].type)
					{
						powerBeamTarget.BeamMods[addonSlotType].TurnToAir();
					}
				}
				//activate
				if (Main.mouseItem.IsAir && !Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift)) //this can be cleaner
				{
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<OmegaCannonAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.OmegaCannonLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<BattleHammerAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.BattleHammerLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<VoltDriverAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<MagMaulAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.MagMaulLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<ImperialistAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.ImperialistLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<JudicatorAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<ShockCoilAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<VoltDriverAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<ChargeBeamAddon>() || powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<ChargeBeamV2Addon>() || powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<LuminiteBeamAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeBeamLoad);
					}
					if (powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<HyperBeamAddon>() || powerBeamTarget.BeamChange[beamSlotType].type == ModContent.ItemType<PhazonBeamAddon>())
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.BeamAquired);
					}
					powerBeamTarget.BeamMods[addonSlotType] = powerBeamTarget.BeamChange[beamSlotType].Clone();
				}
			}
			else if (!Main.mouseItem.IsAir || condition == null || (condition != null && condition(Main.mouseItem)))
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					//SoundEngine.PlaySound(SoundID.Grab);
					powerBeamTarget.BeamChange[beamSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			Item target = Main.LocalPlayer.inventory[Main.LocalPlayer.MetroidPlayer().selectedItem];
			if (target == null || target.type != ModContent.ItemType<PowerBeam>()) { return; }
			PowerBeam powerBeamTarget = (PowerBeam)target.ModItem;

			spriteBatch.Draw(itemBoxTexture, DrawRectangle, new Color(255, 255, 255));

			// Item drawing.
			if (powerBeamTarget == null || powerBeamTarget.BeamChange == null || powerBeamTarget.BeamChange[beamSlotType].IsAir) { return; }

			Color itemColor = powerBeamTarget.BeamChange[beamSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[powerBeamTarget.BeamChange[beamSlotType].type].Value;
			CalculatedStyle innerDimensions = GetDimensions();

			if (IsMouseHovering)
			{
				Main.hoverItemName = powerBeamTarget.BeamChange[beamSlotType].Name;
				Main.HoverItem = powerBeamTarget.BeamChange[beamSlotType].Clone();
			}

			Rectangle frame = Main.itemAnimations[powerBeamTarget.BeamChange[beamSlotType].type] != null
						? Main.itemAnimations[powerBeamTarget.BeamChange[beamSlotType].type].GetFrame(itemTexture)
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

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, powerBeamTarget.BeamChange[beamSlotType].type);

			Vector2 drawPosition = new(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (powerBeamTarget.BeamChange[beamSlotType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, itemColor);//, 0f,
																	   //Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
		}
	}
}
