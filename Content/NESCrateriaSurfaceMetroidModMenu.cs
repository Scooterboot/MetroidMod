using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
//using MetroidMod.Backgrounds;

namespace MetroidMod.Content
{
	public class NESCrateriaSurfaceMetroidModMenu : ModMenu
	{
		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/NESTitle");
		public override int Music => MusicLoader.GetMusicSlot($"{Mod.Name}/Assets/Music/Title");
		public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<Biomes.NESCrateriaSurfaceBackgroundStyle>();
		public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel");
		public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel");
		public Asset<Texture2D> LogoGlint => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/NESTitleGlint");

		public override string DisplayName => Mod.GetLocalization("Menus.NES", PrettyPrintName).Value;

		/*public override void OnSelected()
		{
			//SoundEngine.PlaySound(SoundID.GuitarC);
		}*/
		
		internal int tick = 0;
		internal static readonly int tickMax = 500;
		internal static readonly int tickThresholdForGlint = 400;
		internal float glintPercent = 0;


		public override void Update(bool isOnTitleScreen)
		{
			if (Main.gameMenu)
			{
				Main.time = 16200;
				Main.dayTime = false;
			}
			if (++tick >= tickMax)
			{
				tick = 0;
				glintPercent = 0f;
			}
			if (tick > tickThresholdForGlint)
			{
				glintPercent = (float)(tick - tickThresholdForGlint) / (tickMax - tickThresholdForGlint);
			}
		}
		
		internal static readonly float constLogoScale = 0.8f;
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			logoRotation = 0f;
			logoScale = constLogoScale;
			drawColor = new(255, 255, 255);
			return true;
			//return base.PreDrawLogo(spriteBatch, ref logoDrawCenter, ref logoRotation, ref logoScale, ref drawColor);
		}
		public override void PostDrawLogo(SpriteBatch spriteBatch, Vector2 logoDrawCenter, float logoRotation, float logoScale, Color drawColor)
		{
			if (tick >= tickThresholdForGlint)
			{
				if (tick % 5 > 0)
				{
					Vector2 glint1val = new(546 * (1f - glintPercent) - Logo.Width() / 2 - LogoGlint.Width() / 2, 136 - Logo.Height() / 2 - LogoGlint.Height() / 2);
					spriteBatch.Draw(LogoGlint.Value, logoDrawCenter + glint1val * constLogoScale, new(0,0,14,14), drawColor, (float)(Math.PI * 0.25f), new(0.5f,0.5f), 0.8f, SpriteEffects.None, 0);
				}
				if (tick % 6 > 0)
				{
					Vector2 glint2val = new(546 * (glintPercent) - Logo.Width() / 2 - LogoGlint.Width() / 2, 0 - Logo.Height() / 2 - LogoGlint.Height() / 2);
					spriteBatch.Draw(LogoGlint.Value, logoDrawCenter + glint2val * constLogoScale, new(0,0,14,14), drawColor, (float)(Math.PI * 0.25f), new(0.5f,0.5f), 0.8f, SpriteEffects.None, 0);
				}
			}
		}
	}
}
