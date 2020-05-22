using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Items.damageclass
{
	public class HunterDamagePlayer : ModPlayer
	{
		public static HunterDamagePlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<HunterDamagePlayer>();
		}

		public float hunterDamageAdd;
		public float hunterDamageMult = 1f;
		public float hunterKnockback;
		public int hunterCrit;

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
			hunterDamageAdd = 0f;
			hunterDamageMult = 1f;
			hunterKnockback = 0f;
			hunterCrit = 0;
		}
	}
}