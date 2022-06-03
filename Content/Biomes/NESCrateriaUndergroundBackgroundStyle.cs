using Terraria.ModLoader;

namespace MetroidModPorted.Content.Biomes
{
	public class NESCrateriaUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
		{
			//throw new System.NotImplementedException();
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot($"{Mod.Name}/Assets/Textures/Backgrounds/NESCrateriaUnderground0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot($"{Mod.Name}/Assets/Textures/Backgrounds/NESCrateriaUnderground1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot($"{Mod.Name}/Assets/Textures/Backgrounds/NESCrateriaUnderground2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot($"{Mod.Name}/Assets/Textures/Backgrounds/NESCrateriaUnderground3");
		}
	}
}
