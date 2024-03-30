using Terraria.GameContent.ItemDropRules;

namespace MetroidMod.Common.ItemDropRules.Conditions
{
	public class EnergyCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !info.IsInSimulation && info.player.TryMetroidPlayer(out Players.MPlayer mp) && mp.ShouldShowArmorUI && !mp.PrimeHunter;

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "No.";
	}
}
