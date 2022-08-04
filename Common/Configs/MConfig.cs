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
	[Label("Client Side")]
	public class MConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		public static MConfig Instance;

		public MConfig()
		{
			Instance = this;
		}
		
		[Label("Toggle alternate weapon textures")]
		[Tooltip("When enabled, shows Metroid Prime style weapons, as opposed to the default Super Metroid style.\n" +
		"Default value: false")]
		public bool UseAltWeaponTextures;
		
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
	public class MDebugConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		public static MDebugConfig Instance;

		public MDebugConfig()
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
