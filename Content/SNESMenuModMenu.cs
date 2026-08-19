using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content
{
	public class SNESMenuModMenu : ModMenu
	{
		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/SNESTitle");
		public override int Music => MusicLoader.GetMusicSlot($"{Mod.Name}/Assets/Music/SuperMetroidAdjacentMenu");
		public override string DisplayName => Mod.GetLocalization("Menus.SNES", PrettyPrintName).Value;

		private Asset<Texture2D> background;
		public Asset<Texture2D> Background
		{
			get {
				background ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/SNESTitle_Background");
				return background;
			}
		}
		
		private Asset<Texture2D> theBaby;
		public Asset<Texture2D> TheBaby
		{
			get {
				theBaby ??= ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/SNESTitle_Baby");
				return theBaby;
			}
		}


		// current tick of waiting before updating backgroundFrame
		private int backgroundFrameUpdateTick = 0;
		// how many ticks to wait before updating backgroundFrame
		private static readonly int backgroundFrameUpdateInterval = 8;
		// index of currently drawn frame of the background
		private int backgroundFrame = 0;
		// frame count of background
		private static readonly int backgroundFrameCount = 4;

		// current tick of waiting before updating gtheBabyFrame
		private int theBabyFrameUpdateTick = 0;
		// how many ticks to wait before updating theBabyFrame
		private static readonly int theBabyFrameUpdateInterval = 8;
		// index of currenly drawn frame of the baby
		private int theBabyFrame = 0;
		// frame count of the baby
		private static readonly int theBabyFrameCount = 4;
		// spasm tick of the baby, ticked up every time we change the baby's frame.
		// above theBabyFrameSpasmThreshold, decreases the update interval by a factor of theBabyFrameSpasmSpeed
		private int theBabyFrameSpasm = 0;
		// threshold at which frames speed up
		private static readonly int theBabyFrameSpasmThreshold = 3;
		// maximum number of spasm ticks
		private static readonly int theBabyFrameSpasmCount = 5;
		// multiplier of the speed of ticking
		private static readonly int theBabyFrameSpasmSpeed = 2;

		public override void Update(bool isOnTitleScreen)
		{
			UpdateBackgroundSprite();
			UpdateTheBabySprite();
		}
		
		private void UpdateBackgroundSprite()
		{
			if (++backgroundFrameUpdateTick < backgroundFrameUpdateInterval)
			{
				return;
			}

			backgroundFrameUpdateTick = 0;
			
			if (++backgroundFrame < backgroundFrameCount)
			{
				return;
			}

			backgroundFrame = 0;
		}

		private void UpdateTheBabySprite()
		{
			if (++theBabyFrameUpdateTick < theBabyFrameUpdateInterval / (theBabyFrameSpasm >= theBabyFrameSpasmThreshold ? theBabyFrameSpasmSpeed : 1))
			{
				return;
			}
			
			theBabyFrameUpdateTick = 0;
			
			if (++theBabyFrame < theBabyFrameCount)
			{
				return;
			}

			theBabyFrame = 0;

			if (++theBabyFrameSpasm < theBabyFrameSpasmCount)
			{
				return;
			}

			theBabyFrameSpasm = 0;
		}

		// da bebeh's size because just keeping it normal size was too big
		private readonly float theBabySpriteSize = 0.6f;
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			// draw to the full screen
			spriteBatch.Draw(Background.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle(0, Background.Height() * backgroundFrame / 4, Background.Width(), Background.Height() / 4), Color.White);
			// draw to a spot in the center of the screen, but 63.5% of the y height of the screen
			spriteBatch.Draw(TheBaby.Value, new Rectangle(Main.screenWidth / 2 - (int)(TheBaby.Width() * theBabySpriteSize) / 2, (int)(Main.screenHeight * 0.635 - (int)(TheBaby.Height() * theBabySpriteSize) / 8), (int)(TheBaby.Width() * theBabySpriteSize), (int)(TheBaby.Height() * theBabySpriteSize) / 4), new Rectangle(0, TheBaby.Height() * theBabyFrame / 4, TheBaby.Width(), TheBaby.Height() / 4), Color.White);

			logoRotation = 0f;
			logoScale = 1f;
			drawColor = new(255, 255, 255);
			return true;
		}
	}
}
