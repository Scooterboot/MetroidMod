using System;

namespace MetroidMod
{
	public class WeightedChance
	{
		public Action Func { get; }
		public double Ratio { get; }

		public WeightedChance(Action func, double ratio)
		{
			Func = func;
			Ratio = ratio;
		}
	}
}
