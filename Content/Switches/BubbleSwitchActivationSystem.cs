using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Content.Switches
{
	internal class BubbleSwitchActivationSystem : ModSystem
	{
		private int CooldownPerHit => 1;
		private readonly Dictionary<Point16, int> cooldowns = [];
		
		public void HitSwitchAt(int i, int j)
		{
			Point16 key = new(i, j);
			if(!cooldowns.ContainsKey(key))
			{
				Wiring.TripWire(i, j, 1, 1);
			}

			cooldowns[key] = CooldownPerHit;
		}

		public override void PostUpdateEverything()
		{
			foreach (Point16 position in cooldowns.Keys)
			{
				if (cooldowns[position]-- <= 0)
				{
					cooldowns.Remove(position);
				}
			}
		}
	}
}
