using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace MetroidMod.Common.ItemDropRules.Conditions
{
	public class PumpkingBombDrop : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation) { return false; }
			int wave = NPC.waveNumber - 6;
			if (Main.expertMode)
			{
				wave += 5;
			}
			int chance = (int)MathHelper.Max(16 - wave, 1);
			return info.rng.NextBool(chance);
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "No.";
	}
}
