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
		
		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				return whoAmI == 0;
			}
			return true;
		}
		
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
	}
}