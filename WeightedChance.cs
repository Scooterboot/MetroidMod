using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroidModPorted
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
