using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Common.Configs;
using MetroidMod.Common.UI;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.MorphBallAddons;
using MetroidMod.Content.Tiles.ItemTile.Missile;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	/// <summary>
	/// Busted godmode!
	/// </summary>
	internal class NarpasSwordPlayer : ModPlayer
	{
		public static bool IsEnabled(Player player)
		{
			bool properName = player.name.ToLower() == "narpas sword";
			
			if(properName)
			{
				return true;
			}

			return MConfigMain.Instance.enableGlobalNarpasSword;
		}

		public override void ResetEffects()
		{
			if (!IsEnabled(Player)) return;
			Player.GetModPlayer<MPlayer>().NarpasSwordEffects();
		}
	}
}
