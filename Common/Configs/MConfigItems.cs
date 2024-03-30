using System;
using System.ComponentModel;
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

		internal delegate bool CanEditServerConfig(ModConfig pendingConfig, int whoAmI, ref string message);

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) => condition(pendingConfig, whoAmI, ref message);

		public static MConfigItems Instance;

		public MConfigItems()
		{
			condition = delegate (ModConfig pendingConfig, int whoAmI, ref string message) {
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

		[Header("[i:MetroidMod/PowerGripAddon]SuitAddons")]

		[Label("[i:MetroidMod/ReserveTankAddon] Reserve Tank Stack")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(4)]
		public int stackReserveTank;

		[Label("[i:MetroidMod/ReserveTankAddon] Reserve Tank Energy Capacity")]
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

		[Header("[i:MetroidMod/PowerBeam]StandardWeapons")]

		//[Label("[i:MetroidMod/PowerBeam] Power Beam Damage")]
		[Range(1, 50)]
		[Increment(1)]
		[Slider]
		[DefaultValue(10)]
		public int damagePowerBeam;

		//[Label("[i:MetroidMod/PowerBeam] Power Beam Use Time")]
		[Range(5, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(14)]
		public int useTimePowerBeam;

		//[Label("[i:MetroidMod/PowerBeam] Power Beam Overheat")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(4)]
		public int overheatPowerBeam;

		//[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(30)]
		public int damageMissileLauncher;

		//[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Use Time")]
		[Range(5, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(18)]
		public int useTimeMissileLauncher;

		//[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Starting Ammo")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int ammoMissileLauncher;

		//[Label("[i:MetroidMod/MissileExpansion] Ammo per Missile Tank")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int ammoMissileTank;

		[Header("[i:MetroidMod/PowerBeam]SpecialBeams")]

		//[Label("[i:MetroidMod/HyperBeamAddon] Hyper Beam Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(35)]
		public int damageHyperBeam;

		//[Label("[i:MetroidMod/HyperBeamAddon] Hyper Beam Use Time")]
		[Range(1, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(16)]
		public int useTimeHyperBeam;

		//[Label("[i:MetroidMod/HyperBeamAddon] Hyper Beam Overheat")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(7)]
		public int overheatHyperBeam;

		//[Label("[i:MetroidMod/PhazonBeamAddon] Phazon Beam Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int damagePhazonBeam;

		//[Label("[i:MetroidMod/PhazonBeamAddon] Phazon Beam Use Time")]
		[Range(1, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int useTimePhazonBeam;

		//[Label("[i:MetroidMod/PhazonBeamAddon] Phazon Beam Overheat")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(1)]
		public int overheatPhazonBeam;

		[Header("[i:MetroidMod/PowerBeam]PowerBeamV1Addons")]

		//[Label("[i:MetroidMod/ChargeBeamAddon] Charge Beam Damage Multiplier")]
		[Range(1f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(3f)]
		public float damageChargeBeam;

		//[Label("[i:MetroidMod/ChargeBeamAddon] Charge Beam Overheat Multiplier")]
		[Range(1f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2f)]
		public float overheatChargeBeam;

		//[Label("[i:MetroidMod/IceBeamAddon] Ice Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.75f)]
		public float damageIceBeam;

		//[Label("[i:MetroidMod/IceBeamAddon] Ice Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.25f)]
		public float overheatIceBeam;

		//[Label("[i:MetroidMod/IceBeamAddon] Ice Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.3f)]
		public float speedIceBeam;

		//[Label("[i:MetroidMod/WaveBeamAddon] Wave Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float damageWaveBeam;

		//[Label("[i:MetroidMod/WaveBeamAddon] Wave Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float overheatWaveBeam;

		//[Label("[i:MetroidMod/WaveBeamAddon] Wave Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0f)]
		public float speedWaveBeam;

		//[Label("[i:MetroidMod/SpazerAddon] Spazer Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.25f)]
		public float damageSpazer;

		//[Label("[i:MetroidMod/SpazerAddon] Spazer Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float overheatSpazer;

		//[Label("[i:MetroidMod/SpazerAddon] Spazer Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.15f)]
		public float speedSpazer;

		//[Label("[i:MetroidMod/PlasmaBeamGreenAddon] Plasma Beam (Green) Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1f)]
		public float damagePlasmaBeamGreen;

		//[Label("[i:MetroidMod/PlasmaBeamGreenAddon] Plasma Beam (Green) Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.75f)]
		public float overheatPlasmaBeamGreen;

		//[Label("[i:MetroidMod/PlasmaBeamGreenAddon] Plasma Beam (Green) Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.15f)]
		public float speedPlasmaBeamGreen;

		//[Label("[i:MetroidMod/PlasmaBeamRedAddon] Plasma Beam (Red) Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1f)]
		public float damagePlasmaBeamRed;

		//[Label("[i:MetroidMod/PlasmaBeamRedAddon] Plasma Beam (Red) Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.75f)]
		public float overheatPlasmaBeamRed;

		//[Label("[i:MetroidMod/PlasmaBeamRedAddon] Plasma Beam (Red) Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.15f)]
		public float speedPlasmaBeamRed;

		[Header("[i:MetroidMod/PowerBeam]PowerBeamV2Addons")]

		//[Label("[i:MetroidMod/ChargeBeamV2Addon] Charge Beam V2 Damage Multiplier")]
		[Range(1f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(4f)]
		public float damageChargeBeamV2;

		//[Label("[i:MetroidMod/ChargeBeamV2Addon] Charge Beam V2 Overheat Multiplier")]
		[Range(1f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2.5f)]
		public float overheatChargeBeamV2;

		//[Label("[i:MetroidMod/WideBeamAddon] Wide Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1f)]
		public float damageWideBeam;

		//[Label("[i:MetroidMod/WideBeamAddon] Wide Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.75f)]
		public float overheatWideBeam;

		//[Label("[i:MetroidMod/WideBeamAddon] Wide Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.15f)]
		public float speedWideBeam;

		//[Label("[i:MetroidMod/IceBeamV2Addon] Ice Beam V2 Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2.25f)]
		public float damageIceBeamV2;

		//[Label("[i:MetroidMod/IceBeamV2Addon] Ice Beam V2 Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float overheatIceBeamV2;

		//[Label("[i:MetroidMod/IceBeamV2Addon] Ice Beam V2 Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.3f)]
		public float speedIceBeamV2;

		//[Label("[i:MetroidMod/WaveBeamV2Addon] Wave Beam V2 Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1.25f)]
		public float damageWaveBeamV2;

		//[Label("[i:MetroidMod/WaveBeamV2Addon] Wave Beam V2 Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.75f)]
		public float overheatWaveBeamV2;

		//[Label("[i:MetroidMod/WaveBeamV2Addon] Wave Beam V2 Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0f)]
		public float speedWaveBeamV2;

		//[Label("[i:MetroidMod/NovaBeamAddon] Nova Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2.25f)]
		public float damageNovaBeam;

		//[Label("[i:MetroidMod/NovaBeamAddon] Nova Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1f)]
		public float overheatNovaBeam;

		//[Label("[i:MetroidMod/NovaBeamAddon] Nova Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.15f)]
		public float speedNovaBeam;

		[Header("[i:MetroidMod/PowerBeam]PowerBeamV3Addons")]

		//[Label("[i:MetroidMod/LuminiteBeamAddon] Luminite Beam Damage Multiplier")]
		[Range(1f, 5f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(5f)]
		public float damageLuminiteBeam;

		//[Label("[i:MetroidMod/LuminiteBeamAddon] Luminite Beam Overheat Multiplier")]
		[Range(1f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(3f)]
		public float overheatLuminiteBeam;

		//[Label("[i:MetroidMod/NebulaBeamAddon] Nebula Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2.25f)]
		public float damageNebulaBeam;

		//[Label("[i:MetroidMod/NebulaBeamAddon] Nebula Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1f)]
		public float overheatNebulaBeam;

		//[Label("[i:MetroidMod/NebulaBeamAddon] Nebula Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0f)]
		public float speedNebulaBeam;

		//[Label("[i:MetroidMod/SolarBeamAddon] Solar Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(3f)]
		public float damageSolarBeam;

		//[Label("[i:MetroidMod/SolarBeamAddon] Solar Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1.5f)]
		public float overheatSolarBeam;

		//[Label("[i:MetroidMod/SolarBeamAddon] Solar Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.15f)]
		public float speedSolarBeam;

		//[Label("[i:MetroidMod/StardustBeamAddon] Stardust Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2.6f)]
		public float damageStardustBeam;

		//[Label("[i:MetroidMod/StardustBeamAddon] Stardust Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float overheatStardustBeam;

		//[Label("[i:MetroidMod/StardustBeamAddon] Stardust Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.3f)]
		public float speedStardustBeam;

		//[Label("[i:MetroidMod/VortexBeamAddon] Vortex Beam Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1.5f)]
		public float damageVortexBeam;

		//[Label("[i:MetroidMod/VortexBeamAddon] Vortex Beam Overheat Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(1f)]
		public float overheatVortexBeam;

		//[Label("[i:MetroidMod/VortexBeamAddon] Vortex Beam Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.25f)]
		public float speedVortexBeam;

		[Header("[i:MetroidMod/PowerBeam]PowerBeamHuntersAddons")]

		//[Label("[i:MetroidMod/VoltDriverAddon] VoltDriver Damage Modifier")]
		[Range(0f, 2f)]
		[Increment(0.1f)]
		[Slider]
		[DefaultValue(.1f)]
		public float damageVoltDriver;

		//[Label("[i:MetroidMod/VoltDriverAddon] VoltDriver Use Time")]
		[Range(1f, 60f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(17f)]
		public float useTimeVoltDriver;

		//[Label("[i:MetroidMod/VoltDriverAddon] VoltDriver Overheat Modifer")]
		[Range(0f, 2f)]
		[Increment(0.5f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float overheatVoltDriver;

		//[Label("[i:MetroidMod/MagMaulAddon] MagMaul Damage Modifier")]
		[Range(0f, 2f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(.45f)]
		public float damageMagMaul;

		//[Label("[i:MetroidMod/MagMaulAddon] MagMaul Use Time")]
		[Range(1f, 60f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(36f)]
		public float useTimeMagMaul;

		//[Label("[i:MetroidMod/MagMaulAddon] MagMaul Overheat Modifer")]
		[Range(0f, 2f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(.75f)]
		public float overheatMagMaul;

		//[Label("[i:MetroidMod/ImperialistAddon] Imperialist Damage Modifier")]
		[Range(0f, 10f)]
		[Increment(0.5f)]
		[Slider]
		[DefaultValue(5f)]
		public float damageImperialist;

		//[Label("[i:MetroidMod/ImperialistAddon] Imperialist Use Time")]
		[Range(1f, 100f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(92f)]
		public float useTimeImperialist;

		//[Label("[i:MetroidMod/ImperialistAddon] Imperialist Overheat Modifer")]
		[Range(0f, 5f)]
		[Increment(0.5f)]
		[Slider]
		[DefaultValue(2.5f)]
		public float overheatImperialist;

		//[Label("[i:MetroidMod/JudicatorAddon] Judicator Damage Modifier")]
		[Range(0f, 2f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(.45f)]
		public float damageJudicator;

		//[Label("[i:MetroidMod/JudicatorAddon] Judicator Use Time")]
		[Range(1f, 60f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(27f)]
		public float useTimeJudicator;

		//[Label("[i:MetroidMod/JudicatorAddon] Judicator Overheat Modifer")]
		[Range(0f, 2f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(.75f)]
		public float overheatJudicator;

		//[Label("[i:MetroidMod/ShockCoilAddon] ShockCoil Damage Modifier")]
		[Range(-0.9f, 2f)]
		[Increment(0.1f)]
		[Slider]
		[DefaultValue(0f)]
		public float damageShockCoil;

		//[Label("[i:MetroidMod/ShockCoilAddon] ShockCoil Damage Modifier")]
		[Range(.1f, 1f)]
		[Increment(0.1f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float minSpeedShockCoil;

		//[Label("[i:MetroidMod/ShockCoilAddon] ShockCoil Damage Modifier")]
		[Range(1f, 5f)]
		[Increment(0.5f)]
		[Slider]
		[DefaultValue(3.0f)]
		public float maxSpeedShockCoil;

		//[Label("[i:MetroidMod/ShockCoilAddon] ShockCoil Use Time")]
		[Range(1f, 60f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(14f)]
		public float useTimeShockCoil;

		//[Label("[i:MetroidMod/ShockCoilAddon] ShockCoil Heat Modifier")]
		[Range(-1f, 2f)]
		[Increment(0.5f)]
		[Slider]
		[DefaultValue(0f)]
		public float heatShockCoil;

		//[Label("[i:MetroidMod/BattleHammerAddon] BattleHammer Damage Modifier")]
		[Range(-0.5f, 2f)]
		[Increment(0.25f)]
		[Slider]
		[DefaultValue(0f)]
		public float damageBattleHammer;

		//[Label("[i:MetroidMod/BattleHammerAddon] BattleHammer Use Time")]
		[Range(1f, 60f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(27f)]
		public float useTimeBattleHammer;

		//[Label("[i:MetroidMod/BattleHammerAddon] BattleHammer Overheat Modifer")]
		[Range(0f, 3f)]
		[Increment(0.1f)]
		[Slider]
		[DefaultValue(.5f)]
		public float overheatBattleHammer;

		//[Label("[i:MetroidMod/OmegaCannonAddon] OmegaCannon Damage Modifier")]
		[Range(0f, 50f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(20f)]
		public float damageOmegaCannon;

		//[Label("[i:MetroidMod/OmegaCannonAddon] OmegaCannon Use Time")]
		[Range(1f, 125f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(109f)]
		public float useTimeOmegaCannon;

		//[Label("[i:MetroidMod/OmegaCannonAddon] OmegaCannon Overheat Modifer")]
		[Range(0f, 20f)]
		[Increment(1f)]
		[Slider]
		[DefaultValue(10f)]
		public float overheatOmegaCannon;

		[Header("[i:MetroidMod/MissileLauncher]MissileLauncherAddons")]

		//[Label("[i:MetroidMod/IceMissileAddon] Ice Missile Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.5f)]
		public float damageIceMissile;

		//[Label("[i:MetroidMod/IceMissileAddon] Ice Missile Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0f)]
		public float speedIceMissile;

		//[Label("[i:MetroidMod/SuperMissileAddon] Super Missile Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2f)]
		public float damageSuperMissile;

		//[Label("[i:MetroidMod/SuperMissileAddon] Super Missile Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.5f)]
		public float speedSuperMissile;

		//[Label("[i:MetroidMod/IceSuperMissileAddon] Ice Super Missile Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(2f)]
		public float damageIceSuperMissile;

		//[Label("[i:MetroidMod/IceSuperMissileAddon] Ice Super Missile Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.5f)]
		public float speedIceSuperMissile;

		//[Label("[i:MetroidMod/NebulaMissileAddon] Nebula Missile Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(4f)]
		public float damageNebulaMissile;

		//[Label("[i:MetroidMod/NebulaMissileAddon] Nebula Missile Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.5f)]
		public float speedNebulaMissile;

		//[Label("[i:MetroidMod/StardustMissileAddon] Stardust Missile Damage Multiplier")]
		[Range(-0.5f, 4f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(4f)]
		public float damageStardustMissile;

		//[Label("[i:MetroidMod/StardustMissileAddon] Stardust Missile Speed Multiplier")]
		[Range(-0.5f, 0.3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(-0.5f)]
		public float speedStardustMissile;
	}
}
