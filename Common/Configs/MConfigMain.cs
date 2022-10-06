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
	// NOTE ABOUT SUBPAGES!! [DefaultValue()] does NOT work on values inside of subpages. Use variable = value instead.
	[Label("Main Config")]
	public class MConfigMain : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		internal CanEditServerConfig condition;

		internal delegate bool CanEditServerConfig(ModConfig pendingConfig, int whoAmI, ref string message);

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) => condition(pendingConfig, whoAmI, ref message);

		public static MConfigMain Instance;

		public MConfigMain()
		{
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

		// TODO: get this working
		// keep this internal, we do not want this to show up, but we want players to be able to configure a value for this
		//[DefaultValue(false)]
		//internal bool veryBrokenHatchControl;
		
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
	}
}