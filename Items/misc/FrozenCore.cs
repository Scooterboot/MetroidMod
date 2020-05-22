using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.misc
{
	public class FrozenCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supercooled Plasma Core");
			Tooltip.SetDefault("'Strange energy core capable of producing supercooled plasma'");
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