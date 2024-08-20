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
	internal class SuitLockPlayer : ModPlayer
	{
		private static bool SuitLockEnabled => !MConfigItems.Instance.disengageSuitLock;

		public bool CanUseHyperBeam()
		{
			if(NarpasSwordPlayer.IsEnabled(Player))
			{
				return true;
			}

			if(SuitLockEnabled)
			{
				return Player.GetModPlayer<MPlayer>().accessHyperBeam; //Makes it easier for external mods to add suits with Hyper Beam usage
			}
			else
			{
				return true;
			}
		}

		public bool CanUsePhazonBeam()
		{
			if (NarpasSwordPlayer.IsEnabled(Player))
			{
				return true;
			}

			if (SuitLockEnabled)
			{
				return Player.GetModPlayer<MPlayer>().accessPhazonBeam; //Makes it easier for external mods to add suits with Phazon Beam usage
			}
			else
			{
				return true;
			}
		}

		/*private bool HasAddon<T>() where T: ModSuitAddon         ||||Not necessary
		{
			var addons = MPlayer.GetPowerSuit(Player);
			return addons.Contains(SuitAddonLoader.GetAddon<T>());
		}*/
	}
}
