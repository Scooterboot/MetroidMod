using Terraria;
using Terraria.ModLoader;

using MetroidMod.Content.DamageClasses;

namespace MetroidMod.Common.Players
{
	// This class only really still exists for ease of combining multipliers, so...
	public class HunterDamagePlayer : ModPlayer
	{
		public static HunterDamagePlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<HunterDamagePlayer>();
		}

		/// <summary>
		/// Damage to add to the <see cref="HunterDamageClass"/>.<br />
		/// Added after <see cref="HunterDamageMult"/> is calculated.
		/// </summary>
		public float HunterDamageAdd;
		/// <summary>
		/// Damage to multiply <see cref="HunterDamageClass"/> with.<br />
		/// Calculated before <see cref="HunterDamageAdd"/> is added.
		/// </summary>
		public float HunterDamageMult = 1f;
		/// <summary>
		/// Knockback to add to <see cref="HunterDamageClass"/>.
		/// </summary>
		public float HunterKnockback;
		/// <summary>
		/// Critical strike chance to add to <see cref="HunterDamageClass"/>.
		/// </summary>
		public int HunterCrit = 4;

		public override void ResetEffects()
		{
			ResetVariables();
		}

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			HunterDamageAdd = 0f;
			HunterDamageMult = 1f;
			HunterKnockback = 0f;
			HunterCrit = 4;
		}

		public override void PostUpdateEquips()
		{
			Player.GetDamage<HunterDamageClass>() *= HunterDamageMult;
			Player.GetDamage<HunterDamageClass>() += HunterDamageAdd;
			Player.GetKnockback<HunterDamageClass>() += HunterKnockback;
			Player.GetCritChance<HunterDamageClass>() += HunterCrit;
		}
	}
}
