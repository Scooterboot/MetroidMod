using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Common.Configs;
using MetroidMod.Content.SuitAddons;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	internal class HyperBeamAllowedPlayer : ModPlayer
	{
		private static bool SuitLockEnabled => !MConfigItems.Instance.revertHyperBeamSuitLock;

		public bool CanUseHyperBeam()
		{
			if(Player.name == "narpas sword")
			{
				return true;
			}

			if(SuitLockEnabled)
			{
				return HasAddon<TerraGravitySuitAddon>() || HasAddon<VortexAugment>();
			}

			bool hasBlacklistedAddon = HasAddon<PhazonSuitAddon>() || HasAddon<NebulaAugment>();
			return !hasBlacklistedAddon;
		}

		private bool HasAddon<T>() where T: ModSuitAddon
		{
			var addons = MPlayer.GetPowerSuit(Player);
			return addons.Contains(SuitAddonLoader.GetAddon<T>());
		}
	}
}
