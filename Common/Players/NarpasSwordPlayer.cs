using MetroidMod.Common.Configs;
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
