using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod {
public class MetroidMod : Mod
{
		public static Color powColor = new Color(248, 248, 110);
		public static Color iceColor = new Color(0, 255, 255);
		public static Color waveColor = new Color(215, 0, 215);
		public static Color waveColor2 = new Color(239, 153, 239);
		public static Color plaRedColor = new Color(253, 221, 3);
		public static Color plaGreenColor = new Color(0, 248, 112);
		public static Color plaGreenColor2 = new Color(61, 248, 154);
		public static Color novColor = new Color(50, 255, 1);
		public static Color wideColor = new Color(255, 210, 255);


		internal static ModHotKey MorphBallKey;
		internal static ModHotKey SpiderBallKey;
		internal static ModHotKey BoostBallKey;
		internal static ModHotKey PowerBombKey;
		internal static ModHotKey SenseMoveKey;
		public const string SerrisHead = "MetroidMod/NPCs/Serris/Serris_Head_Head_Boss_";
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
			SenseMoveKey = RegisterHotKey("Use Sense Move", "F");
			if (!Main.dedServ)
			{
		AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Serris"), ItemType("SerrisMusicBox"), TileType("SerrisMusicBox"));
			}
			for (int k = 1; k <= 7; k++)
			{
				AddBossHeadTexture(SerrisHead + k);
			}
		}
}
}
