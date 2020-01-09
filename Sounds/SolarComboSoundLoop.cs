using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Sounds
{
	public class SolarComboSoundLoop : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			if(soundInstance.State == SoundState.Playing)
			{
				soundInstance.Stop(true);
			}
			soundInstance = sound.CreateInstance();
			soundInstance.IsLooped = true;
			return soundInstance;
		}
	}
}