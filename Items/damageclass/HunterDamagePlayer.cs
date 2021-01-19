using Microsoft.Xna.Framework;
using System;
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
		public int hunterCrit = 4;

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
			hunterCrit = 4;
		}
		
		public override void PostUpdateEquips()
		{
			// Any time a vanilla accessory or buff simply increases all round crit, it'll now affect hunter damage weapons.
			// It's not a perfect solution, because if a player has 3 separate items that increase melee, ranged, and magic crit,
			// hunter crit will be increased also.
			int critNum = 4+player.inventory[player.selectedItem].crit;
			if(player.meleeCrit > critNum && player.rangedCrit > critNum && player.magicCrit > critNum)
			{
				//picks the lowest, in case a player is specializing in one while still having an all round buff
				hunterCrit += Math.Min(player.meleeCrit,Math.Min(player.rangedCrit,player.magicCrit))-critNum;
			}
		}
	}
}