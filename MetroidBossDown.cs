using System;

namespace MetroidMod
{
	[Flags]
	public enum MetroidBossDown
	{
		downedNone = 0,
		downedTorizo = 1<<0,
		downedSerris = 1<<1,
		downedKraid = 1<<2,
		downedPhantoon = 1<<3,
		downedNightmare = 1<<4,
		downedOmegaPirate = 1<<5,
		downedGoldenTorizo = 1<<6
	}
}
