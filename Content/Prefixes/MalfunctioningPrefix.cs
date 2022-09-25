using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Prefixes
{
	public class MalfunctioningPrefix : HunterClassWeaponPrefix
	{
		public override float RollChance(Item item) => 2f;

		public override void ModifyValue(ref float valueMult) => valueMult *= 0.9f;

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			useTimeMult = 1.2f;
			shootSpeedMult = 0.975f;
			damageMult = 0.9f;
		}
	}
}
