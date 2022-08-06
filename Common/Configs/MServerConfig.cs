using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace MetroidMod.Common.Configs
{
	[Label("Server Side")]
	public class MServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		internal CanEditServerConfig condition;

		internal delegate bool CanEditServerConfig(ModConfig pendingConfig, int whoAmI, ref string message);

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) => condition(pendingConfig, whoAmI, ref message);

		public static MServerConfig Instance;

		public MServerConfig()
		{
			Instance = this;
			condition = delegate (ModConfig pendingConfig, int whoAmI, ref string message)
			{
				return whoAmI == 0;
			};
		}
		
	[Header("General")]
		
		[Label("[i:MetroidMod/TorizoSummon] Boss Summon Consumption")]
		[Tooltip("When enabled, Boss Summon items will be consumed upon usage.")]
		[DefaultValue(true)]
		public bool enableBossSummonConsumption;
		
		//The following isn't done yet.
		/*[Label("[i:MetroidMod/TorizoBag] Bosses drop addons")]
		[Tooltip("When enabled, certain Bosses will drop Suit and Beam addons upon death.")]
		[DefaultValue(false)]
		public bool enableBossAddonDrops;*/
		
	[Header("[i:MetroidMod/BlueHatch] Automatically Closing Hatches")]
		
		[Label("Enabled")]
		[Tooltip("When enabled, hatches will automatically close after a certain period of time.")]
		[DefaultValue(true)]
		public bool AutocloseHatchesEnabled;
		
		[Label("Timer")]
		[Tooltip("Time before hatches automatically close, in seconds.")]
		[Range(0, 120)]
		[Increment(5)]
		[Slider]
		[DefaultValue(10)]
		public int AutocloseHatchesTime;
		
	[Header("[i:MetroidMod/ChoziteBar] Chozite Gear\n(REQUIRES WORLD RELOAD)")]
		
		[Label("[i:MetroidMod/HiJumpBootsAddon] Wall Jump")]
		[Tooltip("When enabled, a full set of Chozite Armor grants the ability to Wall Jump.")]
		[DefaultValue(true)]
		public bool enableWallJumpChoziteArmor;
		
		[Label("[i:MetroidMod/ChoziteHelmet] Chozite Helmet Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defenseChoziteHelmet;
		
		[Label("[i:MetroidMod/ChoziteBreastplate] Chozite Breastplate Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int defenseChoziteBreastplate;
		
		[Label("[i:MetroidMod/ChoziteGreaves] Chozite Greaves Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(4)]
		public int defenseChoziteGreaves;
		
		[Label("[i:MetroidMod/ChoziteSword] Chozite Sword Damage")]
		[Range(1, 30)]
		[Increment(1)]
		[Slider]
		[DefaultValue(16)]
		public int damageChoziteSword;
		
		[Label("[i:MetroidMod/ChoziteShortsword] Chozite Shortsword Damage")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(14)]
		public int damageChoziteShortsword;
		
		[Label("[i:MetroidMod/ChoziteCrossbow] Chozite Crossbow Damage")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(12)]
		public int damageChoziteCrossbow;
		
	[Header("[i:MetroidMod/VariaSuitV2AddonAddon] Power Suit\n(REQUIRES WORLD RELOAD)")]
		
		[Label("[i:MetroidMod/HiJumpBootsAddon] Wall Jump")]
		[DefaultValue(true)]
		public bool enableWallJumpPowerSuitGreaves;
		
		[Label("[i:LuckyHorseshoe] Negate Fall Damage")]
		[DefaultValue(true)]
		public bool enableNoFallDamagePowerSuitGreaves;
		
		[Label("[i:MetroidMod/PowerSuitHelmet] Power Suit Helmet Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defensePowerSuitHelmet;
		
		[Label("[i:MetroidMod/PowerSuitBreastplate] Power Suit Breastplate Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int defensePowerSuitBreastplate;
		
		[Label("[i:MetroidMod/PowerSuitGreaves] Power Suit Greaves Defense")]
		[Range(1, 20)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defensePowerSuitGreaves;
		
		[Label("[i:MetroidMod/EnergyTankAddon] Energy Defense Efficiency")]
		[Range(0.05f, 1f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.1f)]
		public float energyDefenseEfficiency;
		
		[Label("[i:MetroidMod/ReserveTankAddon] Energy Expense Efficiency")]
		[Range(0.05f, 1f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.1f)]
		public float energyExpenseEfficiency;
		
	[Header("[i:MetroidMod/PowerGripAddon] Suit Addons\n(REQUIRES WORLD RELOAD)")]
		
		[Label("[i:MetroidMod/EnergyTankAddon] Energy Tank Stack")]
		[Range(1, 14)]
		[Increment(1)]
		[Slider]
		[DefaultValue(10)]
		public int stackEnergyTank;
		
		[Label("[i:MetroidMod/PowerGripAddon][i:MetroidMod/HiJumpBootsAddon] Power Grip grants Wall Jump")]
		[DefaultValue(false)]
		public bool enableWallJumpPowerGrip;
		
	[Header("[i:MetroidMod/PowerBeam] Standard Weapons\n(REQUIRES WORLD RELOAD)")]
		
		[Label("[i:MetroidMod/PowerBeam] Power Beam Damage")]
		[Range(1, 50)]
		[Increment(1)]
		[Slider]
		[DefaultValue(14)]
		public int damagePowerBeam;
		
		[Label("[i:MetroidMod/PowerBeam] Power Beam Use Time")]
		[Range(1, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(14)]
		public int useTimePowerBeam;
		
		[Label("[i:MetroidMod/PowerBeam] Power Beam Overheat")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(4)]
		public int overheatPowerBeam;
		
		[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(30)]
		public int damageMissileLauncher;
		
		[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Use Time")]
		[Range(1, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(9)]
		public int useTimeMissileLauncher;
		
		[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Starting Ammo")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int ammoMissileLauncher;
		
		[Label("[i:MetroidMod/MissileExpansion] Ammo per Missile Tank")]
		[Range(1, 10)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int ammoMissileTank;
		
	[Header("[i:MetroidMod/PowerBeam] Special Beams\n(REQUIRES WORLD RELOAD)")]
		
		[Label("[i:MetroidMod/HyperBeamAddon] Hyper Beam Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(35)]
		public int damageHyperBeam;
		
		[Label("[i:MetroidMod/HyperBeamAddon] Hyper Beam Use Time")]
		[Range(1, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(16)]
		public int useTimeHyperBeam;
		
		[Label("[i:MetroidMod/HyperBeamAddon] Hyper Beam Overheat")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(7)]
		public int overheatHyperBeam;
		
		[Label("[i:MetroidMod/PhazonBeamAddon] Phazon Beam Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int damagePhazonBeam;
		
		[Label("[i:MetroidMod/PhazonBeamAddon] Phazon Beam Use Time")]
		[Range(1, 60)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int useTimePhazonBeam;
		
		[Label("[i:MetroidMod/PhazonBeamAddon] Hyper Beam Overheat")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(1)]
		public int overheatPhazonBeam;
	}
}
