using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod {
public class MetroidMod : Mod
{
		internal static ModHotKey MorphBallKey;
		internal static ModHotKey SpiderBallKey;
		internal static ModHotKey BoostBallKey;
		internal static ModHotKey PowerBombKey;
		internal static ModHotKey BeamInterfaceKey;
		internal static ModHotKey SenseMoveKey;
	public static Mod Instance;
    public MetroidMod()
    {

       Properties = new ModProperties()
		{
			Autoload = true,
			AutoloadSounds = true,
			AutoloadGores = true

		};
		
    }
		public override void Load()
		{
			Instance = this;
			MorphBallKey = RegisterHotKey("Morph Ball", "Z");
			SpiderBallKey = RegisterHotKey("Spider Ball", "X");
			BoostBallKey = RegisterHotKey("Boost Ball", "F");
			PowerBombKey = RegisterHotKey("Power Bomb", "R");
			BeamInterfaceKey = RegisterHotKey("Open Combination Interface", "Q");
			SenseMoveKey = RegisterHotKey("Use Sense Move", "F");
		}
}
}
