using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidModPorted.Common.Players
{
	public partial class MPlayer : ModPlayer
	{
		/// <summary>
		/// The percentage of energy that is subtracted from the damage the player has taken. <br />
		/// Known as 'Energy Barrier Efficiency' (PROVISIONAL NAME).
		/// </summary>
		public float EnergyDefenseEfficiency = 0.0f;
		/// <summary>
		/// The percentage of damage that is subtracted from the player's energy. <br />
		/// Known as 'Energy Barrier Resilience' (PROVISIONAL NAME).
		/// </summary>
		public float EnergyExpenseEfficiency = 0.1f;
		/// <summary>
		/// The number of Energy Tanks the player has.
		/// </summary>
		public int EnergyTanks = 0;
		/// <summary>
		/// The maximum possible energy the player can have.
		/// </summary>
		public int MaxEnergy => EnergyTanks * 100 + 99 + AdditionalMaxEnergy;
		public int AdditionalMaxEnergy = 0;
		/// <summary>
		/// The amount of filled energy tanks the player has.
		/// </summary>
		public int FilledEnergyTanks => (int)Math.Floor(Energy / 100f);
		/// <summary>
		/// The amount remaining outside of filled energy tanks.
		/// </summary>
		public int EnergyRemainder => Energy - (FilledEnergyTanks * 100);
		/// <summary>
		/// The amount of energy the player has.
		/// </summary>
		public int Energy = 0;

		public int SuitReserves = 0;
		public int SuitReserveTanks = 0;

		public void ResetEffects_SuitEnergy()
		{
			EnergyDefenseEfficiency = 0f;
			EnergyExpenseEfficiency = 0.1f;

			bool flag = false;
			for (int i = 0; i < Player.buffType.Length; i++)
			{
				if (Player.buffType[i] == ModContent.BuffType<Content.Buffs.EnergyRecharge>() && Player.buffTime[i] > 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				EnergyTanks = 0;
				AdditionalMaxEnergy = 0;
			}
		}
		public bool PreHurt_SuitEnergy(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (!ShouldShowArmorUI || Player.immune) { return true; }
			int oldEnergy = Energy;
			float damageToSubtractFromEnergy = Math.Max(damage * (1 - EnergyExpenseEfficiency), 1f);
			Energy = (int)Math.Max(Energy - damageToSubtractFromEnergy, 0);
			damage -= (int)(oldEnergy * EnergyDefenseEfficiency);
			if (damage < 0) { damage = 0; }
			return true;
		}
		public override void UpdateLifeRegen()
		{
			if (Energy > MaxEnergy) { Energy = MaxEnergy; }
			SetMinMax(ref EnergyDefenseEfficiency);
			SetMinMax(ref EnergyExpenseEfficiency);
			if (!ShouldShowArmorUI || Player.immune) { return; }
			if (Energy > 0 && Player.lifeRegen < 0)
			{
				//Player.lifeRegen = 0;
				int oldEnergy = Energy;
				float damageToSubtractFromEnergy = Math.Min((-Player.lifeRegen) / 60 * (1 - EnergyExpenseEfficiency), 1f);
				Energy = (int)Math.Max(Energy - damageToSubtractFromEnergy, 0);
				Player.lifeRegen += (int)(oldEnergy * EnergyDefenseEfficiency);
				if (Player.lifeRegen > 0) { Player.lifeRegen = 0; }
			}
		}
		private static void SetMinMax(ref float value, float min = 0f, float max = 1f) => value = Math.Min(Math.Max(value, min), max);
	}
}
