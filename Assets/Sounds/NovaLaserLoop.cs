using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Assets.Sounds
{
	public class NovaLaserLoop : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan)
		{
			if(soundInstance.State == SoundState.Playing)
			{
				soundInstance.Stop(true);
			}
			soundInstance = Sound.Value.CreateInstance();
			soundInstance.IsLooped = true;
			return soundInstance;
		}
	}
}
