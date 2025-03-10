using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace MetroidMod.Common.Configs
{
	// NOTE ABOUT SUBPAGES!! [DefaultValue()] does NOT work on values inside of subpages. Use variable = value instead.
	//TODO: add configs for hunter weapons
	//[Label("Items Config")]
	public class MConfigItems : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		internal CanEditServerConfig condition;

		internal delegate bool CanEditServerConfig(ModConfig pendingConfig, int whoAmI, ref NetworkText message);

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => condition(pendingConfig, whoAmI, ref message);

		public static MConfigItems Instance;

		public MConfigItems()
		{
			condition = delegate (ModConfig pendingConfig, int whoAmI, ref NetworkText message) {
				return whoAmI == 0;
			};
		}

		[Header("[i:MetroidMod/ChoziteBar]ChoziteGear")]

		////[Label("[i:MetroidMod/HiJumpBootsAddon] Wall Jump")]
		////[Tooltip("When enabled, a full set of Chozite Armor grants the ability to Wall Jump.")]
		[DefaultValue(true)]
		public bool enableWallJumpChoziteArmor = true;

		////[Label("[i:MetroidMod/ChoziteHelmet] Chozite Warmask Defense")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defenseChoziteHelmet = 5;

		////[Label("[i:MetroidMod/ChoziteBreastplate] Chozite Breastplate Defense")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int defenseChoziteBreastplate = 6;

		//[Label("[i:MetroidMod/ChoziteGreaves] Chozite Greaves Defense")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(4)]
		public int defenseChoziteGreaves = 4;

		//[Label("[i:MetroidMod/ChoziteSword] Chozite Sword Damage")]
		[Range(1, 30)]
		[Increment(1)]
		[Slider]
		[DefaultValue(16)]
		public int damageChoziteSword = 16;

		//[Label("[i:MetroidMod/ChoziteShortsword] Chozite Shortsword Damage")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(14)]
		public int damageChoziteShortsword = 14;

		//[Label("[i:MetroidMod/ChoziteCrossbow] Chozite Crossbow Damage")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(12)]
		public int damageChoziteCrossbow = 12;

		[Header("[i:MetroidMod/VariaSuitV2AddonAddon]PowerSuit")]

		//[Label("[i:MetroidMod/PowerGripAddon] Ledge Climb")]
		//[Tooltip("When enabled, the Power Suit's Breastplate grants Ledge Climb.")]
		[DefaultValue(false)]
		public bool enableLedgeClimbPowerSuitBreastplate;

		//[Label("[i:MetroidMod/HiJumpBootsAddon] Wall Jump")]
		//[Tooltip("When enabled, the Power Suit's Greaves grant Wall-Jump.")]
		[DefaultValue(true)]
		public bool enableWallJumpPowerSuitGreaves;

		//[Label("[i:LuckyHorseshoe] Negate Fall Damage")]
		[DefaultValue(true)]
		public bool enableNoFallDamagePowerSuitGreaves;

		//[Label("[i:MetroidMod/PowerSuitHelmet] Power Suit Helmet Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defensePowerSuitHelmet;

		//[Label("[i:MetroidMod/PowerSuitBreastplate] Power Suit Breastplate Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int defensePowerSuitBreastplate;

		//[Label("[i:MetroidMod/PowerSuitGreaves] Power Suit Greaves Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defensePowerSuitGreaves;

		//[Label("[i:MetroidMod/EnergyTankAddon] Energy Defense Efficiency")]
		[Range(0.05f, 1f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.1f)]
		public float energyDefenseEfficiency;

		//[Label("[i:MetroidMod/ReserveTankAddon] Energy Expense Efficiency")]
		[Range(0.05f, 1f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.1f)]
		public float energyExpenseEfficiency;

		[Header("SuitAddons")]

		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(4)]
		public int stackReserveTank;

		[Range(20, 200)]
		[Increment(20)]
		[Slider]
		[DefaultValue(100)]
		public int reserveTankStoreCount;

		//[Label("[i:MetroidMod/PowerGripAddon] Ledge Climb without Power Suit")]
		//[Tooltip("When enabled, lacking the Power Suit Breastplate will automatically grant you Ledge Climb,\njust like in Metroid: Zero Mission.")]
		[DefaultValue(false)]
		public bool enableLedgeClimbNoPowerSuit;

		//[Label("[i:MetroidMod/PowerGripAddon][i:MetroidMod/HiJumpBootsAddon] Power Grip grants Wall Jump")]
		[DefaultValue(false)]
		public bool enableWallJumpPowerGrip;

		//[Label("[i:MetroidMod/PowerGripAddon] Power Grip grants Ledge Climb")]
		[DefaultValue(true)]
		public bool enableLedgeClimbPowerGrip;

		//[Label("[i:MetroidMod/SpeedBoosterAddon] Speedbooster is silent")]
		[DefaultValue(false)]
		public bool muteSpeedBooster;

		[Header("[i:MetroidMod/PowerBeam]SpecialBeams")]

		//[Label("check localization")]
		[DefaultValue(false)]
		public bool disengageSuitLock;
	}
}
