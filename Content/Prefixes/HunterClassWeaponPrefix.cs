using MetroidMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Prefixes
{
	public abstract class HunterClassWeaponPrefix : ModPrefix
	{
		public override PrefixCategory Category => PrefixCategory.Custom;

		public override bool CanRoll(Item item) => item.DamageType.CountsAsClass<HunterDamageClass>();
	}
}
