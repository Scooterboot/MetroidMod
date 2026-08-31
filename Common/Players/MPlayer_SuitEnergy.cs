using System;
using MetroidMod.Common.GlobalItems;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Common.Players
{
	public partial class MPlayer : ModPlayer
	{
		/// <summary>
		/// The amount of damage removed from incoming damage to Energy.
		/// </summary>
		public int EnergyDefense = 0;
		/// <summary>
		/// The number of Energy Tanks the player has.
		/// </summary>
		public int EnergyTanks = 0;
		/// <summary>
		/// The number of Energy Tanks the player can use.
		/// </summary>
		public int tankCapacity = 0;
		/// <summary>
		/// The maximum possible energy the player can have.
		/// </summary>
		public int MaxEnergy => Math.Min((EnergyTanks * 100) + 99 + AdditionalMaxEnergy, (tankCapacity * 100) + 99 + AdditionalMaxEnergy);
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
		public int Energy = 99;

		/// <summary>
		/// The number of Reserve Tanks the player has.
		/// </summary>
		public int SuitReserveTanks = 0;
		/// <summary>
		/// The maximum possible reserve energy the player can have.
		/// </summary>
		public int MaxSuitReserves => (SuitReserveTanks * Configs.MConfigItems.Instance.reserveTankStoreCount) + AdditionalMaxReserves;
		public int AdditionalMaxReserves = 0;
		/// <summary>
		/// The amount of energy the player has in reserves.
		/// </summary>
		public int SuitReserves = 0;

		public bool SuitReservesAuto = false;
		public bool drainingReserves = false;

		public void ResetEffects_SuitEnergy()
		{			bool flag = false;
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
				SuitReserveTanks = 0;
				EnergyTanks = 0;
				tankCapacity = 0;
				AdditionalMaxEnergy = 0;
			}
		}
		public bool EnergyDamageLogic(Player.HurtInfo info)
		{
			if (!ShouldShowArmorUI)
			{
				return false;
			}

			if (Energy > 0)
			{
				Energy -= Math.Max(info.Damage - EnergyDefense, 1);
				Player.immune = true;
				Player.AddImmuneTime(info.CooldownCounter, info.PvP ? 8 : 40);
				// TODO: Implement immunity frames properly. Cross Necklace is ignored because
				// Terraria doesn't cache it being equipped so we'll have to do that ourselves.
				// - Armipotent

				if (Energy > 0)
				{
					if (Configs.MConfigClient.Instance.energyHit)
					{
						SoundEngine.PlaySound(Sounds.Suit.EnergyHit, Player.position);
					}

					return true;
				}
			}

			return false;
		}
		public override void OnRespawn()
		{
			if (Player.TryMetroidPlayer(out MPlayer mp))
			{
				mp.reserveHearts = mp.reserveTanks;
				mp.Energy = mp.MaxEnergy;
				mp.SuitReserves = mp.MaxSuitReserves;
				if (mp.PrimeHunter)
				{
					mp.PrimeHunter = !mp.PrimeHunter;
				}
				if (mp.ShouldShowArmorUI)
				{
					SoundEngine.PlaySound(Sounds.Suit.SpawnIn, new(Player.SpawnX, Player.SpawnY));
				}
			}
			for (int i = 0; i < Player.inventory.Length; i++)
			{
				if (Player.inventory[i].type == ModContent.ItemType<Content.Items.Weapons.ArmCannon>())
				{
					MGlobalItem mi = Player.inventory[i].GetGlobalItem<MGlobalItem>();

					if (mi.statMissiles < mi.maxMissiles || mi.statUA < mi.maxUA)
					{
						if (mi.statMissiles < mi.maxMissiles)
						{
							mi.statMissiles = mi.maxMissiles;
							mi.statUA = mi.maxUA;
						}
						if (mi.statUA < mi.maxUA)
						{
							mi.statUA += mi.maxUA;
						}
					}
				}
			}
		}
		private int Stinger = 0;
		public override void UpdateLifeRegen()
		{
			if (Energy > MaxEnergy) { Energy = MaxEnergy; }
			if (EnergyTanks > tankCapacity) { EnergyTanks = tankCapacity; }
			if (SuitReserves > MaxSuitReserves) { SuitReserves = MaxSuitReserves; }
			// SetMinMax(ref EnergyDefenseEfficiency);
			// SetMinMax(ref EnergyExpenseEfficiency);
			if (!ShouldShowArmorUI) { return; }
			if (SuitReservesAuto)
			{
				//Energy += Math.Min(SuitReserves, MaxEnergy);
				//SuitReserves -= Math.Min(SuitReserves, MaxEnergy);
				while (Energy > MaxEnergy)
				{
					SuitReserves += 1;
					Energy -= 1;
				}
				if (Energy <= 0)
				{
					drainingReserves = true;
				}
				if (drainingReserves)
				{
					if (Energy < MaxEnergy && SuitReserves > 0)
					{
						int amount = 3;
						SetMinMax(ref amount, 1, Math.Min(MaxEnergy - Energy, SuitReserves));
						Energy += amount;
						SuitReserves -= amount;
					}
					else
					{
						drainingReserves = false;
					}
				}
			}
			else
			{
				drainingReserves = false;
			}
			if (Player.immune || Player.creativeGodMode) { return; }
			if (Energy > 0 && Player.lifeRegen < 0)
			{
				int regen = Player.lifeRegen;
				int oldEnergy = Energy;
				Stinger++;
				if (Stinger >= 30 && !Player.creativeGodMode)
				{
					Stinger = 0;
					// float damageToSubtractFromEnergy = Math.Max((-Player.lifeRegen) * (1 - EnergyExpenseEfficiency), 1f);// Math.Max((-Player.lifeRegen) / 60 * (1 - EnergyExpenseEfficiency), 1f); //why was this set to min? it nullified dot
					// Energy = (int)Math.Max(Energy - damageToSubtractFromEnergy, 0);
					//Player.lifeRegen += (int)(oldEnergy * EnergyDefenseEfficiency);
				}
				Player.lifeRegen -= Player.lifeRegen;
				//if (Player.lifeRegen > 0) { Player.lifeRegen = 0; }
			}
		}
		private static void SetMinMax(ref float value, float min = 0f, float max = 1f) => value = Math.Min(Math.Max(value, min), max);
		private static void SetMinMax(ref int value, int min = 0, int max = 1) => value = Math.Min(Math.Max(value, min), max);
	}
}
