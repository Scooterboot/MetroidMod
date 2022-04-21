using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
//using MetroidModPorted.Backgrounds;

namespace MetroidModPorted.Content
{
	public class NESCrateriaSurfaceMetroidModMenu : ModMenu
	{
		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Menu/NESTitle");
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/NESTitle");
		public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<Biomes.NESCrateriaSurfaceBackgroundStyle>();

		public override string DisplayName => "Crateria (Surface, Metroid NES)";

		/*public override void OnSelected()
		{
			//SoundEngine.PlaySound(SoundID.GuitarC);
		}*/
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			logoRotation = 0f;
			logoScale = 1f;
			drawColor = new(255, 255, 255);
			return true;
			//return base.PreDrawLogo(spriteBatch, ref logoDrawCenter, ref logoRotation, ref logoScale, ref drawColor);
		}
		public override void Update(bool isOnTitleScreen)
		{
			if (isOnTitleScreen)
			{
				Main.SkipToTime(16200, false);
			}
			//Main.time = 16200;
			//Main.dayTime = false;
		}
	}
}
