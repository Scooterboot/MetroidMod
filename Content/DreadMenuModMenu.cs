using MetroidMod.Content.Biomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content
{
	public class DreadMenuModMenu : ModMenu
	{
		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/DreadTitleAni");
		public override int Music => MusicLoader.GetMusicSlot($"{Mod.Name}/Assets/Music/Title");
		public override string DisplayName => Mod.GetLocalization("Menus.Dread", PrettyPrintName).Value;

		private Asset<Texture2D>[] backgrounds;
		public Asset<Texture2D>[] Backgrounds
		{
			get {
				backgrounds ??= new Asset<Texture2D>[5];
				backgrounds[0] ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/DreadTitleBack_Sky");
				backgrounds[1] ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/DreadTitleBack_FarTerrain");
				backgrounds[2] ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/DreadTitleBack_Front-01");
				backgrounds[3] ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/DreadTitleBack_Front-02");
				backgrounds[4] ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/DreadTitleBack_Front-03");
				return backgrounds;
			}
		}

		#region background values

		private static readonly double backgroundScrollSpeed = 0.0005;
		private double backgroundCurrentXAsScreenWidthPercentage = 1;

		#endregion
		
		#region foreground values

		private static readonly double foregroundScrollSpeed = 0.001;
		private double foregroundCurrentXAsScreenWidthPercentage = 1;
		private int foregroundCurrentIndex = 2;
		private static readonly int foregroundMinIndex = 2;
		private static readonly int foregroundMaxIndex = 4;
		private int foregroundOldIndex = 2;

		#endregion

		#region logo values

		private int logoWaitFrame = 0;
		private static readonly int logoWaitFrameThreshold = 180;
		private int logoFrame = 0;
		private static readonly int logoFrameCount = 7;
		private int logoFrameTick = 0;
		private static readonly int logoFrameTickCount = 6;

		#endregion

		public override void Update(bool isOnTitleScreen)
		{
			UpdateBackground();
			UpdateForeground();
			UpdateLogo();
		}

		private void UpdateBackground()
		{
			if (backgroundCurrentXAsScreenWidthPercentage > Main.screenWidth)
			{
				backgroundCurrentXAsScreenWidthPercentage = Main.screenWidth;
				return;
			}

			backgroundCurrentXAsScreenWidthPercentage -= backgroundScrollSpeed;
			
			if (backgroundCurrentXAsScreenWidthPercentage > 0)
			{
				return;
			}
			
			backgroundCurrentXAsScreenWidthPercentage = 1;
		}

		private void UpdateForeground()
		{
			if (foregroundCurrentXAsScreenWidthPercentage > Main.screenWidth)
			{
				foregroundCurrentXAsScreenWidthPercentage = Main.screenWidth;
				return;
			}

			foregroundCurrentXAsScreenWidthPercentage -= foregroundScrollSpeed;

			if (foregroundCurrentXAsScreenWidthPercentage > 0)
			{
				return;
			}

			foregroundCurrentXAsScreenWidthPercentage = 1;
			foregroundOldIndex = foregroundCurrentIndex;
			foregroundCurrentIndex = Main.rand.Next(foregroundMinIndex, foregroundMaxIndex);
		}

		private void UpdateLogo()
		{
			if (++logoWaitFrame < logoWaitFrameThreshold)
			{
				return;
			}
				if (++logoFrameTick < logoFrameTickCount)
			{
				return;
			}
			
			logoFrameTick = 0;
				if (++logoFrame < logoFrameCount)
			{
				return;
			}
			
			logoWaitFrame = 0;
			logoFrame = 0;
		}

		private static readonly float logoScale = 0.5f;
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			Rectangle screenRect = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
			
			Rectangle backgroundRectCurrent = screenRect;
			backgroundRectCurrent.X = (int)(Main.screenWidth * backgroundCurrentXAsScreenWidthPercentage);
			Rectangle backgroundRectOld = backgroundRectCurrent;
			backgroundRectCurrent.X -= Main.screenWidth;

			Rectangle foregroundRectCurrent = screenRect;
			foregroundRectCurrent.X = (int)(Main.screenWidth * foregroundCurrentXAsScreenWidthPercentage);
			Rectangle foregroundRectOld = foregroundRectCurrent;
			foregroundRectOld.X -= Main.screenWidth;
			spriteBatch.Draw(Backgrounds[0].Value, screenRect, Color.White);
			spriteBatch.Draw(Backgrounds[1].Value, backgroundRectCurrent, Color.White);
			spriteBatch.Draw(Backgrounds[1].Value, backgroundRectOld, Color.White);
			spriteBatch.Draw(Backgrounds[foregroundCurrentIndex].Value, foregroundRectCurrent, Color.White);
			spriteBatch.Draw(Backgrounds[foregroundOldIndex].Value, foregroundRectOld, Color.White);

			logoRotation = 0f;
			logoScale = 1f;
			drawColor = new(255, 255, 255);

			Rectangle drawRect = new((int)(logoDrawCenter.X - (Logo.Width() * logoScale / 2)), (int)(logoDrawCenter.Y - (Logo.Height() * logoScale / 14)), (int)(Logo.Width() * logoScale), (int)(Logo.Height() * logoScale / 7));
			
			spriteBatch.Draw(Logo.Value, drawRect, new Rectangle(0, (int)(Logo.Height() * (logoFrame / 7f)), Logo.Width(), Logo.Height() / 7), drawColor);
			return false;
		}
	}
}
