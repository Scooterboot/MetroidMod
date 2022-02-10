using System;
using Terraria;
//using Terraria.ModLoader;
//using MetroidMod;
using MetroidMod.Common.Worlds;

namespace MetroidMod
{
	public class ModCalls
	{
		public static bool GetBossDowned(string boss)
		{
			switch (boss.ToLower())
			{
				default:
					return false;
				case "torizo":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedTorizo);
				case "serris":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedSerris);
				case "kraid":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedKraid);
				case "phantoon":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedPhantoon);
				case "nightmare":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedNightmare);
				case "omegapirate":
				case "omega pirate":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedOmegaPirate);
				case "goldentorizo":
				case "golden torizo":
					return MWorld.bossesDown.HasFlag(MetroidBossDown.downedGoldenTorizo);
				//case "pirateinvasion":
				//case "pirate invasion":
				//case "spacepirateinvasion":
				//case "space pirate invasion":
				//case "space pirate":
				//    return MWorld.bossesDown.HasFlag(MetroidBossDown.downedSpacePirates);
			}
		}

		public static object Call(params object[] args)
		{
			if (args != null && args.Length != 0)
			{
				if (!(args[0] is string))
				{
					return new ArgumentException("ERROR: First argument must be a string function name.");
				}
				switch (args[0].ToString())
				{
					case "Downed":
					case "GetDowned":
					case "BossDowned":
					case "GetBossDowned":
						if (args.Length < 2)
						{
							return new ArgumentNullException("ERROR: Must specify a boss or event name as a string.");
						}
						if (!(args[1] is string))
						{
							return new ArgumentException("ERROR: The argument to \"Downed\" must be a string.");
						}
						return GetBossDowned(args[1].ToString());
					
					default:
						return new ArgumentException("ERROR: Invalid method name.");
				}
			}
			return new ArgumentNullException("ERROR: No function name specified. First argument must be a function name.");
		}
		internal static Player getPlayer(object o)
		{
			if (o is int)
			{
				int i = (int)o;
				return Main.player[i];
			}
			Player p;
			if ((p = (o as Player)) != null)
			{
				return p;
			}
			return null;
		}
	}
}
