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
		
		public static MServerConfig Instance;
		
		[Header("General")]
		
		[Label("Boss Summon Consumption")]
		[Tooltip("When enabled, Boss Summon items will be consumed upon usage.")]
		[DefaultValue(true)]
		public bool enableBossSummonConsumption;
		
		[Header("Automatically Closing Hatches")]
		
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
		
		public override void OnChanged()
		{
			MetroidMod.AutocloseHatchesEnabled = AutocloseHatchesEnabled;
			MetroidMod.AutocloseHatchesTime = AutocloseHatchesTime;
		}
		
		[Header("Stat Modifiers\n(REQUIRES WORLD RELOAD)")]
		
		[Label("[i:MetroidMod/PowerBeam] Power Beam Damage")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(14)]
		public int damagePowerBeam;
		
		[Label("[i:MetroidMod/PowerBeam][i:MetroidMod/ChargeBeamAddon] Power Beam Charge Damage")]
		[Range(0.05f, 3f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(3f)]
		public float damageChargePowerBeam;
		
		[Label("[i:MetroidMod/PowerBeam] Power Beam Use Time")]
		[Range(1, 100)]
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
		
		[Label("[i:MetroidMod/MissileLauncher] Missile Launcher Starting Ammo")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int ammoMissileLauncher;
		
		[Label("[i:MetroidMod/MissileExpansion] Ammo per Missile Tank")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int ammoMissileTank;
		
		[Label("[i:MetroidMod/PowerSuitHelmet] Power Suit Helmet Defense")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defensePowerSuitHelmet;
		
		[Label("[i:MetroidMod/PowerSuitBreastplate] Power Suit Breastplate Defense")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(6)]
		public int defensePowerSuitBreastplate;
		
		[Label("[i:MetroidMod/PowerSuitGreaves] Power Suit Greaves Defense")]
		[Range(1, 100)]
		[Increment(1)]
		[Slider]
		[DefaultValue(5)]
		public int defensePowerSuitGreaves;
		
		[Label("[i:MetroidMod/EnergyTank] Energy Defense Efficiency")]
		[Range(0.05f, 1f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.1f)]
		public float energyDefenseEfficiency;
		
		[Label("[i:MetroidMod/ReserveTank] Energy Expense Efficiency")]
		[Range(0.05f, 1f)]
		[Increment(0.05f)]
		[Slider]
		[DefaultValue(0.1f)]
		public float energyExpenseEfficiency;
	}
}