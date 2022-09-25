using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Prefixes
{
	public class FaultyPrefix : HunterClassWeaponPrefix
	{
		public override float RollChance(Item item) => 1.3f;

		public override void ModifyValue(ref float valueMult) => valueMult *= 1.1f;

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			useTimeMult = 1.75f;
			shootSpeedMult = 0.75f;
			damageMult = 0.825f;
		}
	}
}
