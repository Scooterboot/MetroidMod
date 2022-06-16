using Terraria.Audio;

namespace MetroidMod
{
	public static partial class Sounds
	{
		public static class UI
		{
			public static readonly SoundStyle SwitchVisor = new($"{nameof(MetroidMod)}/Assets/Sounds/SwitchVisor")
			{
				Volume = 0.5f
			};
			public static readonly SoundStyle SwitchVisor2 = new($"{nameof(MetroidMod)}/Assets/Sounds/SwitchVisor2")
			{
				Volume = 0.5f
			};
		}
	}
}
