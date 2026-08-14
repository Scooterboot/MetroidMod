using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
//using MetroidMod.Backgrounds;

namespace MetroidMod.Content
{
	public class SNESMenuModMenu : ModMenu
	{
		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/SNESTitle");
		public override int Music => MusicLoader.GetMusicSlot($"{Mod.Name}/Assets/Music/Title");
		public override string DisplayName => "Laboratory";
		
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


		private int backgroundFrameUpdateTick = 0;
		private static readonly int backgroundFrameUpdateInterval = 8;
		private int backgroundFrame = 0;
		private static readonly int backgroundFrameCount = 4;

		private int theBabyFrameUpdateTick = 0;
		private static readonly int theBabyFrameUpdateInterval = 8;
		private int theBabyFrame = 0;
		private static readonly int theBabyFrameCount = 4;
		private int theBabyFrameSpasm = 0;
		private static readonly int theBabyFrameSpasmThreshold = 3;
		private static readonly int theBabyFrameSpasmCount = 5;

		public override void Update(bool isOnTitleScreen)
		{
			if (++backgroundFrameUpdateTick >= backgroundFrameUpdateInterval)
			{
				backgroundFrameUpdateTick = 0;
				if (++backgroundFrame >= backgroundFrameCount)
				{
					backgroundFrame = 0;
				}
			}
			if (++theBabyFrameUpdateTick >= theBabyFrameUpdateInterval / (theBabyFrameSpasm >= theBabyFrameSpasmThreshold ? 2 : 1))
			{
				theBabyFrameUpdateTick = 0;
				if (++theBabyFrame >= theBabyFrameCount)
				{
					theBabyFrame = 0;
					if (++theBabyFrameSpasm >= theBabyFrameSpasmCount)
					{
						theBabyFrameSpasm = 0;
					}
				}
			}
		}

		private float theBabySpriteSize = 0.6f;
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			spriteBatch.Draw(Background.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle(0, Background.Height() * backgroundFrame / 4, Background.Width(), Background.Height() / 4), Color.White);
			spriteBatch.Draw(TheBaby.Value, new Rectangle(Main.screenWidth / 2 - (int)(TheBaby.Width() * theBabySpriteSize) / 2, (int)(Main.screenHeight * 0.635 - (int)(TheBaby.Height() * theBabySpriteSize) / 8), (int)(TheBaby.Width() * theBabySpriteSize), (int)(TheBaby.Height() * theBabySpriteSize) / 4), new Rectangle(0, TheBaby.Height() * theBabyFrame / 4, TheBaby.Width(), TheBaby.Height() / 4), Color.White);

			logoRotation = 0f;
			logoScale = 1f;
			drawColor = new(255, 255, 255);
			return true;
		}
	}
}
