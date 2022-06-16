using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace MetroidMod.Common.ItemDropRules.Conditions
{
	public class MissileCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation) { return false; }
			bool flag = false;
			for (int i = 0; i < info.player.inventory.Length; i++)
			{
				if (info.player.inventory[i].type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return true;
			}
			return false;
		}

		public bool CanShowItemDropInUI() => false;

		public string GetConditionDescription() => "No.";
	}
}
