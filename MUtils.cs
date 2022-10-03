using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MetroidMod
{
	public static class MUtils
	{
		/*public static string ConcatBeamNames(params ModBeam[] modBeams)
		{
			string result = "";
			for (int i = 0; i < modBeams.Length; i++)
			{
				result += modBeams[i].Name;//[propertyName];
			}
			return result;
		}*/

		public static bool AnyBossesActive()
		{
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].boss)
				{
					return true;
				}
			}
			return false;
		}
	}
}
