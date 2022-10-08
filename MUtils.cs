using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Content.NPCs.GoldenTorizo;
using MetroidMod.Content.NPCs.Torizo;
using Terraria;
using Terraria.ModLoader;

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
				if (Main.npc[i].boss && Main.npc[i].active && Main.npc[i].type != ModContent.NPCType<IdleTorizo>() && Main.npc[i].type != ModContent.NPCType<IdleGoldenTorizo>())
				{
					return true;
				}
			}
			return false;
		}
	}
}
