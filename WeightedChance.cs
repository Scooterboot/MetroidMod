using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
