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
	[LegacyName("MConfig")]
	[Label("Client Side")]
	public class MConfigClient : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		public static MConfigClient Instance;

		public MConfigClient()
		{
		}
		
		[Header("General")]
		
		[Label("Toggle alternate weapon textures")]
		[Tooltip("When enabled, shows Metroid Prime style weapons, as opposed to the default Super Metroid style.\n" +
		"Default value: false")]
		public bool UseAltWeaponTextures;

		[Label("[i:MetroidMod/EnergyTankAddon] Energy Hit Sound")]
		[Tooltip("When enabled, a custom sound will play when Suit Energy receives damage.\n" +
		"Default value: true")]
		[DefaultValue(true)]
		public bool energyHit;

		[Label("[i:MetroidMod/EnergyTankAddon] Low Energy Alert")]
		[Tooltip("When enabled, a beep will be heard when Suit Energy is low.\n" +
		"Default value: true")]
		[DefaultValue(true)]
		public bool energyLow;

        [Label("[i:MetroidMod/EnergyTankAddon] Low Energy Alert Interval")]
        [Tooltip("The interval between Low Energy beeps.\n[Default: 20]")]
        [Slider]
        [DefaultValue(20)]
        [Range(5, 200)]
        [Increment(5)]
        public int energyLowInterval;

		[Label("[i:MetroidMod/EnergyTankAddon] Low Energy Alert Fade")]
		[Tooltip("When enabled, a fading, non-looping beep will be heard when Suit Energy is low.\n" +
		"Default value: false\n" + "(WORK IN PROGRESS)")]
		[DefaultValue(false)]
		public bool energyLowFade;

		[Header("Draggable Power Beam UI")]
		
		[Label("Enabled")]
		[Tooltip("Allows the Power Beam UI to be draggable.\n" +
		"Default value: false")]
		public bool DragablePowerBeamUI;
		
		[Header("Draggable Missile Launcher UI")]
		
		[Label("Enabled")]
		[Tooltip("Allows the Missile Launcher UI to be draggable.\n" +
		"Default value: false")]
		public bool DragableMissileLauncherUI;
		
		[Header("Draggable Morph Ball UI")]
		
		[Label("Enabled")]
		[Tooltip("Allows the Morph Ball UI to be draggable.\n" +
		"Default value: false")]
		public bool DragableMorphBallUI;
		
		[Header("Draggable Sense Move UI")]
		
		[Label("Enabled")]
		[Tooltip("Allows the Sense Move UI to be draggable.\n" +
		"Default value: false")]
		public bool DragableSenseMoveUI;

		[Header("Map Icons")]
		
		[Label("[i:MetroidMod/GoldenTorizoSummon] Show Torizo Room Location on Map")]
		[Tooltip("When enabled, the map will show an icon where Torizo's boss room is.\n" +
		"Default value: true")]
		[DefaultValue(true)]
		public bool showTorizoRoomIcon;

		public override void OnChanged()
		{
			MetroidMod.UseAltWeaponTextures = UseAltWeaponTextures;
			MetroidMod.DragablePowerBeamUI = DragablePowerBeamUI;
			MetroidMod.DragableMissileLauncherUI = DragableMissileLauncherUI;
			MetroidMod.DragableMorphBallUI = DragableMorphBallUI;
			MetroidMod.DragableSenseMoveUI = DragableSenseMoveUI;
		}
	}

	[Label("Client Side Debug")]
	public class MConfigClientDebug : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		public static MConfigClientDebug Instance;

		public MConfigClientDebug()
		{
			Instance = this;
		}
		
		[Label("Draw NPC hitboxes")]
		[Tooltip("When enabled, draws NPC hitboxes.\n" +
		"Default value: false")]
		public bool DrawNPCHitboxes;

		[Label("Markers for statue items")]
		[Tooltip("When enabled, draws markers for statue items\n" +
		"Note: Performance will tank on world load\n" +
		"Default value: false")]
		public bool StatueItemMarkers;

		public override void OnChanged()
		{
			MetroidMod.DebugDH = DrawNPCHitboxes;
			MetroidMod.DebugDSI = StatueItemMarkers;
		}
	}
}
