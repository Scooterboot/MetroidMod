using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Sounds
{
	public class PhazonBeamSound : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			if(soundInstance.State == SoundState.Playing)
			{
				soundInstance.Stop(true);
			}
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume;// * 0.75f;
			return soundInstance;
		}
	}
}