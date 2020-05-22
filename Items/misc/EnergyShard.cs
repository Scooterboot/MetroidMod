using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
	public class EnergyShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Shard");
			Tooltip.SetDefault("'It could be used to power something if it were whole'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 18;
			item.height = 16;
			item.value = 100;
			item.rare = 2;
		}
	}
}