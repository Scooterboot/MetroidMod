using System.Reflection;
using MetroidMod.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace MetroidMod.Common.UI
{
	internal class ChoziteDualtoolUI
	{
		private static float ElementSpacing => 45f;
		private static float ButtonDetectionRadius => 40f / 2;

		private bool uiActive;
		private Vector2 uiCenterPosition;

		public void Update()
		{
			if (!ChoziteDualtoolSettings.CanShow)
			{
				uiActive = false;
				return;
			}

			bool rightClicked = Main.mouseRight && Main.mouseRightRelease;
			if (rightClicked)
			{
				uiActive = !uiActive;
				uiCenterPosition = Main.MouseScreen;
			}

			if (!uiActive) return;

			bool inInterface = false;
			foreach(Button button in Buttons)
			{
				Vector2 center = ButtonCenter(button);
				button.Hovered = Vector2.Distance(Main.MouseScreen, center) <= ButtonDetectionRadius;

				if (button.Hovered)
				{
					inInterface = true;

					bool leftClicked = Main.mouseLeft && Main.mouseLeftRelease;
					if(leftClicked) button.Toggle();
				}
			}

			if(inInterface) Main.LocalPlayer.mouseInterface = true;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!uiActive) return;

			foreach(Button button in Buttons)
			{
				DrawButton(spriteBatch, button);
			}
		}

		private void DrawButton(SpriteBatch spriteBatch, Button button)
		{
			Vector2 center = ButtonCenter(button);

			Rectangle backgroundSource = TextureFrame((int)TextureIndex.Background);
			Rectangle borderSource = TextureFrame((int)(button.Hovered ? TextureIndex.BorderSelected : TextureIndex.Border));
			Rectangle iconSource = TextureFrame((int)button.TextureIndex + (button.Enabled ? 0 : 1));

			Vector2 origin = borderSource.Size() * 0.5f;

			Color color = Color.White;
			Color backgroundColor = ChoziteDualtoolSettings.IsPlacing ? Color.SlateBlue : Color.MediumVioletRed;
			
			if (!button.Enabled && button.DarkWhenDisabled)
			{
				color = color.MultiplyRGB(Color.Gray);
				backgroundColor = backgroundColor.MultiplyRGB(Color.Gray);
			}

			spriteBatch.Draw(Texture, center, backgroundSource, backgroundColor, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Texture, center, borderSource, color, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(Texture, center, iconSource, color, 0f, origin, 1f, SpriteEffects.None, 0f);
		}

		private Vector2 ButtonCenter(Button button)
		{
			return uiCenterPosition + button.CenterOffset * ElementSpacing;
		}

		private readonly Button[] Buttons = [
			new(nameof(ChoziteDualtoolSettings.IsPlacing), TextureIndex.PlaceOption, Vector2.Zero, false),
			new(nameof(ChoziteDualtoolSettings.ApplyRegen), TextureIndex.RegenOption, -Vector2.UnitY),
			new(nameof(ChoziteDualtoolSettings.AllowPlaceNew), TextureIndex.PlaceNewOption, -Vector2.UnitX),
			new(nameof(ChoziteDualtoolSettings.AllowPlaceOnEmpty), TextureIndex.PlaceAirOption, Vector2.UnitX),
			];

		private record Button(string Field, TextureIndex TextureIndex, Vector2 CenterOffset, bool DarkWhenDisabled = true)
		{
			private readonly FieldInfo fieldInfo = typeof(ChoziteDualtoolSettings).GetField(Field);

			public bool Enabled => (bool)fieldInfo.GetValue(null);
			public bool Hovered = false;

			public void Toggle()
			{
				fieldInfo.SetValue(null, !Enabled);
			}
		}

		private static Texture2D Texture =>
			ModContent.GetInstance<MetroidMod>().Assets.Request<Texture2D>(
			"Assets/Textures/UI/ChoziteDualtool", AssetRequestMode.ImmediateLoad).Value;
		private static Rectangle TextureFrame(int index)
		{
			int textureSize = Texture.Width;
			return new(0, index * textureSize, textureSize, textureSize);
		}
		private enum TextureIndex
		{
			Border,
			BorderSelected,
			Background,
			PlaceOption = 3,
			RegenOption = 5,
			PlaceNewOption = 7,
			PlaceAirOption = 9,
		}
	}
}
