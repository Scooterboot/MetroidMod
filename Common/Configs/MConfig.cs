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

namespace MetroidModPorted.Common.Configs
{
	[Label("Client Side")]
	public class MConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
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
			MetroidModPorted.UseAltWeaponTextures = UseAltWeaponTextures;
			MetroidModPorted.DragablePowerBeamUI = DragablePowerBeamUI;
			MetroidModPorted.DragableMissileLauncherUI = DragableMissileLauncherUI;
			MetroidModPorted.DragableMorphBallUI = DragableMorphBallUI;
			MetroidModPorted.DragableSenseMoveUI = DragableSenseMoveUI;
		}
	}

	[Label("Client Side Debug")]
	public class MDebugConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Draw NPC hitboxes")]
		[Tooltip("When enabled, draws NPC hitboxes.\n" +
		"Default value: false")]
		public bool DrawNPCHitboxes;

		public override void OnChanged()
		{
			MetroidModPorted.DebugDH = DrawNPCHitboxes;
		}
	}
}
