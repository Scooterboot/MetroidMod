using Terraria.Audio;

namespace MetroidModPorted
{
	public static partial class Sounds
	{
		public static class UI
		{
			public static readonly SoundStyle SwitchVisor = new($"{nameof(MetroidModPorted)}/Assets/Sounds/SwitchVisor")
			{
				Volume = 0.5f
			};
			public static readonly SoundStyle SwitchVisor2 = new($"{nameof(MetroidModPorted)}/Assets/Sounds/SwitchVisor2")
			{
				Volume = 0.5f
			};
		}
	}
}
