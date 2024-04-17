using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace MetroidMod.Common.ItemDropRules.Conditions
{
	public class UniversalAmmoCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation) { return false; }
			bool flag = false;
			for (int i = 0; i < info.player.inventory.Length; i++)
			{
				if (info.player.inventory[i].type == ModContent.ItemType<Content.Items.Weapons.PowerBeam>())
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "No.";
	}
}
