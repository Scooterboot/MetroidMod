using Terraria.GameContent.ItemDropRules;
using MetroidMod.Common.Players;

namespace MetroidMod.Common.ItemDropRules.Conditions
{
	public class ReserveHearts : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				MPlayer mp = info.player.GetModPlayer<MPlayer>();
				return mp.reserveTanks > 0 && mp.reserveHearts < mp.reserveTanks && info.player.statLife >= info.player.statLifeMax2;
			}
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "No.";
	}
}
