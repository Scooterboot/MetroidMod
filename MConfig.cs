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

namespace MetroidMod
{
	[Label("Client Side")]
	public class MConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		
		[Label("Toggle alternate weapon textures")]
		[Tooltip("When enabled, shows Metroid Prime style weapons, as opposed to the default Super Metroid style.\n" +
		"Default value: false")]
		public bool UseAltWeaponTextures;
		
		[Label("Draggable power beam ui")]
		[Tooltip("Allows the power beam UI to be draggable.\n" +
		"Default value: false")]
		public bool DragablePowerBeamUI;
		
		[Label("Draggable missile launcher ui")]
		[Tooltip("Allows the missile launcher UI to be draggable.\n" +
		"Default value: false")]
		public bool DragableMissileLauncherUI;
		
		[Label("Draggable morph ball ui")]
		[Tooltip("Allows the morph ball UI to be draggable.\n" +
		"Default value: false")]
		public bool DragableMorphBallUI;
		
		[Label("Draggable sense move ui")]
		[Tooltip("Allows the sense move UI to be draggable.\n" +
		"Default value: false")]
		public bool DragableSenseMoveUI;
		
		public override void OnChanged()
		{
			MetroidMod.UseAltWeaponTextures = UseAltWeaponTextures;
		}
	}
}