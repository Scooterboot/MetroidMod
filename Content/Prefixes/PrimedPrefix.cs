using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Prefixes
{
	public class PrimedPrefix : HunterClassWeaponPrefix
	{
		public override float RollChance(Item item) => 0.8f;

		public override void ModifyValue(ref float valueMult) => valueMult *= 1.25f;

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			useTimeMult = 0.75f;
			shootSpeedMult = 2f;
			damageMult = 1.4f;
		}
	}
}
